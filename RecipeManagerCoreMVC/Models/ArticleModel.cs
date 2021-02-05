using RecipeManagerCoreMVC.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeManagerCoreMVC.Models
{
    public class ArticleModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string ArticleTitle { get; set; }
        [Required]
        public string ArticleBody { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string PhotoPath { get; set; }
        public string AuthorId { get; set; }
        public ApplicationUser Author { get; set; }
    }
}
