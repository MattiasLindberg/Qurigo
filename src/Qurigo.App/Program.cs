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
        var serviceCollection = new ServiceCollection();

        serviceCollection.AddSingleton<IState, VectorState>();
        serviceCollection.AddSingleton<IInstructionSet, HTCNOTXYZ>();
        //serviceCollection.AddSingleton<IInstructionSet, IBMEagleR3>();
        serviceCollection.AddSingleton<ICircuit, BaseCircuit>();
        serviceCollection.AddSingleton<IQuantumCircuit, QuantumCircuit>();
        serviceCollection.AddSingleton<IExecutionContext, Circuit.BaseCircuit.ExecutionContext>();

        var serviceProvider = serviceCollection.BuildServiceProvider();

        FactorIntegerQiskit.Factor(15, serviceProvider);

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

    public double Run(string filepath)
    {
        string program = File.ReadAllText(filepath);

        Parser parser = new Parser();
        parser.Parse(new Tokenizer(program));

        foreach (IExecutionNode node in parser.Nodes)
        {
            node.Execute(_executionContext);
        }

        Console.WriteLine(_circuit.GetState().ToString());

        double measurement = _circuit.GetState().Measure();
        Console.WriteLine($"measurement= {measurement}");

        int measurementInt = (int)measurement;
        Console.WriteLine($"measurementInt= {measurementInt}");

        int measurementInt2 = (int)measurementInt & 0b111111;
        Console.WriteLine($"measurementInt2= {measurementInt2}");

        double returnValue = measurementInt2 / 64.0;

        return returnValue;
    }
}



