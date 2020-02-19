--
-- NOTE:
--
-- File paths need to be edited. Search for $$PATH$$ and
-- replace it with the path to the directory containing
-- the extracted data files.
--
--
-- PostgreSQL database dump
--

-- Dumped from database version 9.6.12
-- Dumped by pg_dump version 9.6.12

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET client_min_messages = warning;
SET row_security = off;

ALTER TABLE ONLY public.titleusers DROP CONSTRAINT title_users_target;
ALTER TABLE ONLY public.titleusers DROP CONSTRAINT title_users_source;
ALTER TABLE ONLY public.roleusers DROP CONSTRAINT role_users_target;
ALTER TABLE ONLY public.roleusers DROP CONSTRAINT role_users_source;
ALTER TABLE ONLY public.rolepowers DROP CONSTRAINT role_powers_target;
ALTER TABLE ONLY public.rolepowers DROP CONSTRAINT role_powers_source;
ALTER TABLE ONLY public.onlines DROP CONSTRAINT online_user;
ALTER TABLE ONLY public.menus DROP CONSTRAINT menu_viewpower;
ALTER TABLE ONLY public.menus DROP CONSTRAINT menu_parent;
ALTER TABLE ONLY public.points DROP CONSTRAINT f4;
ALTER TABLE ONLY public.unitrangers DROP CONSTRAINT f3;
ALTER TABLE ONLY public.unitrangers DROP CONSTRAINT f2;
ALTER TABLE ONLY public.photos DROP CONSTRAINT f2;
ALTER TABLE ONLY public.tracks DROP CONSTRAINT f1;
ALTER TABLE ONLY public.users DROP CONSTRAINT dept_users;
ALTER TABLE ONLY public.depts DROP CONSTRAINT dept_parent;
ALTER TABLE ONLY public.users DROP CONSTRAINT users_pkey;
ALTER TABLE ONLY public.unitrangers DROP CONSTRAINT unitrangers_pkey;
ALTER TABLE ONLY public.rangers DROP CONSTRAINT u3;
ALTER TABLE ONLY public.tracks DROP CONSTRAINT tracks_pkey;
ALTER TABLE ONLY public.titleusers DROP CONSTRAINT titleusers_pkey;
ALTER TABLE ONLY public.titles DROP CONSTRAINT titles_pkey;
ALTER TABLE ONLY public.roleusers DROP CONSTRAINT roleusers_pkey;
ALTER TABLE ONLY public.roles DROP CONSTRAINT roles_pkey;
ALTER TABLE ONLY public.rolepowers DROP CONSTRAINT rolepowers_pkey;
ALTER TABLE ONLY public.powers DROP CONSTRAINT powers_pkey;
ALTER TABLE ONLY public.points DROP CONSTRAINT points_pkey;
ALTER TABLE ONLY public.photos DROP CONSTRAINT points_copy1_pkey;
ALTER TABLE ONLY public.rangers DROP CONSTRAINT persons_pkey;
ALTER TABLE ONLY public.onlines DROP CONSTRAINT onlines_pkey;
ALTER TABLE ONLY public.messages DROP CONSTRAINT messages_pkey;
ALTER TABLE ONLY public.menus DROP CONSTRAINT menus_pkey;
ALTER TABLE ONLY public.logs DROP CONSTRAINT logs_pkey;
ALTER TABLE ONLY public.units DROP CONSTRAINT forests_pkey;
ALTER TABLE ONLY public.depts DROP CONSTRAINT depts_pkey;
ALTER TABLE ONLY public.configs DROP CONSTRAINT configs_pkey;
ALTER TABLE public.units ALTER COLUMN id DROP DEFAULT;
ALTER TABLE public.unitrangers ALTER COLUMN id DROP DEFAULT;
ALTER TABLE public.tracks ALTER COLUMN id DROP DEFAULT;
ALTER TABLE public.rangers ALTER COLUMN id DROP DEFAULT;
ALTER TABLE public.points ALTER COLUMN id DROP DEFAULT;
ALTER TABLE public.messages ALTER COLUMN id DROP DEFAULT;
DROP TABLE public.users;
DROP SEQUENCE public.users_id_seq;
DROP SEQUENCE public.unitrangers_id_seq;
DROP TABLE public.unitrangers;
DROP SEQUENCE public.tracks_id_seq;
DROP TABLE public.titleusers;
DROP TABLE public.titles;
DROP SEQUENCE public.titles_id_seq;
DROP SEQUENCE public.seqname;
DROP TABLE public.roleusers;
DROP TABLE public.roles;
DROP SEQUENCE public.roles_id_seq;
DROP TABLE public.rolepowers;
DROP TABLE public.powers;
DROP SEQUENCE public.powers_id_seq;
DROP SEQUENCE public.points_id_seq;
DROP TABLE public.points;
DROP SEQUENCE public.persons_id_seq;
DROP VIEW public.patrolinfos;
DROP TABLE public.photos;
DROP TABLE public.onlines;
DROP SEQUENCE public.onlines_id_seq;
DROP SEQUENCE public.messages_id_seq;
DROP TABLE public.messages;
DROP TABLE public.menus;
DROP SEQUENCE public.menus_id_seq;
DROP TABLE public.logs;
DROP SEQUENCE public.logs_id_seq;
DROP SEQUENCE public.forests_id_seq;
DROP TABLE public.units;
DROP TABLE public.depts;
DROP SEQUENCE public.depts_id_seq;
DROP TABLE public.configs;
DROP SEQUENCE public.configs_id_seq;
DROP VIEW public.auditinfos;
DROP TABLE public.tracks;
DROP TABLE public.rangers;
DROP EXTENSION postgis_topology;
DROP EXTENSION postgis_tiger_geocoder;
DROP EXTENSION postgis_sfcgal;
DROP EXTENSION pointcloud_postgis;
DROP EXTENSION pointcloud;
DROP EXTENSION pgrouting;
DROP EXTENSION postgis;
DROP EXTENSION ogr_fdw;
DROP EXTENSION fuzzystrmatch;
DROP EXTENSION adminpack;
DROP EXTENSION address_standardizer;
DROP EXTENSION plpgsql;
DROP SCHEMA topology;
DROP SCHEMA tiger_data;
DROP SCHEMA tiger;
DROP SCHEMA public;
--
-- Name: public; Type: SCHEMA; Schema: -; Owner: postgres
--

