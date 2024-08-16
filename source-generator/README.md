m31-fluentapi-playground: Não criar classe no padrão "(Dto)Builder".

FluentBuilder: Cria no padrão builder, mas não tem inteligência de identificar campos obrigatórios do contrutor da classe.

DasMulli.DataBuilderGenerator: Melhor opção
- Padrão ok
- Identifica campos obrigatórios pelo construtor da classe
- Bug com file-scoped namespaces.
