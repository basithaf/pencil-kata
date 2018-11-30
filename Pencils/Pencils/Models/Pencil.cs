using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pencils.Models
{
    public class Pencil
    {
        private const int DEFAULT_MAX_DURABILITY = 10000;

        // The number of characters that can be written with a sharp point
        private int MaxPointDurability { get; }

        // lowercase characters use 1 durability, uppercase use 2
        public int PointDurability { get; private set; }

        
        
        public Pencil(int maxDurability = DEFAULT_MAX_DURABILITY)
        {
            MaxPointDurability = PointDurability = maxDurability;
            
        }

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
    }
}
