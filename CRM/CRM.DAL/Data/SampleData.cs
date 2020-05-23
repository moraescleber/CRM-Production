using CRM.DAL.EF;
using CRM.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CRM.DAL.Data
{
    public class SampleData
    {
        public static async Task Initialize(UserManager<User> userManager, ApiContext context, RoleManager<IdentityRole> roleManager)
        {
            string adminEmail = "admin@gmail.com";
            string password = "_Aa123456";
            if (await userManager.FindByNameAsync(adminEmail) == null)
            {
                await roleManager.CreateAsync(new IdentityRole("admin"));
                await roleManager.CreateAsync(new IdentityRole("lead"));
                await roleManager.CreateAsync(new IdentityRole("client"));
                User admin = new User { Email = adminEmail, UserName = adminEmail };
                await userManager.CreateAsync(admin, password);
                await userManager.AddToRoleAsync(admin, "admin");
                User lead = new User { Email = "dustin.carroll@example.com", UserName = "dustin.carroll@example.com", FirstName = "Dustin", LastName = "Carroll" };
                await userManager.CreateAsync(lead, password);
                await userManager.AddToRoleAsync(lead, "lead");
                User client = new User { Email = "john.hale@example.com", UserName = "john.hale@example.com", FirstName = "John", LastName = "Hale" };
                await userManager.CreateAsync(client, password);
                await userManager.AddToRoleAsync(client, "client");
                User admin1 = new User { Email = "douglas.henry@example.com", UserName = "douglas.henry@example.com", FirstName = "Douglas", LastName = "Henry" };
                await userManager.CreateAsync(admin1, password);
                await userManager.AddToRoleAsync(admin1, "admin");
                User client1 = new User { Email = "dale.hopkins@example.com", UserName = "dale.hopkins@example.com", FirstName = "Dale", LastName = "Hopkins" };
                await userManager.CreateAsync(client1, password);
                await userManager.AddToRoleAsync(client1, "client");
                Region Africa = new Region { Name = "Africa" };
                Region Antarctica = new Region { Name = "Antarctica" };
                Region AsiaPacific = new Region { Name = "Asia/Pacifig" };
                Region Europe = new Region { Name = "Europe" };
                Region NorthAmerica = new Region { Name = "North America" };
                Region SouthAmerica = new Region { Name = "South America" };
                await context.Regions.AddRangeAsync(Africa, Antarctica, AsiaPacific, Europe, NorthAmerica, SouthAmerica);
                Country Australia = new Country { Capital = "Canberra", Name = "Australia", Region = AsiaPacific };
                Country Germany = new Country { Capital = "Berlin", Name = "Germany", Region = Europe };
                Country Slovakia = new Country { Capital = "Bratislava", Name = "Slovakia", Region = Europe };
                Country USA = new Country { Capital = "Washington", Name = "Unated States", Region = NorthAmerica };
                Country TM = new Country { Capital = "Ashgabat", Name = "Turkmenistan", Region = AsiaPacific };
                await context.Countries.AddRangeAsync(Australia, Germany, Slovakia, USA);
                CompanyQualification Qualified = new CompanyQualification { QualificationName = "Qualified" };
                CompanyQualification NotQualified = new CompanyQualification { QualificationName = "NotQualified" };
                CompanyQualification NewCompany = new CompanyQualification { QualificationName = "NewCompany" };
                await context.CompanyQualifications.AddRangeAsync(Qualified, NotQualified, NewCompany);
                Linkedin SlastininLinkedin = new Linkedin { FullLink = "linkedin.com/in/aleksandr-slastinin-379654183/" };
                Linkedin TTRLinkedin = new Linkedin { FullLink = "linkedin.com/company/turkmen-tranzit/" };
                Linkedin TTWLinkedin = new Linkedin { FullLink = "linkedin.com/company/ttweb/" };
                await context.Linkedins.AddRangeAsync(SlastininLinkedin, TTRLinkedin, TTWLinkedin);
                await context.SaveChangesAsync();
                Contact contact1 = new Contact { Email = "aojv@mail.ru" };
                Contact contact2 = new Contact { Email = "kggfpxw@yandex.ru" };
                Contact contact3 = new Contact { Email = "oxxv@yandex.ru" };
                Contact contact4 = new Contact { Email = "f9jxjd14@gmail.com" };
                Contact contact5 = new Contact { Email = "p24a@mail.ru" };
                Contact contact6 = new Contact { Email = "ahbg@yandex.ru" };
                Contact contact7 = new Contact { Email = "xl9bc5@gmail.com" };
                Contact contact8 = new Contact { Email = "q4aptu@mail.ru" };
                Contact contact9 = new Contact { Email = "n4zc9kz@yandex.ru" };
                Contact contact10 = new Contact { Email = "copaa6@gmail.com" };
                Contact contact11 = new Contact { Email = "myrfqpb@mail.ru" };
                Contact contact12 = new Contact { Email = "kc29hc0e@yandex.ru" };
                Contact TtrContact1 = new Contact
                {
                    Email = "a.slastinin@turkmen-tranzit.com",
                    FirstName = "Aleksandr",
                    Position = "department head",
                    Surname = "Slastinin",
                    Linkedin = SlastininLinkedin
                };
                Contact TtrContact2 = new Contact { Email = "help@turkmen-tranzit.com" };
                Contact TtrContact3 = new Contact { Email = "support@turkmen-tranzit.com" };
                Contact TtwebContact1 = new Contact
                {
                    Email = "irada@ttweb.org",
                    FirstName = "Irada",
                    Position = "Project Manager",
                    Surname = "davletowa",
                };
                Contact TtwebContact2 = new Contact { Email = "help@ttweb.org" };
                Contact TtwebContact3 = new Contact { Email = "support@ttweb.org" };

                await context.Contacts.AddRangeAsync(contact1, contact2, contact3, contact4, contact5, contact6, contact7, contact8, contact9, contact10, contact11, contact12, TtrContact1, TtrContact2, TtrContact3, TtwebContact1, TtwebContact2, TtwebContact3);
                await context.SaveChangesAsync();
                Company ACN = new Company
                {
                    CompanyLegalName = "ATM ATM Pty Ltd",
                    HGBasedInCountry = Australia,
                    Qualification = Qualified,
                    TradingName = "A.C.N.",
                    Website = "b24.turkmen-tranzit.com"
                };
                await context.Companies.AddAsync(ACN);
                CompanyContactLink ContactACN1 = new CompanyContactLink
                {
                    Company = ACN,
                    Contact = contact1
                };
                CompanyContactLink ContactACN2 = new CompanyContactLink
                {
                    Company = ACN,
                    Contact = contact2
                };
                CompanyContactLink ContactACN3 = new CompanyContactLink
                {
                    Company = ACN,
                    Contact = contact3
                };
                await context.CompanyContactLinks.AddRangeAsync(ContactACN1, ContactACN2, ContactACN3);
                Company ATM = new Company
                {
                    CompanyLegalName = "A.C.N. 605 479 678 Pty Ltd",
                    HGBasedInCountry = Australia,
                    Qualification = Qualified,
                    TradingName = "ATM",
                    LeadOwner = lead,
                    QualifiedDate = DateTime.Now,
                    Website = "ttweb.org/blog"
                };
                await context.Companies.AddAsync(ATM);
                CompanyContactLink ContactATM1 = new CompanyContactLink
                {
                    Company = ATM,
                    Contact = contact4
                };
                CompanyContactLink ContactATM2 = new CompanyContactLink
                {
                    Company = ATM,
                    Contact = contact5
                };
                CompanyContactLink ContactATM3 = new CompanyContactLink
                {
                    Company = ATM,
                    Contact = contact6
                };
                await context.CompanyContactLinks.AddRangeAsync(ContactATM1, ContactATM2, ContactATM3);
                Company Pay = new Company
                {
                    CompanyLegalName = "24-pay s.r.o",
                    HGBasedInCountry = Australia,
                    Qualification = NotQualified,
                    TradingName = "24 Pay SRO",
                    LeadOwner = lead,
                    Website = "24-pay.sk"
                };
                await context.Companies.AddAsync(Pay);
                CompanyContactLink ContactPay1 = new CompanyContactLink
                {
                    Company = Pay,
                    Contact = contact7
                };
                CompanyContactLink ContactPay2 = new CompanyContactLink
                {
                    Company = Pay,
                    Contact = contact8
                };
                CompanyContactLink ContactPay3 = new CompanyContactLink
                {
                    Company = Pay,
                    Contact = contact9
                };
                await context.CompanyContactLinks.AddRangeAsync(ContactPay1, ContactPay2, ContactPay3);
                Company Ttz = new Company
                {
                    CompanyLegalName = "HO ''Turkmen-Tranzit''",
                    HGBasedInCountry = TM,
                    Qualification = NewCompany,
                    TradingName = "Turkmen-Tranzit",
                    Website = "Turkmen-Tranzit.com",
                    CompanyLinkedin = TTRLinkedin
                };
                await context.Companies.AddAsync(Ttz);
                CompanyContactLink ContactTtz1 = new CompanyContactLink
                {
                    Company = Ttz,
                    Contact = TtrContact1
                };
                CompanyContactLink ContactTtz2 = new CompanyContactLink
                {
                    Company = Ttz,
                    Contact = TtrContact2
                };
                CompanyContactLink ContactTtz3 = new CompanyContactLink
                {
                    Company = Ttz,
                    Contact = TtrContact3
                };
                await context.CompanyContactLinks.AddRangeAsync(ContactTtz1, ContactTtz2, ContactTtz3);
                await context.SaveChangesAsync();
                Company Ttw = new Company
                {
                    CompanyLegalName = "HO ''Turkmen-Tranzit'' -> TTWeb",
                    HGBasedInCountry = TM,
                    Qualification = Qualified,
                    TradingName = "TtWeb",
                    Website = "Ttweb.org",
                    CompanyLinkedin = TTWLinkedin
                };
                await context.Companies.AddAsync(Ttw);
                CompanyContactLink ContactTtw1 = new CompanyContactLink
                {
                    Company = Ttw,
                    Contact = TtwebContact1
                };
                CompanyContactLink ContactTtw2 = new CompanyContactLink
                {
                    Company = Ttw,
                    Contact = TtwebContact2
                };
                CompanyContactLink ContactTtw3 = new CompanyContactLink
                {
                    Company = Ttw,
                    Contact = TtwebContact3
                };
                await context.CompanyContactLinks.AddRangeAsync(ContactTtw1, ContactTtw2, ContactTtw3);
                await context.SaveChangesAsync();
                Company google = new Company
                {
                    CompanyLegalName = "google",
                    HGBasedInCountry = USA,
                    Qualification = Qualified,
                    TradingName = "google",
                    Website = "google.com"
                };
                await context.Companies.AddAsync(google);
                CompanyContactLink ContactGoogle1 = new CompanyContactLink
                {
                    Company = google,
                    Contact = contact10
                };
                CompanyContactLink ContactGoogle2 = new CompanyContactLink
                {
                    Company = google,
                    Contact = contact11
                };
                CompanyContactLink ContactGoogle3 = new CompanyContactLink
                {
                    Company = google,
                    Contact = contact12
                };
                await context.CompanyContactLinks.AddRangeAsync(ContactGoogle1, ContactGoogle2, ContactGoogle3);
                Company microsoft = new Company
                {
                    CompanyLegalName = "microsoft",
                    HGBasedInCountry = USA,
                    Qualification = Qualified,
                    TradingName = "microsoft",
                    Website = "microsoft.com"
                };
                await context.Companies.AddAsync(microsoft);

                AdvertisingCompany advertisingCompany = new AdvertisingCompany
                {
                    Id = "cam_r7jfwbKo46XiS9okW",
                    Name = "ttweb"
                };

                await context.AddAsync(advertisingCompany);


                await context.SaveChangesAsync();
            }
        }
    }
}
