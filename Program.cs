// See https://aka.ms/new-console-template for more information

using ScottPlot;
using TrafficVSL_OMI;

const int scaleFactorTraffic = 1000;
const int maximumCars = 1000;
const int iterationsWithSameDensity = 10;

List<double> averageTrafficFlowPer100 = [];
List<double> averageTrafficCount = [];

List<List<CsvItem>> allCsvItems = [];

var trafficFlowPer100 = new double[iterationsWithSameDensity];

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

Road road = new(1000, 0, 5, 0.35f, 5);
RunModel();
PlotGraph(averageTrafficCount, averageTrafficFlowPer100, "scatterplot-5");
SaveResultsToCsvItems(averageTrafficCount, averageTrafficFlowPer100);

road = new(1000, 0, 5, 0.35f, 4);
RunModel();
PlotGraph(averageTrafficCount, averageTrafficFlowPer100, "scatterplot-4");
SaveResultsToCsvItems(averageTrafficCount, averageTrafficFlowPer100);

road = new(1000, 0, 5, 0.35f, 3);
RunModel();
PlotGraph(averageTrafficCount, averageTrafficFlowPer100, "scatterplot-3");
SaveResultsToCsvItems(averageTrafficCount, averageTrafficFlowPer100);

road = new(1000, 0, 5, 0.35f, 2);
RunModel();
PlotGraph(averageTrafficCount, averageTrafficFlowPer100, "scatterplot-2");
SaveResultsToCsvItems(averageTrafficCount, averageTrafficFlowPer100);

road = new(1000, 0, 5, 0.35f, 1);
RunModel();
PlotGraph(averageTrafficCount, averageTrafficFlowPer100, "scatterplot-1");
SaveResultsToCsvItems(averageTrafficCount, averageTrafficFlowPer100);

var finalItems = GetCsvItems();
CsvExporter.Export("test", finalItems);

return;

void RunModel()
{
    averageTrafficFlowPer100.Clear();
    averageTrafficCount.Clear();

    for (int numberOfCars = 2; numberOfCars < maximumCars; numberOfCars++)
    {
        road.Reset(numberOfCars);
        
        for (int numberOfIterations = 0; numberOfIterations < iterationsWithSameDensity; numberOfIterations++)
        {
            road.IterateRounds(100);

            var scaledAverageTrafficCount = road.TotalSpeed * scaleFactorTraffic;
        
            trafficFlowPer100[numberOfIterations] = (scaledAverageTrafficCount);
        }

        var averageFlow = trafficFlowPer100.Average();
        averageTrafficFlowPer100.Add(averageFlow);
        averageTrafficCount.Add(numberOfCars);
    
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

void SaveResultsToCsvItems(IEnumerable<double> xs, IEnumerable<double> ys)
{
    var newItems = xs.Zip(ys).Select(tuple => new CsvItem(tuple.First, tuple.Second)).ToList();
    allCsvItems.Add(newItems);
}

IEnumerable<FinalCsvItem> GetCsvItems()
{
    for (int i = 0; i < allCsvItems[0].Count; i++)
    {

        yield return new FinalCsvItem(allCsvItems[0][i].X,
            allCsvItems[0][i].Y,
            allCsvItems[1][i].Y,
            allCsvItems[2][i].Y,
            allCsvItems[3][i].Y,
            allCsvItems[4][i].Y);
    }
}

file record CsvItem(double X, double Y);

file record FinalCsvItem(double X, double Y1, double Y2, double Y3, double Y4, double Y5);


