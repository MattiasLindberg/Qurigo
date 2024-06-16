using Microsoft.Extensions.DependencyInjection;
using Qurigo.Circuit.BaseCircuit;
using Qurigo.InstructionSet.HTCNOT;
using Qurigo.Interfaces;
using Qurigo.State.VectorState;

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

        var serviceProvider = serviceCollection.BuildServiceProvider();

        string program = File.ReadAllText("Programs/mini_function.qasm");

        Parser parser = new Parser();
        parser.Parse(new Tokenizer(program));
        foreach (var node in parser.Nodes)
        {
            node.Execute();
        }

        var circuit = serviceProvider.GetService<ICircuit>();
        circuit.ExecuteProgram(program);
                
        Console.WriteLine(circuit.GetState().ToString());
    }
}
