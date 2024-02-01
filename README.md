create table position (
	id varchar(255),
	unixtime bigint,
	altitude_pressure_icao numeric(7,2),
	altitude_gnss numeric(7,2),
	geom public.geometry(pointz,4326)
);
