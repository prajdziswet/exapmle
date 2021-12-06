# mvc

создайте докер контейнер,запустите докер контейнер
docker container run -d --name=psq1 -p 2345:5432 -e POSTGRES_PASSWORD=praj -v "${pwd}/pgdata:/pgdata" postgres:14.1
(создает каталог но не вежу базу)

