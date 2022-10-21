using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WbAppMVC.Classes;
using WbAppMVC.Models;
using WbAppMVC.Models.MyModels;
using WbAppMVC.ViewModels;

namespace WbAppMVC.Controllers
{
    class HallSearch
    {
        public int HallID { get; set; }
        public string HallCode { get; set; }
        public string BranchName { get; set; }

    }
    public class HomeController : Controller
    {
        private smartEntities db = new smartEntities();
        public ActionResult Index()
        {

            var Users = db.Users.ToList();
            var students = db.students.ToList();
            return View(students);
        }

        public ActionResult Detail(string Id)
        {
            //    ViewBag.Message = "Your application description page.";

            var st = db.students.Where(x => x.Id == Id).FirstOrDefault();
            return View(st);
        }

        public ActionResult Delete()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


        public ActionResult Create()
        {


            var student = new student { };
            return View(student);
        }

        [HttpPost]
        public ActionResult CreateStudent(student student)
        {



            var ID = Guid.NewGuid();
            student.Id = ID.ToString();
            db.students.Add(student);
            db.SaveChanges();

            return RedirectToAction("Index", "Home");
        }



        public ActionResult Parents(string id)
        {

            var parents = db.students.Where(x => x.Id == id).Select(x => x.parents).ToList().FirstOrDefault();
            //   ViewData["parents"] = parents;

            return View("Parents/index", parents);
        }


        public ActionResult CreateParent()
        {
            var student = db.students.ToList();
            ViewData["Students"] = student;
            return View("Parents/Create");
        }



        [HttpPost]
        public ActionResult CreateParent(parent parent)
        {
            var ID = Guid.NewGuid();
            parent.Id = ID.ToString();
            var studentId = Request.Form.GetValues("ChildId")[0];

            var student = db.students.Where(x => x.Id == studentId).FirstOrDefault();
            if (student.secondname != parent.firstname)
            {
                student.secondname = parent.firstname;
            }
            if (student.thirdname != parent.secondname)
            {
                student.thirdname = parent.secondname;
            }
            student.parents.Add(parent);
            db.students.AddOrUpdate(student);
            db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }



        public ActionResult CreateSubject()
        {


            return View("Subject/Create");
        }


        public ActionResult SubjectIndex(string id)
        {
            var AllSubjects = db.subjects.ToList();

            var Registerdsubject = db.subjects.Where(x=>x.studentId == id).Select(x=>x.subjectname).ToList();



            List<string> SubjRemain = new List<string>();
            foreach (var subj in AllSubjects) {

                if (!Registerdsubject.Contains(subj.subjectname)) {

                    SubjRemain.Add(subj.subjectname);
                }
            
            };

            ViewData["Subjects"] = SubjRemain.Distinct().ToList();
            ViewData["StudentId"] = id;
           return View("Subject/Index", Registerdsubject);
        }


        public ActionResult StudentSubject(string subjName,string StudentId)
        {
            var ID = Guid.NewGuid();
          
          
            db.students.FirstOrDefault(x => x.Id == StudentId).subjects.Add(new subject { subjectname = subjName ,Id = ID.ToString()});
            db.SaveChanges();



            return RedirectToAction("Index","Home");
        }
    }
}

