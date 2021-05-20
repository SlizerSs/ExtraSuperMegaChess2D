using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientServerApi
{
    public class GameSendMoveData
    {
        public int UserID { get; set; }
        public int ID { get; set; }
        public string Move { get; set; }

    }
}
