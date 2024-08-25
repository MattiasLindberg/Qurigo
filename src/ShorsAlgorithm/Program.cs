using Microsoft.Extensions.DependencyInjection;
using Qurigo.Circuit;
using Qurigo.InstructionSet;
using Qurigo.Interfaces;
using Qurigo.State;
namespace ShorsAlgorithm;

internal class Program
{
    static void Main(string[] args)
    {
        var serviceCollection = new ServiceCollection();

        /////////////////////////////////////////
        // Pick a state representation
        serviceCollection.AddSingleton<IState, VectorState>();
        //serviceCollection.AddSingleton<IState, DensityMatrix>();

        /////////////////////////////////////////
        // Pick an instruction set
        serviceCollection.AddSingleton<IInstructionSet, HTCNOTXYZ>();
        //serviceCollection.AddSingleton<IInstructionSet, IBMEagleR3>();

        /////////////////////////////////////////
        // These are always the same
        serviceCollection.AddSingleton<IQuantumCircuit, QuantumCircuit>();
        serviceCollection.AddSingleton<IExecutionContext, Qurigo.Simulator.ExecutionContext>();

        var serviceProvider = serviceCollection.BuildServiceProvider();

        FactorInteger.Factor(15, serviceProvider);
    }
}
