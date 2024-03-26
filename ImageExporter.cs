using System.Drawing;
using System.Drawing.Imaging;

namespace TrafficVSL_OMI;

public class ImageExporter(ModelConfiguration modelConfiguration)
{
    private readonly Bitmap _bitmap = new(100, 100);
    private int _currentHeight;

    public void PrintLine(Road road)
    {
        foreach (var (color, x) in road.GetSites().Select(GetColorForSite).WithIndex())
        {
            _bitmap.SetPixel(x, _currentHeight, color);
        }

        _currentHeight++;
    }

    private Color GetColorForSite(Car? site) =>
        site is null
            ? Color.White
            : site.IsInTrafficJam(modelConfiguration.CarDistance)
                ? GetColorInTrafficForCar(site)
                : site.HasSpeedLimit(modelConfiguration.BackBufferSize, modelConfiguration.CarDistance)
                    ? GetColorWithSpeedLimitForCar(site)
                    : GetColorOutOfTrafficForCar(site);

    private static Color GetColorOutOfTrafficForCar(Car car) => car.Speed switch
    {
        0 => Color.FromArgb(0, 204, 0),
        1 => Color.FromArgb(0, 255, 0),
        2 => Color.FromArgb(51, 255, 51),
        3 => Color.FromArgb(102, 255, 102),
        4 => Color.FromArgb(153, 255, 153),
        5 => Color.FromArgb(204, 255, 204),
        _ => throw new ArgumentOutOfRangeException()
    };
    
    private static Color GetColorWithSpeedLimitForCar(Car car) => car.Speed switch
    {
        0 => Color.FromArgb(0, 0, 204),
        1 => Color.FromArgb(0, 0, 255),
        2 => Color.FromArgb(51, 51, 255),
        3 => Color.FromArgb(102, 102, 255),
        4 => Color.FromArgb(153, 153, 255),
        5 => Color.FromArgb(204, 204, 255),
        _ => throw new ArgumentOutOfRangeException()
    };

    private static Color GetColorInTrafficForCar(Car car) => car.Speed switch
    {
        0 => Color.FromArgb(204, 0, 0),
        1 => Color.FromArgb(255, 0, 0),
        2 => Color.FromArgb(255, 51, 51),
        3 => Color.FromArgb(255, 102, 102),
        4 => Color.FromArgb(255, 153, 153),
        5 => Color.FromArgb(255, 204, 204),
        _ => throw new ArgumentOutOfRangeException()
    };

    public void Export(string fileName) => _bitmap.Save($"../../../{fileName}", ImageFormat.Png);
}

public static class EnumerableExtensions
{
    public static IEnumerable<(T item, int index)> WithIndex<T>(this IEnumerable<T> source)
    {
        int index = 0;
        foreach (var item in source)
        {
            yield return (item, index++);
        }
    }
}
