using System.Numerics;

namespace Qurigo.Factorization;

internal class Program
{
    static async Task Main(string[] args)
    {
        swaggerClient client = new("https://localhost:7130/", new HttpClient());
        IEnumerable<Complex> state = await client.ExecuteAsync("qreg q[2];creg c[4];h q[0];cx q[0], q[1];");
        foreach(Complex complex in state)
            Console.WriteLine(complex.Real + " + " + complex.Imaginary + "i");
    }
}
