namespace Qurigo.Interfaces;

public interface IQuantumCircuit
{
    void Initialize(int qubitCount);
    void ApplyGate(GateNames gateType, IList<Parameter> parameters);
    IState GetState();
}
