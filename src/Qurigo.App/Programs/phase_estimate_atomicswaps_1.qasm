OPENQASM 3;

# 0, 1, 2, 3    : control registers
# 4, 5, 6, 7 : target registers
qubit[5] q;

def qft1i() {
	h q[0];
}


# Psi_prep
x q[1];

h q[0];

# control_register[0]
cswap q[0], q[1], q[3];
cswap q[0], q[2], q[4];
cswap q[0], q[3], q[5];
