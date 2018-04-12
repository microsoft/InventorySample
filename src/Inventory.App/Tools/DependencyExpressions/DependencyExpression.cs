using System;

using Windows.UI.Xaml;

namespace Inventory
{
    public class DependencyExpression
    {
        public DependencyExpression(string name, string[] dependencies)
        {
            Name = name;
            Dependencies = dependencies;
        }

        public string Name { get; }
        public string[] Dependencies { get; }
    }
}
