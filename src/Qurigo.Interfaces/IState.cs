using Numpy;
using System.Numerics;

namespace Qurigo.Interfaces;
public interface IState
{
    int Size { get; }

    IState ApplyGate(IGate gate);
    void Initialize(int qubitCount);
    NDarray State { get; }
    string ToString();
    Complex[] ToArray();
}
