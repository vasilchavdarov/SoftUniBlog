﻿using Blog.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blog.Controllers
{
    public class ArticleController : Controller
    {
        public ActionResult List()
        {
            using (var db = new BlogDbContext())
            {
                var articles = db.Articles.OrderByDescending(a => a.Id).Include(a => a.Author).ToList();

                return View(articles);
            }
        }

        [Authorize]
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult Create(Article model)
        {
            if (ModelState.IsValid)
            {
                using (var db = new BlogDbContext())
                {
                    var authorId = this.User.Identity.GetUserId();

                    model.AuthorId = authorId;

                    db.Articles.Add(model);
                    db.SaveChanges();
                }

                return RedirectToAction("List");
            }

            return View(model);
        }

        public ActionResult Details(int id)
        {
            using (var db = new BlogDbContext())
            {
                var article = db.Articles.Include(a => a.Author).Where(a => a.Id == id).FirstOrDefault();

                if (article == null)
                {
                    return HttpNotFound();
                }

                return View(article);
            }
        }

        [Authorize]
        [HttpGet]
        public ActionResult Delete(int id)
        {
            using (var db = new BlogDbContext())
            {
                var article = db.Articles.Find(id);

                if (article == null || !IsAuthorized(article))
                {
                    return HttpNotFound();
                }

                return View(article);
            }
        }

        [Authorize]
        [ActionName("Delete")]
        [HttpPost]
        public ActionResult ConfirmDelete(int id)
        {
            using (var db = new BlogDbContext())
            {
                var article = db.Articles.Find(id);

                if (article == null || !IsAuthorized(article))
                {
                    return HttpNotFound();
                }

                db.Articles.Remove(article);
                db.SaveChanges();

                return RedirectToAction("List");
            }
        }

        [Authorize]
        [HttpGet]
        public ActionResult Edit(int id)
        {
            using (var db = new BlogDbContext())
            {
                var article = db.Articles.Find(id);

                if (article == null || !IsAuthorized(article))
                {
                    return HttpNotFound();
                }

                var articleViewModel = new ArticleViewModel
                {
                    Id = article.Id,
                    Title = article.Title,
                    Content = article.Content,
                    AuthorId = article.AuthorId
                };

                return View(articleViewModel);
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult Edit(ArticleViewModel model)
        {
            if (ModelState.IsValid)
            {
                using (var db = new BlogDbContext())
                {
                    var article = db.Articles.Find(model.Id);

                    if (article == null || !IsAuthorized(article))
                    {
                        return HttpNotFound();
                    }

                    article.Title = model.Title;
                    article.Content = model.Content;

                    db.SaveChanges();
                }

                return RedirectToAction("Details", new { id = model.Id });
            }

            return View(model);
        }

        public bool IsAuthorized(Article article)
        {
            var isAdmin = this.User.IsInRole("Admin");
            var isAuthor = article.IsAuthor(this.User.Identity.GetUserId());

            return isAdmin || isAuthor;
        }
    }
}