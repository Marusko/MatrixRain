using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace MatrixRain
{
    class Program
    {
        private static readonly int[,] Sets = {{48, 57}, {65, 90}, {48, 90}};

        ///<summary>
        /// Digital Matrix rain from the Matrix movie, homework
        /// </summary>
        /// <param name="directionUp">Direction of falling code up</param>
        /// <param name="color">Color of falling code up</param>
        /// <param name="delaySpeed">Delay speed of falling code in milliseconds</param>
        /// <param name="characters">The set of characters from which the falling code will be generated</param>
        static void Main(bool directionUp = false, ConsoleColor color = ConsoleColor.Green, int delaySpeed = 1, Characters characters = Characters.AlphaNumeric)
        {
            List<Drop> drops = new List<Drop>();
            List<Drop> deleteList = new List<Drop>();
            MatrixRain matrixRain = new MatrixRain();

            Console.Clear();
            Console.CursorVisible = false;

            bool up = directionUp;
            int sleep = delaySpeed;
            int set = (int)characters;

            Console.ForegroundColor = color;
            int counter = 0;
            while (!Console.KeyAvailable)
            {
                matrixRain.MoveDrops(ref counter, ref drops, ref deleteList, up, sleep, Sets[set,0], Sets[set,1], color);
                if ((counter % 2) == 0)
                {
                    matrixRain.GenerateDrops(ref drops, up);
                }
            }

            Console.Clear();
            Console.CursorVisible = true;
            Console.ResetColor();
        }
    }

    enum Characters
    {
        Numeric,
        Alpha,
        AlphaNumeric
    }

    class MatrixRain
    {
        private Random rand = new();
        public void MoveDrops(ref int i, ref List<Drop> drops, ref List<Drop> deleteList, bool up, int sleep, int lower, int upper, ConsoleColor color)
        {
            foreach (var drop in drops)
            {
                Console.SetCursorPosition(drop.XPosition, drop.YPosition);

                int decChar = rand.Next(lower, upper + 1);

                if (!drop.FirstHidden)
                    Console.ForegroundColor = ConsoleColor.White;

                Console.Write((char)decChar);

                if (drop.VisibleCount >= drop.Length)
                    drop.VisibleCount = drop.Length;
                else
                    drop.VisibleCount++;

                Console.ForegroundColor = color;
                
                if (!up)
                {
                    if (drop.VisibleCount > 0)
                    {
                        for (int j = 1; j < drop.VisibleCount; j++)
                        {
                            if (drop.YPosition > 0)
                            {
                                int tmp = drop.YPosition - j;
                                Console.SetCursorPosition(drop.XPosition, tmp);
                                decChar = rand.Next(lower, upper + 1);
                                Console.Write((char)decChar);
                            }
                        }
                    }

                    Thread.Sleep(sleep);

                    if ((drop.YPosition - drop.Length) >= 0)
                    {
                        i = drop.YPosition - drop.Length;
                        Console.SetCursorPosition(drop.XPosition, i);
                        Console.Write(" ");
                    }
                    drop.YPosition++;

                    if (drop.YPosition >= Console.WindowHeight)
                    {
                        drop.YPosition = Console.WindowHeight - 1;
                        drop.Length--;
                        drop.FirstHidden = true;
                        if (drop.Length == -1)
                        {
                            deleteList.Add(drop);
                        }
                    }
                }
                else
                {
                    if (drop.VisibleCount > 0)
                    {
                        for (int j = 1; j < drop.VisibleCount; j++)
                        {
                            if (drop.YPosition < Console.WindowHeight - 1)
                            {
                                int tmp = drop.YPosition + j;
                                Console.SetCursorPosition(drop.XPosition, tmp);
                                decChar = rand.Next(lower, upper + 1);
                                Console.Write((char)decChar);
                            }
                        }
                    }

                    Thread.Sleep(sleep);

                    if ((drop.YPosition + drop.Length) <= Console.WindowHeight - 1)
                    {
                        i = drop.YPosition + drop.Length;
                        Console.SetCursorPosition(drop.XPosition, i);
                        Console.Write(" ");
                    }

                    drop.YPosition--;

                    if (drop.YPosition < 0)
                    {
                        drop.YPosition = 0;
                        drop.Length--;
                        drop.FirstHidden = true;
                        if (drop.Length == -1)
                        {
                            deleteList.Add(drop);
                        }
                    }
                }
            }
            foreach (var drop in deleteList)
            {
                drops.Remove(drop);
            }
            deleteList.Clear();
        }

        public void GenerateDrops(ref List<Drop> drops, bool up)
        {
            Drop drop;
            if (!up)
                drop = new Drop(rand.Next(Console.WindowWidth), (rand.Next(Console.WindowHeight - 3)) + 2, 0);
            else
                drop = new Drop(rand.Next(Console.WindowWidth), (rand.Next(Console.WindowHeight - 3)) + 2, Console.WindowHeight - 1);

            drops.Add(drop);
        }
    }

    class Drop
    {
        public int XPosition { get; }
        public int Length { get; set;  }
        public int YPosition { get; set; }
        public int VisibleCount { get; set; }
        public bool FirstHidden { get; set; }


        public Drop(int xPosition, int length, int yPosition)
        {
            XPosition = xPosition;
            Length = length;
            YPosition = yPosition;
            VisibleCount = 0;
            FirstHidden = false;
        }
    }
}