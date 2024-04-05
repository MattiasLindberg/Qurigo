using Numpy;
using System.Drawing;
using System.Numerics;
using System.Text;

namespace QurigoPoC1;

public class VectorState2
{
    private NDarray gateT = np.array(new Complex[,] {
        { 1, 0 },
        { 0, new Complex(1/Math.Sqrt(2), 1/Math.Sqrt(2)) }
    });

    private NDarray gateX = np.array(new double[,] { 
        { 0, 1 },
        { 1, 0 } 
    });
    private NDarray gateY = np.array(new Complex[,] {
        { 0, new Complex(0,-1) }, 
        { new Complex(0,1), 0 } 
    });
    private NDarray gateZ = np.array(new double[,] { 
        { 1, 0 }, 
        { 0, -1 } 
    });
    private NDarray Identity = np.array(new double[,] { 
        { 1, 0 }, 
        { 0, 1 } 
    });
    private NDarray gateH = np.array(new double[,] { 
        { 1, 1 }, 
        { 1, -1 } 
    }) / (double)Math.Sqrt(2);
    private NDarray gateCNOT = np.array(new double[,] {
        { 0, 1 }, 
        { 1, 0 } 
    });

    public NDarray State { get; set; }

    public new string ToString()
    {
        return State.ToString();
    }

    public VectorState2(double[] state)
    {
        State = np.array(state);
    }

    public VectorState2(Complex[] state)
    {
        State = np.array(state);
    }

    public VectorState2(int numberOfQubit)
    {
        State = np.zeros((int)Math.Pow(2, numberOfQubit));
        State[0] = (NDarray)1D;
    }

    public VectorState2 PauliX(int actOnQubit)
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
    public VectorState2 PauliY(int actOnQubit)
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

    public VectorState2 PauliZ(int actOnQubit)
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

    public VectorState2 Hadamard(int actOnQubit)
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

    public VectorState2 CNOT(int controlQubit, int actOnQubit)
    {
        int qubitCount = (int)Math.Log(State.size, 2);
        NDarray zeroOperator = np.array(new double[,] {
            { 1, 0 },
            { 0, 0 }
        });
        NDarray oneOperator = np.array(new double[,] {
            { 0, 0 },
            { 0, 1 }
        });

        // I_(i-1) x |0><0| x I_(j-i-1) x I x I_(n-j)
        NDarray Mzero = Identity;
        // I_(i-1)
        for (int i = 1; i < controlQubit - 1; i++)
        {
            Mzero = np.kron(Mzero, Identity);
        }

        if (controlQubit == 1)
        {
            // |0><0|
            Mzero = zeroOperator;
        }
        else
        {
            Mzero = np.kron(Mzero, zeroOperator);
        }

        // I_(j-i-1) x I x I_(n-j)
        for (int i = controlQubit; i < qubitCount; i++)
        {
            Mzero = np.kron(Mzero, Identity);
        }


        // I_(i - 1) x | 1 >< 1 | x I_(j - i - 1) x X x I_(n-j)
        NDarray Mone = Identity;
        // I_(j-i-1)
        for (int i = 1; i < controlQubit - 1; i++)
        {
            Mone = np.kron(Mone, Identity);
        }

        if (controlQubit == 1)
        {
            // |1><1|
            Mone = oneOperator;
        }
        else
        {
            Mone = np.kron(Mone, oneOperator);
        }

        // I_(j-i-1)
        for (int i = controlQubit + 1; i < actOnQubit; i++)
        {
            Mone = np.kron(Mone, Identity);
        }

        // X gate
        Mone = np.kron(Mone, gateX);

        // I_(n-j)
        for (int i = actOnQubit; i < qubitCount; i++)
        {
            Mone = np.kron(Mone, Identity);
        }

        NDarray M = Mzero + Mone;

        State = State.dot(M);

        return this;
    }

    public VectorState2 T(int actOnQubit)
    {
        int qubitCount = (int)Math.Log(State.size, 2);
        NDarray Px = Identity;
        for (int i = 1; i < actOnQubit - 1; i++)
        {
            Px = np.kron(Px, Identity);
        }
        if (actOnQubit == 1)
        {
            Px = gateT;
        }
        else
        {
            Px = np.kron(Px, gateT);
        }

        for (int i = actOnQubit; i < qubitCount; i++)
        {
            Px = np.kron(Px, Identity);
        }

        State = State.dot(Px);

        return this;
    }

    public VectorState2 GenericX(int actOnQubit)
    {
        return this.GenericGate(gateX, actOnQubit);
    }

    public VectorState2 GenericCNOT(int controlQubit, int actOnQubit)
    {
        return this.GenericControlled(gateX, controlQubit, actOnQubit);
    }


    private VectorState2 GenericGate(NDarray gateMatrix, int actOnQubit)
    {
        int qubitCount = (int)Math.Log(State.size, 2);

        NDarray result = Identity;
        for (int i = 1; i < actOnQubit - 1; i++)
        {
            result = np.kron(result, Identity);
        }
        if (actOnQubit == 1)
        {
            result = gateMatrix;
        }
        else
        {
            result = np.kron(result, gateMatrix);
        }

        for (int i = actOnQubit; i < qubitCount; i++)
        {
            result = np.kron(result, Identity);
        }

        State = State.dot(result);

        return this;
    }

    public VectorState2 GenericControlled(NDarray gateMatrix, int controlQubit, int actOnQubit)
    {
        int qubitCount = (int)Math.Log(State.size, 2);
        NDarray zeroOperator = np.array(new double[,] {
            { 1, 0 },
            { 0, 0 }
        });
        NDarray oneOperator = np.array(new double[,] {
            { 0, 0 },
            { 0, 1 }
        });

        // I_(i-1) x |0><0| x I_(j-i-1) x I x I_(n-j)
        NDarray matrixZeroPart = Identity;
        // I_(i-1)
        for (int i = 1; i < controlQubit - 1; i++)
        {
            matrixZeroPart = np.kron(matrixZeroPart, Identity);
        }

        if (controlQubit == 1)
        {
            // |0><0|
            matrixZeroPart = zeroOperator;
        }
        else
        {
            matrixZeroPart = np.kron(matrixZeroPart, zeroOperator);
        }

        // I_(j-i-1) x I x I_(n-j)
        for (int i = controlQubit; i < qubitCount; i++)
        {
            matrixZeroPart = np.kron(matrixZeroPart, Identity);
        }


        // I_(i - 1) x | 1 >< 1 | x I_(j - i - 1) x X x I_(n-j)
        NDarray matrixOnePart = Identity;
        // I_(j-i-1)
        for (int i = 1; i < controlQubit - 1; i++)
        {
            matrixOnePart = np.kron(matrixOnePart, Identity);
        }

        if (controlQubit == 1)
        {
            // |1><1|
            matrixOnePart = oneOperator;
        }
        else
        {
            matrixOnePart = np.kron(matrixOnePart, oneOperator);
        }

        // I_(j-i-1)
        for (int i = controlQubit + 1; i < actOnQubit; i++)
        {
            matrixOnePart = np.kron(matrixOnePart, Identity);
        }

        // X gate
        matrixOnePart = np.kron(matrixOnePart, gateMatrix);

        // I_(n-j)
        for (int i = actOnQubit; i < qubitCount; i++)
        {
            matrixOnePart = np.kron(matrixOnePart, Identity);
        }

        NDarray M = matrixZeroPart + matrixOnePart;

        State = State.dot(M);

        return this;
    }
}
