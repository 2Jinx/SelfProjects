using System;
using System.Xml.Linq;

namespace Snake
{
    /// <summary>
    /// Игра змейка
    /// </summary>
    public class Game
    {
        /// <summary>
        /// Игровое поле
        /// </summary>
        private class Field
        {
            protected char[,] field;
            readonly int N;
            readonly int M;

            internal Field()
            {
                this.N = 25;
                this.M = 80;
                this.field = new char[N, M];
            }
            /// <summary>
            /// Вывод игрового поля в консоль
            /// </summary>
            internal void PrintField()
            {
                for (int i = 0; i < N; i++)
                {
                    for (int j = 0; j < M; j++)
                        Console.Write(field[i, j]);
                    Console.WriteLine();
                }
            }
            /// <summary>
            /// Заполнение игрового поля
            /// </summary>
            internal void FillTheField()
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
            /// Добавляет яблоко на игровое поле
            /// </summary>
            /// <param name="IsEaten"></param>
            internal void PlaceAnApple(bool IsEaten)
            {
                Random r = new Random();
                if (IsEaten)
                {
                    var x = r.Next(1, 78);
                    var y = r.Next(1, 24);
                    field[y, x] = '';
                }
            }

            public void InsertTheSnake(int x, int y, int size)
            {
                while (size != 0)
                {
                    field[y, x] = '*';
                    x++;
                    size--;
                }
            }

            public char Get(int x, int y)
            {
                if (x == 3 && y == 75)
                {
                    return 's';
                }
                return field[x, y];
            }

            public void Set(int x1, int y1, int x2, int y2)
            {
                field[x1, y1] = field[x2, y2];
            }

            public void SetChar(int x, int y, char c)
            {
                field[x, y] = c;
            }
            /// <summary>
            /// Добавляет на игровое поле надпись об окончании игры
            /// </summary>
            public void WriteOnTheField()
            {
                string lose = "GAME OVER!";
                for (int i = 0; i < lose.Length; i++)
                {
                    field[7, 35 + i] = lose[i];
                }
                Console.Clear();
                PrintField();
            }

            public void WriteScore(Snake s)
            {
                string score = s.Size.ToString();
                for (int i = 0; i < score.Length; i++)
                    field[3, 75 + i] = score[i];
            }
        }
        /// <summary>
        /// Змейка
        /// </summary>
        private class Snake
        {
            internal int Size { get; set; }
            internal int xHead { get; set; }
            internal int yHead { get; set; }
            internal int xSpeed { get; set; }
            internal int ySpeed { get; set; }
            internal bool FoodCheck { get; set; }
            internal bool IsAlive { get; set; }
            
            public Snake()
            {
                Size = 3;
                xHead = 40;
                yHead = 13;
                xSpeed = -1;
                ySpeed = 0;
                IsAlive = true;
                FoodCheck = true;
            }
            /// <summary>
            /// Управление змейкой с помощью клавиатуры
            /// </summary>
            /// <returns></returns>
            public bool SnakeKeys()
            {
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey();
                    if (key.Key == ConsoleKey.UpArrow && ySpeed == 0)
                    {
                        xSpeed = 0;
                        ySpeed = -1;
                    }
                    if (key.Key == ConsoleKey.DownArrow && ySpeed == 0)
                    {
                        xSpeed = 0;
                        ySpeed = 1;
                    }
                    if (key.Key == ConsoleKey.RightArrow && xSpeed == 0)
                    {
                        xSpeed = 1;
                        ySpeed = 0;
                    }
                    if (key.Key == ConsoleKey.LeftArrow && xSpeed == 0)
                    {
                        xSpeed = -1;
                        ySpeed = 0;
                    }

                    if (key.KeyChar == 'Q' || key.KeyChar == 'q')
                        return false;
                }
                return true;
            }
            /// <summary>
            /// Взаимодействие змейки с объектами игрового поля
            /// (яблоки, стены, препятствия)
            /// </summary>
            /// <param name="f"></param>
            /// <returns></returns>
            public bool Collision(Field f)
            {
                if (f.Get(yHead + ySpeed, xHead + xSpeed) == '║')
                {
                    f.WriteOnTheField();
                    return false;
                }
                if (f.Get(yHead + ySpeed, xHead + xSpeed) == '═')
                {
                    f.WriteOnTheField();
                    return false;
                }
                if (f.Get(yHead + ySpeed, xHead + xSpeed) == '')
                {
                    Size++;
                    f.SetChar(yHead + ySpeed, xHead + xSpeed, ' ');
                    FoodCheck = true;
                }
                if (f.Get(yHead + ySpeed, xHead + xSpeed) == '*')
                {
                    f.WriteOnTheField();
                    return false;
                }
                return true;
            }
            /// <summary>
            /// Передвижение змейки по игровому полю
            /// </summary>
            /// <param name="f"></param>
            public void Movement(Field f)
            {
                char curr = f.Get(yHead, xHead);
                f.Set(yHead, xHead, yHead + ySpeed, xHead + xSpeed);
                f.SetChar(yHead + ySpeed, xHead + xSpeed, curr);
                Console.Clear();
                f.PlaceAnApple(FoodCheck);
                FoodCheck = false;
                f.PrintField();
            }
        }

        Field f;
        Snake s;

        public Game()
        {
            this.f = new Field();
            this.s = new Snake();
        }
        /// <summary>
        /// Запуск игры
        /// </summary>
        public void StartTheGame()
        {
            f.FillTheField();
            f.InsertTheSnake(s.xHead, s.yHead, s.Size);
            f.PrintField();

            while (s.IsAlive)
            {
                if (!s.Collision(f))
                    break;

                s.Movement(f);
                f.WriteScore(s);
                

                for (int i = 0; i < 50000000; i++)
                {

                }

                s.xHead += s.xSpeed;
                s.yHead += s.ySpeed;

                if (!s.SnakeKeys())
                    break;
            }
        }
    }
}

