using Amandaba.API.Domain.Interfaces;
using Amandaba.Application.Dtos.Pesos;
using Amandaba.Application.Interfaces;
using Amandaba.Application.Mappers;

namespace Amandaba.Application.UseCases
{
    public class PesoUseCase : IPesoUseCase
    {
        private readonly IPesoRepository _pesoRepository;

        public PesoUseCase(IPesoRepository pesoRepository)
        {
            _pesoRepository = pesoRepository;
        }

        public IEnumerable<PesoResponseDto> ObterPorPet(decimal idPet)
        {
            ValidarPet(idPet);

            return _pesoRepository
                .ObterPorPet(idPet)
                .Select(p => p.ToResponseDto())
                .ToList();
        }

        public PesoResponseDto? ObterAtual(decimal idPet)
        {
            ValidarPet(idPet);

            var peso = _pesoRepository.ObterAtual(idPet);

            return peso?.ToResponseDto();
        }

        public PesoResponseDto Cadastrar(decimal idPet, PesoRequestDto dto)
        {
            ValidarPet(idPet);
            ValidarDados(dto);

            var peso = dto.ToEntity(idPet);

            peso = _pesoRepository.Cadastrar(peso);

            return peso.ToResponseDto();
        }

        public bool Atualizar(
            decimal idPet,
            decimal pesoId,
            PesoRequestDto dto)
        {
            ValidarPet(idPet);

            var peso = _pesoRepository.ObterPorId(idPet, pesoId);

            if (peso is null)
                return false;

            ValidarDados(dto);

            dto.UpdateEntity(peso);

            _pesoRepository.Atualizar(peso);

            return true;
        }

        public bool Excluir(decimal idPet, decimal pesoId)
        {
            ValidarPet(idPet);

            var peso = _pesoRepository.ObterPorId(idPet, pesoId);

            if (peso is null)
                return false;

            _pesoRepository.Excluir(peso);

            return true;
        }

        private void ValidarPet(decimal idPet)
        {
            if (!_pesoRepository.ExistePet(idPet))
                throw new KeyNotFoundException("Pet não encontrado.");
        }

        private static void ValidarDados(PesoRequestDto dto)
        {
            if (dto.Peso <= 0)
                throw new ArgumentException("O peso deve ser maior que zero.");

            if (dto.DataMedicao.Date > DateTime.Today)
            {
                throw new ArgumentException(
                    "A data da medição não pode ser futura."
                );
            }
        }
    }
}