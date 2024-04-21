﻿namespace Qurigo.Interfaces;

[Flags]
public enum GateNames
{
    X = 1,
    Y = 2,
    Z = 4,
    H = 8,
    CNOT = 16,
    T = 32,
    S = 64,
    SqrtX = 128,
    RX = 256,
    RY = 512,
    RZ = 1024
}
