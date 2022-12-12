// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");
var tt = new RangePolicy<DateTime>() { Min=DateTime.Now,max=DateTime.Now.AddDays(2)};
Console.WriteLine(tt.check(DateTime.Now.AddDays(0)));
Console.WriteLine(tt.check(tt.max));
Console.WriteLine(tt.check(DateTime.Now.AddDays(4)));


public interface Ipolicy<T> {
    bool check(T value);
}
public class RangePolicy<T> : Ipolicy<T>
    where T : IComparable<T>
{
    public T Min { get; set; }
    public T max { get; set; }
    public bool check(T value)
    {
        if(value.CompareTo(Min)>=0 && value.CompareTo(max)<=0)
            return true;    
        return false;
    }

    
}