using Numpy;
using Numpy.Models;
using Qurigo.Interfaces;
using System.Drawing;
using System.Numerics;
using System.Text;

namespace Qurigo.State.VectorState;

public class VectorState : IState
{
    public int Size { get; private set; }

    private NDarray _state;
    public NDarray State { get => _state; }

    public IState ApplyGate(IGate gate)
    {
        _state = np.dot(gate.Base, State);
        return this;
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

    public void Initialize(int qubitCount)
    {
        Size = (int)Math.Pow(2, qubitCount);
        
        // Init vector with size 2^qubitCount
        Shape shape = new Shape(shape: new int[] { Size });
        var state = np.zeros(shape, dtype: np.complex64);

        // Set state to |000...0> (or whatever the qubit count is)
        state[":1"] = np.array(new Complex[] { new Complex(1, 0) });


        // Without this extra np.dot the numbers does not come out as expected
        // when it is accessed. So this is a workaround, to compensate for
        // this behavior.
        var identity = _identity;
        for(int i = 1; i < qubitCount; i++)
        {
            identity = np.kron(identity, _identity);
        }
        _state = np.dot(state, identity);
    }

    private readonly NDarray _identity = np.array(new double[,]
    {
            { 1, 0 },
            { 0, 1 }
    });

    public override string ToString()
    {
        int stringLength = (int) Math.Log2(Size);

        StringBuilder sb = new StringBuilder();
        Complex[] complexArray = _state.GetData<Complex>();

        for (int i = 0; i < Size; i++)
        {
            var temp2 = _state[$"{i}:"];
            // Convert the number to a binary string with leading zeros
            string binaryString = Convert.ToString(i, 2).PadLeft(stringLength, '0');
            string stateValue = string.Format("{0,10:0.000000}", complexArray[i]);
            sb.AppendLine($"|{binaryString}> = {stateValue}");
        }
        return sb.ToString();
    }

    Complex[] IState.ToArray()
    {
        return _state.GetData<Complex>();
    }
}
