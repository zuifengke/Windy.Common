// ***********************************************************
// 封装一些基本的时间操作方法集合
// Creator:YangMingkun  Date:2009-6-22
// Copyright:supconhealth
// ***********************************************************
using System;
using System.Text;

namespace Windy.Common.Libraries
{
    /// <summary>
    /// 日期间隔计算类型
    /// </summary>
    public enum DateInterval
    {
        /// <summary>
        /// 以年为单位计算
        /// </summary>
        Year,
        /// <summary>
        /// 月为单位计算
        /// </summary>
        Month,
        /// <summary>
        /// 以天为单位计算
        /// </summary>
        Day,
        /// <summary>
        /// 以小时为单位计算
        /// </summary>
        Hour,
        /// <summary>
        /// 以分钟为单位计算
        /// </summary>
        Minute,
        /// <summary>
        /// 以秒为单位计算
        /// </summary>
        Second
    }

    public partial struct GlobalMethods
    {
        /// <summary>
        /// 封装日期时间操作方法
        /// </summary>
        public struct SysTime
        {
            /// <summary>
            /// 比较指定的两个日期的时间值和日期值是否相等,仅精确到秒
            /// </summary>
            /// <param name="datetime1">日期时间1</param>
            /// <param name="datetime2">日期时间2</param>
            /// <returns>bool</returns>
            public static bool CompareTime(DateTime datetime1, DateTime datetime2)
            {
                if (datetime1.Year != datetime2.Year)
                    return false;
                if (datetime1.Month != datetime2.Month)
                    return false;
                if (datetime1.Day != datetime2.Day)
                    return false;
                if (datetime1.Hour != datetime2.Hour)
                    return false;
                if (datetime1.Minute != datetime2.Minute)
                    return false;
                if (datetime1.Second != datetime2.Second)
                    return false;
                return true;
            }

            /// <summary>
            /// 得到中文汉字表示的时间值
            /// </summary>
            /// <param name="timeValue">时间值</param>
            /// <returns>汉字表示的时间值</returns>
            public static string GetChineseTime(int timeValue)
            {
                switch (timeValue)
                {
                    case 0: return "零";
                    case 1: return "一";
                    case 2: return "二";
                    case 3: return "三";
                    case 4: return "四";
                    case 5: return "五";
                    case 6: return "六";
                    case 7: return "七";
                    case 8: return "八";
                    case 9: return "九";
                    case 10: return "十";
                    case 11: return "十一";
                    case 12: return "十二";
                    case 13: return "十三";
                    case 14: return "十四";
                    case 15: return "十五";
                    case 16: return "十六";
                    case 17: return "十七";
                    case 18: return "十八";
                    case 19: return "十九";
                    case 20: return "二十";
                    case 21: return "二十一";
                    case 22: return "二十二";
                    case 23: return "二十三";
                    case 24: return "二十四";
                    case 25: return "二十五";
                    case 26: return "二十六";
                    case 27: return "二十七";
                    case 28: return "二十八";
                    case 29: return "二十九";
                    case 30: return "三十";
                    case 31: return "三十一";
                    case 32: return "三十二";
                    case 33: return "三十三";
                    case 34: return "三十四";
                    case 35: return "三十五";
                    case 36: return "三十六";
                    case 37: return "三十七";
                    case 38: return "三十八";
                    case 39: return "三十九";
                    case 40: return "四十";
                    case 41: return "四十一";
                    case 42: return "四十二";
                    case 43: return "四十三";
                    case 44: return "四十四";
                    case 45: return "四十五";
                    case 46: return "四十六";
                    case 47: return "四十七";
                    case 48: return "四十八";
                    case 49: return "四十九";
                    case 50: return "五十";
                    case 51: return "五十一";
                    case 52: return "五十二";
                    case 53: return "五十三";
                    case 54: return "五十四";
                    case 55: return "五十五";
                    case 56: return "五十六";
                    case 57: return "五十七";
                    case 58: return "五十八";
                    case 59: return "五十九";
                }
                return timeValue.ToString();
            }

            /// <summary>
            /// 获取指定的日期中23点59分59秒的时间值
            /// </summary>
            /// <param name="day">时间值</param>
            /// <returns>DateTime</returns>
            public static DateTime GetDayLastTime(DateTime day)
            {
                return new DateTime(day.Year, day.Month, day.Day, 23, 59, 59);
            }

