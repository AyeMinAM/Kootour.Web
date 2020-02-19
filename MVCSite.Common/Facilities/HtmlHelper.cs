using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Caching;
using System.Xml;

namespace MVCSite.Common
{
    #region HtmlTag
    public class HtmlTag
    {
        string tagName;
        int tagCount;

        public string TagName
        {
            get
            {
                return tagName;
            }
            set
            {
                tagName = value;
            }
        }

        public int TagCount
        {
            get
            {
                return tagCount;
            }
            set
            {
                tagCount = value;
            }
        }

        public HtmlTag(string tagName, int tagCount)
        {
            this.tagName = tagName;
            this.tagCount = tagCount;
        }
    }

    public class HtmlTagCollection : CollectionBase
    {
        #region Methods

        public HtmlTag this[int index]
        {
            get
            {
                return (HtmlTag)List[index];
            }
            set
            {
                List[index] = value;
            }
        }

        public void Add(HtmlTag value)
        {
            List.Add( value );
        }

        public int IndexOf(HtmlTag value)
        {
            for ( int i = 0 ; i < this.List.Count ; i++ )
            {
                if ( string.Compare( value.TagName, ( this.List[i] as HtmlTag ).TagName, true ) == 0 )
                    return i;
            }
            return -1;
        }

        public int IndexOf(string tagName)
        {
            for ( int i = 0 ; i < this.List.Count ; i++ )
            {
                if ( string.Compare( tagName, ( this.List[i] as HtmlTag ).TagName, true ) == 0 )
                    return i;
            }
            return -1;
        }

        public void Insert(int index, HtmlTag value)
        {
            List.Insert( index, value );
        }

        public void Remove(HtmlTag value)
        {
            int index = IndexOf( value );
            if ( index != -1 )
                RemoveAt( index );
        }

        public bool Contains(HtmlTag value)
        {
            if ( IndexOf( value ) == -1 )
                return false;
            return true;
        }

        public bool Contains(string tagName)
        {
            if ( IndexOf( tagName ) == -1 )
                return false;
            return true;
        }

        #endregion
    }

    #endregion

    public class HtmlHelper
    {
        #region Static Default

        protected Dictionary<string, string> allowedTags = null;
        static Regex regex = new Regex( "<[^>]+>", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Multiline );
        static Regex jsAttributeRegex = new Regex( "javascript:", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled );
        static Regex xmlLineBreak = new Regex( "&#x([DA9]|20|85|2028|0A|0D)(;)?", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled );
        static Regex filterdCharacters = new Regex( "\\=|\\\"|\\'|\\s|\"'", RegexOptions.Compiled );
        static Regex validProtocols = new Regex( "^((http(s)?|mailto|mms):|#|/)", RegexOptions.Compiled | RegexOptions.IgnoreCase );
        static Regex bannedChars = new Regex( "\\s", RegexOptions.Compiled );
        //static Regex blankTagRegex = new Regex ( "(<([^/<>]*)></([^\"'<>]*)>)|(<([^/<>]*)>¡á</([^\"'<>]*)>)", RegexOptions.IgnoreCase );
        //static Regex imageOnloadTagRegex1 = new Regex ( "onload=(\")?\\s*[^\"\n]*(\")?", RegexOptions.IgnoreCase );
        //static Regex imageOnloadTagRegex1 = new Regex ( @"onload=(\"")?\s*[^\""\n]*(\"")?", RegexOptions.IgnoreCase );
        //static Regex imageOnloadTagRegex2 = new Regex ( "onload=\\\"if[^\"]*\\\"", RegexOptions.IgnoreCase );
        //static Regex blankTagRegex = new Regex ( "(<p></p>)|(<p>¡á</p>)|(¡á{4,})", RegexOptions.IgnoreCase );
        static Regex blankTagRegex = new Regex( "(<p></p>(\r\n)?){2,}", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Multiline );
        static Regex blankParagraphTagRegex = new Regex( "(<p>¡á</p>(\r\n)?){2,}", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Multiline );
        private const string blankParagraphTagReplaceString = "<p>¡á</p><p>¡á</p>";
        static Regex blankSpaceTagRegex = new Regex( "(¡á{16,})", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Multiline );
        private const string blankSpaceTagReplaceString = "¡á¡á¡á¡á¡á¡á¡á¡á¡á¡á¡á¡á¡á¡á¡á¡á";
        #endregion

