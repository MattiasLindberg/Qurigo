OPENQASM 3;

# 0, 1, 2, 3    : control registers
# 4, 5, 6, 7 : target registers
qubit[8] q;

def qft4i() {
	h q[3];
	crk q[3], q[2], 2;
	crk q[3], q[1], 3;
	crk q[3], q[0], 4;

	h q[2];
	crk q[2], q[1], 2;
	crk q[2], q[0], 3;

	h q[1];
	crk q[1], q[0], 2;

	h q[0];

	swap q[0], q[3];
	swap q[1], q[2];
}

def cswapHelper(int control, int repeat) {
	int index = 0;
	
	while(index < repeat) {
		cswap q[control], q[4], q[5];
		cswap q[control], q[5], q[6];
		cswap q[control], q[6], q[7];

		index = index + 1;
	}
})

# Psi_prep
x q[4];

h q[0];
h q[1];
h q[2];
h q[3];

cswapHelper(0, 1);
cswapHelper(1, 2);
cswapHelper(2, 4);
cswapHelper(3, 8);

# Inverse QFT
qft4i();
