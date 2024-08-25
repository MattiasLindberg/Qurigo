using Numpy;
using Qurigo.Circuit;
using Qurigo.InstructionSet;
using Qurigo.Interfaces;
using Qurigo.State;
using System.Numerics;

namespace Qurigo.UnitTests.Density;

[TestClass]
public class DM_HTCNOTXYZOtherGatesTests
{
    private IQuantumCircuit _circuit;

    [TestInitialize]
    public void SetupTests()
    {
        var instructionSet = new HTCNOTXYZ();
        var state = new DensityMatrix();
        _circuit = new QuantumCircuit(state, instructionSet);
    }

    [TestMethod]
    public void SWAP_1()
    {
        _circuit.Initialize(2);
        _circuit.ApplyGate(GateNames.X, new List<Parameter>() { new Parameter() { Index = 0 } });
        _circuit.ApplyGate(GateNames.SWAP, new List<Parameter>() { new Parameter() { Index = 0 }, new Parameter() { Index = 1 } });

        var result = _circuit.GetState().State;
        var expected = np.array(new Complex[] {
            new Complex(0, 0),
            new Complex(0, 0),
            new Complex(1, 0),
            new Complex(0, 0)
        });

        var expectedMatrix = expected.outer(expected.conj());
        Assert.IsTrue(result.array_equal(expectedMatrix));
    }

    [TestMethod]
    public void SWAP_2()
    {
        _circuit.Initialize(2);
        _circuit.ApplyGate(GateNames.X, new List<Parameter>() { new Parameter() { Index = 1 } });
        _circuit.ApplyGate(GateNames.SWAP, new List<Parameter>() { new Parameter() { Index = 0 }, new Parameter() { Index = 1 } });

        var result = _circuit.GetState().State;
        var expected = np.array(new Complex[] {
            new Complex(0, 0),
            new Complex(1, 0),
            new Complex(0, 0),
            new Complex(0, 0)
        });

        var expectedMatrix = expected.outer(expected.conj());
        Assert.IsTrue(result.array_equal(expectedMatrix));
    }

    [TestMethod]
    public void SWAP_3()
    {
        _circuit.Initialize(2);
        _circuit.ApplyGate(GateNames.SWAP, new List<Parameter>() { new Parameter() { Index = 0 }, new Parameter() { Index = 1 } });

        var result = _circuit.GetState().State;
        var expected = np.array(new Complex[] {
            new Complex(1, 0),
            new Complex(0, 0),
            new Complex(0, 0),
            new Complex(0, 0)
        });

        var expectedMatrix = expected.outer(expected.conj());
        Assert.IsTrue(result.array_equal(expectedMatrix));
    }

    [TestMethod]
    public void SWAP_4()
    {
        _circuit.Initialize(2);
        _circuit.ApplyGate(GateNames.X, new List<Parameter>() { new Parameter() { Index = 0 } });
        _circuit.ApplyGate(GateNames.X, new List<Parameter>() { new Parameter() { Index = 1 } });
        _circuit.ApplyGate(GateNames.SWAP, new List<Parameter>() { new Parameter() { Index = 0 }, new Parameter() { Index = 1 } });

        var result = _circuit.GetState().State;
        var expected = np.array(new Complex[] {
            new Complex(0, 0),
            new Complex(0, 0),
            new Complex(0, 0),
            new Complex(1, 0)
        });

        var expectedMatrix = expected.outer(expected.conj());
        Assert.IsTrue(result.array_equal(expectedMatrix));
    }

    [TestMethod]
    public void Toffoli_1()
    {
        _circuit.Initialize(3);
        _circuit.ApplyGate(GateNames.X, new List<Parameter>() { new Parameter() { Index = 0 } });
        _circuit.ApplyGate(GateNames.X, new List<Parameter>() { new Parameter() { Index = 1 } });
        _circuit.ApplyGate(GateNames.Toffoli, new List<Parameter>() { new Parameter() { Index = 0 }, new Parameter() { Index = 1 }, new Parameter() { Index = 2 } });

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

        var expectedMatrix = expected.outer(expected.conj());
        Assert.IsTrue(result.allclose(expectedMatrix));
    }

    [TestMethod]
    public void Toffoli_2()
    {
        _circuit.Initialize(3);
        _circuit.ApplyGate(GateNames.X, new List<Parameter>() { new Parameter() { Index = 0 } });
        _circuit.ApplyGate(GateNames.Toffoli, new List<Parameter>() { new Parameter() { Index = 0 }, new Parameter() { Index = 1 }, new Parameter() { Index = 2 } });

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

        var expectedMatrix = expected.outer(expected.conj());
        Assert.IsTrue(result.allclose(expectedMatrix));
    }

