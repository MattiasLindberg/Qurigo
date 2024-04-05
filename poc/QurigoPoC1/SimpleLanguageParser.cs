using System;

public class SimpleLanguageParser
{
    public void Parse(string input)
    {
        // Split the input into lines
        string[] lines = input.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

        foreach (var line in lines)
        {
            // Split each line into tokens
            string[] tokens = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            // Process the tokens
            if (tokens.Length >= 2) // Ensure there's at least a command and one parameter
            {
                string command = tokens[0];
                string param1 = tokens[1];
                string param2 = tokens.Length > 2 ? tokens[2] : null; // Third token is optional

                // Handle the command (example)
                HandleCommand(command, param1, param2);
            }
        }
    }

    private void HandleCommand(string command, string param1, string? param2)
    {
        // Example handling logic
        Console.WriteLine($"Command: {command}, Param1: {param1}, Param2: {param2}");

        // Add your command handling logic here
        // This could involve a switch statement on the command,
        // and then doing something with the parameters.
    }
}