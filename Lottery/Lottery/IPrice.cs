using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lottery20
{
    // Detta interface används av TicketPrice-objekten för att bestämma hur metoden SetPrice ska implementeras.
    internal interface IPrice
    {
        public double SetPrice();
    }
}
