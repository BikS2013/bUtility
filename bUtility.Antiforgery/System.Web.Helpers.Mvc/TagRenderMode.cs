using System;
using System.Runtime.CompilerServices;

namespace System.Web.Mvc
{
	/// <summary>Enumerates the modes that are available for rendering HTML tags.</summary>
	[TypeForwardedFrom("System.Web.Mvc, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35")]
	public enum TagRenderMode
	{
		/// <summary>Represents the mode for rendering normal text.</summary>
		Normal,
		/// <summary>Represents the mode for rendering an opening tag (for example, &lt;tag&gt;).</summary>
		StartTag,
		/// <summary>Represents the mode for rendering a closing tag (for example, &lt;/tag&gt;).</summary>
		EndTag,
		/// <summary>Represents the mode for rendering a self-closing tag (for example, &lt;tag /&gt;).</summary>
		SelfClosing
	}
}
