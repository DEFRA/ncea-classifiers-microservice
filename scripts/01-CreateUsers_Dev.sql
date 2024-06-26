-- Role: "DEVNCEINFID1404"
-- DROP ROLE IF EXISTS "DEVNCEINFID1404";

CREATE ROLE "DEVNCEINFID1404" WITH
  LOGIN
  NOSUPERUSER
  INHERIT
  NOCREATEDB
  NOCREATEROLE
  NOREPLICATION;

GRANT pg_read_all_data, pg_write_all_data TO "DEVNCEINFID1404";

SECURITY LABEL FOR pgaadauth-int
  ON ROLE "DEVNCEINFID1404"
  IS 'type=service,oid=16e79f61-a301-49b2-b9d2-ca841d049fdf,tenant_id=770a2450-0227-4c62-90c7-4e38537f1102';
SECURITY LABEL FOR pgaadauth
  ON ROLE "DEVNCEINFID1404"
  IS 'aadauth';


-- Role: "Oliver.Portingale@Defra.onmicrosoft.com"
-- DROP ROLE IF EXISTS "Oliver.Portingale@Defra.onmicrosoft.com";

CREATE ROLE "Oliver.Portingale@Defra.onmicrosoft.com" WITH
  LOGIN
  NOSUPERUSER
  INHERIT
  CREATEDB
  CREATEROLE
  NOREPLICATION;

GRANT azure_pg_admin TO "Oliver.Portingale@Defra.onmicrosoft.com";

SECURITY LABEL FOR pgaadauth-int
  ON ROLE "Oliver.Portingale@Defra.onmicrosoft.com"
  IS 'type=user,oid=195fa433-e1aa-430a-b6cc-493707c6f258,tenant_id=770a2450-0227-4c62-90c7-4e38537f1102,admin';
SECURITY LABEL FOR pgaadauth
  ON ROLE "Oliver.Portingale@Defra.onmicrosoft.com"
  IS 'aadauth,oid=195fa433-e1aa-430a-b6cc-493707c6f258,type=user,admin';

-- Role: "janardhanan.susaimanipeter@Defra.onmicrosoft.com"
-- DROP ROLE IF EXISTS "janardhanan.susaimanipeter@Defra.onmicrosoft.com";

CREATE ROLE "janardhanan.susaimanipeter@Defra.onmicrosoft.com" WITH
  LOGIN
  NOSUPERUSER
  INHERIT
  CREATEDB
  CREATEROLE
  NOREPLICATION;

GRANT azure_pg_admin, pg_read_all_data, pg_write_all_data TO "janardhanan.susaimanipeter@Defra.onmicrosoft.com";

SECURITY LABEL FOR pgaadauth-int
  ON ROLE "janardhanan.susaimanipeter@Defra.onmicrosoft.com"
  IS 'type=user,oid=e9a1ba76-728e-4ad1-b483-3224c8d85112,tenant_id=770a2450-0227-4c62-90c7-4e38537f1102,admin';
SECURITY LABEL FOR pgaadauth
  ON ROLE "janardhanan.susaimanipeter@Defra.onmicrosoft.com"
  IS 'aadauth,admin';