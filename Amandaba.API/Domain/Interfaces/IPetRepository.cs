using Amandaba.Domain.Entities;

namespace Amandaba.API.Domain.Interfaces
{
    public interface IPetRepository
    {
        IEnumerable<PetEntity> ObterPorTutor(decimal idTutor);
        (IEnumerable<PetEntity> data, int rowsCount) ObterPorTutorPaginado(
            decimal idTutor, string? nome, bool ordemDescendente, int deslocamento, int registrosRetornados);

        PetEntity? ObterPorId(decimal idPet);

        PetEntity Cadastrar(PetEntity pet);

        void Atualizar(PetEntity pet);

        bool ExisteTutor(decimal idTutor);

        bool ExisteEspecie(decimal idEspecie);
    }
}