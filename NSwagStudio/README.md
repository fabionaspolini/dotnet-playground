Dependências com NewtonSoft

Propriedades:
- **Namespace:** Definir namespace para geração de código
- **Operation Generate Model:** Layout para o design de código gerado
  - **MultipleClientsFromOperationid:** `var response = await new Client(http).FilmsAllAsync();`
  - **MultipleClientsFromPathSegment:** `var response = await new Client(http).FilmsGetAsync();`
  - **MultipleClientsFromFirstTagAndPathSegments:** `var response = await new FilmsClient(http).FilmsGetAsync();`
  - **MultipleClientsFromFirstTagAndOperationId:** `var response = await new FilmsClient(http).FilmsAllAsync();`
  - **SingleClientFromOperationId:** `var response = await new Client(http).FilmsAllAsync();`
  - **SingleClientFromPathSegments:** `var response = await new Client(http).FilmsGetAsync();`
- **Generated optional parameters:** Parâmetros opcionais do arquivo Swagger são gerador com o default null no C#
- **Base Class Name:** Permite definir uma classe personalizada para herdar o client do código gerado;
  - **Configuration Class Name:** Ao definir um Base Class Name é possível indicar uma classe para configurações a injetar pelo construtor
