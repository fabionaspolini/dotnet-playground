- NotEmpty(): Valida se a valor é diferente do default da classe.
    - `string`: Diferente de `null`, vazia e não é espaço em branco.
    - `Guid`: Diferente do default `00000000-0000-0000-0000-000000000000`
    - Números (`int, float, double, decimal`): Diferente de `0`.
    - `Enum`: Diferente de 0 (Primeiro item do enum caso não estiverem com valores atribuidos).
- Subtipos complexos:
    - `SetValidator(new SubpropValidator())`: Atribuir validador para objeto filho. Quando este for nulo o mesmo não é executado. Este modelo imprime apenas o nome do campo filho na mensagem.
    - inline (`.RuleFor(x => x.Prop.Campo)`): Deve possuir a verificação para checar se `Prop` é diferente de `null` em todas expressões. `.When(x => x.Prop != null)`. Este modelo imprime a mensagem no padrão `Prop Campo ....`
- Validadores personalizados:
    - Predicate Validator: `RuleFor(x => x.Pets).Must(list => list.Count < 10).WithMessage("The list must contain fewer than 10 items")`;
        - Neste caso podem ser feitos extensions methods para facilitar o uso.
    - Também é possível criar placeholder personalizados para incluir na mensagem de validação, veja na [documentação](https://docs.fluentvalidation.net/en/latest/custom-validators.html#custom-message-placeholders).

```csharp
var result = pessoaValidator.Validate(pessoa);

result.ToString();      // Retorna todos os erros quebrados por linha
result.ToString(" / ")  // Retorna todos os erros em uma única linha separados por " / "
result.Errors           // Lista com cada regra que falhou
```

## Configuração da validação

- `CascadeMode = CascadeMode.Stop`: Falha rápida. Aborta a validação na primeira falha que ocorrer. Irá retornar somente um erro no validador.
- `CascadeMode = CascadeMode.StopOnFirstFailure`: Valida todos as regras de campos, mas irá retornar somente um erro por campo.
- `CascadeMode = CascadeMode.Continue`:  Opção padrão, valida todas as regras. Poderá retornar mais de um erro por campo.
- `ValidatorOptions.Global.CascadeMode`: Propriedades para alterar o comportamento global