CREATE SCHEMA public;


ALTER SCHEMA public OWNER TO postgres;

--
-- Name: SCHEMA public; Type: COMMENT; Schema: -; Owner: postgres
--

COMMENT ON SCHEMA public IS 'standard public schema';


--
-- Name: tiger; Type: SCHEMA; Schema: -; Owner: postgres
--

CREATE SCHEMA tiger;


ALTER SCHEMA tiger OWNER TO postgres;

--
-- Name: tiger_data; Type: SCHEMA; Schema: -; Owner: postgres
--

CREATE SCHEMA tiger_data;


ALTER SCHEMA tiger_data OWNER TO postgres;

--
-- Name: topology; Type: SCHEMA; Schema: -; Owner: postgres
--

CREATE SCHEMA topology;


ALTER SCHEMA topology OWNER TO postgres;

--
-- Name: SCHEMA topology; Type: COMMENT; Schema: -; Owner: postgres
--

COMMENT ON SCHEMA topology IS 'PostGIS Topology schema';


--
-- Name: plpgsql; Type: EXTENSION; Schema: -; Owner: 
--

CREATE EXTENSION IF NOT EXISTS plpgsql WITH SCHEMA pg_catalog;


--
-- Name: EXTENSION plpgsql; Type: COMMENT; Schema: -; Owner: 
--

COMMENT ON EXTENSION plpgsql IS 'PL/pgSQL procedural language';


--
-- Name: address_standardizer; Type: EXTENSION; Schema: -; Owner: 
--

CREATE EXTENSION IF NOT EXISTS address_standardizer WITH SCHEMA public;


--
-- Name: EXTENSION address_standardizer; Type: COMMENT; Schema: -; Owner: 
--

COMMENT ON EXTENSION address_standardizer IS 'Used to parse an address into constituent elements. Generally used to support geocoding address normalization step.';


--
-- Name: adminpack; Type: EXTENSION; Schema: -; Owner: 
--

CREATE EXTENSION IF NOT EXISTS adminpack WITH SCHEMA pg_catalog;


--
-- Name: EXTENSION adminpack; Type: COMMENT; Schema: -; Owner: 
--

COMMENT ON EXTENSION adminpack IS 'administrative functions for PostgreSQL';


--
-- Name: fuzzystrmatch; Type: EXTENSION; Schema: -; Owner: 
--

CREATE EXTENSION IF NOT EXISTS fuzzystrmatch WITH SCHEMA public;


--
-- Name: EXTENSION fuzzystrmatch; Type: COMMENT; Schema: -; Owner: 
--

COMMENT ON EXTENSION fuzzystrmatch IS 'determine similarities and distance between strings';


--
-- Name: ogr_fdw; Type: EXTENSION; Schema: -; Owner: 
--

CREATE EXTENSION IF NOT EXISTS ogr_fdw WITH SCHEMA public;


--
-- Name: EXTENSION ogr_fdw; Type: COMMENT; Schema: -; Owner: 
--

COMMENT ON EXTENSION ogr_fdw IS 'foreign-data wrapper for GIS data access';


--
-- Name: postgis; Type: EXTENSION; Schema: -; Owner: 
--

CREATE EXTENSION IF NOT EXISTS postgis WITH SCHEMA public;


--
-- Name: EXTENSION postgis; Type: COMMENT; Schema: -; Owner: 
--

COMMENT ON EXTENSION postgis IS 'PostGIS geometry, geography, and raster spatial types and functions';


--
-- Name: pgrouting; Type: EXTENSION; Schema: -; Owner: 
--

CREATE EXTENSION IF NOT EXISTS pgrouting WITH SCHEMA public;


--
-- Name: EXTENSION pgrouting; Type: COMMENT; Schema: -; Owner: 
--

COMMENT ON EXTENSION pgrouting IS 'pgRouting Extension';


--
-- Name: pointcloud; Type: EXTENSION; Schema: -; Owner: 
--

CREATE EXTENSION IF NOT EXISTS pointcloud WITH SCHEMA public;


--
-- Name: EXTENSION pointcloud; Type: COMMENT; Schema: -; Owner: 
--

COMMENT ON EXTENSION pointcloud IS 'data type for lidar point clouds';


--
-- Name: pointcloud_postgis; Type: EXTENSION; Schema: -; Owner: 
--

CREATE EXTENSION IF NOT EXISTS pointcloud_postgis WITH SCHEMA public;


--
-- Name: EXTENSION pointcloud_postgis; Type: COMMENT; Schema: -; Owner: 
--

COMMENT ON EXTENSION pointcloud_postgis IS 'integration for pointcloud LIDAR data and PostGIS geometry data';


--
-- Name: postgis_sfcgal; Type: EXTENSION; Schema: -; Owner: 
--

CREATE EXTENSION IF NOT EXISTS postgis_sfcgal WITH SCHEMA public;


--
-- Name: EXTENSION postgis_sfcgal; Type: COMMENT; Schema: -; Owner: 
--

COMMENT ON EXTENSION postgis_sfcgal IS 'PostGIS SFCGAL functions';


--
-- Name: postgis_tiger_geocoder; Type: EXTENSION; Schema: -; Owner: 
--

CREATE EXTENSION IF NOT EXISTS postgis_tiger_geocoder WITH SCHEMA tiger;


--
-- Name: EXTENSION postgis_tiger_geocoder; Type: COMMENT; Schema: -; Owner: 
--

COMMENT ON EXTENSION postgis_tiger_geocoder IS 'PostGIS tiger geocoder and reverse geocoder';


