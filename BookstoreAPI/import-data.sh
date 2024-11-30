docker run --name mongodb -d -p 27017:27017 mongo
docker cp books.json mongodb:/data/books.json
docker exec -i mongodb mongoimport --db humTask --collection books --file /data/books.json --jsonArray