using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame
{
    class MoveCodeParser
    {
        public static int charToDigit(char ch)
        {
            return ((int)ch) - 97;
        }

        public static char digitToChar(int digit)
        {
            return (char)(digit + 97);
        }

        public static Point getDigitCoords(string str)
        {
            int x = 8 - (Convert.ToInt32(str[1].ToString()));
            int y = charToDigit(str[0]);
            return new Point(x, y);
        }
    }
}
