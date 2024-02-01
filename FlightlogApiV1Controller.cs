using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using NHibernate;
using primebird.core;

namespace primebird.portal.airtime.controllers.api.v1
{
    //[EnableCors("AllowAllOrigins")]
    [ApiController]
    [EnableCors]
    public class FlightlogApiV1Controller : Controller
    {
        NHibernate.ISession _session;
        public FlightlogApiV1Controller(ILogger<FlightlogApiV1Controller> logger, NHibernate.ISession session)
        {
            _session = session;
        }

        [Route("test")]
        public IActionResult ImportFlightLog(IFormFile[] files)
        {
            using(ITransaction tx = _session.BeginTransaction()) {
                Position p = new Position();
                p.Altitude_gnss = 1;
                p.Altitude_pressure_icao = 2;
                p.Unixtime = 0;
                p.Id = "point123";
                p.Geom = new NetTopologySuite.Geometries.Point(12.1,43.5,1050.0);
                _session.Save(p);
                tx.Commit();
            }
            return Content("OK");
        }
    }
}