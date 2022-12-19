using System;

namespace BWJ.Web.OTM.Models
{
    public abstract class SearchRange<T>
    {
        internal SearchRange(T min)
        {
            Min = min;
            Max = min;
        }

        internal SearchRange(T min, T max)
        {
            Min = min;
            Max = max;

            if(!IsRangeValid())
            {
                throw new ArgumentException("Invalid range values");
            }
        }

        public T Min { get; }
        public T Max { get; }

        protected abstract bool IsRangeValid();
    }
}
