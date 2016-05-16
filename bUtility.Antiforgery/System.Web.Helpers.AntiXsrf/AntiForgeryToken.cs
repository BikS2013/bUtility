using System;

namespace System.Web.Helpers.AntiXsrf
{
	internal sealed class AntiForgeryToken
	{
		internal const int SecurityTokenBitLength = 128;

		internal const int ClaimUidBitLength = 256;

		private string _additionalData;

		private BinaryBlob _securityToken;

		private string _username;

		public string AdditionalData
		{
			get
			{
				return this._additionalData ?? string.Empty;
			}
			set
			{
				this._additionalData = value;
			}
		}

		public BinaryBlob ClaimUid
		{
			get;
			set;
		}

		public bool IsSessionToken
		{
			get;
			set;
		}

		public BinaryBlob SecurityToken
		{
			get
			{
				if (this._securityToken == null)
				{
					this._securityToken = new BinaryBlob(128);
				}
				return this._securityToken;
			}
			set
			{
				this._securityToken = value;
			}
		}

		public string Username
		{
			get
			{
				return this._username ?? string.Empty;
			}
			set
			{
				this._username = value;
			}
		}
	}
}
