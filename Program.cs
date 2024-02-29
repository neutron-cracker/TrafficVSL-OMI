// See https://aka.ms/new-console-template for more information

using ScottPlot;
using TrafficVSL_OMI;

const int scaleFactorTraffic = 1000;
const int maximumCars = 500;
const int iterationsWithSameDensity = 10;

List<double> averageTrafficFlowPer100 = [];
List<double> averageTrafficCount = [];

var trafficFlowPer100 = new double[iterationsWithSameDensity];
var trafficCount = new double[iterationsWithSameDensity];

Console.WriteLine("Starting Iterations");

// Road road = new(100, 0, 5, 0.35f, 5);
// road.Reset(30);
// for (int i = 0; i < 100; i++)
// {
//     road.IterateRounds(1);
//     RoadPrinter.PrintDebugLine(road);
// }
// Console.WriteLine("Starting Iterations");
//
// road = new(100, 0, 5, 0.35f, 4);
// road.Reset(30);
// for (int i = 0; i < 100; i++)
// {
//     road.IterateRounds(1);
//     RoadPrinter.PrintDebugLine(road);
// }
// Console.WriteLine("Starting Iterations");
//
// road = new(100, 0, 5, 0.35f, 3);
// road.Reset(30);
// for (int i = 0; i < 100; i++)
// {
//     road.IterateRounds(1);
//     RoadPrinter.PrintDebugLine(road);
// }
// Console.WriteLine("Starting Iterations");
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
PlotGraph(averageTrafficCount, averageTrafficFlowPer100, "scatterplot-5.png");

road = new(1000, 0, 5, 0.35f, 4);
RunModel();
PlotGraph(averageTrafficCount, averageTrafficFlowPer100, "scatterplot-4.png");

road = new(1000, 0, 5, 0.35f, 3);
RunModel();
PlotGraph(averageTrafficCount, averageTrafficFlowPer100, "scatterplot-3.png");

road = new(1000, 0, 5, 0.35f, 2);
RunModel();
PlotGraph(averageTrafficCount, averageTrafficFlowPer100, "scatterplot-2.png");

road = new(1000, 0, 5, 0.35f, 1);
RunModel();
PlotGraph(averageTrafficCount, averageTrafficFlowPer100, "scatterplot-1.png");

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
    //scatter.LineStyle = LineStyle.None;
    scatter.Color = Color.Gray(200);

    // display the plot
    myPlot.Axes.SetLimitsY(0, 500000);
    myPlot.Axes.SetLimitsX(0, maximumCars);
    myPlot.SavePng(fileName, 500, 500);
}


