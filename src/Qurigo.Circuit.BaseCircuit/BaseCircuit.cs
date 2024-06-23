using Qurigo.Interfaces;

using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using System.Text.RegularExpressions;
using Qurigo.State.VectorState;

namespace Qurigo.Circuit.BaseCircuit;

public interface IQuantumCircuit
{
    void Initialize(int qubitCount);
    void ApplyGate(GateNames gateType, IList<Parameter> parameters);
}
public class QuantumCircuit : IQuantumCircuit
{
    private IState _state;
    private readonly IInstructionSet _instructionSet;

    public QuantumCircuit(IState state, IInstructionSet instructionSet)
    {
        _state = state;
        _instructionSet = instructionSet;
    }
    public void Initialize(int qubitCount)
    {
        _state.Initialize(qubitCount);
        _instructionSet.Initialize(qubitCount);
    }

    public void ApplyGate(GateNames gateType, IList<Parameter> parameters)
    {
        switch (gateType)
        {
            case GateNames.X:
                _state.ApplyGate(_instructionSet.X(parameters[0].Index));
                break;

            case GateNames.Y:
                _state.ApplyGate(_instructionSet.Y(parameters[0].Index));
                break;

            case GateNames.Z:
                _state.ApplyGate(_instructionSet.Z(parameters[0].Index));
                break;

            case GateNames.H:
                _state.ApplyGate(_instructionSet.H(parameters[0].Index));
                break;

            case GateNames.T:
                _state.ApplyGate(_instructionSet.T(parameters[0].Index));
                break;

            case GateNames.SWAP:
                _state.ApplyGate(_instructionSet.SWAP(parameters[0].Index, parameters[1].Index));
                break;

            case GateNames.CNOT:
                _state.ApplyGate(_instructionSet.CNOT(parameters[0].Index, parameters[1].Index));
                break;

            case GateNames.CRk:
                _state.ApplyGate(_instructionSet.CRk(parameters[0].Index, parameters[1].Index, parameters[2].Index));
                break;
        }
    }
}



public class BaseCircuit : ICircuit
{
    private readonly IInstructionSet _instructionSet;
    private IState _state;
    private Dictionary<string, string[]> _functions = new();
    private Dictionary<string, string[]> _extendedFunctions = new();

    public BaseCircuit(IInstructionSet instructionSet, IState state)
    {
        _instructionSet = instructionSet;
        _state = state;
    }

    public IState GetState() => _state;

    private int PreProcessProgram(string[] lines)
    {
        for(int loop = 0; loop < lines.Length; loop++)
        {
            string[] tokens = lines[loop].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            switch(tokens[0].Trim())
            {
                case "OPENQASM":
                    // Ignore this directive, it is just a header.
                    break;
                case "include":
                    // Ignore these includes, all gates needs to be defined in the instruction set.
                    break;
                case "gate":
                    // Ignore these custom gates, all gates needs to be defined in the instruction set.
                    break;
                case "creg":
                    // Ignore these classical registers, we are only interested in quantum registers.
                    // This is a limitation of the current implementation which may be addressed later.
                    break;
                case "qreg":
                    int qubitSize = int.Parse(tokens[1].Substring(2, 1));
                    Initialize(qubitSize);
                    break;
                case "def":
                    IList<string> subRoutine = new List<string>();
                    string functionName = tokens[1].Substring(0, tokens[1].IndexOf('('));

                    loop++; // Do not add the def line to the subroutine
                    while (lines[loop] != "}\r" && loop < lines.Length)
                    {
                        subRoutine.Add(lines[loop].Trim());
                        loop++;
                    }
                    _functions.Add(functionName, subRoutine.ToArray());
                    break;
                case "":
                    // Ignore empty lines
                    break;
                default:
                    // When an unknown preprocessor directive is found, the program has started.
                    return loop;
                    break;
            }
        }

        return -1;
    }

    public void ExecuteProgram(string program)
    {
        string[] lines = program.Split(new[] { '\n', ';' }, StringSplitOptions.RemoveEmptyEntries);

        int startExecutionOnRow = PreProcessProgram(lines);

        ExecuteInstructions(lines, startExecutionOnRow);
    }

