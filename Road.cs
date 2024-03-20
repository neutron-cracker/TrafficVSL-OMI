namespace TrafficVSL_OMI;

public class Road
{
    private readonly int _numberOfSites;
    private readonly int _vMax;
    private readonly float _p;
    private readonly int _vslSpeed;
    private readonly List<Car> _cars;
    private int _numberOfCars;
    private readonly int _carDistance;
    private readonly int _backBufferSize;

    public double TrafficFlow => _cars.Average(x => x.Speed);

    public double TrafficIntensity =>
        (double)_cars.Count(x => x.IsInTrafficJam(_carDistance)) / _numberOfCars;
    public Road(ModelConfiguration modelConfiguration)
    {
        _numberOfSites = modelConfiguration.NumberOfSites;
        _vMax = modelConfiguration.MaximumSpeed;
        _p = modelConfiguration.ProbabilityToSlowDown;
        _vslSpeed = modelConfiguration.DynamicSpeedLimit;
        _cars = new List<Car>(modelConfiguration.NumberOfCars);
        _backBufferSize = modelConfiguration.BackBufferSize;
        _carDistance = modelConfiguration.CarDistance;

        FillRoad();
    }

    public void Reset(int numberOfCars)
    {
        _cars.Clear();
        _numberOfCars = numberOfCars;
        FillRoad();
    }

    public IEnumerable<Car?> GetSites()
    {
        var car = _cars.MinBy(x => x.Location)!;
        for (int i = 0; i < _numberOfSites; i++)
        {
            if (car.Location == i)
            {
                var returnedCar = car;
                car = car.GetNext();
                yield return returnedCar;
            }
            else
            {
                yield return null;
            }
        }
    }

    public void IterateRounds(int count)
    {
        for (int i = 0; i < count; i++)
        {
            IterateOneRound();
        }
    }
    
    private void FillRoad()
    {
        var allIndices = Enumerable.Range(0, _numberOfSites).ToArray();
        
        Random.Shared.Shuffle(allIndices);

        foreach (var carIndex in allIndices.Take(_numberOfCars))
        {
            var car = new Car(_numberOfSites)
            {
                Location = carIndex,
            };

            _cars.Add(car);
        }
        
        _cars.Sort((a, b)=> a.Location.CompareTo(b.Location));

        for (int i = 0; i < _numberOfCars; i++)
        {
            var car = _cars[i];
            var nextCar = _cars[(i + 1) % _numberOfCars];
            car.SetNext(nextCar);
        }
    }
    
    private void IterateOneRound()
    {
        foreach (var car in _cars) 
            UpdateCarSpeed(car);
    }

    private void UpdateCarSpeed(Car car)
    {
        car.Accelerate(_vMax);
        car.BreakIfNeeded();
        car.ApplySpeedLimit(_vslSpeed, _backBufferSize, _carDistance);
        car.RandomiseSpeed(Random.Shared, _p);
        car.Drive();
    }
}