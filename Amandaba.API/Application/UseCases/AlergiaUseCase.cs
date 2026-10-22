using Amandaba.API.Application.Dtos.Alergias;
using Amandaba.Application.Dtos.Alergias;
using Amandaba.Application.Interfaces;
using Amandaba.Application.Mappers;
using Amandaba.Domain.Interfaces;

namespace Amandaba.Application.UseCases
{
    public class AlergiaUseCase : IAlergiaUseCase
    {
        private readonly IAlergiaRepository _alergiaRepository;

        public AlergiaUseCase(IAlergiaRepository alergiaRepository)
        {
            _alergiaRepository = alergiaRepository;
        }

        public IEnumerable<AlergiaResponseDto> ObterPorPet(decimal petId)
        {
            ValidarPet(petId);

            return _alergiaRepository
                .ObterPorPet(petId)
                .Select(a => a.ToResponseDto())
                .ToList();
        }

        public AlergiaResponseDto? ObterPorId(
            decimal petId,
            decimal alergiaId)
        {
            ValidarPet(petId);

            var alergia = _alergiaRepository
                .ObterPorId(petId, alergiaId);

            return alergia?.ToResponseDto();
        }

        public AlergiaResponseDto Cadastrar(
            decimal petId,
            AlergiaRequestDto dto)
        {
            ValidarPet(petId);
            ValidarDados(dto);

            var alergia = dto.ToEntity(petId);

            alergia = _alergiaRepository.Cadastrar(alergia);

            return alergia.ToResponseDto();
        }

        public bool Atualizar(
            decimal petId,
            decimal alergiaId,
            AlergiaRequestDto dto)
        {
            ValidarPet(petId);

            var alergia = _alergiaRepository
                .ObterPorId(petId, alergiaId);

            if (alergia is null)
                return false;

            ValidarDados(dto);

            dto.UpdateEntity(alergia);

            _alergiaRepository.Atualizar(alergia);

            return true;
        }

        public bool Excluir(
            decimal petId,
            decimal alergiaId)
        {
            ValidarPet(petId);

            var alergia = _alergiaRepository
                .ObterPorId(petId, alergiaId);

            if (alergia is null)
                return false;

            _alergiaRepository.Excluir(alergia);

            return true;
        }

        private void ValidarPet(decimal petId)
        {
            if (!_alergiaRepository.ExistePet(petId))
                throw new KeyNotFoundException("Pet não encontrado.");
        }

        private static void ValidarDados(AlergiaRequestDto dto)
        {
            dto.Nome = dto.Nome.Trim();

            if (string.IsNullOrWhiteSpace(dto.Nome))
            {
                throw new ArgumentException(
                    "O nome da alergia é obrigatório."
                );
            }

            if (dto.DataIdentificacao.HasValue &&
                dto.DataIdentificacao.Value.Date > DateTime.Today)
            {
                throw new ArgumentException(
                    "A data de identificação não pode ser futura."
                );
            }

            if (!string.IsNullOrWhiteSpace(dto.Tipo))
            {
                dto.Tipo = dto.Tipo.Trim().ToUpper();

                if (dto.Tipo != "MEDICAMENTO" &&
                    dto.Tipo != "ALIMENTO" &&
                    dto.Tipo != "AMBIENTAL" &&
                    dto.Tipo != "OUTRO")
                {
                    throw new ArgumentException(
                        "Tipo inválido. Utilize MEDICAMENTO, ALIMENTO, AMBIENTAL ou OUTRO."
                    );
                }
            }

            if (!string.IsNullOrWhiteSpace(dto.Gravidade))
            {
                dto.Gravidade = dto.Gravidade.Trim().ToUpper();

                if (dto.Gravidade != "LEVE" &&
                    dto.Gravidade != "MODERADA" &&
                    dto.Gravidade != "GRAVE")
                {
                    throw new ArgumentException(
                        "Gravidade inválida. Utilize LEVE, MODERADA ou GRAVE."
                    );
                }
            }
        }
    }
}