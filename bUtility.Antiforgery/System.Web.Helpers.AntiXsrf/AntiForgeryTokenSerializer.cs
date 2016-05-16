using System;
using System.IO;
using System.Web.Mvc;

namespace System.Web.Helpers.AntiXsrf
{
	internal sealed class AntiForgeryTokenSerializer : IAntiForgeryTokenSerializer
	{
		private const byte TokenVersion = 1;

		private readonly ICryptoSystem _cryptoSystem;

		internal AntiForgeryTokenSerializer(ICryptoSystem cryptoSystem)
		{
			this._cryptoSystem = cryptoSystem;
		}

		public AntiForgeryToken Deserialize(string serializedToken)
		{
			try
			{
				using (MemoryStream memoryStream = new MemoryStream(this._cryptoSystem.Unprotect(serializedToken)))
				{
					using (BinaryReader binaryReader = new BinaryReader(memoryStream))
					{
						AntiForgeryToken antiForgeryToken = AntiForgeryTokenSerializer.DeserializeImpl(binaryReader);
						if (antiForgeryToken != null)
						{
							return antiForgeryToken;
						}
					}
				}
			}
			catch
			{
			}
			throw HttpAntiForgeryException.CreateDeserializationFailedException();
		}

		private static AntiForgeryToken DeserializeImpl(BinaryReader reader)
		{
			byte b = reader.ReadByte();
			if (b != 1)
			{
				return null;
			}
			AntiForgeryToken antiForgeryToken = new AntiForgeryToken();
			byte[] data = reader.ReadBytes(16);
			antiForgeryToken.SecurityToken = new BinaryBlob(128, data);
			antiForgeryToken.IsSessionToken = reader.ReadBoolean();
			if (!antiForgeryToken.IsSessionToken)
			{
				bool flag = reader.ReadBoolean();
				if (flag)
				{
					byte[] data2 = reader.ReadBytes(32);
					antiForgeryToken.ClaimUid = new BinaryBlob(256, data2);
				}
				else
				{
					antiForgeryToken.Username = reader.ReadString();
				}
				antiForgeryToken.AdditionalData = reader.ReadString();
			}
			if (reader.BaseStream.ReadByte() != -1)
			{
				return null;
			}
			return antiForgeryToken;
		}

		public string Serialize(AntiForgeryToken token)
		{
			string result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
				{
					binaryWriter.Write(1);
					binaryWriter.Write(token.SecurityToken.GetData());
					binaryWriter.Write(token.IsSessionToken);
					if (!token.IsSessionToken)
					{
						if (token.ClaimUid != null)
						{
							binaryWriter.Write(true);
							binaryWriter.Write(token.ClaimUid.GetData());
						}
						else
						{
							binaryWriter.Write(false);
							binaryWriter.Write(token.Username);
						}
						binaryWriter.Write(token.AdditionalData);
					}
					binaryWriter.Flush();
					result = this._cryptoSystem.Protect(memoryStream.ToArray());
				}
			}
			return result;
		}
	}
}
