using System;
using System.Threading.Tasks;

namespace Inventory.Services
{
    public interface IDialogService
    {
        Task ShowAsync(string title, Exception ex, string ok = "Ok");
        Task<bool> ShowAsync(string title, string content, string ok = "Ok", string cancel = null);
    }
}
