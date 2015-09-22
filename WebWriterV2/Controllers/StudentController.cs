using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Autofac;
using Dao.IRepository;
using Dao.Model;
using WebWriterV2.Models;

namespace WebWriterV2.Controllers
{
    public class StudentController : Controller
    {
        //
        // GET: /Student/

        public IStudentLoginRepository StudentLoginRepository { get; set; }

        public StudentController()
        {
            using (var scope = StaticContainer.Container.BeginLifetimeScope())
            {
                StudentLoginRepository = scope.Resolve<IStudentLoginRepository>();
            }
        }

        private string BaseHtmlPath
        {
            get
            {
                return Server.MapPath("~/html/");
            }
        }

        public ActionResult AllFiles()
        {
            var model = Directory.GetDirectories(BaseHtmlPath).Select(directoryName => new StudentHtmlFile
            {
                StudentName = Path.GetFileName(directoryName),
                FilesName = Directory.GetFiles(Path.Combine(BaseHtmlPath, directoryName)).Select(Path.GetFileName).ToList() 
            }).ToList();
            
            return View(model);
        }

        public ActionResult MyFiles()
        {
            var idStr = Session["StudentId"];
            if (idStr == null)
            {
                RedirectToAction("Login");
            }

            var id = long.Parse(idStr + "");
            var student = StudentLoginRepository.GetStudent(id);

            var path = Path.Combine(BaseHtmlPath, student.Name);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var model = new StudentHtmlFile
            {
                StudentName = student.Name,
                FilesName = Directory.GetFiles(path).Select(Path.GetFileName).ToList()
            };

            return View(model);
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(StudentLogin studentLogin)
        {
            if (StudentLoginRepository.IsExist(studentLogin.Name))
            {
                studentLogin = StudentLoginRepository.Login(studentLogin.Name, studentLogin.Password);
            }

            if (studentLogin == null || !StudentLoginRepository.SaveStudent(studentLogin))
            {
                return View(studentLogin);
            }

            Session["StudentId"] = studentLogin.Id;
            return RedirectToAction("MyFiles");
        }

        public ActionResult Example()
        {
            return View();
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

            return RedirectToAction("MyFiles");
        }

        [HttpPost]
        public ActionResult AddHtmlPage(HttpPostedFileBase file, string studentName, bool useValidation)
        {
            //var file = Request.Files[0];
            if (file != null && file.ContentLength > 0)
            {
                var extension = Path.GetExtension(file.FileName);
                if (useValidation)
                {
                    var allowExtension = new List<string> { ".html", ".css", ".js", ".jpg", ".mp3" };
                    if (extension == null || !allowExtension.Any(x => x.Equals(extension.ToLower())))
                    {
                        return View();
                    }
                }

                var directoryPath = Path.Combine(BaseHtmlPath, studentName.ToLower());
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                var fileName = Path.GetFileName(file.FileName);
                var path = Path.Combine(directoryPath, fileName);
                file.SaveAs(path);

                //var url = Url.Content("~/html/"studentName + "/" + fileName);
                //return Redirect(url);

                return RedirectToAction("MyFiles");
            }

            return View();
        }
    }
}
