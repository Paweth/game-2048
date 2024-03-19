/*
C# implementation of 2048 game, based on RosettaCode exercise description (https://rosettacode.org/wiki/2048):
"Task

Implement a 2D sliding block puzzle game where blocks with numbers are combined to add their values.

Rules of the game
    - The rules are that on each turn the player must choose a direction (up, down, left or right).
    - All tiles move as far as possible in that direction, some move more than others.
    - Two adjacent tiles (in that direction only) with matching numbers combine into one bearing the sum of those numbers.
    - A move is valid when at least one tile can be moved, if only by combination.
    - A new tile with the value of 2 is spawned at the end of each turn at a randomly chosen empty square   (if there is one).
    - Adding a new tile on a blank space. Most of the time, a new 2 is to be added, and occasionally (10% of the time), a 4.
    - To win, the player must create a tile with the number 2048.
    - The player loses if no valid moves are possible."
*/

using System;

namespace _2048game
{
    public static class Game
    {
        public static int[,] fields;
        static int sizeX;
        static int sizeY;
        public static int emptyCount;
        static Game()
        {
            emptyCount = 0;
            fields = new int[4,4];
            sizeX = fields.GetLength(0);
            sizeY = fields.GetLength(1);
            for(int i = 0; i < sizeX; i++)
            {
                for(int j = 0; j < sizeY; j++)
                {
                    fields[i, j] = 0;
                    emptyCount++;
                }
            }
        }
        static public bool IsAnyMoveAvaible(bool[] directions)
        {
            if(directions.GetLength(0) != 4)
            {
                throw new ArgumentException("input array should be size of 4", "directions");
            }
            for(int i = 0; i < 4; i++)
            {
                if(directions[i] == true)
                {
                    return true;
                }
            }
            return false;
        }
        static public bool[] CheckAvaibleMoves()
        {
            bool[] directionAvaible = new bool[4];
            for(int i = 0; i < directionAvaible.GetLength(0); i++)
            {
                directionAvaible[i] = false;
            }
            for(int i = 0; i < sizeX; i++)
            {
                for(int j = 0; j < sizeY; j++)
                {
                    //up
                    if(i - 1 >= 0 && ((fields[i, j] == fields[i - 1, j] || fields[i - 1, j] == 0) && fields[i, j] != 0))
                    {
                        directionAvaible[0] = true;
                    }
                    //right
                    if(j + 1 < sizeY && ((fields[i, j] == fields[i, j + 1] || fields[i, j + 1] == 0) && fields[i, j] != 0))
                    {
                        directionAvaible[1] = true;
                    }
                    //down
                    if(i + 1 < sizeX && ((fields[i, j] == fields[i + 1, j] || fields[i + 1, j] == 0) && fields[i, j] != 0))
                    {
                        directionAvaible[2] = true;
                    }
                    //left                        
                    if(j - 1 >= 0 && ((fields[i, j] == fields[i, j - 1] || fields[i, j - 1] == 0) && fields[i, j] != 0))
                    {
                        directionAvaible[3] = true;
                    }
                    if(directionAvaible[0] == true && directionAvaible[1] == true
                    && directionAvaible[2] == true && directionAvaible[3] == true)
                    {
                        return directionAvaible;
                    }
                }
            }
            return directionAvaible;
        }
        static public void PlayerMove(bool[] directions)
        {
            bool wasMoved = false;
            ConsoleKeyInfo keyInfo;
            while(!wasMoved)
            {
                keyInfo = Console.ReadKey();
                switch (keyInfo.Key)
                {
                    case ConsoleKey.UpArrow:
                        if(directions[0] == true)
                        {
                            ShiftUp();
                            wasMoved = true;
                        }
                        break;
                    case ConsoleKey.DownArrow:
                        if(directions[2] == true)
                        {
                            ShiftDown();
                            wasMoved = true;                        
                        }
                        break;
                    case ConsoleKey.RightArrow:
                        if(directions[1] == true)
                        {
                            ShiftRight();
                            wasMoved = true;                        
                        }
                        break;
                    case ConsoleKey.LeftArrow:
                        if(directions[3] == true)
                        {
                            ShiftLeft();
                            wasMoved = true;                        
                        }
                        break;
                }
            }
        }

