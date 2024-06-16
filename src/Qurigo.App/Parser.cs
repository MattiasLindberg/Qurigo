using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qurigo.App;
internal class Parser
{
    public IDictionary<string, SubroutineNode> Subroutines = new Dictionary<string, SubroutineNode>();
    public IDictionary<string, string> Parameters = new Dictionary<string, string>();
    public IList<IExecutionNode> Nodes = new List<IExecutionNode>();

    public void Parse(Tokenizer tokenizer)
    {
        Nodes = ParseNodes(tokenizer, TokenType.EndOfFile);
    }

    private IList<IExecutionNode> ParseNodes(Tokenizer tokenizer, TokenType endToken)
    {
        IList<IExecutionNode> nodes = new List<IExecutionNode>();

        Token token = tokenizer.NextToken();
        while (token.Type != endToken)
        {
            // Console.WriteLine(token.Type + " : " + token.Value);

            switch(token.Type)
            {
                case TokenType.OpenQASM:
                    tokenizer.ReadUntilSemicolon();
                    break;

                case TokenType.Identifier:
                    if(Subroutines.ContainsKey(token.Value))
                    {
                        SubroutineNode subroutineNode = Subroutines[token.Value];
                        nodes.Add(subroutineNode);
                    }
                    else if(Parameters.ContainsKey(token.Value))
                    {
                        // Noop
                    }
                    else
                    {
                        throw new Exception($"Unknown identifier {token.Value}.");
                    }
                    break;

                case TokenType.Subroutine:
                    SubroutineNode subroutine = new SubroutineNode();

                    token = tokenizer.NextToken(TokenType.Identifier);
                    subroutine.Name = token.Value;

                    token = tokenizer.NextToken(TokenType.LeftParenthesis);

                    token = tokenizer.NextToken();
                    if (token.Type == TokenType.RightParenthesis)
                    { 
                        // signature is done
                    }
                    else
                    {
                        if (token.Type != TokenType.Identifier)
                        {
                            throw new Exception($"Expected Identifier (datatype) but was {token.Type}, {token.Value}.");
                        }

                        token = tokenizer.NextToken(TokenType.Identifier);
                        Parameters.Add(token.Value, token.Value);

                        token = tokenizer.NextToken(TokenType.RightParenthesis);
                    }

                    token = tokenizer.NextToken(TokenType.LeftBrace);

                    //token = tokenizer.NextToken();
                    subroutine.Nodes = ParseNodes(tokenizer, TokenType.RightBrace);
                    Console.WriteLine("Subroutine parsed: " + subroutine.Name);

                    // Save subroutine for later use
                    Subroutines.Add(subroutine.Name, subroutine);
                    break;

                case TokenType.Qubit:
                    QubitNode qubitNode = new QubitNode();

                    token = tokenizer.NextToken();
                    if(token.Type == TokenType.LeftBracket)
                    {
                        // Qubit array, if no [] then it's a single qubit
                        token = tokenizer.NextToken(TokenType.Number);
                        qubitNode.IsArray = true;
                        qubitNode.Size = int.Parse(token.Value);

                        token = tokenizer.NextToken(TokenType.RightBracket);

                        token = tokenizer.NextToken(TokenType.Identifier);
                        qubitNode.Name = token.Value;
                    }
                    else if (token.Type == TokenType.Identifier)
                    {
                        qubitNode.Name = token.Value;
                    }
                    else
                    {
                        throw new Exception($"Expected identifier or left bracket but was {token.Type}, {token.Value}.");
                    }

                    token = tokenizer.NextToken();
                    if (token.Type != TokenType.SemiColon)
                    {
                        throw new Exception($"Expected semicolon but was {token.Type}, {token.Value}.");
                    }

                    nodes.Add(qubitNode);
                    break;

                case TokenType.Gate:
                    switch(token.GateType)
                    {
                        // Single qubit gates
                        case Interfaces.GateNames.X:
                        case Interfaces.GateNames.Y:
                        case Interfaces.GateNames.Z:
                        case Interfaces.GateNames.H:
                        case Interfaces.GateNames.T:
                            var gateOneQubit = new GateNode
                            {
                                GateType = token.GateType,
                                Parameters = new List<Parameter>()
                            };
                            nodes.Add(gateOneQubit);

                            token = tokenizer.NextToken(TokenType.Identifier);
                            var param1 = new Parameter
                            {
                                Name = token.Value,
                                Type = "qubit"
                            };
                            gateOneQubit.Parameters.Add(param1);

                            token = tokenizer.NextToken(TokenType.LeftBracket);

                            token = tokenizer.NextToken(TokenType.Number);
                            param1.index = int.Parse(token.Value);

                            token = tokenizer.NextToken(TokenType.RightBracket);

                            token = tokenizer.NextToken(TokenType.SemiColon);
                            break;

                        // Two qubit gates
                        case Interfaces.GateNames.CNOT:
                        case Interfaces.GateNames.SWAP:
                            var gateTwoQubits = new GateNode
                            {
                                GateType = token.GateType,
                                Parameters = new List<Parameter>()
                            };
                            nodes.Add(gateTwoQubits);

                            token = tokenizer.NextToken(TokenType.Identifier);
                            var param2 = new Parameter
                            {
                                Name = token.Value,
                                Type = "qubit"
                            };
                            gateTwoQubits.Parameters.Add(param2);

                            token = tokenizer.NextToken(TokenType.LeftBracket);

                            token = tokenizer.NextToken(TokenType.Number);
                            param2.index = int.Parse(token.Value);

                            token = tokenizer.NextToken(TokenType.RightBracket);

                            token = tokenizer.NextToken();
                            if (token.Value != ",")
                            {
                                throw new Exception($"Expected ',' but was {token.Type}, {token.Value}.");
                            }

                            token = tokenizer.NextToken(TokenType.Identifier);
                            param2 = new Parameter
                            {
                                Name = token.Value,
                                Type = "qubit"
                            };
                            gateTwoQubits.Parameters.Add(param2);

                            token = tokenizer.NextToken(TokenType.LeftBracket);

                            token = tokenizer.NextToken(TokenType.Number);
                            param2.index = int.Parse(token.Value);

                            token = tokenizer.NextToken(TokenType.RightBracket);

                            token = tokenizer.NextToken(TokenType.SemiColon);
                            break;
                    }

                    break;

                case TokenType.If:
                    // if ( expr ) { nodes }

                    IfNode ifNode = new IfNode();
                    ifNode.Condition = new Expression();
                    nodes.Add(ifNode);

                    token = tokenizer.NextToken(TokenType.LeftParenthesis);

                    token = tokenizer.NextToken(TokenType.Identifier);
                    ifNode.Condition.Variable = token.Value;

                    token = tokenizer.NextToken(TokenType.Operator);
                    ifNode.Condition.Operator = token.Value;

                    token = tokenizer.NextToken(TokenType.Number);
                    ifNode.Condition.Value = token.Value;

                    token = tokenizer.NextToken(TokenType.RightParenthesis);

                    token = tokenizer.NextToken(TokenType.LeftBrace);

                    ifNode.IfNodes = ParseNodes(tokenizer, TokenType.RightBrace);

                    break;

                case TokenType.While:
                    // while( expr ) { nodes }

                    WhileNode whileNode = new WhileNode();
                    whileNode.Condition = new Expression();
                    nodes.Add(whileNode);

                    token = tokenizer.NextToken(TokenType.LeftParenthesis);

                    token = tokenizer.NextToken(TokenType.Identifier);
                    whileNode.Condition.Variable = token.Value;

                    token = tokenizer.NextToken(TokenType.Operator);
                    whileNode.Condition.Operator = token.Value;

                    token = tokenizer.NextToken(TokenType.Number);
                    whileNode.Condition.Value = token.Value;

                    token = tokenizer.NextToken(TokenType.RightParenthesis);

                    token = tokenizer.NextToken(TokenType.LeftBrace);

                    whileNode.Nodes = ParseNodes(tokenizer, TokenType.RightBrace);

                    break;
            }

            token = tokenizer.NextToken();
        }

        return nodes;
    }
}
