using Numpy;
using Qurigo.Circuit;
using Qurigo.InstructionSet;
using Qurigo.Interfaces;
using Qurigo.State;
using System.Numerics;

namespace Qurigo.UnitTests.Vector;

[TestClass]
public class VS_IBMEagleR3NativeGatesTests
{
    private IQuantumCircuit _circuit;

    [TestInitialize]
    public void SetupTests()
    {
        var instructionSet = new IBMEagleR3();
        var state = new VectorState();
        _circuit = new QuantumCircuit(state, instructionSet);
    }

    [TestMethod]
    public void RZ()
    {
        // _circuit.ExecuteProgram("qreg q[1];\nrz(pi/2) q[0];");
        _circuit.Initialize(1);
        _circuit.ApplyGate(GateNames.RZ, new List<Parameter>() { new Parameter() { Index = 0 }, new Parameter() { Value = Math.PI / 2 } });

        var result = _circuit.GetState().State;
        var expected = np.array(new Complex[] {
            new Complex(1 / Math.Sqrt(2), -1 / Math.Sqrt(2) ), 0 }
        );

        Assert.IsTrue(result.allclose(expected));
    }

    [TestMethod]
    public void RZ_4()
    {
        // _circuit.ExecuteProgram("qreg q[1];\nrz(pi/4) q[0];");
        _circuit.Initialize(1);
        _circuit.ApplyGate(GateNames.RZ, new List<Parameter>() { new Parameter() { Index = 0 }, new Parameter() { Value = Math.PI / 4 } });

        var result = _circuit.GetState().State;
        var expected = np.array(new Complex[] {
            new Complex(0.92387953, -0.38268343),
            new Complex(0, 0)
        }
        );

        Assert.IsTrue(result.allclose(expected));
    }

    [TestMethod]
    public void RZ_2()
    {
        // _circuit.ExecuteProgram("qreg q[2];\nsx q[0];\r\nrz(pi/2) q[1];\r\nrz(pi/2) q[0];\r\nsx q[1];");
        _circuit.Initialize(2);
        _circuit.ApplyGate(GateNames.SX, new List<Parameter>() { new Parameter() { Index = 0 } });
        _circuit.ApplyGate(GateNames.RZ, new List<Parameter>() { new Parameter() { Index = 1 }, new Parameter() { Value = Math.PI / 2 } });
        _circuit.ApplyGate(GateNames.RZ, new List<Parameter>() { new Parameter() { Index = 0 }, new Parameter() { Value = Math.PI / 2 } });
        _circuit.ApplyGate(GateNames.SX, new List<Parameter>() { new Parameter() { Index = 1 } });

        var result = _circuit.GetState().State;
        var expected = np.array(new Complex[] {
            0.5,
            0.5,
            new Complex(0, -0.5),
            new Complex(0, -0.5)
        }
        );

        Assert.IsTrue(result.allclose(expected));
    }

    [TestMethod]
    public void RZX()
    {
        // _circuit.ExecuteProgram("qreg q[2];\nh q[1];\ncx q[0], q[1];\nrz(pi/4) q[1];\ncx q[0], q[1];\nh q[1];");
        _circuit.Initialize(2);
        _circuit.ApplyGate(GateNames.H, new List<Parameter>() { new Parameter() { Index = 1 } });
        _circuit.ApplyGate(GateNames.CX, new List<Parameter>() { new Parameter() { Index = 0 }, new Parameter() { Index = 1 } });
        _circuit.ApplyGate(GateNames.RZ, new List<Parameter>() { new Parameter() { Index = 1 }, new Parameter() { Value = Math.PI / 4 } });
        _circuit.ApplyGate(GateNames.CX, new List<Parameter>() { new Parameter() { Index = 0 }, new Parameter() { Index = 1 } });
        _circuit.ApplyGate(GateNames.H, new List<Parameter>() { new Parameter() { Index = 1 } });

        var result = _circuit.GetState().State;
        var expected = np.array(new Complex[] {
            new Complex(0.923879533, 0),
            0,
            new Complex(0, -0.382683432),
            0
        }
        );

        Assert.IsTrue(result.allclose(expected));
    }

