// ***********************************************************
// ��װһЩ������ʱ�������������
// Creator:YangMingkun  Date:2009-6-22
// Copyright:supconhealth
// ***********************************************************
using System;
using System.Text;

namespace Windy.Common.Libraries
{
    /// <summary>
    /// ���ڼ����������
    /// </summary>
    public enum DateInterval
    {
        /// <summary>
        /// ����Ϊ��λ����
        /// </summary>
        Year,
        /// <summary>
        /// ��Ϊ��λ����
        /// </summary>
        Month,
        /// <summary>
        /// ����Ϊ��λ����
        /// </summary>
        Day,
        /// <summary>
        /// ��СʱΪ��λ����
        /// </summary>
        Hour,
        /// <summary>
        /// �Է���Ϊ��λ����
        /// </summary>
        Minute,
        /// <summary>
        /// ����Ϊ��λ����
        /// </summary>
        Second
    }

    public partial struct GlobalMethods
    {
        /// <summary>
        /// ��װ����ʱ���������
        /// </summary>
        public struct SysTime
        {
            /// <summary>
            /// �Ƚ�ָ�����������ڵ�ʱ��ֵ������ֵ�Ƿ����,����ȷ����
            /// </summary>
            /// <param name="datetime1">����ʱ��1</param>
            /// <param name="datetime2">����ʱ��2</param>
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
            /// �õ����ĺ��ֱ�ʾ��ʱ��ֵ
            /// </summary>
            /// <param name="timeValue">ʱ��ֵ</param>
            /// <returns>���ֱ�ʾ��ʱ��ֵ</returns>
            public static string GetChineseTime(int timeValue)
            {
                switch (timeValue)
                {
                    case 0: return "��";
                    case 1: return "һ";
                    case 2: return "��";
                    case 3: return "��";
                    case 4: return "��";
                    case 5: return "��";
                    case 6: return "��";
                    case 7: return "��";
                    case 8: return "��";
                    case 9: return "��";
                    case 10: return "ʮ";
                    case 11: return "ʮһ";
                    case 12: return "ʮ��";
                    case 13: return "ʮ��";
                    case 14: return "ʮ��";
                    case 15: return "ʮ��";
                    case 16: return "ʮ��";
                    case 17: return "ʮ��";
                    case 18: return "ʮ��";
                    case 19: return "ʮ��";
                    case 20: return "��ʮ";
                    case 21: return "��ʮһ";
                    case 22: return "��ʮ��";
                    case 23: return "��ʮ��";
                    case 24: return "��ʮ��";
                    case 25: return "��ʮ��";
                    case 26: return "��ʮ��";
                    case 27: return "��ʮ��";
                    case 28: return "��ʮ��";
                    case 29: return "��ʮ��";
                    case 30: return "��ʮ";
                    case 31: return "��ʮһ";
                    case 32: return "��ʮ��";
                    case 33: return "��ʮ��";
                    case 34: return "��ʮ��";
                    case 35: return "��ʮ��";
                    case 36: return "��ʮ��";
                    case 37: return "��ʮ��";
                    case 38: return "��ʮ��";
                    case 39: return "��ʮ��";
                    case 40: return "��ʮ";
                    case 41: return "��ʮһ";
                    case 42: return "��ʮ��";
                    case 43: return "��ʮ��";
                    case 44: return "��ʮ��";
                    case 45: return "��ʮ��";
                    case 46: return "��ʮ��";
                    case 47: return "��ʮ��";
                    case 48: return "��ʮ��";
                    case 49: return "��ʮ��";
                    case 50: return "��ʮ";
                    case 51: return "��ʮһ";
                    case 52: return "��ʮ��";
                    case 53: return "��ʮ��";
                    case 54: return "��ʮ��";
                    case 55: return "��ʮ��";
                    case 56: return "��ʮ��";
                    case 57: return "��ʮ��";
                    case 58: return "��ʮ��";
                    case 59: return "��ʮ��";
                }
                return timeValue.ToString();
            }

            /// <summary>
            /// ��ȡָ����������23��59��59���ʱ��ֵ
            /// </summary>
            /// <param name="day">ʱ��ֵ</param>
            /// <returns>DateTime</returns>
            public static DateTime GetDayLastTime(DateTime day)
            {
                return new DateTime(day.Year, day.Month, day.Day, 23, 59, 59);
            }

