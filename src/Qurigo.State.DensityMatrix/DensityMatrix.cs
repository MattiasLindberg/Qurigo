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

    public double Measure()
    {
        var probabilities = np.abs(State).pow(2);
        double[] values = new double[State.size];

        for (int i = 0; i < State.size; i++)
        {
            values[i] = i;
        }
        var measurement_result = np.random.choice(values, null, true, probabilities.flatten());
        return (double)measurement_result;
    }
}
