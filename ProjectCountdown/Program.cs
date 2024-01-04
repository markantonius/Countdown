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
        private static char[,] game = new char[23, 53]; //game board as an 2 dimensional array

        //global variables
        private static Random prandom = new Random();
        private static int cursorx = prandom.Next(1, 51), cursory = prandom.Next(1, 21); // random position of the cursor     
        private static int playerhp = 5, score = 0, zerotimer = 0, timee = 0; // variables for game elements

        private static string playagain1; //variables for playing the game again and the score history
        private static bool playagain = true;
        public static int[] scores = new int[5];
        public static string[] times = new string[5];

        private static string backgroundcolor;
        static void Main(string[] args)
        {
            GameFrames(); //create the frame design first
            StartScreen(); //then display the start menu
            BackgroundColorSelection(); //method for background color selection

            while (playagain == true) //while loop for making the game playable again after finishing
            {
                for (int i = 0; i < game.GetLength(0); i++) //filling game board's array with spaces
                {
                    for (int j = 0; j < game.GetLength(1); j++)
                    {
                        game[i, j] = ' ';
                    }
                }

                for (int i = 0; i < game.GetLength(0); i++) // assigning outer walls with # in the game array
                {
                    game[i, 0] = '#';
                    game[i, 52] = '#';
                }

                for (int i = 0; i < game.GetLength(1); i++)
                {
                    game[0, i] = '#';
                    game[22, i] = '#';
                }

                game[cursory, cursorx] = 'P'; //player position

                Random walld = new Random(); //inner wall placement start
                int walldirection;

                for (int i = 0; i < 3; i++) //place 11 block(#) long wall 3 times
                {
                    walldirection = walld.Next(1, 3); // randomly selecting if the wall will be vertical or horizontal
                    if (walldirection == 1) WallV(11);
                    else WallH(11);
                }

                for (int i = 0; i < 5; i++) //place 7 block(#) long wall 5 times
                {
                    walldirection = walld.Next(1, 3);  // randomly selecting if the wall will be vertical or horizontal
                    if (walldirection == 1) WallV(7);
                    else WallH(7);
                }

                for (int i = 0; i < 20; i++) //place 3 block(#) long walls 20 times
                {
                    walldirection = walld.Next(1, 3);  // randomly selecting if the wall will be vertical or horizontal
                    if (walldirection == 1) WallV(3);
                    else WallH(3);
                }                            //inner wall placement end

                for (int i = 0; i < 70; i++) //place 70 numbers between 0-9 (0 and 9 included)
                {
                    Numbers(0);
                }

                DrawColoredBox(86, 3, 30, 20, ConsoleColor.Blue, ConsoleColor.Black, ""); //design of the screen at the right of the game board

                Display(); //displaying the game board
                Player(); //the player and 0 movements
                PlayAgain(); //play again questions

            }

        }


        static void Display() //method for displaying the game board
        {

            for (int i = 0; i < game.GetLength(0); i++)
            {
                Console.SetCursorPosition(30, 3 + i); //setting cursor position in the middle of to display the game board in the middle of the screen
                for (int j = 0; j < game.GetLength(1); j++)
                {
                    Console.ResetColor();
                    switch (game[i, j]) //switch case for coloring the numbers
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

            //displaying time, life and score at the start of the game
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


        static void WallV(int length) //method for replacing a vertical wall
        {
            Random rnd = new Random();
            int wallxpos, wallypos;
            bool poscheck;

            wallxpos = rnd.Next(2, 52); //randomly assigning a x position for a wall
            wallypos = rnd.Next(2, 22 - length); //randomly assigning a y position for a wall

            poscheck = false; //variable for checking if the position is suitable for wall placement
            while (poscheck == false)
            {
                wallxpos = rnd.Next(2, 52);
                wallypos = rnd.Next(2, 22 - length);
                poscheck = true;
                for (int j = 0; j < length + 2; j++)
                {
                    if (game[wallypos + j - 1, wallxpos] != ' ' || game[wallypos + j - 1, wallxpos + 1] != ' ' || game[wallypos + j - 1, wallxpos - 1] != ' ') //if statement for checking if the assigned positions are available for wall placement
                    {
                        poscheck = false; //if the selected position is not suitable poscheck will be false and the loop will continue working. if the selected position is suitable then poscheck will stay true and position checking will end.
                    }
                }
            }

            for (int j = 0; j < length; j++)
            {
                game[wallypos + j, wallxpos] = '#'; //filling the selected position with blocks(#)
            }

        }


        static void WallH(int length) //method for replacing a horizontal wall (same as vertical wall method but the position checking is different)
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


        static void Numbers(int startnumber) //method for random number placing starting from the desired number.
        {
            Random rnd = new Random();
            int numberxpos, numberypos;

            numberxpos = rnd.Next(1, 51);
            numberypos = rnd.Next(1, 21);
            while (game[numberypos, numberxpos] != ' ') //check if the positions is available for placing the number
            {
                numberxpos = rnd.Next(1, 51);
                numberypos = rnd.Next(1, 21);
            }

            game[numberypos, numberxpos] = Convert.ToChar(rnd.Next(48 + startnumber, 58)); //select a number between a starting number and 57 (0-9 in ascii) and convert it to char then assign it to the array.
            Console.SetCursorPosition(numberxpos + 30, numberypos + 3);
            if (game[numberypos, numberxpos] == '0') //coloring for zeros
            {
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Black;
            }
            Console.WriteLine(game[numberypos, numberxpos]);
            Console.ResetColor();
        }


        static void Player()
        {

            //variables for counting how many numbers are being pushed
            int tempcursorx, tempcursory;
            int spaceposcount = 0;

            ConsoleKeyInfo cki; // required for readkey

            //Main game loop
            while (true)
            {
                TimeSpan timespan = TimeSpan.FromSeconds(timee);
                string elapsedtime = ((TimeSpan)(timespan)).ToString(@"hh\:mm\:ss"); //converting timee variable to hh\:mm\:ss format
                Console.SetCursorPosition(95, 4);
                Console.BackgroundColor = ConsoleColor.Blue;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine(elapsedtime);
                Console.ResetColor();

                //string elapsedtime = ((TimeSpan)(endTime - startTime)).ToString(@"hh\:mm\:ss"); (code for hh\:mm\:ss format)


                // Player movement
                bool cntrl = true; //variable to check if the number sequence is pushable

                if (Console.KeyAvailable)  // true: there is a key in keyboard buffer
                {
                    cki = Console.ReadKey(true); // true: do not write character 

                    if (cki.Key == ConsoleKey.RightArrow && cursorx < 51 && Convert.ToInt16(game[cursory, cursorx + 1]) > 48 && Convert.ToInt16(game[cursory, cursorx + 1]) <= 57) //if statement for if there are numbers at the right of the player to push
                    {
                        spaceposcount = 0; //count of how many numbers there are that player tries to push

                        tempcursorx = cursorx;
                        while (game[cursory, cursorx + 1] != ' ' && game[cursory, cursorx + 1] != '#') //while loop for counting how many numbers there are
                        {
                            cursorx++;
                            spaceposcount++;
                        }
                        cursorx = tempcursorx;

                        for (int i = 1; i < spaceposcount; i++) //for loop to check if the number sequence is non increasing.
                        {
                            if (game[cursory, cursorx + i + 1] > game[cursory, cursorx + i])
                            {
                                cntrl = false;
                            }
                        }

                        if (spaceposcount == 1 && game[cursory, cursorx + 2] == '#') //if statement to check if there is only 1 number between a wall and the player
                        {
                            cntrl = false;
                        }


                        if (cntrl == true) //if statement if the number sequence is pushable or smashable
                        {
                            Console.SetCursorPosition(cursorx + 30, cursory + 3);
                            Console.WriteLine(" ");
                            game[cursory, cursorx] = ' ';
                            cursorx++;

                            for (int i = spaceposcount; i > 0; i--)
                            {

                                if (game[cursory, cursorx + i] == '#') //if statement for if the number sequence is next to the wall and smashable
                                {
                                    switch (game[cursory, cursorx + i - 1]) //switch case for scoring and coloring
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
                                    Numbers(5); //generate a number starting from 5 when a number is smashed
                                    Console.ResetColor();
                                }

                                else //else statement to push the numbers
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

                    if (cki.Key == ConsoleKey.RightArrow && game[cursory, cursorx + 1] == '0') //if the player tries to move to zero's square
                    {
                        Console.SetCursorPosition(95, 6); //lose a life
                        playerhp--;
                        Console.BackgroundColor = ConsoleColor.Blue;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.WriteLine(playerhp);
                        Console.ResetColor();

                        Console.SetCursorPosition(cursorx + 30, cursory + 3); //make the player dark yellow
                        Console.BackgroundColor = ConsoleColor.DarkYellow;
                        Console.WriteLine(game[cursory, cursorx]);
                        if (zerotimer == 20)
                        {
                            Console.ResetColor();
                        }
                    }


                    if (cki.Key == ConsoleKey.RightArrow && cursorx < 51 && game[cursory, cursorx + 1] == ' ') //if there are no numbers next to the player to push or smash
                    {
                        Console.SetCursorPosition(cursorx + 30, cursory + 3);
                        Console.WriteLine(" ");
                        game[cursory, cursorx] = ' ';
                        cursorx++;
                    }

                    //same code for other directions
                    //the difference is the direction in the controls
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


                game[cursory, cursorx] = 'P'; //'P' in the array
                Console.ForegroundColor = ConsoleColor.Red;
                Console.SetCursorPosition(cursorx + 30, cursory + 3); // refresh P (current position)
                Console.WriteLine("P");
                Console.ResetColor();



                // Random 0 Movement
                char[,] tempgame = new char[23, 53]; //a clone of the game array for controlling the zeros in the board.
                for (int i = 0; i < tempgame.GetLength(0); i++) //cloning the game array into the tempgame array
                {
                    for (int j = 0; j < tempgame.GetLength(1); j++)
                    {
                        tempgame[i, j] = game[i, j];
                    }
                }

                Random zerornd = new Random();
                int zerodirection;
                int zerochance;

                if (zerotimer == 20) //if statement for zero timing (zeros will move in every 20 iteration of the player's move loop (50ms x 20 = 1000ms which is 1 second))
                {
                    for (int i = 0; i < tempgame.GetLength(0); i++)
                    {
                        for (int j = 0; j < tempgame.GetLength(1); j++)
                        {
                            if (tempgame[i, j] == '0') //if statement to find the zeros
                            {

                                zerodirection = zerornd.Next(1, 5); //randomly choosing zero's movement direction
                                bool poscheck = false; //variable to check if the zero can move in the selected direction
                                while (poscheck == false) //while loop to choose another direction if the zero cannot move in the direction selected.
                                {
                                    poscheck = true;

                                    zerodirection = zerornd.Next(1, 5);
                                    if (zerodirection == 1 && (game[i, j + 1] == '#' || (game[i, j + 1] >= '0' && game[i, j + 1] <= '9'))) //poscheck will be false if the zero cannot move in the selected direction and the loop will continue to iterate.
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
                                    //if statement to prevent the game from hardlocking if the zero cannot move in any of the directions
                                    {
                                        poscheck = true;
                                        zerodirection = 0;
                                    }

                                }

                                if (zerodirection == 1)
                                {
                                    if (game[i, j + 1] == 'P') //decrease the life of the player if the player is in the square the zero tries to move to 
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

                                    else //move the zero in the selected direction
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

                                //same code for other directions, the difference is the direction
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

                    if (timee % 15 == 0 && timee != 0) //if statement for decreasing the numbers in every 15 seconds.
                    {
                        for (int i = 0; i < game.GetLength(0); i++)
                        {
                            for (int j = 0; j < game.GetLength(1); j++)
                            {
                                switch (game[i, j]) //switch case for decreasing the numbers
                                {
                                    case '1': // 3% chance for ones to turn into zeros.
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


                    timee++; //since zeros move in every 1 seconds, timee variable will be increased by one every 1 seconds.
                    zerotimer = 0; //reset the zerotimer
                }


                if (playerhp <= 0) //if player does not have any lives left
                {
                    Thread.Sleep(500);
                    EndScreen(elapsedtime);
                    break;
                }

                while (Console.KeyAvailable) //code to clear buffer
                {
                    ConsoleKeyInfo key = Console.ReadKey(true);
                }

                Stopwatch stopwatch = Stopwatch.StartNew(); // the code we used instead of Thread.Sleep(50)
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

        //methods for saving and playing again
        static void Save(string elapsedtime, int score) //method for saving the score and the elapsed time when the game is finished
        {

            for (int i = 0; i < 5; i++)
            {
                if (score > scores[i])
                {
                    for (int j = 4; j > i; j--)
                    {
                        scores[j] = scores[j - 1];
                        times[j] = times[j - 1];
                    }

                    scores[i] = score;
                    times[i] = elapsedtime;

                    break;
                }

            }

        }
        static void PlayAgain()
        {
            Console.SetCursorPosition(42, 10);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Would you like to play again?(Y/N):");
            playagain1 = Console.ReadLine().ToUpper();
            Console.ResetColor();

            while (playagain1 != "Y" && playagain1 != "N") // Invalid input
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.SetCursorPosition(36, 12);
                Console.Write("You have entered invalid input. Please enter again:");
                Console.ResetColor();
                playagain1 = Console.ReadLine().ToUpper();
            }

            if (playagain1 == "Y")
            {
                playagain = true;
                score = 0; //change variables to their original values if the "Y" is selected.
                timee = 0;
                zerotimer = 0;
                playerhp = 5;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.SetCursorPosition(36, 12);
                Console.Write("                                                              ");
                Console.SetCursorPosition(51, 12);
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
                Console.SetCursorPosition(36, 12);
                Console.Write("                                                              ");
                Console.SetCursorPosition(52, 12);
                Console.WriteLine("Exiting the game...");
                Console.ResetColor();
                System.Threading.Thread.Sleep(1000);
            }
        }



        //visual
        static void EndScreen(string elapsedtime) //method for displaying highest scores and end screen
        {

            Save(elapsedtime, score);
            Console.Clear();
            GameFrames();

            //printing high scores
            for (int i = 0; i < 5; i++)
            {
                if (scores[i] != 0) //if statement for printing only the scores that player got
                {
                    if (i == 0)
                        Console.ForegroundColor = ConsoleColor.Red;
                    if (i == 1)
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                    if (i == 2)
                        Console.ForegroundColor = ConsoleColor.Yellow;
                    if (i == 3)
                        Console.ForegroundColor = ConsoleColor.Green;
                    if (i == 4)
                        Console.ForegroundColor = ConsoleColor.Cyan;

                    Console.SetCursorPosition(35, 16 + i);
                    Console.WriteLine((i + 1) + ".");

                    Console.SetCursorPosition(48, 16 + i);
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine(scores[i]);

                    Console.SetCursorPosition(70, 16 + i);
                    Console.WriteLine(times[i]);

                    Console.ResetColor();
                }
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.SetCursorPosition(42, 14);
            Console.Write("HIGHEST SCORE                TIME");
            Console.ResetColor();

            Console.SetCursorPosition(56, 6);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("GAME OVER");
            Console.ResetColor();
            Console.SetCursorPosition(49, 8);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"Your final score was: {score}");
            Console.ResetColor();
            Console.ReadKey();
        }


        static void StartScreen() //method for printing start screen logo and "press any key to start"
        {
            Console.ForegroundColor = ConsoleColor.Blue; //logo
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

            while (true) //blinking "PRESS ANY KEY TO START" button
            {
                Console.SetCursorPosition(49, 18);
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("»PRESS ANY KEY TO START«");
                Console.ResetColor();

                Thread.Sleep(1000);

                Console.SetCursorPosition(49, 18);
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("»PRESS ANY KEY TO START«");

                if (Console.KeyAvailable)
                {
                    Console.ReadKey(true);
                    break;
                }

                Thread.Sleep(1000);
            }

            Console.Clear();
            Thread.Sleep(1000);

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

        static void DrawColoredBox(int startX, int startY, int width, int height, ConsoleColor bgColor, ConsoleColor fgColor, string content) //the colored box next to the game board
        {
            Console.BackgroundColor = bgColor;

            for (int i = 0; i < height + 1; i++) //frames of the colored box
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
            Console.WriteLine("╔════════════════════════════╗"); // top frame of the box
            Console.SetCursorPosition(86, 24);
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("║     C O U N T D O W N      ║"); //countdown logo at the bottom of the colored box
            Console.SetCursorPosition(86, 25);
            Console.WriteLine("╚════════════════════════════╝"); // bottom frame of the box

            Console.ResetColor();
        }


        static void BackgroundColorSelection() //method for the background color of the area around the game screen
        {


            Console.Clear(); // Clear the console before displaying the menu
            GameFrames();

            Console.ForegroundColor = ConsoleColor.White;

            Console.SetCursorPosition(46, 10); // color selection menu
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

            ConsoleKeyInfo keyInfo;

            keyInfo = Console.ReadKey(); //input for selection

            switch (keyInfo.Key) //switch case for selection
            {

                case ConsoleKey.D1:
                    Console.BackgroundColor = ConsoleColor.DarkBlue;
                    backgroundcolor = "darkblue";
                    break;
                case ConsoleKey.D2:
                    Console.BackgroundColor = ConsoleColor.DarkGreen;
                    backgroundcolor = "darkgreen";
                    break;
                case ConsoleKey.D3:
                    Console.BackgroundColor = ConsoleColor.DarkRed;
                    backgroundcolor = "darkred";
                    break;
                case ConsoleKey.D4:
                    Console.BackgroundColor = ConsoleColor.DarkYellow;
                    backgroundcolor = "darkyellow";
                    break;
                case ConsoleKey.Escape:
                    Environment.Exit(0); // Exit the program if Esc key is pressed
                    break;
                default:
                    Console.Clear();     // Clear the console if an invalid key is pressed. (Continue with the current background color.)
                    break;
            }


            Console.Clear();

        }

    }
}
