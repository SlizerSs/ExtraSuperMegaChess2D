using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ChessClient
{
    public class ChessClient
    {
        public string host { get; private set; }
        public string user { get; private set; }

        public ChessClient(string host, string user)
        {
            this.host = host;
            this.user = user;
        }

        public void GetCurrentGame()
        {
            
        }
        private string CallServer(string param = "")
        {
            WebRequest request = WebRequest.Create(host + user + "/" + param);
            WebResponse response = request.GetResponse();
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
                return reader.ReadToEnd();
        }
    }
}
