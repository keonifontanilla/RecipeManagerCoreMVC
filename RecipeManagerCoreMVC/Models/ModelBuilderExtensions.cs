using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeManagerCoreMVC.Models
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RecipeIngredientModel>()
                .HasKey(x => new { x.RecipeId, x.IngredientId });
            modelBuilder.Entity<RecipeIngredientModel>()
                .HasOne(x => x.RecipeModel)
                .WithMany(x => x.RecipeIngredientModels)
                .HasForeignKey(x => x.RecipeId);
            modelBuilder.Entity<RecipeIngredientModel>()
                .HasOne(x => x.IngredientsModel)
                .WithMany(x => x.RecipeIngredientModels)
                .HasForeignKey(x => x.IngredientId);

            modelBuilder.Entity<InstructionModel>()
                .HasOne(x => x.RecipeModel)
                .WithMany(x => x.InstructionModels)
                .HasForeignKey(x => x.RecipeId);

            modelBuilder.Entity<RecipeModel>()
                .HasOne(x => x.RecipeInfoModel)
                .WithOne(x => x.RecipeModel)
                .HasForeignKey<RecipeInfoModel>(x => x.RecipeId);
        }
    }
}
