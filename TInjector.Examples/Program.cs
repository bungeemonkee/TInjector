﻿// TInjector: TInjector.Examples
// Program.cs
// Created: 2015-10-17 5:54 PM
// Modified: 2015-10-18 11:32 AM

using System;
using TInjector.Examples.Simple;

namespace TInjector.Examples
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            SimpleMain.SimpleMain_01();

            Console.Write("Complete");
            Console.Read();
        }
    }
}