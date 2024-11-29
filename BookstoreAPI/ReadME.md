### Overview

This is a RESTful API developed in .NET Core that uses MongoDB as its primary database. It allows users to perform CRUD (Create, Read, Update, Delete) operations on the database through HTTP methods.

## How to configure DB

To configure the DB you can either spin up the docket containser or use your local DB. you can spin up the docker container by running the following command.

    docker run --name mongodb -d -p 27017:27017 mongo

If you care using the docker contianer, you can also import the data as well, you need to go to the BookstoreAPI directory and execute the following commands.

    docker cp books.json mongodb:/data/books.json
    docker exec -i mongodb mongoimport --db humTask --collection books --file /data/books.json --jsonArray

Now if you want to use the local DB, you can import the following json file

    https://github.com/Hamzaarshad1/Task_hum/blob/main/BookstoreAPI/humTask.books.json

## How to Make the Project Runnable

    1: After running the container and importing the data. you need to update the MongoDB connection string  accordingly in appsettings.json file in MongoDbSettings.
    2: Install the dependencies.
    3: To run server execute command
        dontnet run
    4: To run the test cases you can execute
        dontnet test

By following the above steps we can configure and run the Backend.

## Recommended Improvements

    1: Authorization and Permission Validation:
        * Add robust authorization and permission checks to ensure only authorized users can perform certain actions.
    2: Implement Monitoring:
        * Integrate monitoring tools like Datadog or Prometheus to track application performance and detect anomalies in real time
    3: Should have implement the search.

## How to Deploy on AWS

Deploying the backend application on AWS can be accomplished using the following approaches:
1: GitHub Actions for CI/CD:
_ Set up a CI/CD pipeline using GitHub Actions to automate deployments.
_ Configure AWS credentials and deployment scripts in the GitHub Actions workflow file.
2: Manual Deployment:
_ Package the application and upload it to an AWS service like Elastic Beanstalk, ECS, or Lambda.
_ Use AWS CLI or the AWS Management Console to configure and deploy resources.
