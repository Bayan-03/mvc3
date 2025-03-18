using System;
using System.Collections.Generic;
using EventMVC3.Models;
using Microsoft.EntityFrameworkCore;

namespace EventMVC3.Data;

public partial class EventContext : DbContext
{
    public EventContext()
    {
    }

    public EventContext(DbContextOptions<EventContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CardsPay> CardsPays { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<Event> Events { get; set; }

    public virtual DbSet<EventCategory> EventCategories { get; set; }

    public virtual DbSet<Reservation> Reservations { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("server=LAPTOP-4OQPSNRM\\SQLEXPRESS;database=event;trusted_connection=true;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CardsPay>(entity =>
        {
            entity.ToTable("cardsPay");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Cvv).HasColumnName("cvv");
            entity.Property(e => e.ExpiryDate)
                .HasColumnType("date")
                .HasColumnName("expiryDate");
            entity.Property(e => e.Name)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.NumberOfCard).HasColumnName("numberOfCard");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.User).WithMany(p => p.CardsPays)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_cardsPay_User");
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Comment1).HasColumnName("comment");
            entity.Property(e => e.EventId).HasColumnName("eventId");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.Event).WithMany(p => p.Comments)
                .HasForeignKey(d => d.EventId)
                .HasConstraintName("FK_Comments_events");

            entity.HasOne(d => d.User).WithMany(p => p.Comments)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Comments_User");
        });

        modelBuilder.Entity<Event>(entity =>
        {
            entity.ToTable("events");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Category).HasColumnName("category");
            entity.Property(e => e.City)
                .HasMaxLength(50)
                .HasColumnName("city");
            entity.Property(e => e.Discription).HasColumnName("discription");
            entity.Property(e => e.FineshTime).HasColumnName("fineshTime");
            entity.Property(e => e.FinishDate)
                .HasColumnType("date")
                .HasColumnName("finishDate");
            entity.Property(e => e.Image).HasColumnName("image");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.PlaceName).HasColumnName("placeName");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.StartDate)
                .HasColumnType("date")
                .HasColumnName("startDate");
            entity.Property(e => e.StartTime).HasColumnName("startTime");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.CategoryNavigation).WithMany(p => p.Events)
                .HasForeignKey(d => d.Category)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_events_eventCategory");

            entity.HasOne(d => d.User).WithMany(p => p.Events)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_events_User");
        });

        modelBuilder.Entity<EventCategory>(entity =>
        {
            entity.ToTable("eventCategory");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(50)
                .HasColumnName("categoryName");
        });

        modelBuilder.Entity<Reservation>(entity =>
        {
            entity.HasKey(e => new { e.IdNumber, e.BookingDate });

            entity.Property(e => e.IdNumber)
                .ValueGeneratedOnAdd()
                .HasColumnName("idNumber");
            entity.Property(e => e.BookingDate)
                .HasColumnType("date")
                .HasColumnName("bookingDate");
            entity.Property(e => e.Email)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.EventId).HasColumnName("eventId");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("phoneNumber");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.Event).WithMany(p => p.Reservations)
                .HasForeignKey(d => d.EventId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Reservations_events");

            entity.HasOne(d => d.User).WithMany(p => p.Reservations)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Reservations_User");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Email).HasColumnName("email");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.Six)
                .HasMaxLength(10)
                .HasColumnName("six");
            entity.Property(e => e.UserName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("userName");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
