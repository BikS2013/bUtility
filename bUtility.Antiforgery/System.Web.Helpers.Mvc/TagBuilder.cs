using Microsoft.Internal.Web.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;

namespace System.Web.Mvc
{
	/// <summary>Contains classes and properties that are used to create HTML elements. This class is used to write helpers, such as those found in the <see cref="N:System.Web.Helpers" /> namespace.</summary>
	[TypeForwardedFrom("System.Web.Mvc, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35")]
	public class TagBuilder
	{
		private static class Html401IdUtil
		{
			private static bool IsAllowableSpecialCharacter(char c)
			{
				return c == '-' || c == ':' || c == '_';
			}

			private static bool IsDigit(char c)
			{
				return '0' <= c && c <= '9';
			}

			public static bool IsLetter(char c)
			{
				return ('A' <= c && c <= 'Z') || ('a' <= c && c <= 'z');
			}

			public static bool IsValidIdCharacter(char c)
			{
				return TagBuilder.Html401IdUtil.IsLetter(c) || TagBuilder.Html401IdUtil.IsDigit(c) || TagBuilder.Html401IdUtil.IsAllowableSpecialCharacter(c);
			}
		}

		private string _idAttributeDotReplacement;

		private string _innerHtml;

		/// <summary>Gets the collection of attributes.</summary>
		/// <returns>The collection of attributes.</returns>
		public IDictionary<string, string> Attributes
		{
			get;
			private set;
		}

		/// <summary>Gets or sets a string that can be used to replace invalid HTML characters.</summary>
		/// <returns>The string to use to replace invalid HTML characters.</returns>
		public string IdAttributeDotReplacement
		{
			get
			{
				if (string.IsNullOrEmpty(this._idAttributeDotReplacement))
				{
					this._idAttributeDotReplacement = HtmlHelper.IdAttributeDotReplacement;
				}
				return this._idAttributeDotReplacement;
			}
			set
			{
				this._idAttributeDotReplacement = value;
			}
		}

		/// <summary>Gets or sets the inner HTML value for the element.</summary>
		/// <returns>The inner HTML value for the element.</returns>
		public string InnerHtml
		{
			get
			{
				return this._innerHtml ?? string.Empty;
			}
			set
			{
				this._innerHtml = value;
			}
		}

		/// <summary>Gets the tag name for this tag.</summary>
		/// <returns>The name.</returns>
		public string TagName
		{
			get;
			private set;
		}

		/// <summary>Creates a new tag that has the specified tag name.</summary>
		/// <param name="tagName">The tag name without the "&lt;", "/", or "&gt;" delimiters.</param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="tagName" /> is null or empty.</exception>
		public TagBuilder(string tagName)
		{
			if (string.IsNullOrEmpty(tagName))
			{
				throw new ArgumentException(CommonResources.Argument_Cannot_Be_Null_Or_Empty, "tagName");
			}
			this.TagName = tagName;
			this.Attributes = new SortedDictionary<string, string>(StringComparer.Ordinal);
		}

		/// <summary>Adds a CSS class to the list of CSS classes in the tag.</summary>
		/// <param name="value">The CSS class to add.</param>
		public void AddCssClass(string value)
		{
			string str;
			if (this.Attributes.TryGetValue("class", out str))
			{
				this.Attributes["class"] = value + " " + str;
				return;
			}
			this.Attributes["class"] = value;
		}

		/// <summary>Replaces each invalid character in the tag ID with a valid HTML character.</summary>
		/// <returns>The sanitized tag ID, or null if <paramref name="originalId" /> is null or empty, or if <paramref name="originalId" /> does not begin with a letter.</returns>
		/// <param name="originalId">The ID that might contain characters to replace.</param>
		public static string CreateSanitizedId(string originalId)
		{
			return TagBuilder.CreateSanitizedId(originalId, HtmlHelper.IdAttributeDotReplacement);
		}

		/// <summary>Replaces each invalid character in the tag ID with the specified replacement string.</summary>
		/// <returns>The sanitized tag ID, or null if <paramref name="originalId" /> is null or empty, or if <paramref name="originalId" /> does not begin with a letter.</returns>
		/// <param name="originalId">The ID that might contain characters to replace.</param>
		/// <param name="invalidCharReplacement">The replacement string.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="invalidCharReplacement" /> is null.</exception>
		public static string CreateSanitizedId(string originalId, string invalidCharReplacement)
		{
			if (string.IsNullOrEmpty(originalId))
			{
				return null;
			}
			if (invalidCharReplacement == null)
			{
				throw new ArgumentNullException("invalidCharReplacement");
			}
			char c = originalId[0];
			if (!TagBuilder.Html401IdUtil.IsLetter(c))
			{
				return null;
			}
			StringBuilder stringBuilder = new StringBuilder(originalId.Length);
			stringBuilder.Append(c);
			for (int i = 1; i < originalId.Length; i++)
			{
				char c2 = originalId[i];
				if (TagBuilder.Html401IdUtil.IsValidIdCharacter(c2))
				{
					stringBuilder.Append(c2);
				}
				else
				{
					stringBuilder.Append(invalidCharReplacement);
				}
			}
			return stringBuilder.ToString();
		}

