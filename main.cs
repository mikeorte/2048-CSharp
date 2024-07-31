using System;
using System.Collections.Generic;

class Program
{
    const int N = 4; // The row and column size of the grid
    static Random rand = new Random();

    static void Main()
    {
        int[,] gameGrid = new int[N, N]; // Creates the grid and fills it with 0s

        InitializeGame(gameGrid); // Initializes the game by placing 2 random tiles
        DisplayGrid(gameGrid); // Displays the initial instance

        while (!IsGameOver(gameGrid))
        {
            Console.WriteLine("Enter 'W' to move up, 'S' to move down, 'A' to move left, or 'D' to move right: ");
            char move = Console.ReadKey().KeyChar;
            Console.WriteLine();

            if (IsValidMove(move))
            {
                MakeMove(gameGrid, move);
                PlaceRandomTile(gameGrid); // Adds new tile
                DisplayGrid(gameGrid); // Displays updated grid
            }
            else
            {
                Console.WriteLine("Invalid move. Please try again.");
            }
        }

        Console.WriteLine("Game Over!");
        int score = ScoreCalculation(gameGrid); // Gets score
        Console.WriteLine("Your Score is: " + score);
    }

    static void InitializeGame(int[,] gameGrid)
    {
        PlaceRandomTile(gameGrid); // Adds new tile
        PlaceRandomTile(gameGrid); // Adds new tile
    }

    static void DisplayGrid(int[,] gameGrid)
    {
        Console.WriteLine("2048 Game");
        Console.WriteLine("-----------------------------");
        for (int row = 0; row < N; row++)
        {
            for (int col = 0; col < N; col++)
            {
                Console.Write(gameGrid[row, col] + "\t");
            }
            Console.WriteLine();
        }
        Console.WriteLine("-----------------------------");
    }

    static void PlaceRandomTile(int[,] gameGrid)
    {
        bool emptyCellFound = false;
        while (!emptyCellFound)
        {
            int row = rand.Next(N); // Generate a random row within the grid size
            int col = rand.Next(N); // Generate a random column within the grid size

            if (gameGrid[row, col] == 0) // Finds empty cell
            {
                int randomValue = rand.Next(100); // Generate a random number between 0 and 99
                int tileValue = (randomValue < 80) ? 2 : 4; // 80% for 2, 20% for 4
                gameGrid[row, col] = tileValue; // Adds to grid
                emptyCellFound = true; // Found empty cell and filled it
            }
        }
    }

    static bool IsGameOver(int[,] gameGrid)
    {
        for (int row = 0; row < N; row++)
        {
            for (int col = 0; col < N; col++)
            {
                if (gameGrid[row, col] == 0 || HasAdjacentEqualTiles(gameGrid, row, col))
                {
                    return false; // If any cell is empty or has adjacent equal tiles, the game is not over
                }
            }
        }
        return true; // If all cells are filled and no adjacent equal tiles, the game is over
    }

    static bool HasAdjacentEqualTiles(int[,] gameGrid, int row, int col)
    {
        if (row > 0 && gameGrid[row, col] == gameGrid[row - 1, col]) return true;
        if (row < N - 1 && gameGrid[row, col] == gameGrid[row + 1, col]) return true;
        if (col > 0 && gameGrid[row, col] == gameGrid[row, col - 1]) return true;
        if (col < N - 1 && gameGrid[row, col] == gameGrid[row, col + 1]) return true;
        return false;
    }

    static int ScoreCalculation(int[,] gameGrid)
    {
        int score = 0;
        for (int row = 0; row < N; row++)
        {
            for (int col = 0; col < N; col++)
            {
                score += gameGrid[row, col];
            }
        }
        return score;
    }

    static bool IsValidMove(char move)
    {
        return move == 'W' || move == 'w' || move == 'S' || move == 's' || move == 'A' || move == 'a' || move == 'D' || move == 'd';
    }

    static void MakeMove(int[,] gameGrid, char move)
    {
        switch (move)
        {
            case 'W':
            case 'w':
                MoveTilesUp(gameGrid);
                break;
            case 'S':
            case 's':
                MoveTilesDown(gameGrid);
                break;
            case 'A':
            case 'a':
                MoveTilesLeft(gameGrid);
                break;
            case 'D':
            case 'd':
                MoveTilesRight(gameGrid);
                break;
        }
    }

    static void MoveTilesUp(int[,] gameGrid)
    {
        for (int col = 0; col < N; col++)
        {
            List<int> tempCol = new List<int>(); // Temp col to hold non-zeros in column
            for (int row = 0; row < N; row++)
            {
                if (gameGrid[row, col] != 0)
                {
                    tempCol.Add(gameGrid[row, col]); // Adds non-zeros to tempCol
                }
            }
            MergeTiles(tempCol); // Merge the tiles in the column

            for (int row = 0; row < N; row++) // Replaces the gameGrid col with the merged tempCol
            {
                gameGrid[row, col] = (row < tempCol.Count) ? tempCol[row] : 0;
            }
        }
    }

    static void MoveTilesDown(int[,] gameGrid)
    {
        for (int col = 0; col < N; col++)
        {
            List<int> tempCol = new List<int>(); // Temp col to hold non-zeros in column
            for (int row = N - 1; row >= 0; row--)
            {
                if (gameGrid[row, col] != 0)
                {
                    tempCol.Add(gameGrid[row, col]); // Adds non-zeros to tempCol, but in reverse (it's down)
                }
            }
            MergeTiles(tempCol); // Merge the tiles in the column

            for (int row = N - 1; row >= 0; row--) // Replaces the gameGrid col with the merged tempCol
            {
                gameGrid[row, col] = (N - 1 - row < tempCol.Count) ? tempCol[N - 1 - row] : 0;
            }
        }
    }

    static void MoveTilesLeft(int[,] gameGrid)
    {
        for (int row = 0; row < N; row++)
        {
            List<int> tempRow = new List<int>(); // Temp row to hold non-zeros in row
            for (int col = 0; col < N; col++)
            {
                if (gameGrid[row, col] != 0)
                {
                    tempRow.Add(gameGrid[row, col]); // Adds non-zeros to tempRow
                }
            }
            MergeTiles(tempRow); // Merge the tiles in the row

            for (int col = 0; col < N; col++) // Replaces the gameGrid row with the merged tempRow
            {
                gameGrid[row, col] = (col < tempRow.Count) ? tempRow[col] : 0;
            }
        }
    }

    static void MoveTilesRight(int[,] gameGrid)
    {
        for (int row = 0; row < N; row++)
        {
            List<int> tempRow = new List<int>(); // Temp row to hold non-zeros in row
            for (int col = N - 1; col >= 0; col--)
            {
                if (gameGrid[row, col] != 0)
                {
                    tempRow.Add(gameGrid[row, col]); // Adds non-zeros to tempRow, but in reverse (it's right)
                }
            }
            MergeTiles(tempRow); // Merge the tiles in the row

            for (int col = N - 1; col >= 0; col--) // Replaces the gameGrid row with the merged tempRow
            {
                gameGrid[row, col] = (N - 1 - col < tempRow.Count) ? tempRow[N - 1 - col] : 0;
            }
        }
    }

    static void MergeTiles(List<int> tiles)
    {
        for (int i = 0; i < tiles.Count - 1; i++)
        {
            if (tiles[i] == tiles[i + 1])
            {
                tiles[i] *= 2;
                tiles.RemoveAt(i + 1);
            }
        }
    }
}
