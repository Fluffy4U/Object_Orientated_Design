using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lottery20
{
    // Detta interface används för att skapa ticket-objekt
    internal interface ICreateTicket
    {
        internal double CreateTicket(bool subscriber, int[] ticketNumbers);
    }
}
