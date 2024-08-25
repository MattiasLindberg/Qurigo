using Numpy;
using Qurigo.Circuit;
using Qurigo.InstructionSet;
using Qurigo.Interfaces;
using Qurigo.State;
using Qurigo.Simulator;
using System.Numerics;
using Microsoft.Extensions.DependencyInjection;

namespace Qurigo.UnitTests.Vector;

[TestClass]
public class VS_IBMEagleR3ProgramTests
{
    private IQuantumCircuit _circuit;
    private QurigoSimulator _simulator;

    [TestInitialize]
    public void SetupTests()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddSingleton<IState, VectorState>();
        serviceCollection.AddSingleton<IInstructionSet, IBMEagleR3>();
        serviceCollection.AddSingleton<IQuantumCircuit, QuantumCircuit>();
        serviceCollection.AddSingleton<IExecutionContext, Simulator.ExecutionContext>();
        var serviceProvider = serviceCollection.BuildServiceProvider();

        _simulator = new QurigoSimulator(serviceProvider.GetService<IExecutionContext>()!);

        var instructionSet = new HTCNOTXYZ();
        var state = new VectorState();
        _circuit = new QuantumCircuit(state, instructionSet);
    }

    [TestMethod]
    public void ZXY()
    {
        IState state = _simulator.RunString("qubit[1] q;\nz q[0];\nx q[0];\ny q[0];");
        var result = state.State;
        var expected = np.array(new Complex[] {
            new Complex(0, -1),
            0
        });

        Assert.IsTrue(result.array_equal(expected));
    }

    [TestMethod]
    public void RandomProgram_1()
    {
        IState state = _simulator.RunString("qubit[5] q;\r\nx q[0];\r\nh q[2];\r\ncx q[0], q[1];\r\nccx q[0], q[1], q[2];\r\nswap q[2], q[3];\r\nz q[0];\r\nt q[3];\r\nh q[0];\r\ncx q[0], q[1];\r\nswap q[3], q[4];\r\nccx q[4], q[0], q[2];\r\ny q[2];\r\nt q[0];");
        var result = state.State;
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

        Assert.IsTrue(result.allclose(expected));
    }

    [TestMethod]
    public void RandomProgram_2()
    {
        IState state = _simulator.RunString("qubit[4] q;\r\nh q[0];\r\ncx q[0], q[1];\r\nh q[2];\r\ncx q[2], q[3];\r\nt q[1];\r\nx q[3];\r\nz q[2];\r\nh q[1];\r\ncx q[1], q[2];\r\nt q[2];\r\nx q[1];\r\ny q[0];");
        var result = state.State;
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

        Assert.IsTrue(result.allclose(expected));
    }

}
