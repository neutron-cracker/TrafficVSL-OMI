namespace TrafficVSL_OMI;

public class CsvModelRunner
{
    private const int MaximumCars = 100;
    private const int IterationsWithSameDensity = 100;
    private readonly ModelConfiguration _standardModelConfiguration = new(BackBufferSize: 5,
        CarDistance: 1,
        NumberOfSites: 100,
        NumberOfCars: 0,
        MaximumSpeed: 5,
        ProbabilityToSlowDown: 0.35f,
        DynamicSpeedLimit: 5);

    private readonly List<double> _trafficFlow = [];
    private readonly List<int> _trafficCount = [];
    private readonly List<double> _trafficJamIntensity = [];

    private readonly List<List<CsvItem>> _allCsvItems = [];

    public void Run()
    {
        Road road = new(_standardModelConfiguration);
        RunModel(road);
        SaveResultsToCsvItems();

        road = new Road(_standardModelConfiguration with { DynamicSpeedLimit = 4});
        RunModel(road);
        SaveResultsToCsvItems();

        road = new Road(_standardModelConfiguration with { DynamicSpeedLimit = 3});
        RunModel(road);
        SaveResultsToCsvItems();

        road = new Road(_standardModelConfiguration with { DynamicSpeedLimit = 2});
        RunModel(road);
        SaveResultsToCsvItems();

        road = new Road(_standardModelConfiguration with { DynamicSpeedLimit = 1});
        RunModel(road);
        SaveResultsToCsvItems();

        var finalItems = GetCsvItems();
        CsvExporter.Export("test", finalItems);
    }

    private void RunModel(Road road)
    {
        _trafficCount.Clear();
        _trafficFlow.Clear();
        _trafficJamIntensity.Clear();
    
        for (int numberOfCars = 2; numberOfCars < MaximumCars; numberOfCars++)
        {
            road.Reset(numberOfCars);
            road.IterateRounds(100);
            
            for (int numberOfIterations = 0; numberOfIterations < IterationsWithSameDensity; numberOfIterations++)
            {
                road.IterateRounds(100);
                _trafficCount.Add(numberOfCars);
                _trafficFlow.Add(road.TrafficFlow);
                _trafficJamIntensity.Add(road.TrafficIntensity);
            }
    
            Console.WriteLine("Iteration" + numberOfCars);
            Console.CursorTop--;
        }
        Console.WriteLine("Finished iterations");
    }
    
    private void SaveResultsToCsvItems()
    {
        var newItems = _trafficCount.Zip(_trafficFlow, _trafficJamIntensity)
            .Select(x => new CsvItem(x.First, x.Second, x.Third)).ToList();
        _allCsvItems.Add(newItems);
    }
    
    private IEnumerable<FinalCsvItem> GetCsvItems()
    {
        for (int i = 0; i < _allCsvItems[0].Count; i++)
        {
    
            yield return new FinalCsvItem(_allCsvItems[0][i].TrafficCount,
                _allCsvItems[0][i].TrafficFlow,
                _allCsvItems[1][i].TrafficFlow,
                _allCsvItems[2][i].TrafficFlow,
                _allCsvItems[3][i].TrafficFlow,
                _allCsvItems[4][i].TrafficFlow,
                _allCsvItems[0][i].TrafficJamIntensity,
                _allCsvItems[1][i].TrafficJamIntensity,
                _allCsvItems[2][i].TrafficJamIntensity,
                _allCsvItems[3][i].TrafficJamIntensity,
                _allCsvItems[4][i].TrafficJamIntensity);
        }
    }
    
    private record CsvItem(int TrafficCount, double TrafficFlow, double TrafficJamIntensity);

    // ReSharper disable NotAccessedPositionalProperty.Local
    private record FinalCsvItem(int TrafficCount, 
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
}

