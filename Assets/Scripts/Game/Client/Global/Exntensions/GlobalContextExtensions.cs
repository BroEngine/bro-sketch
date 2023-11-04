using Bro.Client.Configs;
using Bro.Client.Context;

namespace Game.Client
{
    public static class GlobalContextExtensions
    {
        public static IClientContext GetGlobalContext(this IClientContext context)
        {
            return context.Application.GlobalContext;
        }

        public static Game GetApplication(this IClientContext context)
        {
            return (Game) context.Application;
        }

        public static T GetGlobal<T>(this IClientContext context) where T : class, IClientContextElement
        {
            return context.Application.GlobalContext.Get<T>();
        }
        
        public static VehicleConfigStorage GetVehicleConfigStorage(this IClientContext context)
        {
            return context.GetGlobal<ConfigModule>().GetConfigStorage<VehicleConfigStorage>();
        }
        
        public static bool IsDevelopment(this IClientContext context)
        {
            // var isDevelopment = Application.Environment == BuildEnvironment.Development;
            // return isDevelopment;
            return true;
        }
    }
}