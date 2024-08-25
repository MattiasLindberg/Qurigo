using Numpy;
using Qurigo.Interfaces;

namespace Qurigo.InstructionSet;

internal class Gate : IGate
{
    public NDarray Base { get; }
    public NDarray Dagger => np.conj(Base.T);

    public Gate(NDarray baseMatrix)
    {
        Base = baseMatrix;
    }
}
