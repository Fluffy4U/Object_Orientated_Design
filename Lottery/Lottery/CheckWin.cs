using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lottery20
{
    // CheckWin har en metod för att jämföra arrayen i ett ticket objekt med den dragna lottoraden. Metoden returnerar en int med antalet rätt till varje ticket. 
    public class CheckWin
    {
        internal int CheckWinnings(int[] ticketNumbers, int[] drawnNumbers) 
        {
            List<int> matchingNumbers = ticketNumbers.Intersect(drawnNumbers).ToList();
            return matchingNumbers.Count();
        }
    }
}