        #region Private members

        string input = null;
        StringBuilder output = new StringBuilder();
        HtmlTagCollection beginTag = new HtmlTagCollection();
        HtmlTagCollection endTag = new HtmlTagCollection();
        bool cleanJS = false;
        bool isFormatted = false;
        bool encodeExceptions = false;

        #endregion

        #region Public Statics

        public static string Clean(string html, bool encodeExceptions, bool filterScripts)
        {
            //if ( string.IsNullOrEmpty ( html ) )
            //    return html;
            //HtmlHelper htmlHelper = new HtmlHelper ( html, encodeExceptions, filterScripts );
            //return htmlHelper.Clean ();
            return Clean( html, encodeExceptions, filterScripts, null );
        }

        public static string Clean(string html, bool encodeExceptions, bool filterScripts, Dictionary<string,string> allowedTags)
        {
            if ( string.IsNullOrEmpty( html ) )
                return html;
            HtmlHelper htmlHelper = new HtmlHelper( html, encodeExceptions, filterScripts, allowedTags );
            return htmlHelper.Clean();
        }

        public static string MakeupTag(string html)
        {
            //if ( string.IsNullOrEmpty ( html ) )
            //    return html;
            //HtmlHelper htmlHelper = new HtmlHelper ( html, false, true );
            //return htmlHelper.MakeupTag();
            return MakeupTag( html, null );
        }

		public static string MakeupTag( string html, Dictionary<string, string> allowedTags )
        {
            if ( string.IsNullOrEmpty( html ) )
                return html;
            HtmlHelper htmlHelper = new HtmlHelper( html, false, true, allowedTags );
            return htmlHelper.MakeupTag();
        }

        public static string Substring(string html, int length)
        {
        	return Substring( html, length, null );
        }

		public static string Substring( string html, int length, Dictionary<string, string> allowedTags )
		{
			if ( string.IsNullOrEmpty ( html ) )
				return html;
			if ( html.Length <= length )
			{
				return html;
			}
			HtmlHelper htmlHelper = new HtmlHelper ( html, false, true, allowedTags );
			return htmlHelper.Substring ( length );
		}
		
        public static string StripScriptTags(string text)
        {
            return jsAttributeRegex.Replace( text, string.Empty );
        }

        #endregion

        #region cnstr

        /// <summary>
        /// Filters unknown markup. Will not encode exceptions
        /// </summary>
        /// <param name="html">Markup to filter</param>
        public HtmlHelper(string html)
            : this( html, false, true )
        {
        }

        /// <summary>
        /// Filters unknown markup
        /// </summary>
        /// <param name="html">Markup to filter</param>
        /// <param name="encodeRuleExceptions">Should unknown elements be encoded or removed?</param>
        public HtmlHelper(string html, bool encodeRuleExceptions)
            : this( html, encodeRuleExceptions, true )
        {

        }

        /// <summary>
        /// Filters unknown markup
        /// </summary>
        /// <param name="html">Markup to filter</param>
        /// <param name="encodeRuleExceptions">Should unknown elements be encoded or removed?</param>
        /// <param name="removeScripts">Check for javascript: attributes</param>
        public HtmlHelper(string html, bool encodeRuleExceptions, bool removeScripts)
        {
            //input = imageOnloadTagRegex1.Replace ( html, "" );
            //input = imageOnloadTagRegex2.Replace ( input, "" );
            input = html;
            cleanJS = removeScripts;
            isFormatted = false;
            encodeExceptions = encodeRuleExceptions;
            allowedTags = AllowedHtmlTags.Current.GetAllowedHtmlTags( AllowedHtmlTagType.Default );
        }

        public HtmlHelper(string html, bool encodeRuleExceptions, bool removeScripts, Dictionary<string,string> allowedTags)
            : this( html, encodeRuleExceptions, removeScripts )
        {
            if ( allowedTags != null )
                this.allowedTags = allowedTags;
        }
        #endregion

