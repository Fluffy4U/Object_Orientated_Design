using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lottery20
{
    // Lottoraderna  (tickets) lagrar sina spelade siffror i en array. När ett ticket-objekt skapas så insansieras objekt av TicketPrice och priset för den skapade Ticketen sätts av
    // TicketPrice.SetPrice. Sedan körs en kontroll om någon rabatt ska dras av från priset. Om ticketen är prenumererad skapas ett objekt av SubscriberDiscount. 
    // När ett ticket-objekt skapas så lägger det till sig själv till LotteryDraw's observer-lista. När en dragning har genomförts kommer varje ticket-objekt få reda på det 
    // genom sin UpdateDraw-metod. När denna körs kommer varje ticket att på egen hand kontrollera hur många rätt de hade genom att jämföra sin array med spelade nummer
    // med de dragna numren. För FiveTicket-objekt så krävs 5 rätt för vinst. Om en FiveTicket har 5 rätt kommer det att lägga till personnumret för den spelare som skapade 
    // objektet till FiveLottery's lista med vinnare. 
    public abstract class Ticket
    {
        public bool Subscriber { get; set; }
        public int[] ticketNumbers { get; set; }
        public int[] drawnNumbers;
        public string pnr;
        public double price;
        public abstract bool GetSubscriber();
        public abstract void SetSubscriber(bool subscriber);
        public abstract void UpdateDraw(int[] drawnNumbers);
        public abstract void CheckWin(int[] ticketNumbers, int[] drawnNumbers);
    }
    
    internal class FiveTicketPrice : IPrice
    {
        public double SetPrice()
        {
            return 10;
        }
    }
    public class FiveTicket : Ticket, ILotteryObserver
    {
        public bool Subscriber { get; set; }
        public LotteryDrawFive Subject;
        public string pnr;
        public double price;
        public FiveLottery currentLottery;

        public FiveTicket(FiveLottery currentLottery, string pnr, bool subscriber, int[] ticketNumbers) 
        {
            this.price = new FiveTicketPrice().SetPrice();
            Subscriber = subscriber;
            this.ticketNumbers = ticketNumbers;
            this.currentLottery = currentLottery;
            this.Subject = currentLottery.lotteryDrawFive;
            this.pnr = pnr;
            Subject.RegisterObserver(this);
            if(subscriber == true) 
            {
                SubscriberDiscount discount = new SubscriberDiscount(price);
                this.price = discount.ApplyDiscount();
            }
            
        }

        public override bool GetSubscriber()
        {
            return Subscriber;
        }
        public override void SetSubscriber(bool subscriber)
        {
            this.Subscriber = subscriber;
        }
        public override void UpdateDraw(int[] drawnNumbers) 
        {
            this.drawnNumbers = drawnNumbers;
            CheckWin(ticketNumbers, drawnNumbers);
        }
        public override void CheckWin(int[] ticketNumbers, int[] drawnNumbers) 
        {
            CheckWin winEvaluator = new CheckWin();
            int correctNumbers = winEvaluator.CheckWinnings(ticketNumbers, drawnNumbers);
            if (correctNumbers == 5) 
            {
                currentLottery.AddWinner(this.pnr);
                Console.WriteLine($"Du hade {correctNumbers} rätt.");
                Console.WriteLine("Grattis");
            }
            else
            {
                Console.WriteLine($"Du hade {correctNumbers} rätt.");
                Console.WriteLine("Tyvärr");
            }
        }
    }
}
