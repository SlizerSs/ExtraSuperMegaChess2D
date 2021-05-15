using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using ClientServerApi;

namespace ChessClient
{
    public class Client
    {
        private readonly HttpClient _httpClient;
        private readonly string _serverUrl; // "http://localhost:5001/api/Cards"
        private readonly Uri _apiUri;
        public string host { get; private set; }
        public int userID { get; private set; }

        public Client(string host, int userid)
        {
            this.host = host;
            this.userID = userid;

            _serverUrl = "http://localhost:56213/";
            _httpClient = new HttpClient();
            _apiUri = new Uri(_serverUrl);
        }
        public Client(string host)
        {
            this.host = host;

            _serverUrl = "http://localhost:56213/";
            _httpClient = new HttpClient();
            _apiUri = new Uri(_serverUrl);
        }

        public async Task<GameInfo> GetCurrentGame()
        {
            return new GameInfo(ParseJSON(await CallServer()));
        }

        public async Task<List<GameInfo>> GetAllGames()
        {
            List<GameInfo> result = new List<GameInfo>();

            foreach (NameValueCollection col in MultipleParseJSON(await CallServerForGames()))
                result.Add(new GameInfo(col));
            return result;
        }

        public async Task<GameInfo> SendMove(int id, string move)
        {
            return new GameInfo(ParseJSON(await CallServer(id, move)));
        }

        public async Task<PlayerInfo> MakeNewPlayer(string name, string password)
        {
            return new PlayerInfo(ParseJSON(await CallServerForPlayer(name, password)));
        }
        public async Task<List<PlayerInfo>> GetAllPlayers()
        {
            List<PlayerInfo> result = new List<PlayerInfo>();

            foreach (NameValueCollection col in MultipleParseJSON(await CallServerForPlayer()))
                result.Add(new PlayerInfo(col));
            return result;
        }

        private async Task<string> CallServer(int param1, string param2 = "")
        {
            //WebRequest request = WebRequest.Create(host + userID + "/" + param);
            //WebResponse response = request.GetResponse();
            //using (Stream stream = response.GetResponseStream())
            //using (StreamReader reader = new StreamReader(stream))
            //    return reader.ReadToEnd();
            var bodyData = JsonConvert.SerializeObject(new GameSendMoveData { UserID = userID, ID = param1, Move = param2 });
            var requestResponse = await _httpClient.PostAsync($"{ _serverUrl}Games/Details/", new StringContent(bodyData, Encoding.UTF8, "application/json")).ConfigureAwait(false);
            return await requestResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
        }
        private async Task<string> CallServer()
        {
            //WebRequest request = WebRequest.Create(host + "/" + userID);
            //    WebResponse response = request.GetResponse();
            //    using (Stream stream = response.GetResponseStream())
            //    using (StreamReader reader = new StreamReader(stream))
            //        return reader.ReadToEnd();
            var bodyData = JsonConvert.SerializeObject(userID);
            var requestResponse = await _httpClient.PostAsync($"{ _serverUrl}Games/Create/", new StringContent(bodyData, Encoding.UTF8, "application/json")).ConfigureAwait(false);
            return await requestResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
        }

        private async Task<string> CallServerForPlayer(string param1 = "", string param2 = "")
        {

            //WebRequest request = WebRequest.Create(host + param1 + "/" + param2);
            //request.Method = "POST";
            //request.
            //WebResponse response = request.GetResponse();
            //using (Stream stream = response.GetResponseStream())
            //using (StreamReader reader = new StreamReader(stream))
            //    return reader.ReadToEnd();
            var bodyData = JsonConvert.SerializeObject(new UserLoginData { Login = param1, HashedPassword = param2 });
            var requestResponse = await _httpClient.PostAsync($"{ _serverUrl}Players/Create/", new StringContent(bodyData, Encoding.UTF8, "application/json")).ConfigureAwait(false);
            return await requestResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
        }
        private async Task<string> CallServerForPlayer()
        {
            //WebRequest request = WebRequest.Create(host);
            //WebResponse response = request.GetResponse();
            //using (Stream stream = response.GetResponseStream())
            //using (StreamReader reader = new StreamReader(stream))
            //    return reader.ReadToEnd();
            var bodyData = "";
            var requestResponse = await _httpClient.PostAsync($"{ _serverUrl}Players", new StringContent(bodyData, Encoding.UTF8, "application/json")).ConfigureAwait(false);
            return await requestResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
        }
        private async Task<string> CallServerForGames()
        {
            //WebRequest request = WebRequest.Create(host);
            //WebResponse response = request.GetResponse();
            //using (Stream stream = response.GetResponseStream())
            //using (StreamReader reader = new StreamReader(stream))
            //    return reader.ReadToEnd();
            var bodyData = "";
            var requestResponse = await _httpClient.PostAsync($"{ _serverUrl}Games", new StringContent(bodyData, Encoding.UTF8, "application/json")).ConfigureAwait(false);
            return await requestResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
        }
        private NameValueCollection ParseJSON(string json)
        {
            NameValueCollection list = new NameValueCollection();

            string pattern = @"""(\w+)\"":""?([^,""}]*)""?";

            foreach (Match m in Regex.Matches(json, pattern))
                if (m.Groups.Count == 3)
                    list[m.Groups[1].Value] = m.Groups[2].Value;

            return list;
        }
        private List<NameValueCollection> MultipleParseJSON(string json)
        {
            NameValueCollection list = new NameValueCollection();
            List<NameValueCollection> result = new List<NameValueCollection>();
            string pattern = @"""(\w+)\"":""?([^,""}]*)""?";
            int x = 0;
            foreach (Match m in Regex.Matches(json, pattern))
            {
                if (m.Groups.Count == 3)
                    list[m.Groups[1].Value] = m.Groups[2].Value;
                x++;
                if (x%8==0)
                {
                    result.Add(list);
                    list = new NameValueCollection();
                }
            }




            return result;
        }
    }
}
