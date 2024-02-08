FROM public.ecr.aws/lambda/dotnet:8-preview as base

FROM mcr.microsoft.com/dotnet/sdk:8.0 as build-image
WORKDIR /app

# Copy only the projects we want to work with
COPY timeout_sample timeout_sample
COPY timeout_sample_datalayer timeout_sample_datalayer
ENV PATH="${PATH}:/root/.dotnet/tools"
RUN apt-get update && \
    apt-get install -y unzip && \
dotnet tool install -g Amazon.Lambda.Tools && \
ls
RUN dotnet publish ./timeout_sample/timeout_sample.csproj -c Release -o /publish

FROM base AS final
WORKDIR /var/task
COPY --from=build-image /publish .
CMD ["timeout_sample"]