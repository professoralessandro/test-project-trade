#region REFERENCES
using AppMktPlaceV2.Start.Application.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#endregion REFERENCES

namespace AppMktPlaceV2.Start.Domain.Entities
{
    [Table("Trade")]
    public partial class Trade
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key]
        public Guid? TradeId { get; set; }

        public int Value { get; set; }

        public string ClientSector { get; set; }

        public string? ClientRisk { get; set; }

        public DateTime DateRegistered { get; set; }

        public DateTime? DateUpdated { get; set; }
    }
}
