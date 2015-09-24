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
                long numTiles = calcNumTiles(areaToTile, tileSize);
                decimal totalCost = numTiles * tilePrice;

                // Output the results
                string totalCostDollars = String.Format("{0:C2}", totalCost);
                Console.WriteLine("Area to tile: {0} sq ft", areaToTile);
                Console.WriteLine("Required # of tiles: {0}", numTiles);
                Console.WriteLine("--");
                Console.WriteLine("Total cost: {0}", totalCostDollars);

                // Draw the area to be tiled
                Console.WriteLine("--");
                Console.WriteLine("This is the area to tile: ");
                drawArea(length, width);
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
        static long calcNumTiles(decimal area, decimal size)
        {
            // Calculate number of tiles
            // If size does not divide evenly into area, an extra tile is needed
            long numTiles = (long)(area / size);
            if (Decimal.Remainder(area, size) != 0)
            {
                numTiles++;
            }
            return numTiles;
        }
        /* Draw a representation of an area
           Input: length, width 
           (The length and width values are rounded to the nearest integer, 
             and adjusted if too large)
           The drawing looks like this:

                                 Width: xxxx
                Length: oooooooooooooooooooooooooooooooo
                yyy     o                              o
                        o                              o
                        o                              o
                        o                              o
                        oooooooooooooooooooooooooooooooo

        */
        static void drawArea(decimal length, decimal width)
        {

            // Calculate length & width for drawing rectangle
            int drawLength;
            int drawWidth;
            calcDrawLengthWidth(length, width, out drawLength, out drawWidth);

            // Build length titles to calculate left indent of rectangle
            // The left indent is the greater of the lengths of the strings
            //   "Length:" and the string representation of the length number
            string lengthTitle = "Length: ";
            string lengthNum = length.ToString() + " ";
            int indentLeft = Math.Max(lengthTitle.Length, lengthNum.Length);


            // Build width title to calculate indent for width title
            // The title indent is  (drawing width - length of the title)/2
            string widthTitle = "Width: " + width;
            //Console.WriteLine("Indent left: {0}, indent width title: {1}", 
            //indentLeft, (drawWidth - widthTitle.Length) / 2);

            // Write the width title, indented by the left indent and the title indent
            repeatChartoConsole(' ', (((int)drawWidth - widthTitle.Length) / 2) + indentLeft);
            Console.WriteLine(widthTitle);

            // Write the length title
            Console.Write(lengthTitle);

            // Write padding after length title if necessary
            if (lengthNum.Length > lengthTitle.Length)
            {
                repeatChartoConsole(' ', lengthNum.Length - lengthTitle.Length);
            }

            // Write the top line of the rectangle            
            repeatChartoConsole('o', drawWidth);
            Console.Write(Environment.NewLine);

            // Write the second line: write the length of the rectangle
            Console.Write(lengthNum);

            // Write padding after length if necessary
            if (lengthTitle.Length > lengthNum.Length)
            {
                repeatChartoConsole(' ', lengthTitle.Length - lengthNum.Length);
            }

            // Write the rest of the second line
            writeRectangleMidLine('o', drawWidth, 0);
           
            // Write the middle lines if any
            // (Two lines were already written, and the bottom line will be written)
            if (drawLength > 3)
            {
                for (int i = 1; i <= drawLength-3; i++)
                {
                    writeRectangleMidLine('o', drawWidth, indentLeft);
                }
            }

            // Write the bottom line
            writeRectangleBottomLine('o', drawWidth, indentLeft);
        }

        // Write a middle line of the rectangle, followed by a new line
        // Input: character for outside lines, drawing width, indent
        static void writeRectangleMidLine(char charRect, int width, int indent)
        {
            if (indent > 0)
            {
                repeatChartoConsole(' ', indent);
            }
            Console.Write(charRect);
            repeatChartoConsole(' ', width - 2);
            Console.Write(charRect);
            Console.Write(Environment.NewLine);
        }

        // Write the bottom line of the rectangle, followed by a new line
        // Input: character for line, drawing width, indent
        static void writeRectangleBottomLine(char charRect, int width, int indent)
        {
            if (indent > 0)
            {
                repeatChartoConsole(' ', indent);
            }            
            repeatChartoConsole(charRect, width);            
            Console.Write(Environment.NewLine);
        }

        // Write the specified character to the console the specified number of times
        // without writing a newline
        // if the number of times is 0 or less, do nothing
        static void repeatChartoConsole(char charToWrite, int num)
        {
            if (num > 0)
            {
                for (int i = 0; i < num; i++)
                {
                    Console.Write(charToWrite);
                }
            }
        }

        // Calculate length & width for drawing rectangle
        // Input length & width are decimal
        // Output length & width are long integer, rounded
        // If length or width is less than 3, scale up
        // If length or width is greater than 100, scale down
        static void calcDrawLengthWidth(decimal inputLength, decimal inputWidth,
                        out int outputLength, out int outputWidth)
        {
            // Round length and width
            // If midway between two numbers, take the value farther from 0
            long calcOutputLength =
                (long)Decimal.Round(inputLength, System.MidpointRounding.AwayFromZero);
            long calcOutputWidth =
                (long)Decimal.Round(inputWidth, System.MidpointRounding.AwayFromZero);

            // Adjust values if necessary so that drawing is feasible
            // Rectangle should have at least 3 lines
            if (calcOutputLength < 3)
            {
                calcOutputLength *= 3;
                //Console.WriteLine("length adjusted to {0}", calcOutputLength);
                if (calcOutputWidth <= 90)
                {
                    calcOutputWidth *= 3;
                    //Console.WriteLine("width adjusted to {0}", calcOutputWidth);
                }
            }

            // Width shouldn't be too wide
            if (calcOutputWidth > 90)
            {
                decimal ratioLengthtoWidth = (decimal)calcOutputLength / (decimal)calcOutputWidth;
                //Console.WriteLine("Ratio length to width: {0}", ratioLengthtoWidth);
                calcOutputWidth = 90;
                //Console.WriteLine("width adjusted to {0}", calcOutputWidth);
                calcOutputLength = Math.Max(3,
                            (long)((decimal)(calcOutputLength * ratioLengthtoWidth)));
                //Console.WriteLine("length adjusted to {0}", calcOutputLength);
            }

            // Rectangle shouldn't be too long
            if (calcOutputLength > 40)
            {
                calcOutputLength = 40;
                //Console.WriteLine("length adjusted to {0}", calcOutputLength);
            }
            outputLength = (int)calcOutputLength;
            outputWidth = (int)calcOutputWidth;
        }

        
    }             
}
