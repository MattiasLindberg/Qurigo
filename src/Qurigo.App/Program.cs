using Microsoft.Extensions.DependencyInjection;
using Qurigo.Circuit.BaseCircuit;
using Qurigo.InstructionSet.HTCNOT;
using Qurigo.Interfaces;
using Qurigo.State.VectorState;
using System;

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

        QurigoApp app = new QurigoApp(serviceProvider.GetService<ICircuit>(), serviceProvider.GetService<IQuantumCircuit>(), serviceProvider.GetService<IExecutionContext>());
        app.Run();

        // string program = File.ReadAllText("Programs/mini2.qasm");
        // var circuit = serviceProvider.GetService<ICircuit>();
        // circuit.ExecuteProgram(program);
        // Console.WriteLine(circuit.GetState().ToString());
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

    public void Run()
    {
        string program = File.ReadAllText("Programs/mini3.qasm");

        Parser parser = new Parser();
        parser.Parse(new Tokenizer(program));

        foreach (IExecutionNode node in parser.Nodes)
        {
            node.Execute(_executionContext);
        }

        Console.WriteLine(_circuit.GetState().ToString());
    }
}