--
-- Name: postgis_topology; Type: EXTENSION; Schema: -; Owner: 
--

CREATE EXTENSION IF NOT EXISTS postgis_topology WITH SCHEMA topology;


--
-- Name: EXTENSION postgis_topology; Type: COMMENT; Schema: -; Owner: 
--

COMMENT ON EXTENSION postgis_topology IS 'PostGIS topology spatial types and functions';


SET default_tablespace = '';

SET default_with_oids = false;

--
-- Name: rangers; Type: TABLE; Schema: public; Owner: user
--

CREATE TABLE public.rangers (
    id integer NOT NULL,
    name character varying(20),
    password character varying(20),
    "character" character varying(20),
    tel character varying(30) NOT NULL,
    town character varying(50),
    village character varying(50)
);


ALTER TABLE public.rangers OWNER TO "user";

--
-- Name: COLUMN rangers."character"; Type: COMMENT; Schema: public; Owner: user
--

COMMENT ON COLUMN public.rangers."character" IS 'temp';


--
-- Name: tracks; Type: TABLE; Schema: public; Owner: user
--

CREATE TABLE public.tracks (
    id integer NOT NULL,
    rangerid integer NOT NULL,
    starttime timestamp without time zone NOT NULL,
    endtime timestamp without time zone NOT NULL,
    uploadtime timestamp without time zone NOT NULL,
    degree real,
    length smallint
);


ALTER TABLE public.tracks OWNER TO "user";

--
-- Name: auditinfos; Type: VIEW; Schema: public; Owner: user
--

CREATE VIEW public.auditinfos AS
 SELECT tracks.id,
    rangers.town,
    rangers.village,
    rangers.name,
    rangers.tel,
    tracks.degree,
    tracks.length,
    tracks.uploadtime
   FROM (public.rangers
     JOIN public.tracks ON ((tracks.rangerid = rangers.id)));


ALTER TABLE public.auditinfos OWNER TO "user";

--
-- Name: configs_id_seq; Type: SEQUENCE; Schema: public; Owner: user
--

CREATE SEQUENCE public.configs_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.configs_id_seq OWNER TO "user";

--
-- Name: configs; Type: TABLE; Schema: public; Owner: user
--

CREATE TABLE public.configs (
    id integer DEFAULT nextval('public.configs_id_seq'::regclass) NOT NULL,
    configkey character varying(150) NOT NULL,
    configvalue character varying(12000) NOT NULL,
    remark character varying(1500)
);


ALTER TABLE public.configs OWNER TO "user";

--
-- Name: depts_id_seq; Type: SEQUENCE; Schema: public; Owner: user
--

CREATE SEQUENCE public.depts_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.depts_id_seq OWNER TO "user";

--
-- Name: depts; Type: TABLE; Schema: public; Owner: user
--

CREATE TABLE public.depts (
    id integer DEFAULT nextval('public.depts_id_seq'::regclass) NOT NULL,
    name character varying(150) NOT NULL,
    sortindex integer DEFAULT 0 NOT NULL,
    remark character varying(1500),
    parentid integer DEFAULT 0
);


ALTER TABLE public.depts OWNER TO "user";

--
-- Name: units; Type: TABLE; Schema: public; Owner: user
--

CREATE TABLE public.units (
    id integer NOT NULL,
    geom public.geometry(MultiPolygonZ,4549),
    lin_ban character varying(4),
    xiao_ban character varying(5),
    xiao_ban_w character varying(5),
    mian_ji double precision,
    di_ming character varying(16),
    shi_quan_d character varying(2),
    g_cheng_lb character varying(2),
    gyl_mj double precision,
    lin_zhong character varying(3),
    smd double precision,
    xianname character varying(50),
    xiangname character varying(50),
    cunname character varying(50)
);


ALTER TABLE public.units OWNER TO "user";

--
-- Name: forests_id_seq; Type: SEQUENCE; Schema: public; Owner: user
--

CREATE SEQUENCE public.forests_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.forests_id_seq OWNER TO "user";

--
-- Name: forests_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: user
--

ALTER SEQUENCE public.forests_id_seq OWNED BY public.units.id;


--
-- Name: logs_id_seq; Type: SEQUENCE; Schema: public; Owner: user
--

CREATE SEQUENCE public.logs_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.logs_id_seq OWNER TO "user";

--
-- Name: logs; Type: TABLE; Schema: public; Owner: user
--

CREATE TABLE public.logs (
    id integer DEFAULT nextval('public.logs_id_seq'::regclass) NOT NULL,
    level character varying(60),
    logger character varying(600),
    message character varying(12000),
    exception character varying(12000),
    logtime timestamp(6) without time zone NOT NULL
);


ALTER TABLE public.logs OWNER TO "user";

--
-- Name: menus_id_seq; Type: SEQUENCE; Schema: public; Owner: user
--

CREATE SEQUENCE public.menus_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.menus_id_seq OWNER TO "user";

--
-- Name: menus; Type: TABLE; Schema: public; Owner: user
--

CREATE TABLE public.menus (
    id integer DEFAULT nextval('public.menus_id_seq'::regclass) NOT NULL,
    name character varying(150) NOT NULL,
    imageurl character varying(600),
    navigateurl character varying(600),
    remark character varying(1500),
    sortindex integer DEFAULT 0 NOT NULL,
    parentid integer DEFAULT 0,
    viewpowerid integer DEFAULT 0
);


ALTER TABLE public.menus OWNER TO "user";

--
-- Name: messages; Type: TABLE; Schema: public; Owner: user
--

CREATE TABLE public.messages (
    id integer NOT NULL,
    recipients character varying(255),
    content character varying(255) NOT NULL,
    sender character varying(255) NOT NULL,
    "time" timestamp(6) without time zone NOT NULL
);


ALTER TABLE public.messages OWNER TO "user";

--
-- Name: messages_id_seq; Type: SEQUENCE; Schema: public; Owner: user
--

CREATE SEQUENCE public.messages_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.messages_id_seq OWNER TO "user";

