OPENQASM 3;

qubit p;
qubit[4] q;

def qft4i() {
    # This should not be here, I think
    x q[0];
    x q[1];
    x q[2];
    x q[3];

    h q[3];
    crk q[2], q[3], 2;
    crk q[1], q[3], 3;
    crk q[0], q[3], 4;

    h q[2];
    crk q[1], q[2], 2;
    crk q[0], q[2], 3;

    h q[1];
    crk q[0], q[1], 2;

    h q[0];

    swap q[0], q[2];
}

def c_amod15(int control) {
    # U = QuantumCircuit(4)
    if( control == 1) {

        if (arg1 == 2) {
            swap q[2], q[3];
            swap q[1], q[2];
            swap q[0], q[1];
        }

        if (arg1 == 13) {
            swap q[2], q[3];
            swap q[1], q[2];
            swap q[0], q[1];
        }

        if (arg1 == 7) {
            swap q[0], q[1];
            swap q[1], q[2];
            swap q[2], q[3];
        }

        if (arg1 == 8) {
            swap q[0], q[1];
            swap q[1], q[2];
            swap q[2], q[3];
        }

        if (arg1 == 4) {
            swap q[1], q[3];
            swap q[0], q[2];
        }

        if (arg1 == 11) {
            swap q[1], q[3];
            swap q[0], q[2];
        }

        if (arg1 == 7) {
            x q[0];
            x q[1];
            x q[2];
            x q[3];
        }

        if (arg1 == 11) {
            x q[0];
            x q[1];
            x q[2];
            x q[3];
        }

        if (arg1 == 13) {
            x q[0];
            x q[1];
            x q[2];
            x q[3];
        }

    } // end of control check
}

# Psi_prep
x q[0];



# Phase estimation


c_amod15();

qft4i();
