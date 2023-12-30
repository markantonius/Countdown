using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CountdownProject
{
    internal class Program
    {
        private static char[,] game = new char[23, 53];

        private static Random prandom = new Random();
        private static int cursorx = prandom.Next(1, 51), cursory = prandom.Next(1, 21); // position of cursor     
        private static int playerhp = 5, score = 0, zerotimer = 0, timee = 0;

        private static string playagain1;
        private static bool playagain = true;
        private static bool settings = false;
        private static string backgroundcolor;
        static void Main(string[] args)
        {
            
            StartScreen();
                      

            while (playagain == true)
            {

                for (int i = 0; i < game.GetLength(0); i++)
                {
                    for (int j = 0; j < game.GetLength(1); j++)
                    {
                        game[i, j] = ' ';
                    }
                }

                for (int i = 0; i < game.GetLength(0); i++)
                {
                    game[i, 0] = '#';
                    game[i, 52] = '#';
                }

                for (int i = 0; i < game.GetLength(1); i++)
                {
                    game[0, i] = '#';
                    game[22, i] = '#';
                }

                game[cursory, cursorx] = 'P'; // player position

                Random walld = new Random(); //block yerleştirme başlangıç
                int walldirection;
                for (int i = 0; i < 3; i++)
                {
                    walldirection = walld.Next(1, 3);
                    if (walldirection == 1) WallV(11);
                    else WallH(11);
                }

                for (int i = 0; i < 5; i++)
                {
                    walldirection = walld.Next(1, 3);
                    if (walldirection == 1) WallV(7);
                    else WallH(7);
                }

                for (int i = 0; i < 20; i++)
                {
                    walldirection = walld.Next(1, 3);
                    if (walldirection == 1) WallV(3);
                    else WallH(3);
                }                            //block yerleştirme son

                for (int i = 0; i < 70; i++)
                {
                    Numbers(0);
                }

                DrawColoredBox(86, 3, 30, 20, ConsoleColor.Blue, ConsoleColor.Black, "");

                Display();
                Player();
                PlayAgain();

            }

        }


        static void Display()
        {

            for (int i = 0; i < game.GetLength(0); i++)
            {
                Console.SetCursorPosition(30, 3 + i);
                for (int j = 0; j < game.GetLength(1); j++)
                {
                    Console.ResetColor();
                    switch (game[i, j])
                    {
                        case '0':
                            Console.BackgroundColor = ConsoleColor.White;
                            Console.ForegroundColor = ConsoleColor.Black;
                            break;
                        case '1':
                        case '2':
                        case '3':
                        case '4':
                        case '5':
                        case '6':
                        case '7':
                        case '8':
                        case '9':
                            Console.ForegroundColor = ConsoleColor.Green;
                            break;
                    }
                    Console.Write(game[i, j]);
                }

            }
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.SetCursorPosition(88, 4);
            Console.WriteLine("Time:");
            Console.SetCursorPosition(88, 6);
            Console.WriteLine($"Life:  {playerhp}");
            Console.SetCursorPosition(88, 8);
            Console.WriteLine($"Score: {score}");
            Console.ResetColor();
        }


        static void WallV(int length)
        {
            Random rnd = new Random();
            int wallxpos, wallypos;
            bool poscheck;

            wallxpos = rnd.Next(2, 52);
            wallypos = rnd.Next(2, 22 - length);

            poscheck = false;
            while (poscheck == false)
            {
                wallxpos = rnd.Next(2, 52);
                wallypos = rnd.Next(2, 22 - length);
                poscheck = true;
                for (int j = 0; j < length + 2; j++)
                {
                    if (game[wallypos + j - 1, wallxpos] != ' ' || game[wallypos + j - 1, wallxpos + 1] != ' ' || game[wallypos + j - 1, wallxpos - 1] != ' ')
                    {
                        poscheck = false;
                    }
                }
            }

            for (int j = 0; j < length; j++)
            {
                game[wallypos + j, wallxpos] = '#';
            }

        }


        static void WallH(int length)
        {
            Random rnd = new Random();
            int wallxpos, wallypos;
            bool poscheck;

            wallxpos = rnd.Next(2, 52 - length);
            wallypos = rnd.Next(2, 22);

            poscheck = false;
            while (poscheck == false)
            {
                wallxpos = rnd.Next(2, 52 - length);
                wallypos = rnd.Next(2, 22);
                poscheck = true;
                for (int j = 0; j < length + 2; j++)
                {
                    if (game[wallypos, wallxpos + j - 1] != ' ' || game[wallypos + 1, wallxpos + j - 1] != ' ' || game[wallypos - 1, wallxpos + j - 1] != ' ')
                    {
                        poscheck = false;
                    }
                }
            }

            for (int j = 0; j < length; j++)
            {
                game[wallypos, wallxpos + j] = '#';
            }

        }


        static void Numbers(int startnumber)
        {
            Random rnd = new Random();
            int numberxpos, numberypos;

            numberxpos = rnd.Next(1, 51);
            numberypos = rnd.Next(1, 21);
            while (game[numberypos, numberxpos] != ' ')
            {
                numberxpos = rnd.Next(1, 51);
                numberypos = rnd.Next(1, 21);
            }

            game[numberypos, numberxpos] = Convert.ToChar(rnd.Next(48 + startnumber, 58));
            Console.SetCursorPosition(numberxpos + 30, numberypos + 3);
            if (game[numberypos, numberxpos] == '0')
            {
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Black;
            }
            Console.WriteLine(game[numberypos, numberxpos]);
            Console.ResetColor();
        }


        static void Player()
        {

            int tempcursorx, tempcursory;
            int spaceposcount = 0;




            ConsoleKeyInfo cki;             // required for readkey
                                            // direction of A:   1:rigth   -1:left

            // --- Main game loop
            while (true)
            {
                TimeSpan timespan = TimeSpan.FromSeconds(timee);
                string elapsedtime = ((TimeSpan)(timespan)).ToString(@"hh\:mm\:ss");
                Console.SetCursorPosition(95, 4);
                Console.BackgroundColor = ConsoleColor.Blue;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine(elapsedtime);
                Console.ResetColor();

                //string elapsedtime = ((TimeSpan)(endTime - startTime)).ToString(@"hh\:mm\:ss");


                // Player movement
                bool cntrl = true;
                if (Console.KeyAvailable)
                {                                      // true: there is a key in keyboard buffer
                    cki = Console.ReadKey(true);       // true: do not write character 


                    if (cki.Key == ConsoleKey.RightArrow && cursorx < 51 && Convert.ToInt16(game[cursory, cursorx + 1]) > 48 && Convert.ToInt16(game[cursory, cursorx + 1]) <= 57)
                    {
                        spaceposcount = 0;

                        tempcursorx = cursorx;
                        while (game[cursory, cursorx + 1] != ' ' && game[cursory, cursorx + 1] != '#')
                        {
                            cursorx++;
                            spaceposcount++;
                        }
                        cursorx = tempcursorx;

                        for (int i = 1; i < spaceposcount; i++)
                        {
                            if (game[cursory, cursorx + i + 1] > game[cursory, cursorx + i])
                            {
                                cntrl = false;
                            }
                        }

                        if (spaceposcount == 1 && game[cursory, cursorx + 2] == '#')
                        {
                            cntrl = false;
                        }


                        if (cntrl == true)
                        {
                            Console.SetCursorPosition(cursorx + 30, cursory + 3);
                            Console.WriteLine(" ");
                            game[cursory, cursorx] = ' ';
                            cursorx++;

                            for (int i = spaceposcount; i > 0; i--)
                            {

                                if (game[cursory, cursorx + i] == '#')
                                {
                                    switch (game[cursory, cursorx + i - 1])
                                    {
                                        case '0':
                                            score += 20;
                                            Console.SetCursorPosition(95, 8);
                                            Console.BackgroundColor = ConsoleColor.Blue;
                                            Console.ForegroundColor = ConsoleColor.Black;
                                            Console.WriteLine(score);
                                            Console.ResetColor();
                                            break;
                                        case '1':
                                        case '2':
                                        case '3':
                                        case '4':
                                            score += 2;
                                            Console.SetCursorPosition(95, 8);
                                            Console.BackgroundColor = ConsoleColor.Blue;
                                            Console.ForegroundColor = ConsoleColor.Black;
                                            Console.WriteLine(score);
                                            Console.ResetColor();
                                            break;
                                        case '5':
                                        case '6':
                                        case '7':
                                        case '8':
                                        case '9':
                                            score += 1;
                                            Console.SetCursorPosition(95, 8);
                                            Console.BackgroundColor = ConsoleColor.Blue;
                                            Console.ForegroundColor = ConsoleColor.Black;
                                            Console.WriteLine(score);
                                            Console.ResetColor();
                                            break;
                                    }
                                    game[cursory, cursorx + i - 1] = ' ';
                                    Console.ForegroundColor = ConsoleColor.Green;
                                    Numbers(5);
                                    Console.ResetColor();
                                }

                                else
                                {
                                    game[cursory, cursorx + i] = game[cursory, cursorx + i - 1];
                                    game[cursory, cursorx + i - 1] = ' ';
                                    Console.SetCursorPosition(cursorx + 30 + i, cursory + 3);
                                    Console.ForegroundColor = ConsoleColor.Green;
                                    if (game[cursory, cursorx + i] == '0')
                                    {
                                        Console.BackgroundColor = ConsoleColor.White;
                                        Console.ForegroundColor = ConsoleColor.Black;
                                    }
                                    Console.WriteLine(game[cursory, cursorx + i]);
                                    Console.ResetColor();

                                }

                            }

                        }


                    }

                    if (cki.Key == ConsoleKey.RightArrow && game[cursory, cursorx + 1] == '0')
                    {
                        Console.SetCursorPosition(95, 6);
                        playerhp--;
                        Console.BackgroundColor = ConsoleColor.Blue;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.WriteLine(playerhp);
                        Console.ResetColor();

                        Console.SetCursorPosition(cursorx + 30, cursory + 3);
                        Console.BackgroundColor = ConsoleColor.DarkYellow;
                        Console.WriteLine(game[cursory, cursorx]);
                        if (zerotimer == 20)
                        {
                            Console.ResetColor();
                        }
                    }


                    if (cki.Key == ConsoleKey.RightArrow && cursorx < 51 && game[cursory, cursorx + 1] == ' ')
                    {
                        Console.SetCursorPosition(cursorx + 30, cursory + 3);
                        Console.WriteLine(" ");
                        game[cursory, cursorx] = ' ';
                        cursorx++;
                    }


                    if (cki.Key == ConsoleKey.LeftArrow && cursorx > 1 && Convert.ToInt16(game[cursory, cursorx - 1]) > 48 && Convert.ToInt16(game[cursory, cursorx - 1]) <= 57)
                    {
                        spaceposcount = 0;

                        tempcursorx = cursorx;
                        while (game[cursory, cursorx - 1] != ' ' && game[cursory, cursorx - 1] != '#')
                        {
                            cursorx--;
                            spaceposcount++;
                        }
                        cursorx = tempcursorx;

                        for (int i = 1; i < spaceposcount; i++)
                        {
                            if (game[cursory, cursorx - i - 1] > game[cursory, cursorx - i])
                            {
                                cntrl = false;
                            }
                        }

                        if (spaceposcount == 1 && game[cursory, cursorx - 2] == '#')
                        {
                            cntrl = false;
                        }




                        if (cntrl == true)
                        {
                            Console.SetCursorPosition(cursorx + 30, cursory + 3);
                            Console.WriteLine(" ");
                            game[cursory, cursorx] = ' ';
                            cursorx--;


                            for (int i = spaceposcount; i > 0; i--)
                            {

                                if (game[cursory, cursorx - i] == '#')
                                {
                                    switch (game[cursory, cursorx - i + 1])
                                    {
                                        case '0':
                                            score += 20;
                                            Console.SetCursorPosition(95, 8);
                                            Console.BackgroundColor = ConsoleColor.Blue;
                                            Console.ForegroundColor = ConsoleColor.Black;
                                            Console.WriteLine(score);
                                            Console.ResetColor();
                                            break;
                                        case '1':
                                        case '2':
                                        case '3':
                                        case '4':
                                            score += 2;
                                            Console.SetCursorPosition(95, 8);
                                            Console.BackgroundColor = ConsoleColor.Blue;
                                            Console.ForegroundColor = ConsoleColor.Black;
                                            Console.WriteLine(score);
                                            Console.ResetColor();
                                            break;
                                        case '5':
                                        case '6':
                                        case '7':
                                        case '8':
                                        case '9':
                                            score += 1;
                                            Console.SetCursorPosition(95, 8);
                                            Console.BackgroundColor = ConsoleColor.Blue;
                                            Console.ForegroundColor = ConsoleColor.Black;
                                            Console.WriteLine(score);
                                            Console.ResetColor();
                                            break;
                                    }
                                    game[cursory, cursorx - i + 1] = ' ';
                                    Console.ForegroundColor = ConsoleColor.Green;
                                    Numbers(5);
                                    Console.ResetColor();
                                }

                                else
                                {
                                    game[cursory, cursorx - i] = game[cursory, cursorx - i + 1];
                                    game[cursory, cursorx - i + 1] = ' ';
                                    Console.SetCursorPosition(cursorx + 30 - i, cursory + 3);
                                    Console.ForegroundColor = ConsoleColor.Green;
                                    if (game[cursory, cursorx - i] == '0')
                                    {
                                        Console.BackgroundColor = ConsoleColor.White;
                                        Console.ForegroundColor = ConsoleColor.Black;
                                    }
                                    Console.WriteLine(game[cursory, cursorx - i]);
                                    Console.ResetColor();
                                }

                            }

                        }

                    }

                    if (cki.Key == ConsoleKey.LeftArrow && game[cursory, cursorx - 1] == '0')
                    {
                        Console.SetCursorPosition(95, 6);
                        playerhp--;
                        Console.BackgroundColor = ConsoleColor.Blue;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.WriteLine(playerhp);
                        Console.ResetColor();

                        Console.SetCursorPosition(cursorx + 30, cursory + 3);
                        Console.BackgroundColor = ConsoleColor.DarkYellow;
                        Console.WriteLine(game[cursory, cursorx]);
                        if (zerotimer == 20)
                        {
                            Console.ResetColor();
                        }
                    }


                    if (cki.Key == ConsoleKey.LeftArrow && cursorx > 1 && game[cursory, cursorx - 1] == ' ')
                    {
                        Console.SetCursorPosition(cursorx + 30, cursory + 3);
                        Console.WriteLine(" ");
                        game[cursory, cursorx] = ' ';
                        cursorx--;
                    }

                    if (cki.Key == ConsoleKey.UpArrow && cursory > 1 && Convert.ToInt16(game[cursory - 1, cursorx]) > 48 && Convert.ToInt16(game[cursory - 1, cursorx]) <= 57)
                    {
                        spaceposcount = 0;

                        tempcursory = cursory;
                        while (game[cursory - 1, cursorx] != ' ' && game[cursory - 1, cursorx] != '#')
                        {
                            cursory--;
                            spaceposcount++;
                        }
                        cursory = tempcursory;

                        for (int i = 1; i < spaceposcount; i++)
                        {
                            if (game[cursory - i - 1, cursorx] > game[cursory - i, cursorx])
                            {
                                cntrl = false;
                            }
                        }

                        if (spaceposcount == 1 && game[cursory - 2, cursorx] == '#')
                        {
                            cntrl = false;
                        }


                        if (cntrl == true)
                        {
                            Console.SetCursorPosition(cursorx + 30, cursory + 3);
                            Console.WriteLine(" ");
                            game[cursory, cursorx] = ' ';
                            cursory--;

                            for (int i = spaceposcount; i > 0; i--)
                            {

                                if (game[cursory - i, cursorx] == '#')
                                {
                                    switch (game[cursory - i + 1, cursorx])
                                    {
                                        case '0':
                                            score += 20;
                                            Console.SetCursorPosition(95, 8);
                                            Console.BackgroundColor = ConsoleColor.Blue;
                                            Console.ForegroundColor = ConsoleColor.Black;
                                            Console.WriteLine(score);
                                            Console.ResetColor();
                                            break;
                                        case '1':
                                        case '2':
                                        case '3':
                                        case '4':
                                            score += 2;
                                            Console.SetCursorPosition(95, 8);
                                            Console.BackgroundColor = ConsoleColor.Blue;
                                            Console.ForegroundColor = ConsoleColor.Black;
                                            Console.WriteLine(score);
                                            Console.ResetColor();
                                            break;
                                        case '5':
                                        case '6':
                                        case '7':
                                        case '8':
                                        case '9':
                                            score += 1;
                                            Console.SetCursorPosition(95, 8);
                                            Console.BackgroundColor = ConsoleColor.Blue;
                                            Console.ForegroundColor = ConsoleColor.Black;
                                            Console.WriteLine(score);
                                            Console.ResetColor();
                                            break;
                                    }
                                    game[cursory - i + 1, cursorx] = ' ';
                                    Console.ForegroundColor = ConsoleColor.Green;
                                    Numbers(5);
                                    Console.ResetColor();
                                }

                                else
                                {
                                    game[cursory - i, cursorx] = game[cursory - i + 1, cursorx];
                                    game[cursory - i + 1, cursorx] = ' ';
                                    Console.SetCursorPosition(cursorx + 30, cursory + 3 - i);
                                    Console.ForegroundColor = ConsoleColor.Green;
                                    if (game[cursory - i, cursorx] == '0')
                                    {
                                        Console.BackgroundColor = ConsoleColor.White;
                                        Console.ForegroundColor = ConsoleColor.Black;
                                    }
                                    Console.WriteLine(game[cursory - i, cursorx]);
                                    Console.ResetColor();
                                }

                            }

                        }


                    }

                    if (cki.Key == ConsoleKey.UpArrow && game[cursory - 1, cursorx] == '0')
                    {
                        Console.SetCursorPosition(95, 6);
                        playerhp--;
                        Console.BackgroundColor = ConsoleColor.Blue;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.WriteLine(playerhp);
                        Console.ResetColor();

                        Console.SetCursorPosition(cursorx + 30, cursory + 3);
                        Console.BackgroundColor = ConsoleColor.DarkYellow;
                        Console.WriteLine(game[cursory, cursorx]);
                        if (zerotimer == 20)
                        {
                            Console.ResetColor();
                        }
                    }


                    if (cki.Key == ConsoleKey.UpArrow && cursory > 1 && game[cursory - 1, cursorx] == ' ')
                    {
                        Console.SetCursorPosition(cursorx + 30, cursory + 3);
                        Console.WriteLine(" ");
                        game[cursory, cursorx] = ' ';
                        cursory--;
                    }

                    if (cki.Key == ConsoleKey.DownArrow && cursory < 21 && Convert.ToInt16(game[cursory + 1, cursorx]) > 48 && Convert.ToInt16(game[cursory + 1, cursorx]) <= 57)
                    {
                        spaceposcount = 0;

                        tempcursory = cursory;
                        while (game[cursory + 1, cursorx] != ' ' && game[cursory + 1, cursorx] != '#')
                        {
                            cursory++;
                            spaceposcount++;
                        }
                        cursory = tempcursory;

                        for (int i = 1; i < spaceposcount; i++)
                        {
                            if (game[cursory + i + 1, cursorx] > game[cursory + i, cursorx])
                            {
                                cntrl = false;
                            }
                        }

                        if (spaceposcount == 1 && game[cursory + 2, cursorx] == '#')
                        {
                            cntrl = false;
                        }


                        if (cntrl == true)
                        {
                            Console.SetCursorPosition(cursorx + 30, cursory + 3);
                            Console.WriteLine(" ");
                            game[cursory, cursorx] = ' ';
                            cursory++;

                            for (int i = spaceposcount; i > 0; i--)
                            {

                                if (game[cursory + i, cursorx] == '#')
                                {
                                    switch (game[cursory + i - 1, cursorx])
                                    {
                                        case '0':
                                            score += 20;
                                            Console.SetCursorPosition(95, 8);
                                            Console.BackgroundColor = ConsoleColor.Blue;
                                            Console.ForegroundColor = ConsoleColor.Black;
                                            Console.WriteLine(score);
                                            Console.ResetColor();
                                            break;
                                        case '1':
                                        case '2':
                                        case '3':
                                        case '4':
                                            score += 2;
                                            Console.SetCursorPosition(95, 8);
                                            Console.BackgroundColor = ConsoleColor.Blue;
                                            Console.ForegroundColor = ConsoleColor.Black;
                                            Console.WriteLine(score);
                                            Console.ResetColor();
                                            break;
                                        case '5':
                                        case '6':
                                        case '7':
                                        case '8':
                                        case '9':
                                            score += 1;
                                            Console.SetCursorPosition(95, 8);
                                            Console.BackgroundColor = ConsoleColor.Blue;
                                            Console.ForegroundColor = ConsoleColor.Black;
                                            Console.WriteLine(score);
                                            Console.ResetColor();
                                            break;
                                    }
                                    game[cursory + i - 1, cursorx] = ' ';
                                    Console.ForegroundColor = ConsoleColor.Green;
                                    Numbers(5);
                                    Console.ResetColor();
                                }

                                else
                                {
                                    game[cursory + i, cursorx] = game[cursory + i - 1, cursorx];
                                    game[cursory + i - 1, cursorx] = ' ';
                                    Console.SetCursorPosition(cursorx + 30, cursory + 3 + i);
                                    Console.ForegroundColor = ConsoleColor.Green;
                                    if (game[cursory + i, cursorx] == '0')
                                    {
                                        Console.BackgroundColor = ConsoleColor.White;
                                        Console.ForegroundColor = ConsoleColor.Black;
                                    }
                                    Console.WriteLine(game[cursory + i, cursorx]);
                                    Console.ResetColor();
                                }


                            }

                        }

                    }

                    if (cki.Key == ConsoleKey.DownArrow && game[cursory + 1, cursorx] == '0')
                    {
                        Console.SetCursorPosition(95, 6);
                        playerhp--;
                        Console.BackgroundColor = ConsoleColor.Blue;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.WriteLine(playerhp);
                        Console.ResetColor();

                        Console.SetCursorPosition(cursorx + 30, cursory + 3);
                        Console.BackgroundColor = ConsoleColor.DarkYellow;
                        Console.WriteLine(game[cursory, cursorx]);
                        if (zerotimer == 20)
                        {
                            Console.ResetColor();
                        }
                    }


                    if (cki.Key == ConsoleKey.DownArrow && cursory < 21 && game[cursory + 1, cursorx] == ' ')
                    {
                        Console.SetCursorPosition(cursorx + 30, cursory + 3);
                        Console.WriteLine(" ");
                        game[cursory, cursorx] = ' ';
                        cursory++;
                    }


                }




                game[cursory, cursorx] = 'P';
                Console.ForegroundColor = ConsoleColor.Red;
                Console.SetCursorPosition(cursorx + 30, cursory + 3);    // refresh P (current position)
                Console.WriteLine("P");
                Console.ResetColor();

                char[,] tempgame = game.Clone() as char[,];
                Random zerornd = new Random();
                int zerodirection;
                int zerochance;





                // Random 0 Movement
                if (zerotimer == 20)
                {
                    for (int i = 0; i < tempgame.GetLength(0); i++)
                    {
                        for (int j = 0; j < tempgame.GetLength(1); j++)
                        {
                            if (tempgame[i, j] == '0')
                            {

                                zerodirection = zerornd.Next(1, 5);
                                bool poscheck = false;
                                while (poscheck == false)
                                {
                                    poscheck = true;

                                    zerodirection = zerornd.Next(1, 5);
                                    if (zerodirection == 1 && (game[i, j + 1] == '#' || (game[i, j + 1] >= '0' && game[i, j + 1] <= '9')))
                                    {
                                        poscheck = false;
                                    }

                                    if (zerodirection == 2 && (game[i, j - 1] == '#' || (game[i, j - 1] >= '0' && game[i, j - 1] <= '9')))
                                    {
                                        poscheck = false;
                                    }

                                    if (zerodirection == 3 && (game[i + 1, j] == '#' || (game[i + 1, j] >= '0' && game[i + 1, j] <= '9')))
                                    {
                                        poscheck = false;
                                    }

                                    if (zerodirection == 4 && (game[i - 1, j] == '#' || (game[i - 1, j] >= '0' && game[i - 1, j] <= '9')))
                                    {
                                        poscheck = false;
                                    }

                                    if ((game[i, j + 1] == '#' || (game[i, j + 1] >= '0' && game[i, j + 1] <= '9')) && (game[i, j - 1] == '#' || (game[i, j - 1] >= '0' && game[i, j - 1] <= '9')) && (game[i + 1, j] == '#' || (game[i + 1, j] >= '0' && game[i + 1, j] <= '9')) && (game[i - 1, j] == '#' || (game[i - 1, j] >= '0' && game[i - 1, j] <= '9')))
                                    {
                                        poscheck = true;
                                        zerodirection = 0;
                                    }

                                }

                                if (zerodirection == 1)
                                {
                                    if (game[i, j + 1] == 'P')
                                    {
                                        Console.SetCursorPosition(j + 30 + 1, i + 3);
                                        Console.BackgroundColor = ConsoleColor.DarkYellow;
                                        Console.WriteLine(game[i, j + 1]);
                                        Console.ResetColor();

                                        Console.SetCursorPosition(95, 6);
                                        playerhp--;
                                        Console.BackgroundColor = ConsoleColor.Blue;
                                        Console.ForegroundColor = ConsoleColor.Black;
                                        Console.WriteLine(playerhp);
                                        Console.ResetColor();
                                    }

                                    else
                                    {
                                        Console.SetCursorPosition(j + 30, i + 3);
                                        game[i, j] = ' ';
                                        Console.WriteLine(game[i, j]);
                                        Console.SetCursorPosition(j + 30 + 1, i + 3);
                                        Console.BackgroundColor = ConsoleColor.White;
                                        Console.ForegroundColor = ConsoleColor.Black;
                                        game[i, j + 1] = '0';
                                        Console.WriteLine(game[i, j + 1]);
                                        Console.ResetColor();
                                    }

                                }


                                if (zerodirection == 2)
                                {
                                    if (game[i, j - 1] == 'P')
                                    {
                                        Console.SetCursorPosition(j + 30 - 1, i + 3);
                                        Console.BackgroundColor = ConsoleColor.DarkYellow;
                                        Console.WriteLine(game[i, j - 1]);
                                        Console.ResetColor();

                                        Console.SetCursorPosition(95, 6);
                                        playerhp--;
                                        Console.BackgroundColor = ConsoleColor.Blue;
                                        Console.ForegroundColor = ConsoleColor.Black;
                                        Console.WriteLine(playerhp);
                                        Console.ResetColor();
                                    }

                                    else
                                    {
                                        Console.SetCursorPosition(j + 30, i + 3);
                                        game[i, j] = ' ';
                                        Console.WriteLine(game[i, j]);
                                        Console.SetCursorPosition(j + 30 - 1, i + 3);
                                        Console.BackgroundColor = ConsoleColor.White;
                                        Console.ForegroundColor = ConsoleColor.Black;
                                        game[i, j - 1] = '0';
                                        Console.WriteLine(game[i, j - 1]);
                                        Console.ResetColor();
                                    }

                                }

                                if (zerodirection == 3)
                                {
                                    if (game[i + 1, j] == 'P')
                                    {
                                        Console.SetCursorPosition(j + 30, i + 3 + 1);
                                        Console.BackgroundColor = ConsoleColor.DarkYellow;
                                        Console.WriteLine(game[i + 1, j]);
                                        Console.ResetColor();

                                        Console.SetCursorPosition(95, 6);
                                        playerhp--;
                                        Console.BackgroundColor = ConsoleColor.Blue;
                                        Console.ForegroundColor = ConsoleColor.Black;
                                        Console.WriteLine(playerhp);
                                        Console.ResetColor();
                                    }

                                    else
                                    {
                                        Console.SetCursorPosition(j + 30, i + 3);
                                        game[i, j] = ' ';
                                        Console.WriteLine(game[i, j]);
                                        Console.SetCursorPosition(j + 30, i + 3 + 1);
                                        Console.BackgroundColor = ConsoleColor.White;
                                        Console.ForegroundColor = ConsoleColor.Black;
                                        game[i + 1, j] = '0';
                                        Console.WriteLine(game[i + 1, j]);
                                        Console.ResetColor();
                                    }
                                }

                                if (zerodirection == 4)
                                {
                                    if (game[i - 1, j] == 'P')
                                    {
                                        Console.SetCursorPosition(j + 30, i + 3 - 1);
                                        Console.BackgroundColor = ConsoleColor.DarkYellow;
                                        Console.WriteLine(game[i - 1, j]);
                                        Console.ResetColor();

                                        Console.SetCursorPosition(95, 6);
                                        playerhp--;
                                        Console.BackgroundColor = ConsoleColor.Blue;
                                        Console.ForegroundColor = ConsoleColor.Black;
                                        Console.WriteLine(playerhp);
                                        Console.ResetColor();
                                    }

                                    else
                                    {
                                        Console.SetCursorPosition(j + 30, i + 3);
                                        game[i, j] = ' ';
                                        Console.WriteLine(game[i, j]);
                                        Console.SetCursorPosition(j + 30, i + 3 - 1);
                                        Console.BackgroundColor = ConsoleColor.White;
                                        Console.ForegroundColor = ConsoleColor.Black;
                                        game[i - 1, j] = '0';
                                        Console.WriteLine(game[i - 1, j]);
                                        Console.ResetColor();
                                    }

                                }




                            }
                        }
                    }

                    if (timee % 15 == 0 && timee != 0)
                    {
                        for (int i = 0; i < game.GetLength(0); i++)
                        {
                            for (int j = 0; j < game.GetLength(1); j++)
                            {
                                switch (game[i, j])
                                {
                                    case '1':
                                        zerochance = zerornd.Next(1, 101);
                                        if (zerochance == 1 || zerochance == 2 || zerochance == 3)
                                        {
                                            game[i, j]--;
                                            Console.SetCursorPosition(j + 30, i + 3);
                                            Console.BackgroundColor = ConsoleColor.White;
                                            Console.ForegroundColor = ConsoleColor.Black;
                                            Console.WriteLine(game[i, j]);
                                            Console.ResetColor();
                                        }
                                        break;
                                    case '2':
                                    case '3':
                                    case '4':
                                    case '5':
                                    case '6':
                                    case '7':
                                    case '8':
                                    case '9':
                                        game[i, j]--;
                                        Console.SetCursorPosition(j + 30, i + 3);
                                        Console.ForegroundColor = ConsoleColor.Green;
                                        Console.WriteLine(game[i, j]);
                                        Console.ResetColor();
                                        break;
                                }
                            }
                        }

                    }


                    timee++;
                    zerotimer = 0;
                }


                if (playerhp <= 0)
                {
                    Thread.Sleep(500);
                    EndScreen();
                    break;
                }

                while (Console.KeyAvailable) //code to clear buffer
                {
                    ConsoleKeyInfo key = Console.ReadKey(true);
                }

                Stopwatch stopwatch = Stopwatch.StartNew();
                while (true)
                {

                    if (stopwatch.ElapsedMilliseconds >= 50)  // sleep 50 ms  
                    {
                        zerotimer++;
                        break;
                    }

                }
            }


        }


        static void PlayAgain()
        {
            Console.SetCursorPosition(42, 16);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Would you like to play again?(Y/N):");
            playagain1 = Console.ReadLine().ToUpper();
            Console.ResetColor();

            while (playagain1 != "Y" && playagain1 != "N") // Invalid input
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.SetCursorPosition(36, 18);
                Console.Write("You have entered invalid input. Please enter again:");
                Console.ResetColor();
                playagain1 = Console.ReadLine();
            }

            if (playagain1 == "Y")
            {
                playagain = true;
                score = 0;
                timee = 0;
                playerhp = 5;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.SetCursorPosition(36, 18);
                Console.Write("                                                              ");
                Console.SetCursorPosition(51, 18);
                Console.WriteLine("Restarting the game...");
                Console.ResetColor();
                System.Threading.Thread.Sleep(1000);

                if (backgroundcolor == "darkblue")
                {
                    Console.BackgroundColor = ConsoleColor.DarkBlue;
                }

                if (backgroundcolor == "darkgreen")
                {
                    Console.BackgroundColor = ConsoleColor.DarkGreen;
                }

                if (backgroundcolor == "darkred")
                {
                    Console.BackgroundColor = ConsoleColor.DarkRed;
                }

                if (backgroundcolor == "darkyellow")
                {
                    Console.BackgroundColor = ConsoleColor.DarkYellow;
                }

                Console.Clear();
            }

            else if (playagain1 == "N")
            {
                playagain = false;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.SetCursorPosition(36, 18);
                Console.Write("                                                              ");
                Console.SetCursorPosition(52, 18);
                Console.WriteLine("Exiting the game...");
                Console.ResetColor();
                System.Threading.Thread.Sleep(1000);
            }
        }



        //visual
        static void EndScreen()
        {
            Console.Clear();
            GameFrames();

            Console.SetCursorPosition(56, 12);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("GAME OVER");
            Console.ResetColor();
            Console.SetCursorPosition(49, 14);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"Your final score was: {score}");
            Console.ResetColor();
            Console.ReadKey();
        }


        static void StartScreen()
        {

            Console.Clear();
            GameFrames();

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.SetCursorPosition(34, 5);
            Console.WriteLine(@"                          __      __                   ");
            Console.SetCursorPosition(34, 6);
            Console.WriteLine(@"  _________  __  ______  / / ____/ /_____      ______  ");
            Console.SetCursorPosition(34, 7);
            Console.WriteLine(@" / ___/ __ \/ / / / __ \/ __/ __  / __ \ | /| / / __ \ ");
            Console.SetCursorPosition(34, 8);
            Console.WriteLine(@"/ /__/ /_/ / /_/ / / / / /_/ /_/ / /_/ / |/ |/ / / / / ");
            Console.SetCursorPosition(34, 9);
            Console.WriteLine(@"\___/\____/\____/_/ /_/\__/\____/\____/|__/|__/_/ /_/ ");
            Console.ResetColor();

            
            Console.SetCursorPosition(48, 18);
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("»PRESS ANY KEY TO START«");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(55, 20);
            Console.WriteLine("»SETTINGS«");
            Console.ResetColor();

            int menuselection = 0;

            while (true)
            {
                ConsoleKeyInfo cki;
                cki = Console.ReadKey(true);

                if (cki.Key == ConsoleKey.UpArrow)
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.SetCursorPosition(48, 18);
                    Console.WriteLine("»PRESS ANY KEY TO START«");
                    Console.ResetColor();

                    Console.ForegroundColor = ConsoleColor.White;
                    Console.SetCursorPosition(55, 20);
                    Console.WriteLine("»SETTINGS«");

                    settings = false;
                    menuselection = 0;
                }

                if (cki.Key == ConsoleKey.DownArrow)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.SetCursorPosition(48, 18);
                    Console.WriteLine("»PRESS ANY KEY TO START«");

                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.SetCursorPosition(55, 20);
                    Console.WriteLine("»SETTINGS«");
                    Console.ResetColor();

                    menuselection = 1;
                }

                if (menuselection == 0 && cki.Key == ConsoleKey.Enter)
                {
                    break;
                }

                if (menuselection == 1 && cki.Key == ConsoleKey.Enter)
                {
                    settings = true;
                    break;
                }

            }

            if(settings == true)
            {
                BackgroundColorSelection();
            }                   

            if (backgroundcolor == "darkblue")
            {
                Console.BackgroundColor = ConsoleColor.DarkBlue;
            }

            if (backgroundcolor == "darkgreen")
            {
                Console.BackgroundColor = ConsoleColor.DarkGreen;
            }

            if (backgroundcolor == "darkred")
            {
                Console.BackgroundColor = ConsoleColor.DarkRed;
            }

            if (backgroundcolor == "darkyellow")
            {
                Console.BackgroundColor = ConsoleColor.DarkYellow;
            }

            Console.Clear();
        }


        static void GameFrames()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.White;

            for (int i = 0; i < 25; i++)
            {
                Console.SetCursorPosition(12, 2 + i);
                Console.WriteLine("║");
            }


            for (int i = 0; i < 25; i++)
            {
                Console.SetCursorPosition(107, 2 + i);
                Console.WriteLine("║");
            }


            Console.SetCursorPosition(13, 1);
            Console.WriteLine("══════════════════════════════════════════════════════════════════════════════════════════════");

            Console.SetCursorPosition(13, 27);
            Console.WriteLine("══════════════════════════════════════════════════════════════════════════════════════════════");

            Console.SetCursorPosition(107, 27);
            Console.WriteLine("╝");

            Console.SetCursorPosition(12, 27);
            Console.WriteLine("╚");

            Console.SetCursorPosition(12, 1);
            Console.WriteLine("╔");

            Console.SetCursorPosition(107, 1);
            Console.WriteLine("╗");

            Console.ResetColor();

        }

        static void DrawColoredBox(int startX, int startY, int width, int height, ConsoleColor bgColor, ConsoleColor fgColor, string content)
        {
            Console.BackgroundColor = bgColor;

            for (int i = 0; i < height + 1; i++)
            {
                Console.SetCursorPosition(startX, startY + i);
                Console.ForegroundColor = fgColor;
                Console.Write("║");
                Console.Write(content.PadRight(width - 2));
                Console.Write("║");
            }


            Console.SetCursorPosition(86, 3);
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("╔════════════════════════════╗");
            Console.SetCursorPosition(86, 24);
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("║     C O U N T D O W N      ║");
            Console.SetCursorPosition(86, 25);
            Console.WriteLine("╚════════════════════════════╝");

            Console.ResetColor();
        }


        static void BackgroundColorSelection()
        {


            Console.Clear(); // Clear the console before displaying the menu
            GameFrames();

            Console.ForegroundColor = ConsoleColor.White;

            Console.SetCursorPosition(46, 10);
            Console.WriteLine(" ╔══════════════════════════╗ ");

            Console.SetCursorPosition(46, 11);
            Console.WriteLine(" ║ Select Background Color: ║ ");

            Console.SetCursorPosition(46, 12);
            Console.WriteLine(" ║                          ║ ");

            Console.SetCursorPosition(46, 13);
            Console.WriteLine(" ║  Press 1. for Dark Blue  ║ ");

            Console.SetCursorPosition(46, 14);
            Console.WriteLine(" ║  Press 2. for Dark Green ║ ");

            Console.SetCursorPosition(46, 15);
            Console.WriteLine(" ║  Press 3. for Dark Red   ║ ");

            Console.SetCursorPosition(46, 16);
            Console.WriteLine(" ║  Press 4. for Dark Yellow║ ");

            Console.SetCursorPosition(46, 17);
            Console.WriteLine(" ║                          ║ ");

            Console.SetCursorPosition(46, 18);
            Console.WriteLine(" ║    Press ESC to exit     ║ ");

            Console.SetCursorPosition(46, 19);
            Console.WriteLine(" ╚══════════════════════════╝ ");



            Console.SetCursorPosition(30, 8);
            Console.WriteLine("(Or press any key to continue with the current background colour)");
            Console.ResetColor();
          

            while (true)
            {

                ConsoleKeyInfo keyInfo;
                keyInfo = Console.ReadKey(true);

                if (keyInfo.Key == ConsoleKey.D1)
                {                   
                    backgroundcolor = "darkblue";
                    break;
                }

                if (keyInfo.Key == ConsoleKey.D2)
                {                 
                    backgroundcolor = "darkgreen";
                    break;
                }

                if (keyInfo.Key == ConsoleKey.D3)
                {
                    backgroundcolor = "darkred";
                    break;
                }

                if (keyInfo.Key == ConsoleKey.D4)
                {
                    backgroundcolor = "darkyellow";
                    break;
                }

                if (keyInfo.Key == ConsoleKey.Escape)
                {
                    StartScreen();
                    break;
                }                 
               
            }


            Console.Clear();

        }

    }
}
