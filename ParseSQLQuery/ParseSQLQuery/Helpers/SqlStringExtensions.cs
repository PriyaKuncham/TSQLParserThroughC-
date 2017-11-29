using Microsoft.SqlServer.TransactSql.ScriptDom;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParseSQLQuery
{
    public static class SqlStringExtensions
    {
        public static bool IsValidSql(this string str)
        {
            return !str.ValidateSql().Any();
        }

        public static IEnumerable<string> ValidateSql(this string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return new[] { "SQL query should be non empty." };
            }
            var parser = new TSql120Parser(false);
            IList<ParseError> errors;
            using (var reader = new StringReader(str))
            {
                parser.Parse(reader, out errors);
            }
            return errors.Select(err => err.Message);
        }
    }
}
