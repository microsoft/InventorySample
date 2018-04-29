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
    public class OrderCollection : VirtualCollection<OrderModel>
    {
        private DataRequest<Order> _dataRequest = null;

        public OrderCollection(IOrderService orderService, ILogService logService) : base(logService)
        {
            OrderService = orderService;
        }

        public IOrderService OrderService { get; }

        private OrderModel _defaultItem = OrderModel.CreateEmpty();
        protected override OrderModel DefaultItem => _defaultItem;

        public async Task LoadAsync(DataRequest<Order> dataRequest)
        {
            try
            {
                _dataRequest = dataRequest;
                Count = await OrderService.GetOrdersCountAsync(_dataRequest);
                Ranges[0] = await OrderService.GetOrdersAsync(0, RangeSize, _dataRequest);
            }
            catch (Exception ex)
            {
                Count = 0;
                throw ex;
            }
        }

        protected override async Task<IList<OrderModel>> FetchDataAsync(int rangeIndex, int rangeSize)
        {
            try
            {
                return await OrderService.GetOrdersAsync(rangeIndex * rangeSize, rangeSize, _dataRequest);
            }
            catch (Exception ex)
            {
                LogException("OrderCollection", "Fetch", ex);
            }
            return null;
        }
    }
}
