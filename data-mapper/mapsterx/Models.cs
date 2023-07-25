namespace Mapster_Sample;

public class PersonModel
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? LastName { get; set; }
    // public string? Test { get; set; }
}

public class PersonModelReadOnly
{
    public PersonModelReadOnly(Guid id, string name)
    {
        Id = id;
        Name = name;
    }

    public Guid Id { get; }
    public string Name { get; }
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

    public Guid Codigo { get; }
    public string Nome { get; }
}

// [Mapper]
// public interface IMapperToPersonModel
// {
//     //map from POCO to DTO
//     PersonModel MapToDto(Person entity);
// }

// [Mapper]
// public interface IMapperToPersonModelReadOnly
// {
//     //map from POCO to DTO
//     PersonModelReadOnly MapToDto(Person entity);
// }

// [Mapper]
// public interface IMapperToPersonModelDif
// {
//     //map from POCO to DTO
//     PersonModelDif MapToDto(Person entity);
// }