// See https://aka.ms/new-console-template for more information

using TrafficVSL_OMI;

// CsvModelRunner runner = new();
// runner.Run();

Console.WriteLine("Starting Iterations");

var modelConfiguration = ModelConfiguration.Default with
{
    DynamicSpeedLimit = 5,
    BackBufferSize = 10,
    CarDistance = 1,
};
Road road = new(modelConfiguration);
ImageExporter exporter = new(modelConfiguration);

road.Reset(modelConfiguration.NumberOfCars);

for (int i = 0; i < 100; i++)
{
    road.IterateRounds(1);
    exporter.PrintLine(road);
}

exporter.Export("traffic-flow-not-limited");

Console.WriteLine("Finished");

// ModelConfiguration standardModelConfiguration = new(BackBufferSize: 5,
//     CarDistance: 1,
//     NumberOfSites: 100,
//     NumberOfCars: 0,
//     MaximumSpeed: 5,
//     ProbabilityToSlowDown: 0.35f,
//     DynamicSpeedLimit: 5);
//
//
// Console.WriteLine("Starting Iterations");
//
// Road road = new(standardModelConfiguration with { DynamicSpeedLimit = 5, BackBufferSize = 10});
// road.IterateRounds(1000000);
//
//
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
// road = new(standardModelConfiguration with { DynamicSpeedLimit = 5, BackBufferSize = 5 });
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
// road = new(standardModelConfiguration with { DynamicSpeedLimit = 5, BackBufferSize = 3 });
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
// return;


