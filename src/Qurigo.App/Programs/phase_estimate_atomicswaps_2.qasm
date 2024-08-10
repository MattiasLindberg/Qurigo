OPENQASM 3;

# 0, 1, 2, 3    : control registers
# 4, 5, 6, 7 : target registers
qubit[6] q;

def qft2i() {
	h q[1];
	crk q[1], q[0], 2;

	h q[0];

	swap q[0], q[1];
}


# Psi_prep
x q[2];

h q[0];
h q[1];

# control_register[0]
cswap q[0], q[2], q[3];
cswap q[0], q[3], q[4];
cswap q[0], q[4], q[5];

# control_register[1]
cswap q[1], q[2], q[3];
cswap q[1], q[3], q[4];
cswap q[1], q[4], q[5];

cswap q[1], q[2], q[3];
cswap q[1], q[3], q[4];
cswap q[1], q[4], q[5];

qft2i();
