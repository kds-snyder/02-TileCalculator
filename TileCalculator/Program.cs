using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileCalculator
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Get from user: area width & length, and  tile size & price
                decimal width = inputPosDecNum("What is the width to tile? (ft): ");
                decimal length = inputPosDecNum("What is the length to tile ? (ft): ");
                decimal tileSize = inputPosDecNum("What is the tile size? (sq ft): ");
                decimal tilePrice = inputPosDecNum(" How much per tile ? ($): ");                

                // Calculate area to tile, number of tiles needed, and cost
                decimal areaToTile = length * width;
                decimal numTiles = calcNumTiles(areaToTile, tileSize);
                decimal totalCost = numTiles * tilePrice;

                // Output the results
                string totalCostDollars = String.Format("{0:C2}", totalCost);
                Console.WriteLine("Area to tile: {0} sq ft", areaToTile);
                Console.WriteLine("Required # of tiles: {0}", numTiles);
                Console.WriteLine("--");
                Console.WriteLine("Total cost: {0}", totalCostDollars);
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred: ");
                Console.WriteLine(e.Message);
            }

            Console.ReadLine();
        }

        // Output message, then read in a decimal number that must be greater than 0
        // If input is invalid, keep prompting
        // When input is valid, return the decimal number
        static decimal inputPosDecNum(string message)
        {
            decimal resultNum = 0;
            while (true)
            {
                // Output message, and read user input
                Console.Write(message);
                string input = Console.ReadLine();

                // Attempt to convert input to decimal
                // If result is positive decimal number, return the result,
                //    else output appropriate message
                if (Decimal.TryParse(input, out resultNum))
                {
                    if (resultNum > 0)
                    {
                        return resultNum;
                    }
                    else
                    {
                        Console.WriteLine("Please enter a number greater than 0.");
                    }
                }
                else
                {
                    Console.WriteLine("Please enter a valid number.");
                }
            }
        }
        // Calculate number of tiles needed for room
        // Input is: area to tile, and tile size        
        // Return the calculated number
        static int calcNumTiles(decimal area, decimal size)
        {
            // Calculate number of tiles
            // If size does not divide evenly into area, an extra tile is needed
            int numTiles = (int) (area / size);
            if (Decimal.Remainder(area, size) != 0)
            {
                numTiles++;
            }
            return numTiles;
        }
    }      
}
