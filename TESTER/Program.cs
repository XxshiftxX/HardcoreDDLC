using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using static System.Console;

namespace TESTER
{
    internal class A{}
    class AA : A
    {
        public int i;
    }
    class AB : A{}
    class Program
    {
        private static void Main(string[] args)
        {
            A a = new AA{i = 31};
            switch (a)
            {
                case AB _:
                    WriteLine("It's AB Type");
                    break;
                case AA aa when aa.i > 10:
                    WriteLine($"It's AA ({aa.i})");
                    break;
                case AA aa when aa.i > 20:
                    WriteLine($"It's AA >>{aa.i}<<");
                    break;
                case AA _:
                    WriteLine("It's AA");
                    break;
            }
        }
    }
}
