OPENQASM 3;

# 0, 1, 2    : control registers
# 3, 4, 5, 6 : target registers
qubit[7] q;

def qft3i() {
	h q[2];
	crk q[1], q[2], 2;
	crk q[0], q[2], 3;

	h q[1];
	crk q[0], q[1], 2;

	h q[0];

	swap q[0], q[2];
}


# Psi_prep
x q[3];

h q[0];
h q[1];
h q[2];

# control_register[0]
cswap q[0], q[3], q[4];
cswap q[0], q[4], q[5];
cswap q[0], q[5], q[6];


# control_register[1]
cswap q[1], q[3], q[4];
cswap q[1], q[4], q[5];
cswap q[1], q[5], q[6];

cswap q[1], q[3], q[4];
cswap q[1], q[4], q[5];
cswap q[1], q[5], q[6];


# control_register[2]
cswap q[2], q[3], q[4];
cswap q[2], q[4], q[5];
cswap q[2], q[5], q[6];

cswap q[2], q[3], q[4];
cswap q[2], q[4], q[5];
cswap q[2], q[5], q[6];

cswap q[2], q[3], q[4];
cswap q[2], q[4], q[5];
cswap q[2], q[5], q[6];

cswap q[2], q[3], q[4];
cswap q[2], q[4], q[5];
cswap q[2], q[5], q[6];


# Inverse QFT
qft3i();

# Measure control registers (0, 1, 2)
