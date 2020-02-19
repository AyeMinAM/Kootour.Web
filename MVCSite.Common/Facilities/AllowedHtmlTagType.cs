using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Collections;
namespace MVCSite.Common
{
	/// <summary>
	/// 允许的HTML标记类型
	/// </summary>
	public enum AllowedHtmlTagType
	{
		Default = 0,
		Simple = 1,
        LeaveWord = 2,
		GroupDescription = 3,
		GroupTopic = 4,
		CleanBlog = 5,
        GroupTopicFromBlog,
	}

	/// <summary>
	/// 暂时放在这里
	/// </summary>
	
	public sealed class AllowedHtmlTags
	{
		#region Singleton
		private static AllowedHtmlTags current = null;

		public static AllowedHtmlTags Current
		{
			get
			{
				if ( current == null )
				{
					System.Threading.Interlocked.CompareExchange ( ref current, new AllowedHtmlTags (), null );
				}
				return current;
			}
		}

		#endregion
		
		Dictionary<string, string> DEFAULT_HTML_TAG_DICTIONARY = new Dictionary<string, string> ( StringComparer.InvariantCultureIgnoreCase );
		Dictionary<string, string> SIMPLE_HTML_TAG = new Dictionary<string, string> ( StringComparer.InvariantCultureIgnoreCase );
		Dictionary<string, string> LEAVEWORD_HTML_TAG = new Dictionary<string, string> ( StringComparer.InvariantCultureIgnoreCase );
		Dictionary<string, string> GROUP_DESCRIPTION_HTML_TAG = new Dictionary<string, string> ( StringComparer.InvariantCultureIgnoreCase );
		Dictionary<string, string> GROUP_TOPIC_HTML_TAG = new Dictionary<string, string> ( StringComparer.InvariantCultureIgnoreCase );
		Dictionary<string, string> CLEAN_BLOG_HTML_TAG = new Dictionary<string, string> ( StringComparer.InvariantCultureIgnoreCase );

		private AllowedHtmlTags()
		{
			GetDefaultTags ();
			GetSimpleTags ();
			GetLeaveWordTags ();
			GetGroupDescriptionTags ();
			GetGroupTopicTags ();
			GetCleanBlogTags ();
		}

		#region GetDefaultTags
		Dictionary<string, string> GetDefaultTags()
		{
			if ( DEFAULT_HTML_TAG_DICTIONARY.Count == 0 )
			{
				lock ( ( ( ICollection ) DEFAULT_HTML_TAG_DICTIONARY ).SyncRoot )
				{
					if ( DEFAULT_HTML_TAG_DICTIONARY.Count == 0 )
					{
						DEFAULT_HTML_TAG_DICTIONARY.Add( "h1", "align" );
						DEFAULT_HTML_TAG_DICTIONARY.Add( "h2", "align" );
						DEFAULT_HTML_TAG_DICTIONARY.Add( "h3", "align" );
						DEFAULT_HTML_TAG_DICTIONARY.Add( "h4", "align" );
						DEFAULT_HTML_TAG_DICTIONARY.Add( "h5", "align" );
						DEFAULT_HTML_TAG_DICTIONARY.Add( "h6", "align" );
						DEFAULT_HTML_TAG_DICTIONARY.Add( "strong", "" );
						DEFAULT_HTML_TAG_DICTIONARY.Add( "strike", "" );
						DEFAULT_HTML_TAG_DICTIONARY.Add( "u", "color" );
						DEFAULT_HTML_TAG_DICTIONARY.Add( "b", "color" );
						DEFAULT_HTML_TAG_DICTIONARY.Add( "i", "" );
						DEFAULT_HTML_TAG_DICTIONARY.Add( "font", "color,size,face,style" );
						DEFAULT_HTML_TAG_DICTIONARY.Add( "blockquote", "dir" );
						DEFAULT_HTML_TAG_DICTIONARY.Add( "ul", "" );
						DEFAULT_HTML_TAG_DICTIONARY.Add( "ol", "" );
						DEFAULT_HTML_TAG_DICTIONARY.Add( "li", "" );
						DEFAULT_HTML_TAG_DICTIONARY.Add( "p", "class,align,style" );
						DEFAULT_HTML_TAG_DICTIONARY.Add( "address", "" );
						DEFAULT_HTML_TAG_DICTIONARY.Add( "div", "align,style" );
						DEFAULT_HTML_TAG_DICTIONARY.Add( "hr", "id" );
						DEFAULT_HTML_TAG_DICTIONARY.Add( "br", "" );
						DEFAULT_HTML_TAG_DICTIONARY.Add( "a", "href,target,name" );
						DEFAULT_HTML_TAG_DICTIONARY.Add( "span", "align,style" );
						DEFAULT_HTML_TAG_DICTIONARY.Add( "img", "class,src,alt,title,align, onload,loop,dynsrc,width,height,margin,border" );
						DEFAULT_HTML_TAG_DICTIONARY.Add( "object", "classid,codebase,height,width" );
						DEFAULT_HTML_TAG_DICTIONARY.Add( "param", "name,value" );
						DEFAULT_HTML_TAG_DICTIONARY.Add ( "embed", "pluginspace,showstatusbar,showgotobar,showaudiocontrols,showtracker,showpositioncontrols,showcontrols,autostart,autosize,codebase,class,id,title,scale,play,loop,menu,height,pluginspage,quality,type,width,wmode,src,pluginspace,name,bgcolor,flashVars,seamlesstabbing,swLiveConnect" );
						DEFAULT_HTML_TAG_DICTIONARY.Add( "table", "background,border,bordercolor,bordercolorlight,bordercolordark,colspan,rowspan,bgcolor,cellpadding,cellspacing" );
						DEFAULT_HTML_TAG_DICTIONARY.Add( "tr", "" );
						DEFAULT_HTML_TAG_DICTIONARY.Add( "td", "align,valign, colspan,rowspan" );
						DEFAULT_HTML_TAG_DICTIONARY.Add( "tbody", "" );
					}
				}
			}
			return DEFAULT_HTML_TAG_DICTIONARY;
		} 
		#endregion

