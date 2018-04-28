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

namespace Inventory.Services
{
    public class SettingsService : ISettingsService
    {
        public string Version => AppSettings.Current.Version;

        public DataProviderType DataProvider
        {
            get => AppSettings.Current.DataProvider;
            set => AppSettings.Current.DataProvider = value;
        }

        public string SQLServerConnectionString
        {
            get => AppSettings.Current.SQLServerConnectionString;
            set => AppSettings.Current.SQLServerConnectionString = value;
        }

        public bool IsRandomErrorsEnabled
        {
            get => AppSettings.Current.IsRandomErrorsEnabled;
            set => AppSettings.Current.IsRandomErrorsEnabled = value;
        }
    }
}
