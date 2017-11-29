using Microsoft.SqlServer.TransactSql.ScriptDom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParseSQLQuery
{
    public static class TSqlDomHelpers
    {
        public static string ToSourceSqlString(this TSqlFragment fragment)
        {
            StringBuilder sqlText = new StringBuilder();
            for (int i = fragment.FirstTokenIndex; i <= fragment.LastTokenIndex; i++)
            {
                sqlText.Append(fragment.ScriptTokenStream[i].Text);
            }
            return sqlText.ToString();
        }

        public static string ToSqlString(this TSqlFragment fragment)
        {
            SqlScriptGenerator generator = new Sql120ScriptGenerator();
            string sql;
            generator.GenerateScript(fragment, out sql);
            return sql;
        }
    }
}
