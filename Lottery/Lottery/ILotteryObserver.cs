using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lottery20
{
    // Detta interface används av ticket-objekten för att bestämma hur ticket-objekten (observers) ska uppdatera den dragna raden efter att en dragning genomförts.
    public interface ILotteryObserver
    {
        internal void UpdateDraw(int[] drawnNumber);
    }
}
