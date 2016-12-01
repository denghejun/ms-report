using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSReport.Core
{
    public struct PageMargin
    {
        public decimal Left, Right, Top, Bottom;

        public PageMargin(decimal left, decimal right, decimal top, decimal bottom)
        {
            Left = left;
            Right = right;
            Top = top;
            Bottom = bottom;
        }

        public static PageMargin Default
        {
            get
            {
                return new PageMargin(0.25m, 0.25m, 0.5m, 0.25m);
            }
        }
    }
}