        #region Cleaners
        /// <summary>
        /// Returns the results of a cleaning.
        /// </summary>
        /// <returns></returns>
        public string Clean()
        {
            if ( !isFormatted )
            {
                Format();
                isFormatted = true;
            }
            return CleanBlankTag( output.ToString() );
        }

        #endregion

        public string MakeupTag()
        {
            Format();
            /*
            for ( int beginCount = beginTag.Count - 1; beginCount >= 0; beginCount-- )
            {
                HtmlTag tag = beginTag [beginCount];
                if ( string.Compare ( tag.TagName, "br", true ) == 0 )
                    continue;
                if ( string.Compare ( tag.TagName, "img", true ) == 0 )
                    continue;
                int endIndex = endTag.IndexOf ( tag );
                if ( endIndex == -1 )
                {
                    for ( int i = 0, count = tag.TagCount; i < count; i++ )
                    {
                        output.Append ( "</" );
                        output.Append ( tag.TagName );
                        output.Append ( ">" );
                    }
                }
                else
                {
                    if ( tag.TagCount > endTag [endIndex].TagCount )
                    {
                        int count = tag.TagCount - endTag [endIndex].TagCount;
                        for ( int i = 0; i < count; i++ )
                        {
                            output.Append ( "</" );
                            output.Append ( tag.TagName );
                            output.Append ( ">" );
                        }
                    }
                }
            }
             * */
            return CleanBlankTag( MakeupTag( output ) );
        }

        public string Substring(int length)
        {
            Format();
            string cleanString = CleanBlankTag( output.ToString() );
            if ( cleanString.Length <= length )
                return cleanString;

            beginTag.Clear();
            endTag.Clear();
            int substringLength = 0;
            bool substringEnough = false;
            StringBuilder cleanBuilder = new StringBuilder();
            Match match = regex.Match( cleanString, 0, cleanString.Length );
            while ( !string.IsNullOrEmpty( match.Value ) )
            {
                int index = cleanString.IndexOf( match.Value );
                int indexLength = index;
                while ( indexLength > 0 )
                {
                    string contentString = cleanString.Substring( 0, 1 ).Trim();
                    if ( contentString.Length == 0 )
                    {
                        cleanBuilder.Append( cleanString.Substring( 0, 1 ) );
                        cleanString = cleanString.Remove( 0, 1 );
                        indexLength--;
                        continue;
                    }
                    cleanBuilder.Append( cleanString.Substring( 0, 1 ) );
                    cleanString = cleanString.Remove( 0, 1 );
                    indexLength--;
                    substringLength++;
                    if ( substringLength == length )
                    {
                        substringEnough = true;
                        break;
                    }
                }
                if ( substringEnough )
                    break;
                //output.Append ( input.Substring ( 0, index ) );
                //cleanString = cleanString.Remove ( 0, index );
                cleanBuilder.Append( Validate( match.Value ) );
                cleanString = cleanString.Remove( 0, match.Length );
                match = regex.Match( cleanString, 0, cleanString.Length );
            }
            if ( substringEnough )
            {
                return CleanBlankTag( MakeupTag( cleanBuilder ) );
            }
            else
            {
                if ( cleanString != null && cleanString.Trim().Length > 0 )
                {
                    int cleanStringLength = cleanString.Length;
                    if ( cleanStringLength > length )
                        cleanBuilder.Append( cleanString.Substring( 0, length ) );
                    else
                        cleanBuilder.Append( cleanString );
                }
            }
            return CleanBlankTag( MakeupTag( cleanBuilder ) );
        }

