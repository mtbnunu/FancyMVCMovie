using FancyMVCMovie2.Models;
using FancyMVCMovie2.Models.Context;
using JBWebappLibrary.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;

namespace FancyMVCMovie2.Controllers
{
    public class BaseController : Controller
    {
        protected IUnitOfWork unitOfWork;
        protected ApplicationSignInManager _signInManager;
        protected ApplicationUserManager _userManager;


        public BaseController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            unitOfWork = new UnitOfWork(new DefaultContext());
        }

        public BaseController()
        {
            unitOfWork = new UnitOfWork(new DefaultContext());
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
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