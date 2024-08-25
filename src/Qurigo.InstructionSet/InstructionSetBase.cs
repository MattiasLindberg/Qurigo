using Numpy;

namespace Qurigo.InstructionSet;

public class InstructionSetBase
{
    protected int _qubitCount;
    protected readonly NDarray _identity = np.array(new double[,]
        {
            { 1, 0 },
            { 0, 1 }
        });


    public NDarray GenericControlledBigEndian(NDarray gateMatrix, int controlQubit, int actOnQubit)
    {
        if (controlQubit < actOnQubit)
        {
            return ImplGenericControlledGateBigEndian_ControlBeforeAct(gateMatrix, controlQubit, actOnQubit);
        }
        else
        {
            return ImplGenericControlledGateBigEndian_ActBeforeControl(gateMatrix, controlQubit, actOnQubit);
        }
    }

    public NDarray GenericControlledBigEndian(NDarray gateMatrix, int controlQubit, int actOnQubit1, int actOnQubit2)
    {
        // Control qubits will always be before the act on qubits in Shor's algorithm, so keeping it simple.
        return null; // ImplGenericControlledGateBigEndian_ControlBeforeAct(gateMatrix, controlQubit, actOnQubit1, actOnQubit2);
    }

    public NDarray GenericControlledLittleEndian(NDarray gateMatrix, int controlQubit, int actOnQubit)
    {
        int newControl = _qubitCount - controlQubit - 1;
        int newActOn = _qubitCount - actOnQubit - 1;

        return GenericControlledBigEndian(gateMatrix, newControl, newActOn);
    }

    public NDarray GenericControlledLittleEndian(NDarray gateMatrix, int controlQubit, int actOnQubit1, int actOnQubit2)
    {
        int newControl = _qubitCount - controlQubit - 1;
        int newActOn1 = _qubitCount - actOnQubit1 - 1;
        int newActOn2 = _qubitCount - actOnQubit2 - 1;

        return GenericControlledBigEndian(gateMatrix, newControl, newActOn1, actOnQubit2);
    }

    public NDarray GenericGateBigEndian(NDarray gateMatrix, int actOnQubit)
    {
        NDarray gate = _identity;
        for (int i = 1; i < actOnQubit - 1; i++)
        {
            gate = np.kron(gate, _identity);
        }
        if (actOnQubit == 0)
        {
            gate = gateMatrix;
        }
        else
        {
            gate = np.kron(gate, gateMatrix);
        }

        for (int i = actOnQubit + 1; i < _qubitCount; i++)
        {
            gate = np.kron(gate, _identity);
        }

        return gate;
    }

    public NDarray GenericGateLittleEndian(NDarray gateMatrix, int actOnQubit)
    {
        NDarray gate = _identity;
        for (int i = _qubitCount - 1; i > actOnQubit + 1; i--)
        {
            gate = np.kron(gate, _identity);
        }
        if (actOnQubit == _qubitCount - 1)
        {
            gate = gateMatrix;
        }
        else
        {
            gate = np.kron(gate, gateMatrix);
        }

        for (int i = actOnQubit; i > 0; i--)
        {
            gate = np.kron(gate, _identity);
        }

        return gate;
    }

    private NDarray ImplGenericControlledGateBigEndian_ActBeforeControl(NDarray gateMatrix, int controlQubit, int actOnQubit)
    {
        NDarray zeroOperator = np.array(new double[,] {
            { 1, 0 },
            { 0, 0 }
        });
        NDarray oneOperator = np.array(new double[,] {
            { 0, 0 },
            { 0, 1 }
        });

        NDarray matrixZeroPart = _identity;
        int zeroCount = 1;

        while (zeroCount < controlQubit)
        {
            matrixZeroPart = np.kron(matrixZeroPart, _identity);
            zeroCount++;
        }

        if (controlQubit == 0)
        {
            // |0><0|
            matrixZeroPart = zeroOperator;
            zeroCount = 1;
        }
        else
        {
            matrixZeroPart = np.kron(matrixZeroPart, zeroOperator);
            zeroCount++;
        }

        while (zeroCount < _qubitCount)
        {
            matrixZeroPart = np.kron(matrixZeroPart, _identity);
            zeroCount++;
        }

        NDarray matrixOnePart = _identity;
        int oneCount = 1;

        while (oneCount < actOnQubit)
        {
            matrixOnePart = np.kron(matrixOnePart, _identity);
            oneCount++;
        }

        if (actOnQubit == 0)
        {
            // |1><1|
            matrixOnePart = gateMatrix;
            oneCount = 1;
        }
        else
        {
            matrixOnePart = np.kron(matrixOnePart, gateMatrix);
            oneCount++;
        }

        while (oneCount < controlQubit)
        {
            matrixOnePart = np.kron(matrixOnePart, _identity);
            oneCount++;
        }

        matrixOnePart = np.kron(matrixOnePart, oneOperator);
        oneCount++;

        while (oneCount < _qubitCount)
        {
            matrixOnePart = np.kron(matrixOnePart, _identity);
            oneCount++;
        }

        NDarray result = matrixZeroPart + matrixOnePart;

        return result;
    }

    private NDarray ImplGenericControlledGateBigEndian_ControlBeforeAct(NDarray gateMatrix, int controlQubit, int actOnQubit)
    {
        NDarray zeroOperator = np.array(new double[,] {
            { 1, 0 },
            { 0, 0 }
        });
        NDarray oneOperator = np.array(new double[,] {
            { 0, 0 },
            { 0, 1 }
        });

        NDarray matrixZeroPart = _identity;
        int zeroCount = 1;

        while (zeroCount < controlQubit)
        {
            matrixZeroPart = np.kron(matrixZeroPart, _identity);
            zeroCount++;
        }

        if (controlQubit == 0)
        {
            // |0><0|
            matrixZeroPart = zeroOperator;
            zeroCount = 1;
        }
        else
        {
            matrixZeroPart = np.kron(matrixZeroPart, zeroOperator);
            zeroCount++;
        }

        while (zeroCount < _qubitCount)
        {
            matrixZeroPart = np.kron(matrixZeroPart, _identity);
            zeroCount++;
        }

        NDarray matrixOnePart = _identity;
        int oneCount = 1;


        while (oneCount < controlQubit)
        {
            matrixOnePart = np.kron(matrixOnePart, _identity);
            oneCount++;
        }

        if (controlQubit == 0)
        {
            // |1><1|
            matrixOnePart = oneOperator;
            oneCount = 1;
        }
        else
        {
            matrixOnePart = np.kron(matrixOnePart, oneOperator);
            oneCount++;
        }

        while (oneCount < actOnQubit)
        {
            matrixOnePart = np.kron(matrixOnePart, _identity);
            oneCount++;
        }

        matrixOnePart = np.kron(matrixOnePart, gateMatrix);
        oneCount++;

        while (oneCount < _qubitCount)
        {
            matrixOnePart = np.kron(matrixOnePart, _identity);
            oneCount++;
        }

        NDarray result = matrixZeroPart + matrixOnePart;

        return result;
    }
}