using System;

namespace BitmapCompressor.Console.Utilities
{
    public class SimpleConsoleInput : IInputSystem
    {
        public bool PromptYesOrNo()
        {
            ConsoleKeyInfo key;
            do
            {
                System.Console.Write("[y/n]: ");
                key = System.Console.ReadKey();
                System.Console.WriteLine();
            }
            while (key.Key != ConsoleKey.Y && key.Key != ConsoleKey.N);

            return key.Key == ConsoleKey.Y;
        }
    }
}