		/// <summary>Generates a sanitized ID attribute for the tag by using the specified name.</summary>
		/// <param name="name">The name to use to generate an ID attribute.</param>
		public void GenerateId(string name)
		{
			if (!this.Attributes.ContainsKey("id"))
			{
				string value = TagBuilder.CreateSanitizedId(name, this.IdAttributeDotReplacement);
				if (!string.IsNullOrEmpty(value))
				{
					this.Attributes["id"] = value;
				}
			}
		}

		private void AppendAttributes(StringBuilder sb)
		{
			foreach (KeyValuePair<string, string> current in this.Attributes)
			{
				string key = current.Key;
				if (!string.Equals(key, "id", StringComparison.Ordinal) || !string.IsNullOrEmpty(current.Value))
				{
					string value = HttpUtility.HtmlAttributeEncode(current.Value);
					sb.Append(' ').Append(key).Append("=\"").Append(value).Append('"');
				}
			}
		}

		/// <summary>Adds a new attribute to the tag.</summary>
		/// <param name="key">The key for the attribute.</param>
		/// <param name="value">The value of the attribute.</param>
		public void MergeAttribute(string key, string value)
		{
			this.MergeAttribute(key, value, false);
		}

		/// <summary>Adds a new attribute or optionally replaces an existing attribute in the opening tag.</summary>
		/// <param name="key">The key for the attribute.</param>
		/// <param name="value">The value of the attribute.</param>
		/// <param name="replaceExisting">true to replace an existing attribute if an attribute exists that has the specified <paramref name="key" /> value, or false to leave the original attribute unchanged.</param>
		public void MergeAttribute(string key, string value, bool replaceExisting)
		{
			if (string.IsNullOrEmpty(key))
			{
				throw new ArgumentException(CommonResources.Argument_Cannot_Be_Null_Or_Empty, "key");
			}
			if (replaceExisting || !this.Attributes.ContainsKey(key))
			{
				this.Attributes[key] = value;
			}
		}

		/// <summary>Adds new attributes to the tag.</summary>
		/// <param name="attributes">The collection of attributes to add.</param>
		/// <typeparam name="TKey">The type of the key object.</typeparam>
		/// <typeparam name="TValue">The type of the value object.</typeparam>
		public void MergeAttributes<TKey, TValue>(IDictionary<TKey, TValue> attributes)
		{
			this.MergeAttributes<TKey, TValue>(attributes, false);
		}

		/// <summary>Adds new attributes or optionally replaces existing attributes in the tag.</summary>
		/// <param name="attributes">The collection of attributes to add or replace.</param>
		/// <param name="replaceExisting">For each attribute in <paramref name="attributes" />, true to replace the attribute if an attribute already exists that has the same key, or false to leave the original attribute unchanged.</param>
		/// <typeparam name="TKey">The type of the key object.</typeparam>
		/// <typeparam name="TValue">The type of the value object.</typeparam>
		public void MergeAttributes<TKey, TValue>(IDictionary<TKey, TValue> attributes, bool replaceExisting)
		{
			if (attributes != null)
			{
				foreach (KeyValuePair<TKey, TValue> current in attributes)
				{
					string key = Convert.ToString(current.Key, CultureInfo.InvariantCulture);
					string value = Convert.ToString(current.Value, CultureInfo.InvariantCulture);
					this.MergeAttribute(key, value, replaceExisting);
				}
			}
		}

		/// <summary>Sets the <see cref="P:System.Web.Mvc.TagBuilder.InnerHtml" /> property of the element to an HTML-encoded version of the specified string.</summary>
		/// <param name="innerText">The string to HTML-encode.</param>
		public void SetInnerText(string innerText)
		{
			this.InnerHtml = HttpUtility.HtmlEncode(innerText);
		}

		internal HtmlString ToHtmlString(TagRenderMode renderMode)
		{
			return new HtmlString(this.ToString(renderMode));
		}

		/// <summary>Renders the element as a <see cref="F:System.Web.Mvc.TagRenderMode.Normal" /> element.</summary>
		public override string ToString()
		{
			return this.ToString(TagRenderMode.Normal);
		}

		/// <summary>Renders the HTML tag by using the specified render mode.</summary>
		/// <returns>The rendered HTML tag.</returns>
		/// <param name="renderMode">The render mode.</param>
		public string ToString(TagRenderMode renderMode)
		{
			StringBuilder stringBuilder = new StringBuilder();
			switch (renderMode)
			{
			case TagRenderMode.StartTag:
				stringBuilder.Append('<').Append(this.TagName);
				this.AppendAttributes(stringBuilder);
				stringBuilder.Append('>');
				break;
			case TagRenderMode.EndTag:
				stringBuilder.Append("</").Append(this.TagName).Append('>');
				break;
			case TagRenderMode.SelfClosing:
				stringBuilder.Append('<').Append(this.TagName);
				this.AppendAttributes(stringBuilder);
				stringBuilder.Append(" />");
				break;
			default:
				stringBuilder.Append('<').Append(this.TagName);
				this.AppendAttributes(stringBuilder);
				stringBuilder.Append('>').Append(this.InnerHtml).Append("</").Append(this.TagName).Append('>');
				break;
			}
			return stringBuilder.ToString();
		}
	}
}