--
-- Name: messages_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: user
--

ALTER SEQUENCE public.messages_id_seq OWNED BY public.messages.id;


--
-- Name: onlines_id_seq; Type: SEQUENCE; Schema: public; Owner: user
--

CREATE SEQUENCE public.onlines_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.onlines_id_seq OWNER TO "user";

--
-- Name: onlines; Type: TABLE; Schema: public; Owner: user
--

CREATE TABLE public.onlines (
    id integer DEFAULT nextval('public.onlines_id_seq'::regclass) NOT NULL,
    ipadddress character varying(150),
    logintime timestamp(6) without time zone NOT NULL,
    updatetime timestamp(6) without time zone,
    userid integer DEFAULT 0 NOT NULL
);


ALTER TABLE public.onlines OWNER TO "user";

--
-- Name: photos; Type: TABLE; Schema: public; Owner: user
--

CREATE TABLE public.photos (
    id integer NOT NULL,
    rangerid integer NOT NULL,
    point public.geometry(Point,4549) DEFAULT NULL::public.geometry,
    "time" timestamp(6) without time zone NOT NULL,
    path character varying(100)
);


ALTER TABLE public.photos OWNER TO "user";

--
-- Name: patrolinfos; Type: VIEW; Schema: public; Owner: user
--

CREATE VIEW public.patrolinfos AS
 SELECT photos.id,
    rangers.town,
    rangers.village,
    rangers.name,
    rangers.tel,
    photos.path,
    photos."time"
   FROM (public.rangers
     JOIN public.photos ON ((photos.rangerid = rangers.id)));


ALTER TABLE public.patrolinfos OWNER TO "user";

--
-- Name: persons_id_seq; Type: SEQUENCE; Schema: public; Owner: user
--

CREATE SEQUENCE public.persons_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.persons_id_seq OWNER TO "user";

--
-- Name: persons_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: user
--

ALTER SEQUENCE public.persons_id_seq OWNED BY public.rangers.id;


--
-- Name: points; Type: TABLE; Schema: public; Owner: user
--

CREATE TABLE public.points (
    id integer NOT NULL,
    rangerid integer,
    "time" timestamp without time zone,
    point public.geometry(Point,4326)
);


ALTER TABLE public.points OWNER TO "user";

--
-- Name: points_id_seq; Type: SEQUENCE; Schema: public; Owner: user
--

CREATE SEQUENCE public.points_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.points_id_seq OWNER TO "user";

--
-- Name: points_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: user
--

ALTER SEQUENCE public.points_id_seq OWNED BY public.points.id;


--
-- Name: powers_id_seq; Type: SEQUENCE; Schema: public; Owner: user
--

CREATE SEQUENCE public.powers_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.powers_id_seq OWNER TO "user";

--
-- Name: powers; Type: TABLE; Schema: public; Owner: user
--

CREATE TABLE public.powers (
    id integer DEFAULT nextval('public.powers_id_seq'::regclass) NOT NULL,
    name character varying(150) NOT NULL,
    groupname character varying(150) NOT NULL,
    title character varying(600) NOT NULL,
    remark character varying(1500)
);


ALTER TABLE public.powers OWNER TO "user";

--
-- Name: rolepowers; Type: TABLE; Schema: public; Owner: user
--

CREATE TABLE public.rolepowers (
    roleid integer DEFAULT 0 NOT NULL,
    powerid integer DEFAULT 0 NOT NULL
);


ALTER TABLE public.rolepowers OWNER TO "user";

--
-- Name: roles_id_seq; Type: SEQUENCE; Schema: public; Owner: user
--

CREATE SEQUENCE public.roles_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.roles_id_seq OWNER TO "user";

--
-- Name: roles; Type: TABLE; Schema: public; Owner: user
--

CREATE TABLE public.roles (
    id integer DEFAULT nextval('public.roles_id_seq'::regclass) NOT NULL,
    name character varying(150) NOT NULL,
    remark character varying(1500)
);


ALTER TABLE public.roles OWNER TO "user";

--
-- Name: roleusers; Type: TABLE; Schema: public; Owner: user
--

CREATE TABLE public.roleusers (
    roleid integer DEFAULT 0 NOT NULL,
    userid integer DEFAULT 0 NOT NULL
);


ALTER TABLE public.roleusers OWNER TO "user";

--
-- Name: seqname; Type: SEQUENCE; Schema: public; Owner: user
--

CREATE SEQUENCE public.seqname
    START WITH 999999999999999
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.seqname OWNER TO "user";

--
-- Name: titles_id_seq; Type: SEQUENCE; Schema: public; Owner: user
--

CREATE SEQUENCE public.titles_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.titles_id_seq OWNER TO "user";

--
-- Name: titles; Type: TABLE; Schema: public; Owner: user
--

CREATE TABLE public.titles (
    id integer DEFAULT nextval('public.titles_id_seq'::regclass) NOT NULL,
    name character varying(150) NOT NULL,
    remark character varying(1500)
);


ALTER TABLE public.titles OWNER TO "user";

--
-- Name: titleusers; Type: TABLE; Schema: public; Owner: user
--

CREATE TABLE public.titleusers (
    titleid integer DEFAULT 0 NOT NULL,
    userid integer DEFAULT 0 NOT NULL
);


ALTER TABLE public.titleusers OWNER TO "user";

--
-- Name: tracks_id_seq; Type: SEQUENCE; Schema: public; Owner: user
--

CREATE SEQUENCE public.tracks_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.tracks_id_seq OWNER TO "user";

--
-- Name: tracks_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: user
--

ALTER SEQUENCE public.tracks_id_seq OWNED BY public.tracks.id;


--
-- Name: unitrangers; Type: TABLE; Schema: public; Owner: user
--

CREATE TABLE public.unitrangers (
    id integer NOT NULL,
    unitid integer NOT NULL,
    ckpoint public.geometry(Point,4549),
    rangerid integer
);


