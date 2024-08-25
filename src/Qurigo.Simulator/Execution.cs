using Microsoft.CodeAnalysis.CSharp.Scripting;
using Qurigo.Interfaces;

namespace Qurigo.Simulator;

public class ExecutionContext : IExecutionContext
{
    public IQuantumCircuit QuantumCircuit { get; set; }

    public IDictionary<string, FunctionParameter> FunctionParameters { get; set; }
    public IDictionary<string, string> Variables { get; set; }

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
    public string ValueReference { get; set; }

    void Evaluate(ExecutionContext context)
    {

    }
}

public class SubroutineNode : IExecutionNode
{
    public string Name;

    public IList<Argument> Arguments = new List<Argument>();
    public IList<IExecutionNode> Nodes;
    public IDictionary<string, string> Variables = new Dictionary<string, string>();

    public void Execute(IExecutionContext context)
    {
        throw new NotImplementedException();
    }
}

public class CallSubroutineNode : IExecutionNode
{
    public IDictionary<string, FunctionParameter> FunctionParameters;
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

public class VariableNode : IExecutionNode
{
    public string VariableName = "";
    public string Expression = "";


    public void Execute(IExecutionContext context)
    {
        string expression = Expression;
        string declarions = GenerateVariablesCode(context);
        string script = GenerateScript(expression, declarions);
        int result = CSharpScript.EvaluateAsync<int>(script).Result;

        context.Variables[VariableName] = result.ToString();
    }


    private string GenerateScript(string expression, string declarations)
    {
        // Generates a script that reads variables from the Globals dictionary
        return @$"
            int Evaluate()
            {{
                {declarations}
                return {expression};
            }}

            return Evaluate();
        ";
    }

    private string GenerateVariablesCode(IExecutionContext context)
    {
        string declarations = "";
        foreach (var parameter in context.Variables)
        {
            declarations += $"var {parameter.Key} = (int){parameter.Value}; \n";
        }

        return declarations;
    }
}

public class GateNode : IExecutionNode
{
    public GateNames GateType;
    public IList<Parameter> Parameters;

    public void Execute(IExecutionContext context)
    {
        if(GateType == GateNames.CCX)
        {
            string s = "";
        }
        foreach(Parameter p in Parameters)
        {
            if (p.ValueReference != null)
            {
                if (context.Variables.ContainsKey(p.ValueReference))
                {
                    p.Index = int.Parse(context.Variables[p.ValueReference]);
                }
                else if (context.FunctionParameters.ContainsKey(p.ValueReference))
                {
                    p.Index = context.FunctionParameters[p.ValueReference].Value;
                }
            }
        }
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
        Dictionary<string, object> globals = new Dictionary<string, object>();

        if (context.FunctionParameters != null)
        {
            globals.Add("arg1", 8);
        }

        // Get condition for the if statement and evaluate it.
        string expression = Condition.Variable + Condition.Operator + (Condition.Value ?? Condition.ValueReference);
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
            declarations += $"var {parameter.Key} = (int){parameter.Value.Value}; \n";
        }

        foreach (var parameter in context.Variables)
        {
            declarations += $"var {parameter.Key} = (int){parameter.Value}; \n";
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
        Dictionary<string, object> globals = new Dictionary<string, object>();

        if (context.FunctionParameters != null)
        {
            globals.Add("arg1", 8);
        }

        int arg1 = 0;

        string expression = Condition.Variable + Condition.Operator + (Condition.Value ?? Condition.ValueReference);
        string declarions = GenerateVariablesCode(context, globals);
        string script = GenerateScript(expression, declarions);
        bool conditionResult = CSharpScript.EvaluateAsync<bool>(script).Result;


        while (conditionResult)
        {
            foreach (var node in Nodes)
            {
                node.Execute(context);
            }

            // And the arg1 must be updated to somehow break the loop
            arg1++;

            declarions = GenerateVariablesCode(context, globals);
            script = GenerateScript(expression, declarions);
            conditionResult = CSharpScript.EvaluateAsync<bool>(script).Result;
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
            declarations += $"var {parameter.Key} = (int){parameter.Value.Value}; \n";
        }

        foreach (var variable in context.Variables)
        {
            declarations += $"var {variable.Key} = (int){variable.Value}; \n";
        }

        foreach (var global in globals)
        {
            declarations += $"var {global.Key} = (int){global.Value}; \n";
        }

        return declarations;
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
