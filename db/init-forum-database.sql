-- 1. Création des rôles utilisateurs
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

-- 2. Création de la base de données (si elle n'existe pas)
DO $$
BEGIN
   IF NOT EXISTS (SELECT 1 FROM pg_database WHERE datname = 'forum') THEN
      CREATE DATABASE forum OWNER forum_admin;
   END IF;
END
$$;

-- 3. Suite du script dans la base 'forum'
\c forum

-- 4. Création du rôle applicatif (si besoin)
DO $$
BEGIN
   IF NOT EXISTS (SELECT 1 FROM pg_roles WHERE rolname = 'forum_app_role') THEN
      CREATE ROLE forum_app_role;
   END IF;
END
$$;

-- 5. Attribution des droits à forum_app_role sur le schéma public et les tables existantes et futures
GRANT USAGE ON SCHEMA public TO forum_app_role;
GRANT SELECT, INSERT, UPDATE, DELETE, REFERENCES ON ALL TABLES IN SCHEMA public TO forum_app_role;
GRANT CREATE ON SCHEMA public TO forum_app_role;

ALTER DEFAULT PRIVILEGES IN SCHEMA public
    GRANT SELECT, INSERT, UPDATE, DELETE, REFERENCES ON TABLES TO forum_app_role;

-- 6. Ajout de forum_app au rôle technique
GRANT forum_app_role TO forum_app;

-- (Optionnel) Pour donner tous les droits futurs sur les tables à forum_app_role (y compris si tu veux ALTÉRER/dropper des tables)
-- ALTER DEFAULT PRIVILEGES IN SCHEMA public GRANT ALL ON TABLES TO forum_app_role;

-- 7. Vérification des attributions (Optionnel)
-- \du
-- \dt

