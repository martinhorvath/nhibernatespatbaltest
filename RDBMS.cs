using FluentNHibernate.Cfg.Db;
using NHibernate.Dialect;
using NHibernate.Spatial.Dialect;
using Npgsql;
using primebird.core;

public class RDBMS
{
    public static string POSTGRESQL = "postgresql";
    public static string SQLITE = "sqlite";

    public string connectionstring = "";
    public string dbType = "";
    
    public RDBMS()
    {

    }
 
    public IPersistenceConfigurer configure()
    {
        if (connectionstring is null)
            throw new Exception("Connectionstring is not set. Database connection configuration was not possible.");

        if(dbType == POSTGRESQL)
            return ConfigurePostgreSQL();
        else if (dbType == SQLITE)
            return ConfigureSQLite();
        else
            throw new NotSupportedException("Database type related to configuration " + dbType + " is not yet supported.");
    }

    private IPersistenceConfigurer ConfigurePostgreSQL() {
        var db = PostgreSQLConfiguration.PostgreSQL82;
        //NpgsqlConnection.GlobalTypeMapper.UseNetTopologySuite();

        return db
            .ConnectionString(connectionstring)
            .AdoNetBatchSize(20)
            .Driver<CustomNpgSqlDriver>()
            .Dialect<NHibernate.Spatial.Dialect.PostGis20Dialect>();
    }

    private IPersistenceConfigurer ConfigureSQLite() {
        var db = SQLiteConfiguration.Standard;
        return db
            .ConnectionString(connectionstring);
    }
}