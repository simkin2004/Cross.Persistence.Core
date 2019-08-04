using System;
using System.Collections.Generic;
using System.Text;

namespace Cross.Persistence.Core.Tests
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class SampleSqlQueryBuilder : SqlQueryBuilder
    {
        public SampleSqlQueryBuilder() 
            : base()
        {
            // no op
        }

        public override bool IncludeOrderByClause => true;

        public override bool RequireSortOrder => false;

        public override string SelectStatementFormat => "SELECT COUNT() as row_count, {1} FROM {0} WHERE row_no >= {3} AND row_no < {4}{2}{5};";
    }
}
