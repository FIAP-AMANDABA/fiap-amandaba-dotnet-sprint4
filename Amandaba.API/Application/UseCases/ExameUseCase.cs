using Amandaba.Application.Dtos.Exames;
using Amandaba.Application.Interfaces;
using Amandaba.Application.Mappers;
using Amandaba.Domain.Interfaces;

namespace Amandaba.Application.UseCases
{
    public class ExameUseCase : IExameUseCase
    {
        private readonly IExameRepository _exameRepository;

        public ExameUseCase(
            IExameRepository exameRepository)
        {
            _exameRepository = exameRepository;
        }

        public IEnumerable<ExameResponseDto> ObterPorPet(
            decimal petId,
            string? status)
        {
            ValidarPet(petId);

            if (!string.IsNullOrWhiteSpace(status))
            {
                status = NormalizarEValidarStatus(status);
            }

            return _exameRepository
                .ObterPorPet(petId, status)
                .Select(e => e.ToResponseDto())
                .ToList();
        }

        public ExameResponseDto? ObterPorId(
            decimal petId,
            decimal exameId)
        {
            ValidarPet(petId);

            var exame = _exameRepository.ObterPorId(
                petId,
                exameId
            );

            return exame?.ToResponseDto();
        }

        public ExameResponseDto Cadastrar(
            decimal petId,
            ExameRequestDto dto)
        {
            ValidarPet(petId);
            ValidarDados(dto);

            var exame = dto.ToEntity(petId);

            exame = _exameRepository.Cadastrar(exame);

            return exame.ToResponseDto();
        }

        public bool Atualizar(
            decimal petId,
            decimal exameId,
            ExameRequestDto dto)
        {
            ValidarPet(petId);

            var exame = _exameRepository.ObterPorId(
                petId,
                exameId
            );

            if (exame is null)
                return false;

            ValidarDados(dto);

            dto.UpdateEntity(exame);

            _exameRepository.Atualizar(exame);

            return true;
        }

        public bool AtualizarStatus(
            decimal petId,
            decimal exameId,
            ExameStatusRequestDto dto)
        {
            ValidarPet(petId);

            var exame = _exameRepository.ObterPorId(
                petId,
                exameId
            );

            if (exame is null)
                return false;

            exame.Status = NormalizarEValidarStatus(
                dto.Status
            );

            _exameRepository.Atualizar(exame);

            return true;
        }

        public bool Excluir(
            decimal petId,
            decimal exameId)
        {
            ValidarPet(petId);

            var exame = _exameRepository.ObterPorId(
                petId,
                exameId
            );

            if (exame is null)
                return false;

            _exameRepository.Excluir(exame);

            return true;
        }

        private void ValidarPet(decimal petId)
        {
            if (!_exameRepository.ExistePet(petId))
            {
                throw new KeyNotFoundException(
                    "Pet não encontrado."
                );
            }
        }

        private static void ValidarDados(
            ExameRequestDto dto)
        {
            dto.Nome = dto.Nome.Trim();

            if (string.IsNullOrWhiteSpace(dto.Nome))
            {
                throw new ArgumentException(
                    "O nome do exame é obrigatório."
                );
            }

            if (dto.DataSolicitacao.HasValue &&
                dto.DataRealizacao.HasValue &&
                dto.DataRealizacao.Value.Date <
                dto.DataSolicitacao.Value.Date)
            {
                throw new ArgumentException(
                    "A data de realização não pode ser anterior à data de solicitação."
                );
            }

            dto.Status = NormalizarEValidarStatus(
                dto.Status
            );
        }

        private static string NormalizarEValidarStatus(
            string status)
        {
            status = status.Trim().ToUpper();

            if (status != "SOLICITADO" &&
                status != "REALIZADO" &&
                status != "RESULTADO_DISPONIVEL")
            {
                throw new ArgumentException(
                    "Status inválido. Utilize SOLICITADO, REALIZADO ou RESULTADO_DISPONIVEL."
                );
            }

            return status;
        }
    }
}