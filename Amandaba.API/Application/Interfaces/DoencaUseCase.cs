using Amandaba.Application.Dtos.Doencas;
using Amandaba.Application.Interfaces;
using Amandaba.Application.Mappers;
using Amandaba.Domain.Interfaces;

namespace Amandaba.Application.UseCases
{
    public class DoencaUseCase : IDoencaUseCase
    {
        private readonly IDoencaRepository _doencaRepository;

        public DoencaUseCase(IDoencaRepository doencaRepository)
        {
            _doencaRepository = doencaRepository;
        }

        public IEnumerable<DoencaResponseDto> ObterPorPet(decimal petId)
        {
            ValidarPet(petId);

            return _doencaRepository
                .ObterPorPet(petId)
                .Select(d => d.ToResponseDto())
                .ToList();
        }

        public DoencaResponseDto? ObterPorId(
            decimal petId,
            decimal doencaId)
        {
            ValidarPet(petId);

            var doenca = _doencaRepository.ObterPorId(
                petId,
                doencaId
            );

            return doenca?.ToResponseDto();
        }

        public DoencaResponseDto Cadastrar(
            decimal petId,
            DoencaRequestDto dto)
        {
            ValidarPet(petId);
            ValidarDados(dto);

            var doenca = dto.ToEntity(petId);

            doenca = _doencaRepository.Cadastrar(doenca);

            return doenca.ToResponseDto();
        }

        public bool Atualizar(
            decimal petId,
            decimal doencaId,
            DoencaRequestDto dto)
        {
            ValidarPet(petId);

            var doenca = _doencaRepository.ObterPorId(
                petId,
                doencaId
            );

            if (doenca is null)
                return false;

            ValidarDados(dto);

            dto.UpdateEntity(doenca);

            _doencaRepository.Atualizar(doenca);

            return true;
        }

        public bool Excluir(
            decimal petId,
            decimal doencaId)
        {
            ValidarPet(petId);

            var doenca = _doencaRepository.ObterPorId(
                petId,
                doencaId
            );

            if (doenca is null)
                return false;

            _doencaRepository.Excluir(doenca);

            return true;
        }

        private void ValidarPet(decimal petId)
        {
            if (!_doencaRepository.ExistePet(petId))
                throw new KeyNotFoundException("Pet não encontrado.");
        }

        private static void ValidarDados(DoencaRequestDto dto)
        {
            dto.Nome = dto.Nome.Trim();

            if (string.IsNullOrWhiteSpace(dto.Nome))
            {
                throw new ArgumentException(
                    "O nome da doença é obrigatório."
                );
            }

            if (dto.DataDiagnostico.HasValue &&
                dto.DataDiagnostico.Value.Date > DateTime.Today)
            {
                throw new ArgumentException(
                    "A data do diagnóstico não pode ser futura."
                );
            }

            dto.Status = dto.Status.Trim().ToUpper();

            if (dto.Status != "ATIVA" &&
                dto.Status != "CONTROLADA" &&
                dto.Status != "ENCERRADA")
            {
                throw new ArgumentException(
                    "Status inválido. Utilize ATIVA, CONTROLADA ou ENCERRADA."
                );
            }
        }
    }
}