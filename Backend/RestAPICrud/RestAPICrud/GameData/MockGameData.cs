using RestAPICrud.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RestAPICrud.GameData
{
    public class MockGameData : IGameData
    {
        private List<Game> games = new List<Game>()
        {
            new Game()
            {
                Id = Guid.NewGuid(),
                Name = "Battlefield 2042",
                Category = "Action",
                Rating = "5/5",
                //Picture = "google.com"
            },
            new Game()
            {
                Id = Guid.NewGuid(),
                Name = "Dying Light 2",
                Category = "Horror",
                Rating = "5/5",
                //Picture = "google.com"
            }
        };

        public Game AddGame(Game game)
        {
            game.Id = Guid.NewGuid();
            games.Add(game);
            return game;
        }

        public void DeleteGame(Game game)
        {
            games.Remove(game);
        }

        public Game EditGame(Game game)
        {
            var existingGame = GetGame(game.Id);
            existingGame.Name = game.Name;
            existingGame.Category = game.Category;
            existingGame.Rating = game.Rating;
            existingGame.Picture = game.Picture;
            return existingGame;
        }

        public Game GetGame(Guid id)
        {
            return games.SingleOrDefault(x => x.Id == id);
        }

        public List<Game> SearchGame(string name)
        {
            return games.Where(g => g.Name == name).ToList();
        }

        public List<Game> GetGames()
        {
            return games;
        }


    }
}
