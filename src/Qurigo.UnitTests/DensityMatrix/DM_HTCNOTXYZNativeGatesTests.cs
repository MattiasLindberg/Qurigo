using Numpy;
using Qurigo.Circuit;
using Qurigo.InstructionSet;
using Qurigo.Interfaces;
using Qurigo.State;
using System.Numerics;

namespace Qurigo.UnitTests.Density;

[TestClass]
public class DM_HTCNOTXYZNativeGatesTests
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
    public void H()
    {
        _circuit.Initialize(1);
        _circuit.ApplyGate(GateNames.H, new List<Parameter>() { new Parameter() { Index = 0 } });

        var result = _circuit.GetState().State;
        var expected = np.array(new Complex[] {
            new Complex(1 / Math.Sqrt(2), 0 ),
            new Complex( 1/Math.Sqrt(2), 0 ) }
        );

        var expectedMatrix = expected.outer(expected.conj());
        Assert.IsTrue(result.array_equal(expectedMatrix));
    }

    [TestMethod]
    public void X()
    {
        _circuit.Initialize(1);
        _circuit.ApplyGate(GateNames.X, new List<Parameter>() { new Parameter() { Index = 0 } });

        var result = _circuit.GetState().State;
        var expected = np.array(new Complex[] {
            0,
            1
        });

        var expectedMatrix = expected.outer(expected.conj());
        Assert.IsTrue(result.array_equal(expectedMatrix));
    }

    [TestMethod]
    public void Y()
    {
        _circuit.Initialize(1);
        _circuit.ApplyGate(GateNames.Y, new List<Parameter>() { new Parameter() { Index = 0 } });

        var result = _circuit.GetState().State;
        var expected = np.array(new Complex[] {
            0,
            new Complex(0, 1)
        });

        var expectedMatrix = expected.outer(expected.conj());
        Assert.IsTrue(result.array_equal(expectedMatrix));
    }

    [TestMethod]
    public void Z()
    {
        _circuit.Initialize(1);
        _circuit.ApplyGate(GateNames.Z, new List<Parameter>() { new Parameter() { Index = 0 } });

        var result = _circuit.GetState().State;
        var expected = np.array(new Complex[] {
            1,
            0
        });

        var expectedMatrix = expected.outer(expected.conj());
        Assert.IsTrue(result.array_equal(expectedMatrix));
    }

    [TestMethod]
    public void T_1()
    {
        _circuit.Initialize(1);
        _circuit.ApplyGate(GateNames.T, new List<Parameter>() { new Parameter() { Index = 0 } });

        var result = _circuit.GetState().State;
        var expected = np.array(new Complex[] {
            new Complex(1, 0),
            new Complex(0, 0)
        });

        var expectedMatrix = expected.outer(expected.conj());
        Assert.IsTrue(result.array_equal(expectedMatrix));
    }

    [TestMethod]
    public void T_2()
    {
        _circuit.Initialize(1);
        _circuit.ApplyGate(GateNames.X, new List<Parameter>() { new Parameter() { Index = 0 } });
        _circuit.ApplyGate(GateNames.T, new List<Parameter>() { new Parameter() { Index = 0 } });


        var result = _circuit.GetState().State;
        var expected = np.array(new Complex[] {
            new Complex(0, 0),
            new Complex(1/Math.Sqrt(2), 1/Math.Sqrt(2))
        });

        var expectedMatrix = expected.outer(expected.conj());
        Assert.IsTrue(result.array_equal(expectedMatrix));
    }

    [TestMethod]
    public void CNOT_1()
    {
        _circuit.Initialize(2);
        _circuit.ApplyGate(GateNames.CX, new List<Parameter>() { new Parameter() { Index = 0 }, new Parameter() { Index = 1 } });

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
    public void CNOT_2()
    {
        _circuit.Initialize(2);
        _circuit.ApplyGate(GateNames.X, new List<Parameter>() { new Parameter() { Index = 0 } });
        _circuit.ApplyGate(GateNames.CX, new List<Parameter>() { new Parameter() { Index = 0 }, new Parameter() { Index = 1 } });

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
    public void CNOT_3()
    {
        _circuit.Initialize(2);
        _circuit.ApplyGate(GateNames.X, new List<Parameter>() { new Parameter() { Index = 0 } });
        _circuit.ApplyGate(GateNames.X, new List<Parameter>() { new Parameter() { Index = 1 } });
        _circuit.ApplyGate(GateNames.CX, new List<Parameter>() { new Parameter() { Index = 0 }, new Parameter() { Index = 1 } });

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
    public void CNOT_4()
    {
        _circuit.Initialize(2);
        _circuit.ApplyGate(GateNames.X, new List<Parameter>() { new Parameter() { Index = 0 } });
        _circuit.ApplyGate(GateNames.X, new List<Parameter>() { new Parameter() { Index = 1 } });
        _circuit.ApplyGate(GateNames.CX, new List<Parameter>() { new Parameter() { Index = 1 }, new Parameter() { Index = 0 } });

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
}