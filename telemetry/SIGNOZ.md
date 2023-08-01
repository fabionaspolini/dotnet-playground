# SigNoz

Exemplo instrumentação: <https://github.com/SigNoz/sample-ASPNETCore-app/blob/main/Startup.cs#L84>

## Local server

[Tutorial](https://signoz.io/docs/install/docker/#install-signoz-using-docker-compose)

```bash
git clone -b main https://github.com/SigNoz/signoz.git && cd signoz/deploy/

docker compose -f docker/clickhouse-setup/docker-compose.yaml up -d

docker compose -f docker/clickhouse-setup/docker-compose.yaml down -v
```

Web Page: <http://localhost:3301>