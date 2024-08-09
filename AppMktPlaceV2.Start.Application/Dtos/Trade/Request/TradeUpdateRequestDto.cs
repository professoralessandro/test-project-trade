namespace AppMktPlaceV2.Start.Application.Dtos.Trade.Request
{
    public class TradeUpdateRequestDto
    {
        public Guid TradeId { get; set; }

        public int Value { get; set; }

        public string ClientSector { get; set; }
    }
}
