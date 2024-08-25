using Microsoft.Extensions.DependencyInjection;
using Qurigo.Simulator;
using Qurigo.Interfaces;

namespace ShorsAlgorithm;

internal class FactorInteger
{
    private static int Attempt = 0;

    public static void Factor(int N, IServiceProvider serviceProvider)
    {
        Console.WriteLine($"Using Shor's algorithm to factor {N}");

        FactorImpl(N, serviceProvider);
    }

    private static void FactorImpl(int N, IServiceProvider serviceProvider)
    {
        Attempt++;
        Console.WriteLine($"Attempt= {Attempt}");

        if (N % 2 == 0)
        {
            Console.WriteLine("N is even: 2 is a factor!");
            return;
        }

        //// START OF QUANTUM PART

        QurigoSimulator app = new QurigoSimulator(serviceProvider.GetService<IExecutionContext>()!);
        double phase = app.Run("phase_estimate_atomicswaps_4-func.qasm");

        //// END OF QUANTUM PART

        Console.WriteLine($"Measured phase: {phase}");

        if (phase == 0 || phase == 1)
        {
            // Try again
            Console.WriteLine($"Phase was 0 or 1, try again.");
            FactorImpl(N, serviceProvider);
            return;
        }
        
        // It should be 2^#qubits. TODO: Calculate this dynamically
        Fraction frac = Fraction.LimitDenominator(phase, 64);
        Console.WriteLine($"frac: {frac}");
        int r = frac.Denominator;
        Console.WriteLine($"r: {r}");

        int factor = GCD((int)Math.Pow(8, r / 2) - 1, N);
        Console.WriteLine($"Candidate factor: {factor}");

        if(factor == 1 || factor == N)
        {
            // Try again
            Console.WriteLine($"Trivial factor, try again.");
            FactorImpl(N, serviceProvider);
            return;
        }

        if(N % factor == 0)
        {
            Console.WriteLine($"Attempt= {Attempt}");

            Console.WriteLine($"Non-trivial factor found: {factor}");
            return;
        }

        // Try again
        Console.WriteLine($"Not an real factor, try again.");
        FactorImpl(N, serviceProvider);
    }

    private static int GCD(int x, int y)
    {
        while (y != 0)
        {
            (x, y) = (y, x % y);
        }

        return x;
    }
}