        static public void ShiftUp()
        {
            bool recentAddition;
            int lastPosition;
            for(int j = 0; j < sizeY; j++)
            {
                lastPosition = 1;
                for(int i = 0; lastPosition < sizeX; i++)
                {
                    recentAddition = false;
                    if(i == lastPosition)
                    {
                        lastPosition++;
                    }
                    while(lastPosition < sizeX)
                    {
                        if(fields[i, j] == 0)
                        {
                            if(fields[lastPosition, j] != 0)
                            {
                                fields[i, j] = fields[lastPosition, j];
                                fields[lastPosition, j] = 0;
                            }
                        }
                        else
                        {
                            if(fields[lastPosition, j] != 0)
                            {
                                if(fields[i, j] == fields[lastPosition, j] && !recentAddition)
                                {
                                    fields[i, j] += fields[lastPosition, j];
                                    recentAddition = true;
                                    fields[lastPosition, j] = 0;
                                    emptyCount++;
                                }
                                else break;
                            }
                        }
                        lastPosition++;
                    }
                }
            }
        }
        static public void ShiftDown()
        {
            bool recentAddition;
            int lastPosition;
            for(int j = 0; j < sizeY; j++)
            {
                lastPosition = sizeX - 1;
                for(int i = sizeX - 1; lastPosition >= 0; i--)
                {
                    recentAddition = false;
                    if(i == lastPosition)
                    {
                        lastPosition--;
                    }
                    while(lastPosition >= 0)
                    {
                        if(fields[i, j] == 0)
                        {
                            if(fields[lastPosition, j] != 0)
                            {
                                fields[i, j] = fields[lastPosition, j];
                                fields[lastPosition, j] = 0;
                            }
                        }
                        else
                        {
                            if(fields[lastPosition, j] != 0)
                            {
                                if(fields[i, j] == fields[lastPosition, j] && !recentAddition)
                                {
                                    fields[i, j] += fields[lastPosition, j];
                                    recentAddition = true;
                                    fields[lastPosition, j] = 0;
                                    emptyCount++;
                                }
                                else break;
                            }
                        }
                        lastPosition--;
                    }
                }
            }
        }
        static public void ShiftLeft()
        {
            bool recentAddition;
            int lastPosition;
            for(int i = 0; i < sizeX; i++)
            {
                lastPosition = 1;
                for(int j = 0; lastPosition < sizeY; j++)
                {
                    recentAddition = false;
                    if(j == lastPosition)
                    {
                        lastPosition++;
                    }
                    while(lastPosition < sizeY)
                    {
                        if(fields[i, j] == 0)
                        {
                            if(fields[i, lastPosition] != 0)
                            {
                                fields[i, j] = fields[i, lastPosition];
                                fields[i, lastPosition] = 0;
                            }
                        }
                        else
                        {
                            if(fields[i, lastPosition] != 0)
                            {
                                if(fields[i, j] == fields[i, lastPosition] && !recentAddition)
                                {
                                    fields[i, j] += fields[i, lastPosition];
                                    recentAddition = true;
                                    fields[i, lastPosition] = 0;
                                    emptyCount++;
                                }
                                else break;
                            }
                        }
                        lastPosition++;
                    }
                }
            }
        }
        static public void ShiftRight()
        {
            bool recentAddition;
            int lastPosition;
            for(int i = 0; i < sizeX; i++)
            {
                lastPosition = sizeY - 2;
                for(int j = sizeY - 1; lastPosition >= 0; j--)
                {
                    recentAddition = false;
                    if(j == lastPosition)
                    {
                        lastPosition--;
                    }
                    while(lastPosition >= 0)
                    {
                        if(fields[i, j] == 0)
                        {
                            if(fields[i, lastPosition] != 0)
                            {
                                fields[i, j] = fields[i, lastPosition];
                                fields[i, lastPosition] = 0;
                            }
                            lastPosition--;
                        }
                        else
                        {
                            if(fields[i, lastPosition] != 0)
                            {
                                if(fields[i, j] == fields[i, lastPosition] && !recentAddition)
                                {
                                    fields[i, j] += fields[i, lastPosition];
                                    recentAddition = true;
                                    fields[i, lastPosition] = 0;
                                    emptyCount++;
                                }
                                else break;
                            }
                            lastPosition--;
                        }
                    }
                }
            }
        }
        static public void RenderFields()
        {
            Console.Clear();
            for(int i = 0; i < sizeX; i++)
            {
                Console.WriteLine("|------|------|------|------|");
                for(int j = 0; j < sizeY; j++)
                {
                    const int boxWidth = 6;
                    string strValue = Convert.ToString(fields[i, j]);
                    int strLength = strValue.Length;
                    int widthDiff = boxWidth - strLength;
                    Console.Write("|");
                    for(int n = 0; n < widthDiff; n++)
                    {
                        Console.Write(" ");
                    }
                    if(fields[i, j] != 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write(strValue);
                        Console.ForegroundColor = ConsoleColor.Gray;
                    }
                    else Console.Write(strValue);
                }
                Console.WriteLine("|");
            }
            Console.WriteLine("|------|------|------|------|");
        }
        static public void GenerateNewValue()
        {
            Random random = new Random();
            int value = random.Next(1, 101);//<1,100>
            if(value <= 90)
            {
                SetRandomEmptyField(2);
            }
            if(value > 90)
            {
                SetRandomEmptyField(4);
            }
        }
        static private void SetRandomEmptyField(int number)
        {
            int n = emptyCount;
            Random random = new Random();
            for(int i = 0; i < sizeX; i++)
            {
                for(int j = 0; j < sizeY; j++)
                {
                    if(fields[i, j] == 0)
                    {
                        int x = random.Next(1, n + 1);
                        if(x == 1)
                        {
                            fields[i, j] = number;
                            emptyCount--;
                            return;
                        }
                        n--;
                    }
                }
            }
        }
        static public void PlayGame()
        {
            Console.WriteLine("");
            bool[] moves = Game.CheckAvaibleMoves();
            Game.GenerateNewValue();
            while(true)
            {
                Game.RenderFields();
                moves = Game.CheckAvaibleMoves();
                for(int j = 0; j < 4; j++)
                {
                    Console.WriteLine($"direction[{j}] = {moves[j]} ");
                }
                if(IsAnyMoveAvaible(moves))
                {
                    Game.PlayerMove(moves);
                }
                else
                {
                    Console.WriteLine("No more moves. Game over!");
                    break;
                } 
                Game.GenerateNewValue();
            }
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Game.PlayGame();
        }
    }
}