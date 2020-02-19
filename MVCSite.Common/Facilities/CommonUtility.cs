using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;       using System.Data.Common; 
using System.Text;
using System.Globalization;
using System.Web;

namespace MVCSite.Common
{

    /// <summary>
    /// 常用方法类
    /// </summary>
    public class CommonUtility
    {
        #region 取得应用程序URL,不带结尾的"/"
        public static string ApplicationUrl
        {
            get
            {
                Uri uri = HttpContext.Current.Request.Url;
                string applicationPath = ApplicationPath;
                string url = "http://" + uri.Host;
                if (uri.Port != 80)
                {
                    url += ":" + uri.Port.ToString();
                }
                return url + applicationPath;

            }
        }
        #endregion

        #region 取得虚拟目录
        static public string ApplicationPath
        {
            get
            {
                string applicationPath = HttpContext.Current.Request.ApplicationPath;
                if (applicationPath == "/")
                {
                    return string.Empty;
                }
                else
                {
                    return applicationPath;
                }
            }

        }
        #endregion

        #region 取得当前本地时间
        /// <summary>
        /// 取得当前本地时间
        /// 用于显示当前登录会员的本地时间
        /// </summary>
        /// <param name="utc">时区</param>
        /// <returns></returns>
        public static DateTime GetLocalTime(double utc)
        {
            return DateTime.Now.AddHours(utc - 8);
        }
        #endregion

        #region 取得指定的本地时间
        /// <summary>
        /// 取得指定的本地时间
        /// 用于显示指定的本地时间
        /// </summary>
        /// <param name="utc">时区</param>
        /// <param name="theDateTime">指定的日期时间</param>
        /// <returns></returns>
        public static DateTime GetLocalTime(double utc,DateTime theDateTime)
        {
            return theDateTime.AddHours(utc - 8);
        }
        #endregion

        #region 取得UTC描述
        /// <summary>
        /// 取得UTC描述
        /// </summary>
        /// <param name="utc">时区</param>
        /// <returns></returns>
        public static string GetUTCDescription(double utc)
        {
            if (utc == -12)
                return ("UTC-12");
            if (utc == -11)
                return ("UTC-11");
            if (utc == -10)
                return ("UTC-10");
            if (utc == -9.5)
                return ("UTC-9:30");
            if (utc == -9)
                return ("UTC-9");
            if (utc == -8)
                return ("UTC-8");
            if (utc == -7)
                return ("UTC-7");
            if (utc == -6)
                return ("UTC-6");
            if (utc == -5)
                return ("UTC-5");
            if (utc == -4)
                return ("UTC-4");
            if (utc == -3.5)
                return ("UTC-3:30");
            if (utc == -3)
                return ("UTC-3");
            if (utc == -2)
                return ("UTC-2");
            if (utc == -1)
                return ("UTC-1");
            if (utc == 0)
                return ("UTC");
            if (utc == 1)
                return ("UTC+1");
            if (utc == 2)
                return ("UTC+2");
            if (utc == 3)
                return ("UTC+3");
            if (utc == 3.5)
                return ("UTC+3:30");
            if (utc == 4)
                return ("UTC-4");
            if (utc == 4.5)
                return ("UTC+4:30");
            if (utc == 5)
                return ("UTC+5");
            if (utc == 5.5)
                return ("UTC+5:30");
            if (utc == 5.75)
                return ("UTC+5:45");
            if (utc == 6)
                return ("UTC+6");
            if (utc == 6.5)
                return ("UTC+6:30");
            if (utc == 7)
                return ("UTC+7");
            if (utc == 8)
                return ("UTC+8");
            if (utc == 9)
                return ("UTC+9");
            if (utc == 9.5)
                return ("UTC+9:30");
            if (utc == 10)
                return ("UTC+10");
            if (utc == 10.5)
                return ("UTC+10:30");
            if (utc == 11)
                return ("UTC+11");
            if (utc == 11.5)
                return ("UTC+11:3");
            if (utc == 12)
                return ("UTC+12");
            if (utc == 12.75)
                return ("UTC+12:45");
            if (utc == 13)
                return ("UTC+13");
            if (utc == 14)
                return ("UTC+14");

            return string.Empty;
        }
        #endregion