ALTER TABLE public.unitrangers OWNER TO "user";

--
-- Name: unitrangers_id_seq; Type: SEQUENCE; Schema: public; Owner: user
--

CREATE SEQUENCE public.unitrangers_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.unitrangers_id_seq OWNER TO "user";

--
-- Name: unitrangers_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: user
--

ALTER SEQUENCE public.unitrangers_id_seq OWNED BY public.unitrangers.id;


--
-- Name: users_id_seq; Type: SEQUENCE; Schema: public; Owner: user
--

CREATE SEQUENCE public.users_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.users_id_seq OWNER TO "user";

--
-- Name: users; Type: TABLE; Schema: public; Owner: user
--

CREATE TABLE public.users (
    id integer DEFAULT nextval('public.users_id_seq'::regclass) NOT NULL,
    name character varying(150) NOT NULL,
    email character varying(300) NOT NULL,
    password character varying(150) NOT NULL,
    enabled boolean DEFAULT false NOT NULL,
    gender character varying(30) NOT NULL,
    chinesename character varying(300),
    photo character varying(600),
    officephone character varying(150),
    cellphone character varying(150),
    address character varying(1500),
    remark character varying(1500),
    lastlogintime timestamp(6) without time zone,
    createtime timestamp(6) without time zone,
    deptid integer DEFAULT 0
);


ALTER TABLE public.users OWNER TO "user";

--
-- Name: messages id; Type: DEFAULT; Schema: public; Owner: user
--

ALTER TABLE ONLY public.messages ALTER COLUMN id SET DEFAULT nextval('public.messages_id_seq'::regclass);


--
-- Name: points id; Type: DEFAULT; Schema: public; Owner: user
--

ALTER TABLE ONLY public.points ALTER COLUMN id SET DEFAULT nextval('public.points_id_seq'::regclass);


--
-- Name: rangers id; Type: DEFAULT; Schema: public; Owner: user
--

ALTER TABLE ONLY public.rangers ALTER COLUMN id SET DEFAULT nextval('public.persons_id_seq'::regclass);


--
-- Name: tracks id; Type: DEFAULT; Schema: public; Owner: user
--

ALTER TABLE ONLY public.tracks ALTER COLUMN id SET DEFAULT nextval('public.tracks_id_seq'::regclass);


--
-- Name: unitrangers id; Type: DEFAULT; Schema: public; Owner: user
--

ALTER TABLE ONLY public.unitrangers ALTER COLUMN id SET DEFAULT nextval('public.unitrangers_id_seq'::regclass);


--
-- Name: units id; Type: DEFAULT; Schema: public; Owner: user
--

ALTER TABLE ONLY public.units ALTER COLUMN id SET DEFAULT nextval('public.forests_id_seq'::regclass);


--
-- Data for Name: configs; Type: TABLE DATA; Schema: public; Owner: user
--

COPY public.configs (id, configkey, configvalue, remark) FROM stdin;
\.
COPY public.configs (id, configkey, configvalue, remark) FROM '$$PATH$$/4562.dat';

--
-- Name: configs_id_seq; Type: SEQUENCE SET; Schema: public; Owner: user
--

SELECT pg_catalog.setval('public.configs_id_seq', 6, false);


--
-- Data for Name: depts; Type: TABLE DATA; Schema: public; Owner: user
--

COPY public.depts (id, name, sortindex, remark, parentid) FROM stdin;
\.
COPY public.depts (id, name, sortindex, remark, parentid) FROM '$$PATH$$/4563.dat';

--
-- Name: depts_id_seq; Type: SEQUENCE SET; Schema: public; Owner: user
--

SELECT pg_catalog.setval('public.depts_id_seq', 20, false);


--
-- Name: forests_id_seq; Type: SEQUENCE SET; Schema: public; Owner: user
--

SELECT pg_catalog.setval('public.forests_id_seq', 21079, true);


--
-- Data for Name: logs; Type: TABLE DATA; Schema: public; Owner: user
--

COPY public.logs (id, level, logger, message, exception, logtime) FROM stdin;
\.
COPY public.logs (id, level, logger, message, exception, logtime) FROM '$$PATH$$/4564.dat';

--
-- Name: logs_id_seq; Type: SEQUENCE SET; Schema: public; Owner: user
--

SELECT pg_catalog.setval('public.logs_id_seq', 2, false);


--
-- Data for Name: menus; Type: TABLE DATA; Schema: public; Owner: user
--

COPY public.menus (id, name, imageurl, navigateurl, remark, sortindex, parentid, viewpowerid) FROM stdin;
\.
COPY public.menus (id, name, imageurl, navigateurl, remark, sortindex, parentid, viewpowerid) FROM '$$PATH$$/4565.dat';

--
-- Name: menus_id_seq; Type: SEQUENCE SET; Schema: public; Owner: user
--

SELECT pg_catalog.setval('public.menus_id_seq', 29, true);


--
-- Data for Name: messages; Type: TABLE DATA; Schema: public; Owner: user
--

COPY public.messages (id, recipients, content, sender, "time") FROM stdin;
\.
COPY public.messages (id, recipients, content, sender, "time") FROM '$$PATH$$/4550.dat';

--
-- Name: messages_id_seq; Type: SEQUENCE SET; Schema: public; Owner: user
--

SELECT pg_catalog.setval('public.messages_id_seq', 10, true);


--
-- Data for Name: onlines; Type: TABLE DATA; Schema: public; Owner: user
--

COPY public.onlines (id, ipadddress, logintime, updatetime, userid) FROM stdin;
\.
COPY public.onlines (id, ipadddress, logintime, updatetime, userid) FROM '$$PATH$$/4566.dat';

--
-- Name: onlines_id_seq; Type: SEQUENCE SET; Schema: public; Owner: user
--

SELECT pg_catalog.setval('public.onlines_id_seq', 3, false);


--
-- Name: persons_id_seq; Type: SEQUENCE SET; Schema: public; Owner: user
--

