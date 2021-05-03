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
    public class PlayerStatisticsController : Controller
    {
        private ModelChessDB db = new ModelChessDB();

        // GET: PlayerStatistics
        public List<PlayerStatistic> Index()
        {
            var playerStatistics = db.PlayerStatistics.Include(p => p.Player);
            return playerStatistics.ToList();
        }

        // GET: PlayerStatistics/Details/5
        public PlayerStatistic Details(int? id)
        {

            PlayerStatistic playerStatistic = db.PlayerStatistics.Find(id);

            return playerStatistic;
        }

        // GET: PlayerStatistics/Create
        public string Create(int PlayerID)
        {
            Logic logic = new Logic();
            PlayerStatistic playerStatistics = logic.MakeNewPS(PlayerID);

            ViewBag.PlayerID = new SelectList(db.Players, "PlayerID", "Name");
            return new JavaScriptSerializer().Serialize(playerStatistics);
        }

        // POST: PlayerStatistics/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public string Create([Bind(Include = "PlayerID,Games,Wins,Loses,Draws,Resigns")] PlayerStatistic playerStatistic)
        {
            if (ModelState.IsValid)
            {
                db.PlayerStatistics.Add(playerStatistic);
                db.SaveChanges();
            }

            ViewBag.PlayerID = new SelectList(db.Players, "PlayerID", "Name", playerStatistic.PlayerID);
            return new JavaScriptSerializer().Serialize(playerStatistic);
        }

        //// GET: PlayerStatistics/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    PlayerStatistic playerStatistic = db.PlayerStatistics.Find(id);
        //    if (playerStatistic == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.PlayerID = new SelectList(db.Players, "PlayerID", "Name", playerStatistic.PlayerID);
        //    return View(playerStatistic);
        //}

        //// POST: PlayerStatistics/Edit/5
        //// Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        //// сведения см. в разделе https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "PlayerID,Games,Wins,Loses,Draws,Resigns")] PlayerStatistic playerStatistic)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(playerStatistic).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.PlayerID = new SelectList(db.Players, "PlayerID", "Name", playerStatistic.PlayerID);
        //    return View(playerStatistic);
        //}

        //// GET: PlayerStatistics/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    PlayerStatistic playerStatistic = db.PlayerStatistics.Find(id);
        //    if (playerStatistic == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(playerStatistic);
        //}

        //// POST: PlayerStatistics/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    PlayerStatistic playerStatistic = db.PlayerStatistics.Find(id);
        //    db.PlayerStatistics.Remove(playerStatistic);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

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
