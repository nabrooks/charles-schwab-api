using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using NbTrader.Utility.Serialization;

namespace NbTrader.Brokers.TDAmeritrade.Models
{
    public class TDAccountInfo
    {
        public string? Type { get; set; }

        public string? AccountId { get; set; }

        public long RoundTrips { get; set; }

        public bool IsDayTrader { get; set; }

        public bool IsClosingOnlyRestricted { get; set; }

        public List<TDPositionModel> Positions { get; set; } = new List<TDPositionModel>();

        public List<TDOrder> OrderStrategies { get; set; } = new List<TDOrder>();

        public TDBalancesModel? CurrentBalances { get; set; }

        //[JsonPropertyName("initialBalances")]
        //public TDInitialBalancesModel? InitialBalances { get; set; }

        //[JsonPropertyName("projectedBalances")]
        //public ProjectedBalances ProjectedBalances { get; set; }

        public override string ToString() => $"AccountId: {AccountId}";
    }

    public class TDBalancesModel
    {
        public double AccruedInterest { get; set; }

        public double AvailableFundsNonMarginableTrade { get; set; }

        public double BondValue { get; set; }

        public double BuyingPower { get; set; }

        public double CashBalance { get; set; }

        public double CashReceipts { get; set; }

        public double DayTradingBuyingPower { get; set; }

        public double Equity { get; set; }

        public double EquityPercentage { get; set; }

        public double LiquidationValue { get; set; }

        public double LongMarginValue { get; set; }

        public double LongOptionMarketValue { get; set; }

        public double LongMarketValue { get; set; }

        public double MoneyMarketFund { get; set; }

        public double Savings { get; set; }

        public double ShortMarketValue { get; set; }
        
        public double PendingDeposits { get; set; }

        public double AvailableFunds { get; set; }

        public double BuyingPowerNonMarginableTrade { get; set; }

        public double MaintenanceCall { get; set; }

        public double MaintenanceRequirement { get; set; }

        public double MarginBalance { get; set; }

        public double RegTCall { get; set; }

        public double ShortBalance { get; set; }

        public double ShortMarginValue { get; set; }

        public double ShortOptionMarketValue { get; set; }

        public double Sma { get; set; }

        public double MutualFundValue { get; set; }

        public override string ToString() => $"CashBalance: {CashBalance}, Equity: {Equity}, AvailableFunds {AvailableFunds}";
    }

    public class TDPositionModel
    {
        public double ShortQuantity { get; set; }

        public double AveragePrice { get; set; }

        public double CurrentDayProfitLoss { get; set; }

        public double CurrentDayProfitLossPercentage { get; set; }

        public double LongQuantity { get; set; }

        public double SettledLongQuantity { get; set; }

        public double SettledShortQuantity { get; set; }

        public TDInstrumentModel? Instrument { get; set; }

        public double MarketValue { get; set; }

        public override string ToString() => $"Symbol: {Instrument?.Symbol}, MarketValue: {MarketValue}, ShortQuantity: {ShortQuantity}, LongQuantity: {LongQuantity}";
    }

    public class TDInstrumentModel
    {
        public string? Cusip { get; set; }

        public string? Symbol { get; set; }

        public string? Description { get; set; }

        public string? Exchange { get; set; }

        public string? ExchangeName { get; set; }

        public TDAssetType? AssetType { get; set; }

