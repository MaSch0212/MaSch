using System;
using System.Collections.Generic;
using System.Text;

namespace MaSch.Presentation
{
    public enum AlertButton
    {
        Ok = 0,
        OkCancel = 1,
        YesNoCancel = 3,
        YesNo = 4
    }

    public enum AlertImage
    {
        None = 0,
        Hand = 16,
        Stop = 16,
        Error = 16,
        Question = 32,
        Exclamation = 48,
        Warning = 48,
        Asterisk = 64,
        Information = 64
    }

    public enum AlertOptions
    {
        None = 0,
        DefaultDesktopOnly = 131072,
        RightAlign = 524288,
        RtlReading = 1048576,
        ServiceNotification = 2097152
    }

    public enum AlertResult
    {
        None = 0,
        Ok = 1,
        Cancel = 2,
        Yes = 6,
        No = 7
    }

    public enum WindowVisualState
    {
        Normal = 0,
        Minimized = 1,
        Maximized = 2
    }
}
