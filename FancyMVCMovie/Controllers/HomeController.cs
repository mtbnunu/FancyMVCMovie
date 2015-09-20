using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FancyMVCMovie.Models;

namespace FancyMVCMovie.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            var movies = unitOfWork.Repository<Movie>().Get();
            return View(movies);
        }

        public ActionResult Search(string query)
        {
            var model = unitOfWork.Repository<Movie>().Get(a => a.MovieTitle.Contains(query));
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
                throw new Exception("Invalid Id");
            }
            FillGenreList();
            return View("Edit", model);
        }

        public ActionResult Delete(int id)
        {
            var model = unitOfWork.Repository<Movie>().GetByID(id);
            if (model == null)
            {
                throw new Exception("Wrong ID");
            }
            var title = model.MovieTitle;
            unitOfWork.Repository<Movie>().Delete(id);
            unitOfWork.Save();
            Toast("Deleted " + title);
            return RedirectToAction("Index");
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

            if (model.Id.HasValue && model.Id > 0)
            {
                unitOfWork.Repository<Movie>().Update(model);
                Toast("Successfully Edited " + model.MovieTitle);
            }
            else
            {
                unitOfWork.Repository<Movie>().Insert(model);
                Toast("Successfully Created " + model.MovieTitle);
            }
            unitOfWork.Save();

            return RedirectToAction("Index");
        }
    }
}