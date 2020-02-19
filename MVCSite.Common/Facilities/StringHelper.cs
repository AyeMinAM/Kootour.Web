using System;
using System.Collections;
using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Web;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace MVCSite.Common
{
    /// <summary>
    /// 
    /// </summary>
    public struct HtmlUnescapeConstants
    {
        public const string HtmlBlankSpace = "&nbsp;";
        public const string HtmlBlankSpace2 = "&amp;nbsp;";
        public const string HtmlQuot = "&quot;";
        public const string HtmlQuot2 = "&amp;quot;";

        public const string HtmlCarriageReturn = "\r";
        public const string HtmlNewLine = "\n";

        public const string HtmlStartTag = "&lt;";
        public const string HtmlStartTag2 = "&amp;lt;";
        public const string HtmlEndTag = "&gt;";
        public const string HtmlEndTag2 = "&amp;gt;";
    
    
    }

    /// <summary>
    /// 字符串处理类
    /// </summary>
    public class StringHelper
    {
        public static string[] ChineseNumberArray ={ "", "一", "二", "三", "四", "五", "六", "七", "八", "九", "十" };
		private static Randoms random = new Randoms ();
        public static string RemoveLineEndingChars(string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;
            var result = input.Replace(".", "").Replace("。", "").Replace("?", "").Replace("？", "").TrimEnd();
            return result;
        }
        public static string RemoveBrackets(string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;
            Regex regex = new Regex(@"[\[\]\{\}]+", RegexOptions.IgnoreCase);
            var result =regex.Replace(input,"");
            return result;
        }

        public static string XorDigitsString(string _source)
        {
            if (string.IsNullOrEmpty(_source))
                return string.Empty;
            string key = "@V#!B#%H^O&*9!u%l#u$n&x*8A$6@s!p&i%k$e!R~Q@9#U%E^g&y(a)X-I&N^T$y@z!p@n#3$S%J^7$F!S%z!m$w#S^l&e*w(T#W)I$3@";
            string result = string.Empty;
            for (int i = 0, count = _source.Length; i < count; i++)
            {
                if (i > (key.Length - 1))
                {
                    key += key;
                }
                result += (_source[i] ^ key[i]).ToString().PadLeft(3, '0');
            }
            return result;
        }
        public static string ReverseXorDigitsString(string _source)
        {
            if (string.IsNullOrEmpty(_source) || _source.Length % 3 != 0)
                return string.Empty;
            string key = "@V#!B#%H^O&*9!u%l#u$n&x*8A$6@s!p&i%k$e!R~Q@9#U%E^g&y(a)X-I&N^T$y@z!p@n#3$S%J^7$F!S%z!m$w#S^l&e*w(T#W)I$3@";
            string result = string.Empty;
            for (int i = 0, count = _source.Length; i < count; i = i + 3)
            {
                if (i/3 > (key.Length - 1))
                {
                    key += key;
                }
                result += Convert.ToChar(Convert.ToChar(Convert.ToInt32(_source.Substring(i, 3))) ^ key[i / 3]);
            }
            return result;
        }

        public static string GetStartNWithoutParentheses(string _source, int _startN)
        {
            if (string.IsNullOrEmpty(_source) || _source.Length <= _startN)
                return _source;
            int ptRightIndex = _source.IndexOfAny(new char[] { '}', ']', ')'/*, '｝', '】', '］', '）'*/ });
            int ptLeftIndex = _source.IndexOfAny(new char[] { '{', '[', '(', /*'｛', '【', '［', '（'*/ });
            if (ptLeftIndex > 0 && ptRightIndex > 0 && (_startN - 2) > ptLeftIndex && (_startN - 2) <= ptRightIndex)
                return _source.Substring(0, ptLeftIndex) + "..";
            else
                return _source.Substring(0, _startN - 2) + "..";
        }

        public static string CleanEndLineTags(string _source)
        {
            if (string.IsNullOrEmpty(_source))
                return string.Empty;
            string result = _source.Replace("\n", "");
            result = result.Replace("\r", "").Trim();
            return result;

        }

        //Word special number:
        //⒈⒉⒊⒋⒌⒍⒎⒏⒐⒑⒒⒓⒔⒕⒖⒗⒘⒙⒚⒛⑴⑵⑶⑷⑸⑹⑺⑻⑼⑽⑾⑿⒀⒁⒂⒃⒄⒅⒆⒇①②③④⑤⑥⑦⑧⑨⑩ⅠⅡⅢⅣⅤⅥⅦⅧⅨⅩⅪⅫⅰⅱⅲⅳⅴⅵⅶⅷⅸⅹ

        /// <summary>
        /// Convert the word special number to normal problem or section ID tag
        /// </summary>
        /// <param name="special">the word special number in string</param>
        /// <param name="isProblem">true:return problem ID tag</param>
        /// <returns>normal ID tag</returns>
        public static string ConvertWordSpecialNumberToNormalIDTag(string special,bool isProblem )
        { 
            string resultNo=string.Empty;
            switch (special)
            {
                case "⒈":
                case "⑴":
                case "①":
                case "Ⅰ":
                case "ⅰ":
                    if (isProblem)
                        resultNo = "1．";
                    else
                        resultNo = "一、";
                    break;
                case "⒉":
                case "⑵":
                case "②":
                case "Ⅱ":
                case "ⅱ":
                    if (isProblem)
                        resultNo = "2．";
                    else
                        resultNo = "二、";
                    break;
                case "⒊":
                case "⑶":
                case "③":
                case "Ⅲ":
                case "ⅲ":
                    if (isProblem)
                        resultNo = "3．";
                    else
                        resultNo = "三、";
                    break;
                case "⒋":
                case "⑷":
                case "④":
                case "Ⅳ":
                case "ⅳ":
                    if (isProblem)
                        resultNo = "4．";
                    else
                        resultNo = "四、";
                    break;
                case "⒌":
                case "⑸":
                case "⑤":
                case "Ⅴ":
                case "ⅴ":
                    if (isProblem)
                        resultNo = "5．";
                    else
                        resultNo = "五、";
                    break;
                case "⒍":
                case "⑹":
                case "⑥":
                case "Ⅵ":
                case "ⅵ":
                    if (isProblem)
                        resultNo = "6．";
                    else
                        resultNo = "六、";
                    break;
                case "⒎":
                case "⑺":
                case "⑦":
                case "Ⅶ":
                case "ⅶ":
                    if (isProblem)
                        resultNo = "7．";
                    else
                        resultNo = "七、";
                    break;
                case "⒏":
                case "⑻":
                case "⑧":
                case "Ⅷ":
                case "ⅷ":
                    if (isProblem)
                        resultNo = "8．";
                    else
                        resultNo = "八、";
                    break;
                case "⒐":
                case "⑼":
                case "⑨":
                case "Ⅸ":
                case "ⅸ":
                    if (isProblem)
                        resultNo = "9．";
                    else
                        resultNo = "九、";
                    break;
                case "⒑":
                case "⑽":
                case "⑩":
                case "Ⅹ":
                case "ⅹ":
                    if (isProblem)
                        resultNo = "10．";
                    else
                        resultNo = "十、";
                    break;
                case "⒒":
                case "⑾":
                case "Ⅺ":
                    if (isProblem)
                        resultNo = "11．";
                    else
                        resultNo = "十一、";
                    break;
                case "⒓":
                case "⑿":
                case "Ⅻ":
                    if (isProblem)
                        resultNo = "12．";
                    else
                        resultNo = "十二、";
                    break;
                case "⒔":
                case "⒀":
                    if (isProblem)
                        resultNo = "13．";
                    else
                        resultNo = "十三、";
                    break;
                case "⒕":
                case "⒁":
                    if (isProblem)
                        resultNo = "14．";
                    else
                        resultNo = "十四、";
                    break;
                case "⒖":
                case "⒂":
                    if (isProblem)
                        resultNo = "15．";
                    else
                        resultNo = "十五、";
                    break;
                case "⒗":
                case "⒃":
                    if (isProblem)
                        resultNo = "16．";
                    else
                        resultNo = "十六、";
                    break;
                case "⒘":
                case "⒄":
                    if (isProblem)
                        resultNo = "17．";
                    else
                        resultNo = "十七、";
                    break;
                case "⒙":
                case "⒅":
                    if (isProblem)
                        resultNo = "18．";
                    else
                        resultNo = "十八、";
                    break;
                case "⒚":
                case "⒆":
                    if (isProblem)
                        resultNo = "19．";
                    else
                        resultNo = "十九、";
                    break;
                case "⒛":
                case "⒇":
                    if (isProblem)
                        resultNo = "20．";
                    else
                        resultNo = "二十、";
                    break;
                default:
                    break;
            }
            return resultNo;
        
        }

        /// <summary>
        /// Convert the source string to the URL standard string
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string ConvertToURLText(string source)
        {
            Regex valueReg = new Regex(@"[^a-zA-Z0-9]+", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            string result = valueReg.Replace(source, "-");
            if (result.StartsWith("-"))
                result = result.Substring(1);
            if (result.EndsWith("-"))
                result = result.Substring(0, result.Length - 1);
            return result;
        }


        /// <summary>
        /// Convert the text into integer safely
        /// </summary>
        /// <param name="text"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int SafeInt(string text, int defaultValue)
        {
            if (!IsNullorEmpty(text))
            {
                try
                {
                    return Int32.Parse(text);
                }
                catch { }
            }
            return defaultValue;
        }

        /// <summary>
        /// Get the int part from the designated string.
        /// </summary>
        /// <param name="text">the inpute string</param>
        /// <param name="defaultValue">the default value if failed to get</param>
        /// <returns></returns>
        public static int GetIntPart(string text, int defaultValue)
        {
            if (!IsNullorEmpty(text))
            {
                try
                {
                    Regex getRegex = new Regex(@"\d+",
                        RegexOptions.IgnoreCase | RegexOptions.Singleline);

                    Match getMatch = getRegex.Match(text);
                    if (getMatch.Success)
                    {
                        return Convert.ToInt32(getMatch.Value);
                    }
                    else
                    {
                        return defaultValue;
                    }

                }
                catch { }
            }
            return defaultValue;
        }



        public static int SafeInt(double doubleNumber, int defaultValue)
        {
            try
            {
                return Convert.ToInt32(doubleNumber);
            }
            catch { }
            return defaultValue;
        }

        public static bool SafeBool(string text, bool defaultValue)
        {
            if (!IsNullorEmpty(text))
            {
                try
                {
                    return bool.Parse(text);
                }
                catch (Exception)
                {
                }

            }

            return defaultValue;
        }


        public static bool IsNullorEmpty(CollectionBase list)
        {
            return list == null || list.Count == 0;
        }

        public static bool IsNullorEmpty(string text)
        {
            return text == null || text.Trim().Length == 0;
        }

        public static bool IsDate(string text)
        {
            if (IsNullorEmpty(text))
                return false;

            try
            {
                DateTime dt = DateTime.Parse(text);
                return true;
            }
            catch (Exception)
            {
            }

            return false;
        }

        public static bool IsGUID(string expression)
        {
            if (expression != null)
            {
                Regex guidRegEx = new Regex(@"^(\{{0,1}([0-9a-fA-F]){8}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){12}\}{0,1})$");
                return guidRegEx.IsMatch(expression);
            }
            return false;
        }

        /// <summary>
        /// Check whether a char is Chinese char
        /// </summary>
        /// <param name="c">the char to check</param>
        /// <returns>ture:Chinese char</returns>
        public static bool IsChineseChar(char input)
        {
            int charStart = Convert.ToInt32("4e00", 16); 
            int charEnd = Convert.ToInt32("9fff", 16);
            if (Convert.ToInt32(input) < charStart | Convert.ToInt32(input) > charEnd)
            {
                return false;
            }
            else
                return true;
        
        }


        /// <summary>
        /// Check whether a character is english  charactor
        /// </summary>
        /// <param name="input">the char to check</param>
        /// <returns>ture:English char</returns>
        public static bool IsEnglishCharacter(char input)
        {
            Regex rex = new Regex("[a-zA-Z]+");
            Match ma = rex.Match(input.ToString());
            if (ma.Success)
            {
                return true;
            }
            else
            {
                return false;
            } 
       
        }

        /// <summary>
        /// Check whether a character is ending  charactor
        /// </summary>
        /// <param name="input">the char to check</param>
        /// <returns>ture:ending char</returns>
        public static bool IsEndingCharacter(char input)
        {
            Regex rex = new Regex(@"[\.|．|。|？|?|)|）]+");
            Match ma = rex.Match(input.ToString());
            if (ma.Success)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        /// <summary>
        /// Get the file extension from the designated source file.
        /// </summary>
        /// <param name="sourceFile">the designated source file</param>
        /// <returns>this file's extension, include ".",e.g. ".PPT"</returns>
        public static string GetFileExtension(string sourceFile)
        {
            if (string.IsNullOrEmpty(sourceFile))
                return string.Empty;
            int dotIndex=sourceFile.LastIndexOf('.');
            string extension = string.Empty;
            if (dotIndex > 0)
            {
                extension = sourceFile.Substring(dotIndex);
                if (extension.Length <= 6)
                    return extension.ToLower();
                else
                    return string.Empty;
            }
            else
                return string.Empty;

        }


        /// <summary>
        /// Check whether the source file is an file containing math problem information.
        /// </summary>
        /// <param name="sourceFile">the designated source file</param>
        /// <returns>true:YES</returns>
        public static bool IsMathFileExtension(string sourceFile)
        {
            string extension = StringHelper.GetFileExtension(sourceFile).ToLower();
            if (extension == ".doc" || extension == ".zip" || extension == ".rar" || extension == ".ppt" || extension == ".txt")
                return true;
            return false;


        }


        /// <summary>
        /// Retrive the contents in the fullt-matched HTML tag with designated HTML bigin string
        /// </summary>
        /// <param name="source">the source string to retrieve the HTML contents</param>
        /// <param name="beginString">the begin string containing HTML tag occurs in the source string</param>
        /// <param name="matchIndex">return the  index of the current match</param>
        /// <returns></returns>
        public static string GetContentsBeginWithHtmlTag(string source, string beginString,ref int matchIndex)
        {
            matchIndex = -1;
            if (string.IsNullOrEmpty(source))
            {
                return string.Empty;
            }
            //Retrieve the HTML tag
            if (!beginString.StartsWith("<"))
                return string.Empty;

            //Here we ignore case so we convert it to upper case to deal with
            string upperSource = source.ToUpper();
            beginString = beginString.ToUpper();

            int spaceIndex = beginString.IndexOf(' ');
            //string htmlTag = beginString.Substring(1, spaceIndex-1);
            string htmlStartTag = beginString.Substring(0, spaceIndex);
            string htmlEndTag = "</"+htmlStartTag.Substring(1, spaceIndex-1)+">";
            string htmlOneNodeEndTag = "/>";


            int match = 0;
            int beginIndex = upperSource.IndexOf(beginString);
            if (beginIndex < 0)
                return string.Empty;
            match = 1;
            matchIndex = beginIndex;
            //beginIndex:the begin index in the source string for the contents to retrieve
            //endIndex:the end index in the source string for the contents to retrieve

            int currentIndex = beginIndex + beginString.Length;//We must surpass the beginString, the same for the following
            int tempIndex1 = 0, tempIndex2 = 0, endIndex;

            string currentString = upperSource.Substring(currentIndex);

            //Check whether this is an html tag ended in a node

            if (!beginString.EndsWith(">"))
            {//Only if the beginString does NOT end with ">", we should check the htmlOneNodeEndTag.
                tempIndex1 = currentString.IndexOf(htmlOneNodeEndTag);
                if (tempIndex1 >= 0)
                {                    
                    if (currentString.IndexOf("<", 1, tempIndex1 - 1) < 0)
                    { //This is an HTML tag ended in one node,return this node.
                        currentIndex += tempIndex1 + htmlOneNodeEndTag.Length;
                        currentString = upperSource.Substring(currentIndex);
                        endIndex = currentIndex;
                        //if (currentString.IndexOf(beginString) >= 0)
                        //    haveOtherMatch = true;
                        //else
                        //    haveOtherMatch = false;
                        return source.Substring(beginIndex, endIndex - beginIndex);
                    }
                }
            }
            else
            {
                if (beginString.EndsWith("/>"))
                {
                    endIndex = currentIndex;
                    return beginString;                    
                }
            }

            string tempString=string.Empty;

            bool isFound = false;
            bool tagStarted = true;

            //We need the following two line to let the beginString could be "<div class="abcd">",i.e. with ">" in the end.
            if (beginString.EndsWith(">"))
                tagStarted = false;

            while (currentIndex < upperSource.Length - 1)
            {
                isFound = false;
                //Check whether this is an html tag ended in a node
                //tempIndex1 = currentString.IndexOf(htmlOneNodeEndTag);
                //if (tempIndex1 >= 0)
                //{
                //    isFound = true;
                //    if (currentString.IndexOf("<", 1, tempIndex1 - 1) < 0)
                //    { //This is an HTML tag ended in one node,return this node.
                //        match -= 1;
                //        currentIndex += tempIndex1 + htmlOneNodeEndTag.Length;
                //        currentString = upperSource.Substring(currentIndex);
                //    }
                //    else
                //    {
                //        currentIndex += tempIndex1 + htmlOneNodeEndTag.Length;
                //        currentString = upperSource.Substring(currentIndex);
                    
                //    }
                //}

                // tagStarted =true means a new htmlStartTag is found, we need check whether it ends like <div .... />
                if (tagStarted)
                {
                    tempIndex1 = currentString.IndexOf(">");
                    if (tempIndex1 >= 0)
                    {
                        if (tempIndex1 > 0)
                        {
                            if (currentString.Substring(tempIndex1 - 1, 2) == htmlOneNodeEndTag)
                            {
                                match -= 1;
                            }
                        }
                        isFound = true;
                        tagStarted = false;
                        currentIndex += tempIndex1 + 1;
                        currentString = upperSource.Substring(currentIndex);
                    }
                    else
                        break;//Something must be wrong
                }
                else
                {
                    //Search for the begin tag
                    tempIndex1 = currentString.IndexOf(htmlStartTag);
                    //Search for the end tag
                    tempIndex2 = currentString.IndexOf(htmlEndTag);
                    if (tempIndex1 <= tempIndex2)
                    {
                        if (tempIndex1 >= 0)
                        {
                            match += 1;
                            tagStarted = true;

                            currentIndex += tempIndex1 + htmlStartTag.Length;
                            currentString = upperSource.Substring(currentIndex);
                            isFound = true;

                        }
                        else
                        {//can NOT find htmlStartTag
                            if (tempIndex2 >= 0)
                            {
                                match -= 1;
                                tagStarted = false;
                                currentIndex += tempIndex2 + htmlEndTag.Length;
                                currentString = upperSource.Substring(currentIndex);
                                isFound = true;
                            }
                        }
                    }
                    else
                    { //tempIndex1 > tempIndex2
                        if (tempIndex2 >= 0)
                        {
                            match -= 1;
                            tagStarted = false;
                            currentIndex += tempIndex2 + htmlEndTag.Length;
                            currentString = upperSource.Substring(currentIndex);
                            isFound = true;
                        }
                        else
                        {//can NOT find htmlEndTag
                            if (tempIndex1 >= 0)
                            {
                                match += 1;
                                tagStarted = true;
                                currentIndex += tempIndex1 + htmlStartTag.Length;
                                currentString = upperSource.Substring(currentIndex);
                                isFound = true;

                            }

                        }

                    }
                }//if (tempIndex >= 0)


                if (match <= 0)
                {
                    //endIndex = currentIndex;
                    break;
                }
                //Can NOT find the next match in this loop,break this loop.
                if (!isFound)
                    break;
            }//while (currentIndex < upperSource.Length - 1)
            if (!isFound)
                return string.Empty;
            else
            {
                endIndex = currentIndex;
                //if (upperSource.IndexOf(beginString, endIndex) >= 0)
                //{
                //    haveOtherMatch = true;
                //}
                //else
                //{
                //    haveOtherMatch = false;
                //}
                matchIndex = beginIndex;
                return source.Substring(beginIndex, endIndex - beginIndex);
  
            }
        }



        #region 取得一个字符串包含一个子串的次数
        /// <summary>
        /// 取得一个字符串包含一个子串的次数
        /// </summary>
        /// <param name="main">需要查找的字符串</param>
        /// <param name="sub">子串</param>
        /// <returns>子串出现的次数</returns>
        public static int GetCountOfSubString(string main, string sub)
        {
            string[] stringSeparators = new string[] { sub };
            string[] splitedArray = main.Split(stringSeparators, StringSplitOptions.None);
            return splitedArray.Length - 1;
 
        }

        /// <summary>
        /// 取得一个字符串 在一个指定位置前 包含一个子串的次数
        /// </summary>
        /// <param name="main">需要查找的字符串</param>
        /// <param name="end">结束位置</param>
        /// <param name="sub">子串</param>
        /// <returns>子串出现的次数</returns>
        public static int GetCountOfSubString(string main,int end,string sub)
        {
            if (end <= 0)
                return 0;
            string mainPart = main.Substring(0, end);
            return GetCountOfSubString(mainPart,sub);
        }


        #endregion

        #region 判断一个一个字符串是否带有语义结束标记
        /// <summary>
        /// Return the result of whether a string is sentence ended. i.e.the last char is .?
        /// </summary>
        /// <param name="inputString">the string to deal with</param>
        /// <returns></returns>
        public static bool IsStringSentenceEnded(string inputString)
        {
            if (inputString.Length == 0)
                return true;
            int length=inputString.Length;
            char lastChar=inputString[length-1];
            if (lastChar == '.' /*|| lastChar == '。' || lastChar == '?' || lastChar == '？'*/)
                return true;
            else
                return false;
        }

        #endregion

        public static string RemoveUserInputBoudaryText(string input_source)
        {
            string resultStr = input_source;
            resultStr = resultStr.Replace("\r", string.Empty);
            resultStr = resultStr.Replace("\n", string.Empty);
            resultStr = resultStr.Trim();
            return resultStr;
        }


        #region 清除HTML中的反转义字符串
        /// <summary>
        /// Remove the HTML tags in the input string.
        /// NOTE:sometimes we need "&amp;quot;" in the paper text,so we do NOT replace it.
        /// </summary>
        /// <param name="inputString">the input string </param>
        /// <returns></returns>
        public static string RemoveHtmlUnescapeTags(string inputString)
        {
            ////Sometimes a problem include many sub-problem,we need blank to separate it.
            ////So currently comment this out.
            //////Replace &nbsp;
            ////inputString = inputString.Replace(HtmlUnescapeConstants.HtmlBlankSpace, " ");
            //////Replace &amp;nbsp;
            ////inputString = inputString.Replace(HtmlUnescapeConstants.HtmlBlankSpace2, " ");
            ////Replace &quot;
            //inputString = inputString.Replace(HtmlUnescapeConstants.HtmlQuot, "\"");
            ////Replace &amp;quot;
            //inputString = inputString.Replace(HtmlUnescapeConstants.HtmlQuot2, "\"");
            ////Replace "\r"
            //inputString = inputString.Replace(HtmlUnescapeConstants.HtmlCarriageReturn, "");
            ////Replace "\n"
            //inputString = inputString.Replace(HtmlUnescapeConstants.HtmlNewLine, "");


            //return inputString;
            return RemoveHtmlUnescapeTags(inputString,false);


        }

        /// <summary>
        /// Remove the HTML tags in the input string.
        /// 
        /// </summary>
        /// <param name="inputString">the input string </param>
        /// <returns></returns>
        public static string RemoveHtmlUnescapeTags(string inputString,bool clearAll)
        {
            if (clearAll)
            {
                //Replace &nbsp;
                inputString = inputString.Replace(HtmlUnescapeConstants.HtmlBlankSpace, " ");
                //Replace &amp;nbsp;
                inputString = inputString.Replace(HtmlUnescapeConstants.HtmlBlankSpace2, " ");
            }
            //Replace & quot;
            inputString = inputString.Replace(HtmlUnescapeConstants.HtmlQuot, "\"");
            //Replace &amp;quot;
            inputString = inputString.Replace(HtmlUnescapeConstants.HtmlQuot2, "\"");
            //Replace "\r"
            inputString = inputString.Replace(HtmlUnescapeConstants.HtmlCarriageReturn, "");
            //Replace "\n"
            inputString = inputString.Replace(HtmlUnescapeConstants.HtmlNewLine, "");


            return inputString;


        }
        /// <summary>
        /// Remove all the HTML tags in the input string,including the "&amp;quot;";
        /// </summary>
        /// <param name="inputString">the input string </param>
        /// <returns></returns>
        public static string RemoveAllHtmlUnescapeTags(string inputString)
        {
            //Sometimes a problem include many sub-problem,we need blank to separate it.
            //So currently comment this out.
            //Replace &nbsp;
            inputString = inputString.Replace(HtmlUnescapeConstants.HtmlBlankSpace, " ");
            //Replace &amp;nbsp;
            inputString = inputString.Replace(HtmlUnescapeConstants.HtmlBlankSpace2, " ");
            //Replace &quot;
            inputString = inputString.Replace(HtmlUnescapeConstants.HtmlQuot, "\"");
            //Replace &amp;quot;
            inputString = inputString.Replace(HtmlUnescapeConstants.HtmlQuot2, "\"");
            //Replace "\r"
            inputString = inputString.Replace(HtmlUnescapeConstants.HtmlCarriageReturn, "");
            //Replace "\n"
            inputString = inputString.Replace(HtmlUnescapeConstants.HtmlNewLine, "");


            return inputString;


        }

        /// <summary>
        /// Replace the "<>" with "&lt;&gt;"
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        public static string EscapeHtmlStartAndEndTags(string inputString)
        {

            inputString = inputString.Replace("<", HtmlUnescapeConstants.HtmlStartTag);
            inputString = inputString.Replace(">",HtmlUnescapeConstants.HtmlEndTag);
            inputString = inputString.Replace("＜", HtmlUnescapeConstants.HtmlStartTag);
            inputString = inputString.Replace("＞", HtmlUnescapeConstants.HtmlEndTag);

            return inputString;
        }

        /// <summary>
        /// Preprocessing the HTML tag in the string, i.e. "<>",which will influence the appearance in the browser.
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string EscapeHtmlStartAndEndTagsWithHTML(string html)
        {
            string preTag = string.Empty;
            StringBuilder resultBuilder = new StringBuilder(10240);
            int matchIndex = 0, lastIndex = 0;
            Regex getHtmlTagRegex = new Regex(@"<[^<>]*>",
                RegexOptions.IgnoreCase | RegexOptions.Singleline);
            Match matches = getHtmlTagRegex.Match(html);
            while (matches.Success)
            {
                matchIndex = matches.Index;
                if (matchIndex > 0 && matchIndex > lastIndex)
                {
                    preTag = html.Substring(lastIndex, matchIndex - lastIndex);
                    //resultBuilder.Append(string.Format("<SPAN>{0}</SPAN>",StringHelper.EscapeHtmlStartAndEndTags(preTag)));
                    resultBuilder.Append(string.Format("<SPAN>{0}</SPAN>", preTag));

                }
                resultBuilder.Append(matches.Value);
                lastIndex = matchIndex + matches.Value.Length;
                matches = matches.NextMatch();
            }//while (matches.Success)

            //We need highlight the last part after the match
            if (lastIndex < html.Length)
            {
                preTag = html.Substring(lastIndex);
                //resultBuilder.Append(string.Format("<SPAN>{0}</SPAN>", StringHelper.EscapeHtmlStartAndEndTags(preTag)));
                resultBuilder.Append(string.Format("<SPAN>{0}</SPAN>", preTag));

            }

            return resultBuilder.ToString();
        }
        #endregion

        #region Math tag replacement
        /// <summary>
        /// Replace math html tag with const string so we can analyze it smoothly
        /// </summary>
        /// <param name="inputString">the source string</param>
        /// <returns>the result string after replacing</returns>
        public static string ReplaceMathHtmlTag(string inputString)
        {
            inputString = inputString.Replace(MathHtmlTags.SubBeginTag, MathHtmlTags.SubBeginTagReplacement);
            inputString = inputString.Replace(MathHtmlTags.SubEndTag, MathHtmlTags.SubEndTagReplacement);
            inputString = inputString.Replace(MathHtmlTags.SupBeginTag, MathHtmlTags.SupBeginTagReplacement);
            inputString = inputString.Replace(MathHtmlTags.SupEndTag, MathHtmlTags.SupEndTagReplacement);

            inputString = inputString.Replace(MathHtmlTags.LowerSubBeginTag, MathHtmlTags.SubBeginTagReplacement);
            inputString = inputString.Replace(MathHtmlTags.LowerSubEndTag, MathHtmlTags.SubEndTagReplacement);
            inputString = inputString.Replace(MathHtmlTags.LowerSupBeginTag, MathHtmlTags.SupBeginTagReplacement);
            inputString = inputString.Replace(MathHtmlTags.LowerSupEndTag, MathHtmlTags.SupEndTagReplacement);


            return inputString;


        }

        /// <summary>
        /// Retrieve math html tag 
        /// </summary>
        /// <param name="inputString">the source string</param>
        /// <returns>the result string after retrieving</returns>

        public static string RetrieveMathHtmlTag(string inputString)
        {
            inputString = inputString.Replace(MathHtmlTags.SubBeginTagReplacement, MathHtmlTags.SubBeginTag);
            inputString = inputString.Replace(MathHtmlTags.SubEndTagReplacement, MathHtmlTags.SubEndTag);
            inputString = inputString.Replace(MathHtmlTags.SupBeginTagReplacement, MathHtmlTags.SupBeginTag);
            inputString = inputString.Replace(MathHtmlTags.SupEndTagReplacement, MathHtmlTags.SupEndTag);



            return inputString;


        }

        #endregion



        #region 清除HTML中的非法数字
        /// <summary>
        /// Remove the invalid digits in the input string
        /// </summary>
        /// <param name="inputString">the input string </param>
        /// <returns></returns>
        public static string RemoveInvalidDigits(string inputString)
        {
            //Remove "１２３４５６７８９０"
            inputString = inputString.Replace("１", "1");
            inputString = inputString.Replace("２", "2");
            inputString = inputString.Replace("３", "3");
            inputString = inputString.Replace("４", "4");
            inputString = inputString.Replace("５", "5");
            inputString = inputString.Replace("６", "6");
            inputString = inputString.Replace("７", "7");
            inputString = inputString.Replace("８", "8");
            inputString = inputString.Replace("９", "9");
            inputString = inputString.Replace("０", "0");

            return inputString;


        }

        #endregion

        #region Check whether a location in a string is between a HTML tag
        /// <summary>
        /// Check whether a location in a string is between a HTML tag
        /// </summary>
        /// <param name="originalString">the original string to check against</param>
        /// <param name="location">the location index</param>
        /// <returns></returns>
        public static bool IsBetweenHtmlTag(string originalString,int location)
        {
            string subString = originalString.Substring(location);
            char[] separators ={ NormalHtmlTags.HTMLTagStartFlag, NormalHtmlTags.HTMLTagEndFlag };
            int separtorsIndex = subString.IndexOfAny(separators);
            if (separtorsIndex < 0)
                return false;
            if (subString[separtorsIndex] == NormalHtmlTags.HTMLTagEndFlag)
                return true;
            else
                return false;

        }

        #endregion



        #region  得到一个试卷问题的题干(去除前面括号中的文字)
        /// <summary>
        /// 得到一个字符串的主干(去除前面括号中的文字)
        /// </summary>
        /// <param name="originalString">原始字符串</param>
        /// <param name="prefix">字符串前缀</param>
        /// <returns>字符串主干</returns>
        public static string GetMainText(string originalString,ref string prefix)
        {
            int prefixEndIndex = 0;
            string mainText = string.Empty;
            char[] separators ={ ')', /*'）'*/};

            if (originalString.StartsWith("(") || originalString.StartsWith("（"))
            {
                prefixEndIndex = originalString.IndexOfAny(separators);
                if (prefixEndIndex > 0)
                {
                    prefix = originalString.Substring(0, prefixEndIndex + 1);
                    mainText = originalString.Substring(prefixEndIndex + 1);
                }
                else
                {
                    prefix = "(";
                    mainText = originalString.Substring(1);
                }
            }
            else
            {
                prefix = string.Empty;
                mainText = originalString;
            }
            return mainText;
        
        }

        #endregion

        #region 区分中英文的字符串截取
        public static string GetChineseSubString( string inputString, int count )
		{
			return GetChineseSubString ( inputString, count, ".." );
		}
		public static string GetChineseSubString( string inputString, int count, string addition )
		{
			int limit = count * 2;
			if ( limit >= GetChineseStringLength ( inputString ) )
			{
				return inputString;
			}
			else
			{
				limit -= addition.Length;
				StringBuilder sb = new StringBuilder ();
				char [] chars = inputString.ToCharArray ();
				for ( int i = 0; i < chars.Length; i++ )
				{
					char c = chars [i];
					sb.Append ( c );
					if ( c > 127 )
					{
						limit -= 2;
						if ( limit < 0 )
						{
							sb.Length--;
							break;
						}
					}
					else
					{
						limit--;
					}
					if ( limit == 0 )
					{
						break;
					}
				}
				return sb.ToString () + addition;
			}
		}

		public static int GetChineseStringLength( string inputString )
		{
			int length = 0;
			if ( !string.IsNullOrEmpty ( inputString ) )
			{
				char [] chars = inputString.ToCharArray ();
				for ( int i = 0; i < chars.Length; i++ )
				{
					if ( chars [i] > 127 )
					{
						length += 2;
					}
					else
					{
						length++;
					}
				}
			}
			return length;
		} 
		
		#endregion

        #region 得到一个字符串打头的只包含汉字和英文字母的子字符串
        public static string GetFirstAlphabeticSubstring(string inputText)
        {
            if (inputText == null)
                return string.Empty;
            char[] separators={'(',/*'（',' ',',','，','.','。','、'*/};
            int separtorsIndex=inputText.IndexOfAny(separators);
            if (separtorsIndex <= 0)
                return inputText;
            string firstSubstring=inputText.Substring(0, separtorsIndex);

            return firstSubstring;
        
        }


        #endregion

        #region 取得前n位长度的字串
        /// <summary>
        /// 取得前n位长度的字串
        /// </summary>
        /// <param name="theString">需要处理的字符串</param>
        /// <param name="count">需要的字符串长度</param>
        /// <returns></returns>
        public static string GetSubString(string theString, int count)
        {
            if ( count >= theString.Length )
                return theString;
            else
                return theString.Substring( 0, count );
        }

        /// <summary>
        /// 取得前n位长度的字串加后缀字符串
        /// </summary>
        /// <param name="theString">需要处理的字符串</param>
        /// <param name="count">需要的字符串长度</param>
        /// <param name="addition">后缀字符串</param>
        /// <returns></returns>
        public static string GetSubString(string theString, int count, string addition)
        {
            string a = GetSubString( theString, count );

            if ( a.Length < theString.Length )
                return a + addition;
            else
                return a;
        }
        #endregion

        #region 根据URL参数取得英文名
        /// <summary>
        /// 根据URL参数取得英文名
        /// </summary>
        /// <param name="queryString">URL参数</param>
        /// <returns></returns>
        public static string GetEnglishNameFromUrl(string queryString)
        {
			return queryString.Replace(",", ".").Replace("_", " ").Replace("Ю", ":").Replace("杠", "/").Replace("加", "+");
        }
        #endregion

        #region 根据英文名取得用于URL的字符串
        /// <summary>
        /// 根据英文名取得用于URL的字符串
        /// </summary>
        /// <param name="englishName">英文名</param>
        /// <returns></returns>
        public static string GetUrlFromEnglishName(string englishName)
        {
			return englishName.Replace(".", ",").Replace(" ", "_").Replace(":", "Ю").Replace("/", "杠").Replace("+", "加");
        }
        #endregion

        #region 段落格式化
        /// <summary>
        /// 段落格式化
        /// </summary>
        /// <param name="context">正文</param>
        /// <param name="paragraphClass">段落样式Class</param>
        /// <param name="paragraphStyle">段落样式</param>
        /// <returns></returns>
        public static string ParagraphFormat(string context, string paragraphClass, string paragraphStyle)
        {
            //取消格式转换
            //10.26 by andy
            //return context.Replace( Environment.NewLine, "<br/>");

            //初始化StringBuilder
            StringBuilder sb = new StringBuilder( 512 );

            //初始化段落模式
            Regex regex = new Regex( @"(.*\r?\n)" );
            //取得匹配集合
            MatchCollection mc = regex.Matches( context + "\n" );

            foreach ( Match m in mc )
            {
                //获取匹配项
                string s = m.Groups[0].Value.Trim();

                //若匹配项不为空则继续
                if ( s != string.Empty )
                {
                    //若匹配项并非标题则取得段落，否则取得标题
                    if ( !Regex.Match( s, @"^/h\d.*$" ).Success )
                    {
                        sb.Append( "<p" );
                        //获取段落样式Class
                        if ( paragraphClass.Trim() != string.Empty )
                            sb.Append( " class=\"" + paragraphClass + "\"" );
                        //获取段落样式
                        if ( paragraphStyle.Trim() != string.Empty )
                            sb.Append( " style=\"" + paragraphStyle + "\"" );
                        sb.Append( ">" );
                        //获取段落正文
                        sb.Append( m.Groups[0].Value.Trim() );
                        sb.Append( "</p>\n" );
                    }
                    else
                    {
                        //获取标题标记
                        string h = Regex.Replace( s, @"^/(h\d).*$", "$1" );
                        //获取标题内容
                        string t = Regex.Replace( s, @"^/h\d(.*)$", "$1" );
                        sb.Append( "<" + h + ">" + t + "</" + h + ">\n" );
                    }
                }
            }

            //返回格式化段落
            return sb.ToString();
        }
        #endregion

        #region 取得整型的Base64字符串
        /// <summary>
        /// 取得整型的Base64字符串
        /// </summary>
        /// <param name="o">整型对象</param>
        /// <returns></returns>
        public static string GetBase64StringFromString(object o)
        {
            byte[] b = Encoding.UTF8.GetBytes( o.ToString() );
            return Convert.ToBase64String( b );
        }
        #endregion

        #region 取得Base64字符串的整型
        /// <summary>
        /// 取得Base64字符串的整型
        /// </summary>
        /// <param name="s">字符串</param>
        /// <returns></returns>
        public static string GetStringFromBase64String(string s)
        {
            byte[] b = Convert.FromBase64String( s );
            return Encoding.UTF8.GetString( b );
        }
        #endregion

        #region 高亮显示关键字
        /// <summary>
        /// 返回高亮(红色)显示关键字的内容文本
        /// </summary>
        /// <param name="keyword">关键字文本</param>
        /// <param name="content">内容文本</param>
        /// <returns>高亮后的文本</returns>
        public static string ShowHighlightKeyword(string keyword, string content)
        {
            if ( keyword == string.Empty || content == string.Empty )
                return string.Empty;

            string charset = @"`~!@#$%^&*()_+-=[]\|}{;:'/.,<>?！？，。（）…—￥";

            foreach ( char c in charset )
            {
                keyword = keyword.Replace( c.ToString(), " " );
            }

            string a = keyword;

            for ( int i = 0 ; i < a.Length ; i++ )
            {
                try
                {
                    string c = a.Substring( i, 1 );
                    int asc = (int)Convert.ToChar( c );
                    if ( asc > 255 )
                    {
                        int n = a.IndexOf( c );
                        a = a.Insert( n, " " );
                        a = a.Insert( n + 2, " " );
                        i += 2;
                    }
                }
                catch
                {
                    continue;
                }
            }

            foreach ( string b in a.Split( (char)32 ) )
            {
                switch ( b.Trim().ToLower() )
                {
                    case "":
                        break;
                    default:
                        Regex r2 = new Regex( "(" + b + ")", RegexOptions.IgnoreCase );
                        content = r2.Replace( content, "[(]$1[)]" );
                        break;
                }
            }

            content = content.Replace( "[(]", "<span class=\"red\">" );
            content = content.Replace( "[)]", "</span>" );

            return content;
        }
        #endregion

        #region 获取安全的整形数

        /// <summary>
        /// 获取安全的整形数，默认值为0
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static int SafeInt(string text)
        {
            return SafeInt( text, 0 );
        }


        #endregion

        #region 字符串比较
        /// <summary>
        /// 字符串比较
        /// </summary>
        /// <param name="strA"></param>
        /// <param name="strB"></param>
        /// <returns></returns>
        public static bool StringCompare(string strA, string strB)
        {
            if ( string.Compare( strA, strB, true ) == 0 )
                return true;
            return false;
        }

        #endregion

        #region 获取安全的HTML内容文本
        /// <summary>
        /// 获取安全的HTML内容文本
        /// 此方法将过滤掉可能造成不安全因素的HTML标记
        /// By Marila
        /// </summary>
        /// <param name="content">HTML内容文本</param>
        /// <returns></returns>
        public static string GetSafeHtmlText(string content)
        {
            if ( string.IsNullOrEmpty( content ) )
                return string.Empty;

            string s = content;

            //过滤<iframe>
            s = Regex.Replace( s, @"<iframe[\s\S]*?>([\s\S]*?<\/iframe[\s\S]*?>)?", string.Empty, RegexOptions.IgnoreCase );
            s = Regex.Replace( s, @"<iframe", string.Empty, RegexOptions.IgnoreCase );

            //过滤<script>
            s = Regex.Replace( s, @"<script[\s\S]*?>([\s\S]*?<\/script[\s\S]*?>)?", string.Empty, RegexOptions.IgnoreCase );
            s = Regex.Replace( s, @"<script", string.Empty, RegexOptions.IgnoreCase );

            //过滤<input>
            s = Regex.Replace( s, @"<input[\s\S]*?>([\s\S]*?<\/input[\s\S]*?>)?", string.Empty, RegexOptions.IgnoreCase );
            s = Regex.Replace( s, @"<input", string.Empty, RegexOptions.IgnoreCase );

            //过滤<textarea>
            s = Regex.Replace( s, @"<textarea[\s\S]*?>([\s\S]*?<\/textarea[\s\S]*?>)?", string.Empty, RegexOptions.IgnoreCase );
            s = Regex.Replace( s, @"<textarea", string.Empty, RegexOptions.IgnoreCase );

            //过滤<select>
            s = Regex.Replace( s, @"<select[\s\S]*?>([\s\S]*?<\/select[\s\S]*?>)?", string.Empty, RegexOptions.IgnoreCase );
            s = Regex.Replace( s, @"<select", string.Empty, RegexOptions.IgnoreCase );

            //过滤<option>
            s = Regex.Replace( s, @"<option[\s\S]*?>([\s\S]*?<\/option[\s\S]*?>)?", string.Empty, RegexOptions.IgnoreCase );
            s = Regex.Replace( s, @"<option", string.Empty, RegexOptions.IgnoreCase );

            //过滤<form>
            s = Regex.Replace( s, @"<form[\s\S]*?>", string.Empty, RegexOptions.IgnoreCase );
            s = Regex.Replace( s, @"<\/form[\s\S]*?>", string.Empty, RegexOptions.IgnoreCase );

            //过滤<body>
            s = Regex.Replace( s, @"<body[\s\S]*?>", string.Empty, RegexOptions.IgnoreCase );
            s = Regex.Replace( s, @"<\/body[\s\S]*?>", string.Empty, RegexOptions.IgnoreCase );

            //过滤<html>
            s = Regex.Replace( s, @"<html[\s\S]*?>", string.Empty, RegexOptions.IgnoreCase );
            s = Regex.Replace( s, @"<\/html[\s\S]*?>", string.Empty, RegexOptions.IgnoreCase );

            //过滤<meta>
            s = Regex.Replace( s, @"<meta[\s\S]*?>", string.Empty, RegexOptions.IgnoreCase );
            s = Regex.Replace( s, @"<meta", string.Empty, RegexOptions.IgnoreCase );

            //过滤<link>
            s = Regex.Replace( s, @"<link[\s\S]*?>", string.Empty, RegexOptions.IgnoreCase );
            s = Regex.Replace( s, @"<link", string.Empty, RegexOptions.IgnoreCase );

            //过滤<frameset>
            s = Regex.Replace( s, @"<frameset[\s\S]*?>([\s\S]*?<\/frameset[\s\S]*?>)?", string.Empty, RegexOptions.IgnoreCase );
            s = Regex.Replace( s, @"<frameset", string.Empty, RegexOptions.IgnoreCase );

            //过滤<frame>
            s = Regex.Replace( s, @"<frame[\s\S]*?>([\s\S]*?<\/frame[\s\S]*?>)?", string.Empty, RegexOptions.IgnoreCase );
            s = Regex.Replace( s, @"<frame", string.Empty, RegexOptions.IgnoreCase );

            //过滤<noframe>
            s = Regex.Replace( s, @"<noframe[\s\S]*?>([\s\S]*?<\/noframe[\s\S]*?>)?", string.Empty, RegexOptions.IgnoreCase );
            s = Regex.Replace( s, @"<noframe", string.Empty, RegexOptions.IgnoreCase );

            //过滤onload
            s = Regex.Replace( s, @"onload=(\"")?\s*[^\""\n]*(\"")?", string.Empty, RegexOptions.IgnoreCase );
            //过滤onmouseover
            s = Regex.Replace( s, @"onmouseover=(\"")?\s*[^\""\n]*(\"")?", string.Empty, RegexOptions.IgnoreCase );
            return s;
        }
        #endregion



        public static string GetDisplayText( string inputText )
        {
        	if ( string.IsNullOrEmpty( inputText ) )
        		return string.Empty;
        	return inputText.Replace( Environment.NewLine, "<br/>" );
        }

    	/// <summary>
        /// 获取符合格式的HTML
        /// </summary>
        /// <param name="inputHtml"></param>
        /// <returns></returns>
        public static string GetCleanHTMLStringFromHtml(string inputHtml)
        {
			return GetCleanHTMLStringFromHtml ( inputHtml, AllowedHtmlTagType.Default );
        }

		public static string GetCleanHTMLStringFromHtml( string inputHtml, AllowedHtmlTagType allowedHtmlTagType )
		{
			Dictionary<string, string> allowedTags = AllowedHtmlTags.Current.GetAllowedHtmlTags ( allowedHtmlTagType );
			//Dictionary<string, string> allowedTags = new Dictionary<string, string>( );
			if ( allowedTags == null || allowedTags.Count == 0 )
			{
				try
				{
					allowedTags = AllowedHtmlTags.Current.GetAllowedHtmlTags ( allowedHtmlTagType );
					if ( allowedTags == null || allowedTags.Count == 0 )
					{
                        return inputHtml;
					}
				}
				catch ( Exception e )
				{
					throw  e;
				}
			}
			return GetCleanHTMLStringFromHtml( inputHtml, allowedTags );
		}
        /// <summary>
        /// 获取符合格式的HTML
        /// </summary>
        /// <param name="inputHtml"></param>
        /// <param name="allowedTags">允许的标记集合</param>
        /// <returns></returns>
		public static string GetCleanHTMLStringFromHtml( string inputHtml, Dictionary<string, string> allowedTags )
        {
            if ( string.IsNullOrEmpty( inputHtml ) )
                return string.Empty;
            string output = GetSafeHtmlText( inputHtml );
            output = output.Replace( "&nbsp;", "♂" );
            output = HtmlHelper.MakeupTag( Cubb( output, true ), allowedTags );
            output = output.Replace( "♂", "&nbsp;" );
            return output;
        }

        /// <summary>
        /// 统计字数（忽略HTML标记）
        /// </summary>
        /// <param name="inputHtml"></param>
        /// <returns></returns>
        public static int GetWordCountFromHtml(string inputHtml)
        {
            if ( string.IsNullOrEmpty( inputHtml ) )
                return 0;
            string text = GetCleanText( inputHtml );
            return text.Replace( "&nbsp;", " " ).Replace( "　", " " ).Trim().Length;
        }

        /// <summary>
        /// 去掉标记字符串
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        public static string GetCleanText(string inputString)
        {
            if ( string.IsNullOrEmpty( inputString ) )
                return string.Empty;
            //Todo:测试
            string text = Regex.Replace( inputString, "<script((.|\n)*?)</script>", "", RegexOptions.IgnoreCase | RegexOptions.Multiline );
            text = Regex.Replace( text, "onload=(\")?\\s*[^\"\n]*(\")?", "", RegexOptions.IgnoreCase );
            text = Regex.Replace( text, @"onload=(\"")?\s*[^\""\n]*(\"")?", "", RegexOptions.IgnoreCase );
            text = Regex.Replace( text, "onload=\\\"if[^\"]*\\\"", "", RegexOptions.IgnoreCase );
            text = Regex.Replace( text, "\"javascript:", "", RegexOptions.IgnoreCase | RegexOptions.Multiline );

            string pattern = @"\<[^>]*>";
            Regex regex = new Regex( pattern, RegexOptions.IgnoreCase );
            text = regex.Replace( text, "", -1 );
        	pattern = @"\[[^[]*\]";
			regex = new Regex ( pattern, RegexOptions.IgnoreCase );
			text = regex.Replace ( text, "", -1 );
            return text;
        }
        /// <summary>
        /// 去掉标记字符串,Only remove HTML tags
        /// </summary>
        /// <param name="inputString">the original string</param>
        /// <returns>the clean string after removing the html tags</returns>
        public static string GetCleanTextWithoutHtmlTag(string inputString)
        {
            if (string.IsNullOrEmpty(inputString))
                return string.Empty;

            string pattern = @"\<[^>]*>";
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
            string text = regex.Replace(inputString, "", -1);
            return text;
        }


        /// <summary>
        /// Check whether a string contains some <a></a> links indicating downloading documents
        /// </summary>
        /// <param name="source">the source string to check with</param>
        /// <returns>true:contains <a> links </returns>
        public static bool ContainDownloadLinks(string source)
        {
            if (string.IsNullOrEmpty(source))
                return false;

            string pattern = @"<a[^>]*>(?<AText>.*?)</a>";
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
            Match matches = regex.Match(source);
            string atext = string.Empty;
            while (matches.Success)
            {
                atext = matches.Groups["AText"].Value;
                if (atext.Contains("下载") || atext.Contains("Download") )
                {
                    return true;
                }
                matches = matches.NextMatch();
            }//while (matches.Success)
            return false;
        }



        /// <summary>
        /// 忽略标记得到字符串（需整理）
        /// </summary>
        /// <param name="formattedBody"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GetFormattedSubString(string formattedBody, int length)
        {
            if ( string.IsNullOrEmpty( formattedBody ) )
            {
                return "";
            }
            else if ( formattedBody.Length < length )
            {
                return formattedBody;
            }
            else
            {
                //todo:对切断字符串进行处理
                int count = 0;
                int match = 0;
                StringBuilder body = new StringBuilder();
                while ( count < formattedBody.Length )
                {
                    char c = formattedBody[count];
                    if ( c == '<' )
                        match += 1;
                    else if ( c == '>' )
                        match -= 1;
                    else if ( match == 0 )
                        body.Append( c );
                    if ( match <= 0 && body.Length >= length )
                        break;
                    count++;
                }
                return body.ToString() + "...";
                //return formattedBody.Substring ( 0, length ) + "..";
            }
        }

        /// <summary>
        /// 得到包含标记的子字符串（需整理）
        /// </summary>
        /// <param name="formattedBody"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GetFormattedString(string formattedBody, int length)
        {
            if ( string.IsNullOrEmpty( formattedBody ) )
            {
                return "...";
            }
            else if ( formattedBody.Length < length )
            {
                return formattedBody;
            }
            else
            {
                int totalCount = 0;
                int substring = 0;
                int match = 0;
                StringBuilder body = new StringBuilder();
                while ( totalCount < formattedBody.Length )
                {
                    char c = formattedBody[totalCount];
                    if ( c == '<' )
                        match += 1;
                    else if ( c == '>' )
                        match -= 1;
                    else if ( match == 0 )
                        substring++;
                    body.Append( c );
                    if ( match <= 0 && substring >= length )
                        break;
                    totalCount++;
                }
                return body.ToString() + "...";
                //return formattedBody.Substring ( 0, length ) + "..";
            }
        }

		/// <summary>
		/// 得到替换后的字符串
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public static string GetReplaceCaptionText(string name)
		{
			return name.Replace("\\", "\\\\").Replace("'", "\\'").Replace("\"", "&quot;");
		}
        ///// <summary>
        ///// （需整理）
        ///// </summary>
        ///// <param name="html"></param>
        ///// <returns></returns>
        //public static string GetReduceImageHtml( string html )
        //{
        //    string pattern = "<img.*src=\"([^\"]*)\"[^/]*/>";
        //    Regex regex = new Regex ( pattern, RegexOptions.IgnoreCase );
        //    html = regex.Replace ( html, "<img src=\"$1\" border=\"0\" onload=\"javascript:if(typeof BlogMaxImageSize != 'undefined') {if(this.width > BlogMaxImageSize) this.width= BlogMaxImageSize;} else {if(this.width > screen.width / 2 ) this.width=screen.width / 2;}\" alt=\"\"/>", -1 );
        //    return html;
        //}

        ///// <summary>
        ///// 得到摘要字符串包含Img标记，去掉其它标记（需整理）
        ///// </summary>
        ///// <param name="html"></param>
        ///// <returns></returns>
        //public static string GetSummaryHtml( string html )
        //{
        //    string pattern = @"\<img";
        //    Regex regex = new Regex ( pattern, RegexOptions.IgnoreCase );
        //    html = regex.Replace ( html, "♂", -1 );

        //    pattern = @"</p>";
        //    regex = new Regex ( pattern, RegexOptions.IgnoreCase );
        //    html = regex.Replace ( html, "/r/n", -1 );

        //    pattern = @"<br\s*/?>";
        //    regex = new Regex ( pattern, RegexOptions.IgnoreCase );
        //    html = regex.Replace ( html, "/r/n", -1 );

        //    pattern = @"(♂[^>]*)>";
        //    regex = new Regex ( pattern, RegexOptions.IgnoreCase );
        //    html = regex.Replace ( html, "$1♀", -1 );

        //    pattern = @"\<[^>]*>";
        //    regex = new Regex ( pattern, RegexOptions.IgnoreCase );
        //    html = regex.Replace ( html, "", -1 );

        //    pattern = @"♂";
        //    regex = new Regex ( pattern, RegexOptions.IgnoreCase );
        //    html = regex.Replace ( html, "<img", -1 );

        //    pattern = @"♀";
        //    regex = new Regex ( pattern, RegexOptions.IgnoreCase );
        //    html = regex.Replace ( html, "/>", -1 );

        //    pattern = "<img.*src=\"([^\"]*)\"[^/]*[/]?>";
        //    regex = new Regex ( pattern, RegexOptions.IgnoreCase );
        //    html = regex.Replace ( html, "<img src=\"$1\" alt=\"\"/>", -1 );

        //    //pattern = "<img.*src=\"([^\"]*)\"[^/]*/>";
        //    //regex = new Regex ( pattern, RegexOptions.IgnoreCase );
        //    //html = regex.Replace ( html, "<img src=\"$1\" border=\"0\" onload=\"javascript:if(typeof BlogMaxImageSize != 'undefined') {if(this.width > BlogMaxImageSize) this.width= BlogMaxImageSize;} else {if(this.width > screen.width / 2 ) this.width=screen.width / 2;}\" alt=\"\"/>", -1 );

        //    pattern = @"\r\n";
        //    regex = new Regex ( pattern, RegexOptions.IgnoreCase );
        //    html = regex.Replace ( html, "<br/>", -1 );

        //    pattern = @"\n";
        //    regex = new Regex ( pattern, RegexOptions.IgnoreCase );
        //    html = regex.Replace ( html, "<br/>", -1 );

        //    pattern = @"\r";
        //    regex = new Regex ( pattern, RegexOptions.IgnoreCase );
        //    html = regex.Replace ( html, "<br/>", -1 );

        //    pattern = @"//>";
        //    regex = new Regex ( pattern, RegexOptions.IgnoreCase );
        //    html = regex.Replace ( html, "/>", -1 );
        //    return html;
        //}

        /// <summary>
        /// 返回已经固定图片宽度的HTML
        /// </summary>
        /// <param name="html">HTML</param>
        /// <param name="maxImageWidth">最大宽度</param>
        /// <returns></returns>
        public static string GetFixupImageWidthHtml(string html, int maxImageWidth)
        {
			//用客户端脚本实现
			//string pattern = "<img";
			//Regex regex = new Regex( pattern, RegexOptions.IgnoreCase );
			//html = regex.Replace( html, "<img onload=\"javascript:if(this.width > " + maxImageWidth + ") this.width=" + maxImageWidth + ";\"", -1 );
            return html;
        }

        /// <summary>
        /// 截取一段HTML代码
        /// </summary>
        /// <param name="htmlString"></param>
        /// <param name="length">需要截取的长度</param>
		/// <param name="allowedHtmlTagType">允许的HTML标记类型</param>
        /// <returns></returns>
		public static string GetHtmlSubstring( string htmlString, int length, AllowedHtmlTagType allowedHtmlTagType )
        {
            if ( string.IsNullOrEmpty( htmlString ) )
                return string.Empty;
			Dictionary<string, string> allowedTags = AllowedHtmlTags.Current.GetAllowedHtmlTags ( allowedHtmlTagType );
			try
            {
                string html = htmlString.Replace( "&nbsp;", "♂" );
				string substringHtml = HtmlHelper.Substring ( Cubb ( html, true ), length, allowedTags );
				if ( substringHtml.Length < htmlString.Length )
					html = substringHtml + "...";
				else
					html = substringHtml;
                html = html.Replace( "♂", "&nbsp;" );
                return html;
            }
            catch ( Exception e )
            {
                throw( e );
            }
        }

		public static string GetHtmlSubstring( string htmlString, int length, bool ignoreTag )
		{
			if ( string.IsNullOrEmpty ( htmlString ) )
				return string.Empty;
			try
			{
				string html = htmlString.Replace ( "&nbsp;", "♂" );
				//todo:
				/*
			& 符号 & &amp; 
			单引号 ' &apos; 
			双引号 " &quot; 
			大于 > &gt; 
			小于 < &lt; 
			*/
				if ( ignoreTag )
				{
					html = GetCleanText ( html );
					html = GetSubString ( html, length, "..." );
				}
				else
				{
					//html = HtmlHelper.Clean ( html, false, true );
					//html = GetFormattedString( html, length );
					//html = HtmlHelper.MakeupTag( html );
					string substringHtml = HtmlHelper.Substring ( Cubb ( html, true ), length );
					if ( substringHtml.Length < htmlString.Length )
						html = substringHtml + "...";
					else
						html = substringHtml;
				}
				html = html.Replace ( "♂", "&nbsp;" );
				return html;
			}
			catch ( Exception e )
			{
				throw  e ;
				return htmlString;
			}
		}

        /// <summary>
        /// 针对用户输入的数据进行整理
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        public static string CleanInputString(string inputString)
        {
			if ( string.IsNullOrEmpty( inputString ) )
				return string.Empty;
            StringBuilder retVal = new StringBuilder();
                inputString = inputString.Trim();
                for ( int i = 0 ; i < inputString.Length ; i++ )
                {
                    switch ( inputString[i] )
                    {
                        case '"':
                            retVal.Append( "&quot;" );
                            break;
                        case '<':
                            retVal.Append( "&lt;" );
                            break;
                        case '>':
                            retVal.Append( "&gt;" );
                            break;
						case (char)14:
							retVal.Append(' ');
							break;
                        default:
                            retVal.Append( inputString[i] );
                            break;
                    }
                }
                retVal.Replace( "'", " " );
            return retVal.ToString();
        }

        /// <summary>
        /// 返回格式化后的字符串
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string GetParagraph(string content, int wordCount)
        {
            return ParagraphFormat( GetHtmlSubstring( content, wordCount, false ), "para", String.Empty );
        }

        /// <summary>
        /// 替换BR
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string CheckedString(string str)
        {
            str = Regex.Replace( str, "<b/>", "<b>", RegexOptions.IgnoreCase );
            int startPosition;
            int endPosition;
            for ( int i = 0 ; i < str.Length ; i++ )
            {
                startPosition = str.IndexOf( "<b>" );
                endPosition = str.IndexOf( "</b>" );
                if ( endPosition < startPosition && endPosition != -1 )
                {
                    str = str.Substring( 0, endPosition ) + str.Substring( endPosition + 4, str.Length - endPosition - 4 );
                    i = endPosition;
                    continue;
                }
                if ( startPosition != -1 && endPosition > startPosition )
                {
                    str = str.Substring( 0, startPosition ) + str.Substring( endPosition + 4, str.Length - endPosition - 4 );
                    i = startPosition;
                }
                else
                {
                    break;
                }
            }
            str = Regex.Replace( str, "<b>", "", RegexOptions.IgnoreCase );
            str = Regex.Replace( str, "</b>", "", RegexOptions.IgnoreCase );
            str = Regex.Replace( str, "<br>", "", RegexOptions.IgnoreCase );
            return str;
        }

        /// <summary>
        /// 实体编码
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static String Encode(String text)
        {
            if ( string.IsNullOrEmpty( text ) )
                return string.Empty;
			//string inputString = text.Replace( "&amp;", "&" ).Replace( "&", "&amp;" ).Replace( "&amp;nbsp;", "&nbsp;" ).Replace( "&amp;gt;", "&gt;" ).Replace( "&amp;lt;", "&lt;" ).Replace( "&amp;quot;", "&quot;" );
            StringBuilder sb = new StringBuilder();
			char [] chars = text.ToCharArray ();
            for ( int i = 0 ; i < chars.Length ; i++ )
            {
                if ( chars[i] > 127 )
                {
                    sb.Append( "&#x" );
                    sb.Append( ( (short)chars[i] ).ToString( "X" ) );
                    sb.Append( ";" );
                }
                else
                {
                    sb.Append( chars[i] );
                }
            }
            return sb.ToString();
        }

    	
		public static string BlockSpamEncode( string text )
		{
			if ( string.IsNullOrEmpty( text ) )
			{
				return string.Empty;
			}
			StringBuilder builder = new StringBuilder ();
			/*
			for ( int i = 0; i < text.Length; i++ )
			{
				if ( random.Next ( 2 ) == 1 )
				{
					builder.AppendFormat ( "&#{0};", Convert.ToInt32 ( text [i] ) );
				}
				else
				{
					builder.Append ( text [i] );
				}
			}
			 * */
			for ( int i = 0; i < text.Length; i++ )
			{
				char c = text[ i ];
				builder.Append( ScrambleStyle.Current.GetString ( c ) );
			}
			return builder.ToString ();
		}


        #region BBDecode

        #region UBB转换

        #region 将编码转换成HTML编码并用BR替换换行
        /// <summary>
        /// HTML转换
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string Chtml(string content)
        {
            string a = content;

            if ( a != string.Empty )
            {
                a = HttpUtility.HtmlEncode( a );
                a = a.Replace( "\n", "<br />" );

            }

            return a;
        }
        #endregion

        #region 表情转换
        /// <summary>
        /// 表情转换
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string Cemotion(string content)
        {
            string a = content;

            if ( a != string.Empty )
            {
                Regex r = new Regex( @"\[em(\d{1,3})\]" );

                //Emotion
                a = r.Replace( a, "<img src=\"http://www.e-jjj.com/i/emotion/em$1.gif\" border=\"0\" alt=\"表情图片\" align=\"middle\" />" );

                //Common
                a = a.Replace( "8-)", "<img src=\"http://www.e-jjj.com/i/emotion/em5.gif\" border=\"0\" alt=\"表情图片\" align=\"middle\" />" );
                a = a.Replace( ":-)", "<img src=\"http://www.e-jjj.com/i/emotion/em63.gif\" border=\"0\" alt=\"表情图片\" align=\"middle\" />" );
                a = a.Replace( ":)", "<img src=\"http://www.e-jjj.com/i/emotion/em63.gif\" border=\"0\" alt=\"表情图片\" align=\"middle\" />" );
                a = a.Replace( "：）", "<img src=\"http://www.e-jjj.com/i/emotion/em63.gif\" border=\"0\" alt=\"表情图片\" align=\"middle\" />" );
                a = a.Replace( ":'(", "<img src=\"http://www.e-jjj.com/i/emotion/em35.gif\" border=\"0\" alt=\"表情图片\" align=\"middle\" />" );
            }

            return a;
        }
        #endregion
        /// <summary>
        /// UBB转换
        /// </summary>
        /// <param name="content"></param>
        /// <param name="isconvertemotion">指定是否转换表情</param>
        /// <returns></returns>
        public static string Cubb(string content, bool isconvertemotion)
        {
            string a = content;

            if ( a != string.Empty )
            {
                Regex r;

                //屏蔽JS
                r = new Regex( @"javascript", RegexOptions.IgnoreCase );
                a = r.Replace( a, "javascript" );
                r = new Regex( @"jscript:", RegexOptions.IgnoreCase );
                a = r.Replace( a, "jscript:" );
                r = new Regex( @"js:", RegexOptions.IgnoreCase );
                a = r.Replace( a, "js:" );
                r = new Regex( @"value", RegexOptions.IgnoreCase );
                a = r.Replace( a, "value" );
                r = new Regex( @"about:", RegexOptions.IgnoreCase );
                a = r.Replace( a, "about:" );
                r = new Regex( @"file:", RegexOptions.IgnoreCase );
                a = r.Replace( a, "file:" );
                r = new Regex( @"document.cookie", RegexOptions.IgnoreCase );
                a = r.Replace( a, "document.cookie" );
                r = new Regex( @"vbscript:", RegexOptions.IgnoreCase );
                a = r.Replace( a, "vbscript:" );
                r = new Regex( @"vbs:", RegexOptions.IgnoreCase );
                a = r.Replace( a, "vbs:" );
                r = new Regex( @"(on(mouse|exit|error|click|key))", RegexOptions.IgnoreCase );
                a = r.Replace( a, "on$2" );
                r = new Regex( @"script", RegexOptions.IgnoreCase );
                a = r.Replace( a, "script" );

                //[b][/b]转换
                r = new Regex( @"(\[b\])(.+?)(\[\/b\])", RegexOptions.IgnoreCase );
                a = r.Replace( a, "<b>$2</b>" );

                //[i][/i]转换
                r = new Regex( @"\[i\](.+?)\[/i\]", RegexOptions.IgnoreCase );
                a = r.Replace( a, "<i>$1</i>" );

                //[u][/u]转换
                r = new Regex( @"\[u\](.+?)\[/u\]", RegexOptions.IgnoreCase );
                a = r.Replace( a, "<u>$1</u>" );

                //[s][/s]转换
                r = new Regex( @"\[s\](.+?)\[/s\]", RegexOptions.IgnoreCase );
                a = r.Replace( a, "<s>$1</s>" );

                //[center][/center]转换
                r = new Regex( @"\[center\](.+?)\[/center\]", RegexOptions.IgnoreCase );
                a = r.Replace( a, "<div align=\"center\">$1</div>" );

                //[color=][/color]转换
                r = new Regex( @"\[color=(.+?)\](.+?)\[\/color\]", RegexOptions.IgnoreCase );
                a = r.Replace( a, "<font color=\"$1\">$2</font>" );

                //[size=][/size]转换
                r = new Regex( @"(\[size=1\])(.+?)(\[\/size\])", RegexOptions.IgnoreCase );
                a = r.Replace( a, "<font size=\"1\">$2</font>" );

                r = new Regex( @"(\[size=2\])(.+?)(\[\/size\])", RegexOptions.IgnoreCase );
                a = r.Replace( a, "<font size=\"2\">$2</font>" );

                r = new Regex( @"(\[size=3\])(.+?)(\[\/size\])", RegexOptions.IgnoreCase );
                a = r.Replace( a, "<font size=\"3\">$2</font>" );

                r = new Regex( @"(\[size=4\])(.+?)(\[\/size\])", RegexOptions.IgnoreCase );
                a = r.Replace( a, "<font size=\"4\">$2</font>" );

                r = new Regex( @"(\[size=5\])(.+?)(\[\/size\])", RegexOptions.IgnoreCase );
                a = r.Replace( a, "<font size=\"5\">$2</font>" );

                r = new Regex( @"(\[size=6\])(.+?)(\[\/size\])", RegexOptions.IgnoreCase );
                a = r.Replace( a, "<font size=\"6\">$2</font>" );

                //自动URL解析
                /*
                r = new Regex ( @"((?<prefix>\]|=)?(?<theurl>(http|https|ftp)\://([a-zA-Z0-9\.\-]+(\:[a-zA-Z0-9\.&%\$\-]+)*@)*((25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9])\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[0-9])|localhost|([a-zA-Z0-9\-]+\.)*[a-zA-Z0-9\-]+\.(com|edu|gov|int|mil|net|org|biz|arpa|info|name|pro|aero|coop|museum|[a-zA-Z]{2}))(\:[0-9]+)*(/($|[a-zA-Z0-9\.\,\?\'\\\+&%\$#\=~_\-]+))*))" );
                MatchCollection mc = r.Matches ( a );

                int i = 0;

                foreach ( Match m in mc )
                {
                    if ( m.Success && m.Groups ["prefix"].ToString ().ToLower () != "]" && m.Groups ["prefix"].ToString ().ToLower () != "=" )
                    {
                        a = a.Insert ( m.Index + i, "[url]" );
                        a = a.Insert ( m.Index + m.Length + i + 5, "[/url]" );
                        i += 11;
                    }
                }
                */

                //[url][/url]转换
                r = new Regex( @"\[url\](?<theurl>.+?)\[\/url\]", RegexOptions.IgnoreCase );
                a = r.Replace( a, "<a href=\"${theurl}\" class=\"fc1 ul\" target=\"_blank\">${theurl}</a>" );

                //[url=][/url]转换
                r = new Regex( @"\[url=(?<theurl>.+?)\](?<theurl2>.+?)\[\/url\]", RegexOptions.IgnoreCase );
                a = r.Replace( a, "<a href=\"${theurl}\" class=\"fc1 ul\" target=\"_blank\">${theurl2}</a>" );

                //[email][/email]转换
                r = new Regex( @"\[email\](.?)\[\/email\]", RegexOptions.IgnoreCase );
                a = r.Replace( a, "<a href=\"mailto:$1\" class=\"fc1 ul\">$1</a>" );

                //[email=][/email]转换
                r = new Regex( @"\[email=(.+?)\](.+?)\[\/email\]", RegexOptions.IgnoreCase );
                a = r.Replace( a, "<a href=\"mailto:$1\" class=\"fc1 ul\">$2</a>" );

                //[img][/img]转换
                r = new Regex( @"(\[img\])(.+?)(\[\/img\])", RegexOptions.IgnoreCase );
                a = r.Replace( a, "<img src=\"$2\" border=\"0\" onload=\"javascript:if(typeof MaxImageSize != 'undefined') {if(this.width > MaxImageSize) this.width= MaxImageSize;} else {if(this.width > screen.width / 2 ) this.width=screen.width / 2;}\">" );

                //[img=宽度][/img]转换
                r = new Regex( @"(\[img=(\d+)\])(.+?)(\[\/img\])", RegexOptions.IgnoreCase );
                a = r.Replace( a, "<img src=\"$3\" border=\"0\" width=\"$2\">" );

                //[limg][/limg]转换
                r = new Regex( @"(\[limg\])(.+?)(\[\/limg\])", RegexOptions.IgnoreCase );
                a = r.Replace( a, "<img src=\"$2\" border=\"0\" align=\"left\" onload=\"javascript:if(typeof MaxImageSize != 'undefined') {if(this.width > MaxImageSize) this.width= MaxImageSize;} else {if(this.width > screen.width / 2 ) this.width=screen.width / 2;}\">" );

                //[limg=宽度][/limg]转换
                r = new Regex( @"(\[limg=(\d+)\])(.+?)(\[\/limg\])", RegexOptions.IgnoreCase );
                a = r.Replace( a, "<img src=\"$3\" border=\"0\" width=\"$2\" align=\"left\">" );

                //[rimg][/rimg]转换
                r = new Regex( @"(\[rimg\])(.+?)(\[\/rimg\])", RegexOptions.IgnoreCase );
                a = r.Replace( a, "<img src=\"$2\" border=\"0\" align=\"right\" onload=\"javascript:if(typeof MaxImageSize != 'undefined') {if(this.width > MaxImageSize) this.width= MaxImageSize;} else {if(this.width > screen.width / 2 ) this.width=screen.width / 2;}\">" );

                //[rimg=宽度][/rimg]转换
                r = new Regex( @"(\[rimg=(\d+)\])(.+?)(\[\/rimg\])", RegexOptions.IgnoreCase );
                a = r.Replace( a, "<img src=\"$3\" border=\"0\" width=\"$2\" align=\"right\">" );

                //[code][/code]转换
                r = new Regex( @"\[code\](.+?)\[\/code\]", RegexOptions.IgnoreCase );
                a = r.Replace( a, "<table width=\"80%\" border=\"0\" cellpadding=\"10\" cellspacing=\"0\" bgcolor=\"#FFFFFF\" style=\"border:1px solid #000000;margin:10px;\"><tr><td><font face=\"Courier new\" color=\"black\">$1</font></td></tr></table>" );

                //[face=][/face]转换
                r = new Regex( @"(\[face=(.+?)\])(.+?)(\[\/face\])", RegexOptions.IgnoreCase );
                a = r.Replace( a, "<font face=\"$2\">$3</font>" );

                //[fly][/fly]转换
                r = new Regex( @"\[fly\](.+?)\[\/fly\]", RegexOptions.IgnoreCase );
                a = r.Replace( a, "<marquee width=\"100%\" behavior=\"alternate\" scrollamount=\"3\">$1</marquee>" );

                //[move][/move]转换
                r = new Regex( @"(\[move\])(.+?)(\[\/move\])", RegexOptions.IgnoreCase );
                a = r.Replace( a, "<marquee width=\"100%\" scrollamount=\"3\">$2</marquee>" );

                //[swf=num,num][/swf]转换
                r = new Regex( @"(\[swf=(\d+),(\d+)\])(.+?)(\[\/swf\])", RegexOptions.IgnoreCase );
                a = r.Replace( a, "<OBJECT codeBase=http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=4,0,2,0 classid=clsid:D27CDB6E-AE6D-11cf-96B8-444553540000 width=$2 height=$3><PARAM NAME=movie VALUE=\"$4\"><PARAM NAME=quality VALUE=high><embed src=\"$4\" quality=high pluginspage='http://www.macromedia.com/shockwave/download/index.cgi?P1_Prod_Version=ShockwaveFlash' type='application/x-shockwave-flash' width=$2 height=$3>$4</embed></OBJECT><br />" );

                //[dir=num,num][/dir]转换
                r = new Regex( @"(\[dir=(\d+),(\d+)\])(.+?)(\[\/dir\])", RegexOptions.IgnoreCase );
                a = r.Replace( a, "<OBJECT classid=clsid:166B1BCA-3F9C-11CF-8075-444553540000 codebase=http://download.macromedia.com/pub/shockwave/cabs/director/sw.cab width=$2 height=$3><param name=src value=\"$4\"><embed src=\"$4\" pluginspage=http://www.macromedia.com/shockwave/download/ width=$2 height=$3></embed></OBJECT><br />" );

                //[rm=num,num][/rm]转换
                r = new Regex( @"(\[rm=(\d+),(\d+)\])(.+?)(\[\/rm\])", RegexOptions.IgnoreCase );
                a = r.Replace( a, "<object classid=clsid:CFCDAA03-8BE4-11cf-B84B-0020AFBBCCFA class=object id=RAOCX width=$2 height=$3><PARAM NAME=SRC value=\"$4\"><PARAM NAME=CONSOLE VALUE=2004280084352954><PARAM NAME=CONTROLS VALUE=imagewindow><PARAM NAME=AUTOSTART VALUE=true></object><br><object classid=clsid:CFCDAA03-8BE4-11CF-B84B-0020AFBBCCFA height=32 id=video2 width=$2><PARAM NAME=SRC value=\"$4\"><PARAM NAME=AUTOSTART VALUE=-1><PARAM NAME=CONTROLS VALUE=controlpanel><PARAM NAME=CONSOLE VALUE=2004280084352954></object><br />" );

                //[rm=num,num:y][/rm]转换
                r = new Regex( @"(\[rm=(\d+),(\d+)\:y\])(.+?)(\[\/rm\])", RegexOptions.IgnoreCase );
                a = r.Replace( a, "<object classid=clsid:CFCDAA03-8BE4-11cf-B84B-0020AFBBCCFA class=object id=RAOCX width=$2 height=$3><PARAM NAME=SRC value=\"$4\"><PARAM NAME=CONSOLE VALUE=2004280084352954><PARAM NAME=CONTROLS VALUE=imagewindow><PARAM NAME=AUTOSTART VALUE=true></object><br><object classid=clsid:CFCDAA03-8BE4-11CF-B84B-0020AFBBCCFA height=32 id=video2 width=$2><PARAM NAME=SRC value=\"$4\"><PARAM NAME=AUTOSTART VALUE=-1><PARAM NAME=CONTROLS VALUE=controlpanel><PARAM NAME=CONSOLE VALUE=2004280084352954></object><br />" );

                //[rm=num,num:n][/rm]转换
                r = new Regex( @"(\[rm=(\d+),(\d+)\:n\])(.+?)(\[\/rm\])", RegexOptions.IgnoreCase );
                a = r.Replace( a, "<object classid=clsid:CFCDAA03-8BE4-11cf-B84B-0020AFBBCCFA class=object id=RAOCX width=$2 height=$3><PARAM NAME=SRC value=\"$4\"><PARAM NAME=CONSOLE VALUE=2004280084352954><PARAM NAME=CONTROLS VALUE=imagewindow><PARAM NAME=AUTOSTART VALUE=false></object><br><object classid=clsid:CFCDAA03-8BE4-11CF-B84B-0020AFBBCCFA height=32 id=video2 width=$2><PARAM NAME=SRC value=\"$4\"><PARAM NAME=AUTOSTART VALUE=0><PARAM NAME=CONTROLS VALUE=controlpanel><PARAM NAME=CONSOLE VALUE=2004280084352954></object><br />" );

                //[mp=num,num][/mp]转换
                r = new Regex( @"(\[mp=(\d+),(\d+)\])(.+?)(\[\/mp\])", RegexOptions.IgnoreCase );
                a = r.Replace( a, "<object align=middle classid=clsid:22d6f312-b0f6-11d0-94ab-0080c74c7e95 class=OBJECT id=MediaPlayer width=$2 height=$3><param name=ShowStatusBar value=-1><param name=Filename value=\"$4\"><param name=AutoStart value=1><embed type=application/x-oleobject codebase=http://activex.microsoft.com/activex/controls/mplayer/en/nsmp2inf.cab#Version=5,1,52,701 flename=mp src=\"$4\" width=$2 height=$3></embed></object><br />" );

                //[mp=num,num:y][/mp]转换
                r = new Regex( @"(\[mp=(\d+),(\d+)\:y\])(.+?)(\[\/mp\])", RegexOptions.IgnoreCase );
                a = r.Replace( a, "<object align=middle classid=clsid:22d6f312-b0f6-11d0-94ab-0080c74c7e95 class=OBJECT id=MediaPlayer width=$2 height=$3><param name=ShowStatusBar value=-1><param name=Filename value=\"$4\"><param name=AutoStart value=1><embed type=application/x-oleobject codebase=http://activex.microsoft.com/activex/controls/mplayer/en/nsmp2inf.cab#Version=5,1,52,701 flename=mp src=\"$4\" width=$2 height=$3></embed></object><br />" );

                //[mp=num,num:n][/mp]转换
                r = new Regex( @"(\[mp=(\d+),(\d+)\:n\])(.+?)(\[\/mp\])", RegexOptions.IgnoreCase );
                a = r.Replace( a, "<object align=middle classid=clsid:22d6f312-b0f6-11d0-94ab-0080c74c7e95 class=OBJECT id=MediaPlayer width=$2 height=$3><param name=ShowStatusBar value=-1><param name=Filename value=\"$4\"><param name=AutoStart value=0><embed type=application/x-oleobject codebase=http://activex.microsoft.com/activex/controls/mplayer/en/nsmp2inf.cab#Version=5,1,52,701 flename=mp src=\"$4\" width=$2 height=$3></embed></object><br />" );

                //[qt=num,num][/qt]转换
                r = new Regex( @"(\[qt=(\d+),(\d+)\])(.+?)(\[\/qt\])", RegexOptions.IgnoreCase );
                a = r.Replace( a, "<embed src=\"$4\" width=$2 height=$3 autoplay=true loop=false controller=true playeveryframe=false cache=false scale=TOFIT bgcolor=#000000 kioskmode=false targetcache=false pluginspage=http://www.apple.com/quicktime/><br />" );

                //[qt=num,num:y][/qt]转换
                r = new Regex( @"(\[qt=(\d+),(\d+)\:y\])(.+?)(\[\/qt\])", RegexOptions.IgnoreCase );
                a = r.Replace( a, "<embed src=\"$4\" width=$2 height=$3 autoplay=true loop=false controller=true playeveryframe=false cache=false scale=TOFIT bgcolor=#000000 kioskmode=false targetcache=false pluginspage=http://www.apple.com/quicktime/><br />" );

                //[qt=num,num:n][/qt]转换
                r = new Regex( @"(\[qt=(\d+),(\d+)\:n\])(.+?)(\[\/qt\])", RegexOptions.IgnoreCase );
                a = r.Replace( a, "<embed src=\"$4\" width=$2 height=$3 autoplay=false loop=false controller=true playeveryframe=false cache=false scale=TOFIT bgcolor=#000000 kioskmode=false targetcache=false pluginspage=http://www.apple.com/quicktime/><br />" );



                #region [movie][/movie]转换

                r = new Regex( @"(\[movie\])(.+?)(\[\/movie\])", RegexOptions.IgnoreCase );
                MatchCollection Matches = r.Matches( a );

                foreach ( Match m in Matches )
                {
                    if ( m.Groups[0].Value.ToLower().StartsWith( "[movie]http://www.allmov.com/exec/movjs.aspx?" ) )
                    {
                        string oldString = m.Groups[0].Value;
                        string newString = r.Replace( oldString, "<script type=\"text/javascript\" src=\"$2\"></script>" );
                        a = a.Replace( oldString, newString );
                    }
                }

                #endregion

                if ( isconvertemotion )
                {
                    a = Cemotion( a );
                }
            }

            return a;
        }
        #endregion

        /*
		/// <summary>
		/// 转换UBB代码
		/// </summary>
		/// <param name="encodedString"></param>
		/// <returns></returns>
		public static string UBBcodeToHtml( string encodedString )
		{
			/*
			string quoteStartHtml = "";
			string quoteEndHtml = "</div></BLOCKQUOTE>";
			string emptyquoteStartHtml = "<BLOCKQUOTE><div>";
			string emptyquoteEndHtml = "</div></BLOCKQUOTE>";


			RegexOptions options = RegexOptions.IgnoreCase | RegexOptions.Compiled;
			//color
			encodedString = Regex.Replace ( encodedString, @"\[color=(.+?)\](.+?)\[\/color\]", "<font color=\"$1\">$2</font>", options );
			// Bold, Italic, Underline
			//
			encodedString = Regex.Replace ( encodedString, @"\[b(?:\s*)\]((.|\n)*?)\[/b(?:\s*)\]", "<b>$1</b>", options );
			encodedString = Regex.Replace ( encodedString, @"\[i(?:\s*)\]((.|\n)*?)\[/i(?:\s*)\]", "<i>$1</i>", options );
			encodedString = Regex.Replace ( encodedString, @"\[u(?:\s*)\]((.|\n)*?)\[/u(?:\s*)\]", "<u>$1</u>", options );

			// Left, Right, Center
			encodedString = Regex.Replace ( encodedString, @"\[left(?:\s*)\]((.|\n)*?)\[/left(?:\s*)]", "<div style=\"text-align:left\">$1</div>", options );
			encodedString = Regex.Replace ( encodedString, @"\[center(?:\s*)\]((.|\n)*?)\[/center(?:\s*)]", "<div style=\"text-align:center\">$1</div>", options );
			encodedString = Regex.Replace ( encodedString, @"\[right(?:\s*)\]((.|\n)*?)\[/right(?:\s*)]", "<div style=\"text-align:right\">$1</div>", options );

			// Quote
			//
			//encodedString = Regex.Replace(encodedString, "\\[quote(?:\\s*)user=\"((.|\n)*?)\"\\]((.|\n)*?)\\[/quote(\\s*)\\]", quote, options);
			//encodedString = Regex.Replace(encodedString, "\\[quote(\\s*)\\]((.|\n)*?)\\[/quote(\\s*)\\]", emptyquote, options);
			encodedString = Regex.Replace ( encodedString, "\\[quote(?:\\s*)user=(?:\"|&quot;|&#34;)(.*?)(?:\"|&quot;|&#34;)\\]", quoteStartHtml, options );
			encodedString = Regex.Replace ( encodedString, "\\[/quote(\\s*)\\]", quoteEndHtml, options );
			encodedString = Regex.Replace ( encodedString, "\\[quote(\\s*)\\]", emptyquoteStartHtml, options );
			encodedString = Regex.Replace ( encodedString, "\\[/quote(\\s*)\\]", emptyquoteEndHtml, options );

			// Anchors
			//
			encodedString = Regex.Replace ( encodedString, @"\[url(?:\s*)\]www\.(.*?)\[/url(?:\s*)\]", "<a href=\"http://www.$1\" target=\"_blank\" title=\"$1\">$1</a>", options );
			encodedString = Regex.Replace ( encodedString, @"\[url(?:\s*)\]((.|\n)*?)\[/url(?:\s*)\]", "<a href=\"$1\" target=\"_blank\" title=\"$1\">$1</a>", options );
			encodedString = Regex.Replace ( encodedString, @"\[url=(?:""|&quot;|&#34;)((.|\n)*?)(?:\s*)(?:""|&quot;|&#34;)\]((.|\n)*?)\[/url(?:\s*)\]", "<a href=\"$1\" target=\"_blank\" title=\"$1\">$3</a>", options );
			encodedString = Regex.Replace ( encodedString, @"\[url=((.|\n)*?)(?:\s*)\]((.|\n)*?)\[/url(?:\s*)\]", "<a href=\"$1\" target=\"_blank\" title=\"$1\">$3</a>", options );
			encodedString = Regex.Replace ( encodedString, @"\[link(?:\s*)\]((.|\n)*?)\[/link(?:\s*)\]", "<a href=\"$1\" target=\"_blank\" title=\"$1\">$1</a>", options );
			encodedString = Regex.Replace ( encodedString, @"\[link=((.|\n)*?)(?:\s*)\]((.|\n)*?)\[/link(?:\s*)\]", "<a href=\"$1\" target=\"_blank\" title=\"$1\">$3</a>", options );

			// Image
			//
			encodedString = Regex.Replace ( encodedString, @"\[img(?:\s*)\]((.|\n)*?)\[/img(?:\s*)\]", "<img src=\"$1\" border=\"0\" />", options );
			encodedString = Regex.Replace ( encodedString, @"\[img=((.|\n)*?)x((.|\n)*?)(?:\s*)\]((.|\n)*?)\[/img(?:\s*)\]", "<img width=\"$1\" height=\"$3\" src=\"$5\" border=\"0\" />", options );
			encodedString = Regex.Replace ( encodedString, @"(\[img=(\d+)\])(.+?)(\[\/img\])", "<img src=\"$3\" border=\"0\" width=\"$2\">", options );

			// Color
			//
			encodedString = Regex.Replace ( encodedString, @"\[color=((.|\n)*?)(?:\s*)\]((.|\n)*?)\[/color(?:\s*)\]", "<span style=\"color=$1;\">$3</span>", options );

			// Horizontal Rule
			//
			encodedString = Regex.Replace ( encodedString, @"\[hr(?:\s*)\]", "<hr />", options );

			// Email
			//
			encodedString = Regex.Replace ( encodedString, @"\[email(?:\s*)\]((.|\n)*?)\[/email(?:\s*)\]", "<a href=\"mailto:$1\">$1</a>", options );

			// Font size
			//
			encodedString = Regex.Replace ( encodedString, @"\[size=((.|\n)*?)(?:\s*)\]((.|\n)*?)\[/size(?:\s*)\]", "<span style=\"font-size:$1\">$3</span>", options );
			encodedString = Regex.Replace ( encodedString, @"\[font=((.|\n)*?)(?:\s*)\]((.|\n)*?)\[/font(?:\s*)\]", "<span style=\"font-family:$1;\">$3</span>", options );
			encodedString = Regex.Replace ( encodedString, @"\[align=((.|\n)*?)(?:\s*)\]((.|\n)*?)\[/align(?:\s*)\]", "<div style=\"text-align:$1;\">$3</span>", options );
			encodedString = Regex.Replace ( encodedString, @"\[float=((.|\n)*?)(?:\s*)\]((.|\n)*?)\[/float(?:\s*)\]", "<div style=\"float:$1;\">$3</div>", options );

			string sListFormat = "<ol class=\"anf_list\" style=\"list-style:{0};\">$1</ol>";
			// Lists
			encodedString = Regex.Replace ( encodedString, @"\[\*(?:\s*)]\s*([^\[]*)", "<li>$1</li>", options );
			encodedString = Regex.Replace ( encodedString, @"\[list(?:\s*)\]((.|\n)*?)\[/list(?:\s*)\]", "<ul class=\"anf_list\">$1</ul>", options );
			encodedString = Regex.Replace ( encodedString, @"\[list=1(?:\s*)\]((.|\n)*?)\[/list(?:\s*)\]", string.Format ( sListFormat, "decimal" ), options );
			encodedString = Regex.Replace ( encodedString, @"\[list=i(?:\s*)\]((.|\n)*?)\[/list(?:\s*)\]", string.Format ( sListFormat, "lower-roman" ), RegexOptions.Compiled );
			encodedString = Regex.Replace ( encodedString, @"\[list=I(?:\s*)\]((.|\n)*?)\[/list(?:\s*)\]", string.Format ( sListFormat, "upper-roman" ), RegexOptions.Compiled );
			encodedString = Regex.Replace ( encodedString, @"\[list=a(?:\s*)\]((.|\n)*?)\[/list(?:\s*)\]", string.Format ( sListFormat, "lower-alpha" ), RegexOptions.Compiled );
			encodedString = Regex.Replace ( encodedString, @"\[list=A(?:\s*)\]((.|\n)*?)\[/list(?:\s*)\]", string.Format ( sListFormat, "upper-alpha" ), RegexOptions.Compiled );

			return encodedString;
		}
			 * 			 * */

        #endregion
        #region 后台公共方法
        //影片文字信息解析结构定义
        public struct SynopsisStruct
        {
            public string Source;
            public string SourceLink;
            public string Content;
            public string EnterUser;
            public string EnterUserNickName;
            public string ApproveUser;
            public string ApproveTime;
            public string ApprovePower;
        }
        //影片文字信息结构解析方法
        public static SynopsisStruct GetSynopsis(string content)
        {
            //文字信息正则定义
            const string Source = "source";
            const string SourceLink = "sourcelink";
            const string Content = "content";
            string ParagraphOldRegex = @"(?:^<Synopsis><Source>)(?<source>[\w\W]*?)(?:</Source>" + Environment.NewLine + @"<Content>)(?<content>[\w\W]*?)(?:</Content>" + Environment.NewLine + @"</Synopsis>$)";
            string ParagraphRegex = @"(?:^<Synopsis><Source>)(?<source>[\w\W]*?)(?:</Source><SourceLink>)(?<sourcelink>[\w\W]*?)(?:</SourceLink><Content>)(?<content>[\w\W]*)(?:</Content></Synopsis>$)";
            //结构初始化
            SynopsisStruct synopsis = new SynopsisStruct();
            synopsis.Source = string.Empty;
            synopsis.SourceLink = string.Empty;
            synopsis.Content = string.Empty;
            //优先匹配二期结构，失败后匹配一期结构，若失败取原始内容
            if ( content != null && content != string.Empty )
            {
                MatchCollection Contents = Regex.Matches( content, ParagraphRegex, RegexOptions.None );
                if ( Contents.Count > 0 )
                {
                    synopsis.Source = Contents[0].Groups[Source].Value.Trim();
                    synopsis.SourceLink = Contents[0].Groups[SourceLink].Value.Trim();
                    synopsis.Content = Contents[0].Groups[Content].Value;
                }
                else
                {
                    //兼容一期后台的数据
                    Contents = Regex.Matches( content, ParagraphOldRegex, RegexOptions.None );
                    if ( Contents.Count > 0 )
                    {
                        synopsis.Source = Contents[0].Groups[Source].Value.Trim();
                        synopsis.Content = Contents[0].Groups[Content].Value;
                    }
                    else
                    {
                        synopsis.Content = content;
                    }
                }
            }
            return synopsis;
        }
        //影人传记结构解析方法
        public static SynopsisStruct[] GetBiography(string content)
        {
            //传记正则定义
            const string Source = "source";
            const string SourceLink = "sourcelink";
            const string Content = "content";
            const string EnterUser = "enteruser";
            const string EnterUserNickName = "enterusernickname";
            const string ApproveUser = "approveuser";
            const string ApproveTime = "approvetime";
            const string ApprovePower = "approvepower";
            
            string ParagraphRegex = @"(?:<Bio><Source>)(?<source>[\w\W]*?)(?:</Source><SourceLink>)(?<sourcelink>[\w\W]*?)(?:</SourceLink><Content>)(?<content>[\w\W]*?)(?:</Content></Bio>)";
            string ParagraphRegexV3 = @"(?:<Bio><Source>)(?<source>[\w\W]*?)(?:</Source><SourceLink>)(?<sourcelink>[\w\W]*?)(?:</SourceLink><Content>)(?<content>[\w\W]*?)(?:</Content>" +
                @"<EnterUser>)(?<enteruser>[\w\W]*?)(?:</EnterUser><EnterUserNickName>)(?<enterusernickname>[\w\W]*?)(?:</EnterUserNickName><ApproveUser>)(?<approveuser>[\w\W]*?)(?:</ApproveUser><ApproveTime>)(?<approvetime>[\w\W]*?)(?:</ApproveTime><ApprovePower>)(?<approvepower>[\w\W]*?)(?:</ApprovePower></Bio>)";
            
            bool isMatchSuccess = false;
            SynopsisStruct[] biography = null;
            
            if (!string.IsNullOrEmpty(content))
            {
                //优先匹配三期结构
                MatchCollection Contents = Regex.Matches(content, ParagraphRegexV3);
                int count = Contents.Count;
                if (count > 0)
                {
                    isMatchSuccess = true;
                    biography = new SynopsisStruct[count];
                    for (int i = 0; i < count; i++)
                    {
                        biography[i].Source = Contents[i].Groups[Source].Value.Trim();
                        biography[i].SourceLink = Contents[i].Groups[SourceLink].Value.Trim();
                        biography[i].Content = Contents[i].Groups[Content].Value;
                        biography[i].EnterUser = Contents[i].Groups[EnterUser].Value;
                        biography[i].EnterUserNickName = Contents[i].Groups[EnterUserNickName].Value;
                        biography[i].ApproveUser = Contents[i].Groups[ApproveUser].Value;
                        biography[i].ApproveTime = Contents[i].Groups[ApproveTime].Value;
                        biography[i].ApprovePower = Contents[i].Groups[ApprovePower].Value;
                    }
                }
                else
                {
                    //匹配二期结构
                    Contents = Regex.Matches(content, ParagraphRegex);
                    count = Contents.Count;
                    if (count > 0)
                    {
                        isMatchSuccess = true;
                        biography = new SynopsisStruct[count];
                        for (int i = 0; i < count; i++)
                        {
                            biography[i].Source = Contents[i].Groups[Source].Value.Trim();
                            biography[i].SourceLink = Contents[i].Groups[SourceLink].Value.Trim();
                            biography[i].Content = Contents[i].Groups[Content].Value;
                            biography[i].EnterUser = string.Empty;
                            biography[i].EnterUserNickName = string.Empty;
                            biography[i].ApproveUser = string.Empty;
                            biography[i].ApproveTime = string.Empty;
                            biography[i].ApprovePower = string.Empty;
                        }
                    }
                }
            }
            if ( !isMatchSuccess )
            {
                //取原始内容
                biography = new SynopsisStruct[1];
                biography[0].Source = string.Empty;
                biography[0].SourceLink = string.Empty;
                biography[0].Content = content == null ? string.Empty : content;
                biography[0].EnterUser = string.Empty;
                biography[0].EnterUserNickName = string.Empty;
                biography[0].ApproveUser = string.Empty;
                biography[0].ApproveTime = string.Empty;
                biography[0].ApprovePower = string.Empty;
            }
            return biography;
        }
        #endregion

        public static string GetSourceLink(SynopsisStruct synopsisStruct)
        {
            if ( !string.IsNullOrEmpty( synopsisStruct.Source ) )
            {
                if ( !string.IsNullOrEmpty( synopsisStruct.SourceLink ) )
                {
                    return "<p class=\"tr\"> —— <a target=\"_blank\" href=\"" + synopsisStruct.SourceLink + "\">" + synopsisStruct.Source + "</a></p>";
                }
                else
                {
                    return "<p class=\"tr\"> —— " + synopsisStruct.Source + "</p>";
                }
            }
            return string.Empty;
        }

        #region 格式化时间
        /// <summary>
        /// 格式化时间
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string FormatDateTime( DateTime dateTime )
        {
			return dateTime.ToString ( "yyyy-MM-dd HH:mm" );
        }
        #endregion

		public static string TrimRight( string inputString, string removeString )
		{
			if ( inputString.EndsWith ( removeString, StringComparison.CurrentCultureIgnoreCase ) )
				return inputString.TrimEnd ( removeString.ToCharArray () );
			return inputString;
		}
		
		/// <summary>
		/// MD5 encodes the passed string
		/// </summary>
		/// <param name="input">The string to encode.</param>
		/// <returns>An encoded string.</returns>
		public static string MD5String( string input )
		{
			// Create a new instance of the MD5CryptoServiceProvider object.
			MD5 md5Hasher = MD5.Create ();

			// Convert the input string to a byte array and compute the hash.
			byte [] data = md5Hasher.ComputeHash ( Encoding.Default.GetBytes ( input ) );

			// Create a new Stringbuilder to collect the bytes
			// and create a string.
			StringBuilder sBuilder = new StringBuilder ();

			// Loop through each byte of the hashed data 
			// and format each one as a hexadecimal string.
			for ( int i = 0; i < data.Length; i++ )
			{
				sBuilder.Append ( data [i].ToString ( "x2" ) );
			}

			// Return the hexadecimal string.
			return sBuilder.ToString ();
		}


		public static IList CombinationArrayList( string delimitedString, string delimiter, System.Type baseType )
		{
			ArrayList list = new ArrayList ();
			string [] inputStrings = delimitedString.Split ( delimiter.ToCharArray () );
			foreach ( string inputString in inputStrings )
			{
				if ( baseType.Equals ( typeof ( string ) ) )
					list.Add ( inputString );
				if ( baseType.Equals ( typeof ( int ) ) )
					list.Add ( Convert.ToInt32 ( inputString ) );
			}
			if ( baseType.Equals ( typeof ( int ) ) )
				return ( int [] ) list.ToArray ( typeof ( int ) );
			if ( baseType.Equals ( typeof ( string ) ) )
				return ( string [] ) list.ToArray ( typeof ( string ) );
			return list;
		}
		
		public static string ToDelimitedString( ICollection collection, string delimiter )
		{
			StringBuilder delimitedString = new StringBuilder ();
			if ( collection is Hashtable )
			{
				foreach ( object o in ( ( Hashtable ) collection ).Keys )
				{
					delimitedString.Append ( o.ToString () + delimiter );
				}
			}
			if ( collection is ArrayList )
			{
				foreach ( object o in ( ArrayList ) collection )
				{
					delimitedString.Append ( o.ToString () + delimiter );
				}
			}
			if ( collection is String [] )
			{
				foreach ( string s in ( String [] ) collection )
				{
					delimitedString.Append ( s + delimiter );
				}
			}
			if ( collection is int [] )
			{
				foreach ( int i in ( int [] ) collection )
				{
					delimitedString.Append ( i.ToString () + delimiter );
				}
			}
			if ( collection is List<int> )
			{
				foreach ( int i in collection )
				{
					delimitedString.Append ( i.ToString () + delimiter );
				}
			}
			if ( collection is List<string> )
			{
				foreach ( string i in collection )
				{
					delimitedString.Append ( i.ToString () + delimiter );
				}
			}
			if ( collection is NameValueCollection )
			{
				NameValueCollection o = collection as NameValueCollection;
				for ( int i = 0, count = collection.Count; i < count; i++ )
				{
					delimitedString.Append( o.GetKey( i ) + "=" + o[ i ] + delimiter );
				}
			}
			return delimitedString.ToString ().TrimEnd ( Convert.ToChar ( delimiter ) );
		}

		#region 用WBR处理字符串

		public static string FormatStringWithWBR(string str)
		{
			return FormatStringWithWBR(str, 10);
		}

		public static string FormatStringWithWBR(string str, int wordBradkSize)
		{
			StringBuilder sb = new StringBuilder();
			int len = str.Length;
			int start = 0;
			int rowStart = 0;
			char nextChar;

			for (int i = 1; i < len; i++)
			{
				nextChar = str[i];

				if (nextChar == ' ' || nextChar == '\t' || nextChar == '\r' || nextChar == '\n' || nextChar == '\0')
				{
					rowStart = i;
				}
				else
				{
					if ((i - rowStart) == wordBradkSize)
					{
						sb.Append(str.Substring(start, i - start));
						sb.Append("<wbr/>");
						start = rowStart = i;
					}
				}
			}

			sb.Append(str.Substring(start));

			return sb.ToString();
		}

		#endregion

		#region 滤掉文本中的Html标识
		/// <summary>
		/// 滤掉文本中的Html标识
		/// </summary>
		/// <param name="HtmlCode">源文本</param>
		/// <returns></returns>
		public static string NoHtml(string HtmlCode)
		{
			if (string.IsNullOrEmpty(HtmlCode))
			{
				return string.Empty;
			}
			return Regex.Replace(HtmlCode, "<[^<^>]*?>", "");
		}
		#endregion


		/// <summary>
		/// 把文章的href属性都替换为空
		/// </summary>
		/// <param name="html"></param>
		/// <returns></returns>
		public static string GetNoArchorBody(string html)
		{
			Regex reg = new Regex("<a.+?>|</a>", RegexOptions.IgnoreCase);

			return reg.Replace(html, string.Empty);
		}

        #region 去掉控制字符

        public static string CleanControlChars(string input)
        {
            char c;

            StringBuilder sb = new StringBuilder(input.Length);

            for (int i = 0, l = input.Length; i < l; i++)
            {
                c = input[i];

                if (c >= ' ')
                {
                    sb.Append(c);
                }
            }

            return sb.ToString();
        }

        #endregion

        /// <summary>
        /// 文本分页
        /// </summary>
        /// <param name="text"></param>
        /// <param name="pageToken"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public static List<string> GetPageString(string text, string pageToken)
        {
            int foundPos = text.IndexOf(pageToken);
            if (foundPos >= 0)
            {
                /*
                int foundIndex = 0;
                string pageString = string.Empty;
                while ( foundPos > 0 && foundIndex != pageIndex )
                {
                    foundIndex++;
                    pageString = text.Substring ( 0, foundPos );
                    text = text.Remove ( 0, pageString.Length );
                    foundPos = text.IndexOf ( pageToken );
                }
                return pageString;
                 * */
                List<string> pages = new List<string>(10);
                while (foundPos >= 0)
                {
                    string pageString = text.Substring(0, foundPos);
                    if (pageString.Trim().Length != 0)
                    {
                        pages.Add(pageString);
                    }
                    text = text.Remove(0, pageString.Length + pageToken.Length);
                    foundPos = text.IndexOf(pageToken);
                }
                if (text.Length > 0)
                {
                    pages.Add(text);
                }
                return pages;
            }
            return null;
        }
        /// <summary>
        /// 对HTML进行补齐操作（目前只有日志文章分页需要用到），一般情况下请调用GetCleanHTMLStringFromHtml方法
        /// </summary>
        /// <param name="inputHtml"></param>
        /// <returns></returns>
        public static string GetMakeupTagHTMLStringFromHtml(string inputHtml)
        {
            if (string.IsNullOrEmpty(inputHtml))
                return string.Empty;
            string output = inputHtml.Replace("&nbsp;", "♂");
            output = HtmlHelper.MakeupTag(output);
            output = output.Replace("♂", "&nbsp;");
            return output;
        }

        public static string Escape(string str)
        {
            if (str == null)
                return String.Empty;

            StringBuilder sb = new StringBuilder();
            int len = str.Length;

            for (int i = 0; i < len; i++)
            {
                char c = str[i];

                //everything other than the optionally escaped chars _must_ be escaped
                if (Char.IsLetterOrDigit(c) || c == '-' || c == '_' || c == '/' || c == '\\' || c == '.')
                    sb.Append(c);
                else
                    sb.Append(Uri.HexEscape(c));
            }

            return sb.ToString();
        }

        public static string UnEscape(string str)
        {
            if (str == null)
                return String.Empty;

            StringBuilder sb = new StringBuilder();
            int len = str.Length;
            int i = 0;
            while (i != len)
            {
                if (Uri.IsHexEncoding(str, i))
                    sb.Append(Uri.HexUnescape(str, ref i));
                else
                    sb.Append(str[i++]);
            }

            return sb.ToString();
        }


        #region COPY FROM LuceneSearch project
        /// <summary>
        /// 将null转换为空字符串。
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ValidString(string value)
        {
            if (value == null) return "";
            else return value;
        }

        /// <summary>
        /// 执行不区分大小写的字符串查找。
        /// </summary>
        /// <param name="s"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int IndexOf(string s, string value)
        {
            return s.IndexOf(value, StringComparison.CurrentCultureIgnoreCase);
        }

        public static int IndexOf(string s, char c)
        {
            return s.IndexOf(c.ToString(), StringComparison.CurrentCultureIgnoreCase);
        }

        /// <summary>
        /// 执行不区分大小写的字符串查找。
        /// </summary>
        /// <param name="s"></param>
        /// <param name="value"></param>
        /// <param name="start"></param>
        /// <returns></returns>
        public static int IndexOf(string s, string value, int start)
        {
            return s.IndexOf(value, start, StringComparison.CurrentCultureIgnoreCase);
        }
        public static int IndexOf(string s, char c, int start)
        {
            return s.IndexOf(c.ToString(), start, StringComparison.CurrentCultureIgnoreCase);
        }

        /// <summary>
        /// 执行不区分大小写的反向字符串查找。
        /// </summary>
        /// <param name="s"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int LastIndexOf(string s, string value)
        {
            return s.LastIndexOf(value, StringComparison.CurrentCultureIgnoreCase);
        }

        /// <summary>
        /// 执行不区分大小写的反向字符串查找。
        /// </summary>
        /// <param name="s"></param>
        /// <param name="value"></param>
        /// <param name="start"></param>
        /// <returns></returns>
        public static int LastIndexOf(string s, string value, int start)
        {
            return s.LastIndexOf(value, start, StringComparison.CurrentCultureIgnoreCase);
        }

        public static string Replace(string s, string pattern, string newValue, bool caseSensitive)
        {
            if (caseSensitive)
                return Regex.Replace(s, pattern, newValue);
            else
                return Regex.Replace(s, pattern, newValue, RegexOptions.IgnoreCase);
        }

        public static string Replace(string s, string pattern, string newValue)
        {
            return Replace(s, pattern, newValue, false);
        }

        public static string Replace(string s, char[] oldValues, string newValue, bool caseSensitive, bool holdPlace)
        {
            string pattern;
            if (holdPlace)
                pattern = string.Format("[{0}]", string.Join("", ArrayHelper.ToStringArray(oldValues)));
            else
                pattern = string.Format("[{0}]*", string.Join("", ArrayHelper.ToStringArray(oldValues)));

            return Replace(s, pattern, newValue, caseSensitive);
        }

        public static string Replace(string s, char[] oldValues, string newValue)
        {
            return Replace(s, oldValues, newValue, false, false);
        }

        public static string TrimAny(string s, char[] trimedValues)
        {
            return Replace(s, trimedValues, "");
        }


        public static int Contains(string input, string sub)
        {
            int last = -1, count = 0;
            while (true)
            {
                last = input.IndexOf(sub, last + 1);
                if (last >= 0) count++;
                else break;
            }
            return count;
        }
        #endregion COPY FROM LuceneSearch project


    }

}
