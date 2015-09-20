using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FancyMVCMovie.Models;
using JBWebappLibrary.Repository;

namespace FancyMVCMovie.Controllers
{
    public class BaseController : Controller
    {
        protected IUnitOfWork unitOfWork;

        public BaseController()
        {
            unitOfWork = new UnitOfWork(new DefaultContext());
        }

        protected void FillGenreList()
        {
            var genres = unitOfWork.Repository<Genre>().Get();
            ViewBag.GenreSelectList = new SelectList(genres, "Id", "Name");
        }


        protected void SetAlert(string type, string msg, string detail)
        {
            TempData["alert"] = true;
            TempData["alerttype"] = type;
            TempData["alertmsg"] = msg;
            TempData["alertmsgdetail"] = detail;
        }

        protected void Toast(string msg)
        {
            TempData["Toast"] = true;
            TempData["ToastMsg"] = msg;
        }
    }
}