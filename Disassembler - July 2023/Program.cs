using System;
using System.Collections.Generic;

namespace Disassembler___July_2023
{
    class Program
    {
        static Dictionary<byte, string> Assembly = new Dictionary<byte, string>()
        {
            [0x00] = "NOP",

            [0x10] = "ADD",
            [0x11] = "SUB",
            [0x12] = "MUL",
            [0x13] = "DIV",
            [0x14] = "MOD",

            [0x20] = "NOT",
            [0x21] = "AND",
            [0x22] = "OR",
            [0x23] = "XOR",
            [0x24] = "EQ",
            [0x25] = "NEQ",
            [0x26] = "GTE",
            [0x27] = "LTE",
            [0x28] = "GT",
            [0x29] = "LT",

            [0x30] = "JMP",
            [0x31] = "JMPi",
            [0x32] = "JMPT",
            [0x33] = "JMPi",
            
            [0x40] = "SET",
            [0x41] = "COPY",
            [0x42] = "LOAD",
            [0x43] = "LOADi",
            [0x44] = "STR",
            [0x45] = "STRi",
            [0x46] = "PUSH",
            [0x47] = "POP"
        };
        static Dictionary<byte, string> Registers = new Dictionary<byte, string>()
        {
            [0x00] = "R0",
            [0x01] = "R1",
            [0x02] = "R2",
            [0x03] = "R3",
            [0x04] = "R4",
            [0x05] = "R5",
            [0x06] = "R6",
            [0x07] = "R7"
        };

        static string[] ByteToAssembly(byte[] byteLine)
        {
            string assembly = Assembly[byteLine[0]];
            string pad = "00";
            switch (assembly)
            {
                case "NOP":
                    return new string[] { assembly, pad, pad, pad };
                
                case "ADD":
                case "SUB":
                case "MUL":
                case "DIV":
                case "MOD":
                case "AND":
                case "OR":
                case "XOR":
                case "EQ":
                case "NEQ":
                case "GTE":
                case "LTE":
                case "GT":
                case "LT":
                    return new string[] { assembly, Registers[byteLine[1]], Registers[byteLine[2]], Registers[byteLine[3]] };
                case "NOT":
                    return new string[] { assembly, Registers[byteLine[1]], Registers[byteLine[2]], pad };

                //case "JMP":
                //    return new string[] { assembly, , , pad}; 
                //case "JMPT":
                //case "STR":
                //    return new string[] { assembly, , , Registers[byteLine[3]]};

                case "JMPTi":
                case "COPY":
                case "LOADi":
                case "STRi":
                    return new string[] { assembly, Registers[byteLine[1]], Registers[byteLine[2]], pad };

                case "SET":
                    byte valueLowByte = byteLine[3];
                    byte valueHighByte = (byte)(byteLine[2] << 8);
                    byte value = (byte)(valueHighByte | valueLowByte);
                    return new string[] { assembly, Registers[byteLine[1]], value.ToString() };
                //case "LOAD":
                //    return new string[] { assembly, Registers[byteLine[1]], , };

                case "JMPi":
                case "PUSH":
                case "POP":
                    return new string[] { assembly, Registers[byteLine[1]], pad, pad};

                default: throw new Exception("Invaild Machine Code :(");
            }
        }
        static void Main(string[] args)
        {
            byte[] Bytes = System.IO.File.ReadAllBytes(args[0]);
            List<string> assembledLines = new List<string>();
            List<byte> byteLine = new List<byte>();


            for (int aByte = 0; aByte < Bytes.Length; aByte++)
            {
                byteLine.Add(Bytes[aByte]);
                if((aByte + 1) % 4 == 0)
                {
                    string instruction = "";
                    string[] assemblyLine = ByteToAssembly(byteLine.ToArray());
                    for(int i = 0; i < assemblyLine.Length; i++)
                    {
                        instruction += assemblyLine[i];
                        instruction += " ";
                    }
                    Console.WriteLine(instruction);
                    assembledLines.Add(instruction);
                    byteLine.Clear();
                }
            }
        }
    }
}
