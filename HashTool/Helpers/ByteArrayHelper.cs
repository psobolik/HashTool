using System.Text;

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
