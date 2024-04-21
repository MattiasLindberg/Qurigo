using Numpy;
using Qurigo.Circuit.BaseCircuit;
using Qurigo.InstructionSet.HTCNOT;
using Qurigo.Interfaces;
using Qurigo.State.VectorState;
using System.Numerics;

namespace Qurigo.UnitTests;

[TestClass]
public class IBMEagleR3NativeGatesTests
{
    private ICircuit _circuit;

    [TestInitialize]
    public void SetupTests()
    {
        var instructionSet = new IBMEagleR3();
        var state = new VectorState();
        _circuit = new BaseCircuit(instructionSet, state);
    }

    [TestMethod]
    public void RZ()
    {
        _circuit.ExecuteProgram("qreg q[1];\nrz(pi/2) q[0];");

        var result = _circuit.GetState().State;
        var expected = np.array(new Complex[] { 
            new Complex(1 / Math.Sqrt(2), -1 / Math.Sqrt(2) ), 0 }
        );

        Assert.IsTrue(np.allclose(result, expected));
    }

    [TestMethod]
    public void RZ_4()
    {
        _circuit.ExecuteProgram("qreg q[1];\nrz(pi/4) q[0];");

        var result = _circuit.GetState().State;
        var expected = np.array(new Complex[] {
            new Complex(0.92387953, -0.38268343),
            new Complex(0, 0)
        }
        );

        Assert.IsTrue(np.allclose(result, expected));
    }

    [TestMethod]
    public void RZ_2()
    {
        _circuit.ExecuteProgram("qreg q[2];\nsx q[0];\r\nrz(pi/2) q[1];\r\nrz(pi/2) q[0];\r\nsx q[1];");

        var result = _circuit.GetState().State;
        var expected = np.array(new Complex[] {
            0.5,
            0.5,
            new Complex(0, -0.5),
            new Complex(0, -0.5)
        }
        );

        Assert.IsTrue(np.allclose(result, expected));
    }

    [TestMethod]
    public void RZX()
    {
          _circuit.ExecuteProgram("qreg q[2];\nh q[1];\ncx q[0], q[1];\nrz(pi/4) q[1];\ncx q[0], q[1];\nh q[1];");

        var result = _circuit.GetState().State;
        var expected = np.array(new Complex[] {
            new Complex(0.923879533, 0),
            0,
            new Complex(0, -0.382683432),
            0
        }
        );

        Assert.IsTrue(np.allclose(result, expected));
    }

    [TestMethod]
    public void RZX_2()
    {
        _circuit.ExecuteProgram("qreg q[2];\nh q[1];\ncx q[0], q[1];\nrz(pi/4) q[1];\ncx q[0], q[1];\nh q[1];\nx q[0]");

        var result = _circuit.GetState().State;
        var expected = np.array(new Complex[] {
            0,
            new Complex(0.923879533, 0),
            0,
            new Complex(0, -0.382683432),
        }
        );

        Assert.IsTrue(np.allclose(result, expected));
    }

    [TestMethod]
    public void RZX_3()
    {
        // ECR via gate sequence
        _circuit.ExecuteProgram("qreg q[2];\nh q[1];\ncx q[0], q[1];\nrz(pi/4) q[1];\ncx q[0], q[1];\nh q[1];\nx q[0];\nh q[1];\ncx q[0], q[1];\nrz(-pi/4) q[1];\ncx q[0], q[1];\nh q[1];");

        var result = _circuit.GetState().State;
        var expected = np.array(new Complex[] {
            0,
            new Complex(0.70710678, 0),
            0,
            new Complex(0, -0.70710678)
        }
        );

        Assert.IsTrue(np.allclose(result, expected));
    }

    [TestMethod]
    public void SX_1()
    {
        _circuit.ExecuteProgram("qreg q[1];\nsx q[0];");

        var result = _circuit.GetState().State;
        var expected = np.array(new Complex[] {
            new Complex(0.5, 0.5 ),
            new Complex(0.5, -0.5 )
        }
        );

        Assert.IsTrue(np.allclose(result, expected));
    }