		#region GetSimpleTags
		Dictionary<string, string> GetSimpleTags()
		{
			if ( SIMPLE_HTML_TAG.Count == 0 )
			{
				lock ( ( ( ICollection ) SIMPLE_HTML_TAG ).SyncRoot )
				{
					if ( SIMPLE_HTML_TAG.Count == 0 )
					{
						SIMPLE_HTML_TAG.Add( "strong", "" );
						SIMPLE_HTML_TAG.Add( "strike", "" );
						SIMPLE_HTML_TAG.Add( "u", "color" );
						SIMPLE_HTML_TAG.Add( "b", "color" );
						SIMPLE_HTML_TAG.Add( "i", "" );
						SIMPLE_HTML_TAG.Add( "font", "color,size,face,style" );
						SIMPLE_HTML_TAG.Add( "blockquote", "dir" );
						SIMPLE_HTML_TAG.Add( "ul", "" );
						SIMPLE_HTML_TAG.Add( "ol", "" );
						SIMPLE_HTML_TAG.Add( "li", "" );
						SIMPLE_HTML_TAG.Add( "p", "class,align,style" );
						SIMPLE_HTML_TAG.Add( "address", "" );
						SIMPLE_HTML_TAG.Add( "div", "align,style" );
						SIMPLE_HTML_TAG.Add( "hr", "id" );
						SIMPLE_HTML_TAG.Add( "br", "" );
						SIMPLE_HTML_TAG.Add( "a", "href,target,name" );
						SIMPLE_HTML_TAG.Add( "span", "align,style" );
						SIMPLE_HTML_TAG.Add( "img", "class,src,alt,title,align, onload,loop,dynsrc,width,height,margin,border" );
					}
				}
			}
			return SIMPLE_HTML_TAG;
		}
		#endregion

		#region GetLeaveWordTags

		Dictionary<string, string> GetLeaveWordTags()
        {
			if ( LEAVEWORD_HTML_TAG.Count == 0 )
			{
				lock ( ( ( ICollection ) LEAVEWORD_HTML_TAG ).SyncRoot )
				{
					if ( LEAVEWORD_HTML_TAG.Count == 0 )
					{
						LEAVEWORD_HTML_TAG.Add ( "strong", "" );
						LEAVEWORD_HTML_TAG.Add ( "b", "color" );
						LEAVEWORD_HTML_TAG.Add ( "font", "color,size,face,style" );
						LEAVEWORD_HTML_TAG.Add ( "p", "class,align" );
						LEAVEWORD_HTML_TAG.Add ( "br", "" );
						LEAVEWORD_HTML_TAG.Add ( "a", "href,target,name" );
						LEAVEWORD_HTML_TAG.Add ( "span", "align,style" );
						LEAVEWORD_HTML_TAG.Add ( "img", "class,src,alt,title,align, onload,loop,dynsrc,width,height,margin,border" );
					}
				}
			}
            return LEAVEWORD_HTML_TAG;
        }
        #endregion

		#region GetGroupDescriptionTags

		Dictionary<string, string> GetGroupDescriptionTags()
		{
			if ( GROUP_DESCRIPTION_HTML_TAG.Count == 0 )
			{
				lock ( ( ( ICollection ) GROUP_DESCRIPTION_HTML_TAG ).SyncRoot )
				{
					if ( GROUP_DESCRIPTION_HTML_TAG.Count == 0 )
					{
						GROUP_DESCRIPTION_HTML_TAG.Add ( "strong", "" );
						GROUP_DESCRIPTION_HTML_TAG.Add ( "b", "" );
						GROUP_DESCRIPTION_HTML_TAG.Add ( "br", "" );
						GROUP_DESCRIPTION_HTML_TAG.Add ( "a", "href,target,name" );
						GROUP_DESCRIPTION_HTML_TAG.Add ( "p", "" );
						//加粗
						GROUP_DESCRIPTION_HTML_TAG.Add ( "span", "align,style" );
					}
				}
			}
			return GROUP_DESCRIPTION_HTML_TAG;
		}

		#endregion