    [TestMethod]
    public void RZX_2()
    {
        // _circuit.ExecuteProgram("qreg q[2];\nh q[1];\ncx q[0], q[1];\nrz(pi/4) q[1];\ncx q[0], q[1];\nh q[1];\nx q[0]");
        _circuit.Initialize(2);
        _circuit.ApplyGate(GateNames.H, new List<Parameter>() { new Parameter() { Index = 1 } });
        _circuit.ApplyGate(GateNames.CX, new List<Parameter>() { new Parameter() { Index = 0 }, new Parameter() { Index = 1 } });
        _circuit.ApplyGate(GateNames.RZ, new List<Parameter>() { new Parameter() { Index = 1 }, new Parameter() { Value = Math.PI / 4 } });
        _circuit.ApplyGate(GateNames.CX, new List<Parameter>() { new Parameter() { Index = 0 }, new Parameter() { Index = 1 } });
        _circuit.ApplyGate(GateNames.H, new List<Parameter>() { new Parameter() { Index = 1 } });
        _circuit.ApplyGate(GateNames.X, new List<Parameter>() { new Parameter() { Index = 0 } });

        var result = _circuit.GetState().State;
        var expected = np.array(new Complex[] {
            0,
            new Complex(0.923879533, 0),
            0,
            new Complex(0, -0.382683432),
        }
        );

        Assert.IsTrue(result.allclose(expected));
    }

    [TestMethod]
    public void RZX_3()
    {
        // ECR via gate sequence
        // _circuit.ExecuteProgram("qreg q[2];\nh q[1];\ncx q[0], q[1];\nrz(pi/4) q[1];\ncx q[0], q[1];\nh q[1];\nx q[0];\n   h q[1];\ncx q[0], q[1];\nrz(-pi/4) q[1];\ncx q[0], q[1];\nh q[1];");
        _circuit.Initialize(2);
        _circuit.ApplyGate(GateNames.H, new List<Parameter>() { new Parameter() { Index = 1 } });
        _circuit.ApplyGate(GateNames.CX, new List<Parameter>() { new Parameter() { Index = 0 }, new Parameter() { Index = 1 } });
        _circuit.ApplyGate(GateNames.RZ, new List<Parameter>() { new Parameter() { Index = 1 }, new Parameter() { Value = Math.PI / 4 } });
        _circuit.ApplyGate(GateNames.CX, new List<Parameter>() { new Parameter() { Index = 0 }, new Parameter() { Index = 1 } });
        _circuit.ApplyGate(GateNames.H, new List<Parameter>() { new Parameter() { Index = 1 } });
        _circuit.ApplyGate(GateNames.X, new List<Parameter>() { new Parameter() { Index = 0 } });

        _circuit.ApplyGate(GateNames.H, new List<Parameter>() { new Parameter() { Index = 1 } });
        _circuit.ApplyGate(GateNames.CX, new List<Parameter>() { new Parameter() { Index = 0 }, new Parameter() { Index = 1 } });
        _circuit.ApplyGate(GateNames.RZ, new List<Parameter>() { new Parameter() { Index = 1 }, new Parameter() { Value = -Math.PI / 4 } });
        _circuit.ApplyGate(GateNames.CX, new List<Parameter>() { new Parameter() { Index = 0 }, new Parameter() { Index = 1 } });
        _circuit.ApplyGate(GateNames.H, new List<Parameter>() { new Parameter() { Index = 1 } });

        var result = _circuit.GetState().State;
        var expected = np.array(new Complex[] {
            0,
            new Complex(0.70710678, 0),
            0,
            new Complex(0, -0.70710678)
        }
        );

        Assert.IsTrue(result.allclose(expected));
    }

    [TestMethod]
    public void SX_1()
    {
        // _circuit.ExecuteProgram("qreg q[1];\nsx q[0];");
        _circuit.Initialize(1);
        _circuit.ApplyGate(GateNames.SX, new List<Parameter>() { new Parameter() { Index = 0 } });

        var result = _circuit.GetState().State;
        var expected = np.array(new Complex[] {
            new Complex(0.5, 0.5 ),
            new Complex(0.5, -0.5 )
        }
        );

        Assert.IsTrue(result.allclose(expected));
    }

