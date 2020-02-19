using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace MVCSite.Common
{
	public sealed class Randoms
	{
		private RNGCryptoServiceProvider _rand = new RNGCryptoServiceProvider ();

		private byte [] _rd4 = new byte [4], _rd8 = new byte [8];

		public int Next( int max )
		{
			if ( max <= 0 )
				throw new ArgumentOutOfRangeException ( "max" );
			_rand.GetBytes ( _rd4 );
			int val = BitConverter.ToInt32 ( _rd4, 0 ) % max;
			if ( val < 0 )
				val = -val;
			return val;
		}

		public int Next( int min, int max )
		{
			if ( min > max )
				throw new ArgumentOutOfRangeException ( "max" );
			return Next ( max - min + 1 ) + min;
		}

		public double NextDouble()
		{
			_rand.GetBytes ( _rd8 );
			return BitConverter.ToUInt64 ( _rd8, 0 ) / ( double ) UInt64.MaxValue;
		}
		
		public bool TrueOrFalse()
		{
			return Next( 0, 2 ) == 1 ? true : false;
		}
	}
}
