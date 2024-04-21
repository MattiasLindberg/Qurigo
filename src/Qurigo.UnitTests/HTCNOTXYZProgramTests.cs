using Numpy;
using Qurigo.Circuit.BaseCircuit;
using Qurigo.InstructionSet.HTCNOT;
using Qurigo.Interfaces;
using Qurigo.State.VectorState;
using System.Numerics;

namespace Qurigo.UnitTests;

[TestClass]
public class HTCNOTXYZProgramTests
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
    public void ZXY()
    {
        _circuit.ExecuteProgram("qreg q[1];\nz q[0];\nx q[0];\ny q[0];");

        var result = _circuit.GetState().State;
        var expected = np.array(new Complex[] {
            new Complex(0, -1),
            0
        });

        Assert.IsTrue(np.array_equal(result, expected));
    }

    [TestMethod]
    public void RandomProgram_1()
    {
        _circuit.ExecuteProgram("qreg q[5];\r\nx q[0];\r\nh q[2];\r\ncx q[0], q[1];\r\nccx q[0], q[1], q[2];\r\nswap q[2], q[3];\r\nz q[0];\r\nt q[3];\r\nh q[0];\r\ncx q[0], q[1];\r\nswap q[3], q[4];\r\nccx q[4], q[0], q[2];\r\ny q[2];\r\nt q[0];");

        var result = _circuit.GetState().State;
        var expected = np.array(new Complex[] {
            0,
            0,
            0,
            0,
            0,
            new Complex(-0.353553391, 0.353553391),
            new Complex(0, -0.5),
            0,
 
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            
            0,
            new Complex(0.5, 0),
            0,
            0,
            0,
            0,
            new Complex(0.353553391, -0.353553391),
            0,
            
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0
        });

        Assert.IsTrue(np.allclose(result, expected));
    }

    [TestMethod]
    public void RandomProgram_2()
    {
        _circuit.ExecuteProgram("qreg q[4];\r\nh q[0];\r\ncx q[0], q[1];\r\nh q[2];\r\ncx q[2], q[3];\r\nt q[1];\r\nx q[3];\r\nz q[2];\r\nh q[1];\r\ncx q[1], q[2];\r\nt q[2];\r\nx q[1];\r\ny q[0];;");

        var result = _circuit.GetState().State;
        var expected = np.array(new Complex[] {
            new Complex(0.25, -0.25),
            new Complex(0, -0.353553391),
            0,
            0,
            0,
            0,
            new Complex(-0.353553391, 0),
            new Complex(0.25, -0.25),

            0,
            0,
            new Complex(0.25, -0.25),
            new Complex(0, 0.353553391),
            new Complex(-0.353553391, 0),
            new Complex(-0.25, +0.25),
            0,
            0
        });

        Assert.IsTrue(np.allclose(result, expected));
    }

}
