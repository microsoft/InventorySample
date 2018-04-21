using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Inventory.Data;
using Inventory.Models;

namespace Inventory.Services
{
    public interface ILogService
    {
        Task WriteAsync(LogType type, string source, string action, string message, string description);

        Task<AppLogModel> GetLogAsync(long id);
        Task<IList<AppLogModel>> GetLogsAsync(DataRequest<AppLog> request);
        Task<IList<AppLogModel>> GetLogsAsync(int skip, int take, DataRequest<AppLog> request);
        Task<int> GetLogsCountAsync(DataRequest<AppLog> request);

        Task<int> DeleteLogAsync(AppLogModel model);
        Task<int> DeleteLogRangeAsync(int index, int length, DataRequest<AppLog> request);

        Task MarkAllAsReadAsync();
    }
}
