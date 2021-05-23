using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessClient
{
    public struct PlayerInfo
    {
        public int PlayerID { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public int Games { get; set; }
        public int Wins { get; set; }
        public int Loses { get; set; }


        public PlayerInfo(NameValueCollection list)
        {
            PlayerID = int.Parse(list["PlayerID"]);
            Name = list["Name"];
            Password = list["Password"];
            Games = int.Parse(list["Games"]);
            Wins = int.Parse(list["Wins"]);
            Loses = int.Parse(list["Loses"]);
        }
    }
}
