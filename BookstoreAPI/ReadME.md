### Overview

This is a RESTful API developed in .NET Core that uses MongoDB as its primary database. It allows users to perform CRUD (Create, Read, Update, Delete) operations on the database through HTTP methods.

## How to Make the Project Runnable

    1: Add the MongoDB connection string in appsettings.json file in MongoDbSettings.
    2: Install the dependencies.
    3: To run server execute command dontnet run

By following the above steps we can configure and run the Backend.

## Recommended Improvements

    1: Need to add the test cases due to the lack of the time I did not implement the test cases.
    2: Authorization and Permission Validation:
        * Add robust authorization and permission checks to ensure only authorized users can perform certain actions.
    3: Implement Monitoring:
        * Integrate monitoring tools like Datadog or Prometheus to track application performance and detect anomalies in real time
    4: I would prfer to use SQL server with .net.
    5: Should have implement the paging for get all books and also should have implement the search.

## How to Deploy on AWS

Deploying the backend application on AWS can be accomplished using the following approaches:
1: GitHub Actions for CI/CD:
_ Set up a CI/CD pipeline using GitHub Actions to automate deployments.
_ Configure AWS credentials and deployment scripts in the GitHub Actions workflow file.
2: Manual Deployment:
_ Package the application and upload it to an AWS service like Elastic Beanstalk, ECS, or Lambda.
_ Use AWS CLI or the AWS Management Console to configure and deploy resources.
