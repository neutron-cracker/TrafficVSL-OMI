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

    private readonly Dictionary<string, List<CsvItem>> _allCsvItems2 = new();
    private readonly List<List<CsvItem>> _allCsvItems = [];

    public void Run()
    {
        foreach (var dynamicSpeedLimit in new[] { 1, 2, 3, 4, 5 })
        {
            foreach (var carDistance in new[] { 3, 5, 10 })
            {
                RunAndSaveModel(_standardModelConfiguration with
                {
                    DynamicSpeedLimit = dynamicSpeedLimit,
                    CarDistance = carDistance
                });
            }
        }

        var finalItems = GetCsvItems();
        CsvExporter.Export("test", finalItems);
    }

    private void RunAndSaveModel(ModelConfiguration configuration)
    {
        var road = new Road(configuration);
        var keyName = $"Speed{configuration.DynamicSpeedLimit}Size{configuration.CarDistance}";
        RunModel(road);
        SaveResultsToCsvItems(keyName);
        Console.WriteLine($"Finished iterations for {keyName}");
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
        }
    }
    
    private void SaveResultsToCsvItems(string keyName)
    {
        var newItems = _trafficCount.Zip(_trafficFlow, _trafficJamIntensity)
            .Select(x => new CsvItem(x.First, x.Second, x.Third)).ToList();
        _allCsvItems2[keyName] = newItems;
    }
    
    private IEnumerable<FinalCsvItem> GetCsvItems()
    {
        var firstKey = _allCsvItems2.First().Key;
        for (int i = 0; i < _allCsvItems2[firstKey].Count; i++)
        {
            yield return new FinalCsvItem(
                _allCsvItems2[firstKey][i].TrafficCount,
                _allCsvItems2["Speed1Size3"][i].TrafficFlow,
                _allCsvItems2["Speed2Size3"][i].TrafficFlow,
                _allCsvItems2["Speed3Size3"][i].TrafficFlow,
                _allCsvItems2["Speed4Size3"][i].TrafficFlow,
                _allCsvItems2["Speed5Size3"][i].TrafficFlow,
                _allCsvItems2["Speed1Size5"][i].TrafficFlow,
                _allCsvItems2["Speed2Size5"][i].TrafficFlow,
                _allCsvItems2["Speed3Size5"][i].TrafficFlow,
                _allCsvItems2["Speed4Size5"][i].TrafficFlow,
                _allCsvItems2["Speed5Size5"][i].TrafficFlow,
                _allCsvItems2["Speed1Size10"][i].TrafficFlow,
                _allCsvItems2["Speed2Size10"][i].TrafficFlow,
                _allCsvItems2["Speed3Size10"][i].TrafficFlow,
                _allCsvItems2["Speed4Size10"][i].TrafficFlow,
                _allCsvItems2["Speed5Size10"][i].TrafficFlow,
                _allCsvItems2["Speed1Size3"][i].TrafficJamIntensity,
                _allCsvItems2["Speed2Size3"][i].TrafficJamIntensity,
                _allCsvItems2["Speed3Size3"][i].TrafficJamIntensity,
                _allCsvItems2["Speed4Size3"][i].TrafficJamIntensity,
                _allCsvItems2["Speed5Size3"][i].TrafficJamIntensity,
                _allCsvItems2["Speed1Size5"][i].TrafficJamIntensity,
                _allCsvItems2["Speed2Size5"][i].TrafficJamIntensity,
                _allCsvItems2["Speed3Size5"][i].TrafficJamIntensity,
                _allCsvItems2["Speed4Size5"][i].TrafficJamIntensity,
                _allCsvItems2["Speed5Size5"][i].TrafficJamIntensity,
                _allCsvItems2["Speed1Size10"][i].TrafficJamIntensity,
                _allCsvItems2["Speed2Size10"][i].TrafficJamIntensity,
                _allCsvItems2["Speed3Size10"][i].TrafficJamIntensity,
                _allCsvItems2["Speed4Size10"][i].TrafficJamIntensity,
                _allCsvItems2["Speed5Size10"][i].TrafficJamIntensity
            );
        }
    }
    
    private record CsvItem(int TrafficCount, double TrafficFlow, double TrafficJamIntensity);

    // ReSharper disable NotAccessedPositionalProperty.Local
    private record FinalCsvItem(int TrafficCount, 
        double TrafficFlowSpeed1Size3, 
        double TrafficFlowSpeed2Size3, 
        double TrafficFlowSpeed3Size3, 
        double TrafficFlowSpeed4Size3, 
        double TrafficFlowSpeed5Size3,
        double TrafficFlowSpeed1Size5, 
        double TrafficFlowSpeed2Size5, 
        double TrafficFlowSpeed3Size5, 
        double TrafficFlowSpeed4Size5, 
        double TrafficFlowSpeed5Size5,
        double TrafficFlowSpeed1Size10, 
        double TrafficFlowSpeed2Size10, 
        double TrafficFlowSpeed3Size10, 
        double TrafficFlowSpeed4Size10, 
        double TrafficFlowSpeed5Size10,
        double TrafficJamIntensitySpeed1Size3, 
        double TrafficJamIntensitySpeed2Size3, 
        double TrafficJamIntensitySpeed3Size3, 
        double TrafficJamIntensitySpeed4Size3, 
        double TrafficJamIntensitySpeed5Size3,
        double TrafficJamIntensitySpeed1Size5, 
        double TrafficJamIntensitySpeed2Size5, 
        double TrafficJamIntensitySpeed3Size5, 
        double TrafficJamIntensitySpeed4Size5, 
        double TrafficJamIntensitySpeed5Size5,
        double TrafficJamIntensitySpeed1Size10, 
        double TrafficJamIntensitySpeed2Size10, 
        double TrafficJamIntensitySpeed3Size10, 
        double TrafficJamIntensitySpeed4Size10, 
        double TrafficJamIntensitySpeed5Size10
        );
}

