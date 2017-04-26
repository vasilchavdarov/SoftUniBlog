﻿using Blog.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Blog.Models
{
    public class HomeIndexArticleModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }
        
        public string ImageUrl { get; set; }
    }
}