using Microsoft.Extensions.DependencyInjection;
using Qurigo.Circuit.BaseCircuit;
using Qurigo.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Qurigo.App;
internal class FactorIntegerQiskit
{
    private static int Attempt = 0;

    public static void Factor(int N, IServiceProvider serviceProvider)
    {
        Console.WriteLine($"");

        Attempt++;
        Console.WriteLine($"Attempt= {Attempt}");

        if (N % 2 == 0)
        {
            Console.WriteLine("N is even: 2 is a factor!");
            return;
        }

        //// START OF QUANTUM PART

        QurigoApp app = new QurigoApp(serviceProvider.GetService<ICircuit>()!, serviceProvider.GetService<IQuantumCircuit>()!, serviceProvider.GetService<IExecutionContext>()!);
        // double phase = app.Run("Programs/phase_estimate_atomicswaps_7controls.qasm");
        // double phase = app.Run("Programs/phase_estimate_atomicswaps.qasm");
        // double phase = app.Run("Programs/phase_estimate_atomicswaps_4.qasm");
        // double phase = app.Run("Programs/phase_estimate_atomicswaps_4-func.qasm");
        // double phase = app.Run("Programs/phase_estimate_atomicswaps_5.qasm");
        // double phase = app.Run("Programs/phase_estimate_atomicswaps_6.qasm");
        double phase = app.Run("Programs/phase_estimate_atomicswaps_6-func.qasm");

        //// END OF QUANTUM PART

        Console.WriteLine($"Measured phase: {phase}");

        if (phase == 0 || phase == 1)
        {
            // Try again
            Console.WriteLine($"Phase was 0 or 1, try again.");
            Factor(N, serviceProvider);
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
            Factor(N, serviceProvider);
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
        Factor(N, serviceProvider);
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

public class Fraction
{
    public int Numerator { get; set; }
    public int Denominator { get; set; }

    public Fraction(int numerator, int denominator)
    {
        Numerator = numerator;
        Denominator = denominator;
    }

    public override string ToString()
    {
        return $"{Numerator}/{Denominator}";
    }

    public static Fraction LimitDenominator(double value, int maxDenominator)
    {
        int n = (int)Math.Round(value * maxDenominator);
        int d = maxDenominator;

        // Simplify the fraction
        int gcd = GCD(n, d);
        return new Fraction(n / gcd, d / gcd);
    }

    private static int GCD(int a, int b)
    {
        while (b != 0)
        {
            int temp = b;
            b = a % b;
            a = temp;
        }
        return a;
    }
}