        #region 设置输入框样式
        /// <summary>
        /// 设置输入框样式
        /// </summary>
        /// <param name="obj">输入框</param>
        public static void SetInputBehavior(System.Web.UI.WebControls.WebControl obj)
        {
            obj.CssClass = "box";
            obj.Attributes.Add("onfocus", "this.className='line';");
            obj.Attributes.Add("onblur", "this.className='box';");
        }
        #endregion

        #region 将字符串中的反斜杆（\）转化为斜杠（/）
        /// <summary>
        /// 将字符串中的反斜杆（\）转化为斜杠（/）
        /// </summary>
        /// <param name="src">待转化的字符串</param>
        /// <returns>转换后的字符串</returns>
        public static string BackSlashToSlash(string src)
        {
            return src.Replace('\\', '/');
        } 
        #endregion



        #region 获取记录分页;
        /// <summary>
        /// 获取下一页的起始记录号;
        /// </summary>
        /// <param name="RecordCount">总记录数</param>
        /// <param name="StartAt">本页的起始记录</param>
        /// <param name="PageSize">页大小</param>
        /// <returns>下一页的起始记录号(如果没有下一页返回0)</returns>
        public int GetNextPage(int RecordCount, int StartAt, int PageSize)
        {
            int nextStartAt = StartAt + PageSize;

            return nextStartAt >=RecordCount?0:nextStartAt;
        }
        /// <summary>
        /// 得到下一页的页索引
        /// </summary>
        /// <param name="RecordCount">总记录数</param>
        /// <param name="StartAt">本页的起始记录</param>
        /// <param name="PageSize">页大小</param>
        /// <returns>得到下一页的页索引，如果没有返回0</returns>
        public int GetNextPageIndex(int RecordCount, int StartAt, int PageSize)
        {

            if (StartAt + PageSize - 1 >= RecordCount)
                return 0;
            else
            {
                int prevPageIndex = (StartAt - 1) / PageSize;
                return prevPageIndex + 2;
            }

        }
        /// <summary>
        /// 得到上一页的页索引
        /// </summary>
        /// <param name="RecordCount">总记录数</param>
        /// <param name="StartAt">本页的起始记录</param>
        /// <param name="PageSize">页大小</param>
        /// <returns>得到上一页的页索引，如果没有返回0</returns>
        public int GetPrevPageIndex(int RecordCount, int StartAt, int PageSize)
        {
            if (StartAt == 1 )
                return 0;
            else
            {
                int prevPageIndex = (StartAt - 1) / PageSize;
                return prevPageIndex;
            }
        }
        /// <summary>
        /// 获取上一页的起始记录号;
        /// </summary>
        /// <param name="RecordCount">总记录数</param>
        /// <param name="StartAt">本页的起始记录</param>
        /// <param name="PageSize">页大小</param>
        /// <returns>上一页起始记录号(如果没有上一页返回0)</returns>
        public int GetPrevPage(int RecordCount, int StartAt, int PageSize)
        {
            int preStartAt = StartAt - PageSize;
            return preStartAt <= 0?0:preStartAt;
        }


#endregion

