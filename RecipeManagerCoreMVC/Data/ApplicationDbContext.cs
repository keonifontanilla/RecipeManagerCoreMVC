using Microsoft.EntityFrameworkCore;
using RecipeManagerCoreMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeManagerCoreMVC.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<RecipeModel> Recipes { get; set; }
        public DbSet<IngredientModel> Ingredients { get; set; }
        public DbSet<RecipeIngredientModel> RecipeIngredients { get; set; }
        public DbSet<InstructionModel> Instructions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Seed();
        }
    }
}
