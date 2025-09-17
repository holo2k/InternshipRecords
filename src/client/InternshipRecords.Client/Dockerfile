FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY . .
WORKDIR /src/src/client/InternshipRecords.Client
RUN dotnet publish -c Release -o /app/publish

FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html
COPY --from=build /app/publish/wwwroot .
COPY src/client/InternshipRecords.Client/nginx.conf /etc/nginx/nginx.conf
EXPOSE 80