using Qurigo.Circuit;
using Qurigo.Interfaces;
using Qurigo.Simulator;

namespace Qurigo.App;

internal class QurigoApp
{
    private readonly IExecutionContext _executionContext;

    public QurigoApp(IExecutionContext executionContext)
    {
        _executionContext = executionContext;
    }

    public double Run(string filepath)
    {
        string program = File.ReadAllText(filepath);

        Parser parser = new Parser();
        parser.Parse(new Tokenizer(program));
        _executionContext.Variables = parser.Variables;

        foreach (IExecutionNode node in parser.Nodes)
        {
            node.Execute(_executionContext);
        }

        // Console.WriteLine(_executionContext.QuantumCircuit.GetState().ToString());

        double measurement = _executionContext.QuantumCircuit.GetState().Measure();
        Console.WriteLine($"measurement= {measurement}");

        int measurementInt = (int)measurement;
        Console.WriteLine($"measurementInt= {measurementInt}");

        int measurementInt2 = (int)measurementInt & 0b1111;
        Console.WriteLine($"measurementInt2= {measurementInt2}");

        double returnValue = measurementInt2 / 16.0;

        return returnValue;
    }
}



