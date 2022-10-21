//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.Entity;
//using System.Linq;
//using System.Net;
//using System.Web;
//using System.Web.Mvc;
//using WbAppMVC.Classes;
//using WbAppMVC.Models;
//namespace WbAppMVC.Controllers
//{
//    class HallSearch
//    {
//        public int HallID { get; set; }
//        public string HallCode { get; set; }
//        public string BranchName { get; set; }

//    }
//    public class HallsController : Controller
//    {
//        private SmartEntities db = new SmartEntities();

//        // GET: /Halls/
//        public ActionResult Index()
//        {
//            if (Session["UserID"] == null || Session["BarnchID"] == null)
//            {
//                return RedirectToAction("IFUserSessionTimeOut", "Home", new { PURl = "/Halls/Index" });

//            }
//            var tbl_hall = db.Tbl_Hall.Include(t => t.Tbl_Branch);
//            return View(tbl_hall.Where(x => x.IS_Deleted == false).ToList());
//        }

//        public ActionResult LoadDataTable()
//        {
//            JsonResult result = new JsonResult();
//            try
//            {
//                string search = Request.Form.GetValues("search[value]")[0];
//                string draw = Request.Form.GetValues("draw")[0];
//                string order = Request.Form.GetValues("order[0][column]")[0];
//                string orderDir = Request.Form.GetValues("order[0][dir]")[0];
//                int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
//                int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);
//                int recordsTotal = 0;
//                string BRID = Session["BarnchID"].ToString();
//                int BFRof = int.Parse(BRID);

//                var varData = db.Tbl_Hall.Where(x => x.IS_Deleted == false && (BFRof == 0 ? (true) : (x.BranchID == BFRof))).Select(x => new HallSearch { HallCode = x.HallCode, HallID = x.HallID, BranchName = x.Tbl_Branch.BranchName }).ToList().AsEnumerable();

//                if (!(string.IsNullOrEmpty(order) && string.IsNullOrEmpty(orderDir)))
//                {
//                    if (order == "0" && orderDir == "asc")
//                    {
//                        varData = varData.OrderBy(p => p.HallID);
//                    }
//                    else if (order == "0" && orderDir != "asc")
//                    {
//                        varData = varData.OrderByDescending(p => p.HallID);
//                    }
//                    if (order == "1" && orderDir == "asc")
//                    {
//                        varData = varData.OrderBy(p => p.HallCode);
//                    }
//                    else if (order == "1" && orderDir != "asc")
//                    {
//                        varData = varData.OrderByDescending(p => p.HallCode);
//                    }
//                }
//                //Search    
//                if (!string.IsNullOrEmpty(search))
//                {
//                    varData = varData.Where(m => m.HallCode.Contains(search) || m.BranchName.Contains(search) || m.HallID.ToString().Contains(search));
//                }
//                //total number of rows count     
//                recordsTotal = varData.Count();
//                //Paging  
//                var data = varData.Skip(startRec).Take(pageSize).ToList();
//                //Returning Json Data    
//                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
//            }
//            catch
//            {
//                return Json(data: "Fail", behavior: JsonRequestBehavior.AllowGet);
//            }

//        }
//        // GET: /Halls/Details/5
//        public ActionResult Details(int? id)
//        {
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            Tbl_Hall tbl_hall = db.Tbl_Hall.Find(id);
//            if (tbl_hall == null)
//            {
//                return HttpNotFound();
//            }
//            return View(tbl_hall);
//        }

//        // GET: /Halls/Create
//        public ActionResult Create()
//        {
//            if (Session["UserID"] == null || Session["BarnchID"] == null)
//            {
//                return RedirectToAction("IFUserSessionTimeOut", "Home", new { PURl = "/Halls/Create" });

//            }
//            ViewBag.BranchID = new SelectList(db.Tbl_Branch, "BranchID", "BranchName");

//            int BranchID = int.Parse(Session["BarnchID"].ToString());

//            ViewModels.HallVW hallVW = new ViewModels.HallVW
//            {
//                Branches = db.Tbl_Branch.Where(x => x.IsDeleted == false && (BranchID == 0 || x.BranchID == BranchID)).ToList(),
//                BranchID = BranchID
//            };
//            return View(hallVW);
//        }

//        // POST: /Halls/Create
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Create([Bind(Include = "HallID,HallCode,BranchID,CreatedAt,UpdatedAt")] Tbl_Hall tbl_hall)
//        {
//            if (ModelState.IsValid)
//            {
//                tbl_hall.CreatedAt = DateTimeDependsOnTimeZone.GetDate();
//                tbl_hall.UpdatedAt = DateTimeDependsOnTimeZone.GetDate();
//                tbl_hall.IS_Deleted = false;
//                db.Tbl_Hall.Add(tbl_hall);
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }

