using System.Reflection;

namespace GraphQL_Learning.Service
{
    /// <summary>
    /// Mapper de tipos, esta clase permite obtener un listado de tipos específico
    /// usando reflexión.
    /// 
    /// Esto es útil para evitar registrar servicios manualmente
    /// </summary>
    public class TypesMapper
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
