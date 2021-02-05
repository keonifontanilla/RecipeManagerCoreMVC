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

            modelBuilder.Entity<RecipeModel>()
                .HasOne(x => x.Author)
                .WithMany(x => x.RecipeModels)
                .OnDelete(DeleteBehavior.SetNull)
                .HasForeignKey(x => x.AuthorId);

            modelBuilder.Entity<FavoriteModel>()
                .HasKey(x => new { x.RecipeId, x.UserId });
            modelBuilder.Entity<FavoriteModel>()
                .HasOne(x => x.ApplicationUser)
                .WithMany(x => x.FavoriteModels)
                .HasForeignKey(x => x.UserId);
            modelBuilder.Entity<FavoriteModel>()
                .HasOne(x => x.RecipeModel)
                .WithMany(x => x.FavoriteModels)
                .HasForeignKey(x => x.RecipeId);

            modelBuilder.Entity<ArticleModel>()
                .HasOne(x => x.Author)
                .WithMany(x => x.ArticleModels)
                .OnDelete(DeleteBehavior.SetNull)
                .HasForeignKey(x => x.AuthorId);

            InitialData(modelBuilder);
        }

        private static void InitialData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RecipeModel>().HasData(
                new RecipeModel { Id = 1, RecipeName = "Recipe1" },
                new RecipeModel { Id = 2, RecipeName = "Recipe2" },
                new RecipeModel { Id = 3, RecipeName = "Recipe3" }
            );
        }
    }
}
