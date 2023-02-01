## Configurando a execução

- Cadastra-se gratuitamente no portal de desenvolvedores Marvel: <https://developer.marvel.com/account>
- Localize as chaves publicas e privadas
- Abra o terminal da pasta do `FlurlHttp-Sample.csproj`
- Configure os secrets através dos comando:

```bash
dotnet user-secrets set "privateKey" "<your-private-key>"
dotnet user-secrets set "publicKey" "<your-public-key>"
```

Não sabe como funciona o secrets do dotnet, veja [aqui](https://docs.microsoft.com/pt-br/aspnet/core/security/app-secrets?view=aspnetcore-5.0&tabs=windows#enable-secret-storage).
