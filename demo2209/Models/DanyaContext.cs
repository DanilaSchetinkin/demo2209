using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace demo2209.Models;

public partial class DanyaContext : DbContext
{
    public DanyaContext()
    {
    }

    public DanyaContext(DbContextOptions<DanyaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<LoginHistory> LoginHistories { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<Session> Sessions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=lorksipt.ru;Port=5432;Database=danya;Username=danya;Password=danya");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("clients_pk");

            entity.ToTable("clients");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Address)
                .HasColumnType("character varying")
                .HasColumnName("address");
            entity.Property(e => e.Birthday)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("birthday");
            entity.Property(e => e.CodeCliend).HasColumnName("code_cliend");
            entity.Property(e => e.Email)
                .HasColumnType("character varying")
                .HasColumnName("email");
            entity.Property(e => e.Fio)
                .HasColumnType("character varying")
                .HasColumnName("fio");
            entity.Property(e => e.Passport)
                .HasColumnType("character varying")
                .HasColumnName("passport");
            entity.Property(e => e.Password)
                .HasColumnType("character varying")
                .HasColumnName("password");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("employee_pk");

            entity.ToTable("employee");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.CodeEmployee).HasColumnName("code_employee");
            entity.Property(e => e.Fio)
                .HasColumnType("character varying")
                .HasColumnName("fio");
            entity.Property(e => e.Imagepath)
                .HasColumnType("character varying")
                .HasColumnName("imagepath");
            entity.Property(e => e.LastEnter)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("last_enter");
            entity.Property(e => e.Login)
                .HasColumnType("character varying")
                .HasColumnName("login");
            entity.Property(e => e.Password)
                .HasColumnType("character varying")
                .HasColumnName("password");
            entity.Property(e => e.Position).HasColumnName("position");
            entity.Property(e => e.TypeEnter)
                .HasColumnType("character varying")
                .HasColumnName("type_enter");

            entity.HasOne(d => d.PositionNavigation).WithMany(p => p.Employees)
                .HasForeignKey(d => d.Position)
                .HasConstraintName("employee_role_fk");
        });

        modelBuilder.Entity<LoginHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("login_history_pk");

            entity.ToTable("login_history");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.EmployeeId).HasColumnName("employee_id");
            entity.Property(e => e.LoginTime)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("login_time");
            entity.Property(e => e.LoginType)
                .HasColumnType("character varying")
                .HasColumnName("login_type");

            entity.HasOne(d => d.Employee).WithMany(p => p.LoginHistories)
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("login_history_employee_fk");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("order_pk");

            entity.ToTable("order");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.CodeClient).HasColumnName("code_client");
            entity.Property(e => e.CodeOrder)
                .HasColumnType("character varying")
                .HasColumnName("code_order");
            entity.Property(e => e.DateClose)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("date_close");
            entity.Property(e => e.DateCreate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("date_create");
            entity.Property(e => e.EmployeeId).HasColumnName("employee_id");
            entity.Property(e => e.Status)
                .HasColumnType("character varying")
                .HasColumnName("status");
            entity.Property(e => e.TimeOrder)
                .HasColumnType("character varying")
                .HasColumnName("time_order");
            entity.Property(e => e.TimeRental)
                .HasColumnType("character varying")
                .HasColumnName("time_rental");

            entity.HasOne(d => d.CodeClientNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CodeClient)
                .HasConstraintName("order_clients_fk");

            entity.HasOne(d => d.Employee).WithMany(p => p.Orders)
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("order_employee_fk");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("role_pk");

            entity.ToTable("role");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.RoleName)
                .HasColumnType("character varying")
                .HasColumnName("role_name");
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("service_pk");

            entity.ToTable("service");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Cost)
                .HasColumnType("character varying")
                .HasColumnName("cost");
            entity.Property(e => e.ServiceCode)
                .HasColumnType("character varying")
                .HasColumnName("service_code");
            entity.Property(e => e.ServiceId).HasColumnName("service_id");
            entity.Property(e => e.ServiceName)
                .HasColumnType("character varying")
                .HasColumnName("service_name");

            entity.HasMany(d => d.Idorders).WithMany(p => p.Idservices)
                .UsingEntity<Dictionary<string, object>>(
                    "Orderservice",
                    r => r.HasOne<Order>().WithMany()
                        .HasForeignKey("Idorder")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("orderservice_order_fk"),
                    l => l.HasOne<Service>().WithMany()
                        .HasForeignKey("Idservice")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("orderservice_service_fk"),
                    j =>
                    {
                        j.HasKey("Idservice", "Idorder").HasName("orderservice_pk");
                        j.ToTable("orderservice");
                        j.IndexerProperty<int>("Idservice").HasColumnName("idservice");
                        j.IndexerProperty<int>("Idorder").HasColumnName("idorder");
                    });
        });

        modelBuilder.Entity<Session>(entity =>
        {
            entity.HasKey(e => e.SessionId).HasName("session_pk");

            entity.ToTable("Session");

            entity.Property(e => e.SessionId)
                .ValueGeneratedNever()
                .HasColumnName("session_id");
            entity.Property(e => e.EmployeeId).HasColumnName("employee_id");
            entity.Property(e => e.EndTime)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("end_time");
            entity.Property(e => e.StartTime)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("start_time");

            entity.HasOne(d => d.Employee).WithMany(p => p.Sessions)
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("session_employee_fk");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
