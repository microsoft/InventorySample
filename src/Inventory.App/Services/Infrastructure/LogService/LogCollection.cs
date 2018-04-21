using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Inventory.Data;
using Inventory.Models;

namespace Inventory.Services
{
    public class LogCollection : VirtualCollection<AppLogModel>
    {
        private DataRequest<AppLog> _dataRequest = null;

        public LogCollection(ILogService logService) : base(logService)
        {
        }

        private AppLogModel _defaultItem = AppLogModel.CreateEmpty();
        protected override AppLogModel DefaultItem => _defaultItem;

        public async Task LoadAsync(DataRequest<AppLog> dataRequest)
        {
            try
            {
                _dataRequest = dataRequest;
                Count = await LogService.GetLogsCountAsync(_dataRequest);
                Ranges[0] = await FetchDataAsync(0, RangeSize);
            }
            catch (Exception ex)
            {
                Count = 0;
                throw ex;
            }
        }

        protected override async Task<IList<AppLogModel>> FetchDataAsync(int rangeIndex, int rangeSize)
        {
            try
            {
                return await LogService.GetLogsAsync(rangeIndex * rangeSize, rangeSize, _dataRequest);
            }
            catch (Exception ex)
            {
                LogException("LogCollection", "Fetch", ex);
            }
            return null;
        }
    }
}
