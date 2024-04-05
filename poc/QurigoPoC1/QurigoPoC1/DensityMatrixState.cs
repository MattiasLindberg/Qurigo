using Numpy;
using System.Drawing;
using System.Numerics;
using System.Text;

namespace QurigoPoC1;

public class DensityMatrixState
{
    private NDarray gateX = np.array(new Complex[,] { 
        { 0, 1 },
        { 1, 0 } 
    });
    private NDarray gateY = np.array(new Complex[,] {
        { 0, new Complex(0,-1) }, 
        { new Complex(0,1), 0 } 
    });
    private NDarray gateZ = np.array(new Complex[,] { 
        { 1, 0 }, 
        { 0, -1 } 
    });
    private NDarray Identity = np.array(new Complex[,] { 
        { 1, 0 }, 
        { 0, 1 } 
    });
    private NDarray gateH = np.array(new Complex[,] { 
        { 1, 1 }, 
        { 1, -1 } 
    }) / (double)Math.Sqrt(2);
    private NDarray gateCNOT = np.array(new Complex[,] {
        { 0, 1 }, 
        { 1, 0 } 
    });

    public NDarray State { get; set; }

    public new string ToString()
    {
        // This is completely different for a density matrix, 
        // it is not as simple as a vector state.
        // Will not implement this for now.

        return State.ToString();
    }

    public DensityMatrixState(double[] state)
    {
        // Initialize with a pure state.
        // Create density matrix from the pure state.

        var pureState = np.array(state);

        // Simplified becuase I don't need to conjugate the state
        // as I only have double values and not complex.
        State = pureState.kron(pureState.T);
    }

    public DensityMatrixState(Complex[] state)
    {
        // Initialize with a pure state.
        // Create density matrix from the pure state.

        var pureState = np.array(state);

        // Simplified becuase I don't need to conjugate the state
        // as I only have double values and not complex.
        State = pureState.kron(pureState.T);
    }

    public DensityMatrixState(int numberOfQubit)
    {
        State = np.zeros((int)Math.Pow(2, numberOfQubit));
        State[0] = (NDarray)1D;
    }

    public DensityMatrixState PauliX(int actOnQubit)
    {
        int qubitCount = (int)Math.Log(State.size, 2);
        NDarray Px = Identity;
        for (int i = 1; i < actOnQubit - 1; i++)
        {
            Px = np.kron(Px, Identity);
        }
        if (actOnQubit == 1)
        {
            Px = gateX;
        }
        else
        {
            Px = np.kron(Px, gateX);
        }

        for (int i = actOnQubit; i < qubitCount; i++)
        {
            Px = np.kron(Px, Identity);
        }

        State = State.dot(Px);
        return this;
    }
    public DensityMatrixState PauliY(int actOnQubit)
    {
        int qubitCount = (int)Math.Log(State.size, 2);
        NDarray Py = Identity;
        for (int i = 1; i < actOnQubit - 1; i++)
        {
            Py = np.kron(Py, Identity);
        }
        if (actOnQubit == 1)
        {
            Py = gateY;
        }
        else
        {
            Py = np.kron(Py, gateY);
        }

        for (int i = actOnQubit; i < qubitCount; i++)
        {
            Py = np.kron(Py, Identity);
        }

        State = State.dot(Py);
        return this;
    }

    public DensityMatrixState PauliZ(int actOnQubit)
    {
        int qubitCount = (int)Math.Log(State.size, 2);
        NDarray Pz = Identity;
        for (int i = 1; i < actOnQubit - 1; i++)
        {
            Pz = np.kron(Pz, Identity);
        }
        if (actOnQubit == 1)
        {
            Pz = gateZ;
        }
        else
        {
            Pz = np.kron(Pz, gateZ);
        }

        for (int i = actOnQubit; i < qubitCount; i++)
        {
            Pz = np.kron(Pz, Identity);
        }

        State = State.dot(Pz);
        return this;
    }

    public DensityMatrixState Hadamard(int actOnQubit)
    {
        int qubitCount = (int)Math.Log(State.size, 2);
        NDarray Px = Identity;
        for (int i = 1; i < actOnQubit - 1; i++)
        {
            Px = np.kron(Px, Identity);
        }
        if (actOnQubit == 1)
        {
            Px = gateH;
        }
        else
        {
            Px = np.kron(Px, gateH);
        }

        for (int i = actOnQubit; i < qubitCount; i++)
        {
            Px = np.kron(Px, Identity);
        }

        State = State.dot(Px);

        return this;
    }

    public double Measure()
    {
        // TODO: Should collapse State to a single state, as in reality. 
        // Then we can have repeatability in measurements.
        var probabilities = np.abs(State).pow(2);
        double[] values = new double[State.size];

        for (int i = 0; i < State.size; i++)
        {
            values[i] = i;
        }
        var measurement_result = np.random.choice(values, null, true, probabilities.flatten());
        return (double)measurement_result;
    }

    public DensityMatrixState CNOT(int controlQubit, int actOnQubit)
    {
        int shiftsForControl = (int)Math.Log(State.size, 2) - controlQubit;
        int shiftsForAct = (int)Math.Log(State.size, 2) - actOnQubit;

        int qubitCount = (int)Math.Log(State.size, 2);
        NDarray Px = Identity;
        for (int i = 1; i < qubitCount; i++)
        {
            Px = np.kron(Px, Identity);
        }

        for (int i = 0; i < State.size; i++)
        {
            // Is the control qubit set to 1?
            if ((i & (1 << shiftsForControl)) != 0)
            {
                Px[i, i] = (NDarray)0;

                if ((i & (1 << shiftsForAct)) != 0)
                {
                    // 1 => 0
                    Px[i, i-1] = (NDarray)1;
                }
                else
                {
                    // 0 => 1
                    Px[i, i+1] = (NDarray)1;
                }
            }
        }

        State = State.dot(Px);

        return this;
    }
}
