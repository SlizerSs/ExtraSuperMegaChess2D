using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using ChessAPI.Models;
using ClientServerApi;
using Newtonsoft.Json;

namespace ChessAPI.Controllers
{
    public class PlayersController : Controller
    {
        private ModelChessDB db = new ModelChessDB();

        // GET: Players
        public string Index()
        {
            string result = "";
            Logic logic = new Logic();

            var players = db.Players.Include(p => p.PlayerStatistic);

            List<Player> list = players.ToList();
            foreach (Player pl in list)
            {
                result += logic.PlayerDetails(pl);
            }
            return result;
        }

        // GET: Players/Details/5
        [HttpPost]
        public string Details()
        {
            int userID;
            using (StreamReader stream = new StreamReader(HttpContext.Request.GetBufferlessInputStream()))
            {
                string body = stream.ReadToEnd();
                userID = JsonConvert.DeserializeObject<int>(body);
                
            }
            Logic logic = new Logic();
            Player player = db.Players.Find(userID);
            return logic.PlayerDetails(player);
        }
        // GET: Players/Details/name
        public string Details(string name)
        {
            Logic logic = new Logic();
            Player player = db.Players.Find(name);
            return logic.PlayerDetails(player);
        }

        [HttpPost]
        // POST: Players/Create
        public string Create()
        {
            UserLoginData loginData;
            using (StreamReader stream = new StreamReader(HttpContext.Request.GetBufferlessInputStream()))
            {
                string body = stream.ReadToEnd();
                loginData = JsonConvert.DeserializeObject<UserLoginData>(body);
                // body = "param=somevalue&param2=someothervalue"
            }
            Logic logic = new Logic();
            Player player = logic.MakeNewPlayer(loginData.Login, loginData.HashedPassword);
            PlayerStatistic ps = logic.MakeNewPS(player.PlayerID);

            ViewBag.PlayerID = new SelectList(db.PlayerStatistics, "PlayerID", "PlayerID");

            return logic.PlayerDetails(player);
        }
        
        [HttpPost]
        public string SideColor()
        {
            GetOpponentData info;
            using (StreamReader stream = new StreamReader(HttpContext.Request.GetBufferlessInputStream()))
            {
                string body = stream.ReadToEnd();
                info = JsonConvert.DeserializeObject<GetOpponentData>(body);
                // body = "param=somevalue&param2=someothervalue"
            }
            Logic logic = new Logic();

            string playerColor = logic.GetPlayerColor(info.ID, info.UserID);

            return playerColor;
        }

        // POST: Players/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public string Create([Bind(Include = "PlayerID,Name,Password")] Player player)
        //{
        //    Logic logic = new Logic();

        //    if (ModelState.IsValid)
        //    {
        //        db.Players.Add(player);
        //        db.SaveChanges();
        //    }

        //    ViewBag.PlayerID = new SelectList(db.PlayerStatistics, "PlayerID", "PlayerID", player.PlayerID);
        //    return logic.PlayerDetails(player);
        //}

        // GET: Players/Delete/5
        public string Delete(int? id)
        {
            Player player = db.Players.Find(id);
            if (player == null)
            {
                return null;
            }
            return new JavaScriptSerializer().Serialize(player);
        }

        // POST: Players/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public List<string> DeleteConfirmed(int id)
        {
            Player player = db.Players.Find(id);
            db.Players.Remove(player);
            db.SaveChanges();

            List<string> result = new List<string>();
            List<Player> list = db.Players.ToList();
            foreach (Player g in list)
                result.Add(new JavaScriptSerializer().Serialize(g));
            return result;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
