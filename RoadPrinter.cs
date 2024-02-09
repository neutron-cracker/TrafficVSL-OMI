namespace TrafficVSL_OMI;

public static class RoadPrinter
{
    public static void PrintDebugLine(Road road)
    {
        var siteText = road.Sites.Select(GetDebugSiteText);
        var text = string.Concat(siteText);
        Console.WriteLine(text);
    }

    private static string GetDebugSiteText(Car? car) => car switch
    {
        { } carValue => carValue.Speed.ToString(),
        _ => "."
    };
}