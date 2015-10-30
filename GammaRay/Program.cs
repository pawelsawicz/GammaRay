using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace GammaRay
{
    class Program
    {
        static void Main(string[] args) 
        {
            var mutateRunner = new MutateRunner();

            if (!File.Exists(args[0]))
            {
                Console.WriteLine("File doesnt exist");
                return;
            }
            ModuleDefinition moduleDefinition = ModuleDefinition.ReadModule(args[0]);

            foreach (var s in moduleDefinition.Types) // -> programs
            {
                foreach (var method in s.Methods) // -> function
                {
                    Console.WriteLine(method.Name);
                    foreach (var instruction in method.Body.Instructions) // --> mutate instruction 
                    {
                        mutateRunner.Mutate(instruction);
                    }
                }
            }
            moduleDefinition.Write(@"D:\MutationTalk\MutationTalk\bin\Debug\MutationTalkMutated.dll");
            Console.WriteLine("Assembly mutated and saved");
            Console.ReadLine();
        }
    }

    public class MutateRunner
    {
        public void Mutate(Instruction instruction)
        {
            //negations of conditinals
            if (instruction.OpCode == OpCodes.Brtrue_S)
            {
                instruction.OpCode = OpCodes.Brfalse_S;
                Console.WriteLine(instruction);
            }
            else if (instruction.OpCode == OpCodes.Brfalse_S)
            {
                instruction.OpCode = OpCodes.Brtrue_S;
                Console.WriteLine(instruction);
            }

            //arithmetics operators
            else if (instruction.OpCode == OpCodes.Add)
            {
                instruction.OpCode = OpCodes.Sub;
            }

            else if (instruction.OpCode == OpCodes.Sub)
            {
                instruction.OpCode = OpCodes.Add;
            }

            else if (instruction.OpCode == OpCodes.Mul)
            {
                instruction.OpCode = OpCodes.Div;
            }

            else if (instruction.OpCode == OpCodes.Div)
            {
                instruction.OpCode = OpCodes.Mul;
            }
        }
    }
}
