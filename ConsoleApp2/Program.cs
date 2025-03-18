using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        class Battleship
        {
            // Game variables 
            static char[,] board = new char[10, 10];
            static List<int> shipSizes = new List<int> { 2, 3, 3, 4, 5 }; // Ship sizes 
            static int guessesRemaining = 20;

            static void Main(string[] args)
            {
                // Initialize the game 
                InitializeBoard();
                PlaceShips();
                ShowGameMenu();

                // Game loop 
                while (guessesRemaining > 0 && shipSizes.Count > 0)
                {
                    DisplayBoard();  // Show the board with the rules 
                    Console.WriteLine($"Guesses remaining: {guessesRemaining}");

                    // Get player input 
                    string input = Console.ReadLine().ToUpper();  // Convert to uppercase to handle input like "a1" or "A1" 

                    // Validate the input (e.g., "A1", "J10") 
                    if (IsValidCoordinate(input))
                    {
                        // Process the guess 
                        HandlePlayerGuess(input);
                        guessesRemaining--;
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Please enter coordinates in the correct format (e.g., A1, J10).");
                    }
                }

                // Game Over Check 
                if (shipSizes.Count == 0)
                {
                    Console.WriteLine("Congratulations! You sank all the ships.");
                }
                else
                {
                    Console.WriteLine("Game Over! You ran out of guesses.");
                }
            }

            // Display the game rules and the menu for difficulty 
            static void ShowGameMenu()
            {
                Console.Clear();
                Console.WriteLine("Welcome to Battleship!");
                Console.WriteLine("Choose Difficulty:");
                Console.WriteLine("1. Easy");
                Console.WriteLine("2. Medium");
                Console.WriteLine("3. Hard");
                Console.WriteLine("Enter your choice (1-3): ");
                string choice = Console.ReadLine();

                // Set difficulty (you can expand this based on difficulty level) 
                switch (choice)
                {
                    case "1":
                        guessesRemaining = 30; // Easy 
                        break;
                    case "2":
                        guessesRemaining = 20; // Medium 
                        break;
                    case "3":
                        guessesRemaining = 15; // Hard 
                        break;
                    default:
                        Console.WriteLine("Invalid choice, defaulting to Medium.");
                        guessesRemaining = 20;
                        break;
                }
            }

            // Initialize the board with water ('~') and display instructions 
            static void InitializeBoard()
            {
                for (int i = 0; i < 10; i++)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        board[i, j] = '~'; // '~' represents water 
                    }
                }
            }

            // Place ships randomly on the board
            static void PlaceShips()
            {
                Random rand = new Random();

                foreach (int shipSize in shipSizes)
                {
                    bool shipPlaced = false;
                    while (!shipPlaced)
                    {
                        // Randomly choose orientation: 0 for horizontal, 1 for vertical
                        bool isHorizontal = rand.Next(2) == 0;
                        int row = rand.Next(0, 10);
                        int col = rand.Next(0, 10);

                        // Check if the ship can fit in the selected orientation
                        if (isHorizontal && col + shipSize <= 10)
                        {
                            bool canPlace = true;
                            for (int i = 0; i < shipSize; i++)
                            {
                                if (board[row, col + i] == 'S')
                                {
                                    canPlace = false;
                                    break;
                                }
                            }

                            if (canPlace)
                            {
                                for (int i = 0; i < shipSize; i++)
                                {
                                    board[row, col + i] = 'S';
                                }
                                shipPlaced = true;
                            }
                        }
                        else if (!isHorizontal && row + shipSize <= 10)
                        {
                            bool canPlace = true;
                            for (int i = 0; i < shipSize; i++)
                            {
                                if (board[row + i, col] == 'S')
                                {
                                    canPlace = false;
                                    break;
                                }
                            }

                            if (canPlace)
                            {
                                for (int i = 0; i < shipSize; i++)
                                {
                                    board[row + i, col] = 'S';
                                }
                                shipPlaced = true;
                            }
                        }
                    }
                }
            }

            // Display the current game board with coordinates 
            static void DisplayBoard()
            {
                Console.Clear();

                // Display the game rules at the top of the board 
                Console.WriteLine("Game Rules:");
                Console.WriteLine("1. The board is 10x10 with columns labeled 1-10 and rows labeled A-J.");
                Console.WriteLine("2. You have to guess the location of the ships (S).");
                Console.WriteLine("3. A hit will be marked as 'X', and a miss will be marked as 'O'.");
                Console.WriteLine("4. You have a limited number of guesses based on difficulty.");
                Console.WriteLine("5. The game ends when all ships are sunk or you run out of guesses.\n");

                // Print the column numbers (1-10), ensuring proper spacing for 2-digit numbers 
                Console.Write("    "); // Extra space before the column labels for alignment 
                for (int i = 1; i <= 10; i++)
                {
                    Console.Write(i.ToString().PadLeft(2) + " ");
                }
                Console.WriteLine();  // Move to the next line after printing column numbers 

                // Print the board itself 
                for (int i = 0; i < 10; i++)
                {
                    // Print the row label (A-J) for each row, ensuring alignment 
                    Console.Write((char)(i + 'A') + "  "); // Add extra space for single-letter row labels 

                    for (int j = 0; j < 10; j++)
                    {
                        // Print each cell in the row, ensuring spacing is consistent 
                        Console.Write(board[i, j].ToString().PadLeft(2) + " "); // Pad single characters 
                    }
                    Console.WriteLine();
                }
            }

            // Validate input coordinates 
            static bool IsValidCoordinate(string input)
            {
                if (input.Length < 2 || input.Length > 3)
                    return false;

                // The first character should be a letter between A-J 
                char row = input[0];
                if (row < 'A' || row > 'J')
                    return false;

                // The remaining part should be the column number (1-10) 
                string columnStr = input.Substring(1);
                if (int.TryParse(columnStr, out int column))
                {
                    if (column >= 1 && column <= 10)
                    {
                        return true;
                    }
                }

                return false;
            }

            // Handle the player's guess and update the board 
            static void HandlePlayerGuess(string input)
            {
                // Extract the row (A-J) and column (1-10) from the input 
                char row = input[0];
                string columnStr = input.Substring(1);  // The rest is the column part 
                int column = int.Parse(columnStr); // Convert column to integer 

                // Convert row (A-J) to an index (0-9) 
                int rowIndex = row - 'A';

                // Convert column (1-10) to an index (0-9) 
                int columnIndex = column - 1;

                // Ensure that the coordinates are valid 
                if (rowIndex >= 0 && rowIndex < 10 && columnIndex >= 0 && columnIndex < 10)
                {
                    // Perform your action with the valid coordinates 
                    if (board[rowIndex, columnIndex] == 'S')  // Ship hit 
                    {
                        board[rowIndex, columnIndex] = 'X'; // Mark as hit 
                        shipSizes.RemoveAt(shipSizes.IndexOf(board[rowIndex, columnIndex] == 'S' ? 1 : 0));
                        Console.WriteLine("Hit!");
                    }
                    else if (board[rowIndex, columnIndex] == '~')  // Water 
                    {
                        board[rowIndex, columnIndex] = 'O'; // Mark as miss 
                        Console.WriteLine("Miss!");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid coordinate. Please enter a valid coordinate.");
                }
            }

        }

    }

}
