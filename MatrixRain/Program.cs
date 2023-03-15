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
        private static int[,] sets = {{48, 57}, {65, 90}, {48, 90}};

        ///<summary>
        /// Digital Matrix rain from the Matrix movie, homework
        /// </summary>
        /// <param name="directionUp">Direction of falling code up</param>
        /// <param name="color">Color of falling code up</param>
        /// <param name="delaySpeed">Delay speed of falling code in milliseconds</param>
        /// <param name="characters">The set of characters from which the falling code will be generated</param>
        static void Main(bool directionUp = false, ConsoleColor color = ConsoleColor.Green, int delaySpeed = 1, Characters characters = Characters.AlphaNumeric)
        {
            List<Kvapka> kvapky = new List<Kvapka>();
            List<Kvapka> vymaz = new List<Kvapka>();
            MatrixRain mr = new MatrixRain();

            Console.Clear();
            Console.CursorVisible = false;

            bool hore = directionUp;
            int sleep = delaySpeed;
            int set = (int)characters;

            Console.ForegroundColor = color;
            int i = 0;
            while (!Console.KeyAvailable)
            {
                mr.posunKvapky(ref i, ref kvapky, ref vymaz, hore, sleep, sets[set,0], sets[set,1], color);
                if ((i % 2) == 0)
                {
                    mr.generujKvapky(ref kvapky, hore);
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
        private Random r = new();
        public void posunKvapky(ref int i, ref List<Kvapka> kvapky, ref List<Kvapka> vymaz, bool hore, int sleep, int lower, int upper, ConsoleColor color)
        {
            foreach (var k in kvapky)
            {
                Console.SetCursorPosition(k.Pozicia, k.Y);

                int decChar = r.Next(lower, upper + 1);

                if (!k.End)
                    Console.ForegroundColor = ConsoleColor.White;

                Console.Write((char)decChar);

                if (k.Visible >= k.Dlzka)
                    k.Visible = k.Dlzka;
                else
                    k.Visible++;

                Console.ForegroundColor = color;
                
                if (!hore)
                {
                    if (k.Visible > 0)
                    {
                        for (int j = 1; j < k.Visible; j++)
                        {
                            if (k.Y > 0)
                            {
                                int tmp = k.Y - j;
                                Console.SetCursorPosition(k.Pozicia, tmp);
                                decChar = r.Next(lower, upper + 1);
                                Console.Write((char)decChar);
                            }
                        }
                    }

                    Thread.Sleep(sleep);

                    if ((k.Y - k.Dlzka) >= 0)
                    {
                        i = k.Y - k.Dlzka;
                        Console.SetCursorPosition(k.Pozicia, i);
                        Console.Write(" ");
                    }
                    k.Y++;

                    if (k.Y >= Console.WindowHeight)
                    {
                        k.Y = Console.WindowHeight - 1;
                        k.Dlzka--;
                        k.End = true;
                        if (k.Dlzka == -1)
                        {
                            vymaz.Add(k);
                        }
                    }
                }
                else
                {
                    if (k.Visible > 0)
                    {
                        for (int j = 1; j < k.Visible; j++)
                        {
                            if (k.Y < Console.WindowHeight - 1)
                            {
                                int tmp = k.Y + j;
                                Console.SetCursorPosition(k.Pozicia, tmp);
                                decChar = r.Next(lower, upper + 1);
                                Console.Write((char)decChar);
                            }
                        }
                    }

                    Thread.Sleep(sleep);

                    if ((k.Y + k.Dlzka) <= Console.WindowHeight - 1)
                    {
                        i = k.Y + k.Dlzka;
                        Console.SetCursorPosition(k.Pozicia, i);
                        Console.Write(" ");
                    }

                    k.Y--;

                    if (k.Y < 0)
                    {
                        k.Y = 0;
                        k.Dlzka--;
                        k.End = true;
                        if (k.Dlzka == -1)
                        {
                            vymaz.Add(k);
                        }
                    }
                }
            }
            foreach (var kvapka in vymaz)
            {
                kvapky.Remove(kvapka);
            }
            vymaz.Clear();
        }

        public void generujKvapky(ref List<Kvapka> kvapky, bool hore)
        {
            Kvapka k;
            if (!hore)
                k = new Kvapka(r.Next(Console.WindowWidth), (r.Next(Console.WindowHeight - 3)) + 2, 0);
            else
                k = new Kvapka(r.Next(Console.WindowWidth), (r.Next(Console.WindowHeight - 3)) + 2, Console.WindowHeight - 1);

            kvapky.Add(k);
        }
    }

    class Kvapka
    {
        public int Pozicia { get; }
        public int Dlzka { get; set;  }
        public int Y { get; set; }
        public int Visible { get; set; }
        public bool End { get; set; }


        public Kvapka(int pozicia, int dlzka, int y)
        {
            this.Pozicia = pozicia;
            this.Dlzka = dlzka;
            Y = y;
            Visible = 0;
            End = false;
        }
    }
}