namespace TrafficVSL_OMI;

public class Car(int numberOfSites)
{
    private Car _nextCar = null!;
    public int Speed { get; set; } = 0;
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
        if (Speed >= distance)
        {
            Speed = distance - 1;
        }
    }

    public void ApplySpeedLimit(int speedLimitJam)
    {
        const int maximumDistanceFirstCar = 3;
        const int maximumDistanceSecondCar = maximumDistanceFirstCar + 1;
        const int maximumDistanceThirdCar = maximumDistanceSecondCar + 1;
        
        if (Speed > speedLimitJam) return;
        
        var firstCar = _nextCar;
        if (DistanceToCar(firstCar) > maximumDistanceFirstCar) return;
        
        var secondCar = firstCar._nextCar;
        if (DistanceToCar(secondCar) > maximumDistanceSecondCar) return;

        var thirdCar = secondCar._nextCar;
        if (DistanceToCar(thirdCar) > maximumDistanceThirdCar) return;

        Speed = speedLimitJam;
    }

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