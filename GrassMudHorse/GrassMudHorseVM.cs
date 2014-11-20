using System;
using System.Collections.Generic;
using System.Text;
using System.Numeric;
using System.Collections;

namespace GrassMudHorse
{
    class GrassMudHorseVM
    {
        static char tok_grass = '张', tok_mud = '全', tok_horse = '蛋';
        static int HEAP_MAX_LEN = 65536;

        delegate void instrFunc(GrassMudHorseVM vm);
        
        static void instrPush(GrassMudHorseVM vm)
        {
            vm.ReadNumber();
            //Console.WriteLine("push number " + vm.number);
            vm.stack.Push(vm.number);
        }

        static void instrDuplicate(GrassMudHorseVM vm)
        {
            //Console.WriteLine("Duplicate " + (int)vm.stack.Peek());
            vm.stack.Push(vm.stack.Peek());
        }

        static void instrCopynItem(GrassMudHorseVM vm)
        {
            vm.ReadNumber();
            vm.stack.Push(vm.stack[vm.number]);
        }

        static void instrSwap(GrassMudHorseVM vm)
        {
            int a = (int)vm.stack.Pop();
            int b = (int)vm.stack.Pop();
            vm.stack.Push(a);
            vm.stack.Push(b);
        }

        static void instrDiscard(GrassMudHorseVM vm)
        {
            vm.stack.Pop();
        }

        static void instrSlide(GrassMudHorseVM vm)
        {
            vm.ReadNumber();
            for (int i = 0; i < vm.number; i++)
                vm.stack.Pop();
        }

        static void instrAdd(GrassMudHorseVM vm)
        {
            int a = (int)vm.stack.Pop();
            int b = (int)vm.stack.Pop();
            vm.stack.Push(a + b);
        }

        static void instrSub(GrassMudHorseVM vm)
        {
            int a = (int)vm.stack.Pop();
            int b = (int)vm.stack.Pop();
            vm.stack.Push(b - a);
        }

        static void instrMul(GrassMudHorseVM vm)
        {
            int a = (int)vm.stack.Pop();
            int b = (int)vm.stack.Pop();
            vm.stack.Push(b * a);
        }

        static void instrDiv(GrassMudHorseVM vm)
        {
            int a = (int)vm.stack.Pop();
            int b = (int)vm.stack.Pop();
            vm.stack.Push(b / a);
        }

        static void instrMod(GrassMudHorseVM vm)
        {
            int a = (int)vm.stack.Pop();
            int b = (int)vm.stack.Pop();
            vm.stack.Push(b % a);
        }

        static void instrStore(GrassMudHorseVM vm)
        {
            int val = (int)vm.stack.Pop();
            int idx = (int)vm.stack.Pop();
            vm.heap[idx] = val;
        }

        static void instrRetrieve(GrassMudHorseVM vm)
        {
            int idx = (int)vm.stack.Pop();
            vm.stack.Push(vm.heap[idx]);
        }

        static void instrMark(GrassMudHorseVM vm)
        {
            vm.ReadNumber();
            vm.labels.Add(vm.number, vm.pc);
        }

        static void instrCall(GrassMudHorseVM vm)
        {
            vm.ReadNumber();
            vm.frame.Push(vm.pc);
            vm.pc = (int)vm.labels[vm.number];
            //vm.pc--;
        }

        static void instrJump(GrassMudHorseVM vm)
        {
            vm.ReadNumber();
            Console.WriteLine("[NOTE]: " + vm.number);
            vm.pc = (int)vm.labels[vm.number];
            //vm.pc--;
        }

        static void instrJumpZ(GrassMudHorseVM vm)
        {
            if ((int)vm.stack.Pop() == 0)
            {
                vm.ReadNumber();
                vm.pc = (int)vm.labels[vm.number];
                //vm.pc--;
            }
        }

        static void instrJumpN(GrassMudHorseVM vm)
        {
            if ((int)vm.stack.Pop() < 0)
            {
                vm.ReadNumber();
                vm.pc = (int)vm.labels[vm.number];
                //vm.pc--;
            }
        }

        static void instrRet(GrassMudHorseVM vm)
        {
            vm.pc = vm.frame.Pop();
            //vm.pc--;
        }

        static void instrHalt(GrassMudHorseVM vm)
        {
            vm.isRunning = false;
        }

        static void instrPrintChar(GrassMudHorseVM vm)
        {
            int n = (int)vm.stack.Pop();
            Console.Write((char)n);
        }

