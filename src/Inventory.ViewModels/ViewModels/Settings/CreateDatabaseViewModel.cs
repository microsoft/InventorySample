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

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Infrastructure;

using Inventory.Services;
using Inventory.Data.Services;

namespace Inventory.ViewModels
{
    public class CreateDatabaseViewModel : ViewModelBase
    {
        public CreateDatabaseViewModel(ISettingsService settingsService, ICommonServices commonServices) : base(commonServices)
        {
            SettingsService = settingsService;
            Result = Result.Error("Operation cancelled");
        }

        public ISettingsService SettingsService { get; }

        public Result Result { get; private set; }

        private string _progressStatus = null;
        public string ProgressStatus
        {
            get => _progressStatus;
            set => Set(ref _progressStatus, value);
        }

        private double _progressMaximum = 1;
        public double ProgressMaximum
        {
            get => _progressMaximum;
            set => Set(ref _progressMaximum, value);
        }

        private double _progressValue = 0;
        public double ProgressValue
        {
            get => _progressValue;
            set => Set(ref _progressValue, value);
        }

        private string _message = null;
        public string Message
        {
            get { return _message; }
            set { if (Set(ref _message, value)) NotifyPropertyChanged(nameof(HasMessage)); }
        }

        public bool HasMessage => _message != null;

        private string _primaryButtonText;
        public string PrimaryButtonText
        {
            get => _primaryButtonText;
            set => Set(ref _primaryButtonText, value);
        }

        private string _secondaryButtonText = "Cancel";
        public string SecondaryButtonText
        {
            get => _secondaryButtonText;
            set => Set(ref _secondaryButtonText, value);
        }

        public async Task ExecuteAsync(string connectionString)
        {
            try
            {
                ProgressMaximum = 14;
                ProgressStatus = "Connecting to Database";
                using (var db = new SQLServerDb(connectionString))
                {
                    var dbCreator = db.GetService<IDatabaseCreator>() as RelationalDatabaseCreator;
                    if (!await dbCreator.ExistsAsync())
                    {
                        ProgressValue = 1;
                        ProgressStatus = "Creating Database...";
                        await db.Database.EnsureCreatedAsync();
                        ProgressValue = 2;
                        await CopyDataTables(db);
                        ProgressValue = 14;
                        Message = "Database created successfully.";
                        Result = Result.Ok("Database created successfully.");
                    }
                    else
                    {
                        ProgressValue = 14;
                        Message = $"Database already exists. Please, delete database and try again.";
                        Result = Result.Error("Database already exist");
                    }
                }
            }
            catch (Exception ex)
            {
                Result = Result.Error("Error creating database. See details in Activity Log");
                Message = $"Error creating database: {ex.Message}";
                LogException("Settings", "Create Database", ex);
            }
            PrimaryButtonText = "Ok";
            SecondaryButtonText = null;
        }

        private async Task CopyDataTables(SQLServerDb db)
        {
            using (var sourceDb = new SQLiteDb(SettingsService.PatternConnectionString))
            {
                ProgressStatus = "Creating table Categories...";
                foreach (var item in sourceDb.Categories.AsNoTracking())
                {
                    await db.Categories.AddAsync(item);
                }
                await db.SaveChangesAsync();
                ProgressValue = 3;

                ProgressStatus = "Creating table CountryCodes...";
                foreach (var item in sourceDb.CountryCodes.AsNoTracking())
                {
                    await db.CountryCodes.AddAsync(item);
                }
                await db.SaveChangesAsync();
                ProgressValue = 4;

                ProgressStatus = "Creating table OrderStatus...";
                foreach (var item in sourceDb.OrderStatus.AsNoTracking())
                {
                    await db.OrderStatus.AddAsync(item);
                }
                await db.SaveChangesAsync();
                ProgressValue = 5;

                ProgressStatus = "Creating table PaymentTypes...";
                foreach (var item in sourceDb.PaymentTypes.AsNoTracking())
                {
                    await db.PaymentTypes.AddAsync(item);
                }
                await db.SaveChangesAsync();
                ProgressValue = 6;

                ProgressStatus = "Creating table Shippers...";
                foreach (var item in sourceDb.Shippers.AsNoTracking())
                {
                    await db.Shippers.AddAsync(item);
                }
                await db.SaveChangesAsync();
                ProgressValue = 7;

                ProgressStatus = "Creating table TaxTypes...";
                foreach (var item in sourceDb.TaxTypes.AsNoTracking())
                {
                    await db.TaxTypes.AddAsync(item);
                }
                await db.SaveChangesAsync();
                ProgressValue = 8;

                ProgressStatus = "Creating table Customers...";
                foreach (var item in sourceDb.Customers.AsNoTracking())
                {
                    await db.Customers.AddAsync(item);
                }
                await db.SaveChangesAsync();
                ProgressValue = 9;

                ProgressStatus = "Creating table Products...";
                foreach (var item in sourceDb.Products.AsNoTracking())
                {
                    await db.Products.AddAsync(item);
                }
                await db.SaveChangesAsync();
                ProgressValue = 10;

                ProgressStatus = "Creating table Orders...";
                foreach (var item in sourceDb.Orders.AsNoTracking())
                {
                    await db.Orders.AddAsync(item);
                }
                await db.SaveChangesAsync();
                ProgressValue = 11;

                ProgressStatus = "Creating table OrderItems...";
                foreach (var item in sourceDb.OrderItems.AsNoTracking())
                {
                    await db.OrderItems.AddAsync(item);
                }
                await db.SaveChangesAsync();
                ProgressValue = 12;

                ProgressStatus = "Creating database version...";
                await db.DbVersion.AddAsync(await sourceDb.DbVersion.FirstAsync());
                await db.SaveChangesAsync();
                ProgressValue = 13;
            }
        }
    }
}
