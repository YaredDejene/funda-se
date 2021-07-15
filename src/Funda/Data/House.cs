using System;
using System.ComponentModel.DataAnnotations;

namespace Funda.Data
{
    public class House
    {
        [Key]
        public Guid Id { get; set; }
        public long GlobalId { get; set; }
        public double? Koopprijs { get; set; }
        public long MakelaarId { get; set; }
        [Required]
        [MaxLength(128)]
        public string MakelaarNaam { get; set; }
        [Required]
        [MaxLength(128)]
        public string Woonplaats { get; set; }
        public DateTime? PublicatieDatum { get; set; }
        public bool HasTuin { get; set; }
    }
}