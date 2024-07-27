﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qurigo.Circuit.BaseCircuit;

public class Parser
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
                        CallSubroutineNode callSubroutineNode = new CallSubroutineNode
                        {
                            FunctionParameters = new List<FunctionParameter>(),
                            Subroutine = subroutineNode
                        };
                        nodes.Add(callSubroutineNode);

                        // Only supporting 0 or 1 arguments
                        if (subroutineNode.Arguments.Count == 1)
                        {
                            // Parse argument
                            token = tokenizer.NextToken(TokenType.LeftParenthesis);
                            token = tokenizer.NextToken();

                            if(token.Type == TokenType.Number)
                            {
                                callSubroutineNode.FunctionParameters.Add(new FunctionParameter { Name = subroutineNode.Arguments[0].Name, Value = int.Parse(token.Value) });
                                token = tokenizer.NextToken(TokenType.RightParenthesis);
                            }
                            else if(token.Type == TokenType.Identifier)
                            {
                                token = tokenizer.NextToken(TokenType.LeftBracket);

                                token = tokenizer.NextToken();
                                // HARD CODED: GET A  PROPER VALUE!
                                callSubroutineNode.FunctionParameters.Add(new FunctionParameter { Name = subroutineNode.Arguments[0].Name, Value = 1 });

                                token = tokenizer.NextToken(TokenType.RightBracket);
                                token = tokenizer.NextToken(TokenType.RightParenthesis);
                            }
                            else
                            {
                                throw new Exception($"Expected number or identifier but was {token.Type}, {token.Value}.");
                            }

                        }
                        else
                        {
                            token = tokenizer.NextToken(TokenType.LeftParenthesis);
                            token = tokenizer.NextToken(TokenType.RightParenthesis);
                        }
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
                    SubroutineNode subroutine = new SubroutineNode() { Arguments = new List<Argument>() };

                    token = tokenizer.NextToken(TokenType.Identifier);
                    subroutine.Name = token.Value;

                    token = tokenizer.NextToken(TokenType.LeftParenthesis);

                    token = tokenizer.NextToken();
                    if (token.Type == TokenType.RightParenthesis)
                    {
                        // def xyz() {

                        // signature is done
                    }
                    else
                    {
                        // def xyz(int arg1) {

                        if (token.Type != TokenType.Identifier)
                        {
                            throw new Exception($"Expected Identifier (datatype) but was {token.Type}, {token.Value}.");
                        }

                        // Current token is a datatype
                        // Next token is parameter name
                        string argumentType = token.Value;

                        token = tokenizer.NextToken(TokenType.Identifier);
                        subroutine.Arguments.Add(new Argument { Name = token.Value, Type = argumentType });

                        token = tokenizer.NextToken(TokenType.RightParenthesis);
                    }

                    token = tokenizer.NextToken(TokenType.LeftBrace);

                    //token = tokenizer.NextToken();
                    subroutine.Nodes = ParseNodes(tokenizer, TokenType.RightBrace);

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

                case TokenType.ControlGate:
                    ControlGateNode controlGateNode = new ControlGateNode();

                    token = tokenizer.NextToken(TokenType.At);

                    token = tokenizer.NextToken(TokenType.Identifier);
                    if (Subroutines.ContainsKey(token.Value))
                    {
                        SubroutineNode subroutineNode = Subroutines[token.Value];
                        CallSubroutineNode callSubroutineNode = new CallSubroutineNode
                        {
                            FunctionParameters = new List<FunctionParameter>(),
                            Subroutine = subroutineNode
                        };
                        controlGateNode.Subroutine = callSubroutineNode;

                        token = tokenizer.NextToken(TokenType.Identifier);
                        token = tokenizer.NextToken(TokenType.LeftBracket);
                        token = tokenizer.NextToken(TokenType.Number);
                        token = tokenizer.NextToken(TokenType.RightBracket);

                        token = tokenizer.NextToken(TokenType.SemiColon);

                        nodes.Add(controlGateNode);
                    }

                    nodes.Add(controlGateNode);
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
                            param1.Index = int.Parse(token.Value);

                            token = tokenizer.NextToken(TokenType.RightBracket);

                            token = tokenizer.NextToken(TokenType.SemiColon);
                            break;

                        // Two qubit gates
                        case Interfaces.GateNames.CX:
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
                            param2.Index = int.Parse(token.Value);

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
                            param2.Index = int.Parse(token.Value);

                            token = tokenizer.NextToken(TokenType.RightBracket);

                            token = tokenizer.NextToken(TokenType.SemiColon);
                            break;

                        // Two qubit gates
                        case Interfaces.GateNames.CSWAP:
                            var gateThreeQubits = new GateNode
                            {
                                GateType = token.GateType,
                                Parameters = new List<Parameter>()
                            };
                            nodes.Add(gateThreeQubits);

                            // Qubit reference 1
                            token = tokenizer.NextToken(TokenType.Identifier);
                            var param4 = new Parameter
                            {
                                Name = token.Value,
                                Type = "qubit"
                            };
                            gateThreeQubits.Parameters.Add(param4);

                            token = tokenizer.NextToken(TokenType.LeftBracket);
                            token = tokenizer.NextToken(TokenType.Number);
                            param4.Index = int.Parse(token.Value);
                            token = tokenizer.NextToken(TokenType.RightBracket);

                            token = tokenizer.NextToken();
                            if (token.Value != ",")
                            {
                                throw new Exception($"Expected ',' but was {token.Type}, {token.Value}.");
                            }

                            // Qubit reference 2
                            token = tokenizer.NextToken(TokenType.Identifier);
                            param4 = new Parameter
                            {
                                Name = token.Value,
                                Type = "qubit"
                            };
                            gateThreeQubits.Parameters.Add(param4);

                            token = tokenizer.NextToken(TokenType.LeftBracket);
                            token = tokenizer.NextToken(TokenType.Number);
                            param4.Index = int.Parse(token.Value);
                            token = tokenizer.NextToken(TokenType.RightBracket);

                            token = tokenizer.NextToken();
                            if (token.Value != ",")
                            {
                                throw new Exception($"Expected ',' but was {token.Type}, {token.Value}.");
                            }

                            // Qubit reference 3
                            token = tokenizer.NextToken(TokenType.Identifier);
                            param4 = new Parameter
                            {
                                Name = token.Value,
                                Type = "qubit"
                            };
                            gateThreeQubits.Parameters.Add(param4);

                            token = tokenizer.NextToken(TokenType.LeftBracket);
                            token = tokenizer.NextToken(TokenType.Number);
                            param4.Index = int.Parse(token.Value);
                            token = tokenizer.NextToken(TokenType.RightBracket);

                            token = tokenizer.NextToken(TokenType.SemiColon);
                            break;

                        // Two qubit gates + number
                        case Interfaces.GateNames.CRK:
                            var gateTwoQubitsAndNumber = new GateNode
                            {
                                GateType = token.GateType,
                                Parameters = new List<Parameter>()
                            };
                            nodes.Add(gateTwoQubitsAndNumber);

                            token = tokenizer.NextToken(TokenType.Identifier);
                            var param3 = new Parameter
                            {
                                Name = token.Value,
                                Type = "qubit"
                            };
                            gateTwoQubitsAndNumber.Parameters.Add(param3);

                            token = tokenizer.NextToken(TokenType.LeftBracket);

                            token = tokenizer.NextToken(TokenType.Number);
                            param3.Index = int.Parse(token.Value);

                            token = tokenizer.NextToken(TokenType.RightBracket);

                            token = tokenizer.NextToken();
                            if (token.Value != ",")
                            {
                                throw new Exception($"Expected ',' but was {token.Type}, {token.Value}.");
                            }

                            token = tokenizer.NextToken(TokenType.Identifier);
                            param3 = new Parameter
                            {
                                Name = token.Value,
                                Type = "qubit"
                            };
                            gateTwoQubitsAndNumber.Parameters.Add(param3);

                            token = tokenizer.NextToken(TokenType.LeftBracket);

                            token = tokenizer.NextToken(TokenType.Number);
                            param3.Index = int.Parse(token.Value);

                            token = tokenizer.NextToken(TokenType.RightBracket);

                            token = tokenizer.NextToken();
                            if (token.Value != ",")
                            {
                                throw new Exception($"Expected ',' but was {token.Type}, {token.Value}.");
                            }

                            token = tokenizer.NextToken(TokenType.Number);
                            param3 = new Parameter
                            {
                                Name = token.Value,
                                Type = "qubit",
                                Index = int.Parse(token.Value)
                            };
                            gateTwoQubitsAndNumber.Parameters.Add(param3);

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
