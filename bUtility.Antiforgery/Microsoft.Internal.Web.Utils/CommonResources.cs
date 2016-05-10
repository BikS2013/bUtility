using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Microsoft.Internal.Web.Utils
{
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0"), DebuggerNonUserCode, CompilerGenerated]
	internal class CommonResources
	{
		private static ResourceManager resourceMan;

		private static CultureInfo resourceCulture;

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static ResourceManager ResourceManager
		{
			get
			{
				if (object.ReferenceEquals(CommonResources.resourceMan, null))
				{
					string text = (from s in Assembly.GetExecutingAssembly().GetManifestResourceNames()
					where s.EndsWith("CommonResources.resources", StringComparison.OrdinalIgnoreCase)
					select s).Single<string>();
					text = text.Substring(0, text.Length - 10);
					ResourceManager resourceManager = new ResourceManager(text, typeof(CommonResources).Assembly);
					CommonResources.resourceMan = resourceManager;
				}
				return CommonResources.resourceMan;
			}
		}

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static CultureInfo Culture
		{
			get
			{
				return CommonResources.resourceCulture;
			}
			set
			{
				CommonResources.resourceCulture = value;
			}
		}

		internal static string Argument_Cannot_Be_Null_Or_Empty
		{
			get
			{
				return CommonResources.ResourceManager.GetString("Argument_Cannot_Be_Null_Or_Empty", CommonResources.resourceCulture);
			}
		}

		internal static string Argument_Must_Be_Between
		{
			get
			{
				return CommonResources.ResourceManager.GetString("Argument_Must_Be_Between", CommonResources.resourceCulture);
			}
		}

		internal static string Argument_Must_Be_Enum_Member
		{
			get
			{
				return CommonResources.ResourceManager.GetString("Argument_Must_Be_Enum_Member", CommonResources.resourceCulture);
			}
		}

		internal static string Argument_Must_Be_GreaterThan
		{
			get
			{
				return CommonResources.ResourceManager.GetString("Argument_Must_Be_GreaterThan", CommonResources.resourceCulture);
			}
		}

		internal static string Argument_Must_Be_GreaterThanOrEqualTo
		{
			get
			{
				return CommonResources.ResourceManager.GetString("Argument_Must_Be_GreaterThanOrEqualTo", CommonResources.resourceCulture);
			}
		}

		internal static string Argument_Must_Be_LessThan
		{
			get
			{
				return CommonResources.ResourceManager.GetString("Argument_Must_Be_LessThan", CommonResources.resourceCulture);
			}
		}

		internal static string Argument_Must_Be_LessThanOrEqualTo
		{
			get
			{
				return CommonResources.ResourceManager.GetString("Argument_Must_Be_LessThanOrEqualTo", CommonResources.resourceCulture);
			}
		}

		internal static string Argument_Must_Be_Null_Or_Non_Empty
		{
			get
			{
				return CommonResources.ResourceManager.GetString("Argument_Must_Be_Null_Or_Non_Empty", CommonResources.resourceCulture);
			}
		}

		internal CommonResources()
		{
		}
	}
}
