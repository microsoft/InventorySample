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

namespace Inventory.Services
{
    public interface INavigationService
    {
        bool IsMainView { get; }

        bool CanGoBack { get; }

        void Initialize(object frame);

        bool Navigate<TViewModel>(object parameter = null);
        bool Navigate(Type viewModelType, object parameter = null);

        Task<int> CreateNewViewAsync<TViewModel>(object parameter = null);
        Task<int> CreateNewViewAsync(Type viewModelType, object parameter = null);

        void GoBack();

        Task CloseViewAsync();
    }
}