        /*
         string MakeupTag( StringBuilder tagBuilder )
         {
            for ( int beginCount = beginTag.Count - 1; beginCount >= 0; beginCount-- )
            {
                HtmlTag tag = beginTag [beginCount];
                if ( string.Compare ( tag.TagName, "br", true ) == 0 )
                    continue;
                if ( string.Compare ( tag.TagName, "img", true ) == 0 )
                    continue;
                int endIndex = endTag.IndexOf ( tag );
                if ( endIndex == -1 )
                {
                    for ( int i = 0, count = tag.TagCount; i < count; i++ )
                    {
                        tagBuilder.Append ( "</" );
                        tagBuilder.Append ( tag.TagName );
                        tagBuilder.Append ( ">" );
                    }
                }
                else
                {
                    if ( tag.TagCount > endTag [endIndex].TagCount )
                    {
                        int count = tag.TagCount - endTag [endIndex].TagCount;
                        for ( int i = 0; i < count; i++ )
                        {
                            tagBuilder.Append ( "</" );
                            tagBuilder.Append ( tag.TagName );
                            tagBuilder.Append ( ">" );
                        }
                    }
                }
            }
            return tagBuilder.ToString();
         }
        */

        string MakeupTag(StringBuilder tagBuilder)
        {
            for ( int endCount = endTag.Count - 1 ; endCount >= 0 ; endCount-- )
            {
                HtmlTag tag = endTag[endCount];
                int beginIndex = beginTag.IndexOf( tag );
                if ( beginIndex != -1 )
                {
                    beginTag.RemoveAt( beginIndex );
                }
            }
            for ( int beginCount = beginTag.Count - 1 ; beginCount >= 0 ; beginCount-- )
            {
                HtmlTag tag = beginTag[beginCount];
                if ( string.Compare( tag.TagName, "br", true ) == 0 )
                    continue;
                if ( string.Compare( tag.TagName, "img", true ) == 0 )
                    continue;
                tagBuilder.Append( "</" );
                tagBuilder.Append( tag.TagName );
                tagBuilder.Append( ">" );
            }
            return tagBuilder.ToString();
        }

        #region Format / Walk

        /// <summary>
        /// Walks one time through the HTML. All elements/tags are validated.
        /// The rest of the text is simply added to the internal queue
        /// </summary>
        protected virtual void Format()
        {
            //Lets look for elements/tags
            Match match = regex.Match( input, 0, input.Length );
            //Never seems to be null
            while ( !string.IsNullOrEmpty( match.Value ) )
            {
                //find the first occurence of this elment
                int index = input.IndexOf( match.Value );

                //add the begining to this tag
                output.Append( input.Substring( 0, index ) );

                //remove this from the supplied text
                input = input.Remove( 0, index );

                //validate the element
                output.Append( Validate( match.Value ) );

                //remove this element from the supplied text
                input = input.Remove( 0, match.Length );

                //Get the next match
                match = regex.Match( input, 0, input.Length );
            }

            //If not Html is found, we should just place all the input into the output container
            if ( input != null && input.Trim().Length > 0 )
                output.Append( input );
        }

        protected virtual string CleanBlankTag(string inputString)
        {
            string outputString = inputString;
            /*
            Match match = blankTagRegex.Match ( outputString, 0, outputString.Length );
            while ( match.Success )
            {
                outputString = blankTagRegex.Replace ( outputString, string.Empty, -1 );
                //Get the next match
                match = blankTagRegex.Match ( outputString, 0, outputString.Length );
            }
            return outputString;
             * */
            outputString = blankTagRegex.Replace( outputString, string.Empty, -1 );
            outputString = blankParagraphTagRegex.Replace( outputString, blankParagraphTagReplaceString, -1 );
            outputString = blankSpaceTagRegex.Replace( outputString, blankSpaceTagReplaceString, -1 );
            return outputString;
        }

        #endregion

        #region Validators

        /// <summary>
        /// Main method for starting element validation
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        protected string Validate(string tag)
        {
            if ( tag.StartsWith( "</" ) )
                return ValidateEndTag( tag );

            if ( tag.EndsWith( "/>" ) )
                return ValidateSingleTag( tag );


            return ValidateStartTag( tag );

        }

