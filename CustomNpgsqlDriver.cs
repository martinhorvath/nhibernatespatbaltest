using System.Data;
using System.Data.Common;
using NHibernate.Driver;
using NHibernate.SqlCommand;
using NHibernate.SqlTypes;
using Npgsql;

namespace primebird.core
{
    public class CustomNpgSqlDriver : NpgsqlDriver
    {
        protected override void InitializeParameter(DbParameter dbParam, string name, SqlType sqlType)
        {
            base.InitializeParameter(dbParam, name, sqlType);

            var dbType = sqlType?.DbType;
            var npgsqlParam = dbParam as Npgsql.NpgsqlParameter;
            if (npgsqlParam != null && name.Equals("breakpointme"))
            {
                npgsqlParam.ResetDbType();
                npgsqlParam.NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Geometry;
            }
        }

        public override DbCommand GenerateCommand(CommandType type, SqlString sqlString, SqlType[] parameterTypes)
        {
            DbCommand cmd = base.GenerateCommand(type, sqlString, parameterTypes);
            return cmd;
        }
    } 
}