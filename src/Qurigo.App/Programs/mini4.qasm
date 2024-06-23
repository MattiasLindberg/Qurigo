qubit[4] q;

def c_amod15(int argument) {
    if (argument == 2) {
        swap q[2], q[3];
        swap q[1], q[2];
        swap q[0], q[1];
    }

    if (argument == 13) {
        swap q[2], q[3];
        swap q[1], q[2];
        swap q[0], q[1];
    }

    if (argument == 7) {
        swap q[0], q[1];
        swap q[1], q[2];
        swap q[2], q[3];
    }

    if (argument == 8) {
        swap q[0], q[1];
        swap q[1], q[2];
        swap q[2], q[3];
    }

    if (argument == 4) {
        swap q[1], q[3];
        swap q[0], q[2];
    }

    if (argument == 11) {
        swap q[1], q[3];
        swap q[0], q[2];
    }

    if (argument == 7) {
        x q[0];
        x q[1];
        x q[2];
        x q[3];
    }

    if (argument == 11) {
        x q[0];
        x q[1];
        x q[2];
        x q[3];
    }

    if (argument == 13) {
        x q[0];
        x q[1];
        x q[2];
        x q[3];
    }
}

x q[0];
x q[2];

c_amod15(2);
