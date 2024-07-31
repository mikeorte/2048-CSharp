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

            if (move == 'W' || move == 'w')
                MoveTilesUp(gameGrid);
            else if (move == 'S' || move == 's')
                MoveTilesDown(gameGrid);
            else if (move == 'A' || move == 'a')
                MoveTilesLeft(gameGrid);
            else if (move == 'D' || move == 'd')
                MoveTilesRight(gameGrid);
            else
            {
                Console.WriteLine("Invalid move. Please try again.");
                continue; // Repeats while, to not add new tile or redisplay grid
            }

            PlaceRandomTile(gameGrid); // Adds new tile
            DisplayGrid(gameGrid); // Displays updated grid
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
                if (gameGrid[row, col] == 0)
                {
                    return false; // If any cell is empty, the game is not over
                }
            }
        }
        return true; // If all cells are filled, the game is over
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

    static void MoveTilesUp(int[,] gameGrid)
    {
        int[,] tempGrid = new int[N, N]; // Temp grid to hold non-zeros

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

            for (int row = 0; row < tempCol.Count; row++) // Replaces the tempGrid col with the merged tempCol
            {
                tempGrid[row, col] = tempCol[row];
            }
        }

        Array.Copy(tempGrid, gameGrid, tempGrid.Length); // Applies all the changes
    }

    static void MoveTilesDown(int[,] gameGrid)
    {
        int[,] tempGrid = new int[N, N]; // Temp grid to hold non-zeros

        for (int col = 0; col < N; col++)
        {
            List<int> tempCol = new List<int>(); // Temp col to hold non-zeros in column
            for (int row = 0; row < N; row++)
            {
                if (gameGrid[N - row - 1, col] != 0)
                {
                    tempCol.Add(gameGrid[N - row - 1, col]); // Adds non-zeros to tempCol, but in reverse (it's down)
                }
            }
            MergeTiles(tempCol); // Merge the tiles in the column

            for (int row = 0; row < tempCol.Count; row++) // Replaces the tempGrid col with the merged tempCol
            {
                tempGrid[N - row - 1, col] = tempCol[row];
            }
        }

        Array.Copy(tempGrid, gameGrid, tempGrid.Length); // Applies all the changes
    }

    static void MoveTilesLeft(int[,] gameGrid)
    {
        int[,] tempGrid = new int[N, N]; // Temp grid to hold non-zeros

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

            for (int col = 0; col < tempRow.Count; col++) // Replaces the tempGrid row with the merged tempRow
            {
                tempGrid[row, col] = tempRow[col];
            }
        }

        Array.Copy(tempGrid, gameGrid, tempGrid.Length); // Applies all the changes
    }

    static void MoveTilesRight(int[,] gameGrid)
    {
        int[,] tempGrid = new int[N, N]; // Temp grid to hold non-zeros

        for (int row = 0; row < N; row++)
        {
            List<int> tempRow = new List<int>(); // Temp row to hold non-zeros in row
            for (int col = 0; col < N; col++)
            {
                if (gameGrid[row, N - col - 1] != 0)
                {
                    tempRow.Add(gameGrid[row, N - col - 1]); // Adds non-zeros to tempRow, but in reverse (it's right)
                }
            }
            MergeTiles(tempRow); // Merge the tiles in the row

            for (int col = 0; col < tempRow.Count; col++) // Replaces the tempGrid row with the merged tempRow
            {
                tempGrid[row, N - col - 1] = tempRow[col];
            }
        }

        Array.Copy(tempGrid, gameGrid, tempGrid.Length); // Applies all the changes
    }

    static void MergeTiles(List<int> row)
    {
        int i = 0; // Start index
        int j = 1; // Next index (i+1)

        while (j < row.Count) // Loop through the row and merge adjacent tiles with same value
        {
            if (row[i] == row[j] && row[i] != 0) // If adjacent tiles have the same value AND not zero, merge
            {
                row[i] *= 2; // Double the value of the current tile
                row[j] = 0;  // Set the value of the next tile to zero
                i = j + 1;   // Move the start index to the next element after the merged tiles
                j = i + 1;   // Move the next index to the element after the start index
            }
            else
            {
                if (row[i] == 0 && row[j] != 0) // If the current tile is zero AND the next tile is not zero, swap
                {
                    int temp = row[i];
                    row[i] = row[j];
                    row[j] = temp; // Moves the non-zero tile towards the beginning of row
                }

                // Move to the next pair of elements in the row to compare
                i++;
                j++;
            }
        }
    }
}
