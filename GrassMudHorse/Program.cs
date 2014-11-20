using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Numeric;
using System.Linq.Expressions;

namespace GrassMudHorse
{
    class Program
    {

        public static void Main(string[] args)
        {
            GrassMudHorseVM vm = new GrassMudHorseVM();
            Console.WriteLine("Grass-Mud-HorseProgramming Language (v0.0.1)");
            while (true)
            {
                Console.Write("-> ");
                vm.Load(Console.ReadLine());
                //vm.readNumber();
                //Console.WriteLine(vm.number);
                vm.Run();
            }
        }
    }
}
