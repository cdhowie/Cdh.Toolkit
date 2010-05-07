using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cdh.Toolkit.Cron
{
    public class TraditionalJob : IJob
    {
        #region IJob Members

        public JobLockType LockType { get; set; }

        public RangeCollection MinuteRange { get; private set; }
        public RangeCollection HourRange { get; private set; }
        public RangeCollection DateRange { get; private set; }
        public RangeCollection MonthRange { get; private set; }
        public RangeCollection DayOfWeekRange { get; private set; }

        private Action jobDelegate;

        private TraditionalJob(Action jobDelegate)
        {
            if (jobDelegate == null)
                throw new ArgumentNullException("jobDelegate");

            this.jobDelegate = jobDelegate;

            MinuteRange = new RangeCollection(0, 59);
            HourRange = new RangeCollection(0, 23);
            DateRange = new RangeCollection(1, 31);
            MonthRange = new RangeCollection(1, 12);
            DayOfWeekRange = new RangeCollection(0, 6);
        }

        public TraditionalJob(Action jobDelegate,
            string minuteSpecifier, string hourSpecifier, string dateSpecifier,
            string monthSpecifier, string dayOfWeekSpecifier)
            : this(jobDelegate)
        {
            AddRange(MinuteRange, minuteSpecifier, "minute");
            AddRange(HourRange, hourSpecifier, "hour");
            AddRange(DateRange, dateSpecifier, "date");
            AddRange(MonthRange, monthSpecifier, "month");
            AddRange(DayOfWeekRange, dayOfWeekSpecifier, "day of week");
        }

        private void AddRange(RangeCollection collection, string specifier, string name)
        {
            try
            {
                collection.AddRangeSpecifier(specifier);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Unable to process " + name + " specifier: " + ex.Message, ex);
            }
        }

        public void Run()
        {
            if (LockType == JobLockType.IgnoreIfRunning)
            {
                lock (this) jobDelegate();
            }
            else
            {
                jobDelegate();
            }
        }

        public bool ShouldRunAt(DateTime time)
        {
            return MinuteRange.Contains(time.Minute) &&
                HourRange.Contains(time.Hour) &&
                DateRange.Contains(time.Day) &&
                MonthRange.Contains(time.Month) &&
                DayOfWeekRange.Contains((int)time.DayOfWeek);
        }

        public DateTime? CalculateNextRun()
        {
            return null;
        }

        public bool ShouldRunAgain
        {
            get { return true; }
        }

        public IAsyncResult BeginRun(AsyncCallback callback, object state)
        {
            return jobDelegate.BeginInvoke(callback, state);
        }

        public void EndRun(IAsyncResult result)
        {
            jobDelegate.EndInvoke(result);
        }

        #endregion
    }
}
