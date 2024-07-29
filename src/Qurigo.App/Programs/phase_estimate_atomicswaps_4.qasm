OPENQASM 3;

# 0, 1, 2, 3    : control registers
# 4, 5, 6, 7 : target registers
qubit[8] q;

def qft4i() {
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

	swap q[0], q[3];
	swap q[1], q[2];
}


# Psi_prep
x q[4];

h q[0];
h q[1];
h q[2];
h q[3];

# control_register[0]
cswap q[0], q[4], q[5];
cswap q[0], q[5], q[6];
cswap q[0], q[6], q[7];


# control_register[1]
cswap q[1], q[4], q[5];
cswap q[1], q[5], q[6];
cswap q[1], q[6], q[7];

cswap q[1], q[4], q[5];
cswap q[1], q[5], q[6];
cswap q[1], q[6], q[7];


# control_register[2]
#cswap q[2], q[4], q[5];
#cswap q[2], q[5], q[6];
#cswap q[2], q[6], q[7];

#cswap q[2], q[4], q[5];
#cswap q[2], q[5], q[6];
#cswap q[2], q[6], q[7];

#cswap q[2], q[4], q[5];
#cswap q[2], q[5], q[6];
#cswap q[2], q[6], q[7];

#cswap q[2], q[4], q[5];
#cswap q[2], q[5], q[6];
#cswap q[2], q[6], q[7];

# control_register[3]
#cswap q[3], q[4], q[5];
#cswap q[3], q[5], q[6];
#cswap q[3], q[6], q[7];

#cswap q[3], q[4], q[5];
#cswap q[3], q[5], q[6];
#cswap q[3], q[6], q[7];

#cswap q[3], q[4], q[5];
#cswap q[3], q[5], q[6];
#cswap q[3], q[6], q[7];

#cswap q[3], q[4], q[5];
#cswap q[3], q[5], q[6];
#cswap q[3], q[6], q[7];

#cswap q[3], q[4], q[5];
#cswap q[3], q[5], q[6];
#cswap q[3], q[6], q[7];

#cswap q[3], q[4], q[5];
#cswap q[3], q[5], q[6];
#cswap q[3], q[6], q[7];

#cswap q[3], q[4], q[5];
#cswap q[3], q[5], q[6];
#cswap q[3], q[6], q[7];

#cswap q[3], q[4], q[5];
#cswap q[3], q[5], q[6];
#cswap q[3], q[6], q[7];


# Inverse QFT
#qft4i();

# Measure control registers (0, 1, 2)
