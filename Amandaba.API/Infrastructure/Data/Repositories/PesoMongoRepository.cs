using Amandaba.API.Application.Dtos.Pesos;
using Amandaba.API.Application.Interfaces;
using Amandaba.API.Domain.Entities;
using Amandaba.API.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Amandaba.API.Infrastructure.Data.Repositories;

// Acesso ao MongoDB pelo DbContext/DbSet, como em PedidoRepository da aula.
public class PesoMongoRepository : IPesoMongoRepository
{
    private readonly MongoApplicationContext _context;

    public PesoMongoRepository(MongoApplicationContext context)
    {
        _context = context;
    }

    public PesoHistoricoDto? ObterPorPet(decimal petId)
    {
        var documento = _context.HistoricosPeso
            .AsNoTracking()
            .FirstOrDefault(p => p.PetId == petId);

        if (documento is null) return null;

        return new PesoHistoricoDto
        {
            Id = documento.Id,
            PetId = documento.PetId,
            AtualizadoEm = documento.AtualizadoEm,
            Historico = documento.Historico.Select(p => new MedicaoPesoDto
            {
                PesoId = p.PesoId,
                Peso = p.Peso,
                DataMedicao = p.DataMedicao
            }).ToList()
        };
    }

    public void Salvar(PesoHistoricoDto documento)
    {
        // Um único documento por pet: a sincronização inclui as medições atuais do Oracle.
        var existente = _context.HistoricosPeso.FirstOrDefault(p => p.Id == documento.Id);
        if (existente is null)
        {
            _context.HistoricosPeso.Add(new PesoHistoricoDocumento
            {
                Id = documento.Id,
                PetId = documento.PetId,
                AtualizadoEm = documento.AtualizadoEm,
                Historico = ConverterMedicoes(documento.Historico)
            });
        }
        else
        {
            existente.PetId = documento.PetId;
            existente.AtualizadoEm = documento.AtualizadoEm;
            existente.Historico.Clear();
            existente.Historico.AddRange(ConverterMedicoes(documento.Historico));
        }

        _context.SaveChanges();
    }

    private static List<MedicaoPesoDocumento> ConverterMedicoes(IEnumerable<MedicaoPesoDto> medicoes) =>
        medicoes.Select(p => new MedicaoPesoDocumento
        {
            PesoId = p.PesoId,
            Peso = p.Peso,
            DataMedicao = p.DataMedicao
        }).ToList();
}
