using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Lottery20
{
    // Dessa objekt sköter skrivande och läsande av player-objekt och ticket-objekt till och från json-filer.
    public interface IDatabaseReader<T>
    {
        internal T ReadAll(string pnr);
    }
    public interface IDatabaseWriter<T>
    {
        internal void Write(string pnr, T writeObj);
    }

    public class FiveTicketReader : IDatabaseReader<Dictionary<string ,List<FiveTicket>>>
    {
        string filepath = FiveLottery.filepath;
        public LotteryDrawFive currentLottery;
        public FiveLottery currentFiveLottery;
        public FiveTicketReader(FiveLottery currentLottery)
        {
            this.currentFiveLottery = currentLottery;
            this.currentLottery = currentLottery.lotteryDrawFive;
        }
        
        // När programmet körs så läser ReadAll in alla sparade ticket-objekt och skapar instanser av dem. 
        public Dictionary<string, List<FiveTicket>> ReadAll(string filepath)
        {
            JObject jsonObject;
            Dictionary<string, List<FiveTicket>> ticketDictionary = new Dictionary<string, List<FiveTicket>>();
            string json;
            if (!File.Exists(filepath))
            {
                Dictionary<string, List<FiveTicket>> ticketDict = new Dictionary<string, List<FiveTicket>>();
                return ticketDict;
            }
            json = File.ReadAllText(filepath);
            jsonObject = JObject.Parse(json);
            List<FiveTicket> ticketList = new List<FiveTicket>();
            foreach(KeyValuePair<string, JToken> kvp in jsonObject) 
            {
                foreach(var ticketData in kvp.Value)
                {
                    bool subscriber = ticketData["Subscriber"].ToObject<bool>();
                    int[] ticketNumbers = ticketData["ticketNumbers"].ToObject<int[]>();
                    FiveTicket ticket = new FiveTicket(currentFiveLottery, kvp.Key, subscriber, ticketNumbers);
                    ticketList.Add(ticket);
                }
                currentFiveLottery.Tickets.Add(kvp.Key, ticketList);
            }
            return ticketDictionary; 
        }
    }
    public class FiveTicketWriter : IDatabaseWriter<List<FiveTicket>>
    {
        string filepath = FiveLottery.filepath;
        public void Write(string pnr, List<FiveTicket> ticketList)
        {
            JObject jsonObject;
            if (File.Exists(filepath))
            {
                string json = File.ReadAllText(filepath);
                jsonObject = JObject.Parse(json);
            }
            else
            {
                jsonObject = new JObject();
            }

            JArray jArray;
            if (jsonObject.ContainsKey(pnr))
            {
                jArray = (JArray)jsonObject[pnr];
            }
            else
            {
                jArray = new JArray();
                jsonObject[pnr] = jArray;
            }
            var existingTickets = jArray
                .Select(t => t.ToString(Formatting.None))
                .ToHashSet();
            foreach(Ticket ticket in ticketList)
            {
                string data = JsonConvert.SerializeObject(ticket);
                if(!existingTickets.Contains(data))
                {
                    jArray.Add(JObject.Parse(data));
                }
            }
            File.WriteAllText(filepath, jsonObject.ToString(Formatting.Indented));
        }

    }
    public class PlayerWriter : IDatabaseWriter<Player>
    {
        string playerFilepath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "registreradeplayers.json");
        string guestPlayerFilepath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "oregistreradeplayers.json");
        public void Write(string pnr, Player player)
        {
            JObject jsonObject;
            if (player.Registered == true)
            {
                if (File.Exists(playerFilepath))
                {
                    string json = File.ReadAllText(playerFilepath);
                    jsonObject = JObject.Parse(json);
                }
                else
                {
                    jsonObject = new JObject();
                }
                string playerData = JsonConvert.SerializeObject(player);
                jsonObject[pnr] = (JObject.Parse(playerData));
                File.WriteAllText(playerFilepath, jsonObject.ToString(Formatting.Indented));
            }
            else
            {
                if (File.Exists(guestPlayerFilepath))
                {
                    string json = File.ReadAllText(guestPlayerFilepath);
                    jsonObject = JObject.Parse(json);
                }
                else
                {
                    jsonObject = new JObject();
                }
                string playerData = JsonConvert.SerializeObject(player);
                jsonObject[pnr] = (JObject.Parse(playerData));
                File.WriteAllText(guestPlayerFilepath, jsonObject.ToString(Formatting.Indented));
            }
            
        }
    }
    public class PlayerReader : IDatabaseReader<Player>
    {
        string playerFilepath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "registreradeplayers.json");
        string guestPlayerFilepath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "oregistreradeplayers.json");
        public Player ReadAll(string pnr)
        {
            JObject jsonObject;
            string json;
            if (!File.Exists(playerFilepath))
            {
                return null;
            }
            json = File.ReadAllText(playerFilepath);
            jsonObject = JObject.Parse(json);
            if (jsonObject.TryGetValue(pnr, out JToken playerData))
            {
                var player = playerData.ToObject<Player>();
                return player;
            }
            return null;

        }
        public Dictionary<string, Player> ReadAll()
        {
            JObject jsonObject;
            Dictionary<string, Player> playerDictionary = new Dictionary<string, Player>();
            string json;
            if (File.Exists(playerFilepath))
            {
                json = File.ReadAllText(playerFilepath);
                jsonObject = JObject.Parse(json);
                foreach (KeyValuePair<string, JToken> kvp in jsonObject)
                {
                    var playerList = kvp.Value.ToObject<Player>();
                    playerDictionary.Add(kvp.Key, playerList);
                }
            }
            else if (File.Exists(guestPlayerFilepath))
            {
                json = File.ReadAllText(guestPlayerFilepath);
                jsonObject = JObject.Parse(json);
                foreach (KeyValuePair<string, JToken> kvp in jsonObject)
                {
                    var playerList = kvp.Value.ToObject<Player>();
                    playerDictionary.Add(kvp.Key, playerList);
                }
            }
            else
            {
                Dictionary<string, Player> playerDict = new Dictionary<string, Player>();
                return playerDict;
            }
            return playerDictionary;
        }
    }
}