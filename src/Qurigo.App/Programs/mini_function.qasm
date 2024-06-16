OPENQASM 3;

def qft3i(int arg1) {
	x q[1];

    if (arg1 == 3) {
        x q[0];
    }

    while(arg1 == 2) {
		h q[0];
		h q[1];
	}
}

qft3i();
