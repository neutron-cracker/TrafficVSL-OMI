// See https://aka.ms/new-console-template for more information

using TrafficVSL_OMI;

Road road = new(100, 20, 5, 0.35f);

Console.WriteLine("Starting Iterations");
for (int i = 0; i < 100; i++)
{
    road.IterateRounds(1);
    RoadPrinter.PrintDebugLine(road);
}

Console.WriteLine("Finished iterations");