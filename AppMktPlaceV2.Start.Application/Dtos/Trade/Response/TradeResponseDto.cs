namespace AppMktPlaceV2.Start.Application.Dtos.Trade.Response
{
    public class TradeResponseDto
    {
        public Guid? TradeId { get; set; }

        public int Value { get; set; }

        public string ClientSector { get; set; }

        public string? ClientRisk { get; set; }

        public DateTime DateRegistered { get; set; }

        public DateTime? DateUpdated { get; set; }
    }
}
