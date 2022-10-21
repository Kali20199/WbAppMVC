
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;

using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Configuration;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;
using WbAppMVC.Models;
using System.Drawing;
using Image = System.Drawing.Image;
using System.Drawing.Imaging;
using Microsoft.SqlServer.Server;
using static System.Net.WebRequestMethods;
using GSF.IO;
using WbAppMVC.Models.MyModels;

namespace WbAppMVC.Controllers
{
    public class StudentController : Controller
    {



        private smartEntities db = new smartEntities();

        [HttpPost]
        public ActionResult DataQuery(){
            db.Configuration.ProxyCreationEnabled = false;
            var Currentdate = DateTime.Now.ToString("yyyy/MM/dd").Replace('/','-');
           
            var Date = Request.Form.GetValues("date")[0];
            var Order = Request.Form.GetValues("order")[0];
            var filterBy = Request.Form.GetValues("filterBy")[0];
            var UserId = Request.Form.GetValues("UserId")[0];
            var Signiture = Request.Form.GetValues("canvas")[0].Split(',')[1];
            if (Signiture != "" && UserId!="") {
              
                var img =   Convert.FromBase64String(Signiture);
                String path = System .Web.HttpContext.Current.Server.MapPath("~/Content/Image/"+UserId+".png");
                string imgPath = Path.Combine(path, "New");

                System.IO.File.WriteAllBytes(path, img);
    
            }
            double days = 19000;
            if (Date != "")
            {
               
                days = (Convert.ToDateTime(Currentdate) - Convert.ToDateTime(Date)).TotalDays;
            }
            days /= 365;    
            var years = (int)days;

            List<student> students;

            Expression<Func<student,string>> myExpression = (St) => St.firstname;
            if (filterBy == "SecondName")
            {
                myExpression = (St) => St.secondname;
            }
            else if (filterBy == "ThirdName")
            {
                myExpression = (St) => St.thirdname;
            }
            else if (filterBy == "Age")
            {
                myExpression = (St) => St.age.ToString();
            }
            else if(filterBy == "Email"){
                myExpression = (St) => St.email;
            }
            else if (filterBy == "Wallet")
            {
                myExpression = (St) => St.wallet.ToString();
            }






            if (Order == "Dec")
            {
                 students = db.students.Where(x => x.age <= years).OrderByDescending(myExpression).ToList();
            }
            else {

                 students = db.students.Where(x => x.age <= years).OrderBy(myExpression).ToList();
            }
          

            var Data = Json(students);
            //JsonResult categoryJson = new JsonResult();
            //categoryJson.Data = students;
            return Json(students);
        }



   


        public ActionResult DeleteStudent(string id) {

          var student =  db.students.FirstOrDefault(x => x.Id == id);
            if (student == null)
                return RedirectToAction("Index", "Home");
            db.students.Remove(student);
            db.SaveChanges();

            return RedirectToAction("Index", "Home");
        }


        public static Image Base64ToImage(string base64String)
        {

            byte[] imageBytes = Convert.FromBase64String(base64String);
            Image image;
            using (MemoryStream inStream = new MemoryStream())
            {
                inStream.Write(imageBytes, 0, imageBytes.Length);

                image = Bitmap.FromStream(inStream);
                // image.Save(inStream, System.Drawing.Imaging.ImageFormat.Png);
               // image.Save(System.Web.HttpServerUtility Server.MapPath("~/Uploads/"+fileName),
                image.Save(inStream, ImageFormat.Png);
            }

            return image;
        }
    }
}