using System.Linq;
using FluentValidation;
using FluentValidation.Validators;

namespace FluentValidation_Sample
{
    class PessoaValidator : AbstractValidator<Pessoa>
    {
        public PessoaValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Nome).NotEmpty().Length(3, 60);
            RuleFor(x => x.Email).EmailAddress();

            RuleFor(x => x.Documento)
                .NotEmpty()
                .Length(11)
                .When(x => x.Tipo == TipoPessoa.Fisica)
                .WithMessage("'Documento' deve ter 11 caracteres quando o 'Tipo' é 'Fisica'.");
            RuleFor(x => x.Documento)
                .NotEmpty()
                .Length(14)
                .When(x => x.Tipo == TipoPessoa.Juridica)
                .WithMessage("'Documento' deve ter 14 caracteres quando o 'Tipo' é 'Juridica'.");

            RuleFor(x => x.CidadeId)
                .NotEmpty()
                //.Must(cidadeId => Program.Cidades.Any(c => c.Id == cidadeId)) // Extension method CidadeIdByPredicate() criado para abstrair essa implementação
                .CidadeIdByPredicate()
                .WithMessage("'Cidade Id' não encontrada (by Predicate).");

            // Ou validação com classe implementada CidadeIdValidator
            RuleFor(x => x.CidadeId)
                .NotEmpty()
                .CidadeId()
                .WithMessage("'Cidade Id' não encontrada (by Property Validator).");

            RuleFor(x => x.Fisica)
                .NotNull()
                .SetValidator(new PessoaFisicaValidator())
                .When(x => x.Tipo == TipoPessoa.Fisica);

            RuleFor(x => x.Juridica).NotNull().When(x => x.Tipo == TipoPessoa.Juridica);
            RuleFor(x => x.Juridica.RazaoSocial)
                .NotNull()
                .Length(3, 60)
                .When(x => x.Juridica != null); // Validação inline de subpropriedades devem ter a checagem de null
        }
    }

    class PessoaFisicaValidator : AbstractValidator<PessoaFisica>
    {
        public PessoaFisicaValidator()
        {
            RuleFor(x => x.NomeMae).NotEmpty().Length(3, 60);
        }
    }


    /// <summary>
    /// Validador de propriedade int para verificar se o código informado está em outra fonte de dados.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    class CidadeIdValidator<T> : PropertyValidator<T, int>
    {

        public override string Name => "CidadeIdValidator";

        public override bool IsValid(ValidationContext<T> context, int value)
        {
            return Program.Cidades.Any(x => x.Id == value);
        }

        protected override string GetDefaultMessageTemplate(string errorCode)
            => "'{PropertyName}' não é um código de cidade válido.";
    }

    static class MyValidatorExtensions
    {
        public static IRuleBuilderOptions<T, int> CidadeIdByPredicate<T>(this IRuleBuilder<T, int> ruleBuilder)
        {
            return ruleBuilder.Must(cidadeId => Program.Cidades.Any(x => x.Id == cidadeId));
        }

        public static IRuleBuilderOptions<T, int> CidadeId<T>(this IRuleBuilder<T, int> ruleBuilder)
        {
            return ruleBuilder.SetValidator(new CidadeIdValidator<T>());
        }
    }
}
