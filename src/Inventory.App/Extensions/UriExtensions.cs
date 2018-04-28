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

using Windows.Foundation;

namespace Inventory
{
    static public class UriExtensions
    {
        static public Int64 GetInt64Parameter(this Uri uri, string name)
        {
            string value = GetParameter(uri, name);
            if (value != null)
            {
                if (Int64.TryParse(value, out Int64 n))
                {
                    return n;
                }
            }
            return 0;
        }

        static public Int32 GetInt32Parameter(this Uri uri, string name)
        {
            string value = GetParameter(uri, name);
            if (value != null)
            {
                if (Int32.TryParse(value, out Int32 n))
                {
                    return n;
                }
            }
            return 0;
        }

        static public string GetParameter(this Uri uri, string name)
        {
            string query = uri.Query;
            if (!String.IsNullOrEmpty(query))
            {
                try
                {
                    var decoder = new WwwFormUrlDecoder(uri.Query);
                    return decoder.GetFirstValueByName("id");
                }
                catch { }
            }
            return null;
        }
    }
}
