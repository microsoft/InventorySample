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
using System.Windows.Input;

using Inventory.Services;

namespace Inventory.ViewModels
{
    #region SettingsArgs
    public class SettingsArgs
    {
        static public SettingsArgs CreateDefault() => new SettingsArgs();
    }
    #endregion

    public class SettingsViewModel : ViewModelBase
    {
        public SettingsViewModel(ISettingsService settingsService, ICommonServices commonServices) : base(commonServices)
        {
            SettingsService = settingsService;
        }

        public ISettingsService SettingsService { get; }

        public string Version => $"v{SettingsService.Version}";

        private bool _isBusy = false;
        public bool IsBusy
        {
            get => _isBusy;
            set => Set(ref _isBusy, value);
        }

        private bool _isLocalProvider = false;
        public bool IsLocalProvider
        {
            get => _isLocalProvider;
            set => Set(ref _isLocalProvider, value);
        }

        private bool _isSqlProvider = false;
        public bool IsSqlProvider
        {
            get => _isSqlProvider;
            set => Set(ref _isSqlProvider, value);
        }

        private string _sqlConnectionString = null;
        public string SqlConnectionString
        {
            get => _sqlConnectionString;
            set => Set(ref _sqlConnectionString, value);
        }

        public bool IsRandomErrorsEnabled
        {
            get { return SettingsService.IsRandomErrorsEnabled; }
            set { SettingsService.IsRandomErrorsEnabled = value; }
        }

        public ICommand ResetLocalDataCommand => new RelayCommand(OnResetLocalData);
        public ICommand ValidateSqlConnectionCommand => new RelayCommand(OnValidateSqlConnection);
        public ICommand CreateDatabaseCommand => new RelayCommand(OnCreateDatabase);

        public SettingsArgs ViewModelArgs { get; private set; }

        public Task LoadAsync(SettingsArgs args)
        {
            ViewModelArgs = args ?? SettingsArgs.CreateDefault();

            IsLocalProvider = SettingsService.DataProvider == DataProviderType.SQLite;
            IsSqlProvider = SettingsService.DataProvider == DataProviderType.SQLServer;
            SqlConnectionString = SettingsService.SQLServerConnectionString;

            StatusReady();
            return Task.CompletedTask;
        }

        private async void OnResetLocalData()
        {
            StatusReady();
            DisableAllViews("Waiting database reset...");
            await Task.Delay(2500);
            EnableAllViews();
        }

        private async void OnValidateSqlConnection()
        {
            StatusReady();
            IsBusy = true;
            StatusMessage("Validating connection string...");
            var result = await SettingsService.ValidateConnectionAsync(SqlConnectionString);
            IsBusy = false;
            if (result.IsOk)
            {
                StatusMessage(result.Message);
            }
            else
            {
                StatusMessage(result.Message);
            }
        }

        private async void OnCreateDatabase()
        {
            StatusReady();
            DisableAllViews("Waiting for the database to be created...");
            var result = await SettingsService.CreateDabaseAsync(SqlConnectionString);
            EnableOtherViews();
            EnableThisView("");
            await Task.Delay(100);
            if (result.IsOk)
            {
                StatusMessage(result.Message);
            }
            else
            {
                LogError("Settings", "Create Database", result.Message, result.Description);
                StatusError("Error creating database, please, check activity log for details");
            }
        }
    }
}