        static void instrPrintInt(GrassMudHorseVM vm)
        {
            //Console.WriteLine("Print integer ");
            Console.WriteLine((int)vm.stack.Pop());
        }

        static void instrReadChar(GrassMudHorseVM vm)
        {
            vm.stack.Push(Console.Read());
        }

        static void instrReadInt(GrassMudHorseVM vm)
        {
            vm.stack.Push(Convert.ToInt32(Console.ReadLine()));
        }

        IDictionary<string, instrFunc> instrTable;

        public int[] heap;
        public Hashtable labels;
        public GMHStack stack;
        public Stack<int> frame;
        public int pc = 0;
        string prog;
        public int number;
        public bool isRunning;

        public GrassMudHorseVM()
        {
            stack = new GMHStack();
            frame = new Stack<int>();
            heap = new int[HEAP_MAX_LEN];
            labels = new Hashtable();

      
            instrTable = new Dictionary<string, instrFunc>();
            instrTable.Add("张张", new instrFunc(instrPush));
            instrTable.Add("张蛋张", new instrFunc(instrDuplicate));
            instrTable.Add("张全张", new instrFunc(instrCopynItem));
            instrTable.Add("张蛋全", new instrFunc(instrSwap));
            instrTable.Add("张蛋蛋", new instrFunc(instrDiscard));
            instrTable.Add("张全蛋", new instrFunc(instrSlide));

            instrTable.Add("全张张张", new instrFunc(instrAdd));
            instrTable.Add("全张张全", new instrFunc(instrSub));
            instrTable.Add("全张张蛋", new instrFunc(instrMul));
            instrTable.Add("全张全张", new instrFunc(instrDiv));
            instrTable.Add("全张全全", new instrFunc(instrMod));

            instrTable.Add("全全张", new instrFunc(instrStore));
            instrTable.Add("全全全", new instrFunc(instrRetrieve));

            instrTable.Add("蛋张张", new instrFunc(instrMark));
            instrTable.Add("蛋张全", new instrFunc(instrCall));
            instrTable.Add("蛋张蛋", new instrFunc(instrJump));
            instrTable.Add("蛋全张", new instrFunc(instrJumpZ));
            instrTable.Add("蛋全全", new instrFunc(instrJumpN));
            instrTable.Add("蛋全蛋", new instrFunc(instrRet));
            instrTable.Add("蛋蛋蛋", new instrFunc(instrHalt));


            instrTable.Add("全蛋张张", new instrFunc(instrPrintChar));
            instrTable.Add("全蛋张全", new instrFunc(instrPrintInt));
            instrTable.Add("全蛋全张", new instrFunc(instrReadChar));
            instrTable.Add("全蛋全全", new instrFunc(instrReadInt));
        }
        
        

        public void Load(string s)
        {
            prog = s;
            Rest();
        }

        public void Rest()
        {
            stack = new  GMHStack();
            frame = new Stack<int>();
            heap = new int[HEAP_MAX_LEN];
            labels = new Hashtable();
            number = 0;
            pc = 0;
        }
        
        public void ReadNumber()
        {
            number = 0;
            bool negative = false;
            while (prog[pc] != tok_grass && prog[pc] != tok_mud && prog[pc] != tok_horse)
                pc++;
            if (prog[pc] == tok_grass)
                negative = false;
            else if (prog[pc] == tok_mud)
                negative = true;
            pc++;
            for (; prog[pc] != tok_horse; pc++) {
                if (prog[pc] == tok_grass || prog[pc] == tok_mud) {
                    number = number << 1;
                    number = number | ((prog[pc] == tok_mud) ? 1 : 0);
                }
            }
            if (negative)
                number = -number;
            Console.Write(number);
        }

        public void Run()
        {
            string instr = "";
            instrFunc curInstr;
            char ch;
            isRunning = true;
            for (; pc < prog.Length; pc++){
                if (isRunning == false)
                    break;
                ch = prog[pc];
                if (ch == tok_grass || ch == tok_mud || ch == tok_horse) {
                    instr += ch;
                    if (instrTable.ContainsKey(instr))
                    {
                        //Console.Write("Execute " + instr + " ");
                        curInstr = instrTable[instr];
                        try
                        {
                            curInstr(this);
                        }
                        catch (ArgumentOutOfRangeException e)
                        {
                            Console.WriteLine("[Error]: {0}", e.Message);
                        }
                        Console.WriteLine();
                        //Console.WriteLine("After execute " + instr + " pc = " + pc);
                        instr = "";
                    }
                }
            }
        }
        
    }
}
