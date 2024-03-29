﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RecipeManagerCoreMVC.Data;

namespace RecipeManagerCoreMVC.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20210103235625_initialSeedData")]
    partial class initialSeedData
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.1");

            modelBuilder.Entity("RecipeManagerCoreMVC.Models.IngredientModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Ingredient")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Ingredients");
                });

            modelBuilder.Entity("RecipeManagerCoreMVC.Models.InstructionModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Instruction")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RecipeId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RecipeId");

                    b.ToTable("Instructions");
                });

            modelBuilder.Entity("RecipeManagerCoreMVC.Models.RecipeInfoModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("CookTime")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhotoPath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PrepTime")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RecipeId")
                        .HasColumnType("int");

                    b.Property<string>("TotalTime")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Yield")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("RecipeId")
                        .IsUnique();

                    b.ToTable("RecipeInfoModel");
                });

            modelBuilder.Entity("RecipeManagerCoreMVC.Models.RecipeIngredientModel", b =>
                {
                    b.Property<int>("RecipeId")
                        .HasColumnType("int");

                    b.Property<int>("IngredientId")
                        .HasColumnType("int");

                    b.Property<string>("IngredientQuantity")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IngredientUnit")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("RecipeId", "IngredientId");

                    b.HasIndex("IngredientId");

                    b.ToTable("RecipeIngredients");
                });

            modelBuilder.Entity("RecipeManagerCoreMVC.Models.RecipeModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("RecipeDescription")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RecipeName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RecipeType")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedDated")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Recipes");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CreatedDate = new DateTime(2021, 1, 3, 13, 56, 24, 600, DateTimeKind.Local).AddTicks(5948),
                            RecipeName = "Recipe1",
                            RecipeType = 0
                        },
                        new
                        {
                            Id = 2,
                            CreatedDate = new DateTime(2021, 1, 3, 13, 56, 24, 601, DateTimeKind.Local).AddTicks(5797),
                            RecipeName = "Recipe2",
                            RecipeType = 0
                        },
                        new
                        {
                            Id = 3,
                            CreatedDate = new DateTime(2021, 1, 3, 13, 56, 24, 601, DateTimeKind.Local).AddTicks(5811),
                            RecipeName = "Recipe3",
                            RecipeType = 0
                        });
                });

            modelBuilder.Entity("RecipeManagerCoreMVC.Models.InstructionModel", b =>
                {
                    b.HasOne("RecipeManagerCoreMVC.Models.RecipeModel", "RecipeModel")
                        .WithMany("InstructionModels")
                        .HasForeignKey("RecipeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("RecipeModel");
                });

            modelBuilder.Entity("RecipeManagerCoreMVC.Models.RecipeInfoModel", b =>
                {
                    b.HasOne("RecipeManagerCoreMVC.Models.RecipeModel", "RecipeModel")
                        .WithOne("RecipeInfoModel")
                        .HasForeignKey("RecipeManagerCoreMVC.Models.RecipeInfoModel", "RecipeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("RecipeModel");
                });

            modelBuilder.Entity("RecipeManagerCoreMVC.Models.RecipeIngredientModel", b =>
                {
                    b.HasOne("RecipeManagerCoreMVC.Models.IngredientModel", "IngredientsModel")
                        .WithMany("RecipeIngredientModels")
                        .HasForeignKey("IngredientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RecipeManagerCoreMVC.Models.RecipeModel", "RecipeModel")
                        .WithMany("RecipeIngredientModels")
                        .HasForeignKey("RecipeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("IngredientsModel");

                    b.Navigation("RecipeModel");
                });

            modelBuilder.Entity("RecipeManagerCoreMVC.Models.IngredientModel", b =>
                {
                    b.Navigation("RecipeIngredientModels");
                });

            modelBuilder.Entity("RecipeManagerCoreMVC.Models.RecipeModel", b =>
                {
                    b.Navigation("InstructionModels");

                    b.Navigation("RecipeInfoModel");

                    b.Navigation("RecipeIngredientModels");
                });
#pragma warning restore 612, 618
        }
    }
}
