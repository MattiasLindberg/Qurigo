qreg q[3];

x q[2];

h q[0];
crk q[1], q[0], 2
crk q[2], q[0], 3

h q[1];
crk q[2], q[1], 2

h q[2];
