using System.Collections.Generic;

namespace bUtility.ReverseProxy
{
    public class ReverseProxyConfig
    {
        public string Name { get; set; }

        public List<RouteConfig> RouteTemplatesList { get; set; }

        public string SourceUri { get; set; }

        public string TargetUri { get; set; }
    }
}
