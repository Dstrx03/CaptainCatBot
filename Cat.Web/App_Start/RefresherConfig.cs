
using Cat.Business.Services.InternalServices;

namespace Cat.Web.App_Start
{
    public class RefresherConfig
    {
        public static void Register()
        {
            RefresherService.RunRefresher(StructuremapMvc.StructureMapDependencyScope.Container.GetNestedContainer());
        }
    }
}