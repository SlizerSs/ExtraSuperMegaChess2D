using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtraSuperMegaChess2D
{
    class User
    {
        public string login { get; }
        public string password { get; }
        public int id { get; }
        public User(string l, string p)
        {
            login = l;
            password = p;
        }
    }
}
