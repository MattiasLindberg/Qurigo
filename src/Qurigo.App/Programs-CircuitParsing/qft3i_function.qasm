qreg q[3];

def qft3i() {
	x q[0];
	x q[1];
	x q[2];

	h q[2];
	crk q[1], q[2], 2;
	crk q[0], q[2], 3;

	h q[1];
	crk q[0], q[1], 2;

	h q[0];

	swap q[0], q[2];
}

def c_amod15(int arg1) {
    # U = QuantumCircuit(4)

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
}

# Psi_prep
x q[0];

# Phase estimation

qft3i();

c_amod15();
