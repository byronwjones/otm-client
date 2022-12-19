using System;

namespace BWJ.Web.OTM.Models
{
    public class DateTimeSearchRange : SearchRange<DateTime>
    {
        public DateTimeSearchRange(DateTime min) : base(min) { }
        public DateTimeSearchRange(DateTime min, DateTime max) : base(min, max) { }

        protected override bool IsRangeValid() => Min <= Max;
    }
}
