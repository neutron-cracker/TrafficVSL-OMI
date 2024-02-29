namespace TrafficVSL_OMI;

public class Road
{
    private readonly int _numberOfSites;
    private readonly int _vMax;
    private readonly float _p;
    private readonly int _vslSpeed;
    private readonly List<Car> _cars;
    private int _numberOfCars;

    public int TotalSpeed => _cars.Sum(x => x.Speed);
    public Road(int numberOfSites, int maximumNumberOfCars, int vMax, float p, int vslSpeed)
    {
        _numberOfSites = numberOfSites;
        _vMax = vMax;
        _p = p;
        _vslSpeed = vslSpeed;
        _cars = new List<Car>(maximumNumberOfCars);

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
                car = car.GetNext();
                yield return car;
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
                Speed = 0,
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
        car.ApplySpeedLimit(_vslSpeed);
        car.RandomiseSpeed(Random.Shared, _p);
        car.Drive();
    }
}