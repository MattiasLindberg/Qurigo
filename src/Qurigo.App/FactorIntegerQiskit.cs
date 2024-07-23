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
    public static void Factor(int N)
    {
        if (N % 2 == 0)
        {
            Console.WriteLine(2);
            return;
        }

        int a = 8;

        //// START OF QUANTUM PART

        //// END OF QUANTUM PART

        // Simulate a response from QC
        double phase = 0.75;
        

        if(phase == 0 || phase == 1)
        {
            // Try again
            Console.WriteLine($"Phase was 0 or 1, try again.");
            Factor(N);
            return;
        }
        
        Console.WriteLine($"Measured phase: {phase}");
        Fraction frac = Fraction.LimitDenominator(phase, 16);
        Console.WriteLine($"frac: {frac}");
        int r = frac.Denominator;
        Console.WriteLine($"r: {r}");

        int factor = GCD((int)Math.Pow(a, r / 2) - 1, N);
        Console.WriteLine($"Candidate factor: {factor}");

        if(factor == 1 || factor == N)
        {
            // Try again
            Console.WriteLine($"Trivial factor, try again.");
            Factor(N);
            return;
        }

        if(N % factor == 0)
        {
            Console.WriteLine($"Non-trivial factor found: {factor}");
            return;
        }

        // Try again
        Console.WriteLine($"Not an real factor, try again.");
        Factor(N);
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