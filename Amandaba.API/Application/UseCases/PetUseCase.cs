using Amandaba.API.Domain.Interfaces;
using Amandaba.Application.Dtos.Pets;
using Amandaba.Application.Interfaces;
using Amandaba.Application.Mappers;

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

            var status = dto.Status.Trim().ToUpper();

            if (status != "ATIVO" && status != "INATIVO")
                throw new ArgumentException(
                    "Status inválido. Utilize ATIVO ou INATIVO."
                );

            pet.Status = status;

            _petRepository.Atualizar(pet);

            return true;
        }

        private void ValidarDados(PetRequestDto dto)
        {
            if (!_petRepository.ExisteEspecie(dto.IdEspecie))
                throw new ArgumentException("Espécie informada não existe.");

            if (dto.DataNascimento.HasValue &&
                dto.DataNascimento.Value.Date > DateTime.Today)
            {
                throw new ArgumentException(
                    "A data de nascimento não pode ser futura."
                );
            }

            if (!string.IsNullOrWhiteSpace(dto.Sexo))
            {
                var sexo = dto.Sexo.Trim().ToUpper();

                if (sexo != "MACHO" && sexo != "FEMEA")
                    throw new ArgumentException(
                        "Sexo inválido. Utilize MACHO ou FEMEA."
                    );

                dto.Sexo = sexo;
            }

            dto.Nome = dto.Nome.Trim();
        }
    }
}