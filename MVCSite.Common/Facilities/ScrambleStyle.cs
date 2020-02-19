using System;
using System.Collections.Generic;
using System.Text;

namespace MVCSite.Common
{
	public sealed class ScrambleStyle
	{
		private static ScrambleStyle current = null;
		
		private const int maxScrambleStyleCount = 64;
		private const string template = "<span class=\"{0}\">{1}</span>";
		private const string fakeStyleTemplate = ".{0}{{display:none}}";
		//目前只针对时间做处理，只要数字
		//static char [] scrambleChars = new char [] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };
		static readonly char [] scrambleChars = new char [] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', ':', ' ', '|' };
		private static List<string> realStyles = null;
		private static List<string> fakeStyles = null;
		static readonly Randoms random = new Randoms ();
		
		private ScrambleStyle()
		{
			realStyles = new List<string> ( maxScrambleStyleCount );
			fakeStyles = new List<string> ( maxScrambleStyleCount );
			for ( int i = 0, count = maxScrambleStyleCount; i < count; i++ )
			{
				string key = "m_showtime" + i;
				if ( random.TrueOrFalse () )
				{
					realStyles.Add ( key );
				}
				else
				{
					fakeStyles.Add ( key );
				}
			}
		}
		
		public static ScrambleStyle Current
		{
			get
			{
				if ( current == null )
				{
					System.Threading.Interlocked.CompareExchange ( ref current,
																  new ScrambleStyle (), null );
				}
				return current;
			}
		}
		
		public string GetString( char c )
		{
			ScrambleType scrambleType = GetScrambleType();
			switch( scrambleType )
			{
				case ScrambleType.NoSet:
					return c.ToString ();
					break;
				case ScrambleType.FakeStringPre:
					return GetFakeString() + c;
					break;
				case ScrambleType.FakeStringPost:
					return c + GetFakeString ();
					break;
				case ScrambleType.WrapSelf:
					return GetSelfString( c );
					break;
				case ScrambleType.WrapSelfFakeStringPre:
					return GetFakeString () + GetSelfString ( c );
					break;
				case ScrambleType.WrapSelfFakeStringPost:
					return GetSelfString ( c ) + GetFakeString ();
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		} 
		
		public string GenFakeStyles()
		{
			StringBuilder builder = new StringBuilder ( fakeStyles.Count * ( fakeStyleTemplate.Length + 8 ) );
			foreach( string key in fakeStyles )
			{
				builder.Append( string.Format( fakeStyleTemplate, key ) );
			}
			return builder.ToString();
		}
		
		public enum ScrambleType
		{
			NoSet = 0,
			FakeStringPre = 1,
			FakeStringPost = 2,
			WrapSelf = 3,
			WrapSelfFakeStringPre = 4,
			WrapSelfFakeStringPost = 5
		}
		
		private ScrambleType GetScrambleType()
		{
			//不去计算枚举，直接hardCode
			return ( ScrambleType ) random.Next ( 0, 5 );
		}
		
		private string GetFakeString()
		{
			int fakeStyleIndex = random.Next ( fakeStyles.Count );
			return string.Format ( template, fakeStyles [fakeStyleIndex], scrambleChars [random.Next ( scrambleChars.Length )] );
		}

		private string GetSelfString( char c )
		{
			int index = random.Next ( realStyles.Count );
			return string.Format ( template, realStyles [index], c );
		}
	}
}