        #region 根据生日返回星座
        public static string GetStar(DateTime birthday)
        {
            DateTime start, end;
            DateTime birth = new DateTime(birthday.Year, birthday.Month, birthday.Day);

            string temp = "未知";

            //白羊座 03/21 - 04/19 
            start = new DateTime(birthday.Year, 3, 21);
            end = new DateTime(birthday.Year, 4, 19);
            if (start.CompareTo(birth) <= 0 && end.CompareTo(birth) >= 0)
                return temp = "白羊座";

            //金牛座 04/20 - 05/20 
            start = new DateTime(birthday.Year, 4, 20);
            end = new DateTime(birthday.Year, 5, 20);
            if (start.CompareTo(birth) <= 0 && end.CompareTo(birth) >= 0)
                return temp = "金牛座";

            //双子座 05/21 - 06/21 
            start = new DateTime(birthday.Year, 5, 21);
            end = new DateTime(birthday.Year, 6, 21);
            if (start.CompareTo(birth) <= 0 && end.CompareTo(birth) >= 0)
                return temp = "双子座";

            //巨蟹座 06/22 - 07/22 
            start = new DateTime(birthday.Year, 6, 22);
            end = new DateTime(birthday.Year, 7, 22);
            if (start.CompareTo(birth) <= 0 && end.CompareTo(birth) >= 0)
                return temp = "巨蟹座";

            //狮子座 07/23 - 08/22 
            start = new DateTime(birthday.Year, 7, 23);
            end = new DateTime(birthday.Year, 8, 22);
            if (start.CompareTo(birth) <= 0 && end.CompareTo(birth) >= 0)
                return temp = "狮子座";

            //处女座 08/23 - 09/22 
            start = new DateTime(birthday.Year, 8, 23);
            end = new DateTime(birthday.Year, 9, 22);
            if (start.CompareTo(birth) <= 0 && end.CompareTo(birth) >= 0)
                return temp = "处女座";

            //天秤座 09/23 - 10/23 
            start = new DateTime(birthday.Year, 9, 23);
            end = new DateTime(birthday.Year, 10, 23);
            if (start.CompareTo(birth) <= 0 && end.CompareTo(birth) >= 0)
                return temp = "天秤座";

            //天蝎座 10/24 - 11/22 
            start = new DateTime(birthday.Year, 10, 24);
            end = new DateTime(birthday.Year, 11, 22);
            if (start.CompareTo(birth) <= 0 && end.CompareTo(birth) >= 0)
                return temp = "天蝎座";

            //射手座 11/23 - 12/21 
            start = new DateTime(birthday.Year, 11, 22);
            end = new DateTime(birthday.Year, 12, 21);
            if (start.CompareTo(birth) <= 0 && end.CompareTo(birth) >= 0)
                return temp = "射手座";

            //水瓶座 01/20 - 02/18 
            start = new DateTime(birthday.Year, 1, 20);
            end = new DateTime(birthday.Year, 2, 18);
            if (start.CompareTo(birth) <= 0 && end.CompareTo(birth) >= 0)
                return temp = "水瓶座";

            //双鱼座 02/19 - 03/20 
            start = new DateTime(birthday.Year, 2, 19);
            end = new DateTime(birthday.Year, 3, 20);
            if (start.CompareTo(birth) <= 0 && end.CompareTo(birth) >= 0)
                return temp = "双鱼座";

            //魔羯座 12/22 - 01/19 
            //start = new DateTime(birthday.Year, 12, 22);
            //end = new DateTime(birthday.Year, 1, 19);
            //if (start.CompareTo(birth) <= 0 || end.CompareTo(birth) <= 0)
           //     return temp = "摩羯座";
			temp = "摩羯座";

            return temp;
        }
        #endregion

        #region 根据星座返回起始日期整数值(=月份*100+日期)
        public static int[] GetDateFromConstellation(Constellation c)
        {
            int[] date = null;
            switch ( c )
            {
                case Constellation.Aries:
                    date = new int[] { 321, 419 };
                    break;
                case Constellation.Taurus:
                    date = new int[] { 420, 520 };
                    break;
                case Constellation.Gemini:
                    date = new int[] { 521, 621, };
                    break;
                case Constellation.Cancer:
                    date = new int[] { 622, 722, };
                    break;
                case Constellation.Leo:
                    date = new int[] { 723, 822, };
                    break;
                case Constellation.Virgo:
                    date = new int[] { 823, 922, };
                    break;
                case Constellation.Libra:
                    date = new int[] { 923, 1023, };
                    break;
                case Constellation.Scorpio:
                    date = new int[] { 1024, 1121, };
                    break;
                case Constellation.Sagittarius:
                    date = new int[] { 1122, 1221, };
                    break;
                case Constellation.Capricorn:
                    date = new int[] { 1222, 119, };
                    break;
                case Constellation.Aquarius:
                    date = new int[] { 120, 218, };
                    break;
                case Constellation.Pisces:
                    date = new int[] { 219, 320, };
                    break;
            }
            return date;
        }
        #endregion

