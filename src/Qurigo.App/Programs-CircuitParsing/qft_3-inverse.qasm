qreg q[3];

x q[0];
x q[1];
x q[2];

h q[2];
crk q[1], q[2], 2
crk q[0], q[2], 3

h q[1];
crk q[0], q[1], 2

h q[0];

swap q[0], q[2];

