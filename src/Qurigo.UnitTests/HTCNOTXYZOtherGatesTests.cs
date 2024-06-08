using Numpy;
using Qurigo.Circuit.BaseCircuit;
using Qurigo.InstructionSet.HTCNOT;
using Qurigo.Interfaces;
using Qurigo.State.VectorState;
using System.Numerics;

namespace Qurigo.UnitTests;

[TestClass]
public class HTCNOTXYZOtherGatesTests
{
    private ICircuit _circuit;

    [TestInitialize]
    public void SetupTests()
    {
        var instructionSet = new HTCNOTXYZ();
        var state = new VectorState();
        _circuit = new BaseCircuit(instructionSet, state);
    }

    [TestMethod]
    public void SWAP_1()
    {
        _circuit.ExecuteProgram("qreg q[2];\nx q[0];\nswap q[0], q[1];");

        var result = _circuit.GetState().State;
        var expected = np.array(new Complex[] {
            new Complex(0, 0),
            new Complex(0, 0),
            new Complex(1, 0),
            new Complex(0, 0)
        });

        Assert.IsTrue(np.array_equal(result, expected));
    }

    [TestMethod]
    public void SWAP_2()
    {
        _circuit.ExecuteProgram("qreg q[2];\nx q[1];\nswap q[0], q[1];");

        var result = _circuit.GetState().State;
        var expected = np.array(new Complex[] {
            new Complex(0, 0),
            new Complex(1, 0),
            new Complex(0, 0),
            new Complex(0, 0)
        });

        Assert.IsTrue(np.array_equal(result, expected));
    }

    [TestMethod]
    public void SWAP_3()
    {
        _circuit.ExecuteProgram("qreg q[2];\nswap q[0], q[1];");

        var result = _circuit.GetState().State;
        var expected = np.array(new Complex[] {
            new Complex(1, 0),
            new Complex(0, 0),
            new Complex(0, 0),
            new Complex(0, 0)
        });

        Assert.IsTrue(np.array_equal(result, expected));
    }

    [TestMethod]
    public void SWAP_4()
    {
        _circuit.ExecuteProgram("qreg q[2];\nx q[0];\nx q[1];\nswap q[0], q[1];");

        var result = _circuit.GetState().State;
        var expected = np.array(new Complex[] {
            new Complex(0, 0),
            new Complex(0, 0),
            new Complex(0, 0),
            new Complex(1, 0)
        });

        Assert.IsTrue(np.array_equal(result, expected));
    }

    [TestMethod]
    public void Toffoli_1()
    {
        _circuit.ExecuteProgram("qreg q[3];\nx q[0];\nx q[1];\nccx q[0], q[1], q[2];");

        var result = _circuit.GetState().State;
        var expected = np.array(new Complex[] {
            new Complex(0, 0),
            new Complex(0, 0),
            new Complex(0, 0),
            new Complex(0, 0),

            new Complex(0, 0),
            new Complex(0, 0),
            new Complex(0, 0),
            new Complex(1, 0)
        });

        Assert.IsTrue(np.allclose(result, expected));
    }

    [TestMethod]
    public void Toffoli_2()
    {
        _circuit.ExecuteProgram("qreg q[3];\nx q[0];\nccx q[0], q[1], q[2];");

        var result = _circuit.GetState().State;
        var expected = np.array(new Complex[] {
            new Complex(0, 0),
            new Complex(1, 0),
            new Complex(0, 0),
            new Complex(0, 0),

            new Complex(0, 0),
            new Complex(0, 0),
            new Complex(0, 0),
            new Complex(0, 0)
        });

        Assert.IsTrue(np.allclose(result, expected));
    }

    [TestMethod]
    public void Toffoli_3()
    {
        _circuit.ExecuteProgram("qreg q[3];\nx q[0];\nx q[1];\nx q[2];\nccx q[0], q[1], q[2];");

        var result = _circuit.GetState().State;
        var expected = np.array(new Complex[] {
            new Complex(0, 0),
            new Complex(0, 0),
            new Complex(0, 0),
            new Complex(1, 0),

            new Complex(0, 0),
            new Complex(0, 0),
            new Complex(0, 0),
            new Complex(0, 0)
        });

        Assert.IsTrue(np.allclose(result, expected));
    }

    [TestMethod]
    public void CRk_1()
    {
        _circuit.ExecuteProgram("qreg q[2];\ncrk q[0], q[1], 1;");

        var result = _circuit.GetState().State;
        var expected = np.array(new Complex[] {
            new Complex(1, 0),
            new Complex(0, 0),
            new Complex(0, 0),
            new Complex(0, 0),
        });

        Assert.IsTrue(np.allclose(result, expected));
    }

    [TestMethod]
    public void CRk_2()
    {
        _circuit.ExecuteProgram("qreg q[2];\nx q[0];\ncrk q[0], q[1], 1;");

        var result = _circuit.GetState().State;
        var expected = np.array(new Complex[] {
            new Complex(0, 0),
            new Complex(1, 0),
            new Complex(0, 0),
            new Complex(0, 0),
        });

        Assert.IsTrue(np.allclose(result, expected));
    }

    [TestMethod]
    public void CRk_3()
    {
        _circuit.ExecuteProgram("qreg q[2];\nx q[0];\nx q[1];\ncrk q[0], q[1], 1;");

        var result = _circuit.GetState().State;
        var expected = np.array(new Complex[] {
            new Complex(0, 0),
            new Complex(0, 0),
            new Complex(0, 0),
            new Complex(0, 1),
        });

        Assert.IsTrue(np.allclose(result, expected));
    }
}