    [TestMethod]
    public void SX_2()
    {
        _circuit.ExecuteProgram("qreg q[1];\nsx q[0];\nsx q[0];");

        var result = _circuit.GetState().State;
        var expected = np.array(new Complex[] {
            new Complex(0, 0 ),
            new Complex(1, 0 )
        }
        );

        Assert.IsTrue(np.allclose(result, expected));
    }

    [TestMethod]
    public void ECR_1()
    {
        _circuit.ExecuteProgram("qreg q[2];\necr q[0], q[1];");

        var result = _circuit.GetState().State;
        var expected = np.array(new Complex[] {
            0,
            new Complex(0.70710678, 0),
            0,
            new Complex(0, -0.70710678)
        }
        );

        Assert.IsTrue(np.allclose(result, expected));
    }

    [TestMethod]
    public void ECR_2()
    {
        // ECR via gate sequence
        _circuit.ExecuteProgram("qreg q[2];\nh q[1];\ncx q[0], q[1];\nrz(pi/4) q[1];\ncx q[0], q[1];\nh q[1];\nx q[0];\nh q[1];\ncx q[0], q[1];\nrz(-pi/4) q[1];\ncx q[0], q[1];\nh q[1];");

        var result = _circuit.GetState().State;
        var expected = np.array(new Complex[] {
            0,
            new Complex(0.70710678, 0),
            0,
            new Complex(0, -0.70710678)
        }
        );

        Assert.IsTrue(np.allclose(result, expected));
    }

    [TestMethod]
    public void ECR_3()
    {
        _circuit.ExecuteProgram("qreg q[2];\nx q[0];\necr q[0], q[1];");

        var result = _circuit.GetState().State;
        var expected = np.array(new Complex[] {
            new Complex(0.70710678, 0),
            0,
            new Complex(0, 0.70710678),
            0
        }
        );

        Assert.IsTrue(np.allclose(result, expected));
    }

    [TestMethod]
    public void ECR_4()
    {
        _circuit.ExecuteProgram("qreg q[2];\nh q[0];\necr q[0], q[1];");

        var result = _circuit.GetState().State;
        var expected = np.array(new Complex[] {
            0.5,
            0.5,
            new Complex(0, 0.5),
            new Complex(0, -0.5)
        }
        );

        Assert.IsTrue(np.allclose(result, expected));
    }

    [TestMethod]
    public void ECR_5()
    {
        _circuit.ExecuteProgram("qreg q[2];\necr q[0], q[1];\necr q[0], q[1];\n");

        var result = _circuit.GetState().State;
        var expected = np.array(new Complex[] {
            1,
            0,
            0,
            0
        }
        );

        Assert.IsTrue(np.allclose(result, expected));
    }

    [TestMethod]
    public void ECR_6()
    {
        _circuit.ExecuteProgram("qreg q[2];\necr q[0], q[1];\nx q[0]\necr q[0], q[1];\n");

        var result = _circuit.GetState().State;
        var expected = np.array(new Complex[] {
            0,
            0,
            0,
            new Complex(0, -1)
        }
        );

        Assert.IsTrue(np.allclose(result, expected));
    }

    [TestMethod]
    public void ECR_7()
    {
        _circuit.ExecuteProgram("qreg q[2];\necr q[0], q[1];\nh q[0]\necr q[0], q[1];\n");

        var result = _circuit.GetState().State;
        var expected = np.array(new Complex[] {
            -1/Math.Sqrt(2),
            0,
            0,
            new Complex(0, -1/Math.Sqrt(2))
        }
        );

        Assert.IsTrue(np.allclose(result, expected));
    }

    [TestMethod]
    public void ECR_8()
    {
        _circuit.ExecuteProgram("qreg q[2];\necr q[0], q[1];\nh q[1]\necr q[0], q[1];\n");

        var result = _circuit.GetState().State;
        var expected = np.array(new Complex[] {
            0,
            0,
            new Complex(1/Math.Sqrt(2), 1/Math.Sqrt(2)),
            0
        }
        );

        Assert.IsTrue(np.allclose(result, expected));
    }
}