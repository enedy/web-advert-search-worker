# Serverless Application Project - SNS

This starter project consists of:
* serverless.template - An AWS CloudFormation Serverless Application Model template file for declaring your Serverless functions and other AWS resources
* Function.cs - Class file containing the C# method mapped to the single function declared in the template file
* aws-lambda-tools-defaults.json - Default argument settings for use within Visual Studio and command line deployment tools for AWS

## Packaging as a Docker image.

This project is configured to package the Lambda function as a Docker image. The default configuration for the project and the Dockerfile is to build 
the .NET project on the host machine and then execute the `docker build` command which copies the .NET build artifacts from the host machine into 
the Docker image. 

The `--docker-host-build-output-dir` switch, which is set in the `aws-lambda-tools-defaults.json`, triggers the 
AWS .NET Lambda tooling to build the .NET project into the directory indicated by `--docker-host-build-output-dir`. The Dockerfile 
has a **COPY** command which copies the value from the directory pointed to by `--docker-host-build-output-dir` to the `/var/task` directory inside of the 
image.

Alternatively the Docker file could be written to use [multi-stage](https://docs.docker.com/develop/develop-images/multistage-build/) builds and 
have the .NET project built inside the container. Below is an example of building .NET 5 project inside the image.

```dockerfile
FROM ecr.aws/lambda/dotnet:5.0 AS base

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim as build
WORKDIR /src
COPY ["WebAdvert.SearchWorker.csproj", "WebAdvert.SearchWorker/"]
RUN dotnet restore "WebAdvert.SearchWorker/WebAdvert.SearchWorker.csproj"

WORKDIR "/src/WebAdvert.SearchWorker"
COPY . .
RUN dotnet build "WebAdvert.SearchWorker.csproj" --configuration Release --output /app/build

FROM build AS publish
RUN dotnet publish "WebAdvert.SearchWorker.csproj" \
            --configuration Release \ 
            --runtime linux-x64 \
            --self-contained false \ 
            --output /app/publish \
            -p:PublishReadyToRun=true  

FROM base AS final
WORKDIR /var/task
COPY --from=publish /app/publish .
```

When building the .NET project inside the image you must be sure to copy all of the class libraries the .NET Lambda project is depending on 
as well before the `dotnet build` step. The final published artifacts of the .NET project must be copied to the `/var/task` directory. 
The `--docker-host-build-output-dir` switch can also be removed from the `aws-lambda-tools-defaults.json` to avoid the 
.NET project from being built on the host machine before calling `docker build`.

## Here are some steps to follow from Visual Studio:

To deploy your Serverless application, right click the project in Solution Explorer and select *Publish to AWS Lambda*.

To view your deployed application open the Stack View window by double-clicking the stack name shown beneath the AWS CloudFormation node in the AWS Explorer tree. The Stack View also displays the root URL to your published application.
```
