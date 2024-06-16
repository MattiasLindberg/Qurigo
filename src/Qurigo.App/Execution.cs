using Qurigo.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qurigo.App;

internal interface IExecutionNode
{
    void Execute();
}

internal interface IVariable
{
    string Name { get; }
    string Type { get; }
}

internal class Parameter
{
    public string Name { get; set; }
    public string Type { get; set; }
    public int index { get; set; }
}

internal interface IArgument
{
    string Name { get; }
    string Type { get; }
}

internal class Expression
{
    public string Variable { get; set; }
    public string Operator { get; set; }
    public string Value { get; set; }

    void Evaluate()
    {

    }
}

internal class SubroutineNode : IExecutionNode
{
    public IList<IArgument> Arguments;
    public IList<IExecutionNode> Nodes;
    public string Name;

    public void Execute()
    {
        throw new NotImplementedException();
    }
}

internal class QubitNode : IExecutionNode
{
    public string Name;
    public bool IsArray = false;
    public int Size = -1;

    public void Execute()
    {
        throw new NotImplementedException();
    }
}

internal class GateNode : IExecutionNode
{
    public GateNames GateType;
    public IList<Parameter> Parameters;

    public void Execute()
    {
        throw new NotImplementedException();
    }
}

internal class IfNode : IExecutionNode
{
    public Expression Condition;
    public IList<IExecutionNode> IfNodes;
    public IList<IExecutionNode> ElseNodes;

    public void Execute()
    {
        throw new NotImplementedException();
    }
}

internal class WhileNode : IExecutionNode
{
    public Expression Condition;
    public IList<IExecutionNode> Nodes;

    public void Execute()
    {
        throw new NotImplementedException();
    }
}

internal class ForNode : IExecutionNode
{
    public Expression Condition;
    public IList<IExecutionNode> Nodes;

    public void Execute()
    {
        throw new NotImplementedException();
    }
}
