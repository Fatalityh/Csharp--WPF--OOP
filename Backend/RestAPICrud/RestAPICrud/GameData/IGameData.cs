using RestAPICrud.Models;
using System;
using System.Collections.Generic;

namespace RestAPICrud.GameData
{
    public interface IGameData
    {
        List<Game> GetGames();

        Game GetGame(Guid id);

        List<Game> SearchGame(String name);

        Game AddGame(Game game);

        void DeleteGame(Game game);

        Game EditGame(Game game);
    }
}