        public override string ToString() => $"Symbol: {Symbol}, AssetType: {AssetType}, Exchange:{Exchange}, Description: {Description}";

    }

    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    public enum TDAssetType : int
    {
        [EnumMember(Value = "EQUITY")] Equity,
        [EnumMember(Value = "OPTION")] EquityOption,
        [EnumMember(Value = "ETF")] ExchangeTradedFund,
        [EnumMember(Value = "FUTURE")] Future,
        [EnumMember(Value = "FUTURE_OPTION")] FutureOption,
        [EnumMember(Value = "BOND")] Bond,
        [EnumMember(Value = "NUTUTAL_FUND")] MututalFund,
        [EnumMember(Value = "FOREX")] Forex,
        [EnumMember(Value = "INDEX")] Index,
        [EnumMember(Value = "INDICATOR")] Indicator,
        [EnumMember(Value = "UNKNOWN")] Unknown,
        [EnumMember(Value = "CASH_EQUIVALENT")] CashEquivalent

    }

    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    public enum TDOrderSessionType
    {
        [EnumMember(Value = "NONE")] None,
        [EnumMember(Value = "NORMAL")] Normal,
        [EnumMember(Value = "AM")] Am,
        [EnumMember(Value = "PM")] Pm,
        [EnumMember(Value = "SEAMLESS")] Seamless,
    }
    
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    public enum TDOrderDurationType
    {
        [EnumMember(Value = "DAY")] Day,
        [EnumMember(Value = "GOOD_TILL_CANCEL")] GoodTillCancel,
        [EnumMember(Value = "FILL_OR_KILL")] FillOrKill,
    }
    
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    public enum TDAmeritradeOrderType : int
    {
        [EnumMember(Value = "LIMIT")] Limit,
        [EnumMember(Value = "MARKET")] Market,
        [EnumMember(Value = "EXERCISE")] Exercise,
        [EnumMember(Value = "STOP")] StopMarket,
        [EnumMember(Value = "STOP_LIMIT")] StopLimit,
        [EnumMember(Value = "TRAILING_STOP")] TrailingStopMarket,
        [EnumMember(Value = "TRAILING_STOP_LIMIT")] TrailingStopLimit,
        [EnumMember(Value = "MARKET_ON_OPEN")] MarketOnOpen,
        [EnumMember(Value = "MARKET_ON_CLOSE")] MarketOnClose,
        [EnumMember(Value = "NET_DEBIT")] NetDebit,
        [EnumMember(Value = "NET_CREDIT")] NetCredit,
        [EnumMember(Value = "NET_ZERO")] NetZero,
        [EnumMember(Value = "NONE")] None
    }
    
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    public enum TDComplexOrderStrategyType
    {
        [EnumMember(Value = "NONE")] None,
        [EnumMember(Value = "COVERED")] COVERED,
        [EnumMember(Value = "VERTICAL")] VERTICAL,
        [EnumMember(Value = "BACK_RATIO")] BackRatio,
        [EnumMember(Value = "CALENDAR")] Calendar,
        [EnumMember(Value = "DIAGONAL")] Diagonal,
        [EnumMember(Value = "STRADDLE")] Straddle,
        [EnumMember(Value = "STRANGLE")] Strangle,
        [EnumMember(Value = "COLLAR_SYNTHETIC")] CollarSynthetic,
        [EnumMember(Value = "BUTTERFLY")] Butterfly,
        [EnumMember(Value = "CONDOR")] Condor,
        [EnumMember(Value = "IRON_CONDOR")] IronCondor,
        [EnumMember(Value = "VERTICAL_ROLL")] VerticalRoll,
        [EnumMember(Value = "COLLAR_WITH_STOCK")] CollarWithStock,
        [EnumMember(Value = "DOUBLE_DIAGONAL")] DoubleDiagonal,
        [EnumMember(Value = "UNBALANCED_BUTTERFLY")] UnbalancedButterfly,
        [EnumMember(Value = "UNBALANCED_CONDOR")] UnbalancedCondor,
        [EnumMember(Value = "UNBALANCED_IRON_CONDOR")] UnbalancedIronCondor,
        [EnumMember(Value = "UNBALANCED_VERTICAL_ROLL")] UnbalancedVerticalRoll,
        [EnumMember(Value = "CUSTOM")] Custom
    }
    
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    public enum TDOrderStrategyType
    {
        [EnumMember(Value = "SINGLE")] Single,
        [EnumMember(Value = "OCO")] Oco,
        [EnumMember(Value = "TRIGGER")] Trigger,
    }
    
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    public enum TDOrderStatusType
    {
        [EnumMember(Value = "AWAITING_PARENT_ORDER")] AwaitingParentOrder,
        [EnumMember(Value = "AWAITING_CONDITION")] AwaitingCondition,
        [EnumMember(Value = "AWAITING_MANUAL_REVIEW")] AwaitingManualReview,
        [EnumMember(Value = "ACCEPTED")] Accepted,
        [EnumMember(Value = "AWAITING_UR_OUT")] AwaitingUrOut,
        [EnumMember(Value = "PENDING_ACTIVATION")] PendingActivation,
        [EnumMember(Value = "QUEUED")] Queued,
        [EnumMember(Value = "WORKING")] Working,
        [EnumMember(Value = "REJECTED")] Rejected,
        [EnumMember(Value = "PENDING_CANCEL")] PendingCancel,
        [EnumMember(Value = "CANCELED")] Canceled,
        [EnumMember(Value = "PENDING_REPLACE")] PendingReplace,
        [EnumMember(Value = "FILLED")] Filled,
        [EnumMember(Value = "EXPIRED")] Expired,
        [EnumMember(Value = "NOT_DEFINED")] NotDefined,
    }

    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    public enum TDOrderLegType
    {
        [EnumMember(Value = "EQUITY")] Equity,
        [EnumMember(Value = "OPTION")] Option,
        [EnumMember(Value = "INDEX")] Index,
        [EnumMember(Value = "MUTUAL_FUND")] MutualFund,
        [EnumMember(Value = "CASH_EQUIVALENT")] CashEquivalent,
        [EnumMember(Value = "FIXED_INCOME")] FixedIncome,
        [EnumMember(Value = "CURRENCY")] Currency
    }

    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    public enum TDOrderInstructionType
    {
        [EnumMember(Value = "BUY")] Buy,
        [EnumMember(Value = "SELL")] Sell,
        [EnumMember(Value = "BUY_TO_OPEN")] BuyToOpen,
        [EnumMember(Value = "BUY_TO_CLOSE")] BuyToClose,
        [EnumMember(Value = "BUY_TO_COVER")] BuyToCover,
        [EnumMember(Value = "SELL_TO_OPEN")] SellToOpen,
        [EnumMember(Value = "SELL_TO_CLOSE")] SellToClose,
        [EnumMember(Value = "SELL_SHORT")] SellShort,
        [EnumMember(Value = "NONE")] None,
        [EnumMember(Value = "EXCHANGE")] Exchange
    }
}
