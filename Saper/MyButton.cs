using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Saper
{
    class MyButton: Button
    {
        public int XPosition { get; private set; }
        public int YPosition { get; private set; }

        public MyButton(int x, int y)
        { 
            SetStyle(ControlStyles.Selectable, false);
            XPosition = x;
            YPosition = y;
        }
    }
}
