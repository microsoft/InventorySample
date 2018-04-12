using System;
using System.Windows.Input;

namespace Inventory
{
    static public class ICommandExtensions
    {
        static public void TryExecute(this ICommand command, object parameter = null)
        {
            if (command != null)
            {
                if (command.CanExecute(parameter))
                {
                    command.Execute(parameter);
                }
            }
        }
    }
}
