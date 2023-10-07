using Azure;
using BestFriendQuiz.EntityLayer.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Reflection.Emit;
using System.Security.Principal;


namespace BestFriendQuiz.Context
{


    public class BFQContext : IdentityDbContext<User, Role, int>
    {


        public BFQContext(DbContextOptions<BFQContext> options) : base(options)
        {

        }
        public DbSet<Question> Questionss { get; set; }
        public DbSet<QuestionOfSurvey> QuestionsOfSurveys { get; set; }
        public DbSet<Test> Tests { get; set; }
        public DbSet<TestResult> TestResults { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
      
            SeedRoles(builder);



        }

        private void SeedRoles(ModelBuilder builder)
        {
            Role role1 = new Role { Id = 1, Name = "Admin", ConcurrencyStamp = "1", NormalizedName = "Admin" };
            Role role2 = new Role { Id = 2, Name = "User", ConcurrencyStamp = "2", NormalizedName = "User" };
            Role role3 = new Role { Id = 3, Name = "Guest", ConcurrencyStamp = "3", NormalizedName = "Guest" };
            builder.Entity<Role>().HasData(role1, role2, role3);
        }
    }
}