SELECT pg_catalog.setval('public.persons_id_seq', 405, true);


--
-- Data for Name: photos; Type: TABLE DATA; Schema: public; Owner: user
--

COPY public.photos (id, rangerid, point, "time", path) FROM stdin;
\.
COPY public.photos (id, rangerid, point, "time", path) FROM '$$PATH$$/4544.dat';

--
-- Data for Name: pointcloud_formats; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.pointcloud_formats (pcid, srid, schema) FROM stdin;
\.
COPY public.pointcloud_formats (pcid, srid, schema) FROM '$$PATH$$/4253.dat';

--
-- Data for Name: points; Type: TABLE DATA; Schema: public; Owner: user
--

COPY public.points (id, rangerid, "time", point) FROM stdin;
\.
COPY public.points (id, rangerid, "time", point) FROM '$$PATH$$/4575.dat';

--
-- Name: points_id_seq; Type: SEQUENCE SET; Schema: public; Owner: user
--

SELECT pg_catalog.setval('public.points_id_seq', 297, true);


--
-- Data for Name: powers; Type: TABLE DATA; Schema: public; Owner: user
--

COPY public.powers (id, name, groupname, title, remark) FROM stdin;
\.
COPY public.powers (id, name, groupname, title, remark) FROM '$$PATH$$/4567.dat';

--
-- Name: powers_id_seq; Type: SEQUENCE SET; Schema: public; Owner: user
--

SELECT pg_catalog.setval('public.powers_id_seq', 56, true);


--
-- Data for Name: rangers; Type: TABLE DATA; Schema: public; Owner: user
--

COPY public.rangers (id, name, password, "character", tel, town, village) FROM stdin;
\.
COPY public.rangers (id, name, password, "character", tel, town, village) FROM '$$PATH$$/4543.dat';

--
-- Data for Name: rolepowers; Type: TABLE DATA; Schema: public; Owner: user
--

COPY public.rolepowers (roleid, powerid) FROM stdin;
\.
COPY public.rolepowers (roleid, powerid) FROM '$$PATH$$/4568.dat';

--
-- Data for Name: roles; Type: TABLE DATA; Schema: public; Owner: user
--

COPY public.roles (id, name, remark) FROM stdin;
\.
COPY public.roles (id, name, remark) FROM '$$PATH$$/4569.dat';

--
-- Name: roles_id_seq; Type: SEQUENCE SET; Schema: public; Owner: user
--

SELECT pg_catalog.setval('public.roles_id_seq', 9, true);


--
-- Data for Name: roleusers; Type: TABLE DATA; Schema: public; Owner: user
--

COPY public.roleusers (roleid, userid) FROM stdin;
\.
COPY public.roleusers (roleid, userid) FROM '$$PATH$$/4570.dat';

--
-- Name: seqname; Type: SEQUENCE SET; Schema: public; Owner: user
--

SELECT pg_catalog.setval('public.seqname', 999999999999999, false);


--
-- Data for Name: spatial_ref_sys; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.spatial_ref_sys (srid, auth_name, auth_srid, srtext, proj4text) FROM stdin;
\.
COPY public.spatial_ref_sys (srid, auth_name, auth_srid, srtext, proj4text) FROM '$$PATH$$/4254.dat';

--
-- Data for Name: titles; Type: TABLE DATA; Schema: public; Owner: user
--

COPY public.titles (id, name, remark) FROM stdin;
\.
COPY public.titles (id, name, remark) FROM '$$PATH$$/4571.dat';

--
-- Name: titles_id_seq; Type: SEQUENCE SET; Schema: public; Owner: user
--

SELECT pg_catalog.setval('public.titles_id_seq', 6, true);


--
-- Data for Name: titleusers; Type: TABLE DATA; Schema: public; Owner: user
--

COPY public.titleusers (titleid, userid) FROM stdin;
\.
COPY public.titleusers (titleid, userid) FROM '$$PATH$$/4572.dat';

--
-- Data for Name: tracks; Type: TABLE DATA; Schema: public; Owner: user
--

COPY public.tracks (id, rangerid, starttime, endtime, uploadtime, degree, length) FROM stdin;
\.
COPY public.tracks (id, rangerid, starttime, endtime, uploadtime, degree, length) FROM '$$PATH$$/4548.dat';

--
-- Name: tracks_id_seq; Type: SEQUENCE SET; Schema: public; Owner: user
--

SELECT pg_catalog.setval('public.tracks_id_seq', 5, true);


--
-- Data for Name: unitrangers; Type: TABLE DATA; Schema: public; Owner: user
--

COPY public.unitrangers (id, unitid, ckpoint, rangerid) FROM stdin;
\.
COPY public.unitrangers (id, unitid, ckpoint, rangerid) FROM '$$PATH$$/4552.dat';

--
-- Name: unitrangers_id_seq; Type: SEQUENCE SET; Schema: public; Owner: user
--

SELECT pg_catalog.setval('public.unitrangers_id_seq', 56, true);


--
-- Data for Name: units; Type: TABLE DATA; Schema: public; Owner: user
--

COPY public.units (id, geom, lin_ban, xiao_ban, xiao_ban_w, mian_ji, di_ming, shi_quan_d, g_cheng_lb, gyl_mj, lin_zhong, smd, xianname, xiangname, cunname) FROM stdin;
\.
COPY public.units (id, geom, lin_ban, xiao_ban, xiao_ban_w, mian_ji, di_ming, shi_quan_d, g_cheng_lb, gyl_mj, lin_zhong, smd, xianname, xiangname, cunname) FROM '$$PATH$$/4546.dat';

--
-- Data for Name: users; Type: TABLE DATA; Schema: public; Owner: user
--

COPY public.users (id, name, email, password, enabled, gender, chinesename, photo, officephone, cellphone, address, remark, lastlogintime, createtime, deptid) FROM stdin;
\.
COPY public.users (id, name, email, password, enabled, gender, chinesename, photo, officephone, cellphone, address, remark, lastlogintime, createtime, deptid) FROM '$$PATH$$/4573.dat';

