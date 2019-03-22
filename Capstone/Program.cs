using Capstone;
using System;

namespace capstone
{
    public class Program
    {
        static void Main(string[] args)
        {
            ProgramCLI programCLI = new ProgramCLI();
            programCLI.RunCLI();
        }
    }
}
