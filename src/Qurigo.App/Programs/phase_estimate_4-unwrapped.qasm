OPENQASM 3;

# 0-2: control registers, 3-6: target registers
qubit[7] q;

def qft4i() {
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

        # Change index for each qubit, each should have +3 shift

        if (arg1 == 2) {
            swap q[5], q[6];
            swap q[4], q[5];
            swap q[3], q[4];
        }

        if (arg1 == 13) {
            swap q[5], q[6];
            swap q[4], q[5];
            swap q[3], q[4];
        }

        if (arg1 == 7) {
            swap q[3], q[4];
            swap q[4], q[5];
            swap q[5], q[6];
        }

        if (arg1 == 8) {
#            swap q[3], q[4];
#            swap q[4], q[5];
#            swap q[5], q[6];

            swap q[5], q[6];
            swap q[4], q[5];
            swap q[3], q[4];
        }

        if (arg1 == 4) {
            swap q[4], q[6];
            swap q[3], q[5];
        }

        if (arg1 == 11) {
            swap q[4], q[6];
            swap q[3], q[5];
        }

        if (arg1 == 7) {
            x q[3];
            x q[4];
            x q[5];
            x q[6];
        }

        if (arg1 == 11) {
            x q[3];
            x q[4];
            x q[5];
            x q[6];
        }

        if (arg1 == 13) {
            x q[3];
            x q[4];
            x q[5];
            x q[6];
        }

    } // end of control check
}

# Psi_prep
x q[3];

h q[0];
h q[1];
h q[2];

# control_register[0]
c_amod15(q[0]);

# control_register[1]
c_amod15(q[1]);
c_amod15(q[1]);

# control_register[2]
c_amod15(q[2]);
c_amod15(q[2]);
c_amod15(q[2]);
c_amod15(q[2]);

qft4i();
