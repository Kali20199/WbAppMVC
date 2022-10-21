using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WbAppMVC.ViewModels
{
    public class HallViewModels
    {
        


        public int HallID { get; set; }
        public string HallCode { get; set; }
        public Nullable<int> BranchID { get; set; }
        public Nullable<System.DateTime> CreatedAt { get; set; }
        public Nullable<System.DateTime> UpdatedAt { get; set; }
        public Nullable<bool> IS_Deleted { get; set; }
        public string BranchName { get; set; }


    }
}