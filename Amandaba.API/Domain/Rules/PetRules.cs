namespace Amandaba.Domain.Rules;

// Regras de dominio reutilizadas pelo caso de uso, sem dependencia de HTTP ou banco.
public static class PetRules
{
    public static string ValidarStatus(string? status)
    {
        var valor = status?.Trim().ToUpperInvariant();
        if (valor != "ATIVO" && valor != "INATIVO")
            throw new ArgumentException("Status inválido. Utilize ATIVO ou INATIVO.");
        return valor;
    }

    public static string? ValidarSexo(string? sexo)
    {
        if (string.IsNullOrWhiteSpace(sexo)) return sexo;
        var valor = sexo.Trim().ToUpperInvariant();
        if (valor != "MACHO" && valor != "FEMEA")
            throw new ArgumentException("Sexo inválido. Utilize MACHO ou FEMEA.");
        return valor;
    }

    public static void ValidarNascimento(DateTime? nascimento)
    {
        if (nascimento.HasValue && nascimento.Value.Date > DateTime.Today)
            throw new ArgumentException("A data de nascimento não pode ser futura.");
    }

    public static string ValidarNome(string? nome)
    {
        var valor = nome?.Trim();
        if (string.IsNullOrWhiteSpace(valor))
            throw new ArgumentException("O nome do pet é obrigatório.");
        return valor;
    }
}
