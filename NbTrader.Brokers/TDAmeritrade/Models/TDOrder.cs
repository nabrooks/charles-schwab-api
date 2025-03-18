namespace NbTrader.Brokers.TDAmeritrade.Models
{
    public class TDOrder
    {
        public TDOrderSessionType? Session { get; set; }

        public TDOrderDurationType? Duration { get; set; }

        public TDAmeritradeOrderType? OrderType { get; set; }

        public TDComplexOrderStrategyType? ComplexOrderStrategyType { get; set; }

        public double Quantity { get; set; }

        public double FilledQuantity { get; set; }

        public double RemainingQuantity { get; set; }

        public string? RequestedDestination { get; set; }

        public string? DestinationLinkName { get; set; }

        public double Price { get; set; }

        public double StopPrice { get; set; }

        public List<TDAmeritradeOrderLeg> OrderLegCollection { get; set; } = new List<TDAmeritradeOrderLeg>();

        public TDOrderStrategyType? OrderStrategyType { get; set; }

        public long OrderId { get; set; }

        public bool Cancelable { get; set; }

        public bool Editable { get; set; }

        public TDOrderStatusType? Status { get; set; }

        public string? EnteredTime { get; set; }

        public string? ClosedTime { get; set; }

        public long AccountId { get; set; }

        public override string ToString() => $"AccountId: {AccountId}, Status: {Status}, OrderType: {OrderType}, Price: {Price}";
    }

    public class TDAmeritradeOrderLeg
    {
        public TDOrderLegType? OrderLegType { get; set; }

        public long LegId { get; set; }

        public TDInstrumentModel? Instrument { get; set; }

        public TDOrderInstructionType? Instruction { get; set; }

        public string? PositionEffect { get; set; }

        public double Quantity { get; set; }

        public override string ToString() => $"Symbol: {Instrument?.Symbol}, Quantity: {Quantity}, Instruction: {Instruction}";
    }

}
