using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashTool.Helpers
{
    public static class ByteArrayHelper
    {
        public static string ToHexString(this byte[] bytes)
        {
            StringBuilder sb = new StringBuilder(bytes.Length);
            foreach (byte b in bytes)
            {
                sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
        }
    }
}
