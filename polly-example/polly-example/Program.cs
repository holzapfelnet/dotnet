#pragma warning disable SA1600
#pragma warning disable S1118

namespace PollyExample;

public class Program
{
    public static void Main()
    {
        var service = new FearAndGreedService();

        var result = service.GetFearAndGreedData().Result;

        Console.WriteLine(result);
    }
}