    [TestMethod]
    public void SX_2()
    {
        // _circuit.ExecuteProgram("qreg q[1];\nsx q[0];\nsx q[0];");
        _circuit.Initialize(1);
        _circuit.ApplyGate(GateNames.SX, new List<Parameter>() { new Parameter() { Index = 0 } });
        _circuit.ApplyGate(GateNames.SX, new List<Parameter>() { new Parameter() { Index = 0 } });

        var result = _circuit.GetState().State;
        var expected = np.array(new Complex[] {
            new Complex(0, 0 ),
            new Complex(1, 0 )
        }
        );

        Assert.IsTrue(result.allclose(expected));
    }

    [TestMethod]
    public void ECR_1()
    {
        // _circuit.ExecuteProgram("qreg q[2];\necr q[0], q[1];");
        _circuit.Initialize(2);
        _circuit.ApplyGate(GateNames.ECR, new List<Parameter>() { new Parameter() { Index = 0 }, new Parameter() { Index = 1 } });

        var result = _circuit.GetState().State;
        var expected = np.array(new Complex[] {
            0,
            new Complex(0.70710678, 0),
            0,
            new Complex(0, -0.70710678)
        }
        );

        Assert.IsTrue(result.allclose(expected));
    }

    [TestMethod]
    public void ECR_2()
    {
        // ECR via gate sequence
        _circuit.Initialize(2);
        _circuit.ApplyGate(GateNames.H, new List<Parameter>() { new Parameter() { Index = 1 } });
        _circuit.ApplyGate(GateNames.CX, new List<Parameter>() { new Parameter() { Index = 0 }, new Parameter() { Index = 1 } });
        _circuit.ApplyGate(GateNames.RZ, new List<Parameter>() { new Parameter() { Index = 1 }, new Parameter() { Value = Math.PI / 4 } });
        _circuit.ApplyGate(GateNames.CX, new List<Parameter>() { new Parameter() { Index = 0 }, new Parameter() { Index = 1 } });
        _circuit.ApplyGate(GateNames.H, new List<Parameter>() { new Parameter() { Index = 1 } });
        _circuit.ApplyGate(GateNames.X, new List<Parameter>() { new Parameter() { Index = 0 } });

        _circuit.ApplyGate(GateNames.H, new List<Parameter>() { new Parameter() { Index = 1 } });
        _circuit.ApplyGate(GateNames.CX, new List<Parameter>() { new Parameter() { Index = 0 }, new Parameter() { Index = 1 } });
        _circuit.ApplyGate(GateNames.RZ, new List<Parameter>() { new Parameter() { Index = 1 }, new Parameter() { Value = -Math.PI / 4 } });
        _circuit.ApplyGate(GateNames.CX, new List<Parameter>() { new Parameter() { Index = 0 }, new Parameter() { Index = 1 } });
        _circuit.ApplyGate(GateNames.H, new List<Parameter>() { new Parameter() { Index = 1 } });

        var result = _circuit.GetState().State;
        var expected = np.array(new Complex[] {
            0,
            new Complex(0.70710678, 0),
            0,
            new Complex(0, -0.70710678)
        }
        );

        Assert.IsTrue(result.allclose(expected));
    }

    [TestMethod]
    public void ECR_3()
    {
        // _circuit.ExecuteProgram("qreg q[2];\nx q[0];\necr q[0], q[1];");
        _circuit.Initialize(2);
        _circuit.ApplyGate(GateNames.X, new List<Parameter>() { new Parameter() { Index = 0 } });
        _circuit.ApplyGate(GateNames.ECR, new List<Parameter>() { new Parameter() { Index = 0 }, new Parameter() { Index = 1 } });

        var result = _circuit.GetState().State;
        var expected = np.array(new Complex[] {
            new Complex(0.70710678, 0),
            0,
            new Complex(0, 0.70710678),
            0
        }
        );

        Assert.IsTrue(result.allclose(expected));
    }

