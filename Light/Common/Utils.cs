using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Light
{
    internal static class Utils
    {
        public static bool MouseOnScreen(MouseState m, int width, int height)
        {
            Debug.WriteLine(m.X >= 0 && m.X < width && m.Y >= 0 && m.Y < height);
            return m.X >= 0 && m.X < width && m.Y >= 0 && m.Y < height;
        }
    }
}
