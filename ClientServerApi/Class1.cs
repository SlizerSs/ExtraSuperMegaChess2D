using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientServerApi
{
    public class UserLoginData
    {
        public string Login { get; set; }
        public string HashedPassword { get; set; }
    }
    public class GameSendMoveData
    {
        public int UserID { get; set; }
        public int ID { get; set; }
        public string Move { get; set; }

    }
}
