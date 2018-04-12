using System;

namespace Inventory.ViewModels
{
    public class ViewStateBase
    {
        public ViewStateBase Clone()
        {
            return MemberwiseClone() as ViewStateBase;
        }
    }
}
