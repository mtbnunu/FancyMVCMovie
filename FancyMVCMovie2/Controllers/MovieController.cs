using FancyMVCMovie2.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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
        #region View

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

        public ActionResult Detail(int id)
        {
            var model = unitOfWork.Repository<Movie>().GetByID(id);
            if (!model.UserId.Equals(User.Identity.GetUserId()))
            {
                return new HttpStatusCodeResult(400);
            }
            return View(model);
        }

        #endregion

        #region New/Edit/Delete

        public ActionResult New()
        {
            ClearImageSession();
            ViewBag.guidList = new List<Guid>();
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

            ClearImageSession();
            foreach (PictureModel img in model.Pictures)
            {
                img.TempGuid = Guid.NewGuid();
            }
            Session["Images"] = model.Pictures.Select(a => a.Clone()).ToList();
            ViewBag.guidList = CurrentSessionImages;

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
                ViewBag.guidList = CurrentSessionImages;
                return View("Edit", model);
            }

            model.UserId = User.Identity.GetUserId();
            if (model.Id > 0)
            {
                //confirm user has right to update
                var userId = User.Identity.GetUserId();
                var doesItExist = unitOfWork.Repository<Movie>().Count(a=>a.Id==model.Id && a.UserId.Equals(userId));
                if (doesItExist == 0)
                {
                    return new HttpStatusCodeResult(400);
                }

                var originalImages = unitOfWork.Repository<PictureModel>().Get(a => a.MovieId == model.Id);
                foreach (var originalImage in originalImages)
                {
                    unitOfWork.Repository<PictureModel>().Delete(originalImage);
                }
                model.Pictures = ((List<PictureModel>)Session["Images"]);
                foreach (var img in model.Pictures)
                {
                    img.MovieId = (int)model.Id;
                    unitOfWork.Repository<PictureModel>().Insert(img);
                }

                unitOfWork.Repository<Movie>().Update(model);
                Toast("Successfully Edited " + model.MovieTitle);
            }
            else
            {
                model.Pictures = ((List<PictureModel>)Session["Images"]);
                unitOfWork.Repository<Movie>().Insert(model);
                Toast("Successfully Created " + model.MovieTitle);
            }
            unitOfWork.Save();

            return RedirectToAction("Index","Movie");
        }

        #endregion
        
        #region Images

        [HttpPost]
        public ActionResult UploadImage(List<HttpPostedFileBase> files)
        {
            foreach (var file in files)
            {
                if (file != null && file.ContentLength > 0)
                {
                    MemoryStream target = new MemoryStream();
                    file.InputStream.CopyTo(target);
                    byte[] data = target.ToArray();

                    //create thumbnail
                    MemoryStream stream = new MemoryStream(data);
                    var img = Image.FromStream(stream);

                    var thumbwidth = Convert.ToInt32((150.0 / img.Height) * img.Width);
                    var thumb = img.GetThumbnailImage(thumbwidth, 150, () => false, IntPtr.Zero);


                    MemoryStream ms = new MemoryStream();
                    thumb.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    var thumbImage = ms.ToArray();

                    var imageModel = new PictureModel()
                    {
                        ImageFull = data,
                        ImageThumbnail = thumbImage,
                        TempGuid = Guid.NewGuid()
                    };
                    SaveImageToSession(imageModel);
                }
            }

            return PartialView("_UploadImages", CurrentSessionImages);
        }

        private IEnumerable<Guid> CurrentSessionImages
        {
            get
            {
                if (Session["Images"] == null)
                {
                    return new List<Guid>();
                }
                return ((List<PictureModel>)Session["Images"]).Select(a => a.TempGuid);
            }
        }

        private void SaveImageToSession(PictureModel imgModel)
        {
            if (Session["Images"] == null)
            {
                Session["Images"] = new List<PictureModel>();
            }
            ((List<PictureModel>)Session["Images"]).Add(imgModel);
        }

        private void ClearImageSession()
        {
            Session["Images"] = new List<PictureModel>();
        }

        public FileResult GetTempImageThumb(Guid guid)
        {
            if (Session["Images"] == null)
            {
                return null;
            }
            var imgModel = ((List<PictureModel>)Session["Images"]).First(a => a.TempGuid == guid);
            if (imgModel == null)
            {
                return null;
            }
            return File(imgModel.ImageThumbnail, "image/png");
        }

        public ActionResult DeleteTempImage(Guid guid)
        {
            if (Session["Images"] == null)
            {
                return null;
            }
            var imgModel = ((List<PictureModel>)Session["Images"]).First(a => a.TempGuid == guid);
            if (imgModel != null)
            {
                ((List<PictureModel>)Session["Images"]).Remove(imgModel);
            }
            return PartialView("_UploadImages", CurrentSessionImages);
        }

        public FileResult GetImageFull(int id)
        {
            var imagemodel = unitOfWork.Repository<PictureModel>().GetByID(id);
            if (imagemodel == null)
            {
                return null;
            }
            return File(imagemodel.ImageFull, "image/png");
        }

        public FileResult GetImageThumb(int id)
        {
            var imagemodel = unitOfWork.Repository<PictureModel>().GetByID(id);
            if (imagemodel == null)
            {
                return null;
            }
            return File(imagemodel.ImageThumbnail, "image/png");
        }

    #endregion
    }
}