    [TestMethod]
    public void ECR_4()
    {
        // _circuit.ExecuteProgram("qreg q[2];\nh q[0];\necr q[0], q[1];");
        _circuit.Initialize(2);
        _circuit.ApplyGate(GateNames.H, new List<Parameter>() { new Parameter() { Index = 0 } });
        _circuit.ApplyGate(GateNames.ECR, new List<Parameter>() { new Parameter() { Index = 0 }, new Parameter() { Index = 1 } });

        var result = _circuit.GetState().State;
        var expected = np.array(new Complex[] {
            0.5,
            0.5,
            new Complex(0, 0.5),
            new Complex(0, -0.5)
        }
        );

        Assert.IsTrue(result.allclose(expected));
    }

    [TestMethod]
    public void ECR_5()
    {
        // _circuit.ExecuteProgram("qreg q[2];\necr q[0], q[1];\necr q[0], q[1];\n");
        _circuit.Initialize(2);
        _circuit.ApplyGate(GateNames.ECR, new List<Parameter>() { new Parameter() { Index = 0 }, new Parameter() { Index = 1 } });
        _circuit.ApplyGate(GateNames.ECR, new List<Parameter>() { new Parameter() { Index = 0 }, new Parameter() { Index = 1 } });

        var result = _circuit.GetState().State;
        var expected = np.array(new Complex[] {
            1,
            0,
            0,
            0
        }
        );

        Assert.IsTrue(result.allclose(expected));
    }

    [TestMethod]
    public void ECR_6()
    {
        // _circuit.ExecuteProgram("qreg q[2];\necr q[0], q[1];\nx q[0]\necr q[0], q[1];\n");
        _circuit.Initialize(2);
        _circuit.ApplyGate(GateNames.ECR, new List<Parameter>() { new Parameter() { Index = 0 }, new Parameter() { Index = 1 } });
        _circuit.ApplyGate(GateNames.X, new List<Parameter>() { new Parameter() { Index = 0 } });
        _circuit.ApplyGate(GateNames.ECR, new List<Parameter>() { new Parameter() { Index = 0 }, new Parameter() { Index = 1 } });

        var result = _circuit.GetState().State;
        var expected = np.array(new Complex[] {
            0,
            0,
            0,
            new Complex(0, -1)
        }
        );

        Assert.IsTrue(result.allclose(expected));
    }

    [TestMethod]
    public void ECR_7()
    {
        // _circuit.ExecuteProgram("qreg q[2];\necr q[0], q[1];\nh q[0]\necr q[0], q[1];\n");
        _circuit.Initialize(2);
        _circuit.ApplyGate(GateNames.ECR, new List<Parameter>() { new Parameter() { Index = 0 }, new Parameter() { Index = 1 } });
        _circuit.ApplyGate(GateNames.H, new List<Parameter>() { new Parameter() { Index = 0 } });
        _circuit.ApplyGate(GateNames.ECR, new List<Parameter>() { new Parameter() { Index = 0 }, new Parameter() { Index = 1 } });

        var result = _circuit.GetState().State;
        var expected = np.array(new Complex[] {
            -1/Math.Sqrt(2),
            0,
            0,
            new Complex(0, -1/Math.Sqrt(2))
        }
        );

        Assert.IsTrue(result.allclose(expected));
    }

    [TestMethod]
    public void ECR_8()
    {
        // circuit.ExecuteProgram("qreg q[2];\necr q[0], q[1];\nh q[1]\necr q[0], q[1];\n");
        _circuit.Initialize(2);
        _circuit.ApplyGate(GateNames.ECR, new List<Parameter>() { new Parameter() { Index = 0 }, new Parameter() { Index = 1 } });
        _circuit.ApplyGate(GateNames.H, new List<Parameter>() { new Parameter() { Index = 1 } });
        _circuit.ApplyGate(GateNames.ECR, new List<Parameter>() { new Parameter() { Index = 0 }, new Parameter() { Index = 1 } });

        var result = _circuit.GetState().State;
        var expected = np.array(new Complex[] {
            0,
            0,
            new Complex(1/Math.Sqrt(2), 1/Math.Sqrt(2)),
            0
        }
        );

        Assert.IsTrue(result.allclose(expected));
    }
}