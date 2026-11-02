using Amandaba.API.Domain.Interfaces;
using Amandaba.Application.Dtos.Pets;
using Amandaba.Application.Interfaces;
using Amandaba.Application.Mappers;
using Amandaba.Domain.Rules;

namespace Amandaba.Application.UseCases
{
    public class PetUseCase : IPetUseCase
    {
        private readonly IPetRepository _petRepository;

        public PetUseCase(IPetRepository petRepository)
        {
            _petRepository = petRepository;
        }

        public IEnumerable<PetResponseDto> ObterPorTutor(decimal idTutor)
        {
            if (!_petRepository.ExisteTutor(idTutor))
                throw new KeyNotFoundException("Tutor não encontrado.");

            return _petRepository
                .ObterPorTutor(idTutor)
                .Select(p => p.ToResponseDto())
                .ToList();
        }

        public Amandaba.API.Application.Dtos.PageResponseDto<PetResponseDto> ObterPorTutorPaginado(
            decimal idTutor, int pagina, int tamanho, string? nome, string? ordem)
        {
            if (!_petRepository.ExisteTutor(idTutor))
                throw new KeyNotFoundException("Tutor nao encontrado.");
            pagina = Math.Max(1, pagina);
            tamanho = Math.Clamp(tamanho, 1, 100);
            var deslocamento = (int)Math.Min((long)(pagina - 1) * tamanho, int.MaxValue);
            var (dados, total) = _petRepository.ObterPorTutorPaginado(
                idTutor, nome, string.Equals(ordem, "nome_desc", StringComparison.OrdinalIgnoreCase),
                deslocamento, tamanho);
            return new Amandaba.API.Application.Dtos.PageResponseDto<PetResponseDto>
            {
                Deslocamento = deslocamento, RegistroRetornado = tamanho,
                TotalRegistros = total, Items = dados.Select(p => p.ToResponseDto()).ToList()
            };
        }

        public PetResponseDto? ObterPorId(decimal idPet)
        {
            var pet = _petRepository.ObterPorId(idPet);

            return pet?.ToResponseDto();
        }

        public PetResponseDto Cadastrar(decimal idTutor, PetRequestDto dto)
        {
            if (!_petRepository.ExisteTutor(idTutor))
                throw new KeyNotFoundException("Tutor não encontrado.");

            ValidarDados(dto);

            var pet = dto.ToEntity(idTutor);

            pet = _petRepository.Cadastrar(pet);

            var petCadastrado = _petRepository.ObterPorId(pet.IdPet);

            return petCadastrado!.ToResponseDto();
        }

        public bool Atualizar(decimal idPet, PetRequestDto dto)
        {
            var pet = _petRepository.ObterPorId(idPet);

            if (pet is null)
                return false;

            ValidarDados(dto);

            dto.UpdateEntity(pet);

            _petRepository.Atualizar(pet);

            return true;
        }

        public bool AtualizarStatus(decimal idPet, PetStatusRequestDto dto)
        {
            var pet = _petRepository.ObterPorId(idPet);

            if (pet is null)
                return false;

            pet.Status = PetRules.ValidarStatus(dto.Status);

            _petRepository.Atualizar(pet);

            return true;
        }

        private void ValidarDados(PetRequestDto dto)
        {
            if (!_petRepository.ExisteEspecie(dto.IdEspecie))
                throw new ArgumentException("Espécie informada não existe.");

            PetRules.ValidarNascimento(dto.DataNascimento);
            dto.Sexo = PetRules.ValidarSexo(dto.Sexo);
            dto.Nome = PetRules.ValidarNome(dto.Nome);
        }
    }
}