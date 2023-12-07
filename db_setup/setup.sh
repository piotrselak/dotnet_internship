#!/bin/bash
set -e

IFS=',' read -ra DBS <<< "$POSTGRES_MULTIPLE_DBS"
service postgresql start

for DB in "${DBS[@]}"
do
    psql -v ON_ERROR_STOP=1 --username "$POSTGRES_USER" --dbname "$POSTGRES_DB" <<-EOSQL
        CREATE DATABASE $DB;
EOSQL

    psql -v ON_ERROR_STOP=1 --username "$POSTGRES_USER" --dbname "$POSTGRES_DB" <<-EOSQL
        GRANT ALL PRIVILEGES ON DATABASE $DB TO $POSTGRES_USER;
EOSQL
done

service postgresql stop