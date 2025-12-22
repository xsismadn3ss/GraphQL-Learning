namespace GraphQL_Learning.Services
{
    /// <summary>
    /// Mapper de tipos, esta clase permite obtener un listado de tipos específico
    /// usando reflexión.
    /// 
    /// Esto es útil para evitar registrar servicios o tipos manualmente
    /// </summary>
    public class TypesMapper
    {
        public static Type[] GetServiceTypes()
            => [.. typeof(Program).Assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsInterface && !t.IsAbstract && t.Name.EndsWith("Service"))];

        public static Type[] GetMutationsTypes()
            => [.. typeof(Program).Assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsInterface && !t.IsAbstract && t.Name.EndsWith("Mutation"))];

        public static Type[] GetQueriesTypes()
            => [.. typeof(Program).Assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsInterface && !t.IsAbstract && t.Name.EndsWith("Query"))];

        public static Type[] GetSubscriptionsTypes()
            => [.. typeof(Program).Assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsInterface && !t.IsAbstract && t.Name.EndsWith("Subscription"))];
    }
}
