using RealEStateProject.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealEstate.Models
{
    public class Property
    {
        [Key]
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        public int Area { get; set; }

        public int Bedrooms { get; set; }

        public int Bathrooms { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string ZipCode { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public PropertyType Type { get; set; }

        public PropertyStatus Status { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public string? AgentId { get; set; }

        [ForeignKey("AgentId")]
        public ApplicationUser Agent { get; set; }

        public ICollection<PropertyImage>? Images { get; set; }

        public string? FeaturedImage { get; set; }

        public ICollection<PropertyRequest>? Requests { get; set; }

        public ICollection<Review>? Reviews { get; set; }


    }

    public enum PropertyType
    {
        RENT,
        SALE
    }

    public enum PropertyStatus
    {
        AVAILABLE,
        PENDING,
        SOLD,
        RENTED
    }

    public enum RequestStatus
    {
        PENDING,
        APPROVED,
        REJECTED
    }

    public enum City
    {
        Cairo,
        Alexandria,
        Giza,
        ShubraElKheima,
        PortSaid,
        Suez,
        Luxor,
        Mansoura,
        Tanta,
        Asyut,
        Ismailia,
        Fayyum,
        Zagazig,
        Aswan,
        Damietta,
        Damanhur,
        alMinya,
        BeniSuef,
        Qena,
        Sohag,
        Hurghada,
        ShibinElKom,
        Banha,
        KafrelSheikh,
        Arish,
        Mallawi,
        Bilbais,
        MarsaMatruh,
        Idku,
        MitGhamr,
        AlHamidiyya,
        Desouk,
        Qalyub,
        AbuKabir,
        KafrelDawwar,
        Girga,
        Akhmim,
        Matareya,
        AbuQir,
        KafrElZayat,
        ShibinElQanater,
        Rosetta

    }
}