--
-- Name: users_id_seq; Type: SEQUENCE SET; Schema: public; Owner: user
--

SELECT pg_catalog.setval('public.users_id_seq', 209, true);


--
-- Data for Name: geocode_settings; Type: TABLE DATA; Schema: tiger; Owner: postgres
--

COPY tiger.geocode_settings (name, setting, unit, category, short_desc) FROM stdin;
\.
COPY tiger.geocode_settings (name, setting, unit, category, short_desc) FROM '$$PATH$$/4257.dat';

--
-- Data for Name: pagc_gaz; Type: TABLE DATA; Schema: tiger; Owner: postgres
--

COPY tiger.pagc_gaz (id, seq, word, stdword, token, is_custom) FROM stdin;
\.
COPY tiger.pagc_gaz (id, seq, word, stdword, token, is_custom) FROM '$$PATH$$/4258.dat';

--
-- Data for Name: pagc_lex; Type: TABLE DATA; Schema: tiger; Owner: postgres
--

COPY tiger.pagc_lex (id, seq, word, stdword, token, is_custom) FROM stdin;
\.
COPY tiger.pagc_lex (id, seq, word, stdword, token, is_custom) FROM '$$PATH$$/4259.dat';

--
-- Data for Name: pagc_rules; Type: TABLE DATA; Schema: tiger; Owner: postgres
--

COPY tiger.pagc_rules (id, rule, is_custom) FROM stdin;
\.
COPY tiger.pagc_rules (id, rule, is_custom) FROM '$$PATH$$/4260.dat';

--
-- Data for Name: topology; Type: TABLE DATA; Schema: topology; Owner: postgres
--

COPY topology.topology (id, name, srid, "precision", hasz) FROM stdin;
\.
COPY topology.topology (id, name, srid, "precision", hasz) FROM '$$PATH$$/4255.dat';

--
-- Data for Name: layer; Type: TABLE DATA; Schema: topology; Owner: postgres
--

COPY topology.layer (topology_id, layer_id, schema_name, table_name, feature_column, feature_type, level, child_id) FROM stdin;
\.
COPY topology.layer (topology_id, layer_id, schema_name, table_name, feature_column, feature_type, level, child_id) FROM '$$PATH$$/4256.dat';

--
-- Name: configs configs_pkey; Type: CONSTRAINT; Schema: public; Owner: user
--

ALTER TABLE ONLY public.configs
    ADD CONSTRAINT configs_pkey PRIMARY KEY (id);


--
-- Name: depts depts_pkey; Type: CONSTRAINT; Schema: public; Owner: user
--

ALTER TABLE ONLY public.depts
    ADD CONSTRAINT depts_pkey PRIMARY KEY (id);


--
-- Name: units forests_pkey; Type: CONSTRAINT; Schema: public; Owner: user
--

ALTER TABLE ONLY public.units
    ADD CONSTRAINT forests_pkey PRIMARY KEY (id);


--
-- Name: logs logs_pkey; Type: CONSTRAINT; Schema: public; Owner: user
--

ALTER TABLE ONLY public.logs
    ADD CONSTRAINT logs_pkey PRIMARY KEY (id);


--
-- Name: menus menus_pkey; Type: CONSTRAINT; Schema: public; Owner: user
--

ALTER TABLE ONLY public.menus
    ADD CONSTRAINT menus_pkey PRIMARY KEY (id);


--
-- Name: messages messages_pkey; Type: CONSTRAINT; Schema: public; Owner: user
--

ALTER TABLE ONLY public.messages
    ADD CONSTRAINT messages_pkey PRIMARY KEY (id);


--
-- Name: onlines onlines_pkey; Type: CONSTRAINT; Schema: public; Owner: user
--

ALTER TABLE ONLY public.onlines
    ADD CONSTRAINT onlines_pkey PRIMARY KEY (id);


--
-- Name: rangers persons_pkey; Type: CONSTRAINT; Schema: public; Owner: user
--

ALTER TABLE ONLY public.rangers
    ADD CONSTRAINT persons_pkey PRIMARY KEY (id);


--
-- Name: photos points_copy1_pkey; Type: CONSTRAINT; Schema: public; Owner: user
--

ALTER TABLE ONLY public.photos
    ADD CONSTRAINT points_copy1_pkey PRIMARY KEY (id);


--
-- Name: points points_pkey; Type: CONSTRAINT; Schema: public; Owner: user
--

ALTER TABLE ONLY public.points
    ADD CONSTRAINT points_pkey PRIMARY KEY (id);


--
-- Name: powers powers_pkey; Type: CONSTRAINT; Schema: public; Owner: user
--

ALTER TABLE ONLY public.powers
    ADD CONSTRAINT powers_pkey PRIMARY KEY (id);


--
-- Name: rolepowers rolepowers_pkey; Type: CONSTRAINT; Schema: public; Owner: user
--

ALTER TABLE ONLY public.rolepowers
    ADD CONSTRAINT rolepowers_pkey PRIMARY KEY (roleid, powerid);


--
-- Name: roles roles_pkey; Type: CONSTRAINT; Schema: public; Owner: user
--

ALTER TABLE ONLY public.roles
    ADD CONSTRAINT roles_pkey PRIMARY KEY (id);


--
-- Name: roleusers roleusers_pkey; Type: CONSTRAINT; Schema: public; Owner: user
--

ALTER TABLE ONLY public.roleusers
    ADD CONSTRAINT roleusers_pkey PRIMARY KEY (roleid, userid);


--
-- Name: titles titles_pkey; Type: CONSTRAINT; Schema: public; Owner: user
--

ALTER TABLE ONLY public.titles
    ADD CONSTRAINT titles_pkey PRIMARY KEY (id);


--
-- Name: titleusers titleusers_pkey; Type: CONSTRAINT; Schema: public; Owner: user
--

ALTER TABLE ONLY public.titleusers
    ADD CONSTRAINT titleusers_pkey PRIMARY KEY (titleid, userid);


