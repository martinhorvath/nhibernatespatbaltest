using primebird.portal.airtime;
using FluentNHibernate.Mapping;
using NHibernate.Spatial.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Util;

namespace primebird.portal.airtime.mappings.nhibernate
{
    public class PositionMap : ClassMap<Position>
    {
        public PositionMap()
        {
            Schema("airtime");
            Table("position");
            Id(x => x.Id).GeneratedBy.Assigned();
            Map(x => x.Unixtime);
            Map(x => x.Altitude_pressure_icao);
            Map(x => x.Altitude_gnss);
            Map(x => x.Geom)
                .CustomType<PostGisGeometryType>();
        }
    }
}
