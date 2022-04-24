using System;

namespace BoardGame
{
    class Program
    {
        static void Main(string[] args)
        {
            //     1   2   3   4   5   6   7   8
            //   ---------------------------------
            // 1 |   |   |   |   |   |   |   |   |
            //   ---------------------------------
            // 2 |   |   |   |   |   |   |   |   |
            //   ---------------------------------
            // 3 |   |   |   |   |   |   |   |   |
            //   ---------------------------------
            // 4 |   |   |   |   |   |   |   |   |
            //   ---------------------------------
            // 5 |   |   |   |   |   |   |   |   |
            //   ---------------------------------
            // 6 |   |   |   |   |   |   |   |   |
            //   ---------------------------------
            // 7 |   |   |   |   |   |   |   |   |
            //   ---------------------------------
            // 8 |   |   |   |   |   |   |   |   |
            //   ---------------------------------

            // 1.  Dynamic Board
            // 2.  Board will be rectangle, rows and column are 6 and 7 in number
            // 3.  Win in a row must not be less than board game
            // 4.  Board can not be smaller than four in a row
            // 5.  Game will support two players
            // 6.  Game board will be multi dimensional arrays
            // 7.  Game markers will be the player number
            // 8.  Game will be initialized with Zeros
            // 9.  Check winning move will validate from where the player position

            // default values
            int boardGameSizeRow = 7;
            int boardGameSizeColumn = 7;
            int winInARow = 4;
            bool keepLooping = true;

            while (keepLooping)
            {
                Console.Clear();

                Console.WriteLine("Welcome to our game.");
                Console.Write("Please select the game board size by selecting a numeric value: ");

                /*
                 *Selecting Row for the game board
                 *
                */
                Console.WriteLine("Enter the Size (Row) of the game board");
                if (!int.TryParse(Console.ReadLine(), out var userBoardSizeRow))
                {
                    Console.WriteLine();
                    Console.WriteLine("Please select a numeric value!");
                    Console.WriteLine("Select any key to start over.");
                    Console.Read();

                    continue;
                }

                /*
                 *Selecting Column for the game board
                 *
                */

                Console.WriteLine("Enter the Size (Column) of the game board");
                if (!int.TryParse(Console.ReadLine(), out var userBoardSizeColumn))
                {
                    Console.WriteLine();
                    Console.WriteLine("Please select a numeric value!");
                    Console.WriteLine("Select any key to start over.");
                    Console.Read();

                    continue;
                }
                if (userBoardSizeRow > userBoardSizeColumn)
                {
                    Console.Write($"Please select the game win in a row must be smaller than {userBoardSizeColumn} : ");
                } else 
                {
                    Console.Write($"Please select the game win in a row must be smaller than {userBoardSizeRow} : ");

                }

                if (!int.TryParse(Console.ReadLine(), out var userWinInARow))
                {
                    Console.WriteLine();
                    Console.WriteLine("Please select a numeric value!");
                    Console.WriteLine("Select any key to start over.");
                    Console.Read();

                    continue;
                }

                boardGameSizeRow = userBoardSizeRow;
                boardGameSizeColumn = userBoardSizeColumn;
                // winInARow = userWinInARow;

                keepLooping = false;
            }

            //int[,] board = {
            //    { 0,0,0,0,0,0,0,1 },
            //    { 0,0,0,0,0,0,1,0 },
            //    { 0,0,0,0,0,1,0,0 },
            //    { 0,0,0,0,1,0,0,0 },
            //    { 0,0,0,1,0,0,0,0 },
            //    { 0,0,0,0,0,0,0,0 },
            //    { 0,0,0,0,0,0,0,0 },
            //    { 0,0,0,0,0,0,0,0 },
            //};

            //DrawGameboard(board);

            int currentPlayer = -1;
            AnyInARowGameEngine gameEngine = new AnyInARowGameEngine(boardGameSizeRow, boardGameSizeColumn, winInARow)
            {
                GameStatus = 0
            };

            do
            {
                Console.Clear();

                currentPlayer = gameEngine.GetNextPlayer(currentPlayer);

                HeadsUpDisplay(currentPlayer);
                DrawGameboard(gameEngine.GameBoard);

                GameResult gameResult = new GameResult();

                do
                {
                    // 3.  As the user places markers on the game update the board then notify which player has a turn
                    Console.Write("Please select a numeric value for the row: ");

                    int userInputRow = Convert.ToInt32(Console.ReadLine()) - 1;
                    
                    Console.Write("Please select a numeric value for the column: ");

                    int userInputCol = Convert.ToInt32(Console.ReadLine()) - 1;

                    gameResult = gameEngine.MakeAMove(gameEngine.GameBoard, currentPlayer, userInputRow, userInputCol);

                    if (!gameResult.ValidMove)
                    {
                        Console.WriteLine(gameResult.Message);
                    }

                    // 3.1 After each turn judge if there is a winner
                    // 3.2 If no winner keep playing by going to step 1.
                    gameEngine.GameStatus = gameEngine.CheckWinner(gameEngine.GameBoard, userInputRow, userInputCol);

                } while (!gameResult.ValidMove);

            } while (gameEngine.GameStatus.Equals(0));

            Console.Clear();
            HeadsUpDisplay(currentPlayer);
            DrawGameboard(gameEngine.GameBoard);

            if (gameEngine.GameStatus.Equals(1))
            {
                Console.WriteLine($"Player {currentPlayer} is the winner!");
            }

            if (gameEngine.GameStatus.Equals(2))
            {
                Console.WriteLine($"The game is a draw!");
            }
        }

        static char GetPlayerMarker(int player)
        {
            if (player % 2 == 0)
            {
                return 'O';
            }

            return 'X';
        }

        static void HeadsUpDisplay(int PlayerNumber)
        {
            // 1.  Provide instructions
            // 1.1 A greeting
            Console.WriteLine("Welcome to the Super Duper Tic Tac Toe Game!");

            // 1.2 Display player sign, Player 1 is X and Player 2 is O
            Console.WriteLine("Player 1: X");
            Console.WriteLine("Player 2: O");
            Console.WriteLine();

            // 1.3 Who's turn is it?
            // 1.4 Instruct the user to enter a number between 1 and 9
            Console.WriteLine($"Player {PlayerNumber} to move, select 1 through 9 from the game board.");
            Console.WriteLine();
        }

        static void DrawGameboard(int[,] board)
        {
            PrintConsole(board.GetLength(1), "     ", "{0}   ", 1);
            PrintConsole(board.GetLength(1), "   -", new string('-', 4), 0);

            for (int row = 0; row < board.GetLength(0); row++)
            {
                string space = "  ";

                if (row > 8)
                {
                    space = " ";
                }

                Console.Write($"{row + 1}{space}");

                for (int col = 0; col < board.GetLength(1); col++)
                {
                    int currentColumn = board[row, col];
                    char currentColumnUI = ' ';

                    if (currentColumn != 0)
                    {
                        currentColumnUI = GetPlayerMarker(currentColumn);
                    }

                    Console.Write($"| {currentColumnUI} ");
                }

                Console.WriteLine("|");

                PrintConsole(board.GetLength(1), "   -", new string('-', 4), 0);
            }
        }

        static void PrintConsole(int loopCounter, string printDigit, string printDigits, int incrementPrintDigits)
        {
            Console.Write(printDigit);

            for (int i = 0; i < loopCounter; i++)
            {
                if (i + incrementPrintDigits > 8)
                {
                    printDigits = printDigits.Replace("   ", "  ");
                }

                Console.Write(printDigits, i + incrementPrintDigits);
            }

            Console.WriteLine();
        }
    }
}