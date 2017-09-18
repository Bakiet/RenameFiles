using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using RenameFiles.Models;
using RenameFiles.Managers;
using System.Net;
using System.Threading.Tasks;

namespace RenameFiles.Controllers
{
    public class HomeController : Controller
    {
        private renamefilesEntities db = new renamefilesEntities();
        private readonly CompanyStamp_tabManager _CompanyStamp_tabManager = new CompanyStamp_tabManager();

        static string filenumber = ConfigurationManager.AppSettings["filenumber"];
        static string filename = ConfigurationManager.AppSettings["filename"];
        static string folderfiles = ConfigurationManager.AppSettings["folderfiles"];


        /*
        [HttpPost]
        public ActionResult Photo(string Imagename)
        {
            string sss = Session["val"].ToString();

            ViewBag.pic = "http://localhost:59406/WebImages/" + Session["val"].ToString();

            return View();
        }
        */

        public ActionResult Index(string searchstring, string Imagename)
        {

            if (Convert.ToString(Session["val"]) != string.Empty)
            {
                string sss = Session["val"].ToString();
               ViewBag.pic = "http://localhost:59406/WebImages/" + Session["val"].ToString();
               // ViewBag.pic = @"C://temp/" + Session["val"].ToString();
                 string sss2 = @"C://temp/" + Session["val"].ToString();
                 Session["capture"] = sss2;
                
            }
           
           
            ViewData["filenumber"] = filenumber;
            ViewData["filename"] = filename;
             
            var path = "";
            foreach (var imgPath in Directory.GetFiles(@"C:\temp/", "*.*"))
            {
                path = @" " + imgPath + "";
            }

            if (path == "") {

                ViewBag.Message = "Not Images in folder.";
                ViewData["filenumber"] = "";
                ViewData["filename"] = "";
            }
            var companyStamp_tab = new CompanyStamp_tab();
            var companyStamp = new CompanyStamp_tab();
            //filtering data based on search string..
            if (!String.IsNullOrEmpty(searchstring))
            {
                companyStamp_tab = _CompanyStamp_tabManager.Find(searchstring);
                ViewBag.companyStamp = _CompanyStamp_tabManager.Find(searchstring);
                Session["filepath"] = companyStamp_tab.FilePath;
                return View(companyStamp_tab);
            }
            else
            {
                ViewBag.companyStamp_tab = db.CompanyStamp_tab.ToList();
                ViewBag.companyStamp = _CompanyStamp_tabManager.Find("");
                return View("Index");
            }

        }

        
        public ActionResult GetImg(string id)
        {

            var path = @" " + folderfiles + "" + id + ".jpg";
            var bytes = System.IO.File.ReadAllBytes(path);
            return File(bytes, "image/jpg");
        }

        public ActionResult Save([Bind(Include = "ID,FilePath,FileName,FileNumber")] CompanyStamp_tab companystamp_tab)
        {
            if (ModelState.IsValid)
            {
                
                Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
                config.AppSettings.Settings.Remove("Filenumber");
                config.AppSettings.Settings.Add("Filenumber", companystamp_tab.FileNumber);
                config.AppSettings.Settings.Remove("Filename");
                config.AppSettings.Settings.Add("Filename", companystamp_tab.FileName);
                config.Save();

                var Stamp_tab = _CompanyStamp_tabManager.Find(companystamp_tab.FileNumber);

                var path = "";
                if (Convert.ToString(Session["filepath"]) != string.Empty)
                {
                    path = Session["filepath"].ToString();
                }
                var oldpath = "";
                if (Convert.ToString(Session["capture"]) != string.Empty)
                {
                    oldpath = @" " + Session["capture"].ToString();
                }

                var newpath = @" " + folderfiles + "" + companystamp_tab.FileName + "_" + companystamp_tab.FileNumber + ".jpg";
                //System.IO.File.Create(newpath);

                if (oldpath != "")
                {
                    System.IO.File.Move(oldpath, newpath);
                }
                else
                {
                    System.IO.File.Move(path, newpath);
                }
                companystamp_tab.FilePath = newpath;


                db.CompanyStamp_tab.Add(companystamp_tab);
                db.SaveChanges();

                Session["capture"] = "";

                ModelState.Clear();
                TempData["FlashSuccess"] = "Saved Completed.";
                return RedirectToAction("Index");
            }

            return View(companystamp_tab);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CompanyStamp_tab companystamp_tab = db.CompanyStamp_tab.Find(id);
            if (companystamp_tab == null)
            {
                return HttpNotFound();
            }
            return View(companystamp_tab);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        
        public ActionResult Photo()
        {
            if (Convert.ToString(Session["val"]) != string.Empty)
            {
               // ViewBag.pic = "http://localhost:59406/WebImages/" + Session["val"].ToString();
               ViewBag.pic = @"C://temp/" + Session["val"].ToString();
                
            }
            else
            {
                ViewBag.pic = "../../WebImages/person.jpg";
            }
            return View();
        }


        public JsonResult Rebind()
        {
           string path = "http://localhost:59406/WebImages/" + Session["val"].ToString();
           // string path = @"C://temp/" + Session["val"].ToString();
            return Json(path, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Capture()
        {
            var stream = Request.InputStream;
            string dump;

            using (var reader = new StreamReader(stream))
            {
                dump = reader.ReadToEnd();

                DateTime nm = DateTime.Now;

                string date = nm.ToString("yyyymmddMMss");

                var path =  Server.MapPath("~/WebImages/" + date + ".jpg");
                var path2 =  @"C://temp/" +  date + ".jpg";

                System.IO.File.WriteAllBytes(path, String_To_Bytes2(dump));

                System.IO.File.WriteAllBytes(path2, String_To_Bytes2(dump));

                ViewData["path"] = date + ".jpg";

                Session["val"] = date + ".jpg";
            }

            return View("Index");
        }
        private byte[] String_To_Bytes2(string strInput)
        {
            int numBytes = (strInput.Length) / 2;

            byte[] bytes = new byte[numBytes];

            for (int x = 0; x < numBytes; ++x)
            {
                bytes[x] = Convert.ToByte(strInput.Substring(x * 2, 2), 16);
            }

            return bytes;
        }

    }
}