--
-- Name: tracks tracks_pkey; Type: CONSTRAINT; Schema: public; Owner: user
--

ALTER TABLE ONLY public.tracks
    ADD CONSTRAINT tracks_pkey PRIMARY KEY (id);


--
-- Name: rangers u3; Type: CONSTRAINT; Schema: public; Owner: user
--

ALTER TABLE ONLY public.rangers
    ADD CONSTRAINT u3 UNIQUE (id);


--
-- Name: unitrangers unitrangers_pkey; Type: CONSTRAINT; Schema: public; Owner: user
--

ALTER TABLE ONLY public.unitrangers
    ADD CONSTRAINT unitrangers_pkey PRIMARY KEY (id);


--
-- Name: users users_pkey; Type: CONSTRAINT; Schema: public; Owner: user
--

ALTER TABLE ONLY public.users
    ADD CONSTRAINT users_pkey PRIMARY KEY (id);


--
-- Name: depts dept_parent; Type: FK CONSTRAINT; Schema: public; Owner: user
--

ALTER TABLE ONLY public.depts
    ADD CONSTRAINT dept_parent FOREIGN KEY (parentid) REFERENCES public.depts(id) ON UPDATE RESTRICT ON DELETE RESTRICT;


--
-- Name: users dept_users; Type: FK CONSTRAINT; Schema: public; Owner: user
--

ALTER TABLE ONLY public.users
    ADD CONSTRAINT dept_users FOREIGN KEY (deptid) REFERENCES public.depts(id) ON UPDATE RESTRICT ON DELETE RESTRICT;


--
-- Name: tracks f1; Type: FK CONSTRAINT; Schema: public; Owner: user
--

ALTER TABLE ONLY public.tracks
    ADD CONSTRAINT f1 FOREIGN KEY (rangerid) REFERENCES public.rangers(id);


--
-- Name: photos f2; Type: FK CONSTRAINT; Schema: public; Owner: user
--

ALTER TABLE ONLY public.photos
    ADD CONSTRAINT f2 FOREIGN KEY (rangerid) REFERENCES public.rangers(id);


--
-- Name: unitrangers f2; Type: FK CONSTRAINT; Schema: public; Owner: user
--

ALTER TABLE ONLY public.unitrangers
    ADD CONSTRAINT f2 FOREIGN KEY (unitid) REFERENCES public.units(id);


--
-- Name: unitrangers f3; Type: FK CONSTRAINT; Schema: public; Owner: user
--

ALTER TABLE ONLY public.unitrangers
    ADD CONSTRAINT f3 FOREIGN KEY (rangerid) REFERENCES public.rangers(id);


--
-- Name: points f4; Type: FK CONSTRAINT; Schema: public; Owner: user
--

ALTER TABLE ONLY public.points
    ADD CONSTRAINT f4 FOREIGN KEY (rangerid) REFERENCES public.rangers(id);


--
-- Name: menus menu_parent; Type: FK CONSTRAINT; Schema: public; Owner: user
--

ALTER TABLE ONLY public.menus
    ADD CONSTRAINT menu_parent FOREIGN KEY (parentid) REFERENCES public.menus(id) ON UPDATE RESTRICT ON DELETE RESTRICT;


--
-- Name: menus menu_viewpower; Type: FK CONSTRAINT; Schema: public; Owner: user
--

ALTER TABLE ONLY public.menus
    ADD CONSTRAINT menu_viewpower FOREIGN KEY (viewpowerid) REFERENCES public.powers(id) ON UPDATE RESTRICT ON DELETE RESTRICT;


--
-- Name: onlines online_user; Type: FK CONSTRAINT; Schema: public; Owner: user
--

ALTER TABLE ONLY public.onlines
    ADD CONSTRAINT online_user FOREIGN KEY (userid) REFERENCES public.users(id) ON UPDATE RESTRICT ON DELETE CASCADE;


--
-- Name: rolepowers role_powers_source; Type: FK CONSTRAINT; Schema: public; Owner: user
--

ALTER TABLE ONLY public.rolepowers
    ADD CONSTRAINT role_powers_source FOREIGN KEY (roleid) REFERENCES public.roles(id) ON UPDATE RESTRICT ON DELETE CASCADE;


--
-- Name: rolepowers role_powers_target; Type: FK CONSTRAINT; Schema: public; Owner: user
--

ALTER TABLE ONLY public.rolepowers
    ADD CONSTRAINT role_powers_target FOREIGN KEY (powerid) REFERENCES public.powers(id) ON UPDATE RESTRICT ON DELETE CASCADE;


--
-- Name: roleusers role_users_source; Type: FK CONSTRAINT; Schema: public; Owner: user
--

ALTER TABLE ONLY public.roleusers
    ADD CONSTRAINT role_users_source FOREIGN KEY (roleid) REFERENCES public.roles(id) ON UPDATE RESTRICT ON DELETE CASCADE;


--
-- Name: roleusers role_users_target; Type: FK CONSTRAINT; Schema: public; Owner: user
--

ALTER TABLE ONLY public.roleusers
    ADD CONSTRAINT role_users_target FOREIGN KEY (userid) REFERENCES public.users(id) ON UPDATE RESTRICT ON DELETE CASCADE;


--
-- Name: titleusers title_users_source; Type: FK CONSTRAINT; Schema: public; Owner: user
--

ALTER TABLE ONLY public.titleusers
    ADD CONSTRAINT title_users_source FOREIGN KEY (titleid) REFERENCES public.titles(id) ON UPDATE RESTRICT ON DELETE CASCADE;


--
-- Name: titleusers title_users_target; Type: FK CONSTRAINT; Schema: public; Owner: user
--

ALTER TABLE ONLY public.titleusers
    ADD CONSTRAINT title_users_target FOREIGN KEY (userid) REFERENCES public.users(id) ON UPDATE RESTRICT ON DELETE CASCADE;


--
-- PostgreSQL database dump complete
--