		#region GetGroupTopicTags
		Dictionary<string, string> GetGroupTopicTags()
		{
			if ( GROUP_TOPIC_HTML_TAG.Count == 0 )
			{
				lock ( ( ( ICollection ) GROUP_TOPIC_HTML_TAG ).SyncRoot )
				{
					if ( GROUP_TOPIC_HTML_TAG.Count == 0 )
					{
						GROUP_TOPIC_HTML_TAG.Add ( "strong", "" );
						GROUP_TOPIC_HTML_TAG.Add ( "b", "color" );
						GROUP_TOPIC_HTML_TAG.Add ( "font", "color,size,face,style" );
						GROUP_TOPIC_HTML_TAG.Add ( "p", "class,align,style" );
						GROUP_TOPIC_HTML_TAG.Add ( "div", "align,style" );
						GROUP_TOPIC_HTML_TAG.Add ( "br", "" );
						GROUP_TOPIC_HTML_TAG.Add ( "a", "href,target,name" );
						GROUP_TOPIC_HTML_TAG.Add ( "span", "align,style" );
						GROUP_TOPIC_HTML_TAG.Add ( "img", "class,src,alt,title,align, onload,loop,dynsrc,width,height,margin,border" );
						GROUP_TOPIC_HTML_TAG.Add ( "object", "classid,codebase,height,width" );
						GROUP_TOPIC_HTML_TAG.Add ( "param", "name,value" );
						GROUP_TOPIC_HTML_TAG.Add ( "embed", "pluginspace,showstatusbar,showgotobar,showaudiocontrols,showtracker,showpositioncontrols,showcontrols,autostart,autosize,codebase,class,id,title,scale,play,loop,menu,height,pluginspage,quality,type,width,wmode,src,pluginspace,name,bgcolor,flashVars,seamlesstabbing,swLiveConnect" );
					}
				}
			}
			return GROUP_TOPIC_HTML_TAG;
		}
		#endregion

		#region GetCleanBlogTags
		Dictionary<string, string> GetCleanBlogTags()
		{
			if ( CLEAN_BLOG_HTML_TAG.Count == 0 )
			{
				lock ( ( ( ICollection ) CLEAN_BLOG_HTML_TAG ).SyncRoot )
				{
					if ( CLEAN_BLOG_HTML_TAG.Count == 0 )
					{
						CLEAN_BLOG_HTML_TAG.Add ( "ul", "" );
						CLEAN_BLOG_HTML_TAG.Add ( "ol", "" );
						CLEAN_BLOG_HTML_TAG.Add ( "li", "" );
						CLEAN_BLOG_HTML_TAG.Add ( "p", "align" );
						CLEAN_BLOG_HTML_TAG.Add ( "div", "align" );
						CLEAN_BLOG_HTML_TAG.Add ( "br", "" );
						CLEAN_BLOG_HTML_TAG.Add ( "a", "href,target,name" );
						CLEAN_BLOG_HTML_TAG.Add ( "span", "align" );
						CLEAN_BLOG_HTML_TAG.Add ( "img", "class,src,alt,title,align,onload,loop,dynsrc,width,height,margin,border" );
						CLEAN_BLOG_HTML_TAG.Add ( "object", "classid,codebase,height,width" );
						CLEAN_BLOG_HTML_TAG.Add ( "param", "name,value" );
						CLEAN_BLOG_HTML_TAG.Add ( "embed", "pluginspace,showstatusbar,showgotobar,showaudiocontrols,showtracker,showpositioncontrols,showcontrols,autostart,autosize,codebase,class,id,title,scale,play,loop,menu,height,pluginspage,quality,type,width,wmode,src,pluginspace,name,bgcolor,flashVars,seamlesstabbing,swLiveConnect" );
						CLEAN_BLOG_HTML_TAG.Add ( "table", "border,colspan,rowspan,cellpadding,cellspacing" );
						CLEAN_BLOG_HTML_TAG.Add ( "tr", "" );
						CLEAN_BLOG_HTML_TAG.Add ( "td", "align,valign,colspan,rowspan" );
						CLEAN_BLOG_HTML_TAG.Add ( "tbody", "" );
					}
				}
			}
			return CLEAN_BLOG_HTML_TAG;
		}
		#endregion

		public Dictionary<string, string> GetAllowedHtmlTags( AllowedHtmlTagType allowedHtmlTagType )
		{
			Dictionary<string, string> allowedTags = null;
			switch ( allowedHtmlTagType )
			{
				case AllowedHtmlTagType.Default:
					allowedTags = GetDefaultTags ();
					break;
				case AllowedHtmlTagType.Simple:
					allowedTags = GetSimpleTags ();
					break;
                case AllowedHtmlTagType.LeaveWord:
                    allowedTags = GetLeaveWordTags();
                    break;
				case AllowedHtmlTagType.GroupDescription:
					allowedTags = GetGroupDescriptionTags();
					break;
				case AllowedHtmlTagType.GroupTopic:
					allowedTags = GetGroupTopicTags ();
					break;
				case AllowedHtmlTagType.CleanBlog:
					allowedTags = GetCleanBlogTags ();
					break;
				default:
					throw new ArgumentOutOfRangeException ( "allowedHtmlTagType" );
			}
			return allowedTags;
		}
	}


}
