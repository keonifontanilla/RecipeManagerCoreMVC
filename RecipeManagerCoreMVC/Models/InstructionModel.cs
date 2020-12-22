using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeManagerCoreMVC.Models
{
    public class InstructionModel
    {
        public int Id { get; set; }
        public int RecipeId { get; set; }
        public string Instruction { get; set; }
        public RecipeModel RecipeModel { get; set; }
    }
}