        #region 根据枚举序号返回星座
        public static string GetConstellation(Constellation c)
        {
            string s = string.Empty;
            switch ( c )
            {
                case Constellation.Aries:
                    s = "白羊座";
                    break;
                case Constellation.Taurus:
                    s = "金牛座";
                    break;
                case Constellation.Gemini:
                    s = "双子座";
                    break;
                case Constellation.Cancer:
                    s = "巨蟹座";
                    break;
                case Constellation.Leo:
                    s = "狮子座";
                    break;
                case Constellation.Virgo:
                    s = "处女座";
                    break;
                case Constellation.Libra:
                    s = "天秤座";
                    break;
                case Constellation.Scorpio:
                    s = "天蝎座";
                    break;
                case Constellation.Sagittarius:
                    s = "射手座";
                    break;
                case Constellation.Capricorn:
                    s = "摩羯座";
                    break;
                case Constellation.Aquarius:
                    s = "水瓶座";
                    break;
                case Constellation.Pisces:
                    s = "双鱼座";
                    break;
            }
            return s;
        }
        #endregion

        #region 根据当前时间返回上一周起始时间
        public static DateTime[] GetLastWeekDateTime()
        {
            DateTime time = DateTime.Now.Date;
            DateTime begin = time.AddDays( -7 );
            return new DateTime[] { begin, time };
        }

        #endregion

        #region 根据当前时间返回上一周起始日期整数值(=月份*100+日期)
        public static int[] GetLastWeekDate()
        {
            DateTime time = DateTime.Now;
            DateTime begin = time.AddDays( -7 );
            DateTime end = time.AddDays( -1 );
            return new int[] {begin.Year * 10000 + begin.Month * 100 + begin.Day,end.Year * 10000 +  end.Month * 100 + end.Day };
        }
        #endregion

        


        #region 返回当天日期整数值(=月份*100+日期)
        public static int GetDate()
        {
            return GetDate(DateTime.Now);
        }
        public static int GetDate(DateTime dataTime)
        {
            return dataTime.Year * 10000 + dataTime.Month * 100 + dataTime.Day;
        }
        #endregion

        #region 返回输入时间的相对时间,返回的格式#年#个月#日#时#分前/后
        /// <summary>
        /// 返回输入时间的相对时间,返回的格式#年#个月#日#时#分前/后
        /// </summary>
        /// <param name="time">输入时间</param>
        /// <param name="isShort">True返回短格式,False返回长格式</param>
        /// <returns></returns>
        public static string GetRelative(DateTime time, bool isShort)
        {
            TimeSpan span = new TimeSpan(DateTime.Now.Ticks - time.Ticks);
            string str =  GetRelativeTime(span, isShort);
            if (str.Length == 0)
            {
                str = "一分钟内";
            }
            else
            {
                if (span.Ticks < 0)
                    str += "后";
                else
                    str += "前";
            }
            return str;
        }
        public static string GetRelative(DateTime time)
        {
            return GetRelative(time, false);
        }

        public static string GetRelativeTime(TimeSpan span, bool isShort)
        {
            StringBuilder str = new StringBuilder();
            int temp = 0;

            if (span.Days != 0)
            {
                temp = span.Days;
                if (!isShort)
                {
                    if (temp / 365 != 0)
                    {
                        str.Append(string.Format("{0}年", Math.Abs(temp / 365)));
                        temp %= 365;
                    }

                    if (temp / 31 != 0)
                    {
                        str.Append(string.Format("{0}个月", Math.Abs(temp / 31)));
                        temp %= 31;
                    }
                }
                if (temp != 0)
                {
                    str.Append(string.Format("{0}天", Math.Abs(temp)));
                    if (isShort)
                    {
                        return str.ToString();
                    }
                }
            }
            if (span.Hours != 0)
            {
                str.Append(string.Format("{0}小时", Math.Abs(span.Hours)));
                if (isShort)
                {
                    return str.ToString();
                }

            }
            if (span.Minutes != 0)
            {
                str.Append(string.Format("{0}分钟", Math.Abs(span.Minutes)));
                if (isShort)
                {

                    return str.ToString();
                }
            }           
            return str.ToString();
        }
        #endregion

