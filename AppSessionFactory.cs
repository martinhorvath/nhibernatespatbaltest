using NHibernate;
using System.Reflection;
using primebird.core;
using NHibernate.SqlCommand;
using System.Diagnostics;

namespace com.primebird.portal.core.infrastructure
{
    public class AppSessionFactory
    {
        private DatabaseFactory databaseFactory = new DatabaseFactory();
        private RDBMS? rdbms;
        public ISessionFactory? SessionFactory { get; private set; }
        //public IGraphSessionFactory GraphSessionFactory { get; }

        private Dictionary<string,string> _scannedNamespaces = new Dictionary<string, string>();

        public AppSessionFactory()
        {
        }

        public void SetupRDBMS(RDBMS rdbms, IList<Assembly> assemblies)
        {
            this.rdbms = rdbms;
            SessionFactory = databaseFactory.CreateSessionFactory(this.rdbms.configure(), assemblies, new LoggingInterceptor());
        }

        public NHibernate.ISession OpenSession(bool debug = false)
        {
            if(SessionFactory == null)
                throw new Exception("SessionFactory not initialized");
            
            if(!debug)
                return SessionFactory.OpenSession();
            
            return SessionFactory.WithOptions().Interceptor(new LoggingInterceptor()).OpenSession();
        }

        public IStatelessSession OpenStatelessSession()
        {
            if(SessionFactory == null)
                throw new Exception("SessionFactory not initialized");

            return SessionFactory.OpenStatelessSession();
        }

        public class LoggingInterceptor : EmptyInterceptor
        {
            public LoggingInterceptor() : base()
            {
                Debug.WriteLine("NHibernate debug interceptor active");
            }

            public override SqlString OnPrepareStatement(SqlString sql)
            {
                Debug.WriteLine(sql);
                return sql;
            }
        }
    }
}
