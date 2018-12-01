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
        public const int DEFAULT_ERASER_DURABILITY = 50000;
        public const int DEFAULT_INITIAL_LENGTH = 8;

        // The number of characters that can be written with a sharp point
        private int MaxPointDurability { get; }

        // lowercase characters use 1 durability, uppercase use 2
        public int PointDurability { get; private set; }

        // Erasing non-whitespace characters uses 1 durability/character
        public int EraserDurability { get; private set; }

        // Sharpening uses 1 unit of length
        public int CurrentLength { get; private set; }

        public Pencil(int maxDurability = DEFAULT_MAX_DURABILITY, 
            int initialLength = DEFAULT_INITIAL_LENGTH, 
            int eraserDurability = DEFAULT_ERASER_DURABILITY)
        {
            // initialize PointDurability and store the maximum for sharpening
            PointDurability = MaxPointDurability = maxDurability;
            CurrentLength = initialLength;
            EraserDurability = eraserDurability;
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
                int durabilityDeduction = GetDurabilityDeduction(c);

                // If not enough durability in pencil, we can only write whitespace
                if (PointDurability < durabilityDeduction)
                {
                    page.Contents += ' ';
                }
                else
                {
                    // Write character and adjust durability
                    PointDurability -= durabilityDeduction;
                    page.Contents += c;
                }
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
            var currentCharPlace = lastPlace + toErase.Length - 1;

            // Start with the last char of the string and work backwards as 
            // durability allows
            var newContent = page.Contents;

            while (currentCharPlace >= lastPlace)
            {
                // Stop if no eraser durability left
                if (EraserDurability <= 0) { break; }

                // Use durability if character isn't whitespace
                if (!char.IsWhiteSpace(newContent[currentCharPlace])) { EraserDurability--; }

                // Replace character
                newContent = ReplacePositionInString(newContent, currentCharPlace, " ");

                currentCharPlace--;
            }

            // Set page contents
            page.Contents = newContent;
        }

        /// <summary>
        /// Try to overwrite from the startPosition in the given page
        /// Writes '@' if there is a character already in a space
        /// </summary>
        /// <param name="startPosition"></param>
        /// <param name="toInsert"></param>
        /// <param name="page"></param>
        public void Edit(int position, string toInsert, Page page)
        {
            var newContent = page.Contents;

            // invalid index given
            if (position < 0) { return; }

            foreach (char c in toInsert)
            {
                var durabilityDeduction = GetDurabilityDeduction(c);
                // For Edit, if we don't have durability, we skip this character
                if (PointDurability < durabilityDeduction) { continue; }

                // If we're at the end of the string, just add the new characters
                if (position == newContent.Length)
                {
                    newContent += c;
                }
                // If there is whitespace in the string, write whatever is in toInsert
                else if (char.IsWhiteSpace(newContent[position]))
                {
                    newContent = ReplacePositionInString(newContent, position, c.ToString());
                }
                // if the string and our edit both have non-whitespace, write an '@'
                else if (!char.IsWhiteSpace(c))
                {
                    newContent = ReplacePositionInString(newContent, position, "@");
                }
                // otherwise c is whitespace, so we don't change anything
                PointDurability -= durabilityDeduction;

                position++;
            }

            page.Contents = newContent;
        }

        private int GetDurabilityDeduction(char c)
        {
            return char.IsWhiteSpace(c) ? 0 : char.IsUpper(c) ? 2 : 1;
        }

        private string ReplacePositionInString(string s, int position, string toInsert)
        {
            return s.Remove(position, 1).Insert(position, toInsert);
        }
    }
}
