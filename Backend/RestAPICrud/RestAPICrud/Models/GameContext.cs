﻿using Microsoft.EntityFrameworkCore;

namespace RestAPICrud.Models
{
    public class GameContext : DbContext
    {
        public GameContext(DbContextOptions<GameContext> options) : base(options)
        {

        }

        public DbSet<Game> Games { get; set; }
    }
}
