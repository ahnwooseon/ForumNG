-- 1. Creation of user roles
DO $$
BEGIN
   IF NOT EXISTS (SELECT 1 FROM pg_roles WHERE rolname = 'forum_admin') THEN
      CREATE ROLE forum_admin LOGIN PASSWORD 'Auietsrn0';
   END IF;
   IF NOT EXISTS (SELECT 1 FROM pg_roles WHERE rolname = 'forum_app') THEN
      CREATE ROLE forum_app LOGIN PASSWORD 'Auietsrn0';
   END IF;
END
$$;

-- 2. Creation of the database (if it doesn't exist)
DO $$
BEGIN
   IF NOT EXISTS (SELECT 1 FROM pg_database WHERE datname = 'forum') THEN
      CREATE DATABASE forum OWNER forum_admin;
   END IF;
END
$$;

-- 3. Continue the script in the 'forum' database
\c forum

-- 4. Creation of the application role (if needed)
DO $$
BEGIN
   IF NOT EXISTS (SELECT 1 FROM pg_roles WHERE rolname = 'forum_app_role') THEN
      CREATE ROLE forum_app_role;
   END IF;
END
$$;

-- 5. Grant privileges to forum_app_role on the public schema and all existing and future tables
GRANT USAGE ON SCHEMA public TO forum_app_role;
GRANT SELECT, INSERT, UPDATE, DELETE, REFERENCES ON ALL TABLES IN SCHEMA public TO forum_app_role;
GRANT CREATE ON SCHEMA public TO forum_app_role;

ALTER DEFAULT PRIVILEGES IN SCHEMA public
    GRANT SELECT, INSERT, UPDATE, DELETE, REFERENCES ON TABLES TO forum_app_role;

-- 6. Add forum_app to the technical role
GRANT forum_app_role TO forum_app;

-- (Optional) To give all future table rights to forum_app_role (including ALTER/drop permissions)
-- ALTER DEFAULT PRIVILEGES IN SCHEMA public GRANT ALL ON TABLES TO forum_app_role;

-- 7. Check role grants (Optional)
-- \du
-- \dt
