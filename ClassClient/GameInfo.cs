using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessClient
{
    public struct GameInfo
    {
        public int GameID { get; set; }
        public string FEN { get; set; }
        public string Status { get; set; }
        public string White { get; set; }
        public string Black { get; set; }
        public string LastMove { get; set; }
        public string YourColor { get; set; }
        public string OfferDraw { get; set; }
        public string Winner { get; set; }

        public GameInfo(NameValueCollection list)
        {
            
            GameID = int.Parse(list["GameID"]);
            FEN = list["FEN"];
            Status = list["Status"];
            White = list["White"];
            Black = list["Black"];
            LastMove = list["LastMove"];
            YourColor = list["YourColor"];
            OfferDraw = list["OfferDraw"];
            Winner = list["Winner"];
        }
    }
}
