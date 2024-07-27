using Numpy;
using Numpy.Models;
using Qurigo.Interfaces;
using System;
using System.Numerics;
using System.Security.Cryptography;
using System.Security.Principal;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Qurigo.InstructionSet.HTCNOT;

public class Gate : IGate
{
    public NDarray Base { get; }
    public NDarray Dagger => np.conj(Base.T);

    public Gate(NDarray baseMatrix)
    {
        Base = baseMatrix;
    }
}

public class HTCNOTXYZ : IInstructionSet
{
    public GateNames NativeGates => GateNames.H | GateNames.T | GateNames.CX | GateNames.X | GateNames.Y | GateNames.Z;

    public GateNames SupportedGates => NativeGates | GateNames.SqrtX;

    public bool CacheNativeGates { get; set; }
    public bool CacheSupportedGates { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    private int _qubitCount;
    private readonly NDarray _identity = np.array(new double[,]
        {
            { 1, 0 },
            { 0, 1 }
        });

    public void Initialize(int qubitCount)
    {
        _qubitCount = qubitCount;
        CacheNativeGates = true;
    }

    private Dictionary<string, IGate> _cachedNativeGates = new Dictionary<string, IGate>();

    public IGate H(int actOnQubit)
    {
        if(CacheNativeGates && _cachedNativeGates.ContainsKey($"H_{actOnQubit}"))
        {
            return _cachedNativeGates[$"H_{actOnQubit}"];
        }

        NDarray hGate = np.array(new double[,]
        {
            { 1, 1 },
            { 1, -1 }
        }) / (double)Math.Sqrt(2);

        NDarray matrix = GenericGateLittleEndian(hGate, actOnQubit);

        return new Gate(matrix);
    }

    public IGate T(int actOnQubit)
    {
        NDarray tGate = np.array(new Complex[,]
        {
            { 1, 0 },
            { 0, new Complex(1/Math.Sqrt(2), 1/Math.Sqrt(2)) }
        });

        NDarray matrix = GenericGateLittleEndian(tGate, actOnQubit);

        return new Gate(matrix);
    }

    public IGate Toffoli(int controlQubit1, int controlQubit2, int actOnQubit)
    {
        var step1 = H(actOnQubit);
        var step2 = CNOT(controlQubit2, actOnQubit);
        var step3 = T(actOnQubit).Dagger;
        var step4 = CNOT(controlQubit1, actOnQubit);
        var step5 = T(actOnQubit);
        var step6 = CNOT(controlQubit2, actOnQubit);
        var step7 = T(actOnQubit).Dagger;
        var step8 = CNOT(controlQubit1, actOnQubit);

        var step10 = T(controlQubit2);
        var step11 = CNOT(controlQubit1, controlQubit2);
        var step12 = T(controlQubit2).Dagger;
        var step13 = T(controlQubit1);
        var step14 = CNOT(controlQubit1, controlQubit2);

        var step20 = T(actOnQubit);
        var step21 = H(actOnQubit);

        NDarray result = np.dot(step1.Base,
            np.dot(step2.Base,
                np.dot(step3,
                    np.dot(step4.Base,
                        np.dot(step5.Base,
                            np.dot(step6.Base,
                                np.dot(step7,
                                    np.dot(step8.Base,
                                        np.dot(step10.Base,
                                            np.dot(step11.Base,
                                                np.dot(step12,
                                                    np.dot(step13.Base,
                                                        np.dot(step14.Base,
                                                            np.dot(step20.Base, step21.Base))))))))))))));

        return new Gate(result);
    }

    public IGate CNOT(int controlQubit, int actOnQubit)
    {
        NDarray xGate = np.array(new double[,]
        {
            { 0, 1 },
            { 1, 0 }
        });

        NDarray matrix = GenericControlledLittleEndian(xGate, controlQubit, actOnQubit);

        return new Gate(matrix);
    }

    public IGate X(int applyOnQubit)
    {
        NDarray xGate = np.array(new double[,]
        {
            { 0, 1 },
            { 1, 0 }
        });

        NDarray matrix = GenericGateLittleEndian(xGate, applyOnQubit);

        return new Gate(matrix);
    }

    public IGate Y(int applyOnQubit)
    {
        NDarray yGate = np.array(new Complex[,]
        {
            { 0, new Complex(0, -1) },
            { new Complex(0, 1), 0 }
        });

        NDarray matrix = GenericGateLittleEndian(yGate, applyOnQubit);

        return new Gate(matrix);
    }

    public IGate Z(int applyOnQubit)
    {
        NDarray zGate = np.array(new double[,]
        {
            { 1, 0 },
            { 0, -1 }
        });

        NDarray matrix = GenericGateLittleEndian(zGate, applyOnQubit);

        return new Gate(matrix);
    }

    public IGate SWAP(int actOnQubit1, int actOnQubit2)
    {
        var step1 = CNOT(actOnQubit1, actOnQubit2);
        var step2 = CNOT(actOnQubit2, actOnQubit1);
        var step3 = CNOT(actOnQubit1, actOnQubit2);
        
        var res = np.dot(step1.Base, np.dot(step2.Base, step3.Base));
        return new Gate(res);
    }

    public IGate CSWAP(int controlQubit, int actOnQubit1, int actOnQubit2)
    {
        NDarray swapQiAndQiplus1 = np.array(new double[,]
        {
            { 1, 0, 0, 0 },
            { 0, 0, 1, 0 },
            { 0, 1, 0, 0 },
            { 0, 0, 0, 1 }
        });

        int newControl = _qubitCount - controlQubit - 1;
        int newActOn = _qubitCount - actOnQubit1 - 1 - 1; // 2nd "-1" is due to applying a 2-qubit swap

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

        while (zeroCount < newControl)
        {
            matrixZeroPart = np.kron(matrixZeroPart, _identity);
            zeroCount++;
        }

        if (newControl == 0)
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

        while (oneCount < newActOn)
        {
            matrixOnePart = np.kron(matrixOnePart, _identity);
            oneCount++;
        }

        if (newActOn == 0)
        {
            // |1><1|
            matrixOnePart = swapQiAndQiplus1;
            oneCount = 2;
        }
        else
        {
            matrixOnePart = np.kron(matrixOnePart, swapQiAndQiplus1);
            oneCount++;
            oneCount++;
        }

        while (oneCount < newControl)
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

        return new Gate(result);





        //NDarray zeroOperator = np.array(new double[,] {
        //    { 1, 0 },
        //    { 0, 0 }
        //});
        //NDarray oneOperator = np.array(new double[,] {
        //    { 0, 0 },
        //    { 0, 1 }
        //});

        //NDarray matrixZeroPart = _identity;
        //int zeroCount = 1;

        //while (zeroCount < newControl)
        //{
        //    matrixZeroPart = np.kron(matrixZeroPart, _identity);
        //    zeroCount++;
        //}

        //if (newControl == 0)
        //{
        //    // |0><0|
        //    matrixZeroPart = zeroOperator;
        //    zeroCount = 1;
        //}
        //else
        //{
        //    matrixZeroPart = np.kron(matrixZeroPart, zeroOperator);
        //    zeroCount++;
        //}

        //while (zeroCount < _qubitCount)
        //{
        //    matrixZeroPart = np.kron(matrixZeroPart, _identity);
        //    zeroCount++;
        //}


        //NDarray matrixOnePart = _identity;
        //int oneCount = 1;


        //while (oneCount < newControl)
        //{
        //    matrixOnePart = np.kron(matrixOnePart, _identity);
        //    oneCount++;
        //}

        //if (newControl == 0)
        //{
        //    // |1><1|
        //    matrixOnePart = oneOperator;
        //    oneCount = 1;
        //}
        //else
        //{
        //    matrixOnePart = np.kron(matrixOnePart, oneOperator);
        //    oneCount++;
        //}

        //while (oneCount < newActOn)
        //{
        //    matrixOnePart = np.kron(matrixOnePart, _identity);
        //    oneCount++;
        //}

        //matrixOnePart = np.kron(matrixOnePart, swapQiAndQiplus1);
        //oneCount++;
        //oneCount++;

        //while (oneCount < _qubitCount)
        //{
        //    matrixOnePart = np.kron(matrixOnePart, _identity);
        //    oneCount++;
        //}

        //for (int i = 0; i < size; i++)
        //{
        //    for (int j = 0; j < size; j++)
        //    {
        //        Console.Write(matrixOnePart[i, j] + "   ");
        //    }
        //    Console.WriteLine();
        //}

        // NDarray result = matrixZeroPart + matrixOnePart;

        //    return new Gate(result);
    }

    private int Swap(int number, int pos1, int pos2)
    {
        int bit1 = (number >> pos1) & 1;
        int bit3 = (number >> pos2) & 1;

        // Check if bits are different
        if (bit1 != bit3)
        {
            // Create a bit mask with bits at position pos1 and pos2 set to 1
            int mask = (1 << pos1) | (1 << pos2);

            // Toggle bits at positions pos1 and pos2
            number ^= mask;
        }

        return number;
    }

    public IGate S(int actOnQubit)
    {
        NDarray zGate = np.array(new Complex[,]
        {
            { 1, 0 },
            { 0, new Complex(0,1) }
        });

        NDarray matrix = GenericGateLittleEndian(zGate, actOnQubit);

        return new Gate(matrix);
    }

    public IGate CRk(int controlQubit, int actOnQubit, int k)
    {
        NDarray rkGate = np.array(new Complex[,]
        {
            { 1, 0 },
            { 0, Complex.Exp(new Complex(0,1) * 2 * Math.PI / Math.Pow(2, k)) }
        });

        NDarray matrix = GenericControlledLittleEndian(rkGate, controlQubit, actOnQubit);

        //NDarray zGate = np.array(new double[,]
        //{
        //    { 1, 0 },
        //    { 0, -1 }
        //});

        //NDarray sGate = np.array(new Complex[,]
        //{
        //    { 1, 0 },
        //    { 0, new Complex(0,1) }
        //});

        //NDarray tGate = np.array(new Complex[,]
        //{
        //    { 1, 0 },
        //    { 0, new Complex(1/Math.Sqrt(2), 1/Math.Sqrt(2)) }
        //});


        //NDarray matrix = null;
        //switch (k)
        //{
        //    case 1:
        //        matrix = GenericControlledLittleEndian(zGate, controlQubit, actOnQubit);
        //        break;
        //    case 2:
        //        matrix = GenericControlledLittleEndian(sGate, controlQubit, actOnQubit);
        //        break;
        //    case 3:
        //        matrix = GenericControlledLittleEndian(tGate, controlQubit, actOnQubit);
        //        break;
        //    default:
        //        throw new NotImplementedException();
        //}

        return new Gate(matrix);
    }


    public IGate SX(int actOnQubit)
    {
        throw new NotImplementedException();
    }

    public IGate ECR(int actOnQubit1, int actOnQubit2)
    {
        throw new NotImplementedException();
    }

    public IGate RZ(int actOnQubit, double theta)
    {
        throw new NotImplementedException();
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

    public NDarray GenericControlledBigEndian(NDarray gateMatrix, int controlQubit, int actOnQubit)
    {
        if(controlQubit < actOnQubit)
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

        while(zeroCount < controlQubit)
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

        while(zeroCount < _qubitCount)
        {
            matrixZeroPart = np.kron(matrixZeroPart, _identity);
            zeroCount++;
        }

        NDarray matrixOnePart = _identity;
        int oneCount = 1;

 
        while(oneCount < controlQubit)
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

        while(oneCount < actOnQubit)
        {
            matrixOnePart = np.kron(matrixOnePart, _identity);
            oneCount++;
        }

        matrixOnePart = np.kron(matrixOnePart, gateMatrix);
        oneCount++;

        while(oneCount < _qubitCount)
        {
            matrixOnePart = np.kron(matrixOnePart, _identity);
            oneCount++;
        }

        NDarray result = matrixZeroPart + matrixOnePart;

        return result;
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

        while(zeroCount < controlQubit)
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

        while(zeroCount < _qubitCount)
        {
            matrixZeroPart = np.kron(matrixZeroPart, _identity);
            zeroCount++;
        } 

        NDarray matrixOnePart = _identity;
        int oneCount = 1;

        while(oneCount < actOnQubit)
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
}
