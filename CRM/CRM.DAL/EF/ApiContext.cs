using CRM.DAL.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CRM.DAL.EF
{
    public class ApiContext : IdentityDbContext<User>
    {
        public DbSet<Company> Companies { get; set; }
        public DbSet<CompanyQualification> CompanyQualifications { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<MailMessage> MailMessages { get; set; }
        public DbSet<Email> Emails { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<CompanyContactLink> CompanyContactLinks { get; set; }
        public DbSet<Linkedin> Linkedins { get; set; }
        public DbSet<AdvertisingCompany> AdvertisingCompanies { get; set; }

        public ApiContext(DbContextOptions<ApiContext> options) : base(options)
        {
            Database.EnsureCreated();
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Company>()
                .HasIndex(p => p.CompanyLegalName).IsUnique();
            /*modelBuilder.Entity<Company>()
                .HasIndex(p => p.TradingName).IsUnique();
            modelBuilder.Entity<Company>()
                .HasIndex(p => p.Website).IsUnique();*/
            base.OnModelCreating(modelBuilder);
        }
    }
}
