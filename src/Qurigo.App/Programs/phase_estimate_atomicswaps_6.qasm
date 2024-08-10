OPENQASM 3;

# 0, 1, 2, 3, 4, 5 : control registers
# 6, 7, 8, 9       : target registers
qubit[10] q;

def qft6i() {
	h q[5];
	crk q[4], q[5], 2;
	crk q[3], q[5], 3;
	crk q[2], q[5], 4;
	crk q[1], q[5], 5;
	crk q[0], q[5], 6;

	h q[4];
	crk q[3], q[4], 2;
	crk q[2], q[4], 3;
	crk q[1], q[4], 4;
	crk q[0], q[4], 5;

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

	swap q[0], q[5];
	swap q[2], q[3];
	swap q[1], q[4];
}


# Put each of the control qubits in a superposition
h q[0];
h q[1];
h q[2];
h q[3];
h q[4];
h q[5];

# Prepare the target qubits
x q[6];

# control_register[0]
cswap q[0], q[6], q[7];
cswap q[0], q[7], q[8];
cswap q[0], q[8], q[9];


# control_register[1]
cswap q[1], q[6], q[7];
cswap q[1], q[7], q[8];
cswap q[1], q[8], q[9];

cswap q[1], q[6], q[7];
cswap q[1], q[7], q[8];
cswap q[1], q[8], q[9];


# control_register[2]
cswap q[2], q[6], q[7];
cswap q[2], q[7], q[8];
cswap q[2], q[8], q[9];

cswap q[2], q[6], q[7];
cswap q[2], q[7], q[8];
cswap q[2], q[8], q[9];

cswap q[2], q[6], q[7];
cswap q[2], q[7], q[8];
cswap q[2], q[8], q[9];

cswap q[2], q[6], q[7];
cswap q[2], q[7], q[8];
cswap q[2], q[8], q[9];

# control_register[3]
cswap q[3], q[6], q[7];
cswap q[3], q[7], q[8];
cswap q[3], q[8], q[9];

cswap q[3], q[6], q[7];
cswap q[3], q[7], q[8];
cswap q[3], q[8], q[9];

cswap q[3], q[6], q[7];
cswap q[3], q[7], q[8];
cswap q[3], q[8], q[9];

cswap q[3], q[6], q[7];
cswap q[3], q[7], q[8];
cswap q[3], q[8], q[9];

cswap q[3], q[6], q[7];
cswap q[3], q[7], q[8];
cswap q[3], q[8], q[9];

cswap q[3], q[6], q[7];
cswap q[3], q[7], q[8];
cswap q[3], q[8], q[9];

cswap q[3], q[6], q[7];
cswap q[3], q[7], q[8];
cswap q[3], q[8], q[9];

cswap q[3], q[6], q[7];
cswap q[3], q[7], q[8];
cswap q[3], q[8], q[9];


# control_register[4]
cswap q[4], q[6], q[7];
cswap q[4], q[7], q[8];
cswap q[4], q[8], q[9];

cswap q[4], q[6], q[7];
cswap q[4], q[7], q[8];
cswap q[4], q[8], q[9];

cswap q[4], q[6], q[7];
cswap q[4], q[7], q[8];
cswap q[4], q[8], q[9];

cswap q[4], q[6], q[7];
cswap q[4], q[7], q[8];
cswap q[4], q[8], q[9];

cswap q[4], q[6], q[7];
cswap q[4], q[7], q[8];
cswap q[4], q[8], q[9];

cswap q[4], q[6], q[7];
cswap q[4], q[7], q[8];
cswap q[4], q[8], q[9];

cswap q[4], q[6], q[7];
cswap q[4], q[7], q[8];
cswap q[4], q[8], q[9];

cswap q[4], q[6], q[7];
cswap q[4], q[7], q[8];
cswap q[4], q[8], q[9];

cswap q[4], q[6], q[7];
cswap q[4], q[7], q[8];
cswap q[4], q[8], q[9];

cswap q[4], q[6], q[7];
cswap q[4], q[7], q[8];
cswap q[4], q[8], q[9];

cswap q[4], q[6], q[7];
cswap q[4], q[7], q[8];
cswap q[4], q[8], q[9];

cswap q[4], q[6], q[7];
cswap q[4], q[7], q[8];
cswap q[4], q[8], q[9];

