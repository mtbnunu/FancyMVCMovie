using FancyMVCMovie2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JBWebappLibrary.Repository;
using Microsoft.AspNet.Identity;

namespace FancyMVCMovie2.Controllers
{
    [Authorize]
    public class MovieController : BaseController
    {
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var movies = unitOfWork.Repository<Movie>().Get(a=>a.UserId.Equals(userId));
            return View(movies);
        }

        public ActionResult Search(string query)
        {
            var userId = User.Identity.GetUserId();
            var model = unitOfWork.Repository<Movie>().Get(a => a.UserId.Equals(userId) && a.MovieTitle.Contains(query));
            return View("Index", model);
        }

        public ActionResult New()
        {
            FillGenreList();
            return View("Edit");
        }

        public ActionResult Edit(int id)
        {
            var model = unitOfWork.Repository<Movie>().GetByID(id);
            if (model == null)
            {
                return new HttpNotFoundResult();
            }
            if (!model.UserId.Equals(User.Identity.GetUserId()))
            {
                return new HttpStatusCodeResult(400);
            }
            FillGenreList();
            return View("Edit", model);
        }

        public ActionResult Delete(int id)
        {
            var model = unitOfWork.Repository<Movie>().GetByID(id);
            if (model == null)
            {
                return new HttpNotFoundResult();
            }
            if (!model.UserId.Equals(User.Identity.GetUserId()))
            {
                return new HttpStatusCodeResult(400);
            }
            var title = model.MovieTitle;
            unitOfWork.Repository<Movie>().Delete(id);
            unitOfWork.Save();
            Toast("Deleted " + title);
            return RedirectToAction("Index","Movie");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(Movie model)
        {
            if (!ModelState.IsValid)
            {
                FillGenreList();
                return View("Edit", model);
            }

            model.UserId = User.Identity.GetUserId();
            if (model.Id.HasValue && model.Id > 0)
            {
                //confirm user has right to update
                var userId = User.Identity.GetUserId();
                var doesItExist = unitOfWork.Repository<Movie>().Count(a=>a.Id==model.Id && a.UserId.Equals(userId));
                if (doesItExist == 0)
                {
                    return new HttpStatusCodeResult(400);
                }
                unitOfWork.Repository<Movie>().Update(model);
                Toast("Successfully Edited " + model.MovieTitle);
            }
            else
            {
                unitOfWork.Repository<Movie>().Insert(model);
                Toast("Successfully Created " + model.MovieTitle);
            }
            unitOfWork.Save();

            return RedirectToAction("Index","Movie");
        }
    }
}