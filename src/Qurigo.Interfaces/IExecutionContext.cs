namespace Qurigo.Interfaces;

public interface IExecutionContext
{
    void SetVariable(string name, object value);
    object GetVariable(string name);
    IQuantumCircuit QuantumCircuit { get; set; }
    IDictionary<string, FunctionParameter> FunctionParameters { get; set; }
    IDictionary<string, string> Variables { get; set; }
}
