using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WbAppMVC.Classes
{
    public class DateTimeDependsOnTimeZone
    {
        public static DateTime GetDate()
        {
            DateTime _DateTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("Arab Standard Time"));

            return _DateTime;
        }
        public static DateTime GetDateMin()
        {
            DateTime _DateTime = TimeZoneInfo.ConvertTime(new DateTime(2000,1,1), TimeZoneInfo.FindSystemTimeZoneById("Arab Standard Time"));

            return _DateTime;
        }
        public static DateTime GetDateMax()
        {
            DateTime _DateTime = TimeZoneInfo.ConvertTime(new DateTime(2200, 1, 1), TimeZoneInfo.FindSystemTimeZoneById("Arab Standard Time"));

            return _DateTime;
        }
    }
}