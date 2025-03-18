using NbTrader.Brokers.TDAmeritrade.V2.Services.TransactionHistory.Models;
using NbTrader.Brokers.TDAmeritrade.V2.Services.TransactionHistory.Types;

namespace NbTrader.Brokers.TDAmeritrade.V2.Services.TransactionHistory
{
    public interface ITransactionHistoryService
    {
        Task<IList<Transaction>> GetTransactionsByQueryAsync(string accountID, TransactionType? type = null, string? symbol = null, DateTime? startDate = null, DateTime? fromDate = null);
        Task<Transaction> GetTransactionAsync(string accountID, string transactionID);
    }
}