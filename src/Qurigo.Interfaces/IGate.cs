using Numpy;

namespace Qurigo.Interfaces;

public interface IGate
{
    NDarray Base{ get; }
    NDarray Dagger{ get; }
}
