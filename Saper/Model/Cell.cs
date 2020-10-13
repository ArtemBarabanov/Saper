using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saper
{
    class Cell
    {
        public bool IsMined { get; set; }
        public bool IsBorder { get; set; }
        public bool IsVisited { get; set; }
        public bool IsMarked { get; set; }
        public int MinesNear { get; set; }
        public int PositionX { get; set; }
        public int PositionY { get; set; }
    }
}