//            ViewBag.BranchID = new SelectList(db.Tbl_Branch, "BranchID", "BranchName", tbl_hall.BranchID);
//            return View(tbl_hall);
//        }

//        // GET: /Halls/Edit/5
//        public ActionResult Edit(int? id)
//        {
//            if (Session["UserID"] == null || Session["BarnchID"] == null)
//            {
//                return RedirectToAction("IFUserSessionTimeOut", "Home", new { PURl = "/Halls/Edit?id" + id });

//            }
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            Tbl_Hall tbl_hall = db.Tbl_Hall.Find(id);
//            if (tbl_hall == null)
//            {
//                return HttpNotFound();
//            }
//            ViewBag.BranchID = new SelectList(db.Tbl_Branch, "BranchID", "BranchName", tbl_hall.BranchID);

//            int BranchID = int.Parse(Session["BarnchID"].ToString());

//            ViewModels.HallVW hallVW = new ViewModels.HallVW
//            {
//                Branches = db.Tbl_Branch.Where(x => x.IsDeleted == false && (BranchID == 0 || x.BranchID == BranchID)).ToList(),
//                Hall = tbl_hall,
//                BranchID = BranchID
//            };
//            return View(hallVW);
//        }

//        // POST: /Halls/Edit/5
//        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Edit([Bind(Include = "HallID,HallCode,BranchID,CreatedAt,UpdatedAt")] Tbl_Hall tbl_hall)
//        {
//            if (ModelState.IsValid)
//            {
//                tbl_hall.UpdatedAt = DateTimeDependsOnTimeZone.GetDate();
//                tbl_hall.IS_Deleted = false;
//                db.Entry(tbl_hall).State = EntityState.Modified;
//                db.SaveChanges();
//                return RedirectToAction("Index");
//            }
//            ViewBag.BranchID = new SelectList(db.Tbl_Branch, "BranchID", "BranchName", tbl_hall.BranchID);
//            return View(tbl_hall);
//        }

//        // GET: /Halls/Delete/5
//        public ActionResult Delete(int? id)
//        {
//            if (Session["UserID"] == null || Session["BarnchID"] == null)
//            {
//                return RedirectToAction("IFUserSessionTimeOut", "Home", new { PURl = "/Halls/Delete?id" + id });

//            }
//            if (id == null)
//            {
//                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//            }
//            Tbl_Hall tbl_hall = db.Tbl_Hall.Find(id);
//            if (tbl_hall == null)
//            {
//                return HttpNotFound();
//            }
//            return View(tbl_hall);
//        }

//        // POST: /Halls/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public ActionResult DeleteConfirmed(int id)
//        {
//            Tbl_Hall tbl_hall = db.Tbl_Hall.Find(id);
//            tbl_hall.IS_Deleted = true;
//            //db.Tbl_Hall.Remove(tbl_hall);
//            db.SaveChanges();
//            return RedirectToAction("Index");
//        }

//        protected override void Dispose(bool disposing)
//        {
//            if (disposing)
//            {
//                db.Dispose();
//            }
//            base.Dispose(disposing);
//        }

//        public JsonResult Delete_Action(string ID)
//        {
//            int BID = int.Parse(ID);

//            Tbl_Hall Tbl_Hall = db.Tbl_Hall.Find(BID);
//            Tbl_Hall.IS_Deleted = true;

//            db.SaveChanges();
//            #region
//            //  var xs=  db.Tbl_Branch.Where(x => x.IsDeleted == false);

//            //  string data = "";
//            //  foreach (var x in xs)
//            //  {

//            //    data=data+"  <tr>"+"<td>"+ x.BranchID.ToString()+"</td>"+

//            //    "<td>"+x.BranchName.ToString()+"</td>" +"<td>"+x.Instagram.ToString()+"</td>"+
//            //     "<td> <input id='Delete_ID' type='hidden' value='' /> <a href='"+Url.Action("Edit","Branch",new {id=x.BranchID})+"class='btn btn-circle default blue-stripe'><i class='fa  fa-edit'> </i> تعديل  </a>"
//            //+"<button class='btn btn-circle default red-stripe abdo' data-toggle='confirmation' data-original-title='هل انت متاكد من حذف الفرع ?' onclick='Delete("+x.BranchID+");' title='' > <i class='fa fa-times'></i> حذف الفرع</button></td></tr> ";
//            //  }
//            #endregion
//            return Json("Deleted");
//        }

//    }
//}