        #region 取得本周在一年中的周数
        /// <summary>
        /// 取得本周在一年中的周数
        /// </summary>
        /// <returns></returns>
        public static int GetThisWeekNo()
        {
            CultureInfo myCI = new CultureInfo("en-US");
            Calendar myCal = myCI.Calendar;
            CalendarWeekRule myCWR = myCI.DateTimeFormat.CalendarWeekRule;
            return myCal.GetWeekOfYear(DateTime.Now, myCWR, DayOfWeek.Sunday);
        }
        #endregion

        #region 取得本周的第一天
        /// <summary>
        /// 取得本周的第一天
        /// </summary>
        /// <returns></returns>
        public static DateTime GetWeekFirstDay()
        {
            DateTime dt = DateTime.Now;
            while (dt.DayOfWeek != DayOfWeek.Sunday)
            {
                dt = dt.AddDays(-1);
            }
            return dt;
        }
        #endregion

        #region 将英文月份转换为数字月份

        /// <summary>
        /// 将英文月份转换为数字月份
        /// </summary>
        /// <param name="strMonth"></param>
        /// <returns></returns>
        public static string MonthStringToIntString(string strMonth)
        {
            string IntDay = "0";
            strMonth = strMonth.ToUpper();
            switch (strMonth)
            {
                case "JANUARY":
                    IntDay = "1";
                    break;
                case "FEBRUARY":
                    IntDay = "2";
                    break;
                case "MARCH":
                    IntDay = "3";
                    break;
                case "APRIL":
                    IntDay = "4";
                    break;
                case "MAY":
                    IntDay = "5";
                    break;
                case "JUNE":
                    IntDay = "6";
                    break;
                case "JULY":
                    IntDay = "7";
                    break;
                case "AUGUST":
                    IntDay = "8";
                    break;
                case "SEPTEMBER":
                    IntDay = "9";
                    break;
                case "OCTOBER":
                    IntDay = "10";
                    break;
                case "NOVEMBER":
                    IntDay = "11";
                    break;
                case "DECEMBER":
                    IntDay = "12";
                    break;
            }

            return IntDay;
        }

        #endregion

        #region 根据yyyy-mm-dd格式返回年月日
        public static DateProperty GetYearMonthDay(string strdate)
        {
            DateProperty dateProperty = new DateProperty();
            dateProperty.Year = 0;
            dateProperty.Month = 0;
            dateProperty.Day = 0;
            string[] arrdate = strdate.Split(new char[] { '-' }, StringSplitOptions.None);
            if (arrdate.Length > 0)
            {
                if (arrdate[0].Length > 0)
                {
                    dateProperty.Year = SafeConvert.ToInt32(arrdate[0]);
                    if (arrdate.Length > 1)
                    {
                        if (arrdate[1].Length > 0)
                            dateProperty.Month = SafeConvert.ToInt32(arrdate[1]);
                        if (arrdate.Length > 2)
                        {
                            if (arrdate[2].Length > 0)
                                dateProperty.Day = SafeConvert.ToInt32(arrdate[2]);
                        }
                    }
                }
            }
            return dateProperty;
        }
        #endregion


