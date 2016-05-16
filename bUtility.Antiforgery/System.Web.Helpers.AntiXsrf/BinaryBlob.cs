using System;
using System.Diagnostics;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace System.Web.Helpers.AntiXsrf
{
	[DebuggerDisplay("{DebuggerString}")]
	internal sealed class BinaryBlob : IEquatable<BinaryBlob>
	{
		private static readonly RNGCryptoServiceProvider _prng = new RNGCryptoServiceProvider();

		private readonly byte[] _data;

		public int BitLength
		{
			get
			{
				return checked(this._data.Length * 8);
			}
		}

		private string DebuggerString
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder("0x", 2 + this._data.Length * 2);
				for (int i = 0; i < this._data.Length; i++)
				{
					stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "{0:x2}", new object[]
					{
						this._data[i]
					});
				}
				return stringBuilder.ToString();
			}
		}

		public BinaryBlob(int bitLength) : this(bitLength, BinaryBlob.GenerateNewToken(bitLength))
		{
		}

		public BinaryBlob(int bitLength, byte[] data)
		{
			if (bitLength < 32 || bitLength % 8 != 0)
			{
				throw new ArgumentOutOfRangeException("bitLength");
			}
			if (data == null || data.Length != bitLength / 8)
			{
				throw new ArgumentOutOfRangeException("data");
			}
			this._data = data;
		}

		public override bool Equals(object obj)
		{
			return this.Equals(obj as BinaryBlob);
		}

		public bool Equals(BinaryBlob other)
		{
			return other != null && CryptoUtil.AreByteArraysEqual(this._data, other._data);
		}

		public byte[] GetData()
		{
			return this._data;
		}

		public override int GetHashCode()
		{
			return BitConverter.ToInt32(this._data, 0);
		}

		private static byte[] GenerateNewToken(int bitLength)
		{
			byte[] array = new byte[bitLength / 8];
			BinaryBlob._prng.GetBytes(array);
			return array;
		}
	}
}
