qubit[4] q;

# This should not be here, I think
x q[0];
x q[1];
x q[2];
x q[3];

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

swap q[0], q[2];

