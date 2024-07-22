using Microsoft.Extensions.DependencyInjection;
using Qurigo.Circuit.BaseCircuit;
using Qurigo.InstructionSet.HTCNOT;
using Qurigo.Interfaces;
using Qurigo.State.VectorState;
using System;
using static Numpy.np;
using System.Numerics;
using System.Security.Cryptography;

namespace Qurigo.App;

internal class Program
{
    static void Main(string[] args)
    {
        FactorInteger.Factor(15);

        //var serviceCollection = new ServiceCollection();
        
        //serviceCollection.AddSingleton<IState, VectorState>();
        //serviceCollection.AddSingleton<IInstructionSet, HTCNOTXYZ>();
        ////serviceCollection.AddSingleton<IInstructionSet, IBMEagleR3>();
        //serviceCollection.AddSingleton<ICircuit, BaseCircuit>();
        //serviceCollection.AddSingleton<IQuantumCircuit, QuantumCircuit>();
        //serviceCollection.AddSingleton<IExecutionContext, Circuit.BaseCircuit.ExecutionContext>();

        //var serviceProvider = serviceCollection.BuildServiceProvider();

        //QurigoApp app = new QurigoApp(serviceProvider.GetService<ICircuit>(), serviceProvider.GetService<IQuantumCircuit>(), serviceProvider.GetService<IExecutionContext>());
        //app.Run("Programs/qft_3-inverse-function.qasm");




        //string program = File.ReadAllText("Programs-CircuitParsing/mini3.qasm");
        //var circuit = serviceProvider.GetService<ICircuit>();
        //circuit!.ExecuteProgram(program);
        //Console.WriteLine(circuit.GetState().ToString());
    }
}

internal class QurigoApp
{
    private readonly ICircuit _circuit;
    private readonly IQuantumCircuit _quantumCircuit;
    private readonly IExecutionContext _executionContext;

    public QurigoApp(ICircuit circuit, IQuantumCircuit quantumCircuit, IExecutionContext executionContext)
    {
        _quantumCircuit = quantumCircuit;
        _circuit = circuit;
        _executionContext = executionContext;
    }

    public void Run(string filepath)
    {
        string program = File.ReadAllText(filepath);

        Parser parser = new Parser();
        parser.Parse(new Tokenizer(program));

        foreach (IExecutionNode node in parser.Nodes)
        {
            node.Execute(_executionContext);
        }

        Console.WriteLine(_circuit.GetState().ToString());
    }
}


internal class FactorInteger
{
    public static void Factor(int N)
    {
        if (N % 2 == 0)
        {
            Console.WriteLine(2);
            return;
        }

        int a = RandomNumberGenerator.GetInt32(2, N - 1);
        a = 7; // For testing purposes

        int gcd = GCD(a, N);
        if (gcd != 1)
        {
            Console.WriteLine(gcd);
            return;
        }

        //// START OF QUANTUM PART

        // Step 3: Find the order r of a modulo N
        // Initialize quantum registers
        // - One for superposition of states |x> where x = 0, 1, ..., 2^n-1
        // - One for the modular exponentiation result
        //n = 4 // number of bits required to represent N
        //qreg1 = QuantumRegister(2 * n)
        //qreg2 = QuantumRegister(n)
        //creg = ClassicalRegister(2 * n)
        //qc = QuantumCircuit(qreg1, qreg2, creg)

        //// Apply Hadamard gate to qreg1 to create superposition
        //qc.h(qreg1)

        //// Apply modular exponentiation to qreg2 with base a and exponent qreg1
        //qc.modular_exponentiation(a, qreg1, qreg2, N)

        //// Apply Quantum Fourier Transform to qreg1
        //qc.qft(qreg1)  // Performed by qft_4-inverse.qasm

        //// Measure qreg1
        //qc.measure(qreg1, creg)

        //// Simulate or run the quantum circuit
        //result = execute(qc).result()
        //measured_value = result.get_measurement(creg)

        //// END OF QUANTUM PART

        // Simulate a response from QC
        int measured_value = 5;

        int r = FindPeriod(measured_value, N, a);

        if (r % 2 != 0 || BigInteger.ModPow(a, r, N) == 1)
        {
            Factor(N);
            return;
        }

        int factor1 = GCD((int)Math.Pow(a, r / 2) - 1, N);
        int factor2 = GCD((int)Math.Pow(a, r / 2) + 1, N);

        if (factor1 == 1 || factor1 == N)
        {
            Factor(N);
            return;
        }

        if (factor2 == 1 || factor2 == N)
        {
            Factor(N);
            return;
        }

        Console.WriteLine(factor1);
        Console.WriteLine(factor2);
    }

    //// Helper function: Modular exponentiation
    //function modular_exponentiation(a, x, N):
    //    // Implement modular exponentiation
    //    // (Detailed steps omitted for brevity)
    //    return result

    private static int GCD(int x, int y)
    {
        while (y != 0)
        {
            (x, y) = (y, x % y);
        }

        return x;
    }

    public static int FindPeriod(int measuredValue, int N, int a)
    {
        // Step 1: Normalize the measured value
        int n = (int)Math.Ceiling(Math.Log2(N));
        int Q = 1 << (2 * n); // 2^(2 * n)
        double fraction = (double)measuredValue / Q;

        // Step 2: Use continued fractions to find the period r
        List<Tuple<int, int>> convergents = ContinuedFractions(fraction);

        foreach (var convergent in convergents)
        {
            int numerator = convergent.Item1;
            int denominator = convergent.Item2;

            // Check if the denominator is a candidate for r
            if (denominator < N && BigInteger.ModPow(a, denominator, N) == 1)
            {
                return denominator;
            }
        }

        // If no suitable period found, return -1 indicating failure
        return -1;
    }

    private static List<Tuple<int, int>> ContinuedFractions(double fraction)
    {
        List<Tuple<int, int>> convergents = new List<Tuple<int, int>>();
        List<int> terms = new List<int>();

        // Generate continued fraction terms
        double x = fraction;
        while (x != Math.Floor(x))
        {
            int term = (int)Math.Floor(x);
            terms.Add(term);
            x = 1 / (x - term);
        }

        // Compute convergents
        for (int i = 0; i < terms.Count; i++)
        {
            Tuple<int, int> convergent = ComputeConvergent(terms, i);
            convergents.Add(convergent);
        }

        return convergents;
    }

    private static Tuple<int, int> ComputeConvergent(List<int> terms, int k)
    {
        if (k == 0)
        {
            return Tuple.Create(terms[0], 1);
        }

        int h1 = terms[0];
        int h2 = 1;
        int k1 = 1;
        int k2 = 0;

        for (int i = 1; i <= k; i++)
        {
            int a = terms[i];
            int h = a * h1 + h2;
            int kValue = a * k1 + k2;

            h2 = h1;
            h1 = h;
            k2 = k1;
            k1 = kValue;
        }

        return Tuple.Create(h1, k1);
    }
}
