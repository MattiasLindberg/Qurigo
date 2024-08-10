OPENQASM 3;

# 0    : control registers
# 1, 2: target registers
qubit[3] q;

# Psi_prep
h q[0];
x q[2];

cswap q[0], q[1], q[2];
