using Numpy;
using System.Drawing;
using System.Text;

namespace QurigoPoC1;

public class VectorState
{
    private NDarray X = np.array(new double[,] { { 0, 1 }, { 1, 0 } });
    private NDarray I = np.array(new double[,] { { 1, 0 }, { 0, 1 } });
    private NDarray H = np.array(new double[,] { { 1, 1 }, { 1, -1 } }) / (double)Math.Sqrt(2);

    public NDarray State { get; set; }

    public new string ToString()
    {
        int qubitCount = (int)Math.Log(State.size, 2);
        StringBuilder sb = new StringBuilder();
        double[] state = State.GetData<double>();
        for (int i = 0; i < State.size; i++)
        {
            // Convert the number to a binary string with leading zeros to ensure 3 digits
            string binaryString = Convert.ToString(i, 2).PadLeft(qubitCount, '0');
            string stateValue = string.Format("{0,10:0.000000}", state[i]);
            sb.AppendLine($"|{binaryString}> = {stateValue}");
        }
        return sb.ToString();
        //        return string.Join(", ", .Select(item => , item)));
    }

    public VectorState(double[] state)
    {
        State = np.array(state);
    }

    public VectorState(int numberOfQubit)
    {
        State = np.zeros((int)Math.Pow(2, numberOfQubit));
        State[0] = (NDarray)1D;
    }

    public VectorState PauliX(int actOnQubit)
    {
        int qubitCount = (int)Math.Log(State.size, 2);
        NDarray Px = I;
        for (int i = 1; i < actOnQubit - 1; i++)
        {
            Px = np.kron(Px, I);
        }
        if (actOnQubit == 1)
        {
            Px = X;
        }
        else
        {
            Px = np.kron(Px, X);
        }

        for (int i = actOnQubit; i < qubitCount; i++)
        {
            Px = np.kron(Px, I);
        }

        State = State.dot(Px);
        return this;
    }

    public VectorState Hadamard(int actOnQubit)
    {
        int qubitCount = (int)Math.Log(State.size, 2);
        NDarray Px = I;
        for (int i = 1; i < actOnQubit - 1; i++)
        {
            Px = np.kron(Px, I);
        }
        if (actOnQubit == 1)
        {
            Px = H;
        }
        else
        {
            Px = np.kron(Px, H);
        }

        for (int i = actOnQubit; i < qubitCount; i++)
        {
            Px = np.kron(Px, I);
        }

        State = State.dot(Px);

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
}
