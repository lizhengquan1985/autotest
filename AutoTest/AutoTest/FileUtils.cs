using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTest
{
    public class FileUtils
    {
        public static void write(string content, string filename)
        {
            using (FileStream fs = new FileStream(@"f://logs/" + filename, FileMode.CreateNew))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.Write(content);
                }
            }
        }
    }
}
