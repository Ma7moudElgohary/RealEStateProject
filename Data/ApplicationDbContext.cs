using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RealEstate.Models;
using RealEStateProject.Models;


namespace RealEstate.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Property> Properties { get; set; }

        public DbSet<Agent> Agents { get; set; }

        public DbSet<PropertyImage> PropertyImages { get; set; }

        public DbSet<Favorite> Favorites { get; set; }

        public DbSet<Message> Messages { get; set; }

        public DbSet<Review> Reviews { get; set; }

    }
}