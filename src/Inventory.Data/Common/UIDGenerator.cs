using System;

namespace Inventory.Data
{
    static class UIDGenerator
    {
        static private readonly DateTime DateSeed = DateTime.Parse("2015/01/01");

        static public long Next(int prefix = 1)
        {
            return (long)(DateTime.UtcNow - DateSeed).TotalMilliseconds + prefix * 100000000000;
        }
    }
}
