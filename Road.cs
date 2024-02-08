namespace TrafficVSL_OMI;

public class Road
{
    private readonly int _numberOfSites;
    private readonly int _numberOfCars;
    private readonly int _vMax;
    private readonly float _p;
    private Car?[] _sites;
    private readonly List<Car> _cars;

    public float AverageSpeed => _cars.Average(car => (float)car.Speed);

    public Road(int numberOfSites, int numberOfCars, int vMax, int p)
    {
        _numberOfSites = numberOfSites;
        _numberOfCars = numberOfCars;
        _vMax = vMax;
        _p = p;
        _sites = new Car?[numberOfSites];
        _cars = new List<Car>(numberOfCars);

        FillRoad();
    }

    private void FillRoad()
    {
        var allIndices = Enumerable.Range(0, _numberOfSites).ToArray();
        
        Random.Shared.Shuffle(allIndices);

        foreach (var carIndex in allIndices.Take(_numberOfCars))
        {
            var car = new Car
            {
                Speed = 0
            };

            _sites[carIndex] = car;
            _cars.Add(car);
        }
    }
    
    public void IterateRounds(int count)
    {
        for (int i = 0; i < count; i++)
        {
            IterateOneRound();
        }
    }

    private void IterateOneRound()
    {
        var newCars = new Car?[_numberOfSites];
        for (int i = 0; i < _numberOfSites; i++)
        {
            var nullableCar = _sites[i];
            if (!nullableCar.HasValue) continue;

            UpdateCarSpeed(nullableCar.Value, i);

            var newIndex = nullableCar.Value.Speed + i;
            newIndex %= _numberOfSites;
            newCars[newIndex] = nullableCar.Value;
        }

        _sites = newCars;
    }

    private void UpdateCarSpeed(Car car, int oldIndex)
    {
        var nextCarIndex = GetNextCarIndexWithinRange(oldIndex + 1, car.Speed + 1);

        if (nextCarIndex.HasValue)
        {
            // Slowing down
            var newSpeed = nextCarIndex.Value - oldIndex;
            car.Speed = newSpeed;
        }
        else
        {
            if (car.Speed < _vMax)
            {
                // Accelerating
                car.Speed += 1;
            }
        }
        
        // Reduce speed
        var shouldAcceptSlowDown = Random.Shared.NextSingle() < _p;
        if (shouldAcceptSlowDown)
        {
            car.Speed -= 1;
        }
    }
    
    private int? GetNextCarIndexWithinRange(int startingIndex, int range)
    {
        for (int i = 0; i < range; i++)
        {
            var nextCar = _sites[startingIndex + i];
            if (nextCar.HasValue)
            {
                return startingIndex + i;
            }
        }

        return null;
    }
}

public struct Car
{
    public int Speed { get; set; }
}