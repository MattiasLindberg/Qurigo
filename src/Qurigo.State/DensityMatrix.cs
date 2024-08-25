using Numpy;
using Qurigo.Interfaces;
using System.Numerics;

namespace Qurigo.State;

public class DensityMatrix : IState
{
    NDarray _state;

    public int Size => throw new NotImplementedException();

    public NDarray State => throw new NotImplementedException();

    public IState ApplyGate(IGate gate)
    {
        _state = np.dot(gate.Base, _state, gate.Dagger);
        return this;
    }

    public void Initialize(int qubitCount)
    {
        throw new NotImplementedException();
    }

    Complex[] IState.ToArray()
    {
        throw new NotImplementedException();
    }

    public double Measure()
    {
        throw new NotImplementedException();
    }

    public NDarray PartialTrace(NDarray stateVector, int[] tracedOutQubits, int numQubitsInStateVector)
    {
        throw new NotImplementedException();
    }
}
