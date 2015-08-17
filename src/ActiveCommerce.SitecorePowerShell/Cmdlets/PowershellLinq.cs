using System.Linq;
using System.Linq.Dynamic;
using System.Management.Automation;

namespace ActiveCommerce.SitecorePowerShell.Cmdlets
{
    /**
     * PowerShell extensions which allow us to run dynamic, deferred LINQ queries -- needed for efficiently
     * querying NHibernate.
     * http://bartdesmet.net/blogs/bart/archive/2008/06/07/linq-through-powershell.aspx
     */

    public class LinqQuery
    {
        private IQueryable _queryable;

        internal LinqQuery(IQueryable queryable)
        {
            _queryable = queryable;
        }

        public string Expression
        {
            get
            {
                return _queryable.Expression.ToString();
            }
        }

        internal IQueryable Query
        {
            get
            {
                return _queryable;
            }
        }
    }

    public abstract class LazyCmdlet : PSCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true)]
        public LinqQuery Input { get; set; }

        protected abstract LinqQuery Process();

        protected override void ProcessRecord()
        {
            LinqQuery result = Process();

            if (this.MyInvocation.PipelinePosition < this.MyInvocation.PipelineLength)
            {
                WriteObject(result);
            }
            else
            {
                WriteObject(result.Query);
            }
        }
    }

    [Cmdlet("New", "Query")]
    public class NewQueryCmdlet : Cmdlet
    {
        [Parameter(Mandatory = true, Position = 0)]
        public IQueryable Input { get; set; }

        protected override void ProcessRecord()
        {
            WriteObject(new LinqQuery(Input));
        }
    }

    [Cmdlet("Where", "LinqObject")]
    public class WhereCmdlet : LazyCmdlet
    {
        [Parameter(Mandatory = true, Position = 0)]
        public string Predicate { get; set; }

        protected override LinqQuery Process()
        {
            return new LinqQuery(Input.Query.Where(Predicate));
        }
    }

    [Cmdlet("Sort", "LinqObject")]
    public class SortCmdlet : LazyCmdlet
    {
        [Parameter(Mandatory = true, Position = 0)]
        public string Ordering { get; set; }

        protected override LinqQuery Process()
        {
            return new LinqQuery(Input.Query.OrderBy(Ordering));
        }
    }

    [Cmdlet("Select", "LinqObject")]
    public class SelectCmdlet : LazyCmdlet
    {
        [Parameter(Mandatory = true, Position = 0)]
        public string Selector { get; set; }

        protected override LinqQuery Process()
        {
            return new LinqQuery(Input.Query.Select(Selector));
        }
    }

    [Cmdlet("Group", "LinqObject")]
    public class GroupCmdlet : LazyCmdlet
    {
        [Parameter(Mandatory = true, Position = 0)]
        public string KeySelector { get; set; }

        [Parameter(Mandatory = true, Position = 1)]
        public string ElementSelector { get; set; }

        protected override LinqQuery Process()
        {
            return new LinqQuery(Input.Query.GroupBy(KeySelector, ElementSelector));
        }
    }

    [Cmdlet("Take", "LinqObject")]
    public class TakeCmdlet : LazyCmdlet
    {
        [Parameter(Mandatory = true, Position = 0)]
        public int Count { get; set; }

        protected override LinqQuery Process()
        {
            return new LinqQuery(Input.Query.Take(Count));
        }
    }

    [Cmdlet("Skip", "LinqObject")]
    public class SkipCmdlet : LazyCmdlet
    {
        [Parameter(Mandatory = true, Position = 0)]
        public int Count { get; set; }

        protected override LinqQuery Process()
        {
            return new LinqQuery(Input.Query.Skip(Count));
        }
    }

    [Cmdlet("Execute", "Query")]
    public class ExecuteQueryCmdlet : Cmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true)]
        public LinqQuery Input { get; set; }

        protected override void ProcessRecord()
        {
            WriteObject(Input.Query);
        }
    }

    [Cmdlet("Defer", "Query")]
    public class DeferQueryCmdlet : Cmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true)]
        public LinqQuery Input { get; set; }

        protected override void ProcessRecord()
        {
            WriteObject(Input);
        }
    }
}