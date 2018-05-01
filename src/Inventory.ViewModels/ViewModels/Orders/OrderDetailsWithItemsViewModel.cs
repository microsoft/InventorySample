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
using System.Threading.Tasks;

using Inventory.Models;
using Inventory.Services;

namespace Inventory.ViewModels
{
    public class OrderDetailsWithItemsViewModel : ViewModelBase
    {
        public OrderDetailsWithItemsViewModel(IOrderService orderService, IOrderItemService orderItemService, ICommonServices commonServices) : base(commonServices)
        {
            OrderDetails = new OrderDetailsViewModel(orderService, commonServices);
            OrderItemList = new OrderItemListViewModel(orderItemService, commonServices);
        }

        public OrderDetailsViewModel OrderDetails { get; set; }
        public OrderItemListViewModel OrderItemList { get; set; }

        public async Task LoadAsync(OrderDetailsArgs args)
        {
            await OrderDetails.LoadAsync(args);

            long orderID = args?.OrderID ?? 0;
            if (orderID > 0)
            {
                await OrderItemList.LoadAsync(new OrderItemListArgs { OrderID = args.OrderID });
            }
            else
            {
                await OrderItemList.LoadAsync(new OrderItemListArgs { IsEmpty = true }, silent: true);
            }
        }
        public void Unload()
        {
            OrderDetails.CancelEdit();
            OrderDetails.Unload();
            OrderItemList.Unload();
        }

        public void Subscribe()
        {
            MessageService.Subscribe<OrderDetailsViewModel, OrderModel>(this, OnMessage);
            OrderDetails.Subscribe();
            OrderItemList.Subscribe();
        }

        public void Unsubscribe()
        {
            MessageService.Unsubscribe(this);
            OrderDetails.Unsubscribe();
            OrderItemList.Unsubscribe();
        }

        private async void OnMessage(OrderDetailsViewModel viewModel, string message, OrderModel order)
        {
            if (viewModel == OrderDetails && message == "ItemChanged")
            {
                await OrderItemList.LoadAsync(new OrderItemListArgs { OrderID = order.OrderID });
            }
        }
    }
}