    [TestMethod]
    public void Toffoli_3()
    {
        _circuit.Initialize(3);
        _circuit.ApplyGate(GateNames.X, new List<Parameter>() { new Parameter() { Index = 0 } });
        _circuit.ApplyGate(GateNames.X, new List<Parameter>() { new Parameter() { Index = 1 } });
        _circuit.ApplyGate(GateNames.X, new List<Parameter>() { new Parameter() { Index = 2 } });
        _circuit.ApplyGate(GateNames.Toffoli, new List<Parameter>() { new Parameter() { Index = 0 }, new Parameter() { Index = 1 }, new Parameter() { Index = 2 } });

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

        var expectedMatrix = expected.outer(expected.conj());
        Assert.IsTrue(result.allclose(expectedMatrix));
    }

    [TestMethod]
    public void Toffoli_4()
    {
        _circuit.Initialize(5);
        _circuit.ApplyGate(GateNames.X, new List<Parameter>() { new Parameter() { Index = 0 } });
        _circuit.ApplyGate(GateNames.X, new List<Parameter>() { new Parameter() { Index = 4 } });
        _circuit.ApplyGate(GateNames.Toffoli, new List<Parameter>() { new Parameter() { Index = 4 }, new Parameter() { Index = 0 }, new Parameter() { Index = 2 } });

        var result = _circuit.GetState().State;
        var expected = np.array(new Complex[] {
            new Complex(0, 0),
            new Complex(0, 0),
            new Complex(0, 0),
            new Complex(0, 0),
            new Complex(0, 0),
            new Complex(0, 0),
            new Complex(0, 0),
            new Complex(0, 0),

            new Complex(0, 0),
            new Complex(0, 0),
            new Complex(0, 0),
            new Complex(0, 0),
            new Complex(0, 0),
            new Complex(0, 0),
            new Complex(0, 0),
            new Complex(0, 0),

            new Complex(0, 0),
            new Complex(0, 0),
            new Complex(0, 0),
            new Complex(0, 0),
            new Complex(0, 0),
            new Complex(1, 0),
            new Complex(0, 0),
            new Complex(0, 0),

            new Complex(0, 0),
            new Complex(0, 0),
            new Complex(0, 0),
            new Complex(0, 0),
            new Complex(0, 0),
            new Complex(0, 0),
            new Complex(0, 0),
            new Complex(0, 0)
        });

        var expectedMatrix = expected.outer(expected.conj());
        Assert.IsTrue(result.allclose(expectedMatrix));
    }

    [TestMethod]
    public void CRk_1()
    {
//        _circuit.ExecuteProgram("qreg q[2];\ncrk q[0], q[1], 1;");
        _circuit.Initialize(2);
        _circuit.ApplyGate(GateNames.CRK, new List<Parameter>() { new Parameter() { Index = 0 }, new Parameter() { Index = 1 }, new Parameter() { Index = 1 } });

        var result = _circuit.GetState().State;
        var expected = np.array(new Complex[] {
            new Complex(1, 0), // 00
            new Complex(0, 0), // 01
            new Complex(0, 0), // 10
            new Complex(0, 0), // 11
        });

        var expectedMatrix = expected.outer(expected.conj());
        Assert.IsTrue(result.array_equal(expectedMatrix));
    }

    [TestMethod]
    public void CRk_2()
    {
        // _circuit.ExecuteProgram("qreg q[2];\nx q[0];\ncrk q[0], q[1], 1;");
        _circuit.Initialize(2);
        _circuit.ApplyGate(GateNames.X, new List<Parameter>() { new Parameter() { Index = 0 } });
        _circuit.ApplyGate(GateNames.CRK, new List<Parameter>() { new Parameter() { Index = 0 }, new Parameter() { Index = 1 }, new Parameter() { Index = 1 } });

        var result = _circuit.GetState().State;
        var expected = np.array(new Complex[] {
            new Complex(0, 0), // 00
            new Complex(1, 0), // 01
            new Complex(0, 0), // 10
            new Complex(0, 0), // 11
        });

        var expectedMatrix = expected.outer(expected.conj());
        Assert.IsTrue(result.array_equal(expectedMatrix));
    }

    [TestMethod]
    public void CRk_3()
    {
        // _circuit.ExecuteProgram("qreg q[2];\nx q[0];\nx q[1];\ncrk q[0], q[1], 1;");
        _circuit.Initialize(2);
        _circuit.ApplyGate(GateNames.X, new List<Parameter>() { new Parameter() { Index = 0 } });
        _circuit.ApplyGate(GateNames.X, new List<Parameter>() { new Parameter() { Index = 1 } });
        _circuit.ApplyGate(GateNames.CRK, new List<Parameter>() { new Parameter() { Index = 0 }, new Parameter() { Index = 1 }, new Parameter() { Index = 1 } });

        var result = _circuit.GetState().State;
        var expected = np.array(new Complex[] {
            new Complex(0, 0), // 00
            new Complex(0, 0), // 01
            new Complex(0, 0), // 10
            new Complex(-1, 0), // 11
        });

        var expectedMatrix = expected.outer(expected.conj());
        Assert.IsTrue(result.array_equal(expectedMatrix));
    }
}