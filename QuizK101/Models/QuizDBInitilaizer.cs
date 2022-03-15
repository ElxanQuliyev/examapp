using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace QuizK101.Models
{
    public class QuizDBInitilaizer : CreateDatabaseIfNotExists<ApplicationDbContext>
    {

        protected override void Seed(ApplicationDbContext context)
        {
            SeedRoles(context);
            SeedUsers(context);
        }

        public void SeedRoles(ApplicationDbContext context)
        {
            List<IdentityRole> rolesInDealDouble = new List<IdentityRole>();

            rolesInDealDouble.Add(new IdentityRole() { Name = "Administrator" });
            rolesInDealDouble.Add(new IdentityRole() { Name = "Moderator" });
            rolesInDealDouble.Add(new IdentityRole() { Name = "User" });

            var rolesStore = new RoleStore<IdentityRole>(context);
            var rolesManager = new RoleManager<IdentityRole>(rolesStore);

            foreach (IdentityRole role in rolesInDealDouble)
            {
                if (!rolesManager.RoleExists(role.Name))
                {
                    var result = rolesManager.Create(role);

                    if (result.Succeeded) continue;
                }
            }
        }

        public void SeedUsers(ApplicationDbContext context)
        {
            var usersStore = new UserStore<QuizUser>(context);
            var usersManager = new UserManager<QuizUser>(usersStore);

            QuizUser admin = new QuizUser();

            admin.Email = "quiz@mail.com";
            admin.UserName = "quiz@mail.com";
            var password = "Admin_123";
            admin.PhoneNumber = "0508006000";

            if (usersManager.FindByEmail(admin.Email) == null)
            {
                var result = usersManager.Create(admin, password);

                if (result.Succeeded)
                {
                    //add necessary roles to admin
                    usersManager.AddToRole(admin.Id, "Administrator");
                    usersManager.AddToRole(admin.Id, "Moderator");
                    usersManager.AddToRole(admin.Id, "User");
                }
            }
        }

    }
}