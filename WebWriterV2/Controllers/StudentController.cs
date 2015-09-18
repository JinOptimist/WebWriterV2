using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebWriterV2.Controllers
{
    public class StudentController : Controller
    {
        //
        // GET: /Student/

        private string BaseHtmlPath
        {
            get
            {
                return Server.MapPath("~/html/");
            }
        }

        public ActionResult Index()
        {
            var model = Directory.GetFiles(BaseHtmlPath).Select(Path.GetFileName).ToList();
            return View(model);
        }

        [HttpGet]
        public ActionResult AddHtmlPage()
        {
            return View();
        }

        [HttpGet]
        public ActionResult RemoteHtmlPage(string fileName)
        {
            if (!string.IsNullOrEmpty(fileName))
            {
                var path = Path.Combine(BaseHtmlPath, fileName);
                System.IO.File.Delete(path);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult AddHtmlPage(HttpPostedFileBase file)
        {
            //var file = Request.Files[0];

            if (file != null && file.ContentLength > 0)
            {
                var extension = Path.GetExtension(file.FileName);

                var allowExtension = new List<string> {".html", ".css", ".js"};
                if (extension == null || !allowExtension.Any(x=>x.Equals(extension.ToLower())))
                {
                    return View();
                }

                var fileName = Path.GetFileName(file.FileName);
                var path = Path.Combine(BaseHtmlPath, fileName);
                file.SaveAs(path);

                //var url = Url.Content("~/html/" + fileName);
                //return Redirect(url);

                return RedirectToAction("Index");
            }


            return View();
        }
    }
}
