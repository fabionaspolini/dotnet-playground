namespace Mapster_Sample;

public class PersonModel
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
}

public class PersonModelReadOnly
{
    public PersonModelReadOnly(Guid id, string name)
    {
        Id = id;
        Name = name;
    }

    public Guid Id { get; set; }
    public string Name { get; set; }
}

public class PersonModelDif
{
    public Guid Codigo { get; set; }
    public string? Nome { get; set; }
}

public class PersonModelDifReadonly
{
    public PersonModelDifReadonly(Guid codigo, string nome)
    {
        Codigo = codigo;
        Nome = nome;
    }

    public Guid Codigo { get; set; }
    public string Nome { get; set; }
}