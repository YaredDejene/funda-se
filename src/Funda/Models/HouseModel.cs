using System;

namespace Funda.Models
{
    public class HouseModel
    {
        public Guid Id { get; set; }
        public long GlobalId { get; set; }
        public double? Koopprijs { get; set; }
        public long MakelaarId { get; set; }
        public string MakelaarNaam { get; set; }
        public string Woonplaats { get; set; }
        public DateTime? PublicatieDatum { get; set; }
        public bool HasTuin { get; set; }
    }
}