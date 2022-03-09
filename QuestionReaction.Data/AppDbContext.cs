using Microsoft.EntityFrameworkCore;
using QuestionReaction.Data.Model;
using System;

namespace QuestionReaction.Data
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions options): base(options)
        {

        }

        // Tables
        public DbSet<User> Users { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Choice> Choices { get; set; }
        public DbSet<Reaction> Reactions { get; set; }
        public DbSet<Guest> Guests { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region DATAS

            #endregion

            base.OnModelCreating(modelBuilder);
        }

    }
}
