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
    public class MakeNewSideData
    {
        public int UserID { get; set; }
        public int ID { get; set; }
        public string Color { get; set; }
    }
    public class ChangeGameStatusData
    {
        public int ID { get; set; }
        public string Status { get; set; }
    }
    public class EndGameData
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public bool IsWinner { get; set; }
    }
}
