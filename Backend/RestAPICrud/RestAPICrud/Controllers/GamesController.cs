using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestAPICrud.GameData;
using RestAPICrud.Models;
using System;

namespace RestAPICrud.Controllers
{
    [ApiController]
    public class GamesController : ControllerBase
    {
        private IGameData _gameData;

        public GamesController(IGameData gameData)
        {
            _gameData = gameData;
        }

        [HttpGet]
        [Route("api/[controller]")]
        public IActionResult GetGames()
        {
            return Ok(_gameData.GetGames());
        }

        [HttpGet]
        [Route("api/[controller]/{id}")]
        public IActionResult GetGame(Guid id)
        {
            var game = _gameData.GetGame(id);

            if (game != null)
            {
                return Ok(game);
            }

            return NotFound($"The Game with ID: {id} was not found");
        }


        [HttpGet]
        [Route("api/[controller]/search/{name}")]
        public IActionResult SearchGame(string name)
        {
            var game = _gameData.SearchGame(name);

            if (game != null)
            {
                return Ok(game);
            }

            return NotFound($"The Game with Name: {name} was not found");
        }


        [HttpPost]
        [Route("api/[controller]")]
        public IActionResult GetGame(Game game)
        {
            _gameData.AddGame(game);

            return Created(HttpContext.Request.Scheme + "://" + HttpContext.Request.Host + HttpContext.Request.Path + "/" + game.Id, game);
        }

        [HttpDelete]
        [Route("api/[controller]/{id}")]
        public IActionResult DeleteGame(Guid id)
        {
            var game = _gameData.GetGame(id);

            if (game != null)
            {
                _gameData.DeleteGame(game);
                return Ok();
            }

            return NotFound($"The Game with ID: {id} was not found");
        }

        [HttpPut]
        [Route("api/[controller]/{id}")]
        public IActionResult EditGame(Guid id, Game game)
        {
            var existingGame = _gameData.GetGame(id);

            if (existingGame != null)
            {
                game.Id = existingGame.Id;
                _gameData.EditGame(game);
            }

            return Ok(game);
        }
    }
}
