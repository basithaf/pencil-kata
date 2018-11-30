using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pencils.Models
{
    public class Pencil
    {
        private const uint DEFAULT_MAX_DURABILITY = 10000;

        // The number of characters that can be written with a sharp point
        private uint MaxPointDurability { get; }

        // lowercase characters use 1 durability, uppercase use 2
        public uint PointDurability { get; private set; }

        
        
        public Pencil(uint maxDurability = DEFAULT_MAX_DURABILITY)
        {
            MaxPointDurability = PointDurability = maxDurability;
            
        }

        public void WriteToPage(string toWrite, Page page)
        {
            foreach (char c in toWrite)
            {
                PointDurability--;
                page.Contents += c;
            }

            
        }
    }
}
