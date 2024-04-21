using Numpy;
using Qurigo.Interfaces;
using System.Numerics;
using System.Runtime.Intrinsics.X86;
using System.Security.Principal;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Qurigo.InstructionSet.HTCNOT;

public class IBMEagleR3 : IInstructionSet
{
    public GateNames NativeGates => GateNames.H | GateNames.T | GateNames.CNOT | GateNames.X | GateNames.Y | GateNames.Z;

    public GateNames SupportedGates => NativeGates | GateNames.SqrtX;

    public bool CacheNativeGates { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
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
    }

    public IGate H(int actOnQubit)
    {
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

    public IGate SX(int actOnQubit)
    {
        NDarray sxGate = np.array(new Complex[,]
        {
            { new Complex(1,1), new Complex(1,-1) },
            { new Complex(1,-1), new Complex(1,1) }
        }) / 2;

        NDarray matrix = GenericGateLittleEndian(sxGate, actOnQubit);

        return new Gate(matrix);
    }

    private IGate RZX(double theta, int controlQubit, int actOnQubit)
    {
        //gate rzx(param0) q0, q1 { h q1; cx q0, q1; rz(param0) q1; cx q0, q1; h q1; }

        var step1 = H(actOnQubit);
        var step2 = CNOT(controlQubit, actOnQubit);
        var step3 = RZ(actOnQubit, theta);
        var step4 = CNOT(controlQubit, actOnQubit);
        var step5 = H(actOnQubit);

        NDarray result = np.dot(step1.Base,
            np.dot(step2.Base,
                np.dot(step3.Base,
                    np.dot(step4.Base, step5.Base))));

        return new Gate(result);
    }

    public IGate ECR(int q0, int q1)
    {
        //gate ecr q0, q1 { rzx(pi / 4) q0, q1; x q0; rzx(-pi / 4) q0, q1; }

        var step1 = RZX(Math.PI/4, q0, q1);
        var step2 = X(q0);
        var step3 = RZX(-Math.PI/4, q0, q1);


        var stepA1 = H(q1);
        var stepA2 = CNOT(q0, q1);
        var stepA3 = RZ(q1, Math.PI / 4);
        var stepA4 = CNOT(q0, q1);
        var stepA5 = H(q1);

        NDarray resultA = np.dot(stepA1.Base,
            np.dot(stepA2.Base,
                np.dot(stepA3.Base,
                    np.dot(stepA4.Base, stepA5.Base))));

        var resultB = X(q0).Base;

        var stepC1 = H(q1);
        var stepC2 = CNOT(q0, q1);
        var stepC3 = RZ(q1, -Math.PI / 4);
        var stepC4 = CNOT(q0, q1);
        var stepC5 = H(q1);

        NDarray resultC = np.dot(stepC1.Base,
            np.dot(stepC2.Base,
                np.dot(stepC3.Base,
                    np.dot(stepC4.Base, stepC5.Base))));

        NDarray resultManual = np.dot(resultA, np.dot(resultB, resultC));


        //NDarray result = step1.Base;
        //NDarray result = np.dot(step1.Base, step2.Base);
        //NDarray result = np.dot(step1.Base, np.dot(step2.Base, step3.Base));
        NDarray result = np.dot(step3.Base, np.dot(step2.Base, step1.Base));

        bool equal = np.allclose(result, resultManual);

        return new Gate(result);
    }

    public IGate RZ(int actOnQubit, double theta)
    {
        NDarray rzGate = np.array(new Complex[,]
        {
            {  Complex.Exp(new Complex(0, -theta / 2)), 0 },
            { 0,  Complex.Exp(new Complex(0, theta / 2)) }
        });

        NDarray matrix = GenericGateLittleEndian(rzGate, actOnQubit);

        return new Gate(matrix);
    }

    public NDarray GenericControlledLittleEndian(NDarray gateMatrix, int controlQubit, int actOnQubit)
    {
        int newControl = _qubitCount - controlQubit - 1;
        int newActOn = _qubitCount - actOnQubit - 1;

        return GenericControlledBigEndian(gateMatrix, newControl, newActOn);
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
