// See https://aka.ms/new-console-template for more information

using ScottPlot;
using TrafficVSL_OMI;

const int scaleFactorTraffic = 1000;
const int maximumCars = 100;
const int iterationsWithSameDensity = 100;

List<double> averageTrafficFlowPer100 = [];
List<double> averageTrafficCount = [];
List<double> averageTrafficJamIntensity = [];

List<List<CsvItem>> allCsvItems = [];

var trafficFlowPer100 = new double[iterationsWithSameDensity];
var trafficIntensityPer100 = new double[iterationsWithSameDensity];

// Console.WriteLine("Starting Iterations");
//
// Road road = new(100, 0, 5, 0.35f, 5);
// road.Reset(30);
// for (int i = 0; i < 100; i++)
// {
//     road.IterateRounds(1);
//     RoadPrinter.PrintDebugLine(road);
// }
// Console.WriteLine("Starting Iterations");
// Console.WriteLine();
// Console.WriteLine();
//
// road = new(100, 0, 5, 0.35f, 4);
// road.Reset(30);
// for (int i = 0; i < 100; i++)
// {
//     road.IterateRounds(1);
//     RoadPrinter.PrintDebugLine(road);
// }
// Console.WriteLine("Starting Iterations");
// Console.WriteLine();
// Console.WriteLine();
//
// road = new(100, 0, 5, 0.35f, 3);
// road.Reset(30);
// for (int i = 0; i < 100; i++)
// {
//     road.IterateRounds(1);
//     RoadPrinter.PrintDebugLine(road);
// }
// Console.WriteLine("Starting Iterations");
// Console.WriteLine();
// Console.WriteLine();
//
// road = new(100, 0, 5, 0.35f, 2);
// road.Reset(30);
// for (int i = 0; i < 100; i++)
// {
//     road.IterateRounds(1);
//     RoadPrinter.PrintDebugLine(road);
// }
//
// return;

ModelConfiguration modelConfiguration = new(BackBufferSize: 5,
    CarDistance: 1,
    NumberOfSites: 100,
    NumberOfCars: 0,
    MaximumSpeed: 5,
    ProbabilityToSlowDown: 0.35f,
    DynamicSpeedLimit: 5);

Road road = new(modelConfiguration);
RunModel();
PlotGraph(averageTrafficCount, averageTrafficFlowPer100, "scatterplot-5");
SaveResultsToCsvItems();

road = new Road(modelConfiguration with { DynamicSpeedLimit = 4});
RunModel();
PlotGraph(averageTrafficCount, averageTrafficFlowPer100, "scatterplot-4");
SaveResultsToCsvItems();

road = new Road(modelConfiguration with { DynamicSpeedLimit = 3});
RunModel();
PlotGraph(averageTrafficCount, averageTrafficFlowPer100, "scatterplot-3");
SaveResultsToCsvItems();

road = new Road(modelConfiguration with { DynamicSpeedLimit = 2});
RunModel();
PlotGraph(averageTrafficCount, averageTrafficFlowPer100, "scatterplot-2");
SaveResultsToCsvItems();

road = new Road(modelConfiguration with { DynamicSpeedLimit = 1});
RunModel();
PlotGraph(averageTrafficCount, averageTrafficFlowPer100, "scatterplot-1");
SaveResultsToCsvItems();

var finalItems = GetCsvItems();
CsvExporter.Export("test", finalItems);

return;

void RunModel()
{
    averageTrafficFlowPer100.Clear();
    averageTrafficCount.Clear();
    averageTrafficJamIntensity.Clear();

    for (int numberOfCars = 2; numberOfCars < maximumCars; numberOfCars++)
    {
        road.Reset(numberOfCars);
        
        for (int numberOfIterations = 0; numberOfIterations < iterationsWithSameDensity; numberOfIterations++)
        {
            road.IterateRounds(100);

            var scaledAverageTrafficCount = road.TotalSpeed * scaleFactorTraffic;
            var scaledAverageTrafficJamIntensity = road.TrafficIntensity * scaleFactorTraffic;

            trafficFlowPer100[numberOfIterations] = scaledAverageTrafficCount;
            trafficIntensityPer100[numberOfIterations] = scaledAverageTrafficJamIntensity;
        }

        var averageFlow = trafficFlowPer100.Average();
        var averageJamIntensity = trafficIntensityPer100.Average();
        averageTrafficFlowPer100.Add(averageFlow);
        averageTrafficCount.Add(numberOfCars);
        averageTrafficJamIntensity.Add(averageJamIntensity);
    
        Console.WriteLine("Iteration" + numberOfCars);
        Console.CursorTop--;
    }
    Console.WriteLine("Finished iterations");
}

void PlotGraph(List<double> xs, List<double> ys, string fileName)
{
    // plot the data
    Plot myPlot = new();
    var scatter = myPlot.Add.Scatter(xs.ToArray(), ys.ToArray());
    scatter.Smooth = true;
    scatter.LinePattern = LinePattern.Solid;
    scatter.MarkerSize = 0;
    scatter.LineWidth = 2;
    //scatter.LineStyle = LineStyle.None;
    scatter.Color = Color.Gray(200);

    // display the plot
    myPlot.Axes.SetLimitsY(0, 500000);
    myPlot.Axes.SetLimitsX(0, maximumCars);
    myPlot.SavePng($"{fileName}.png", 500, 500);
}

void SaveResultsToCsvItems()
{
    var newItems = averageTrafficCount.Zip(averageTrafficFlowPer100, averageTrafficJamIntensity)
        .Select(x => new CsvItem(x.First, x.Second, x.Third)).ToList();
    allCsvItems.Add(newItems);
}

IEnumerable<FinalCsvItem> GetCsvItems()
{
    for (int i = 0; i < allCsvItems[0].Count; i++)
    {

        yield return new FinalCsvItem(allCsvItems[0][i].TrafficCount,
            allCsvItems[0][i].TrafficFlow,
            allCsvItems[1][i].TrafficFlow,
            allCsvItems[2][i].TrafficFlow,
            allCsvItems[3][i].TrafficFlow,
            allCsvItems[4][i].TrafficFlow,
            allCsvItems[0][i].TrafficJamIntensity,
            allCsvItems[1][i].TrafficJamIntensity,
            allCsvItems[2][i].TrafficJamIntensity,
            allCsvItems[3][i].TrafficJamIntensity,
            allCsvItems[4][i].TrafficJamIntensity);
    }
}

file record CsvItem(double TrafficCount, double TrafficFlow, double TrafficJamIntensity);

file record FinalCsvItem(double TrafficCount, 
    double TrafficFlow1, 
    double TrafficFlow2, 
    double TrafficFlow3, 
    double TrafficFlow4, 
    double TrafficFlow5,
    double TrafficJamIntensity1, 
    double TrafficJamIntensity2, 
    double TrafficJamIntensity3, 
    double TrafficJamIntensity4, 
    double TrafficJamIntensity5);


