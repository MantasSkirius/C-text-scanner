using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scanner_App
{
    static internal class Tokenizatorius
    {
        static public string[] Tokenizuoti(string teksas)
        {
            char[] Skyriklis = new char[] { ' ', ',', '.', '!', ':', ';', '?', '\'', '"', '\n', '\r', '\t', ')', '('};
            string[] zodziai = teksas.Split(Skyriklis, StringSplitOptions.RemoveEmptyEntries).Select(word => word.Trim()).ToArray();
            return zodziai;
        }
    }
}
