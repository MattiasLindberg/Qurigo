qreg q[2];

# Match with 10.py

# x q[0];
# x q[1];

h q[1];
crk q[0], q[1], 2

h q[0];

swap q[0], q[1];