using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace SeguradosAPI
{
    public partial class db_a62123_italabsContext : DbContext
    {
        public db_a62123_italabsContext()
        {
        }

        public db_a62123_italabsContext(DbContextOptions<db_a62123_italabsContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Pergunta> Pergunta { get; set; }
        public virtual DbSet<Tematica> Tematica { get; set; }
        public virtual DbSet<Usuario> Usuario { get; set; }
        public virtual DbSet<UsuarioHasPergunta> UsuarioHasPergunta { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Pergunta>(entity =>
            {
                entity.HasKey(e => e.IdPergunta);

                entity.ToTable("pergunta", "db_a62123_italabs");

                entity.HasIndex(e => e.IdPergunta)
                    .HasName("idPergunta_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.TematicaIdTematica)
                    .HasName("fk_Pergunta_Tematica1_idx");

                entity.Property(e => e.IdPergunta)
                    .HasColumnName("idPergunta")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Alternativa1)
                    .IsRequired()
                    .HasColumnName("alternativa1")
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Alternativa2)
                    .IsRequired()
                    .HasColumnName("alternativa2")
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Alternativa3)
                    .HasColumnName("alternativa3")
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Alternativa4)
                    .HasColumnName("alternativa4")
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Dificuldade)
                    .IsRequired()
                    .HasColumnName("dificuldade")
                    .HasColumnType("enum('FACIL','MEDIA','DIFICIL')");

                entity.Property(e => e.OpcaoCorreta)
                    .HasColumnName("opcaoCorreta")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Pontuacao)
                    .IsRequired()
                    .HasColumnName("pontuacao")
                    .HasColumnType("enum('5','10','15')");

                entity.Property(e => e.Questao)
                    .IsRequired()
                    .HasColumnName("questao")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.TematicaIdTematica)
                    .HasColumnName("Tematica_idTematica")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Tempo)
                    .IsRequired()
                    .HasColumnName("tempo")
                    .HasColumnType("enum('15','25','35')");

                entity.HasOne(d => d.TematicaIdTematicaNavigation)
                    .WithMany(p => p.Pergunta)
                    .HasForeignKey(d => d.TematicaIdTematica)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Pergunta_Tematica1");
            });

            modelBuilder.Entity<Tematica>(entity =>
            {
                entity.HasKey(e => e.IdTematica);

                entity.ToTable("tematica", "db_a62123_italabs");

                entity.Property(e => e.IdTematica)
                    .HasColumnName("idTematica")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Descricao)
                    .HasColumnName("descricao")
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.Titulo)
                    .IsRequired()
                    .HasColumnName("titulo")
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.IdUsuario);

                entity.ToTable("usuario", "db_a62123_italabs");

                entity.HasIndex(e => e.IdUsuario)
                    .HasName("idUsuario_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.IdUsuario)
                    .HasColumnName("idUsuario")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasColumnName("nome")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Perfil)
                    .IsRequired()
                    .HasColumnName("perfil")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Senha)
                    .IsRequired()
                    .HasColumnName("senha")
                    .HasMaxLength(16)
                    .IsUnicode(false);

                entity.Property(e => e.Usuario1)
                    .IsRequired()
                    .HasColumnName("usuario")
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<UsuarioHasPergunta>(entity =>
            {
                entity.HasKey(e => new { e.IdUsuario, e.IdPergunta });

                entity.ToTable("usuario_has_pergunta", "db_a62123_italabs");

                entity.HasIndex(e => e.IdPergunta)
                    .HasName("fk_Usuario_has_Pergunta_Pergunta1_idx");

                entity.HasIndex(e => e.IdUsuario)
                    .HasName("fk_Usuario_has_Pergunta_Usuario_idx");

                entity.Property(e => e.IdUsuario)
                    .HasColumnName("idUsuario")
                    .HasColumnType("int(11)");

                entity.Property(e => e.IdPergunta)
                    .HasColumnName("idPergunta")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Acertou)
                    .HasColumnName("acertou")
                    .HasColumnType("tinyint(4)");

                entity.HasOne(d => d.IdPerguntaNavigation)
                    .WithMany(p => p.UsuarioHasPergunta)
                    .HasForeignKey(d => d.IdPergunta)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Usuario_has_Pergunta_Pergunta1");

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.UsuarioHasPergunta)
                    .HasForeignKey(d => d.IdUsuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Usuario_has_Pergunta_Usuario");
            });
        }
    }
}
