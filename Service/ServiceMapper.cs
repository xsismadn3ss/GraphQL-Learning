using System.Reflection;

namespace GraphQL_Learning.Service
{
    public class ServiceMapper
    {
        public static Type[] GetServiceTypes()
        {
            var assembly = Assembly.GetExecutingAssembly();

            var types = assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsInterface && !t.IsAbstract && !t.IsSealed && t.Name.EndsWith("Service"))
                .ToArray();
        }
    }
}
