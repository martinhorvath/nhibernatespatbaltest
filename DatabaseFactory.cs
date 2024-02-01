
using System.Reflection;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions.Helpers;
using NHibernate;
using NHibernate.Util;
using Npgsql;

namespace primebird.core
{
    public class DatabaseFactory
    {
        public ISessionFactory CreateSessionFactory(IPersistenceConfigurer conf, IList<Assembly> assembliesWithMappings, EmptyInterceptor interceptor)
        {
            string currentSessionContextClass = "";

            var current_session = Fluently.Configure()
                .CurrentSessionContext(currentSessionContextClass)
                .Database(conf)
                .ExposeConfiguration(cfg =>
                {
                    cfg.SetProperty("current_session_context_class", "web");
                    cfg.SetInterceptor(interceptor);
                })
                .Mappings(m =>
                    ConfigureMappings(m, assembliesWithMappings)
                )
                .Diagnostics(diag => {
                    diag.Enable().OutputToConsole();
                });

            if (currentSessionContextClass != null)
                current_session.CurrentSessionContext(currentSessionContextClass);

            ISessionFactory sessionFactory = current_session.BuildSessionFactory();
            return sessionFactory;
        }

        private void ConfigureMappings(MappingConfiguration mappingConfiguration, IList<Assembly> assembliesWithMappings)
        {
            mappingConfiguration.FluentMappings.Conventions.Setup(x => x.Add(AutoImport.Never()));

            foreach(Assembly a in assembliesWithMappings)
            {
                mappingConfiguration.FluentMappings.AddFromAssembly(a);
                /*Addons*/
                mappingConfiguration.HbmMappings.AddFromAssembly(a);   
            }
        }
    }
}