        /// <summary>
        /// Validates single element tags such as <br /> and <hr class = "X" />
        /// </summary>
        private string ValidateSingleTag(string tag)
        {
            string strip = tag.Substring( 1, tag.Length - 3 ).Trim();

            int index = strip.IndexOfAny( new char[] { ' ', '\r', '\n' } );
            if ( index == -1 )
                index = strip.Length;

            string tagName = strip.Substring( 0, index );

            //Word
            if ( tagName.IndexOf( ":" ) != -1 )
                return string.Empty;

            int colonIndex = tagName.IndexOf( ":" ) + 1;


            string safeTagName = tagName.Substring( colonIndex, tagName.Length - colonIndex );

			//string allowedAttributes = allowedTags[safeTagName] as string;
            if ( ! allowedTags.ContainsKey( safeTagName ) )
				return encodeExceptions ? System.Web.HttpUtility.HtmlEncode ( tag ) : string.Empty;
			//if ( allowedAttributes == null )
			//    return encodeExceptions ? System.Web.HttpUtility.HtmlEncode( tag ) : string.Empty;
			string allowedAttributes = allowedTags [safeTagName] as string;

            string atts = strip.Remove( 0, tagName.Length ).Trim();

            return ValidateAttributes( allowedAttributes, atts, tagName, "<{0}{1} />" );



        }

        /// <summary>
        /// Validates a start tag
        /// </summary>
        /// <param name="tag"></param>
        /// <returns>the tag and validate attributes</returns>
        protected virtual string ValidateStartTag(string tag)
        {
            //Check for potential attributes
            int endIndex = tag.IndexOfAny( new char[] { ' ', '\r', '\n' } );

            //simple tag <tag>
            if ( endIndex == -1 )
                endIndex = tag.Length - 1;

            //Grab the tab name
            string tagName = tag.Substring( 1, endIndex - 1 );

            //Word
            if ( tagName.IndexOf( ":" ) != -1 )
                return string.Empty;

            //watch for html pasted from Office and messy namespaces
            int colonIndex = tagName.IndexOf( ":" );
            string safeTagName = tagName;
            if ( colonIndex != -1 )
                safeTagName = tagName.Substring( colonIndex + 1 );

			////Use safe incase a : is present
			//string allowedAttributes = allowedTags[safeTagName] as string;
		
            //If we do not find a record in the Hashtable, this tag is not valid
            if ( !allowedTags.ContainsKey( safeTagName ) )
				return encodeExceptions ? System.Web.HttpUtility.HtmlEncode ( tag ) : string.Empty; //remove element and all attributes if not valid
			//Use safe incase a : is present
			string allowedAttributes = allowedTags [safeTagName] as string;

			//if ( allowedAttributes == null )
			//    return encodeExceptions ? System.Web.HttpUtility.HtmlEncode( tag ) : string.Empty; //remove element and all attributes if not valid

            /*
            int index = beginTag.IndexOf ( safeTagName );
            if ( index == -1 )
            {
                beginTag.Add ( new HtmlTag ( safeTagName, 1 ) );
            }
            else
            {
                beginTag [index].TagCount++;
            }
            */

            beginTag.Add( new HtmlTag( safeTagName, 1 ) );

            //remove the tag name and find all of the current element's attributes
            int start = ( colonIndex == -1 ) ? tagName.Length : safeTagName.Length + colonIndex + 1;

            string attributes = tag.Substring( start + 1, ( tag.Length - ( start + 2 ) ) );

            //if we have attributes, make sure there is no extra padding in the way
            if ( attributes != null )
                attributes = attributes.Trim();

            //Validate the attributes
            return ValidateAttributes( allowedAttributes, attributes, tagName, "<{0}{1}>" );


        }

