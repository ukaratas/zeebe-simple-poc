# zeebe-simple-poc

Simple POC implementation with Zeebe including below concepts;

* Asynchronous communication between zeebe, server and client.
* UI flow management inside zeebe process.
* Loosely coupled custom persistance implemetation.
* Sampling Headers and Input/Output variables.

## Process
Process is stored in the **Processes** folder of project. Project is deploying process at startup. You can use **Zeebe Modeller** to modify it and also upload to camunda cloud.

![](https://i.imgur.com/LrZISUQ.png)

## How to run 
1. Setup [Zeebe on Docker](https://github.com/camunda-cloud/camunda-cloud-get-started/blob/master/docker-compose.yaml)
2. Run the project :)
