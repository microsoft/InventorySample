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
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using Inventory.Data.Services;

namespace Inventory
{
    static public class SQLServerDbTools
    {
        static public async Task EnsureDatabaseAsync()
        {
            using (var db = new SQLServerDb(AppSettings.Current.SQLServerConnectionString))
            {
                await db.Database.EnsureCreatedAsync();

                // TODO: Check Db version instead
                if (db.Categories.Count() == 0)
                {
                    await FillDatabaseAsync(db);
                }
            }
        }

        static public async Task FillDatabaseAsync(SQLServerDb db)
        {
            using (var sourceDb = new SQLiteDb($"Data Source={AppSettings.DatabasePatternFileName}"))
            {
                foreach (var item in sourceDb.Categories.AsNoTracking())
                {
                    await db.Categories.AddAsync(item);
                }
                await db.SaveChangesAsync();

                foreach (var item in sourceDb.CountryCodes.AsNoTracking())
                {
                    await db.CountryCodes.AddAsync(item);
                }
                await db.SaveChangesAsync();

                foreach (var item in sourceDb.OrderStatus.AsNoTracking())
                {
                    await db.OrderStatus.AddAsync(item);
                }
                await db.SaveChangesAsync();

                foreach (var item in sourceDb.PaymentTypes.AsNoTracking())
                {
                    await db.PaymentTypes.AddAsync(item);
                }
                await db.SaveChangesAsync();

                foreach (var item in sourceDb.Shippers.AsNoTracking())
                {
                    await db.Shippers.AddAsync(item);
                }
                await db.SaveChangesAsync();

                foreach (var item in sourceDb.TaxTypes.AsNoTracking())
                {
                    await db.TaxTypes.AddAsync(item);
                }
                await db.SaveChangesAsync();

                foreach (var item in sourceDb.Customers.AsNoTracking())
                {
                    await db.Customers.AddAsync(item);
                }
                await db.SaveChangesAsync();

                foreach (var item in sourceDb.Products.AsNoTracking())
                {
                    await db.Products.AddAsync(item);
                }
                await db.SaveChangesAsync();

                foreach (var item in sourceDb.Orders.AsNoTracking())
                {
                    await db.Orders.AddAsync(item);
                }
                await db.SaveChangesAsync();

                foreach (var item in sourceDb.OrderItems.AsNoTracking())
                {
                    await db.OrderItems.AddAsync(item);
                }
                await db.SaveChangesAsync();
            }
        }
    }
}
