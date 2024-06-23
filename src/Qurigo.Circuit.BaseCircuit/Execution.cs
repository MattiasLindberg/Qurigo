using Qurigo.Circuit.BaseCircuit;
using Qurigo.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qurigo.Circuit.BaseCircuit;

public interface IExecutionContext
{
    void SetVariable(string name, object value);
    object GetVariable(string name);
    IQuantumCircuit QuantumCircuit { get; set; }

}

public class ExecutionContext : IExecutionContext
{
    public IQuantumCircuit QuantumCircuit { get; set; }

    public ExecutionContext(IQuantumCircuit quantumCircuit)
    {
        QuantumCircuit = quantumCircuit;
    }

    public void SetVariable(string name, object value)
    {
        throw new NotImplementedException();
    }

    public object GetVariable(string name)
    {
        throw new NotImplementedException();
    }
}

public interface IExecutionNode
{
    void Execute(IExecutionContext context);
}

public interface IVariable
{
    string Name { get; }
    string Type { get; }
}

public class Parameter
{
    public string Name { get; set; }
    public string Type { get; set; }
    public int Index { get; set; }
}

public class Argument
{
    public string Name { get; set; }
    public string Type { get; set;  }
}

public class Expression
{
    public string Variable { get; set; }
    public string Operator { get; set; }
    public string Value { get; set; }

    void Evaluate(ExecutionContext context)
    {

    }
}

public class SubroutineNode : IExecutionNode
{
    public IList<Argument> Arguments = new List<Argument>();
    public IList<IExecutionNode> Nodes;
    public string Name;

    public void Execute(IExecutionContext context)
    {
        throw new NotImplementedException();
    }
}

public class CallSubroutineNode : IExecutionNode
{
    public IList<Argument> Arguments;
    public SubroutineNode Subroutine;

    public void Execute(IExecutionContext context)
    {
        // TODO: Implement argument passing

        foreach (var node in Subroutine.Nodes)
        {
            node.Execute(context);
        }
    }
}

public class QubitNode : IExecutionNode
{
    public string Name = "";
    public bool IsArray = false;
    public int Size = -1;


    public void Execute(IExecutionContext context)
    {
        // Initialize qubit with size of the given Size
        context.QuantumCircuit.Initialize(Size);
    }
}

public class GateNode : IExecutionNode
{
    public GateNames GateType;
    public IList<Parameter> Parameters;

    public void Execute(IExecutionContext context)
    {
        context.QuantumCircuit.ApplyGate(GateType, Parameters);
    }
}

public class IfNode : IExecutionNode
{
    public Expression Condition;
    public IList<IExecutionNode> IfNodes;
    public IList<IExecutionNode> ElseNodes;

    public void Execute(IExecutionContext context)
    {
        throw new NotImplementedException();
    }
}

public class WhileNode : IExecutionNode
{
    public Expression Condition;
    public IList<IExecutionNode> Nodes;

    public void Execute(IExecutionContext context)
    {
        throw new NotImplementedException();
    }
}

public class ForNode : IExecutionNode
{
    public Expression Condition;
    public IList<IExecutionNode> Nodes;

    public void Execute(IExecutionContext context)
    {
        throw new NotImplementedException();
    }
}
