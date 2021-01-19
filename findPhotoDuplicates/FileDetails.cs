using System;
using System.Collections.Generic;
using System.Text;

namespace FindPhotoDuplicates
{
    public class FileDetails2
    {
        public string Filelocation { get; set; }
        #nullable enable
        public List<string>? Duplicates { get; set; }
    }
}
