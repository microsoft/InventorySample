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

using Windows.UI.Core;

namespace Inventory.Services
{
    public class ContextService : IContextService
    {
        static private int _mainViewID = -1;

        private CoreDispatcher _dispatcher = null;

        public int MainViewID => _mainViewID;

        public int ContextID { get; private set; }

        public bool IsMainView { get; private set; }

        public void Initialize(object dispatcher, int contextID, bool isMainView)
        {
            _dispatcher = dispatcher as CoreDispatcher;
            ContextID = contextID;
            IsMainView = isMainView;
            if (IsMainView)
            {
                _mainViewID = ContextID;
            }
        }

        public async Task RunAsync(Action action)
        {
            await _dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => action());
        }
    }
}
