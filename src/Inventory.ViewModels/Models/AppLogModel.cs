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

using Inventory.Data;

namespace Inventory.Models
{
    public class AppLogModel : ObservableObject
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
