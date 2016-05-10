
namespace System.Web.Mvc
{
    /// <summary>Supports the rendering of HTML controls in a view.</summary>
    public class HtmlHelper
    {

        /// <summary>Gets or sets the character that replaces periods in the ID attribute of an element.</summary>
        /// <returns>The character that replaces periods in the ID attribute of an element.</returns>
        public static string IdAttributeDotReplacement
        {
            get
            {
                return "_";
            }
        }

    }
}
