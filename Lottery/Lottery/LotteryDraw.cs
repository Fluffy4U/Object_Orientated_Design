using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lottery20
{
    // LotteryDraw implementerar interfacet IDrawNumberLogic och dess DrawNumber-metod. LotteryDraw är subjektet i ett observerpattern där ticket-objekten är observers. 
    // När en dragning har genomförts kommer "NotifyObservers"-köras och alla ticket-objekt kommer då få reda på vilka nummer som drogs. 
    // DrawNumber-metoden används även till att slumpa rader om en spelare inte önskar ange siffrorna på en lottorad själv.
    public abstract class LotteryDraw<T> : IDrawNumberLogic
    {
        public abstract int[] DrawNumber(int max, bool pirko);

        internal abstract void RegisterObserver(T observer);
        internal abstract void RemoveObserver(T observer);
        internal abstract void NotifyObserver(int[] drawnNumbers);


    }

    public class LotteryDrawFive : LotteryDraw<FiveTicket>,IDrawNumberLogic
    {
        private Random random = new Random();
        private List<FiveTicket> observers = new List<FiveTicket>();
        private int[] drawnNumbers;
        public override int[] DrawNumber(int max, bool pirko)
        {
            // Fill a list with number 1 to *max*
            List<int> raffle = Enumerable.Range(1, max).ToList();
            int [] drawnNumbers = new int[5];
            for (int i = 0; i < 5; i++)
            { 
                int index = random.Next(raffle.Count); // Draw a number from the raffle
                drawnNumbers[i] = raffle[index]; // Add the number to drawn-list
                raffle.RemoveAt(index); // Remove number from raffle, no dublicates allowed
            }
            if (pirko)
            {
                return drawnNumbers;
            }
            else
            {
                NotifyObserver(drawnNumbers);
                return null;
            }
           
        }

        internal override void RegisterObserver(FiveTicket observer) 
        {
            observers.Add(observer);
        }
        internal override void RemoveObserver(FiveTicket observer)
        {
            observers.Remove(observer);
        }
        internal override void NotifyObserver(int[] drawnNumbers)
        {
            foreach (var observer in observers)
            {
                observer.UpdateDraw(drawnNumbers);
            }
        }
    }
}
