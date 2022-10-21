using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using WbAppMVC.Models;

namespace WbAppMVC.Controllers.Api
{
    [Route("api/[controller]")]
   
    public class StudentQueryController : Controller
    {
        // GET: StudentQuery
        private smartEntities db = new smartEntities();

        // Controller
      
        public ActionResult NewDataQuery()
        {
            var pageSize = int.Parse(Request.Form["length"]);
            var skip = int.Parse(Request.Form["start"]);
            var searchValue = Request.Form["search[value]"];

        //    var skip = 0;
           
            int isNumber = 1000;
          
            if (int.TryParse(searchValue, out isNumber))
            {
                isNumber = Int32.Parse(searchValue);
            }
        
            Expression<Func<student, object>> selectable = (x) => new {x.Id,x.firstname,x.secondname,x.thirdname,x.email
            ,x.age,x.wallet} ;
            Expression<Func<student, bool>> filter = (x) =>
                x.firstname.Contains(searchValue) ||
                x.secondname.Contains(searchValue) ||
                x.thirdname.Contains(searchValue) ||
                x.age < isNumber;
            
            
    




            var sortColumn = Request.Form[string.Concat("columns[", Request.Form["order[0][column]"], "][name]")];
            var sortColumnDirection = Request.Form["order[0][dir]"];
            var students = db.students.Where(filter).Select(selectable).ToList();

            
            var data = students.Skip(skip).Take(pageSize);
            var recordsTotal = students.Count();
        
            var jsonData = new { recordsFiltered = recordsTotal, recordsTotal, data };
            var Data = Json(jsonData,JsonRequestBehavior.AllowGet);
            return Data;
        }
    }
}