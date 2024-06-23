OPENQASM 3;

qubit[3] q;

x q[0];
cnot q[0], q[1];
swap q[0], q[2];
