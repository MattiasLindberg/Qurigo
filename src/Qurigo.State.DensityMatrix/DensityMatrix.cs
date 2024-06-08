using Numpy;
using Qurigo.Interfaces;
using System.Numerics;

namespace Qurigo.State.DensityMatrix;

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
}
