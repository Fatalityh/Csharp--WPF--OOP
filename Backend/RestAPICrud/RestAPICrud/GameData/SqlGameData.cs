using RestAPICrud.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RestAPICrud.GameData
{
    public class SqlGameData : IGameData
    {
        private GameContext _gameContext;
        public SqlGameData(GameContext gameContext)
        {
            _gameContext = gameContext;
        }

        public Game AddGame(Game game)
        {
            game.Id = Guid.NewGuid();
            _gameContext.Games.Add(game);
            _gameContext.SaveChanges();
            return game;
        }

        public void DeleteGame(Game game)
        {
            _gameContext.Games.Remove(game);
            _gameContext.SaveChanges();
        }

        public Game EditGame(Game game)
        {
            var existingGame = _gameContext.Games.Find(game.Id);
            if (existingGame != null)
            {
                existingGame.Name = game.Name;
                existingGame.Category = game.Category;
                existingGame.Rating = game.Rating;
                existingGame.Picture = game.Picture;
                _gameContext.Games.Update(existingGame);
                _gameContext.SaveChanges();
            }
            return game;
        }

        public Game GetGame(Guid id)
        {
            var game = _gameContext.Games.Find(id);
            return game;
        }

        public List<Game> SearchGame(string name)
        {
            return _gameContext.Games.Where(g=>g.Name.Contains(name)).ToList();
        }

        public List<Game> GetGames()
        {
            return _gameContext.Games.ToList();
        }
    }
}
