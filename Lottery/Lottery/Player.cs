using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Lottery20
{
    // Player-objekten skapar tickets (lottorader).  Dessa tickets lagras i tempTicketList och sedan genomförs en kontroll så att de har tillräckligt mycket pengar i sitt saldo (Balance).
    // Om de inte har det så tas raden bort från tempTicketList igen, och raden tas även bort från LotteryDraw's lista med observers. 
    // Om spelaren är 65 år eller äldre så läggs en pensionärsrabatt så att spelaren får en gratisrad, en spelare kan endast få en gratis-rad tack vare if-satsen som 
    // kollar om spelaren redan har en rad spelad. Alla efterföljande rader som spelaren skapar kommer därmed inte vara gratis. 
    // När en spelare genomför betalningen av en rad så läggs ticket-objektet till i Lottery's dictionary med tickets och ticketen skrivs till json-filen som innehåller alla spelade rader.
    // tempTicketList töms sedan. 
    public class Player : ICreateTicket
    {
        public string Pnr {  get; private set; }
        public string Password { get; set; }
        public bool Registered {  get; private set; }

        [JsonIgnore] // När vi skriver currentLottery till registreradeplayers.json så skapas en infinite loop när vi startar spelet.
        public FiveLottery currentLottery { get; set; }
        internal List<FiveTicket> tempTicketList = new List<FiveTicket>();
        /*När en Player instansieras ska personnr med och registered 
        säts som "false" som standard.*/
        internal double Balance { get; set; }
        public Player(FiveLottery currentLottery, string pnr, bool registered, string pswd, double balance)
        {
            this.Pnr = pnr;
            this.Registered = registered;
            this.Password = pswd;
            this.currentLottery = currentLottery;
            this.Balance = balance;
        }
        internal double GetBalance() 
        {  
            return Balance; 
        }
        internal void IncreaseBalance(double increaseAmount)
        { 
            this.Balance += increaseAmount; 
        }
        internal void RecieveWin(double WinAmount)
        {
            Balance += WinAmount; 
        }
        public double CreateTicket(bool subscriber, int[] ticketNumbers)
        {
            FiveTicket ticket = new FiveTicket(currentLottery, Pnr, subscriber, ticketNumbers);
            string tempPnr = Pnr;
            int birthDate = int.Parse(tempPnr.Substring(0, 4));
            DateTime dateTime = DateTime.Now;
            int dateInt = int.Parse(dateTime.ToString("yyyy"));
            if (birthDate < dateInt - 65)
            {
                if (currentLottery.Tickets != null && currentLottery.Tickets.ContainsKey(Pnr) && currentLottery.Tickets[Pnr].Count < 2) 
                { 
                    if (currentLottery.Tickets.ContainsKey(Pnr) && currentLottery.Tickets[Pnr].Count() == 1)
                    {
                        AgeDiscount discount = new AgeDiscount(ticket.price);
                        ticket.price = discount.ApplyDiscount();
                    }
                }
                else if (!currentLottery.Tickets.ContainsKey(Pnr) && tempTicketList.Count() == 1)
                {
                    AgeDiscount discount = new AgeDiscount(ticket.price);
                    ticket.price = discount.ApplyDiscount();
                }
            }

            tempTicketList.Add(ticket);

            if (ticket.price <= Balance)
            {
                return ticket.price;
            }
            else
            {
                ticket.Subject.RemoveObserver(ticket);
                tempTicketList.Remove(ticket);
                return ticket.price;
            }
        }
        internal void WriteTicketList(List<FiveTicket> ticketList)
        {
            IDatabaseWriter<List<FiveTicket>> ticketWriter = new FiveTicketWriter();
            string jsonString = JsonConvert.SerializeObject(ticketList);
            ticketWriter.Write(Pnr, ticketList);
        }

        internal void PayTicket(double totalPrice)
        {
            Balance -= totalPrice;
            currentLottery.TakePayment(totalPrice);
            if (tempTicketList.Count > 0)
            {
                currentLottery.AddTickets(Pnr, tempTicketList);
                WriteTicketList(tempTicketList);
                tempTicketList = [];
            }
        }

    }
}
