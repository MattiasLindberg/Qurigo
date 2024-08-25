namespace ShorsAlgorithm;

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