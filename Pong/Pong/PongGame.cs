using System;
using System.Drawing;
using System.Threading;
using System.Xml.Linq;

namespace Pong
{
    internal class PongGame
    {
        protected char[,] field;
        protected char[,] menu;
        readonly int N;
        readonly int M;
        protected int leftRacketX;
        protected int leftRacketY;
        protected int rightRacketX;
        protected int rightRacketY;
        protected int rightRacketSpeedY;
        protected int leftRacketSpeedY;
        protected int ballX;
        protected int ballY;
        protected int gameSpeed;
        protected int ballSpeedX;
        protected int ballSpeedY;
        protected int firstPlayerPoints;
        protected int secondPlayerPoints;

        internal PongGame()
        {
            N = 25;
            M = 80;
            leftRacketX = 5;
            leftRacketY = 12;
            rightRacketX = 74;
            rightRacketY = 12;
            rightRacketSpeedY = 0;
            leftRacketSpeedY = 0;
            ballX = 39;
            ballY = 12;
            ballSpeedX = 1;
            ballSpeedY = 1;
            gameSpeed = 80;
            firstPlayerPoints = 0;
            secondPlayerPoints = 0;
            menu = new char[M, M];
            field = new char[N, M];
        }
        /// <summary>
        /// Вывод игрового поля в консоль
        /// </summary>
        private void PrintField()
        {
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < M; j++)
                {
                    if ((i == rightRacketY + 1 || i == rightRacketY - 1 || i == rightRacketY) && j == rightRacketX)
                    {
                        Console.Write("|");
                    }
                    else if ((i == leftRacketY + 1 || i == leftRacketY - 1 || i == leftRacketY) && j == leftRacketX)
                    {
                        Console.Write("|");
                    }
                        else if (i == ballY && j == ballX)
                    {
                        Console.Write("*");
                    }
                    else if (i == 3 && (j == 35 || j == 44))
                    {
                        if (j == 35)
                            Console.Write(firstPlayerPoints);
                        else
                            Console.Write(secondPlayerPoints);
                    }
                    else
                    {
                        Console.Write(field[i, j]);
                    }
                }
                Console.WriteLine();
            }

        }

        private void Print(char[,] f)
        {
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < M; j++)
                {
                    Console.Write(f[i,j]);
                }
                Console.WriteLine();
            }
        }
        /// <summary>
        /// Заполнение игрового поля
        /// </summary>
        private void FillTheField(char[,] field)
        {
            for (int i = 0; i < N; i++)
                for (int j = 0; j < M; j++)
                {
                    if (j == 0 || j == 79)
                    {
                        field[i, j] = '║';
                    }
                    else if (i == 0 || i == N - 1)
                    {
                        field[i, j] = '═';
                    }
                    else
                    {
                        field[i, j] = ' ';
                    }
                }
            field[0, 0] = '╔';
            field[0, M - 1] = '╗';
            field[N - 1, 0] = '╚';
            field[N - 1, M - 1] = '╝';
        }
        /// <summary>
        /// Поведение мяча при столкновениях
        /// </summary>
        private void Collision()
        {
            if (field[ballY + ballSpeedY, ballX + ballSpeedX] == '║')
            {
                if (ballX >= 70)
                    firstPlayerPoints += 1;
                if (ballX <= 5)
                    secondPlayerPoints += 1;

                Console.Clear();
                ballX = 39;
                ballY = 12;
                ballSpeedX = -ballSpeedX;
                ballSpeedY = 1;
            }
            if (field[ballY + ballSpeedY, ballX + ballSpeedX] == '═')
            {
                ballSpeedX = ballSpeedX;
                ballSpeedY = -ballSpeedY;
            }
            if ((ballY + ballSpeedY == rightRacketY || ballY + ballSpeedY == rightRacketY + 1 || ballY + ballSpeedY == rightRacketY - 1 || ballY == rightRacketY) && ballX + ballSpeedX == rightRacketX)
            {
                ballSpeedX = -ballSpeedX;
                ballSpeedY = ballSpeedY;
            }
            if ((ballY + ballSpeedY == leftRacketY || ballY + ballSpeedY == leftRacketY + 1 || ballY + ballSpeedY == leftRacketY - 1 || ballY == leftRacketY) && ballX + ballSpeedX == leftRacketX)
            {
                ballSpeedX = -ballSpeedX;
                ballSpeedY = ballSpeedY;
            }
        }
        /// <summary>
        /// Управление ракетками
        /// </summary>
        /// <returns></returns>
        private bool Keys()
        {
            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey();
                if (key.Key == ConsoleKey.UpArrow && rightRacketY > 2)
                {
                    rightRacketSpeedY = -1;
                }
                if (key.Key == ConsoleKey.DownArrow && rightRacketY < 22)
                {
                    rightRacketSpeedY = 1;
                }
                if (key.Key == ConsoleKey.A && leftRacketY > 2)
                {
                    leftRacketSpeedY = -1;
                }
                if (key.Key == ConsoleKey.Z && leftRacketY < 22)
                {
                    leftRacketSpeedY = 1;
                }

                if (key.Key == ConsoleKey.Escape)
                    return false;
            }
            return true;
        }
        /// <summary>
        /// Игра "Пинг-Понг" для 2 игроков
        /// </summary>
        private void MultiplayerGame()
        {
            FillTheField(field);
            PrintField();
            leftRacketX = 5;
            leftRacketY = 12;
            rightRacketX = 74;
            rightRacketY = 12;
            rightRacketSpeedY = 0;
            leftRacketSpeedY = 0;
            ballX = 39;
            ballY = 12;
            firstPlayerPoints = 0;
            secondPlayerPoints = 0;
            
            while (firstPlayerPoints != 7 && secondPlayerPoints != 7)
            {
                Collision();
                char cur = field[ballY, ballX];
                field[ballY, ballX] = field[ballY + ballSpeedY, ballX + ballSpeedX];
                field[ballY + ballSpeedY, ballX + ballSpeedX] = cur;
                Console.Clear();
                PrintField();

                Thread.Sleep(gameSpeed);

                ballX += ballSpeedX;
                ballY += ballSpeedY;

                rightRacketY += rightRacketSpeedY;
                leftRacketY += leftRacketSpeedY;

                rightRacketSpeedY = 0;
                leftRacketSpeedY = 0;
                if (!Keys())
                {
                    Console.Clear();
                    Menu();
                    break;
                }
            }

            string win = "";
            if ((firstPlayerPoints == 7 || secondPlayerPoints == 7) && firstPlayerPoints > secondPlayerPoints)
            {
                win = "GAME OVER! FIRST PLAYER WON!";
            }
            else if ((firstPlayerPoints == 7 || secondPlayerPoints == 7) && firstPlayerPoints < secondPlayerPoints)
            {
                win = "GAME OVER! SECOND PLAYER WON!";
            }
            for (int i = 0; i < win.Length; i++)
            {
                field[20, 27 + i] = win[i];
            }
            Console.Clear();
            PrintField();
        }
        /// <summary>
        /// Навигация по меню
        /// </summary>
        /// <param name="str1"></param>
        /// <param name="str2"></param>
        /// <param name="str3"></param>
        /// <param name="str4"></param>
        /// <returns></returns>
        private int MenuKeys(string str1, string str2, string str3, string str4)
        {
            int k = 10;
            bool exit = false;
            while (!exit)
            {
                if (k == 10)
                {
                    menu[11, 31] = ' ';
                    menu[11, 32 + str2.Length] = ' ';
                    menu[10, 31] = '>';
                    menu[10, 32 + str1.Length] = '<';
                }
                if (k == 11)
                {
                    menu[10, 31] = ' ';
                    menu[10, 32 + str1.Length] = ' ';
                    menu[12, 31] = ' ';
                    menu[12, 32 + str3.Length] = ' ';
                    menu[11, 31] = '>';
                    menu[11, 32 + str2.Length] = '<';
                }
                if (k == 12)
                {
                    menu[11, 31] = ' ';
                    menu[11, 32 + str2.Length] = ' ';
                    menu[13, 31] = ' ';
                    menu[13, 32 + str4.Length] = ' ';
                    menu[12, 31] = '>';
                    menu[12, 32 + str3.Length] = '<';
                }
                if (k == 13)
                {
                    menu[12, 31] = ' ';
                    menu[12, 32 + str3.Length] = ' ';
                    menu[13, 31] = '>';
                    menu[13, 32 + str4.Length] = '<';
                }
                Console.Clear();
                Print(menu);
                Thread.Sleep(60);

                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey();
                    if (key.Key == ConsoleKey.UpArrow && k > 10)
                    {
                        k--;
                    }
                    if (key.Key == ConsoleKey.DownArrow && k < 13)
                    {
                        k++;
                    }
                    if (key.Key == ConsoleKey.Enter)
                    {
                        Console.Clear();
                        exit = true;
                    }
                }
            }
            return k;
        }
        /// <summary>
        /// Навигация по настройкам
        /// </summary>
        /// <param name="str1"></param>
        /// <param name="str2"></param>
        /// <param name="str3"></param>
        /// <param name="str4"></param>
        /// <returns></returns>
        private int SettigsKeys(string str1, string str2, string str3, string str4)
        {
            int k = 10;
            bool exit = false;
            while (!exit)
            {
                if (k == 10)
                {
                    menu[11, 31] = ' ';
                    menu[11, 32 + str2.Length] = ' ';
                    menu[10, 31] = '>';
                    menu[10, 32 + str1.Length] = '<';
                }
                if (k == 11)
                {
                    menu[10, 31] = ' ';
                    menu[10, 32 + str1.Length] = ' ';
                    menu[12, 32 + str4.Length] = ' ';
                    menu[12, 31] = ' ';
                    menu[11, 31] = '>';
                    menu[11, 32 + str2.Length] = '<';
                }
                if (k == 12)
                {
                    menu[11, 31] = ' ';
                    menu[11, 32 + str2.Length] = ' ';
                    menu[13, 31] = ' ';
                    menu[13, 32 + str3.Length] = ' ';
                    menu[12, 32 + str4.Length] = '<';
                    menu[12, 31] = '>';
                }
                if (k == 13)
                {
                    menu[12, 32 + str4.Length] = ' ';
                    menu[12, 31] = ' ';
                    menu[13, 31] = '>';
                    menu[13, 32 + str3.Length] = '<';
                }
                
                Console.Clear();
                Print(menu);
                Thread.Sleep(60);

                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey();
                    if (key.Key == ConsoleKey.UpArrow && k > 10)
                    {
                        k--;
                    }
                    if (key.Key == ConsoleKey.DownArrow && k < 13)
                    {
                        k++;
                    }
                    if (key.Key == ConsoleKey.Enter)
                    {
                        Console.Clear();
                        exit = true;
                    }
                }
            }
            return k;
        }
        /// <summary>
        /// Режим одиночной игры 
        /// </summary>
        private void SinglePlayer()
        {
            FillTheField(field);
            PrintField();
            leftRacketX = 5;
            leftRacketY = 12;
            rightRacketX = 74;
            rightRacketY = 12;
            rightRacketSpeedY = 0;
            leftRacketSpeedY = 0;
            ballX = 39;
            ballY = 12;
            firstPlayerPoints = 0;
            secondPlayerPoints = 0;

            while (firstPlayerPoints != 7 && secondPlayerPoints != 7)
            {
                Collision();
                char cur = field[ballY, ballX];
                field[ballY, ballX] = field[ballY + ballSpeedY, ballX + ballSpeedX];
                field[ballY + ballSpeedY, ballX + ballSpeedX] = cur;
                Console.Clear();
                PrintField();

                Thread.Sleep(gameSpeed);

                ballX += ballSpeedX;
                ballY += ballSpeedY;

                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey();
                    if (key.Key == ConsoleKey.UpArrow && rightRacketY > 2)
                    {
                        rightRacketSpeedY = -1;
                    }
                    if (key.Key == ConsoleKey.DownArrow && rightRacketY < 22)
                    {
                        rightRacketSpeedY = 1;
                    }
                    if (key.Key == ConsoleKey.Escape)
                    {
                        Console.Clear();
                        Menu();
                        break;
                    }
                }

                //AI
                if (leftRacketY < ballY && leftRacketY > 2 && ballX < 40)
                {
                    leftRacketSpeedY = -1;
                }
                else if (leftRacketY > ballY && leftRacketY < 22 && ballX < 40)
                {
                    leftRacketSpeedY = 1;
                }
                //

                rightRacketY += rightRacketSpeedY;
                leftRacketY += leftRacketSpeedY;

                rightRacketSpeedY = 0;
                leftRacketSpeedY = 0;
            }

            string win = "";
            if ((firstPlayerPoints == 7 || secondPlayerPoints == 7) && firstPlayerPoints > secondPlayerPoints)
            {
                win = "GAME OVER! AI WON!";
            }
            else if ((firstPlayerPoints == 7 || secondPlayerPoints == 7) && firstPlayerPoints < secondPlayerPoints)
            {
                win = "GAME OVER! YOU WON!";
            }
            for (int i = 0; i < win.Length; i++)
            {
                field[20, 27 + i] = win[i];
            }
            Console.Clear();
            PrintField();
        }
        /// <summary>
        /// Авторы проекта
        /// </summary>
        private void Credits()
        {
            string str1 = "AUTHOR - 2Jinx";
            string str4 = "SUPPORT THE AUTHOR";
            string str5 = "SBERBANK - 5469 9804 2486 7410";
            string str6 = "TINKOFF - 5536 9141 6852 6550";
            string str7 = "GitHub - https://github.com/2Jinx/SelfProjects/tree/main/Pong";
            FillTheField(menu);
            for (int i = 0; i < str1.Length; i++)
            {
                menu[3, 32 + i] = str1[i];
            }
            for (int i = 0; i < str4.Length; i++)
            {
                menu[7, 10 + i] = str4[i];
            }
            for (int i = 0; i < str5.Length; i++)
            {
                menu[10, 10 + i] = str5[i];
            }
            for (int i = 0; i < str6.Length; i++)
            {
                menu[11, 10 + i] = str6[i];
            }
            for (int i = 0; i < str7.Length; i++)
            {
                menu[14, 10 + i] = str7[i];
            }
            menu[20, 39] = '♥';
            Print(menu);

            bool exit = false;
            while (!exit)
            {
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey();
                    if (key.Key == ConsoleKey.Escape)
                    {
                        Settings();
                        exit = true;
                    }
                }
            }
        }
        /// <summary>
        /// Руководство по игре
        /// </summary>
        private void HowToPlay()
        {
            string str9 = "HOW TO PLAY PONG GAME";
            string str10 = "SINGLE PLAYER MODE";
            string str11 = "MOVEMENT KEYS";
            string str12 = "TO MOVE YOUR RACKET, YOU NEED TO USE (UP) AND (DOWN) ARROWS ON YOUR KEYBOARD";
            string str13 = "MULTIPLAYER MODE";
            string str14 = "TO MOVE RIGHT RACKET, PLAYER NEED TO USE  (UP) AND (DOWN) ARROWS ON YOUR KEYBOARD";
            string str15 = "TO MOVE LEFT RACKET, PLAYER NEED TO USE (A) AND (Z) KEYS ON YOUR KEYBOARD";
            string str16 = "TO EXIT, PLAYER NEED TO USE (ESC) KEY";
            int j = str12.Length / 2;
            for (int i = 0; i < str9.Length; i++)
            {
                menu[3, 27 + i] = str9[i];
            }
            for (int i = 0; i < str10.Length; i++)
            {
                menu[5, 10 + i] = str10[i];
            }
            for (int i = 0; i < str11.Length; i++)
            {
                menu[6, 10 + i] = str11[i];
            }
            for (int i = 0; i < str12.Length / 2; i++)
            {
                if (i != str12.Length / 2 - 1)
                {
                    menu[8, 10 + i] = str12[i];
                }
                menu[9, 10] = '(';
                menu[9, 11 + i] = str12[j];
                j++;
            }
            for (int i = 0; i < str13.Length; i++)
            {
                menu[12, 10 + i] = str13[i];
            }
            for (int i = 0; i < str11.Length; i++)
            {
                menu[13, 10 + i] = str11[i];
            }
            j = str14.Length / 2;
            for (int i = 0; i < str14.Length / 2; i++)
            {
                menu[15, 10 + i] = str14[i];
                menu[16, 8 + i] = str14[j];
                menu[16, 8 + str14.Length / 2] = 'D';
                j++;
            }
            j = str15.Length / 2;
            for (int i = 0; i < str15.Length / 2; i++)
            {
                menu[18, 10 + i] = str15[i];
                menu[19, 10 + i] = str15[j];
                menu[19, 10 + str15.Length / 2] = 'D';
                j++;
            }
            for (int i = 0; i < str16.Length; i++)
            {
                menu[21, 10 + i] = str16[i];
            }
        }
        /// <summary>
        /// Настройки игры
        /// </summary>
        private void Settings()
        {
            string str1 = "HOW TO PLAY";
            string str2 = "GAME SPEED";
            string str4 = "CREDITS";
            string str3 = "BACK";
            FillTheField(menu);
            for (int i = 0; i < str1.Length; i++)
            {
                menu[10, 32 + i] = str1[i];
            }
            for (int i = 0; i < str2.Length; i++)
            {
                menu[11, 32 + i] = str2[i];
            }
            for (int i = 0; i < str4.Length; i++)
            {
                menu[12, 32 + i] = str4[i];
            }
            for (int i = 0; i < str3.Length; i++)
            {
                menu[13, 32 + i] = str3[i];
            }
            Print(menu);

            int choice = SettigsKeys(str1, str2, str3, str4) - 10;
            if (choice == 0)
            {
                Console.Clear();
                FillTheField(menu);
                HowToPlay();
                Print(menu);

                bool exit = false;
                while (!exit)
                {
                    if (Console.KeyAvailable)
                    {
                        var key = Console.ReadKey();
                        if (key.Key == ConsoleKey.Escape)
                        {
                            Settings();
                            exit = true;
                        }
                    }
                }
            }
            if (choice == 1)
            {
                Console.Clear();
                ChangeTheGameSpeed();
            }
            if (choice == 2)
            {
                Console.Clear();
                Credits();
            }
            if (choice == 3)
            {
                Console.Clear();
                Menu();
            }
        }
        /// <summary>
        /// Настройка скорости игры
        /// </summary>
        private void ChangeTheGameSpeed()
        {
            string str1 = "SLOW";
            string str2 = "NORMAL";
            string str3 = "FAST";
            string str4 = "EXIT";

            FillTheField(menu);
            for (int i = 0; i < str1.Length; i++)
            {
                menu[10, 32 + i] = str1[i];
            }
            for (int i = 0; i < str2.Length; i++)
            {
                menu[11, 32 + i] = str2[i];
            }
            for (int i = 0; i < str3.Length; i++)
            {
                menu[12, 32 + i] = str3[i];
            }
            for (int i = 0; i < str4.Length; i++)
            {
                menu[13, 32 + i] = str4[i];
            }
            Print(menu);

            int choice = SettigsKeys(str1, str2, str3, str4) - 10;

            if (choice == 0)
            {
                gameSpeed = 100;
                Menu();
            }
            if (choice == 1)
            {
                gameSpeed = 80;
                Menu();
            }
            if (choice == 2)
            {
                gameSpeed = 60;
                Menu();
            }
            if (choice == 3)
            {
                Console.Clear();
                Settings();
            }
        }
        /// <summary>
        /// Меню игры
        /// </summary>
        public void Menu()
        {
            string str1 = "1 PLAYER GAME";
            string str2 = "2 PLAYER GAME";
            string str3 = "SETTINGS";
            string str4 = "EXIT";

            FillTheField(menu);
            for (int i = 0; i < str1.Length; i++)
            {
                menu[10, 32 + i] = str1[i];
                menu[11, 32 + i] = str2[i];
            }
            for (int i = 0; i < str3.Length; i++)
            {
                menu[12, 32 + i] = str3[i];
            }
            for (int i = 0; i < str4.Length; i++)
            {
                menu[13, 32 + i] = str4[i];
            }
            Print(menu);

            int choice = MenuKeys(str1, str2, str3, str4) - 10;
            if (choice == 0)
            {
                Console.Clear();
                SinglePlayer();
            }
            if (choice == 1)
            {
                Console.Clear();
                MultiplayerGame();
            }
            if (choice == 2)
            {
                Console.Clear();
                Settings();
            }
        }
    }
}