            /// <summary>
            /// 获取格式化后的时间
            /// </summary>
            /// <param name="time">时间值</param>
            /// <param name="format">格式串</param>
            /// <param name="value">格式化后的时间字符串</param>
            /// <returns>string</returns>
            public static string GetFormatTime(DateTime time, string format)
            {
                if (GlobalMethods.Misc.IsEmptyString(format))
                    return string.Empty;
                try
                {
                    format = format.Replace(@"\", @"\\");
                    format = format.Replace(@"/", @"\/");
                    return time.ToString(format);
                }
                catch (Exception ex)
                {
                    LogManager.Instance.WriteLog("GlobalMethods.GetFormatTime", new string[] { "time", "format" }
                        , new object[] { time, format }, "无法按指定格式格式化时间!", ex);
                    return format;
                }
            }

            /// <summary>
            /// 计算指定的两个日期之间的时间差
            /// </summary>
            /// <param name="eInterval">时间差类型</param>
            /// <param name="dtBeginTime">起始时间</param>
            /// <param name="dtEndTime">截止时间</param>
            /// <returns>时间差</returns>
            public static long DateDiff(DateInterval eInterval, DateTime dtBeginTime, DateTime dtEndTime)
            {
                switch (eInterval)
                {
                    case DateInterval.Hour:
                        {
                            TimeSpan span = dtEndTime.Subtract(dtBeginTime);
                            return (long)Math.Round(GlobalMethods.Convert.Fix(span.TotalHours));
                        }
                    case DateInterval.Minute:
                        {
                            TimeSpan span = dtEndTime.Subtract(dtBeginTime);
                            return (long)Math.Round(GlobalMethods.Convert.Fix(span.TotalMinutes));
                        }
                    case DateInterval.Second:
                        {
                            TimeSpan span = dtEndTime.Subtract(dtBeginTime);
                            return (long)Math.Round(GlobalMethods.Convert.Fix(span.TotalSeconds));
                        }
                    case DateInterval.Year:
                        {
                            System.Globalization.Calendar currentCalendar = null;
                            currentCalendar = System.Threading.Thread.CurrentThread.CurrentCulture.Calendar;
                            return (long)(currentCalendar.GetYear(dtEndTime) - currentCalendar.GetYear(dtBeginTime));
                        }
                    case DateInterval.Month:
                        {
                            System.Globalization.Calendar currentCalendar = null;
                            currentCalendar = System.Threading.Thread.CurrentThread.CurrentCulture.Calendar;
                            return (long)((((currentCalendar.GetYear(dtEndTime) - currentCalendar.GetYear(dtBeginTime)) * 12)
                                + currentCalendar.GetMonth(dtEndTime)) - currentCalendar.GetMonth(dtBeginTime));
                        }
                    default:
                        {
                            TimeSpan span = dtEndTime.Subtract(dtBeginTime);
                            return (long)Math.Round(GlobalMethods.Convert.Fix(span.TotalDays));
                        }
                }
            }

            /// <summary>
            /// 计算指定的两个日期之间的较为确切的浮点数时间差
            /// </summary>
            /// <param name="eInterval">时间差类型</param>
            /// <param name="dtBeginTime">起始时间</param>
            /// <param name="dtEndTime">截止时间</param>
            /// <returns>时间差</returns>
            public static float DateDiffExact(DateInterval eInterval, DateTime dtBeginTime, DateTime dtEndTime)
            {
                switch (eInterval)
                {
                    case DateInterval.Hour:
                        {
                            TimeSpan span = dtEndTime.Subtract(dtBeginTime);
                            return (float)span.TotalHours;
                        }
                    case DateInterval.Minute:
                        {
                            TimeSpan span = dtEndTime.Subtract(dtBeginTime);
                            return (float)span.TotalMinutes;
                        }
                    case DateInterval.Second:
                        {
                            TimeSpan span = dtEndTime.Subtract(dtBeginTime);
                            return (float)span.TotalSeconds;
                        }
                    case DateInterval.Year:
                        {
                            //仅精确到小数天
                            System.Globalization.Calendar currentCalendar = null;
                            currentCalendar = System.Threading.Thread.CurrentThread.CurrentCulture.Calendar;
                            int years = currentCalendar.GetYear(dtEndTime) - currentCalendar.GetYear(dtBeginTime);
                            TimeSpan span = dtEndTime.Subtract(dtBeginTime.AddYears(years));
                            return ((float)years) + ((float)span.TotalDays / 365f);
                        }
                    case DateInterval.Month:
                        {
                            //仅精确到小数天
                            System.Globalization.Calendar currentCalendar = null;
                            currentCalendar = System.Threading.Thread.CurrentThread.CurrentCulture.Calendar;
                            int months = (currentCalendar.GetYear(dtEndTime) - currentCalendar.GetYear(dtBeginTime)) * 12
                                + (currentCalendar.GetMonth(dtEndTime) - currentCalendar.GetMonth(dtBeginTime));
                            TimeSpan span = dtEndTime.Subtract(dtBeginTime.AddMonths(months));
                            return ((float)months) + ((float)span.TotalDays / 30f);
                        }
                    default:
                        {
                            TimeSpan span = dtEndTime.Subtract(dtBeginTime);
                            return (float)span.TotalDays;
                        }
                }
            }

            /// <summary>
            /// 使用入院时间和出院时间计算在院天数
            /// </summary>
            /// <param name="dtAdmissionTime">入院时间</param>
            /// <param name="dtDischargeDate">出院时间</param>
            /// <returns>在院天数</returns>
            public static long GetInpDays(DateTime dtAdmissionDate, DateTime dtDischargeDate)
            {
                dtAdmissionDate = dtAdmissionDate.Date;
                dtDischargeDate = dtDischargeDate.Date;
                long lInpDays = GlobalMethods.SysTime.DateDiff(DateInterval.Day, dtAdmissionDate, dtDischargeDate);
                if (lInpDays < 1) lInpDays = 1;
                return lInpDays;
            }

            /// <summary>
            /// 从出生时间得到年龄显示文本
            /// </summary>
            /// <param name="dtBirthTime">出生时间</param>
            /// <returns>年龄显示文本</returns>
            public static string GetAgeText(DateTime dtBirthTime)
            {
                return GetAgeText(dtBirthTime, DateTime.Now);
            }

            /// <summary>
            /// 从出生时间得到年龄显示文本
            /// </summary>
            /// <param name="dtBirthTime">出生时间</param>
            /// <param name="dtNowTime">现在时间</param>
            /// <returns>年龄显示文本</returns>
            public static string GetAgeText(DateTime dtBirthTime, DateTime dtNowTime)
            {
                //出生日期不能大于截止时间
                int nDays = (int)DateDiff(DateInterval.Day, dtBirthTime, dtNowTime);
                if (nDays < 0)
                    return string.Empty;
                else if (nDays == 0)
                    return "0岁0月0天";

                int nYear = 0;
                int nMonth = 0;
                int nDay = 0;

                //计算出生的实足月份
                int nMonths = (int)DateDiff(DateInterval.Month, dtBirthTime, dtNowTime);

                if (dtNowTime.Day > dtBirthTime.Day)
                {
                    //截止时间大于出生日期，说明为实足月
                    nYear = (int)Math.Round(Convert.Fix(nMonths / 12));

                    //年龄小于6岁说明为小儿，需要知道详细年龄信息。否则，知道年数即可。
                    if (nYear >= 6)
                    {
                        return string.Format("{0}岁", nYear);
                    }
                    else
                    {
                        nMonth = nMonths % 12;
                        nDay = dtNowTime.Day - dtBirthTime.Day - 1;
                        return string.Format("{0}岁{1}月{2}天", nYear, nMonth, nDay);
                    }
                }
                else
                {
                    //截止时间不大于出生日期，说明为实足月比计算出的月数少1
                    nMonths -= 1;
                    nYear = (int)Math.Round(Convert.Fix(nMonths / 12));

                    //年龄小于6岁说明为小儿，需要知道详细年龄信息。否则，知道年数即可。
                    if (nYear >= 6)
                    {
                        return string.Format("{0}岁", nYear);
                    }
                    else
                    {
                        nMonth = nMonths % 12;
                        //上一个月的月末日期数减去出生日期数加上当前日期数即为实足月超出日期。如2001-03-15号出生至2007-02-09日，
                        //除去月份后的实足月超出日期为24天，从01-16开始算起。所以应为2月的上一月的月末（31）减去出生日期（15）
                        //加上当月日期(9)为25天，因为是实足相差日期，所以为24天。
                        DateTime dtPrevMonth = dtNowTime.AddMonths(-1);
                        nDay = DateTime.DaysInMonth(dtPrevMonth.Year, dtPrevMonth.Month) - dtBirthTime.Day + dtNowTime.Day - 1;
                        return string.Format("{0}岁{1}月{2}天", nYear, nMonth, nDay);
                    }
                }
            }
        }
    }
}
