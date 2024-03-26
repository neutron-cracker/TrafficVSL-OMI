// See https://aka.ms/new-console-template for more information

using TrafficVSL_OMI;

// CsvModelRunner runner = new();
// runner.Run();

Console.WriteLine("Starting Iterations");

var modelConfiguration = ModelConfiguration.Default with
{
    DynamicSpeedLimit = 1,
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

exporter.Export("traffic-flow-limited.png");

Console.WriteLine("Finished");

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


