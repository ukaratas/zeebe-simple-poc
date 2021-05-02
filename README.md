# zeebe-simple-poc

Simple POC implementation with Zeebe including below concepts;

* Asynchronous communication between zeebe, server and client.
* UI flow management inside zeebe process.
* Loosely coupled custom persistance implemetation.
* Sampling Headers and Input/Output variables.

![](https://i.imgur.com/LrZISUQ.png)

## How to run 

1. Login to Cloud version of Zeebe  [console.cloud.camunda.io] https://console.cloud.camunda.io/)
2. Follow the [instructions](https://docs.camunda.io/docs/guides/) to create a zeebe cluster.
3. Note that zeebe client connection credentials.
4. Create a config file **zeebe-config.json** including zeebe credentials. *Sample config file is exists on root folder of project which named credentials.zeebe-config.json-sample*

```json
{
      "authServer": "https://login.cloud.camunda.io/oauth/token",
      "clientId": "CD********************************zMI",
      "clientSecret": "1************************************",
      "zeebeUrl": "f28e******************ca819.zeebe.camunda.io:443"
}
```

5. Run the project :)