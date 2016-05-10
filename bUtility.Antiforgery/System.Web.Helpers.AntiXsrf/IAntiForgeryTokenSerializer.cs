using System;

namespace System.Web.Helpers.AntiXsrf
{
	internal interface IAntiForgeryTokenSerializer
	{
		AntiForgeryToken Deserialize(string serializedToken);

		string Serialize(AntiForgeryToken token);
	}
}
