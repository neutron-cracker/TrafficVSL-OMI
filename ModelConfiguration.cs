namespace TrafficVSL_OMI;

public record ModelConfiguration(
    int BackBufferSize,
    int CarDistance,
    int NumberOfSites,
    int NumberOfCars,
    int MaximumSpeed,
    float ProbabilityToSlowDown,
    int DynamicSpeedLimit)
{
    public static ModelConfiguration Default => new(BackBufferSize: 5,
        CarDistance: 1,
        NumberOfSites: 100,
        NumberOfCars: 30,
        MaximumSpeed: 5,
        ProbabilityToSlowDown: 0.35f,
        DynamicSpeedLimit: 2);
}