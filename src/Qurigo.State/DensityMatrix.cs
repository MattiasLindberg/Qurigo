using Numpy;
using Numpy.Models;
using Qurigo.Interfaces;
using System.Numerics;
using System.Security.Principal;

namespace Qurigo.State;

public class DensityMatrix : IState
{
    NDarray _state;

    public int Size { get; private set; }

    public NDarray State { get => _state; }

    public IState ApplyGate(IGate gate)
    {
        _state = np.dot(gate.Base, np.dot(_state, gate.Dagger));
        return this;
    }

    public void Initialize(int qubitCount)
    {
        Size = (int)Math.Pow(2, qubitCount);

        // Init vector with size 2^qubitCount x 2^qubitCount
        Shape shape = new Shape(shape: new int[] { Size, Size });
        var state = np.zeros(shape, dtype: np.complex64);

        // Set state to |000...0> (or whatever the qubit count is)
        state[0,0] = np.array(new Complex[] { new Complex(1, 0) });

        _state = state;
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
