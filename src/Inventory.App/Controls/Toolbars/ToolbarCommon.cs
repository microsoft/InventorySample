using System;

namespace Inventory.Controls
{
    public enum ToolbarButton
    {
        Back,
        New,
        Edit,
        Delete,
        Cancel,
        Save,
        Select,
        Refresh
    }

    public enum ListToolbarMode
    {
        Default,
        Cancel,
        CancelDelete
    }

    public enum DetailToolbarMode
    {
        Default,
        BackEditdDelete,
        CancelSave
    }

    public class ToolbarButtonClickEventArgs : EventArgs
    {
        public ToolbarButtonClickEventArgs(ToolbarButton button)
        {
            ClickedButton = button;
        }

        public ToolbarButton ClickedButton { get; }
    }

    public delegate void ToolbarButtonClickEventHandler(object sender, ToolbarButtonClickEventArgs e);
}
