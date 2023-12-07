FROM postgres:latest

COPY ./setup.sh /docker-entrypoint-initdb.d/setup.sh

EXPOSE 5432

ENTRYPOINT ["docker-entrypoint.sh"]

CMD ["postgres"]