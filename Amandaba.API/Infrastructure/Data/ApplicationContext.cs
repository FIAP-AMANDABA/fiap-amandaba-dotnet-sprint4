using Amandaba.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Amandaba.Infrastructure.Data
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
        }

        public DbSet<UsuarioEntity> Usuarios { get; set; }
        public DbSet<VeterinarioEntity> Veterinarios { get; set; }
        public DbSet<PetVeterinarioEntity> PetVeterinarios { get; set; }
        public DbSet<TutorEntity> Tutores { get; set; }
        public DbSet<EspecieEntity> Especies { get; set; }
        public DbSet<PetEntity> Pets { get; set; }
        public DbSet<PetPesoEntity> Pesos { get; set; }
        public DbSet<VacinaEntity> Vacinas { get; set; }
        public DbSet<PetVacinaEntity> PetVacinas { get; set; }
        public DbSet<PetDoencaEntity> Doencas { get; set; }
        public DbSet<PetAlergiaEntity> Alergias { get; set; }
        public DbSet<PetMedicamentoEntity> Medicamentos { get; set; }
        public DbSet<ConsultaEntity> Consultas { get; set; }
        public DbSet<ExameEntity> Exames { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // =========================
            // USUARIO
            // =========================
            modelBuilder.Entity<UsuarioEntity>(entity =>
            {
                entity.ToTable("TAB_USUARIO");

                entity.HasKey(e => e.IdUsuario);

                entity.Property(e => e.IdUsuario)
                    .HasColumnName("ID_USUARIO")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Nome)
                    .HasColumnName("NM_USUARIO")
                    .HasMaxLength(150)
                    .IsRequired();

                entity.Property(e => e.Cpf)
                    .HasColumnName("NR_CPF_USUARIO")
                    .HasMaxLength(11)
                    .IsRequired();

                entity.Property(e => e.DataNascimento)
                    .HasColumnName("DT_NASCIMENTO_USUARIO");

                entity.Property(e => e.Email)
                    .HasColumnName("DS_EMAIL_USUARIO")
                    .HasMaxLength(150)
                    .IsRequired();

                entity.Property(e => e.Telefone)
                    .HasColumnName("NR_TELEFONE_USUARIO")
                    .HasMaxLength(20);

                entity.Property(e => e.SenhaHash)
                    .HasColumnName("DS_SENHA_HASH_USUARIO")
                    .HasMaxLength(255)
                    .IsRequired();

                entity.Property(e => e.DataCadastro)
                    .HasColumnName("DT_CADASTRO_USUARIO")
                    .IsRequired();

                entity.Property(e => e.Status)
                    .HasColumnName("ST_USUARIO")
                    .HasMaxLength(20)
                    .IsRequired();

                entity.HasIndex(e => e.Cpf)
                    .IsUnique();

                entity.HasIndex(e => e.Email)
                    .IsUnique();
            });

            // =========================
            // TUTOR
            // =========================
            modelBuilder.Entity<TutorEntity>(entity =>
            {
                entity.ToTable("TAB_TUTOR");

                entity.HasKey(e => e.IdTutor);

                entity.Property(e => e.IdTutor)
                    .HasColumnName("ID_TUTOR")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.IdUsuario)
                    .HasColumnName("ID_USUARIO_TUTOR")
                    .IsRequired();

                entity.HasIndex(e => e.IdUsuario)
                    .IsUnique();

                entity.HasOne(e => e.Usuario)
                    .WithOne(e => e.Tutor)
                    .HasForeignKey<TutorEntity>(e => e.IdUsuario);
            });

            // Mapeamentos correspondentes ao SQL final da disciplina de Banco de Dados.
            modelBuilder.Entity<VeterinarioEntity>(entity =>
            {
                entity.ToTable("TAB_VETERINARIO");
                entity.HasKey(e => e.IdVeterinario);
                entity.Property(e => e.IdVeterinario).HasColumnName("ID_VETERINARIO").ValueGeneratedOnAdd();
                entity.Property(e => e.IdUsuario).HasColumnName("ID_USUARIO_VETERINARIO").IsRequired();
                entity.Property(e => e.Crmv).HasColumnName("NR_CRMV_VETERINARIO").HasMaxLength(20).IsRequired();
                entity.Property(e => e.Status).HasColumnName("ST_VETERINARIO").HasMaxLength(20).IsRequired();
                entity.HasIndex(e => e.IdUsuario).IsUnique();
                entity.HasIndex(e => e.Crmv).IsUnique();
            });

            modelBuilder.Entity<PetVeterinarioEntity>(entity =>
            {
                entity.ToTable("TAB_PET_VETERINARIO");
                entity.HasKey(e => e.IdPetVeterinario);
                entity.Property(e => e.IdPetVeterinario).HasColumnName("ID_PET_VETERINARIO").ValueGeneratedOnAdd();
                entity.Property(e => e.IdPet).HasColumnName("ID_PET_COMPARTILHADO").IsRequired();
                entity.Property(e => e.IdVeterinario).HasColumnName("ID_VETERINARIO_ACESSO").IsRequired();
                entity.Property(e => e.DataCompartilhamento).HasColumnName("DT_COMPARTILHAMENTO").IsRequired();
                entity.Property(e => e.Status).HasColumnName("ST_COMPARTILHAMENTO").HasMaxLength(20).IsRequired();
                entity.HasIndex(e => new {e.IdPet, e.IdVeterinario}).IsUnique();
                entity.HasOne<PetEntity>().WithMany().HasForeignKey(e => e.IdPet).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne<VeterinarioEntity>().WithMany().HasForeignKey(e => e.IdVeterinario).OnDelete(DeleteBehavior.Restrict);
            });

            // =========================
            // ESPECIE
            // =========================
            modelBuilder.Entity<EspecieEntity>(entity =>
            {
                entity.ToTable("TAB_ESPECIE");

                entity.HasKey(e => e.IdEspecie);

                entity.Property(e => e.IdEspecie)
                    .HasColumnName("ID_ESPECIE")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Nome)
                    .HasColumnName("NM_ESPECIE")
                    .HasMaxLength(100)
                    .IsRequired();

                entity.HasIndex(e => e.Nome)
                    .IsUnique();
            });

            // =========================
            // PET
            // =========================
            modelBuilder.Entity<PetEntity>(entity =>
            {
                entity.ToTable("TAB_PET");

                entity.HasKey(e => e.IdPet);

                entity.Property(e => e.IdPet)
                    .HasColumnName("ID_PET")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.IdTutor)
                    .HasColumnName("ID_TUTOR_PET")
                    .IsRequired();

                entity.Property(e => e.IdEspecie)
                    .HasColumnName("ID_ESPECIE_PET")
                    .IsRequired();

                entity.Property(e => e.Nome)
                    .HasColumnName("NM_PET")
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(e => e.FotoUrl)
                    .HasColumnName("DS_FOTO_PET")
                    .HasMaxLength(500);

                entity.Property(e => e.Raca)
                    .HasColumnName("NM_RACA_PET")
                    .HasMaxLength(100);

                entity.Property(e => e.Sexo)
                    .HasColumnName("TP_SEXO_PET")
                    .HasMaxLength(10);

                entity.Property(e => e.DataNascimento)
                    .HasColumnName("DT_NASCIMENTO_PET");

                entity.Property(e => e.Cor)
                    .HasColumnName("NM_COR_PET")
                    .HasMaxLength(50);

                entity.Property(e => e.Castrado)
                    .HasColumnName("FL_CASTRADO_PET")
                    .HasConversion(
                        v => v ? "S" : "N",
                        v => v == "S"
                    )
                    .HasMaxLength(1)
                    .IsRequired();

                entity.Property(e => e.Microchip)
                    .HasColumnName("NR_MICROCHIP_PET")
                    .HasMaxLength(50);

                entity.Property(e => e.DataCadastro)
                    .HasColumnName("DT_CADASTRO_PET")
                    .IsRequired();

                entity.Property(e => e.Status)
                    .HasColumnName("ST_PET")
                    .HasMaxLength(20)
                    .IsRequired();

                entity.HasOne(e => e.Tutor)
                    .WithMany(e => e.Pets)
                    .HasForeignKey(e => e.IdTutor);

                entity.HasOne(e => e.Especie)
                    .WithMany(e => e.Pets)
                    .HasForeignKey(e => e.IdEspecie);
            });

            // =========================
            // PET PESO
            // =========================
            modelBuilder.Entity<PetPesoEntity>(entity =>
            {
                entity.ToTable("TAB_PET_PESO");

                entity.HasKey(e => e.IdHistoricoPeso);

                entity.Property(e => e.IdHistoricoPeso)
                    .HasColumnName("ID_HISTORICO_PESO")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.IdPet)
                    .HasColumnName("ID_PET_PESO")
                    .IsRequired();

                entity.Property(e => e.Peso)
                    .HasColumnName("VL_PESO_PET")
                    .HasPrecision(6, 2)
                    .IsRequired();

                entity.Property(e => e.DataMedicao)
                    .HasColumnName("DT_MEDICAO_PESO")
                    .IsRequired();

                entity.Property(e => e.DataCadastro)
                    .HasColumnName("DT_CADASTRO_PESO")
                    .IsRequired();

                entity.HasOne(e => e.Pet)
                    .WithMany(e => e.Pesos)
                    .HasForeignKey(e => e.IdPet);
            });

            // =========================
            // VACINA
            // =========================
            modelBuilder.Entity<VacinaEntity>(entity =>
            {
                entity.ToTable("TAB_VACINA");

                entity.HasKey(e => e.IdVacina);

                entity.Property(e => e.IdVacina)
                    .HasColumnName("ID_VACINA")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Nome)
                    .HasColumnName("NM_VACINA")
                    .HasMaxLength(150)
                    .IsRequired();

                entity.Property(e => e.Tipo)
                    .HasColumnName("TP_VACINA")
                    .HasMaxLength(100);

                entity.Property(e => e.Fabricante)
                    .HasColumnName("NM_FABRICANTE_VACINA")
                    .HasMaxLength(150);

                entity.Property(e => e.Descricao)
                    .HasColumnName("DS_VACINA")
                    .HasMaxLength(1000);
            });

            // =========================
            // PET VACINA
            // =========================
            modelBuilder.Entity<PetVacinaEntity>(entity =>
            {
                entity.ToTable("TAB_PET_VACINA");

                entity.HasKey(e => e.IdAplicacaoVacina);

                entity.Property(e => e.IdAplicacaoVacina)
                    .HasColumnName("ID_APLICACAO_VACINA")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.IdPet)
                    .HasColumnName("ID_PET_VACINA")
                    .IsRequired();

                entity.Property(e => e.IdVacina)
                    .HasColumnName("ID_VACINA_APLICADA")
                    .IsRequired();

                entity.Property(e => e.DataAplicacao)
                    .HasColumnName("DT_APLICACAO_VACINA")
                    .IsRequired();

                entity.Property(e => e.NumeroDose)
                    .HasColumnName("NR_DOSE_VACINA");

                entity.Property(e => e.NumeroLote)
                    .HasColumnName("NR_LOTE_VACINA")
                    .HasMaxLength(50);

                entity.Property(e => e.ProximaDose)
                    .HasColumnName("DT_PROXIMA_DOSE_VACINA");

                entity.Property(e => e.Clinica)
                    .HasColumnName("NM_CLINICA_VACINA")
                    .HasMaxLength(150);

                entity.Property(e => e.Observacao)
                    .HasColumnName("DS_OBSERVACAO_VACINA")
                    .HasMaxLength(1000);

                entity.Property(e => e.Comprovante)
                    .HasColumnName("DS_COMPROVANTE_VACINA")
                    .HasMaxLength(500);

                entity.Property(e => e.DataCadastro)
                    .HasColumnName("DT_CADASTRO_APLICACAO")
                    .IsRequired();

                entity.HasOne(e => e.Pet)
                    .WithMany(e => e.Vacinas)
                    .HasForeignKey(e => e.IdPet);

                entity.HasOne(e => e.Vacina)
                    .WithMany(e => e.Aplicacoes)
                    .HasForeignKey(e => e.IdVacina);
            });

            // =========================
            // PET DOENCA
            // =========================
            modelBuilder.Entity<PetDoencaEntity>(entity =>
            {
                entity.ToTable("TAB_PET_DOENCA");

                entity.HasKey(e => e.IdRegistroDoenca);

                entity.Property(e => e.IdRegistroDoenca)
                    .HasColumnName("ID_REGISTRO_DOENCA")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.IdPet)
                    .HasColumnName("ID_PET_DOENCA")
                    .IsRequired();

                entity.Property(e => e.Nome)
                    .HasColumnName("NM_DOENCA_PET")
                    .HasMaxLength(250)
                    .IsRequired();

                entity.Property(e => e.DataDiagnostico)
                    .HasColumnName("DT_DIAGNOSTICO_DOENCA");

                entity.Property(e => e.Status)
                    .HasColumnName("ST_DOENCA_PET")
                    .HasMaxLength(20)
                    .IsRequired();

                entity.Property(e => e.Tratamento)
                    .HasColumnName("DS_TRATAMENTO_DOENCA")
                    .HasMaxLength(1000);

                entity.Property(e => e.Observacao)
                    .HasColumnName("DS_OBSERVACAO_DOENCA")
                    .HasMaxLength(1000);

                entity.Property(e => e.DataCadastro)
                    .HasColumnName("DT_CADASTRO_DOENCA")
                    .IsRequired();

                entity.HasOne(e => e.Pet)
                    .WithMany(e => e.Doencas)
                    .HasForeignKey(e => e.IdPet);
            });

            // =========================
            // PET ALERGIA
            // =========================
            modelBuilder.Entity<PetAlergiaEntity>(entity =>
            {
                entity.ToTable("TAB_PET_ALERGIA");

                entity.HasKey(e => e.IdRegistroAlergia);

                entity.Property(e => e.IdRegistroAlergia)
                    .HasColumnName("ID_REGISTRO_ALERGIA")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.IdPet)
                    .HasColumnName("ID_PET_ALERGIA")
                    .IsRequired();

                entity.Property(e => e.Nome)
                    .HasColumnName("NM_ALERGIA_PET")
                    .HasMaxLength(250)
                    .IsRequired();

                entity.Property(e => e.Tipo)
                    .HasColumnName("TP_ALERGIA_PET")
                    .HasMaxLength(30);

                entity.Property(e => e.DataIdentificacao)
                    .HasColumnName("DT_IDENTIFICACAO_ALERGIA");

                entity.Property(e => e.Reacao)
                    .HasColumnName("DS_REACAO_ALERGIA")
                    .HasMaxLength(500);

                entity.Property(e => e.Gravidade)
                    .HasColumnName("TP_GRAVIDADE_ALERGIA")
                    .HasMaxLength(20);

                entity.Property(e => e.Observacao)
                    .HasColumnName("DS_OBSERVACAO_ALERGIA")
                    .HasMaxLength(1000);

                entity.Property(e => e.DataCadastro)
                    .HasColumnName("DT_CADASTRO_ALERGIA")
                    .IsRequired();

                entity.HasOne(e => e.Pet)
                    .WithMany(e => e.Alergias)
                    .HasForeignKey(e => e.IdPet);
            });

            // =========================
            // PET MEDICAMENTO
            // =========================
            modelBuilder.Entity<PetMedicamentoEntity>(entity =>
            {
                entity.ToTable("TAB_PET_MEDICAMENTO");

                entity.HasKey(e => e.IdRegistroMedicamento);

                entity.Property(e => e.IdRegistroMedicamento)
                    .HasColumnName("ID_REGISTRO_MEDICAMENTO")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.IdPet)
                    .HasColumnName("ID_PET_MEDICAMENTO")
                    .IsRequired();

                entity.Property(e => e.Nome)
                    .HasColumnName("NM_MEDICAMENTO_PET")
                    .HasMaxLength(250)
                    .IsRequired();

                entity.Property(e => e.Motivo)
                    .HasColumnName("DS_MOTIVO_MEDICAMENTO")
                    .HasMaxLength(500);

                entity.Property(e => e.Dosagem)
                    .HasColumnName("VL_DOSAGEM_MEDICAMENTO")
                    .HasPrecision(10, 2);

                entity.Property(e => e.Unidade)
                    .HasColumnName("TP_UNIDADE_MEDICAMENTO")
                    .HasMaxLength(30);

                entity.Property(e => e.Quantidade)
                    .HasColumnName("QT_MEDICAMENTO")
                    .HasMaxLength(50);

                entity.Property(e => e.Frequencia)
                    .HasColumnName("DS_FREQUENCIA_MEDICAMENTO")
                    .HasMaxLength(100);

                entity.Property(e => e.Administracao)
                    .HasColumnName("DS_ADMINISTRACAO_MEDICAMENTO")
                    .HasMaxLength(100);

                entity.Property(e => e.Horario)
                    .HasColumnName("DS_HORARIO_MEDICAMENTO")
                    .HasMaxLength(500);

                entity.Property(e => e.DataInicio)
                    .HasColumnName("DT_INICIO_MEDICAMENTO")
                    .IsRequired();

                entity.Property(e => e.DataTermino)
                    .HasColumnName("DT_TERMINO_MEDICAMENTO");

                entity.Property(e => e.Status)
                    .HasColumnName("ST_MEDICAMENTO_PET")
                    .HasMaxLength(20)
                    .IsRequired();

                entity.Property(e => e.Prescricao)
                    .HasColumnName("DS_PRESCRICAO_MEDICAMENTO")
                    .HasMaxLength(500);

                entity.Property(e => e.Observacao)
                    .HasColumnName("DS_OBSERVACAO_MEDICAMENTO")
                    .HasMaxLength(1000);

                entity.Property(e => e.DataCadastro)
                    .HasColumnName("DT_CADASTRO_MEDICAMENTO")
                    .IsRequired();

                entity.HasOne(e => e.Pet)
                    .WithMany(e => e.Medicamentos)
                    .HasForeignKey(e => e.IdPet);
            });

            // =========================
            // CONSULTA
            // =========================
            modelBuilder.Entity<ConsultaEntity>(entity =>
            {
                entity.ToTable("TAB_CONSULTA");

                entity.HasKey(e => e.IdConsulta);

                entity.Property(e => e.IdConsulta)
                    .HasColumnName("ID_CONSULTA")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.IdPet)
                    .HasColumnName("ID_PET_CONSULTA")
                    .IsRequired();

                entity.Property(e => e.DataConsulta)
                    .HasColumnName("DT_CONSULTA_PET")
                    .IsRequired();

                entity.Property(e => e.Horario)
                    .HasColumnName("DS_HORARIO_CONSULTA")
                    .HasMaxLength(5);

                entity.Property(e => e.Veterinario)
                    .HasColumnName("NM_VETERINARIO_CONSULTA")
                    .HasMaxLength(150);

                entity.Property(e => e.Clinica)
                    .HasColumnName("NM_CLINICA_CONSULTA")
                    .HasMaxLength(150);

                entity.Property(e => e.Motivo)
                    .HasColumnName("DS_MOTIVO_CONSULTA")
                    .HasMaxLength(500);

                entity.Property(e => e.Sintomas)
                    .HasColumnName("DS_SINTOMAS_CONSULTA")
                    .HasMaxLength(1000);

                entity.Property(e => e.Peso)
                    .HasColumnName("VL_PESO_CONSULTA")
                    .HasPrecision(6, 2);

                entity.Property(e => e.Diagnostico)
                    .HasColumnName("DS_DIAGNOSTICO_CONSULTA")
                    .HasMaxLength(2000);

                entity.Property(e => e.Tratamento)
                    .HasColumnName("DS_TRATAMENTO_CONSULTA")
                    .HasMaxLength(2000);

                entity.Property(e => e.Observacao)
                    .HasColumnName("DS_OBSERVACAO_CONSULTA")
                    .HasMaxLength(2000);

                entity.Property(e => e.Retorno)
                    .HasColumnName("FL_RETORNO_CONSULTA")
                    .HasConversion(
                        v => v ? "S" : "N",
                        v => v == "S"
                    )
                    .HasMaxLength(1)
                    .IsRequired();

                entity.Property(e => e.DataRetorno)
                    .HasColumnName("DT_RETORNO_CONSULTA");

                entity.Property(e => e.Status)
                    .HasColumnName("ST_CONSULTA")
                    .HasMaxLength(20)
                    .IsRequired();

                entity.Property(e => e.DataCadastro)
                    .HasColumnName("DT_CADASTRO_CONSULTA")
                    .IsRequired();

                entity.HasOne(e => e.Pet)
                    .WithMany(e => e.Consultas)
                    .HasForeignKey(e => e.IdPet);
            });

            // =========================
            // EXAME
            // =========================
            modelBuilder.Entity<ExameEntity>(entity =>
            {
                entity.ToTable("TAB_EXAME");

                entity.HasKey(e => e.IdExame);

                entity.Property(e => e.IdExame)
                    .HasColumnName("ID_EXAME")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.IdPet)
                    .HasColumnName("ID_PET_EXAME")
                    .IsRequired();

                entity.Property(e => e.Nome)
                    .HasColumnName("NM_EXAME_PET")
                    .HasMaxLength(150)
                    .IsRequired();

                entity.Property(e => e.DataSolicitacao)
                    .HasColumnName("DT_SOLICITACAO_EXAME");

                entity.Property(e => e.DataRealizacao)
                    .HasColumnName("DT_REALIZACAO_EXAME");

                entity.Property(e => e.Veterinario)
                    .HasColumnName("NM_VETERINARIO_EXAME")
                    .HasMaxLength(150);

                entity.Property(e => e.Clinica)
                    .HasColumnName("NM_CLINICA_EXAME")
                    .HasMaxLength(150);

                entity.Property(e => e.Motivo)
                    .HasColumnName("DS_MOTIVO_EXAME")
                    .HasMaxLength(500);

                entity.Property(e => e.Resultado)
                    .HasColumnName("DS_RESULTADO_EXAME")
                    .HasMaxLength(4000);

                entity.Property(e => e.Observacao)
                    .HasColumnName("DS_OBSERVACAO_EXAME")
                    .HasMaxLength(2000);

                entity.Property(e => e.Arquivo)
                    .HasColumnName("DS_ARQUIVO_EXAME")
                    .HasMaxLength(500);

                entity.Property(e => e.Status)
                    .HasColumnName("ST_EXAME")
                    .HasMaxLength(30)
                    .IsRequired();

                entity.Property(e => e.DataCadastro)
                    .HasColumnName("DT_CADASTRO_EXAME")
                    .IsRequired();

                entity.HasOne(e => e.Pet)
                    .WithMany(e => e.Exames)
                    .HasForeignKey(e => e.IdPet);
            });
        }
    }
}