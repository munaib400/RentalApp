using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace StarterApp.Database.Models;

[Table("rentals")]
[PrimaryKey(nameof(Id))]
public class Rental
{
    public int Id { get; set; }

    [Required]
    public string Status { get; set; } = "Requested";

    [Required]
    public DateTime StartDate { get; set; }

    [Required]
    public DateTime EndDate { get; set; }

    public decimal TotalCost { get; set; }

    public string? Notes { get; set; }

    public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;

    // Foreign keys
    public int ItemId { get; set; }
    public int RenterId { get; set; }

    // Navigation properties
    [ForeignKey(nameof(ItemId))]
    public Item? Item { get; set; }

    [ForeignKey(nameof(RenterId))]
    public User? Renter { get; set; }
}