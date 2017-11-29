using Microsoft.SqlServer.TransactSql.ScriptDom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParseSQLQuery
{
    class OwnVisitor : TSqlFragmentVisitor
    {
        public override void ExplicitVisit(SelectStatement node)
        {
            QuerySpecification querySpecification = node.QueryExpression as QuerySpecification;

            FromClause fromClause = querySpecification.FromClause;

            // There could be more than one TableReference!
            // TableReference is not sure to be a NamedTableReference, could be as example a QueryDerivedTable

            NamedTableReference namedTableReference = fromClause.TableReferences[0] as NamedTableReference;
            TableReferenceWithAlias tableReferenceWithAlias = fromClause.TableReferences[0] as TableReferenceWithAlias;

            if (namedTableReference != null && tableReferenceWithAlias != null)
            {
                string baseIdentifier = namedTableReference?.SchemaObject.BaseIdentifier?.Value;
                string schemaIdentifier = namedTableReference?.SchemaObject.SchemaIdentifier?.Value;
                string databaseIdentifier = namedTableReference?.SchemaObject.DatabaseIdentifier?.Value;
                string serverIdentifier = namedTableReference?.SchemaObject.ServerIdentifier?.Value;

                string alias = tableReferenceWithAlias.Alias?.Value;


                Console.WriteLine("From:");
                Console.WriteLine($"  {"Server:",-10} {serverIdentifier}");
                Console.WriteLine($"  {"Database:",-10} {databaseIdentifier}");
                Console.WriteLine($"  {"Schema:",-10} {schemaIdentifier}");
                Console.WriteLine($"  {"Table:",-10} {baseIdentifier}");
                Console.WriteLine($"  {"Alias:",-10} {alias}");

            }
            else
            {
                
                JoinTableReference JoinReferenceWithAlias1 = fromClause.TableReferences[0] as JoinTableReference;
                NamedTableReference namedTableReference1 = JoinReferenceWithAlias1.FirstTableReference as NamedTableReference;
                TableReferenceWithAlias tableReferenceWithAlias1 = JoinReferenceWithAlias1.FirstTableReference as TableReferenceWithAlias;

                string baseIdentifier = namedTableReference1?.SchemaObject.BaseIdentifier?.Value;
                string schemaIdentifier = namedTableReference1?.SchemaObject.SchemaIdentifier?.Value;
                string databaseIdentifier = namedTableReference1?.SchemaObject.DatabaseIdentifier?.Value;
                string serverIdentifier = namedTableReference1?.SchemaObject.ServerIdentifier?.Value;

                string alias = tableReferenceWithAlias1.Alias?.Value;

                Console.WriteLine("----First Table----");

                Console.WriteLine($"  {"Server:",-10} {serverIdentifier}");
                Console.WriteLine($"  {"Database:",-10} {databaseIdentifier}");
                Console.WriteLine($"  {"Schema:",-10} {schemaIdentifier}");
                Console.WriteLine($"  {"Table:",-10} {baseIdentifier}");
                Console.WriteLine($"  {"Alias:",-10} {alias}");

                NamedTableReference namedTableReference2 = JoinReferenceWithAlias1.SecondTableReference as NamedTableReference;
                TableReferenceWithAlias tableReferenceWithAlias2 = JoinReferenceWithAlias1.SecondTableReference as TableReferenceWithAlias;

                Console.WriteLine("----Second Table----");

                string baseIdentifier1 = namedTableReference2?.SchemaObject.BaseIdentifier?.Value;
                string schemaIdentifier1 = namedTableReference2?.SchemaObject.SchemaIdentifier?.Value;
                string databaseIdentifier1 = namedTableReference2?.SchemaObject.DatabaseIdentifier?.Value;
                string serverIdentifier1 = namedTableReference2?.SchemaObject.ServerIdentifier?.Value;
                string alias2 = tableReferenceWithAlias2.Alias?.Value;

                Console.WriteLine($"  {"Server:",-10} {serverIdentifier1}");
                Console.WriteLine($"  {"Database:",-10} {databaseIdentifier1}");
                Console.WriteLine($"  {"Schema:",-10} {schemaIdentifier1}");
                Console.WriteLine($"  {"Table:",-10} {baseIdentifier1}");
                Console.WriteLine($"  {"Alias:",-10} {alias2}");




            }

            // Example of changing the alias:
            //(fromClause.TableReferences[0] as NamedTableReference).Alias = new Identifier() { Value = baseIdentifier[0].ToString() };

            Console.WriteLine("Statement:");
            Console.WriteLine(node.ToSqlString().Indent(2));

            Console.WriteLine("¯".Multiply(40));

            base.ExplicitVisit(node);
        }
    }
}
