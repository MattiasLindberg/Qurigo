OPENQASM 3;

# 0, 1, 2, 3, 4  : control registers
# 5, 6, 7, 8 : target registers
qubit[9] q;

def qft4i() {
	h q[2];
	crk q[1], q[2], 2;
	crk q[0], q[2], 3;

	h q[1];
	crk q[0], q[1], 2;

	h q[0];

	swap q[0], q[2];
}

def qft5i() {
	h q[4];
	crk q[3], q[2], 2;
	crk q[2], q[2], 3;
	crk q[1], q[2], 4;
	crk q[0], q[2], 5;

	h q[3];
	crk q[2], q[2], 2;
	crk q[1], q[2], 3;
	crk q[0], q[2], 4;

	h q[2];
	crk q[1], q[2], 2;
	crk q[0], q[2], 3;

	h q[1];
	crk q[0], q[1], 2;

	h q[0];

	swap q[0], q[2];
}


# Psi_prep
x q[5];

h q[0];
h q[1];
h q[2];
h q[3];
h q[4];

# control_register[0]
cswap q[0], q[5], q[6];
cswap q[0], q[6], q[7];
cswap q[0], q[7], q[8];


# control_register[1]
cswap q[1], q[5], q[6];
cswap q[1], q[6], q[7];
cswap q[1], q[7], q[8];

cswap q[1], q[5], q[6];
cswap q[1], q[6], q[7];
cswap q[1], q[7], q[8];


# control_register[2]
cswap q[2], q[5], q[6];
cswap q[2], q[6], q[7];
cswap q[2], q[7], q[8];

cswap q[2], q[5], q[6];;
cswap q[2], q[6], q[7];
cswap q[2], q[7], q[8];

cswap q[2], q[5], q[6];
cswap q[2], q[6], q[7];
cswap q[2], q[7], q[8];

cswap q[2], q[5], q[6];
cswap q[2], q[6], q[7];
cswap q[2], q[7], q[8];

# control_register[3]
cswap q[3], q[5], q[6];
cswap q[3], q[6], q[7];
cswap q[3], q[7], q[8];

cswap q[3], q[5], q[6];
cswap q[3], q[6], q[7];
cswap q[3], q[7], q[8];

cswap q[3], q[5], q[6];
cswap q[3], q[6], q[7];
cswap q[3], q[7], q[8];

cswap q[3], q[5], q[6];
cswap q[3], q[6], q[7];
cswap q[3], q[7], q[8];

cswap q[3], q[5], q[6];
cswap q[3], q[6], q[7];
cswap q[3], q[7], q[8];

cswap q[3], q[5], q[6];
cswap q[3], q[6], q[7];
cswap q[3], q[7], q[8];

cswap q[3], q[5], q[6];
cswap q[3], q[6], q[7];
cswap q[3], q[7], q[8];

cswap q[3], q[5], q[6];
cswap q[3], q[6], q[7];
cswap q[3], q[7], q[8];



# control_register[4]
cswap q[4], q[5], q[6];
cswap q[4], q[6], q[7];
cswap q[4], q[7], q[8];

cswap q[4], q[5], q[6];
cswap q[4], q[6], q[7];
cswap q[4], q[7], q[8];

cswap q[4], q[5], q[6];
cswap q[4], q[6], q[7];
cswap q[4], q[7], q[8];

cswap q[4], q[5], q[6];
cswap q[4], q[6], q[7];
cswap q[4], q[7], q[8];

cswap q[4], q[5], q[6];
cswap q[4], q[6], q[7];
cswap q[4], q[7], q[8];

cswap q[4], q[5], q[6];
cswap q[4], q[6], q[7];
cswap q[4], q[7], q[8];

cswap q[4], q[5], q[6];
cswap q[4], q[6], q[7];
cswap q[4], q[7], q[8];

cswap q[4], q[5], q[6];
cswap q[4], q[6], q[7];
cswap q[4], q[7], q[8];

cswap q[4], q[5], q[6];
cswap q[4], q[6], q[7];
cswap q[4], q[7], q[8];

cswap q[4], q[5], q[6];
cswap q[4], q[6], q[7];
cswap q[4], q[7], q[8];

cswap q[4], q[5], q[6];
cswap q[4], q[6], q[7];
cswap q[4], q[7], q[8];

cswap q[4], q[5], q[6];
cswap q[4], q[6], q[7];
cswap q[4], q[7], q[8];

cswap q[4], q[5], q[6];
cswap q[4], q[6], q[7];
cswap q[4], q[7], q[8];

cswap q[4], q[5], q[6];
cswap q[4], q[6], q[7];
cswap q[4], q[7], q[8];

cswap q[4], q[5], q[6];
cswap q[4], q[6], q[7];
cswap q[4], q[7], q[8];

cswap q[4], q[5], q[6];
cswap q[4], q[6], q[7];
cswap q[4], q[7], q[8];


# Inverse QFT
qft5i();

