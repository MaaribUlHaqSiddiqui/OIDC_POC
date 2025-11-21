using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EndpointGRPC.Exposed
{
    public class ExposedAllEndpoints
    {
        public static List<string> GetAllActions(string ApiBaseName, Assembly assembly, Type iFeature)
        {
            var featureInterfaces = assembly.GetTypes()
                                                            .Where(t => t.IsInterface && iFeature.IsAssignableFrom(t) && t != iFeature)

                                                            .ToList();

            var routes = new List<string>();


            foreach (var features in featureInterfaces)
            {
                var featureName = features.Name;


                var implementingClasses = assembly.GetTypes()
                                                  .Where(t => t.IsClass && !t.IsAbstract
                                                              && features.IsAssignableFrom(t))
                                                  .ToList();
                foreach (var actions in implementingClasses)
                {
                    string actionName = actions.Name;
                    routes.Add($"/{ApiBaseName}/{featureName}/{actionName}");
                }


            }
            return routes;
        }
    }
}