        #region 根据年月日返回 年-月-日格式字符串
        public static string GetDateString(int year, int month, int day)
        {
            string returnDate = string.Empty;
            if (year > 0)
            {
                returnDate += year.ToString();
                if (month > 0)
                {
                    returnDate += "-" + month.ToString();
                    if (day > 0)
                        returnDate += "-" + day.ToString();
                }
            }
            return returnDate;
        }
        #endregion
        public static DateTime GetFullDateTime(string strdate)
        {
            string[] arrdate = strdate.Split(new char[] { '-' }, StringSplitOptions.None);
            for (int i = 0; i < arrdate.Length; i++)
            {
                try
                {
                    Convert.ToInt32(arrdate[i]);
                }
                catch
                {
                    return Convert.ToDateTime(null);
                }
            }
            if (arrdate.Length == 1)
            {
                strdate += "-01-01";
                return Convert.ToDateTime(strdate);
            }
            else if (arrdate.Length == 2)
            {
                strdate += "-01";
                return Convert.ToDateTime(strdate);
            }
            else if (arrdate.Length == 3)
            {
                return Convert.ToDateTime(strdate);
            }
            else
            {
                return Convert.ToDateTime(null);
            }
        }
    	
    	/// <summary>
    	/// 判读对象是否有数据
    	/// </summary>
    	/// <param name="o">目前支持类型：DataSet，DataTable，String</param>
    	/// <returns></returns>
    	public static bool IsNullOrEmpty( object o )
    	{
    		try
    		{
    			if ( o == null )
    				return true;
    			if ( o is DataSet )
    			{
    				DataSet dataSet = o as DataSet;
					if ( dataSet != null && dataSet.Tables != null && dataSet.Tables.Count > 0 && dataSet.Tables [0] != null && dataSet.Tables [0].Rows != null && dataSet.Tables [0].Rows.Count > 0 )
    				{
    					return false;
    				}
    				return true;
    			}
    			if ( o is DataTable )
    			{
    				DataTable dataTable = o as DataTable;
    				if ( dataTable != null && dataTable.Rows != null && dataTable.Rows.Count > 0 )
    				{
    					return false;
    				}
    				return true;
    			}
    			/*
    			if ( o is ICollection )
    			{
    				if ( ( o as ICollection ).Count > 0 )
    					return false;
    				return true;
    			}
    			if ( o is IList )
    			{
					if ( ( o as IList ).Count > 0 )
						return false;
					return true;
				}
				*/
    			if ( o is string )
    			{
    				return string.IsNullOrEmpty( o as string );
    			}
    		}
    		catch
    		{
    			return true;
    		}
    		//如果是未知类型返回false
    		return false;
    	}

		public static int DBToInt( object obj )
		{
			return DBToInt ( obj, 0 );
		}

		public static int DBToInt( object obj, int def )
		{
			return obj == DBNull.Value ? def : Convert.ToInt32 ( obj );
		}

		public static string DBToString( object obj )
		{
			return DBToString( obj, string.Empty );
		}
    	
		public static string DBToString( object obj, string def )
		{
			return obj == DBNull.Value ? def : Convert.ToString ( obj );
		}

		public static bool DBToBool( object obj )
		{
			return DBToBool( obj, false );
		}
    	
		public static bool DBToBool( object obj, bool def )
		{
			return obj == DBNull.Value ? def : Convert.ToBoolean ( obj );
		}

		public static DateTime DBToDateTime( object obj )
		{
			return DBToDateTime( obj,  DateTime.Now );
		}
    	
		public static DateTime DBToDateTime( object obj, DateTime def )
		{
			return obj == DBNull.Value ? def : Convert.ToDateTime ( obj );
		}

        public static float DBToFloat(object obj)
        {
            return DBToFloat(obj, 0);
        }

        public static float DBToFloat(object obj, float def)
        {
            return obj == DBNull.Value ? def : Convert.ToSingle(obj);
        }
        public static object StrToDBBit(string str)
        {
            if (str == "1")
                return true;
            else
                return false;
        }
        public static object StrToDBStr(string str)
        {
            if (str.Length == 0)
                return DBNull.Value;
            else
                return str;
        }
        public static object StrToDBInt(string str)
        {
            try
            {
                int i = Convert.ToInt32(str);
                return i;
            }
            catch
            {
                return DBNull.Value;
            }
        }
    }
}
