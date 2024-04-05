﻿namespace QurigoPoC1;

public class OpenQASMParser
{
    public VectorState2 _state = new VectorState2(new double[]
    {
                    1, 0, 0, 0,    0, 0, 0, 0
    });

    public void Parse(string input)
    {
        // Split the input into lines
        string[] lines = input.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

        foreach (var line in lines)
        {
            // Split each line into tokens
            string[] tokens = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            switch (tokens[0])
            {
                case "x":
                    _state.GenericX(int.Parse(tokens[1].Substring(2, 1)));
                    break;
                case "h":
                    _state.Hadamard(int.Parse(tokens[1].Substring(2, 1)));
                    break;
                case "cx":
                    _state.GenericCNOT(int.Parse(tokens[1].Substring(2, 1)), int.Parse(tokens[2].Substring(2, 1)));
                    break;
            }
        }
    }
}