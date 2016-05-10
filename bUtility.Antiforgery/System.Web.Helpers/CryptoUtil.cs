using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;

namespace System.Web.Helpers
{
	internal static class CryptoUtil
	{
		public static bool AreByteArraysEqual(byte[] a, byte[] b)
		{
			if (a == null || b == null || a.Length != b.Length)
			{
				return false;
			}
			bool flag = true;
			for (int i = 0; i < a.Length; i++)
			{
				flag &= (a[i] == b[i]);
			}
			return flag;
		}

		public static byte[] ComputeSHA256(IList<string> parameters)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
				{
					foreach (string current in parameters)
					{
						binaryWriter.Write(current);
					}
					binaryWriter.Flush();
					using (SHA256Cng sHA256Cng = new SHA256Cng())
					{
						byte[] array = sHA256Cng.ComputeHash(memoryStream.GetBuffer(), 0, checked((int)memoryStream.Length));
						result = array;
					}
				}
			}
			return result;
		}
	}
}
