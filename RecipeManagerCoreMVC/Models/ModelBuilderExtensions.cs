using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RecipeManagerCoreMVC.Data;
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

            var adminId = "42f23ef6-3027-4c2b-aec0-62a7677b6597";
            var password = new PasswordHasher<ApplicationUser>();

            modelBuilder.Entity<ApplicationUser>().HasData(
                new ApplicationUser
                {
                    Id = adminId,
                    UserName = "admin",
                    NormalizedUserName = "admin".ToUpper(),
                    Email = "admin@g.com",
                    NormalizedEmail = "admin@g.com".ToUpper(),
                    EmailConfirmed = true,
                    PasswordHash = password.HashPassword(null, "123")
                }
            );

            modelBuilder.Entity<IdentityUserClaim<string>>().HasData(
                new IdentityUserClaim<string>
                {
                    Id = 1,
                    UserId = adminId,
                    ClaimType = "IsAdmin",
                    ClaimValue = "true"
                }
            );
        }
    }
}
