# Redis

[Voltar](../README.md)

## Conceitos

- [Sync vs Async vs Fire-and-Forget](https://stackexchange.github.io/StackExchange.Redis/Basics#sync-vs-async-vs-fire-and-forget)
	- Synchronous: Aguardar retorno do servidor para seguir execução do código
	- Asynchronous: Padrão .net para reaproveitamento de threads
	- Fire-and-Forget: Executa operação e não aguarda retorno. Se ocorrer erro, não será interrompida a aplicação. Usar com muito cuidado!
- Contadores
	- ZSet / Sorted: Contador double ordenado descrescente
	- Hash: Contator com chave/valor interno (Equivalente a um dicionário)
	- Simple: Chave/Valor simples
- Multi thread safe compartilhando a mesma conexão