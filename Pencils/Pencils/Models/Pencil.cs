using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pencils.Models
{
    public class Pencil
    {
        public const int DEFAULT_MAX_DURABILITY = 10000;
        public const int DEFAULT_INITIAL_LENGTH = 8;

        // The number of characters that can be written with a sharp point
        private int MaxPointDurability { get; }

        // lowercase characters use 1 durability, uppercase use 2
        public int PointDurability { get; private set; }

        // Sharpening uses 1 unit of length
        public int CurrentLength { get; private set; }

        public Pencil(int maxDurability = DEFAULT_MAX_DURABILITY, int initialLength = DEFAULT_INITIAL_LENGTH)
        {
            // initialize PointDurability and store the maximum for sharpening
            PointDurability = MaxPointDurability = maxDurability;
            CurrentLength = initialLength;
        }

        /// <summary>
        /// Writes the input string to the given page, pending durability
        /// </summary>
        /// <param name="toWrite"></param>
        /// <param name="page"></param>
        public void WriteToPage(string toWrite, Page page)
        {
            // Add character by character
            foreach (char c in toWrite)
            {
                // Calculate durability used for this character
                int durabilityDeduction =
                    char.IsWhiteSpace(c)
                    ? 0
                    : char.IsUpper(c) 
                        ? 2
                        : 1;

                // If not enough durability in pencil, we can't continue
                if (PointDurability < durabilityDeduction) { break; }

                // Write character and adjust durability
                PointDurability -= durabilityDeduction;
                page.Contents += c;
            }

            
        }

        /// <summary>
        /// Returns PointDurability to it's maximum value for this Pencil
        /// </summary>
        public void Sharpen()
        {
            if (CurrentLength > 0)
            {
                CurrentLength--;
                PointDurability = MaxPointDurability;
            }
        }

        /// <summary>
        /// Erase the input string from the page by replacing it with whitespace
        /// </summary>
        /// <param name="toErase"></param>
        /// <param name="page"></param>
        public void Erase(string toErase, Page page)
        {
            // Index of last occurrence
            var lastPlace = page.Contents.LastIndexOf(toErase);

            // If the string toErase doesn't occur, we do nothing
            if (lastPlace == -1) { return; }

            // Replace with whitespace
            var length = toErase.Length;
            var whitespace = new string(' ', length);
            page.Contents = page.Contents.Remove(lastPlace, length).Insert(lastPlace, whitespace);
        }
    }
}
