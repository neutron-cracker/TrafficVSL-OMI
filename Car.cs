namespace TrafficVSL_OMI;

public class Car(int numberOfSites)
{
    private Car _nextCar = null!;
    public int Speed { get; private set; }
    public required int Location { get; set; }

    public void SetNext(Car nextCar)
    {
        _nextCar = nextCar;
    }

    public Car GetNext() => _nextCar;

    public void Accelerate(int maximumSpeed)
    {
        if (Speed < maximumSpeed)
        {
            Speed++;
        }
    }

    public void BreakIfNeeded()
    {
        var distance = DistanceToCar(_nextCar);
        if (distance == 0)
        {
            throw new Exception("Should never happen");
        }
        if (Speed >= distance)
        {
            Speed = distance - 1;
        }
    }

    public void ApplySpeedLimit(int speedLimitJam, int backBufferSize, int carDistanceInTrafficJam)
    {
        if (Speed > speedLimitJam && HasSpeedLimit(backBufferSize, carDistanceInTrafficJam)) 
            Speed = speedLimitJam;
    }

    public bool HasSpeedLimit(int backBufferSize, int carDistanceInTrafficJam) =>
        DistanceToCar(_nextCar) <= carDistanceInTrafficJam 
        || (DistanceToCar(_nextCar) <= backBufferSize
            && _nextCar.DistanceToCar(_nextCar._nextCar) <= carDistanceInTrafficJam);

    public bool IsInTrafficJam(int carDistanceInTrafficJam) 
        => DistanceToCar(_nextCar) <= carDistanceInTrafficJam;

    private int DistanceToCar(Car car)
    {
        var rawDistance = car.Location - Location;
        if (rawDistance < 0)
        {
            rawDistance += numberOfSites;
        }
        
        return rawDistance;
    }

    public void RandomiseSpeed(Random random, float probability)
    {
        if (Speed > 0 && random.NextSingle() < probability)
            Speed--;
    }

    public void Drive()
    {
        Location += Speed;
        Location %= numberOfSites;
    }
}