        /// <summary>
        /// Validates the elements attribute collection
        /// </summary>
        /// <param name="allowedAttributes"></param>
        /// <param name="tagAttributes"></param>
        /// <param name="tagName"></param>
        /// <returns></returns>
        protected virtual string ValidateAttributes(string allowedAttributes, string tagAttributes, string tagName, string tagFormat)
        {
            try
            {
                //container for attributes. We could use a stringbuilder here, but chances are we are removing most attributes
                string atts = "";
                //Do we even have any attributes to validate?
                if ( allowedAttributes.Length > 0 )
                {
                    tagAttributes = xmlLineBreak.Replace( tagAttributes, string.Empty );

                    for ( int start = 0, end = 0 ;
                         start < tagAttributes.Length ;
                         start = end )
                    {
                        //Put the end index at the end of the attribute name.
                        end = tagAttributes.IndexOf( '=', start );
                        if ( end < 0 )
                            end = tagAttributes.Length;
                        //Get the attribute name and see if it's allowed.
                        string att = tagAttributes.Substring( start, end - start ).Trim();

                        bool allowed = Regex.IsMatch( allowedAttributes, string.Format( "({0},|{0}$)", att ), RegexOptions.IgnoreCase );
                        //Now advance the end index to include the attribute value.
                        if ( end < tagAttributes.Length )
                        {
                            //Skip any blanks after the '='.
                            for ( ++end ;
                                 end < tagAttributes.Length && ( tagAttributes[end] == ' ' || tagAttributes[end] == '\r' || tagAttributes[end] == '\n' ) ;
                                 ++end ) ;
                            if ( end < tagAttributes.Length )
                            {
                                //Find the end of the value.
                                end = tagAttributes[end] == '"' //Quoted with double quotes?
                                        ? tagAttributes.IndexOf( '"', end + 1 )
                                        : tagAttributes[end] == '\'' //Quoted with single quotes?
                                            ? tagAttributes.IndexOf( '\'', end + 1 )
                                            : tagAttributes.IndexOfAny( new char[] { ' ', '\r', '\n' } , end ); //Otherwise, assume not quoted.
                                //If we didn't find the terminating character, just go to the end of the string.
                                //Otherwise, advance the end index past the terminating character.
                                end = end < 0 ? tagAttributes.Length : end + 1;
                            }
                        }
                        //If the attribute is allowed, copy it.
                        if ( allowed )
                        {
                            //Special actions on these attributes. IE will render just about anything that looks like the word javascript:
                            //this includes line breaks, special characters codes, etc.
                            if ( att.ToLower() == "src" || att.ToLower() == "href" )
                            {
                                //File the value of the attribute
                                //string attValue  = tagAttributes.Substring(start + att.Length, end - (start+att.Length)).Trim();
                                string attValue = tagAttributes.Substring( start, end - start ).Trim();

                                attValue = attValue.Substring( att.Length );

                                //temporarily remove some characters - mainly =, ", ', and white spaces
                                attValue = filterdCharacters.Replace( attValue, string.Empty );

                                //validate only http, https, mailto, and / (relative) requests are made
                                if ( validProtocols.IsMatch( attValue ) )
                                {
                                    atts += ' ' + bannedChars.Replace( tagAttributes.Substring( start, end - start ).Trim(), string.Empty );
                                }

                                //If the "if" above fails, we do not render the attribute!

                            }
                            else
                            {
                                atts += ' ' + tagAttributes.Substring( start, end - start ).Trim();
                            }


                        }
                    }
                    //Are we filtering for Javascript?
                    if ( cleanJS )
                        atts = jsAttributeRegex.Replace( atts, string.Empty );
                }
                return string.Format( tagFormat, tagName, atts );
            }
            catch
            {
                return string.Empty;
            }
        }


        /// <summary>
        /// Validate End/Closing tag
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        protected virtual string ValidateEndTag(string tag)
        {
            string tagName = tag.Substring( 2, tag.Length - 3 );

            //Word
            if ( tagName.IndexOf( ":" ) != -1 )
                return string.Empty;

            int index = tag.IndexOf( ":" ) - 1;
            if ( index == -2 )
            {
                index = 0;
            }
            tagName = tagName.Substring( index );
			//string allowed = allowedTags[tagName] as string;

			if ( !allowedTags.ContainsKey( tagName ) )
                return encodeExceptions ? System.Web.HttpUtility.HtmlEncode( tag ) : string.Empty;
                
			string allowed = allowedTags [tagName] as string;

            /*
            int tagIndex = endTag.IndexOf ( tagName );
            if ( tagIndex == -1 )
            {
                endTag.Add ( new HtmlTag ( tagName, 1 ) );
            }
            else
            {
                endTag [tagIndex].TagCount++;
            }
            */

            endTag.Add( new HtmlTag( tagName, 1 ) );


            return tag;

        }

        #endregion
    }
}

