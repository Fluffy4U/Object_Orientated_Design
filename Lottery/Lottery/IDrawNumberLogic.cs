using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lottery20
{
    // Detta interface används för att definiera hur DrawNumber-metoden behöver implementeras av LotteryDraw-objekten.
    internal interface IDrawNumberLogic
    {
        public int[] DrawNumber(int max, bool pirko);
    }
}
