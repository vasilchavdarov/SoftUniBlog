using Blog.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Blog.Models
{
    public class ArticleViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        [Url]
        [Display(Name = "Past URL Image")]
        [ImageUrl]
        public string ImageUrl { get; set; }

        public string AuthorId { get; set; }
    }
}