cswap q[4], q[6], q[7];
cswap q[4], q[7], q[8];
cswap q[4], q[8], q[9];

cswap q[4], q[6], q[7];
cswap q[4], q[7], q[8];
cswap q[4], q[8], q[9];

cswap q[4], q[6], q[7];
cswap q[4], q[7], q[8];
cswap q[4], q[8], q[9];

cswap q[4], q[6], q[7];
cswap q[4], q[7], q[8];
cswap q[4], q[8], q[9];

# control_register[5]
cswap q[5], q[6], q[7];
cswap q[5], q[7], q[8];
cswap q[5], q[8], q[9];

cswap q[5], q[6], q[7];
cswap q[5], q[7], q[8];
cswap q[5], q[8], q[9];

cswap q[5], q[6], q[7];
cswap q[5], q[7], q[8];
cswap q[5], q[8], q[9];

cswap q[5], q[6], q[7];
cswap q[5], q[7], q[8];
cswap q[5], q[8], q[9];

cswap q[5], q[6], q[7];
cswap q[5], q[7], q[8];
cswap q[5], q[8], q[9];

cswap q[5], q[6], q[7];
cswap q[5], q[7], q[8];
cswap q[5], q[8], q[9];

cswap q[5], q[6], q[7];
cswap q[5], q[7], q[8];
cswap q[5], q[8], q[9];

cswap q[5], q[6], q[7];
cswap q[5], q[7], q[8];
cswap q[5], q[8], q[9];

cswap q[5], q[6], q[7];
cswap q[5], q[7], q[8];
cswap q[5], q[8], q[9];

cswap q[5], q[6], q[7];
cswap q[5], q[7], q[8];
cswap q[5], q[8], q[9];

cswap q[5], q[6], q[7];
cswap q[5], q[7], q[8];
cswap q[5], q[8], q[9];

cswap q[5], q[6], q[7];
cswap q[5], q[7], q[8];
cswap q[5], q[8], q[9];

cswap q[5], q[6], q[7];
cswap q[5], q[7], q[8];
cswap q[5], q[8], q[9];

cswap q[5], q[6], q[7];
cswap q[5], q[7], q[8];
cswap q[5], q[8], q[9];

cswap q[5], q[6], q[7];
cswap q[5], q[7], q[8];
cswap q[5], q[8], q[9];

cswap q[5], q[6], q[7];
cswap q[5], q[7], q[8];
cswap q[5], q[8], q[9];

cswap q[5], q[6], q[7];
cswap q[5], q[7], q[8];
cswap q[5], q[8], q[9];

cswap q[5], q[6], q[7];
cswap q[5], q[7], q[8];
cswap q[5], q[8], q[9];

cswap q[5], q[6], q[7];
cswap q[5], q[7], q[8];
cswap q[5], q[8], q[9];

cswap q[5], q[6], q[7];
cswap q[5], q[7], q[8];
cswap q[5], q[8], q[9];

cswap q[5], q[6], q[7];
cswap q[5], q[7], q[8];
cswap q[5], q[8], q[9];

cswap q[5], q[6], q[7];
cswap q[5], q[7], q[8];
cswap q[5], q[8], q[9];

cswap q[5], q[6], q[7];
cswap q[5], q[7], q[8];
cswap q[5], q[8], q[9];

cswap q[5], q[6], q[7];
cswap q[5], q[7], q[8];
cswap q[5], q[8], q[9];

cswap q[5], q[6], q[7];
cswap q[5], q[7], q[8];
cswap q[5], q[8], q[9];

cswap q[5], q[6], q[7];
cswap q[5], q[7], q[8];
cswap q[5], q[8], q[9];

cswap q[5], q[6], q[7];
cswap q[5], q[7], q[8];
cswap q[5], q[8], q[9];

cswap q[5], q[6], q[7];
cswap q[5], q[7], q[8];
cswap q[5], q[8], q[9];

cswap q[5], q[6], q[7];
cswap q[5], q[7], q[8];
cswap q[5], q[8], q[9];

cswap q[5], q[6], q[7];
cswap q[5], q[7], q[8];
cswap q[5], q[8], q[9];

cswap q[5], q[6], q[7];
cswap q[5], q[7], q[8];
cswap q[5], q[8], q[9];

cswap q[5], q[6], q[7];
cswap q[5], q[7], q[8];
cswap q[5], q[8], q[9];


# Inverse QFT
qft6i();