            /// <summary>
            /// ��ȡ��ʽ�����ʱ��
            /// </summary>
            /// <param name="time">ʱ��ֵ</param>
            /// <param name="format">��ʽ��</param>
            /// <param name="value">��ʽ�����ʱ���ַ���</param>
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
                        , new object[] { time, format }, "�޷���ָ����ʽ��ʽ��ʱ��!", ex);
                    return format;
                }
            }

            /// <summary>
            /// ����ָ������������֮���ʱ���
            /// </summary>
            /// <param name="eInterval">ʱ�������</param>
            /// <param name="dtBeginTime">��ʼʱ��</param>
            /// <param name="dtEndTime">��ֹʱ��</param>
            /// <returns>ʱ���</returns>
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
            /// ����ָ������������֮��Ľ�Ϊȷ�еĸ�����ʱ���
            /// </summary>
            /// <param name="eInterval">ʱ�������</param>
            /// <param name="dtBeginTime">��ʼʱ��</param>
            /// <param name="dtEndTime">��ֹʱ��</param>
            /// <returns>ʱ���</returns>
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
                            //����ȷ��С����
                            System.Globalization.Calendar currentCalendar = null;
                            currentCalendar = System.Threading.Thread.CurrentThread.CurrentCulture.Calendar;
                            int years = currentCalendar.GetYear(dtEndTime) - currentCalendar.GetYear(dtBeginTime);
                            TimeSpan span = dtEndTime.Subtract(dtBeginTime.AddYears(years));
                            return ((float)years) + ((float)span.TotalDays / 365f);
                        }
                    case DateInterval.Month:
                        {
                            //����ȷ��С����
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
            /// ʹ����Ժʱ��ͳ�Ժʱ�������Ժ����
            /// </summary>
            /// <param name="dtAdmissionTime">��Ժʱ��</param>
            /// <param name="dtDischargeDate">��Ժʱ��</param>
            /// <returns>��Ժ����</returns>
            public static long GetInpDays(DateTime dtAdmissionDate, DateTime dtDischargeDate)
            {
                dtAdmissionDate = dtAdmissionDate.Date;
                dtDischargeDate = dtDischargeDate.Date;
                long lInpDays = GlobalMethods.SysTime.DateDiff(DateInterval.Day, dtAdmissionDate, dtDischargeDate);
                if (lInpDays < 1) lInpDays = 1;
                return lInpDays;
            }

            /// <summary>
            /// �ӳ���ʱ��õ�������ʾ�ı�
            /// </summary>
            /// <param name="dtBirthTime">����ʱ��</param>
            /// <returns>������ʾ�ı�</returns>
            public static string GetAgeText(DateTime dtBirthTime)
            {
                return GetAgeText(dtBirthTime, DateTime.Now);
            }

            /// <summary>
            /// �ӳ���ʱ��õ�������ʾ�ı�
            /// </summary>
            /// <param name="dtBirthTime">����ʱ��</param>
            /// <param name="dtNowTime">����ʱ��</param>
            /// <returns>������ʾ�ı�</returns>
            public static string GetAgeText(DateTime dtBirthTime, DateTime dtNowTime)
            {
                //�������ڲ��ܴ��ڽ�ֹʱ��
                int nDays = (int)DateDiff(DateInterval.Day, dtBirthTime, dtNowTime);
                if (nDays < 0)
                    return string.Empty;
                else if (nDays == 0)
                    return "0��0��0��";

                int nYear = 0;
                int nMonth = 0;
                int nDay = 0;

                //���������ʵ���·�
                int nMonths = (int)DateDiff(DateInterval.Month, dtBirthTime, dtNowTime);

                if (dtNowTime.Day > dtBirthTime.Day)
                {
                    //��ֹʱ����ڳ������ڣ�˵��Ϊʵ����
                    nYear = (int)Math.Round(Convert.Fix(nMonths / 12));

                    //����С��6��˵��ΪС������Ҫ֪����ϸ������Ϣ������֪���������ɡ�
                    if (nYear >= 6)
                    {
                        return string.Format("{0}��", nYear);
                    }
                    else
                    {
                        nMonth = nMonths % 12;
                        nDay = dtNowTime.Day - dtBirthTime.Day - 1;
                        return string.Format("{0}��{1}��{2}��", nYear, nMonth, nDay);
                    }
                }
                else
                {
                    //��ֹʱ�䲻���ڳ������ڣ�˵��Ϊʵ���±ȼ������������1
                    nMonths -= 1;
                    nYear = (int)Math.Round(Convert.Fix(nMonths / 12));

                    //����С��6��˵��ΪС������Ҫ֪����ϸ������Ϣ������֪���������ɡ�
                    if (nYear >= 6)
                    {
                        return string.Format("{0}��", nYear);
                    }
                    else
                    {
                        nMonth = nMonths % 12;
                        //��һ���µ���ĩ��������ȥ�������������ϵ�ǰ��������Ϊʵ���³������ڡ���2001-03-15�ų�����2007-02-09�գ�
                        //��ȥ�·ݺ��ʵ���³�������Ϊ24�죬��01-16��ʼ��������ӦΪ2�µ���һ�µ���ĩ��31����ȥ�������ڣ�15��
                        //���ϵ�������(9)Ϊ25�죬��Ϊ��ʵ��������ڣ�����Ϊ24�졣
                        DateTime dtPrevMonth = dtNowTime.AddMonths(-1);
                        nDay = DateTime.DaysInMonth(dtPrevMonth.Year, dtPrevMonth.Month) - dtBirthTime.Day + dtNowTime.Day - 1;
                        return string.Format("{0}��{1}��{2}��", nYear, nMonth, nDay);
                    }
                }
            }
        }
    }
}
