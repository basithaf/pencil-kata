using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pencils.Models
{
    public class Page
    {
        // The contents of the page, initialized to empty
        public Observable<string> Contents { get; } = new Observable<string>("");
    }
}
