using Qurigo.Interfaces;

using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using System.Text.RegularExpressions;
using Qurigo.State;

namespace Qurigo.Circuit;

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

    public IState GetState()
    {
        return _state;
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

            case GateNames.CSWAP:
//                Console.WriteLine($"CSWAP: {parameters[0].Index}, {parameters[1].Index}, {parameters[2].Index}");
                _state.ApplyGate(_instructionSet.CSWAP(parameters[0].Index, parameters[1].Index, parameters[2].Index));
                break;

            case GateNames.CX:
                _state.ApplyGate(_instructionSet.CNOT(parameters[0].Index, parameters[1].Index));
                break;

            case GateNames.CRK:
                _state.ApplyGate(_instructionSet.CRk(parameters[0].Index, parameters[1].Index, parameters[2].Index));
                break;

            case GateNames.CCX:
            case GateNames.Toffoli:
                _state.ApplyGate(_instructionSet.Toffoli(parameters[0].Index, parameters[1].Index, parameters[2].Index));
                break;

            case GateNames.RZ:
                _state.ApplyGate(_instructionSet.RZ(parameters[0].Index, parameters[1].Value));
                break;

            case GateNames.SX:
                _state.ApplyGate(_instructionSet.SX(parameters[0].Index));
                break;

            case GateNames.ECR:
                _state.ApplyGate(_instructionSet.ECR(parameters[0].Index, parameters[1].Index));
                break;
        }
    }
}
