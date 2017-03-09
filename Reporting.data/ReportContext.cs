using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Bussiness.Models;
using Reporting.data.Models;
using System.Data.SqlClient;
using DbContext = System.Data.Entity.DbContext;

namespace Reporting.data
{

    public class ReportContext : DbContext
    {
        //use command line [dotnet ef --help] for database and migrations management
        public ReportContext(string connectionString) : base(connectionString)
        {
        }

        public System.Data.Entity.DbSet<Employee> Employees { get; set; }
        public System.Data.Entity.DbSet<Department> Departments { get; set; }

        /// <summary>
        /// Creates a raw SQL query that will return elements of the given generic type.
        /// 
        /// The type can be any type that has properties that match the names of the columns returned
        /// from the query, or can be a simple primitive type.  The type does not have to be an
        /// entity type. The results of this query are never tracked by the context even if the
        /// type of object returned is an entity type.  Use the <see cref="M:System.Data.Entity.DbSet`1.SqlQuery(System.String,System.Object[])"/>
        /// method to return entities that are tracked by the context.
        /// 
        /// As with any API that accepts SQL it is important to parameterize any user input to protect against a SQL injection attack. You can include parameter place holders in the SQL query string and then supply parameter values as additional arguments. Any parameter values you supply will automatically be converted to a DbParameter.
        /// context.Database.SqlQuery&lt;Post&gt;("SELECT * FROM dbo.Posts WHERE Author = @p0", userSuppliedAuthor);
        /// Alternatively, you can also construct a DbParameter and supply it to SqlQuery. This allows you to use named parameters in the SQL query string.
        /// context.Database.SqlQuery&lt;Post&gt;("SELECT * FROM dbo.Posts WHERE Author = @author", new SqlParameter("@author", userSuppliedAuthor));
        /// </summary>
        /// <typeparam name="TElement"></typeparam>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<List<TElement>> SqlQuery<TElement>(string sql, params object[] parameters)
        {
            return await Database.SqlQuery<TElement>(sql, parameters).ToListAsync();

        }

        public async Task<T> SqlQuerySerialized<T>(string sql, params object[] parameters)
        {
            return await Database.SqlQuery<T>(sql, parameters).FirstAsync();

        }

    }
}