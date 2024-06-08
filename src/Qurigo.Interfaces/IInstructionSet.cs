namespace Qurigo.Interfaces;

public interface IInstructionSet
{
    GateNames NativeGates { get; }
    GateNames SupportedGates { get; }
    bool CacheNativeGates { get; set; }
    bool CacheSupportedGates { get; set; }

    void Initialize(int qubitCount);

    IGate H(int actOnQubit);
    IGate T(int actOnQubit);
    IGate CNOT(int controlQubit, int actOnQubit);
    IGate X(int actOnQubit);
    IGate Y(int actOnQubit);
    IGate Z(int actOnQubit);
    IGate SWAP(int actOnQubit1, int actOnQubit2);
    IGate Toffoli(int controlQubit1, int controlQubit2, int actOnQubit);

    IGate SX(int actOnQubit);
    IGate ECR(int actOnQubit1, int actOnQubit2);
    IGate RZ(int actOnQubit, double theta);
    IGate S(int actOnQubit);

    IGate CRk(int controlQubit, int actOnQubit, int k);
}
