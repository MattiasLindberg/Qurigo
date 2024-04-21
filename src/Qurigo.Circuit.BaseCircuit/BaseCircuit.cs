using Qurigo.Interfaces;
using System.Diagnostics;

namespace Qurigo.Circuit.BaseCircuit;

public class BaseCircuit : ICircuit
{
    private readonly IInstructionSet _instructionSet;
    private IState _state;

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

            switch(tokens[0])
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
        string[] lines = program.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

        int executionState = PreProcessProgram(lines);

        for(int loop = executionState; loop < lines.Length; loop++)
        {
            // Split each line into tokens
            string[] tokens = lines[loop].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            int actOnQubit1 = -1;
            int actOnQubit2 = -1;
            int controlQubit1 = -1;
            int controlQubit2 = -1;

            int position = tokens[0].IndexOf('(');
            string gate = tokens[0];
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

                case "measure":
                    // Ignore measurement for now
                    break;

                case "barrier":
                    // Ignore barrier for now
                    break;

                default:
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
