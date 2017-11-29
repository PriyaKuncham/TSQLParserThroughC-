using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.TransactSql.ScriptDom;
using System.IO;
using TSQL.Statements;
using TSQL;

namespace ParseSQLQuery
{
    class Program
    {
        static void Main(string[] args)
        {

            List<TSQLStatement> statements = TSQLStatementReader.ParseStatements(
            @"select t.a, t.b, (select 1) as e
				into #tempt
				from
					[table] t
						inner join [table] t2 on
							t.id = t2.id
				where
					t.c = 5
				group by
					t.a,
					t.b
				having
					count(*) > 1
				order by
					t.a,
					t.b; Update table blogs set url='dasfds';",
            includeWhitespace: true);



            Console.WriteLine(statements.Count);
            for (int i = 0; i < statements.Count; i++)
            {
                Console.WriteLine("-------Statement-----{0}", i);
                Console.WriteLine(statements[0]);
                TSQLSelectStatement select = statements[i] as TSQLSelectStatement;

                Console.WriteLine(TSQLStatementType.Select + "----->" + statements[i].Type);
                if (statements[i].Type != TSQLStatementType.Unknown)
                {
                    Console.WriteLine( "----->" + select.Tokens.Count);
                    Console.WriteLine(TSQLKeywords.SELECT + "----->" + select.Tokens[0].AsKeyword.Keyword);
                    Console.WriteLine(" ", select.Tokens[1].AsWhitespace.Text);
                    Console.WriteLine("t", select.Tokens[2].AsIdentifier.Name);
                    Console.WriteLine(TSQLCharacters.Period + "----->" + select.Tokens[3].AsCharacter.Character);
                    Console.WriteLine( "----->" + select.Select.Tokens.Count);
                    Console.WriteLine( "----->" + select.Into.Tokens.Count);
                    Console.WriteLine( "----->" + select.From.Tokens.Count);
                    Console.WriteLine( "----->" + select.Where.Tokens.Count);
                    Console.WriteLine( "----->" + select.GroupBy.Tokens.Count);
                    Console.WriteLine( "----->" + select.Having.Tokens.Count);
                    Console.WriteLine( "----->" + select.OrderBy.Tokens.Count);
                }
            }

            ParseSQL();


            Console.ReadKey();
            
        }
        public static void ParseSimpleSQL()
        {
            string invalidSql = "SELECT /*comment*/ CustomerID AS ID CustomerNumber FROM Customers";
            IEnumerable<string> results = invalidSql.ValidateSql();

            foreach (var x in results)
            {
                Console.WriteLine("{0} \n -------> {1}", invalidSql, x);
            }

            invalidSql = "SELECT /*comment*/ CustomerID AS ID, CustomerNumber FROM Customers";

            foreach (var x in results)
            {
                Console.WriteLine("{0} \n -------> {1}", invalidSql, x);
            }

            results = null;
            results = invalidSql.ValidateSql();
            foreach (var x in results)
            {
                Console.WriteLine("{0} \n {1}", invalidSql, x);
            }

            invalidSql = "SELECT /*comment*/ CustomerID AS ID CustomerNumber FROM Customers";
            results = null;
            results = invalidSql.ValidateSql();
            foreach (var x in results)
            {
                Console.WriteLine("{0} \n --------> {1}", invalidSql, x);
            }

        }
        public static void ParseSQL()
        {
           var sql = @"select AreaId = A.mcw_areaId,  SurrogateKey = A.AreaKey,  Code = S.statecode, Name = S.statename From CRM.dim_Area as A inner join CRM.dim_AreaState as S ON A.statecode = S.statecode  ; "   ;

//            var sql = @"UPDATE Customers
//            SET ContactName = 'Alfred Schmidt', City = 'Frankfurt'
//WHERE CustomerID = 1; ";

                //@"select p.firstname, p.lastname, p.custid FROM persons as p ;
                //SELECT id, name FROM companies;
                //select s.test from (select 'hello' as test) as s; ";

            TSqlParser parser = new TSql120Parser(true);
            IList<ParseError> parseErrors;
            var tReader = new StringReader(sql);
            TSqlFragment sqlFragment = parser.Parse(tReader, out parseErrors);
            var queryTokens = parser.GetTokenStream(tReader, out parseErrors);

            if (parseErrors.Count > 0) Console.WriteLine("Errors:");
            parseErrors.Select(e => e.Message.Indent(2)).ToList().ForEach(Console.WriteLine);

            OwnVisitor visitor = new OwnVisitor();
           //sqlFragment.Accept(visitor);
            sqlFragment.AcceptChildren(visitor);

            Console.WriteLine("Done.");
            Console.ReadKey();
        }
    }
  
}

