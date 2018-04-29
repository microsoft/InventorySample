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
using System.ComponentModel.DataAnnotations;

namespace Inventory.Data
{
    public enum LogType
    {
        Information,
        Warning,
        Error
    }

    public class AppLog
    {
        [Key]
        public long Id { get; set; }

        public bool IsRead { get; set; }

        public string Name { get; set; }

        [Required]
        public DateTimeOffset DateTime { get; set; }

        [Required]
        [MaxLength(50)]
        public string User { get; set; }

        [Required]
        public LogType Type { get; set; }

        [Required]
        [MaxLength(50)]
        public string Source { get; set; }

        [Required]
        [MaxLength(50)]
        public string Action { get; set; }

        [Required]
        [MaxLength(400)]
        public string Message { get; set; }

        [MaxLength(4000)]
        public string Description { get; set; }
    }
}