    private async Task ExecuteInstructions(string[] lines, int startExecutionOnRow)
    {
        for (int loop = startExecutionOnRow; loop < lines.Length; loop++)
        {
            // Split each line into tokens
            string[] tokens = lines[loop].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            int actOnQubit1;
            int actOnQubit2;
            int controlQubit1;
            int controlQubit2;

            if( tokens.Length == 0 )
            {
                continue;
            }

            int position = tokens[0].IndexOf('(');
            string gate = tokens[0].Trim();
            if (position > 0)
            {
                gate = tokens[0].Substring(0, position);
            }
            switch (gate)
            {
                case "x":
                    actOnQubit1 = int.Parse(tokens[1].Substring(2, 1));
                    _state.ApplyGate(_instructionSet.X(actOnQubit1));
                    break;

                case "y":
                    actOnQubit1 = int.Parse(tokens[1].Substring(2, 1));
                    _state.ApplyGate(_instructionSet.Y(actOnQubit1));
                    break;

                case "z":
                    actOnQubit1 = int.Parse(tokens[1].Substring(2, 1));
                    _state.ApplyGate(_instructionSet.Z(actOnQubit1));
                    break;

                case "h":
                    actOnQubit1 = int.Parse(tokens[1].Substring(2, 1));
                    _state.ApplyGate(_instructionSet.H(actOnQubit1));
                    break;

                case "t":
                    actOnQubit1 = int.Parse(tokens[1].Substring(2, 1));
                    _state.ApplyGate(_instructionSet.T(actOnQubit1));
                    break;

                case "swap":
                    actOnQubit1 = int.Parse(tokens[1].Substring(2, 1));
                    actOnQubit2 = int.Parse(tokens[2].Substring(2, 1));
                    _state.ApplyGate(_instructionSet.SWAP(actOnQubit1, actOnQubit2));
                    break;

                case "cx":
                    controlQubit1 = int.Parse(tokens[1].Substring(2, 1));
                    actOnQubit1 = int.Parse(tokens[2].Substring(2, 1));
                    _state.ApplyGate(_instructionSet.CNOT(controlQubit1, actOnQubit1));
                    break;

                case "crk":
                    controlQubit1 = int.Parse(tokens[1].Substring(2, 1));
                    actOnQubit1 = int.Parse(tokens[2].Substring(2, 1));
                    int k = int.Parse(tokens[3].Substring(0, 1));
                    _state.ApplyGate(_instructionSet.CRk(controlQubit1, actOnQubit1, k));
                    break;

                case "ccx":
                    controlQubit1 = int.Parse(tokens[1].Substring(2, 1));
                    controlQubit2 = int.Parse(tokens[2].Substring(2, 1));
                    actOnQubit1 = int.Parse(tokens[3].Substring(2, 1));
                    _state.ApplyGate(_instructionSet.Toffoli(controlQubit1, controlQubit2, actOnQubit1));
                    break;

                case "sx":
                    actOnQubit1 = int.Parse(tokens[1].Substring(2, 1));
                    _state.ApplyGate(_instructionSet.SX(actOnQubit1));
                    break;

                case "ecr":
                    actOnQubit1 = int.Parse(tokens[1].Substring(2, 1));
                    actOnQubit2 = int.Parse(tokens[2].Substring(2, 1));
                    _state.ApplyGate(_instructionSet.ECR(actOnQubit1, actOnQubit2));
                    break;

                case "rz":
                    int endParenthesis = tokens[0].IndexOf(')');
                    string thetaString = tokens[0].Substring(3, endParenthesis - 3);
                    double theta = 0;
                    switch (thetaString)
                    {
                        case "pi":
                            theta = Math.PI;
                            break;
                        case "-pi":
                            theta = -Math.PI;
                            break;
                        case "pi/2":
                            theta = Math.PI / 2;
                            break;
                        case "-pi/2":
                            theta = -Math.PI / 2;
                            break;
                        case "pi/3":
                            theta = Math.PI / 3;
                            break;
                        case "-pi/3":
                            theta = -Math.PI / 3;
                            break;
                        case "pi/4":
                            theta = Math.PI / 4;
                            break;
                        case "-pi/4":
                            theta = -Math.PI / 4;
                            break;
                        case "3*pi/4":
                            theta = 3 * Math.PI / 4;
                            break;
                        case "-3*pi/4":
                            theta = -3 * Math.PI / 4;
                            break;
                        default:
                            thetaString = thetaString.Replace(".", ",");
                            theta = double.Parse(thetaString);
                            break;
                    }

                    actOnQubit1 = int.Parse(tokens[1].Substring(2, 1));
                    _state.ApplyGate(_instructionSet.RZ(actOnQubit1, theta));
                    break;

                case "if":
                    // Get condition for the if statement and evaluate it.
                    string pattern = @"\(([^)]*)\)";
                    Match match = Regex.Match(lines[loop], pattern);
                    string expression = match.Groups[1].Value;
                    bool conditionResult = await CSharpScript.EvaluateAsync<bool>(expression, globals: new Globals { arg1 = 8 });

                    // Get the block of code to execute if the condition is true
                    // If the condition is false, we need to skip these lines anyway
                    loop++;
                    List<string> block = new();
                    while (lines[loop].Trim() != "}" && loop < lines.Length)
                    {
                        block.Add(lines[loop].Trim());
                        loop++;
                    }

                    // Execute the block of code if the condition is true
                    if (conditionResult)
                    {
                        await ExecuteInstructions(block.ToArray(), 0);
                    }

                    break;

                case "#":
                    // Ignore comments
                    break;

                case "measure":
                    // Ignore measurement for now
                    break;

                case "barrier":
                    // Ignore barrier for now
                    break;

                case "\r":
                case "\r\n":
                case "":
                    break;

                default:
                    if (_functions.ContainsKey(gate))
                    {
                        // Execute the function given by the string[] in the dictionary
                        ExecuteInstructions(_functions[gate], 0);
                        break;
                    }

                    if (_extendedFunctions.ContainsKey(gate))
                    {
                        // Execute the function given by the string[] in the dictionary
                        ExecuteInstructions(_extendedFunctions[gate], 0);
                        break;
                    }

                    throw new Exception("Unknown gate: " + tokens[0]);
            }
        }
    }

    private void Initialize(int qubitSize)
    {
        _state.Initialize(qubitSize);
        _instructionSet.Initialize(qubitSize);
    }
}

public class Globals
{
    public int arg1;
}
