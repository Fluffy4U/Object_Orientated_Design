using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Lottery20
{
    // Lottery-klassen har koll på alla spelare och deras tickets. Lottery-objektet instansierar LotteryDraw-objektet och läser in Players och Tickets som sparats till json.
    // Spelare skapas genom createPlayer-metoden. 
    // Det är till Lotteryklassen som ett spelar-objekt betalar en ticket. Jackpotten ökas då med 75% av priset, resten överförs till ett schweiziskt bankkonto. 
    // När en dragning ska genomföras anropas DoDraw-metoden som LotteryDraw-objektet har, sedan tas icke-prenumererade rader bort och gäst-spelare tas också bort.
    // Om någon av ticket-objekten har vunnit så läggs personnumret för spelaren som skapat det specifika ticket-objektet till i listan "Winners". 
    public abstract class Lottery
    {
        internal double Jackpot { get; set; }
        internal List<string> Winners { get; set; }
        internal Dictionary<string, Player> Players { get; set; }
        internal Dictionary<string, List<Ticket>> Tickets { get; set; }

        private string filepath;
        internal abstract Player createPlayer(FiveLottery currentLottery, string pnr, bool registered, string pswd, double balance);
        internal abstract void TakePayment(double amount);

        internal abstract void AddWinner(string pnr);

        internal abstract void PayWinners();
        internal abstract void DoDraw();
        internal abstract void RemoveNonSubTicket();
        internal abstract void RemoveGuests();

    }

    public class FiveLottery : Lottery
    {
        internal double Jackpot {  get; set; }
        internal List<string> Winners { get; set; }
        internal Dictionary<string, Player> Players { get; set; }
        internal Dictionary<string, List<FiveTicket>> Tickets { get; set; }

        internal LotteryDrawFive lotteryDrawFive;

        public static string filepath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "fivetickets.json");

        public FiveLottery()
        {
            this.lotteryDrawFive = new LotteryDrawFive();
            this.Players = Players;
            this.Winners = new List<string>();
            FiveTicketReader ticketReader = new FiveTicketReader(this);
            PlayerReader playerReader = new PlayerReader();
            Players = new Dictionary<string, Player>();
            Tickets = new Dictionary<string, List<FiveTicket>>();
            Players = playerReader.ReadAll();
            Tickets = ticketReader.ReadAll(filepath);
        }

        internal override Player createPlayer(FiveLottery currentLottery, string pnr, bool registered, string pswd, double balance)
        {
            Player player = new Player(currentLottery, pnr, registered, pswd, balance);
            PlayerWriter writer = new PlayerWriter();
            writer.Write(pnr, player);
            Players.Add(pnr, player);
            return player;
        }
        internal override void TakePayment(double amount)
        {
            double jackpotAmount = amount * 0.75;
            amount -= jackpotAmount;
            Jackpot += jackpotAmount;
            SwissBank.swissBankAccount += amount;
        }

        internal override void AddWinner(string pnr)
        {
            Winners.Add(pnr);
        }

        internal override void PayWinners()
        {
            if (Winners != null)
            {
                int winners = Winners.Count();
                double dividedJackpot = Jackpot / winners;
                foreach (string pnr in Winners)
                {
                    if (Players.ContainsKey(pnr))
                    {
                        Player p = Players[pnr];
                        p.RecieveWin(dividedJackpot);
                    }
                }
            }
        }
        internal override void DoDraw()
        {
            lotteryDrawFive.DrawNumber(35, false);
            ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem();
            PayWinners();
        }
        internal void TakeSubscriberPayment()
        {
            foreach (KeyValuePair<string, List<FiveTicket>> kvp in Tickets)
            {
                double amountToPay = 0;
                foreach(FiveTicket ticket in kvp.Value) 
                {
                    amountToPay += ticket.price;
                }
                Player player = Players[kvp.Key];
                player.Balance -= amountToPay;
                double jackpotAmount = amountToPay * 0.75;
                amountToPay -= jackpotAmount;
                Jackpot += jackpotAmount;
                SwissBank.swissBankAccount += amountToPay;
            }
        }
        internal override void RemoveNonSubTicket()
        {
            Dictionary<string, List<FiveTicket>> ticketDict = this.Tickets.ToDictionary(entry => entry.Key, entry => new List<FiveTicket>(entry.Value));
            FiveTicketWriter ticketWriter = new FiveTicketWriter();
            this.Tickets.Clear();
            File.Delete(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "fivetickets.json"));
            if (ticketDict != null)
            {
                foreach (KeyValuePair<string, List<FiveTicket>> kvp in ticketDict)
                {
                    List<FiveTicket> ticketList = kvp.Value;
                    List<FiveTicket> tempTicketList = new List<FiveTicket>();
                    foreach (FiveTicket ticket in ticketList)
                    {
                        if (ticket.Subscriber == true)
                        {
                            tempTicketList.Add(ticket);
                        }
                    }
                    ticketWriter.Write(kvp.Key, tempTicketList);
                    this.Tickets.Add(kvp.Key, tempTicketList);
                }
            }
        }
        internal override void RemoveGuests()
        {
            File.Delete(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "oregistreradeplayers.json"));
        }
        internal void AddTickets(string pnr, List<FiveTicket> newTickets)
        {
            if (Tickets.ContainsKey(pnr))
            {
                Tickets[pnr].AddRange(newTickets);
            }
            else
            {
                Tickets.Add(pnr, new List<FiveTicket>(newTickets));
            }
        }
    }
}
