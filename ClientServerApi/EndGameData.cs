using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientServerApi
{
    public class EndGameData
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public bool IsWinner { get; set; }
        public bool IsStalemate { get; set; }
    }
}
