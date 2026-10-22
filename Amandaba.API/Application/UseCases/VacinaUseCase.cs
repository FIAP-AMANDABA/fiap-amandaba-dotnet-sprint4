using Amandaba.API.Application.Interfaces;
using Amandaba.Application.Dtos.Vacinas;
using Amandaba.Application.Mappers;
using Amandaba.Domain.Interfaces;

namespace Amandaba.Application.UseCases
{
    public class VacinaUseCase : IVacinaUseCase
    {
        private readonly IVacinaRepository _vacinaRepository;

        public VacinaUseCase(IVacinaRepository vacinaRepository)
        {
            _vacinaRepository = vacinaRepository;
        }

        public IEnumerable<VacinaCatalogoResponseDto> ObterCatalogo()
        {
            return _vacinaRepository
                .ObterCatalogo()
                .Select(v => v.ToCatalogoResponseDto())
                .ToList();
        }

        public VacinaCatalogoResponseDto? ObterVacinaPorId(decimal vacinaId)
        {
            var vacina = _vacinaRepository.ObterVacinaPorId(vacinaId);

            return vacina?.ToCatalogoResponseDto();
        }

        public IEnumerable<VacinaAplicacaoResponseDto> ObterAplicacoesPorPet(
            decimal petId)
        {
            ValidarPet(petId);

            return _vacinaRepository
                .ObterAplicacoesPorPet(petId)
                .Select(v => v.ToAplicacaoResponseDto())
                .ToList();
        }

        public VacinaAplicacaoResponseDto? ObterAplicacaoPorId(
            decimal petId,
            decimal aplicacaoId)
        {
            ValidarPet(petId);

            var aplicacao = _vacinaRepository
                .ObterAplicacaoPorId(petId, aplicacaoId);

            return aplicacao?.ToAplicacaoResponseDto();
        }

        public VacinaAplicacaoResponseDto CadastrarAplicacao(
            decimal petId,
            VacinaAplicacaoRequestDto dto)
        {
            ValidarPet(petId);
            ValidarDados(dto);

            var aplicacao = dto.ToEntity(petId);

            aplicacao = _vacinaRepository.CadastrarAplicacao(aplicacao);

            var cadastrada = _vacinaRepository.ObterAplicacaoPorId(
                petId,
                aplicacao.IdAplicacaoVacina
            );

            return cadastrada!.ToAplicacaoResponseDto();
        }

        public bool AtualizarAplicacao(
            decimal petId,
            decimal aplicacaoId,
            VacinaAplicacaoRequestDto dto)
        {
            ValidarPet(petId);

            var aplicacao = _vacinaRepository
                .ObterAplicacaoPorId(petId, aplicacaoId);

            if (aplicacao is null)
                return false;

            ValidarDados(dto);

            dto.UpdateEntity(aplicacao);

            _vacinaRepository.AtualizarAplicacao(aplicacao);

            return true;
        }

        public bool ExcluirAplicacao(
            decimal petId,
            decimal aplicacaoId)
        {
            ValidarPet(petId);

            var aplicacao = _vacinaRepository
                .ObterAplicacaoPorId(petId, aplicacaoId);

            if (aplicacao is null)
                return false;

            _vacinaRepository.ExcluirAplicacao(aplicacao);

            return true;
        }

        private void ValidarPet(decimal petId)
        {
            if (!_vacinaRepository.ExistePet(petId))
                throw new KeyNotFoundException("Pet não encontrado.");
        }

        private void ValidarDados(VacinaAplicacaoRequestDto dto)
        {
            if (_vacinaRepository.ObterVacinaPorId(dto.IdVacina) is null)
                throw new ArgumentException("Vacina informada não existe.");

            if (dto.DataAplicacao.Date > DateTime.Today)
            {
                throw new ArgumentException(
                    "A data de aplicação não pode ser futura."
                );
            }

            if (dto.ProximaDose.HasValue &&
                dto.ProximaDose.Value.Date < dto.DataAplicacao.Date)
            {
                throw new ArgumentException(
                    "A próxima dose não pode ser anterior à data de aplicação."
                );
            }
        }
    }
}