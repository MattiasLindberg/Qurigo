namespace Qurigo.Interfaces;

public interface ICircuit
{
    IState GetState();
    void ExecuteProgram(string program);
}
