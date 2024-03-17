namespace QurigoPoC1;

internal class Program
{
    static void Main(string[] args)
    {
        ApplyHadamard();

        MeasureNonEqualDist();

        MeasureHadamard();

        PauliXOn3Qubits();

        PauliXOn3QubitsAgain();

        CircuitThroughMultipleOperations();

        LargeQubit();
    }

    private static void CircuitThroughMultipleOperations()
    {
        // Building a circuit through a series of operations
        double a = Math.Sqrt(1D / 2);
        Qubit q = new Qubit(new double[] { 0, a, 0, 0, 0, 0, 0, a });
        q.PauliX(2).Hadamard(2).PauliX(3).Hadamard(1);
        var result = q.Measure();

        // Alternative circuit is to pre-define a matrix operator the perform
        // multiple transformations in one operation.
    }

    private static void LargeQubit()
    {
        var q2 = new Qubit(10);
        q2 = q2.Hadamard(1);
        Console.WriteLine(q2.ToString());
        q2 = q2.Hadamard(8);
        Console.WriteLine(q2.ToString());
    }

    private static void ApplyHadamard()
    {
        // Apply Hadamard on {1, 0}
        Console.WriteLine(new Qubit(new double[] { 1, 0 }).Hadamard(1).ToString());

        // Double Hadamard brings back original state
        Console.WriteLine(new Qubit(new double[] { 1, 0 }).Hadamard(1).Hadamard(1).ToString());

        Qubit q = new Qubit(new double[] { Math.Sqrt(3D/4), Math.Sqrt(1D/4) });
        // Print original state
        Console.WriteLine(q.ToString());
        q = q.Hadamard(1);
        // Print state after Hadamard
        Console.WriteLine(q.ToString());
        q = q.Hadamard(1);
        // Back to original state
        Console.WriteLine(q.ToString());
    }

    private static void MeasureNonEqualDist()
    {
        Console.WriteLine("Measuring a qubit with non-equal distribution. There should be more 0s than 1s.");

        double a = Math.Sqrt(3D/4);
        double b = Math.Sqrt(1D/4);
        Qubit q = new Qubit(new double[] { a, b });

        int zeroCount = 0;
        int oneCount = 0;
        for(int i = 0; i < 100; i++)
        {
            _ = q.Measure() switch
            {
                0 => zeroCount++,
                1 => oneCount++,
                _ => throw new NotImplementedException(),
            };
        }

        Console.WriteLine($"0s: {zeroCount}, 1s: {oneCount}");
    }

    private static void MeasureHadamard()
    {
        Console.WriteLine("Measuring a qubit after applying Hadamard. There should be equal distribution of 0s and 1s.");
        Console.WriteLine(new Qubit(new double[] { 0, 1 }).Hadamard(1).ToString());

        Console.WriteLine(new Qubit(new double[] { 0, 1 }).Hadamard(1).Measure());
        Console.WriteLine(new Qubit(new double[] { 0, 1 }).Hadamard(1).Measure());
        Console.WriteLine(new Qubit(new double[] { 0, 1 }).Hadamard(1).Measure());
        Console.WriteLine(new Qubit(new double[] { 0, 1 }).Hadamard(1).Measure());
        Console.WriteLine(new Qubit(new double[] { 0, 1 }).Hadamard(1).Measure());

        Console.WriteLine(new Qubit(new double[] { 0, 1 }).Hadamard(1).Measure());
        Console.WriteLine(new Qubit(new double[] { 0, 1 }).Hadamard(1).Measure());
        Console.WriteLine(new Qubit(new double[] { 0, 1 }).Hadamard(1).Measure());
        Console.WriteLine(new Qubit(new double[] { 0, 1 }).Hadamard(1).Measure());
        Console.WriteLine(new Qubit(new double[] { 0, 1 }).Hadamard(1).Measure());
    }

    private static Qubit PauliXOn3QubitsAgain()
    {
        Qubit q;
        double a = Math.Sqrt(1D/2);

        Console.WriteLine("Flipping the THIRD qubit");
        q = new Qubit(new double[] { 0, a, 0, 0, 0, 0, 0, a });
        Console.WriteLine(q.ToString());
        Console.WriteLine(q.PauliX(3).ToString());
        return q;
    }

    private static Qubit PauliXOn3Qubits()
    {
        Qubit q;
        double a = Math.Sqrt(1D/4);

        Console.WriteLine("Flipping the FIRST qubit");
        q = new Qubit(new double[] { a, a, a, a, 0, 0, 0, 0 });
        Console.WriteLine(q.ToString());
        Console.WriteLine(q.PauliX(1).ToString());

        Console.WriteLine("Flipping the SECOND qubit");
        q = new Qubit(new double[] { a, a, 0, 0, a, a, 0, 0 });
        Console.WriteLine(q.ToString());
        Console.WriteLine(q.PauliX(2).ToString());

        Console.WriteLine("Flipping the THIRD qubit");
        q = new Qubit(new double[] { 0, a, 0, a, 0, a, 0, a });
        Console.WriteLine(q.ToString());
        Console.WriteLine(q.PauliX(3).ToString());
        return q;
    }
}
