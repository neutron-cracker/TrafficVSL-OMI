namespace TrafficVSL_OMI;

public class Counter
{
    private int _count = 0;
    public void Reset() => _count = 0;
    public int Value => _count;
}