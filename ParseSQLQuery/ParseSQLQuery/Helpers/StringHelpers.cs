using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParseSQLQuery
{
    public static class StringHelpers
    {
        public static string Indent(this string Source, int NumberOfSpaces)
        {
            string indent = new string(' ', NumberOfSpaces);
            return indent + Source.Replace("\n", "\n" + indent);
        }
        public static string Multiply(this string Source, int Multiplier)
        {
            StringBuilder stringBuilder = new StringBuilder(Multiplier * Source.Length);
            for (int i = 0; i < Multiplier; i++)
            {
                stringBuilder.Append(Source);
            }
            return stringBuilder.ToString();
        }
    }
}
