using bUtility.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace bUtility.ReverseProxy
{
    public static class RouteConfigExtensions
    {
        public static void CheckReverseProxyConfiguration(this List<ReverseProxyConfig> routesList, ILogger logger)
        {
            var clientNameHash = new HashSet<string>();
            var routeNameHash = new HashSet<string>();
            var routeTemplatesHash = new HashSet<string>();
            var urisHash = new HashSet<string>();

            foreach (var rc in routesList)
            {
                if (rc == null)
                {
                    logger.Error("Could not retrieve part of route configuration");
                    throw new ArgumentNullException(nameof(rc), "Could not retrieve part of route configuration");
                } 
                CheckName(logger, rc, clientNameHash);
                CheckRouteTemplates(logger, rc, routeTemplatesHash, routeNameHash);
                CheckUris(logger, rc, urisHash);
            }

            clientNameHash.Clear();
            routeNameHash.Clear();
            routeTemplatesHash.Clear();
            urisHash.Clear();
        }

        #region Checks
        private static void CheckName(ILogger logger, ReverseProxyConfig rc, HashSet<string> routesHash)
        {
            if (rc.Name.Clear() == null)
                throw new ArgumentNullException(nameof(rc.Name));
            if (!routesHash.Add(rc.Name))
            {
                logger.Error("Duplicate configuration names have been found");
                throw new InvalidOperationException("Duplicate configuration names have been found");
            }
        }

        private static void CheckRouteTemplates(ILogger logger, ReverseProxyConfig rc, 
            HashSet<string> templatesHash, HashSet<string> routeNameHash)
        {  
            foreach (var template in rc.RouteTemplatesList)
            {
                if (template == null)
                {

                    logger.Error($"Route configuration not registered for {rc.Name}");
                    throw new InvalidOperationException($"Route configuration not registered for {rc.Name}");
                }

                if (template.RouteName.Clear() == null)
                {
                    logger.Error($"Route template name not registered for {rc.Name}");
                    throw new InvalidOperationException($"Route template name not registered for {rc.Name}");
                }

                if (!routeNameHash.Add(template.RouteName.Clear()))
                {
                    logger.Error("Duplicate route template name found");
                    throw new InvalidOperationException("Duplicate route template name found");
                }

                if (template.RouteTemplate.Clear() == null)
                {
                    logger.Error($"Route template for {template.RouteName} not registered");
                    throw new InvalidOperationException($"Route template for {template.RouteName} not registered");
                }

                if (!templatesHash.Add(template.RouteTemplate.Clear()))
                {
                    logger.Error($"Duplicate route template configuration found for {template.RouteName}");
                    throw new InvalidOperationException($"Duplicate route template configuration found for {template.RouteName}");
                }
            }
        }

        private static void CheckUris(ILogger logger, ReverseProxyConfig rc, HashSet<string> urisHash)
        {
            if (rc.SourceUri.Clear() == null)
            {
                logger.Error("Empty source uri");
                throw new ArgumentNullException(nameof(rc.SourceUri), "Empty source uri");
            }

            if (rc.TargetUri.Clear() == null)
            {
                logger.Error("Empty target uri");
                throw new ArgumentNullException(nameof(rc.TargetUri), "Empty target uri");
            }

            if (rc.SourceUri.Clear().Equals(rc.TargetUri.Clear()))
            {
                logger.Error($"Identical SourceUri and TargetUri detected for {rc.Name}");
                throw new InvalidOperationException($"Identical SourceUri and TargetUri detected for {rc.Name}");
            }

            if (!rc.SourceUri.Clear().IsValidUrl())
            {
                logger.Error($"Invalid SourceUri for {rc.Name}");
                throw new InvalidOperationException($"Invalid SourceUri for {rc.Name}");
            }

            if (!rc.TargetUri.Clear().IsValidUrl())
            {
                logger.Error($"Invalid TargetUri for {rc.Name}");
                throw new InvalidOperationException($"Invalid TargetUri for {rc.Name}");
            }

            if (!urisHash.Add(rc.SourceUri))
            {
                logger.Error("Duplicate Source Uris configuration found");
                throw new InvalidOperationException("Duplicate Source Uris configuration found");
            }
        }
        #endregion  
    }
}

