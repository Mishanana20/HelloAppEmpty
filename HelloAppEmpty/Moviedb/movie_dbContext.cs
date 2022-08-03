using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MovieSqlExpress
{
    public partial class movie_dbContext : DbContext
    {
        public movie_dbContext()
        {
        }

        public movie_dbContext(DbContextOptions<movie_dbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Actor> Actors { get; set; } = null!;
        public virtual DbSet<Genre> Genres { get; set; } = null!;
        public virtual DbSet<Movie> Movies { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=DESKTOP-U4MN4UD\\SQLEXPRESS;Database=movie_db;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Actor>(entity =>
            {
                entity.ToTable("actor");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.FirstName)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("first_name");

                entity.Property(e => e.SecondName)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("second_name");

                entity.HasMany(d => d.Movies)
                    .WithMany(p => p.Actors)
                    .UsingEntity<Dictionary<string, object>>(
                        "ActorMovie",
                        l => l.HasOne<Movie>().WithMany().HasForeignKey("MovieId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__actor_mov__movie__08B54D69"),
                        r => r.HasOne<Actor>().WithMany().HasForeignKey("ActorId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__actor_mov__actor__09A971A2"),
                        j =>
                        {
                            j.HasKey("ActorId", "MovieId");

                            j.ToTable("actor_movie");

                            j.IndexerProperty<int>("ActorId").HasColumnName("actor_id");

                            j.IndexerProperty<int>("MovieId").HasColumnName("movie_id");
                        });
            });

            modelBuilder.Entity<Genre>(entity =>
            {
                entity.ToTable("genre");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.HasMany(d => d.Movies)
                    .WithMany(p => p.Genres)
                    .UsingEntity<Dictionary<string, object>>(
                        "GenreMovie",
                        l => l.HasOne<Movie>().WithMany().HasForeignKey("MovieId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__genre_mov__movie__0C85DE4D"),
                        r => r.HasOne<Genre>().WithMany().HasForeignKey("GenreId").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__genre_mov__genre__0D7A0286"),
                        j =>
                        {
                            j.HasKey("GenreId", "MovieId");

                            j.ToTable("genre_movie");

                            j.IndexerProperty<int>("GenreId").HasColumnName("genre_id");

                            j.IndexerProperty<int>("MovieId").HasColumnName("movie_id");
                        });
            });

            modelBuilder.Entity<Movie>(entity =>
            {
                entity.ToTable("movie");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Date)
                    .HasColumnType("date")
                    .HasColumnName("date");

                entity.Property(e => e.Description)
                    .HasMaxLength(2000)
                    .IsUnicode(false)
                    .HasColumnName("description");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("name");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
