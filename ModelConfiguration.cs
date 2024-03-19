namespace TrafficVSL_OMI;

public record ModelConfiguration(
    int BackBufferSize, 
    int CarDistance,
    int NumberOfSites, 
    int NumberOfCars, 
    int MaximumSpeed,
    float ProbabilityToSlowDown, 
    int DynamicSpeedLimit);