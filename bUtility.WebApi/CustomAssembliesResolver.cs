using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;

namespace bUtility.WebApi
{
    public class CustomAssembliesResolver : DefaultAssembliesResolver
    {
        public override ICollection<Assembly> GetAssemblies()
        {
            var appPath = AppDomain.CurrentDomain.BaseDirectory;
            var baseAssemblies = base.GetAssemblies();

            var assemblies = new List<Assembly>(baseAssemblies);
            var files = Directory.GetFiles(appPath + @"\bin\Libraries", "*.dll",
                            SearchOption.AllDirectories);
            var customAssemblies = files.Select(Assembly.LoadFile);

            // register Web API controllers
            var apiControllerAssemblies =
                from assembly in customAssemblies
                where !assembly.IsDynamic
                from type in assembly.GetExportedTypes()
                where typeof(IHttpController).IsAssignableFrom(type)
                where !type.IsAbstract
                where !type.IsGenericTypeDefinition
                where type.Name.EndsWith("Controller", StringComparison.Ordinal)
                select assembly;

            foreach (var assembly in apiControllerAssemblies)
            {
                baseAssemblies.Add(assembly);
            }

            return baseAssemblies;
        }
    }
}
