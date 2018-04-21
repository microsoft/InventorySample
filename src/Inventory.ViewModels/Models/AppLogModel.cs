using System;

using Inventory.Data;

namespace Inventory.Models
{
    public class AppLogModel : ModelBase
    {
        static public AppLogModel CreateEmpty() => new AppLogModel { Id = -1, IsEmpty = true };

        public long Id { get; set; }

        public bool IsRead { get; set; }

        public DateTimeOffset DateTime { get; set; }

        public string User { get; set; }

        public LogType Type { get; set; }
        public string Source { get; set; }
        public string Action { get; set; }
        public string Message { get; set; }
        public string Description { get; set; }
    }
}
