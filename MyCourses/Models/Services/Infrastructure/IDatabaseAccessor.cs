using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MyCourses.Models.Services.Infrastructure
{
    public interface IDatabaseAccessor {
        Task<DataSet> QueryAsync(FormattableString query);
        Task<T> QueryScalarAsync<T>(FormattableString formattableQuery);
        Task<int> CommandAsync(FormattableString formattableCommand);
    }
}