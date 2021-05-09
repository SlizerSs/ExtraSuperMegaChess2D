using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using ChessAPI.Models;

namespace ChessAPI.Controllers
{
    public class PlayersController : Controller
    {
        private ModelChessDB db = new ModelChessDB();

        // GET: Players
        public List<Player> Index()
        {
            var players = db.Players.Include(p => p.PlayerStatistic);

            List<Player> list = players.ToList();
            return list;
        }

        // GET: Players/Details/5
        public Player Details(int? id)
        {

            Player player = db.Players.Find(id);
            return player;
        }
        // GET: Players/Details/name
        public Player Details(string name)
        {

            Player player = db.Players.Find(name);
            return player;
        }

        // GET: Players/Create
        public Player Create(string name, string password)
        {
            Logic logic = new Logic();
            Player player = logic.MakeNewPlayer(name, password);
            PlayerStatistic ps = logic.MakeNewPS(player.PlayerID);

            ViewBag.PlayerID = new SelectList(db.PlayerStatistics, "PlayerID", "PlayerID");
            return player;
        }

        // POST: Players/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public string Create([Bind(Include = "PlayerID,Name,Password")] Player player)
        {
            if (ModelState.IsValid)
            {
                db.Players.Add(player);
                db.SaveChanges();
            }

            ViewBag.PlayerID = new SelectList(db.PlayerStatistics, "PlayerID", "PlayerID", player.PlayerID);
            return new JavaScriptSerializer().Serialize(player);
        }

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
