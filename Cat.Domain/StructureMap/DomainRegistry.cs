using Cat.Domain.Repositories;
using StructureMap.Configuration.DSL;

namespace Cat.Domain.StructureMap
{
    public class DomainRegistry : Registry
    {
        public DomainRegistry()
        {
            For<ITestEntitiesRespository>().Use<TestRespository>();
        }
    }
}
