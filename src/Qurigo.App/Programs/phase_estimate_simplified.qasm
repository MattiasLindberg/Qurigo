OPENQASM 3;

# 0, 1, 2    : control registers
# 3, 4, 5, 6 : target registers
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

def 8mod15() {
    # Change index for each qubit, each should have +3 shift
    swap q[3], q[4];
    swap q[4], q[5];
    swap q[5], q[6];
}

# Psi_prep
x q[3];

h q[0];
h q[1];
h q[2];

# control_register[0]
ctrl @ 8mod15 q[0];

# control_register[1]
ctrl @ 8mod15 q[1];
ctrl @ 8mod15 q[1];

# control_register[2]
ctrl @ 8mod15 q[2];
ctrl @ 8mod15 q[2];
ctrl @ 8mod15 q[2];
ctrl @ 8mod15 q[2];

qft4i();
