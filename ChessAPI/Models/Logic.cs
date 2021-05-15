using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChessLogic;

namespace ChessAPI.Models
{
    public class Logic
    {
        ModelChessDB db;
        public Logic()
        {
            db = new ModelChessDB();
        }
        public Game MakeNewGame()
        {
            Chess chess = new Chess();
            Game game = new Game();
            game.FEN = chess.fen;
            game.Status = "wait";
            game.Winner = "";
            game.LastMove = "";


            db.Games.Add(game);
            db.SaveChanges();

            return game;
        }
        public Game GetGame(int id)
        {
            return db.Games.Find(id);
        }
        public Game MakeMove(int id, string move)
        {
            Game game = GetGame(id);
            if (game == null) 
                return game;

            if (game.Status != "play")
                return game;

            Chess chess = new Chess(game.FEN);
            Chess chessNext = chess.Move(move);

            if (chessNext.fen == game.FEN)
                return game;

            game.FEN = chessNext.fen;

            if (chessNext.IsMate() || chessNext.IsStalemate())
                game.Status = "done";

            db.Entry(game).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return game;
        }
        public Player MakeNewPlayer(string name, string password)
        {
            Player player = new Player();
            player.Name = name;
            player.Password = password;

            db.Players.Add(player);
            db.SaveChanges();

            return player;
        }
        public Side MakeNewSide(int GameID, int PlayerID, string color)
        {
            Side side = new Side();
            side.GameID = GameID;
            side.PlayerID = PlayerID;
            side.Color = color;
            side.OffersDraw = false;

            db.Sides.Add(side);
            db.SaveChanges();

            return side;
        }
        public PlayerStatistic MakeNewPS(int PlayerID)
        {
            PlayerStatistic ps = new PlayerStatistic();
            ps.PlayerID = PlayerID;
            ps.Games = 0;
            ps.Wins = 0;
            ps.Loses = 0;
            ps.Draws = 0;
            ps.Resigns = 0;
            
            db.PlayerStatistics.Add(ps);
            db.SaveChanges();

            return ps;
        }
        public string GameDetails(Game game, int playerID)
        {
            if (game == null)
                return null;
            string whitePlayer = "";
            string blackPlayer = "";
            string yourColor = "";
            string offerDraw = "";

            foreach (Side s in game.Sides)
            {
                if (s.Color == "w")
                    whitePlayer = Convert.ToString(s.PlayerID);
                else if (s.Color == "b")
                    blackPlayer = Convert.ToString(s.PlayerID);

                if (s.PlayerID == playerID)
                {
                    yourColor = s.Color;
                }

                if (s.OffersDraw)
                {
                    offerDraw = Convert.ToString(s.PlayerID);
                }
            }


            return "{"
                + $"\"GameID\":{game.GameID},"
                + $"\"FEN\":\"{game.FEN}\","
                + $"\"Status\":\"{game.Status}\","
                + $"\"White\":{whitePlayer},"
                + $"\"Black\":{blackPlayer},"
                + $"\"LastMove\":{game.LastMove},"
                + $"\"YourColor\":\"{yourColor}\","
                + $"\"OfferDraw\":{offerDraw},"
                + $"\"Winner\":\"{game.Winner}\""
                + "}"
                ;
        }
        public string LiteGameDetails(Game game)
        {
            if (game == null)
                return null;
            string whitePlayer = "";
            string blackPlayer = "";

            foreach (Side s in game.Sides)
            {
                if (s.Color == "w")
                    whitePlayer = Convert.ToString(s.PlayerID);
                else if (s.Color == "b")
                    blackPlayer = Convert.ToString(s.PlayerID);
            }


            return "{"
                + $"\"GameID\":{game.GameID},"
                + $"\"FEN\":\"{game.FEN}\","
                + $"\"Status\":\"{game.Status}\","
                + $"\"White\":{whitePlayer},"
                + $"\"Black\":{blackPlayer},"
                + $"\"LastMove\":{game.LastMove},"
                + $"\"Winner\":\"{game.Winner}\""
                + "}"
                ;
        }
        public string PlayerDetails(Player player)
        {
            if (player == null)
                return null;

            return "{"
                + $"\"PlayerID\":{player.PlayerID},"
                + $"\"Name\":\"{player.Name}\","
                + $"\"Password\":\"{player.Password}\","
                + $"\"Games\":{player.PlayerStatistic.Games},"
                + $"\"Wins\":{player.PlayerStatistic.Wins},"
                + $"\"Loses\":{player.PlayerStatistic.Loses},"
                + $"\"Draws\":{player.PlayerStatistic.Draws},"
                + $"\"Resigns\":{player.PlayerStatistic.Resigns}"
                + "}"
                ;
        }

    }
}