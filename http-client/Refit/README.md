## Refit

- Para ignorar um propriedade ao mostrar q requisição é necessário atribuir o modificador de acesso `private get`. Exemplo: `public int Prop { private get; set; }`

## Configurando a execução

- Cadastra-se gratuitamente no portal de desenvolvedores Marvel: <https://developer.marvel.com/account>
- Localize as chaves publicas e privadas
- Abra o terminal da pasta do `FlurlHttp-Sample.csproj`
- Configure os secrets através dos comando:

```bash
dotnet user-secrets set "privateKey" "d484a8f0f81d5ccfa1ca8640827294f5151a4ac8"
dotnet user-secrets set "publicKey" "1b07df1a3944b57a8d8823c41cfa221b"
```

Não sabe como funciona o secrets do dotnet, veja [aqui](https://docs.microsoft.com/pt-br/aspnet/core/security/app-secrets?view=aspnetcore-5.0&tabs=windows#enable-secret-storage).
