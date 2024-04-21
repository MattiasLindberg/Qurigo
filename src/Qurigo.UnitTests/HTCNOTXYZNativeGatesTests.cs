using Numpy;
using Qurigo.Circuit.BaseCircuit;
using Qurigo.InstructionSet.HTCNOT;
using Qurigo.Interfaces;
using Qurigo.State.VectorState;
using System.Numerics;

namespace Qurigo.UnitTests;

[TestClass]
public class HTCNOTXYZNativeGatesTests
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
    public void H()
    {

        _circuit.ExecuteProgram("qreg q[1];\nh q[0];");

        var result = _circuit.GetState().State;
        var expected = np.array(new Complex[] { 
            new Complex(1 / Math.Sqrt(2), 0 ),  
            new Complex( 1/Math.Sqrt(2), 0 ) }
        );

        Assert.IsTrue(np.array_equal(result, expected));
    }

    [TestMethod]
    public void X()
    {
        _circuit.ExecuteProgram("qreg q[1];\nx q[0];");

        var result = _circuit.GetState().State;
        var expected = np.array(new Complex[] {
            0,
            1
        });

        Assert.IsTrue(np.array_equal(result, expected));
    }

    [TestMethod]
    public void Y()
    {
        _circuit.ExecuteProgram("qreg q[1];\ny q[0];");

        var result = _circuit.GetState().State;
        var expected = np.array(new Complex[] {
            0,
            new Complex(0, 1)
        });

        Assert.IsTrue(np.array_equal(result, expected));
    }

    [TestMethod]
    public void Z()
    {
        _circuit.ExecuteProgram("qreg q[1];\nz q[0];");

        var result = _circuit.GetState().State;
        var expected = np.array(new Complex[] {
            1,
            0
        });

        Assert.IsTrue(np.array_equal(result, expected));
    }

    [TestMethod]
    public void T_1()
    {
        _circuit.ExecuteProgram("qreg q[1];\nt q[0];");

        var result = _circuit.GetState().State;
        var expected = np.array(new Complex[] {
            new Complex(1, 0),
            new Complex(0, 0)
        });

        Assert.IsTrue(np.array_equal(result, expected));
    }

    [TestMethod]
    public void T_2()
    {
        _circuit.ExecuteProgram("qreg q[1];\nx q[0];\nt q[0];");

        var result = _circuit.GetState().State;
        var expected = np.array(new Complex[] {
            new Complex(0, 0),
            new Complex(1/Math.Sqrt(2), 1/Math.Sqrt(2))
        });

        Assert.IsTrue(np.array_equal(result, expected));
    }

    [TestMethod]
    public void CNOT_1()
    {
        _circuit.ExecuteProgram("qreg q[2];\ncx q[0], q[1];");

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
    public void CNOT_2()
    {
        _circuit.ExecuteProgram("qreg q[2];\nx q[0];\ncx q[0], q[1];");

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
    public void CNOT_3()
    {
        _circuit.ExecuteProgram("qreg q[2];\nx q[0];\nx q[1];\ncx q[0], q[1];");

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
    public void CNOT_4()
    {
        _circuit.ExecuteProgram("qreg q[2];\nx q[0];\nx q[1];\ncx q[1], q[0];");

        var result = _circuit.GetState().State;
        var expected = np.array(new Complex[] {
            new Complex(0, 0),
            new Complex(0, 0),
            new Complex(1, 0),
            new Complex(0, 0)
        });

        Assert.IsTrue(np.array_equal(result, expected));
    }
}