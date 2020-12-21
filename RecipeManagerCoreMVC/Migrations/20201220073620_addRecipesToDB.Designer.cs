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
    [Migration("20201220073620_addRecipesToDB")]
    partial class addRecipesToDB
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.1");

            modelBuilder.Entity("RecipeManagerCoreMVC.Models.RecipeModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("RecipeDescription")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RecipeName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RecipeType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("UpdatedDated")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Recipes");
                });
#pragma warning restore 612, 618
        }
    }
}
