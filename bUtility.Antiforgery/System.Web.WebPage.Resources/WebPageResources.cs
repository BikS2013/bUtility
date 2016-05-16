using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace System.Web.WebPages.Resources
{
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0"), DebuggerNonUserCode, CompilerGenerated]
	internal class WebPageResources
	{
		private static ResourceManager resourceMan;

		private static CultureInfo resourceCulture;

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static ResourceManager ResourceManager
		{
			get
			{
				if (object.ReferenceEquals(WebPageResources.resourceMan, null))
				{
					ResourceManager resourceManager = new ResourceManager("System.Web.WebPages.Resources.WebPageResources", typeof(WebPageResources).Assembly);
					WebPageResources.resourceMan = resourceManager;
				}
				return WebPageResources.resourceMan;
			}
		}

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static CultureInfo Culture
		{
			get
			{
				return WebPageResources.resourceCulture;
			}
			set
			{
				WebPageResources.resourceCulture = value;
			}
		}

		internal static string AntiForgeryToken_AdditionalDataCheckFailed
		{
			get
			{
				return WebPageResources.ResourceManager.GetString("AntiForgeryToken_AdditionalDataCheckFailed", WebPageResources.resourceCulture);
			}
		}

		internal static string AntiForgeryToken_ClaimUidMismatch
		{
			get
			{
				return WebPageResources.ResourceManager.GetString("AntiForgeryToken_ClaimUidMismatch", WebPageResources.resourceCulture);
			}
		}

		internal static string AntiForgeryToken_CookieMissing
		{
			get
			{
				return WebPageResources.ResourceManager.GetString("AntiForgeryToken_CookieMissing", WebPageResources.resourceCulture);
			}
		}

		internal static string AntiForgeryToken_DeserializationFailed
		{
			get
			{
				return WebPageResources.ResourceManager.GetString("AntiForgeryToken_DeserializationFailed", WebPageResources.resourceCulture);
			}
		}

		internal static string AntiForgeryToken_FormFieldMissing
		{
			get
			{
				return WebPageResources.ResourceManager.GetString("AntiForgeryToken_FormFieldMissing", WebPageResources.resourceCulture);
			}
		}

		internal static string AntiForgeryToken_SecurityTokenMismatch
		{
			get
			{
				return WebPageResources.ResourceManager.GetString("AntiForgeryToken_SecurityTokenMismatch", WebPageResources.resourceCulture);
			}
		}

		internal static string AntiForgeryToken_TokensSwapped
		{
			get
			{
				return WebPageResources.ResourceManager.GetString("AntiForgeryToken_TokensSwapped", WebPageResources.resourceCulture);
			}
		}

		internal static string AntiForgeryToken_UsernameMismatch
		{
			get
			{
				return WebPageResources.ResourceManager.GetString("AntiForgeryToken_UsernameMismatch", WebPageResources.resourceCulture);
			}
		}

		internal static string AntiForgeryWorker_RequireSSL
		{
			get
			{
				return WebPageResources.ResourceManager.GetString("AntiForgeryWorker_RequireSSL", WebPageResources.resourceCulture);
			}
		}

		internal static string ApplicationPart_ModuleAlreadyRegistered
		{
			get
			{
				return WebPageResources.ResourceManager.GetString("ApplicationPart_ModuleAlreadyRegistered", WebPageResources.resourceCulture);
			}
		}

		internal static string ApplicationPart_ModuleAlreadyRegisteredForVirtualPath
		{
			get
			{
				return WebPageResources.ResourceManager.GetString("ApplicationPart_ModuleAlreadyRegisteredForVirtualPath", WebPageResources.resourceCulture);
			}
		}

		internal static string ApplicationPart_ModuleCannotBeFound
		{
			get
			{
				return WebPageResources.ResourceManager.GetString("ApplicationPart_ModuleCannotBeFound", WebPageResources.resourceCulture);
			}
		}

		internal static string ApplicationPart_ModuleNotRegistered
		{
			get
			{
				return WebPageResources.ResourceManager.GetString("ApplicationPart_ModuleNotRegistered", WebPageResources.resourceCulture);
			}
		}

		internal static string ApplicationPart_ResourceNotFound
		{
			get
			{
				return WebPageResources.ResourceManager.GetString("ApplicationPart_ResourceNotFound", WebPageResources.resourceCulture);
			}
		}

		internal static string ClaimUidExtractor_ClaimNotPresent
		{
			get
			{
				return WebPageResources.ResourceManager.GetString("ClaimUidExtractor_ClaimNotPresent", WebPageResources.resourceCulture);
			}
		}

		internal static string ClaimUidExtractor_DefaultClaimsNotPresent
		{
			get
			{
				return WebPageResources.ResourceManager.GetString("ClaimUidExtractor_DefaultClaimsNotPresent", WebPageResources.resourceCulture);
			}
		}

		internal static string DynamicDictionary_InvalidNumberOfIndexes
		{
			get
			{
				return WebPageResources.ResourceManager.GetString("DynamicDictionary_InvalidNumberOfIndexes", WebPageResources.resourceCulture);
			}
		}

		internal static string DynamicHttpApplicationState_UseOnlyStringOrIntToGet
		{
			get
			{
				return WebPageResources.ResourceManager.GetString("DynamicHttpApplicationState_UseOnlyStringOrIntToGet", WebPageResources.resourceCulture);
			}
		}

		internal static string DynamicHttpApplicationState_UseOnlyStringToSet
		{
			get
			{
				return WebPageResources.ResourceManager.GetString("DynamicHttpApplicationState_UseOnlyStringToSet", WebPageResources.resourceCulture);
			}
		}

		internal static string HtmlHelper_ConversionThrew
		{
			get
			{
				return WebPageResources.ResourceManager.GetString("HtmlHelper_ConversionThrew", WebPageResources.resourceCulture);
			}
		}

		internal static string HtmlHelper_NoConverterExists
		{
			get
			{
				return WebPageResources.ResourceManager.GetString("HtmlHelper_NoConverterExists", WebPageResources.resourceCulture);
			}
		}

		internal static string HttpContextUnavailable
		{
			get
			{
				return WebPageResources.ResourceManager.GetString("HttpContextUnavailable", WebPageResources.resourceCulture);
			}
		}

		internal static string SessionState_InvalidValue
		{
			get
			{
				return WebPageResources.ResourceManager.GetString("SessionState_InvalidValue", WebPageResources.resourceCulture);
			}
		}

		internal static string SessionState_TooManyValues
		{
			get
			{
				return WebPageResources.ResourceManager.GetString("SessionState_TooManyValues", WebPageResources.resourceCulture);
			}
		}

		internal static string StateStorage_RequestScopeNotAvailable
		{
			get
			{
				return WebPageResources.ResourceManager.GetString("StateStorage_RequestScopeNotAvailable", WebPageResources.resourceCulture);
			}
		}

		internal static string StateStorage_ScopeIsReadOnly
		{
			get
			{
				return WebPageResources.ResourceManager.GetString("StateStorage_ScopeIsReadOnly", WebPageResources.resourceCulture);
			}
		}

		internal static string StateStorage_StorageScopesCannotBeCreated
		{
			get
			{
				return WebPageResources.ResourceManager.GetString("StateStorage_StorageScopesCannotBeCreated", WebPageResources.resourceCulture);
			}
		}

		internal static string TokenValidator_AuthenticatedUserWithoutUsername
		{
			get
			{
				return WebPageResources.ResourceManager.GetString("TokenValidator_AuthenticatedUserWithoutUsername", WebPageResources.resourceCulture);
			}
		}

		internal static string UnobtrusiveJavascript_ValidationParameterCannotBeEmpty
		{
			get
			{
				return WebPageResources.ResourceManager.GetString("UnobtrusiveJavascript_ValidationParameterCannotBeEmpty", WebPageResources.resourceCulture);
			}
		}

		internal static string UnobtrusiveJavascript_ValidationParameterMustBeLegal
		{
			get
			{
				return WebPageResources.ResourceManager.GetString("UnobtrusiveJavascript_ValidationParameterMustBeLegal", WebPageResources.resourceCulture);
			}
		}

		internal static string UnobtrusiveJavascript_ValidationTypeCannotBeEmpty
		{
			get
			{
				return WebPageResources.ResourceManager.GetString("UnobtrusiveJavascript_ValidationTypeCannotBeEmpty", WebPageResources.resourceCulture);
			}
		}

		internal static string UnobtrusiveJavascript_ValidationTypeMustBeLegal
		{
			get
			{
				return WebPageResources.ResourceManager.GetString("UnobtrusiveJavascript_ValidationTypeMustBeLegal", WebPageResources.resourceCulture);
			}
		}

		internal static string UnobtrusiveJavascript_ValidationTypeMustBeUnique
		{
			get
			{
				return WebPageResources.ResourceManager.GetString("UnobtrusiveJavascript_ValidationTypeMustBeUnique", WebPageResources.resourceCulture);
			}
		}

		internal static string UrlData_ReadOnly
		{
			get
			{
				return WebPageResources.ResourceManager.GetString("UrlData_ReadOnly", WebPageResources.resourceCulture);
			}
		}

		internal static string ValidationDefault_DataType
		{
			get
			{
				return WebPageResources.ResourceManager.GetString("ValidationDefault_DataType", WebPageResources.resourceCulture);
			}
		}

		internal static string ValidationDefault_EqualsTo
		{
			get
			{
				return WebPageResources.ResourceManager.GetString("ValidationDefault_EqualsTo", WebPageResources.resourceCulture);
			}
		}

		internal static string ValidationDefault_FloatRange
		{
			get
			{
				return WebPageResources.ResourceManager.GetString("ValidationDefault_FloatRange", WebPageResources.resourceCulture);
			}
		}

		internal static string ValidationDefault_IntegerRange
		{
			get
			{
				return WebPageResources.ResourceManager.GetString("ValidationDefault_IntegerRange", WebPageResources.resourceCulture);
			}
		}

		internal static string ValidationDefault_Regex
		{
			get
			{
				return WebPageResources.ResourceManager.GetString("ValidationDefault_Regex", WebPageResources.resourceCulture);
			}
		}

		internal static string ValidationDefault_Required
		{
			get
			{
				return WebPageResources.ResourceManager.GetString("ValidationDefault_Required", WebPageResources.resourceCulture);
			}
		}

		internal static string ValidationDefault_StringLength
		{
			get
			{
				return WebPageResources.ResourceManager.GetString("ValidationDefault_StringLength", WebPageResources.resourceCulture);
			}
		}

		internal static string ValidationDefault_StringLengthRange
		{
			get
			{
				return WebPageResources.ResourceManager.GetString("ValidationDefault_StringLengthRange", WebPageResources.resourceCulture);
			}
		}

		internal static string WebPage_CannotRequestDirectly
		{
			get
			{
				return WebPageResources.ResourceManager.GetString("WebPage_CannotRequestDirectly", WebPageResources.resourceCulture);
			}
		}

		internal static string WebPage_FileNotSupported
		{
			get
			{
				return WebPageResources.ResourceManager.GetString("WebPage_FileNotSupported", WebPageResources.resourceCulture);
			}
		}

		internal static string WebPage_InvalidPageType
		{
			get
			{
				return WebPageResources.ResourceManager.GetString("WebPage_InvalidPageType", WebPageResources.resourceCulture);
			}
		}

		internal static string WebPage_LayoutPageNotFound
		{
			get
			{
				return WebPageResources.ResourceManager.GetString("WebPage_LayoutPageNotFound", WebPageResources.resourceCulture);
			}
		}

		internal static string WebPage_RenderBodyAlreadyCalled
		{
			get
			{
				return WebPageResources.ResourceManager.GetString("WebPage_RenderBodyAlreadyCalled", WebPageResources.resourceCulture);
			}
		}

		internal static string WebPage_RenderBodyNotCalled
		{
			get
			{
				return WebPageResources.ResourceManager.GetString("WebPage_RenderBodyNotCalled", WebPageResources.resourceCulture);
			}
		}

		internal static string WebPage_SectionAleadyDefined
		{
			get
			{
				return WebPageResources.ResourceManager.GetString("WebPage_SectionAleadyDefined", WebPageResources.resourceCulture);
			}
		}

		internal static string WebPage_SectionAleadyRendered
		{
			get
			{
				return WebPageResources.ResourceManager.GetString("WebPage_SectionAleadyRendered", WebPageResources.resourceCulture);
			}
		}

		internal static string WebPage_SectionNotDefined
		{
			get
			{
				return WebPageResources.ResourceManager.GetString("WebPage_SectionNotDefined", WebPageResources.resourceCulture);
			}
		}

		internal static string WebPage_SectionsNotRendered
		{
			get
			{
				return WebPageResources.ResourceManager.GetString("WebPage_SectionsNotRendered", WebPageResources.resourceCulture);
			}
		}

		internal static string WebPageRoute_UnderscoreBlocked
		{
			get
			{
				return WebPageResources.ResourceManager.GetString("WebPageRoute_UnderscoreBlocked", WebPageResources.resourceCulture);
			}
		}

		internal WebPageResources()
		{
		}
	}
}
