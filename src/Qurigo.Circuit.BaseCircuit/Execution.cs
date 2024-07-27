﻿using Microsoft.CodeAnalysis.CSharp.Scripting;
using Qurigo.Circuit.BaseCircuit;
using Qurigo.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Qurigo.Circuit.BaseCircuit;

public interface IExecutionContext
{
    void SetVariable(string name, object value);
    object GetVariable(string name);
    IQuantumCircuit QuantumCircuit { get; set; }
    List<FunctionParameter> FunctionParameters { get; set; }

}

public class ExecutionContext : IExecutionContext
{
    public IQuantumCircuit QuantumCircuit { get; set; }

    public List<FunctionParameter> FunctionParameters { get; set; }

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

public class FunctionParameter
{
    public string Name { get; set; }
    public int Value { get; set; }
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
    public List<FunctionParameter> FunctionParameters;
    public SubroutineNode Subroutine;

    public void Execute(IExecutionContext context)
    {
        context.FunctionParameters = FunctionParameters;
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

public class ControlGateNode : IExecutionNode
{
    public CallSubroutineNode Subroutine;
    public IList<Parameter> Parameters;

    public void Execute(IExecutionContext context)
    {
//        context.QuantumCircuit.ApplyGate(GateType, Parameters);
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

public class GlobalsDictionary
{
    public Dictionary<string, object> Variables { get; set; }
}

public class IfNode : IExecutionNode
{
    public Expression Condition;
    public IList<IExecutionNode> IfNodes;
    public IList<IExecutionNode> ElseNodes;

    public void Execute(IExecutionContext context)
    {
        Dictionary<string, object> globals = new Dictionary<string, object>();

        if (context.FunctionParameters != null)
        {
            globals.Add("arg1", 8);
        }

        // Get condition for the if statement and evaluate it.
        string expression = Condition.Variable + Condition.Operator + Condition.Value;
        string declarions = GenerateVariablesCode(context, globals);
        string script = GenerateScript(expression, declarions);
        bool conditionResult = CSharpScript.EvaluateAsync<bool>(script).Result;

        if(conditionResult)
        {
            foreach (var node in IfNodes)
            {
                node.Execute(context);
            }
        }
        else if(ElseNodes != null)
        {
            foreach (var node in ElseNodes)
            {
                node.Execute(context);
            }
        }
    }


    private string GenerateScript(string expression, string declarations)
    {
        // Generates a script that reads variables from the Globals dictionary
        return @$"
            bool Evaluate()
            {{
                {declarations}
                return {expression};
            }}

            return Evaluate();
        ";
    }

    private string GenerateVariablesCode(IExecutionContext context, Dictionary<string, object> globals)
    {
        string declarations = "";
        foreach (var parameter in context.FunctionParameters)
        {
            declarations += $"var {parameter.Name} = (int){parameter.Value}; \n";
        }

        foreach (var global in globals)
        {
            declarations += $"var {global.Key} = (int){global.Value}; \n";
        }

        return declarations;
    }
}

public class WhileNode : IExecutionNode
{
    public Expression Condition;
    public IList<IExecutionNode> Nodes;

    public void Execute(IExecutionContext context)
    {
        int arg1 = 0;

        // Get condition for the if statement and evaluate it.
        string expression = Condition.Variable + Condition.Operator + Condition.Value;
        bool conditionResult = CSharpScript.EvaluateAsync<bool>(expression, globals: new Globals { arg1 = arg1}).Result;

        while (conditionResult)
        {
            foreach (var node in Nodes)
            {
                node.Execute(context);
            }

            // And the arg1 must be updated to somehow break the loop
            arg1++;

            conditionResult = CSharpScript.EvaluateAsync<bool>(expression, globals: new Globals { arg1 = arg1 }).Result;
        }
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
