using System.Text.Json;
using InnerPeace.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace InnerPeace;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{  
    public DbSet<Doctor> Doctors { get; set; } 
    public DbSet<Education> Educations { get; set; } 
    public DbSet<Country> Countries { get; set; } 
    public DbSet<Language> Languages { get; set; } 
    public DbSet<Specialization> Specializations { get; set; }
    public DbSet<User> Users { get; set; } 
    public DbSet<MoodTracking> MoodsTracking{ get; set; } 
    public DbSet<Rating> Ratings { get; set; } 
    public DbSet<Session> Sessions { get; set; } 
    public DbSet<SessionDuration> SessionDurations { get; set; } 
    
    public DbSet<EducationDegree> EducationDegree { get; set; } 
    
    private readonly JsonSerializerOptions _jsonOptions = new(JsonSerializerDefaults.Web);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        modelBuilder.Entity<Doctor>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).HasMaxLength(50).IsRequired();
            entity.Property(e => e.ProfessionTitle).HasMaxLength(50).IsRequired();
            entity.Property(e => e.Email).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Phone).HasMaxLength(14).IsRequired();
            entity.Property(e => e.Password).IsRequired();
            entity.Property(e => e.ImagePath).IsRequired();

            entity.Property(e => e.SessionSettings)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, _jsonOptions),
                    v => JsonSerializer.Deserialize<DoctorSessionSettings>(v, _jsonOptions) ??
                         new DoctorSessionSettings()
                )
                .HasColumnType("jsonb");

            entity.Property(e => e.SessionSettings)
                .Metadata.SetValueComparer(new ValueComparer<DoctorSessionSettings>(
                    (c1, c2) => JsonSerializer.Serialize(c1, _jsonOptions) == JsonSerializer.Serialize(c2, _jsonOptions),
                    c => JsonSerializer.Serialize(c, _jsonOptions).GetHashCode(),
                    c => JsonSerializer.Deserialize<DoctorSessionSettings>(
                        JsonSerializer.Serialize(c, _jsonOptions), _jsonOptions
                    ) ?? new DoctorSessionSettings()
                ));

            entity.HasIndex(e => e.Email).IsUnique();
            entity.HasIndex(e => e.Phone).IsUnique();
            
            entity.HasMany(e => e.Educations)
                .WithOne(e => e.Doctor)
                .HasForeignKey(e => e.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);
            
            entity.HasMany(e => e.DoctorLanguages)
                .WithOne(e => e.Doctor)
                .HasForeignKey(e => e.DoctorId)
                .OnDelete(DeleteBehavior.Cascade);
            
            entity.HasMany(e => e.DoctorCountries)
                .WithOne(e => e.Doctor)
                .HasForeignKey(e => e.DoctorId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        
        modelBuilder.Entity<Education>(builder =>
        {
            builder.Property(e => e.University)
                .HasMaxLength(50)
                .IsRequired();
        });

        modelBuilder.Entity<EducationDegree>(builder =>
        {
            builder.Property(e => e.Degree)
                .HasMaxLength(50)
                .IsRequired();
            
            builder.HasMany(e => e.Educations)
                .WithOne(e=>e.EducationDegree)
                .HasForeignKey(e=>e.EducationDegreeId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Country>(builder =>
        {
            builder.Property(c => c.Name)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(c => c.ImagePath)
                .IsRequired();
        });

        modelBuilder.Entity<Language>(builder =>
        {
            builder.Property(l => l.Name)
                .HasMaxLength(50)
                .IsRequired();
        });

        modelBuilder.Entity<DoctorCountry>(builder =>
        {
            builder.HasKey(dc => new { dc.DoctorId, dc.CountryId });

            builder.HasOne(dc => dc.Doctor)
                .WithMany(d => d.DoctorCountries)
                .HasForeignKey(dc => dc.DoctorId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(dc => dc.Country)
                .WithMany(c => c.DoctorsCountry)
                .HasForeignKey(dc => dc.CountryId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<DoctorLanguage>(builder =>
        {
            builder.HasKey(dl => new { dl.DoctorId, dl.LanguageId });

            builder.HasOne(dl => dl.Doctor)
                .WithMany(d => d.DoctorLanguages)
                .HasForeignKey(dl => dl.DoctorId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(dl => dl.Language)
                .WithMany(l => l.DoctorsLanguage)
                .HasForeignKey(dl => dl.LanguageId)
                .OnDelete(DeleteBehavior.Restrict);
        });
        
        modelBuilder.Entity<Specialization>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired();
        });
        
        modelBuilder.Entity<DoctorSpecialization>(entity =>
        {
            entity.HasKey(e => new { e.DoctorId, e.SpecializationId });
            
            entity.HasOne(e => e.Doctor)
                .WithMany(e => e.DoctorSpecializations)
                .HasForeignKey(e => e.DoctorId)
                .OnDelete(DeleteBehavior.Cascade);
            
            entity.HasOne(e => e.Specialization)
                .WithMany(e => e.DoctorsSpecialization)
                .HasForeignKey(e => e.SpecializationId)
                .OnDelete(DeleteBehavior.Restrict);
        });
        
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).HasMaxLength(50).IsRequired();
            entity.Property(e => e.Email).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Password).IsRequired();
            entity.Property(e => e.Phone).HasMaxLength(14).IsRequired();
            entity.Property(e => e.Birthday).IsRequired();

            entity.HasIndex(e => e.Email).IsUnique();
            entity.HasIndex(e => e.Phone).IsUnique();
        });
        
        modelBuilder.Entity<MoodTracking>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Mood).IsRequired();
            
            entity.Property(e => e.UserId).IsRequired();

            entity.HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Cascade);
        });
        
        modelBuilder.Entity<Rating>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Communication).IsRequired();
            entity.Property(e => e.UnderstandingOfTheSituation).IsRequired();
            entity.Property(e => e.ProvidingEffectiveSolution).IsRequired();
            entity.Property(e => e.CommitmentToStartAndEndTimes).IsRequired();
            entity.Property(e => e.DoctorId).IsRequired();
            entity.Property(e => e.UserId).IsRequired();
            
            entity.HasOne(e => e.Doctor)
                .WithMany(e => e.Ratings)
                .HasForeignKey(e => e.DoctorId)
                .OnDelete(DeleteBehavior.Cascade);
                
            entity.HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Session>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Price)
                .IsRequired();
                
            entity.Property(e => e.DateTime)
                .IsRequired();
                
            entity.Property(e => e.Prescription)
                .HasMaxLength(1000);
                
            entity.Property(e => e.Note)
                .HasMaxLength(1000);
                
            entity.HasOne(e => e.Duration)
                .WithMany(e => e.Sessions)
                .HasForeignKey(e => e.DurationId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Rating)
                .WithOne(e => e.Session)
                .HasForeignKey<Rating>(e => e.SessionId)
                .OnDelete(DeleteBehavior.SetNull);
                
            entity.HasOne(e => e.User)
                .WithMany(e => e.Sessions)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
                
            entity.HasOne(e => e.Doctor)
                .WithMany(e => e.Sessions)
                .HasForeignKey(e => e.DoctorId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<SessionDuration>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Duration)
                .IsRequired();
        });

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.GetProperties())
            {
                if (property.ClrType == typeof(DateTime))
                {
                    property.SetColumnType("timestamp"); 
                }
            }
        }
        base.OnModelCreating(modelBuilder);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);
        
        foreach (var entry in entries)
        {
            var properties = entry.Properties;

            var propertyEntries = properties.ToList();
            if (entry.State == EntityState.Added)
            {
                if (propertyEntries.Any(p => p.Metadata.Name == "CreatedAt"))
                {
                    entry.Property("CreatedAt").CurrentValue = DateTime.Now;
                }
            }

            if (propertyEntries.Any(p => p.Metadata.Name == "UpdatedAt"))
            {
                entry.Property("UpdatedAt").CurrentValue = DateTime.Now;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}