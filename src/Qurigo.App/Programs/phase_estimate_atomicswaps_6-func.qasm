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

def cswapHelper(int control, int repeat) {
	int index = 0;
	
	while(index < repeat) {
		cswap q[control], q[6], q[7];
		cswap q[control], q[7], q[8];
		cswap q[control], q[8], q[9];

		index = index + 1;
	}
})


# Put each of the control qubits in a superposition
h q[0];
h q[1];
h q[2];
h q[3];
h q[4];
h q[5];

# Prepare the target qubits
x q[6];

cswapHelper(0, 1);
cswapHelper(1, 2);
cswapHelper(2, 4);
cswapHelper(3, 8);
cswapHelper(4, 16);
cswapHelper(5, 32);


# Inverse QFT
qft6i();

