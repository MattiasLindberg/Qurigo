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
        //serviceCollection.AddSingleton<IInstructionSet, HTCNOTXYZ>();
        serviceCollection.AddSingleton<IInstructionSet, IBMEagleR3>();
        serviceCollection.AddSingleton<ICircuit, BaseCircuit>();

        var serviceProvider = serviceCollection.BuildServiceProvider();

        string program = File.ReadAllText("Programs/IBM2.qasm");

        var circuit = serviceProvider.GetService<ICircuit>();
        circuit.ExecuteProgram(program);
        
        //circuit.ExecuteProgram("qreg q[6];\r\nx q[1];\r\nh q[2];\r\nx q[0];\r\nt q[5];\r\nx q[4];\r\ncx q[2], q[3];\r\nz q[2];\r\nccx q[1], q[2], q[4];\r\ncx q[0], q[1];\r\nccx q[2], q[3], q[4];\r\nx q[0];\r\nswap q[1], q[2];\r\nx q[0];\r\nswap q[2], q[3];\r\nswap q[0], q[1];\r\nccx q[3], q[4], q[5];\r\nh q[5];\r\ncx q[1], q[2];");
        
        Console.WriteLine(circuit.GetState().ToString());
    }
}
