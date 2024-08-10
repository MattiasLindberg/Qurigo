OPENQASM 3;

qubit[3] q;

def abc(int a) {
	if(a == 0) {
		x q[0];
		cx q[0], q[1];
		swap q[0], q[2];
	}
}

abc(1);
