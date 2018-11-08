#region copyright
// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************
#endregion

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
        Task WriteAsync(LogType type, string source, string action, Exception ex);
        Task<AppLogModel> GetLogAsync(long id);
        Task<IList<AppLogModel>> GetLogsAsync(DataRequest<AppLog> request);
        Task<IList<AppLogModel>> GetLogsAsync(int skip, int take, DataRequest<AppLog> request);
        Task<int> GetLogsCountAsync(DataRequest<AppLog> request);

        Task<int> DeleteLogAsync(AppLogModel model);
        Task<int> DeleteLogRangeAsync(int index, int length, DataRequest<AppLog> request);

        Task MarkAllAsReadAsync();
    }
}
