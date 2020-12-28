using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static RecipeManagerCoreMVC.Models.Enums;

namespace RecipeManagerCoreMVC.Models
{
    public class RecipeModel
    {
        [Key]
        public int  Id { get; set; }
        [Required]
        [DisplayName("Recipe Name")]
        public string RecipeName { get; set; }
        [DisplayName("Recipe Description")]
        public string RecipeDescription { get; set; }
        [DisplayName("Recipe Type")]
        public RecipeType RecipeType { get; set; }
        [DisplayName("Created Date")]
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
        [DisplayName("Updated Date")]
        public DateTime? UpdatedDated { get; set; }
        public ICollection<RecipeIngredientModel> RecipeIngredientModels { get; set; }
        public ICollection<InstructionModel> InstructionModels { get; set; }
        public RecipeInfoModel RecipeInfoModel { get; set; }
    }
}
