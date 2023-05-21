using System;
using System.Drawing;
using System.Threading;
using System.Xml.Linq;

namespace Pong
{
    public class PongGame
    {
        protected char[,] field;
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
            firstPlayerPoints = 0;
            secondPlayerPoints = 0;
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
        /// <summary>
        /// Заполнение игрового поля
        /// </summary>
        private void FillTheField()
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
        /// Поведение мяча на столкновения
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
        /// PongGame
        /// </summary>
        internal void Game()
        {
            int i = 0;
            FillTheField();
            PrintField();
            
            while (firstPlayerPoints != 7 && secondPlayerPoints != 7)
            {
                Collision();
                char cur = field[ballY, ballX];
                field[ballY, ballX] = field[ballY + ballSpeedY, ballX + ballSpeedX];
                field[ballY + ballSpeedY, ballX + ballSpeedX] = cur;
                Console.Clear();
                PrintField();
                i++;

                Thread.Sleep(80);

                ballX += ballSpeedX;
                ballY += ballSpeedY;

                rightRacketY += rightRacketSpeedY;
                leftRacketY += leftRacketSpeedY;

                rightRacketSpeedY = 0;
                leftRacketSpeedY = 0;
                if (!Keys())
                    break;
            }

            if ((firstPlayerPoints == 7 || secondPlayerPoints == 7) && firstPlayerPoints > secondPlayerPoints)
            {
                Console.WriteLine(" GAME OVER! FIRST PLAYER WON!");
            }
            else if ((firstPlayerPoints == 7 || secondPlayerPoints == 7) && firstPlayerPoints < secondPlayerPoints)
            {
                Console.WriteLine(" GAME OVER! SECOND PLAYER WON!");
            }
        }
    }
}


