using System.Text;

namespace MarbleSorterGame.Utilities
{
    public static class StringExtensions
    {
        public static string ColumnWrap(this string s, int maxColumn)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0, col = 0; i < s.Length; i++, col++)
            {
                if (col >= maxColumn)
                {
                    sb.Append("\n");
                    col = 0;
                }
                
                if (s[i] == '\n')
                    col = 0;
                
                sb.Append(s[i]);
            }
            return sb.ToString();
        }
    }
}