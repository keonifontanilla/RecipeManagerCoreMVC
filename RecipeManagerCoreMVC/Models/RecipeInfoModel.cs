﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeManagerCoreMVC.Models
{
    public class RecipeInfoModel
    {
        [Key]
        public int Id { get; set; }
        public string Yield { get; set; }
        [DisplayName("Prep")]
        public string PrepTime { get; set; }
        [DisplayName("Cook")]
        public string CookTime { get; set; }
        [DisplayName("Total")]
        public string TotalTime { get; set; }
        public string PhotoPath { get; set; }
        public int RecipeId { get; set; }
        public RecipeModel RecipeModel { get; set; }
    }
}
