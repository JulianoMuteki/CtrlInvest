
namespace CtrlInvest.Receive.HistoricalData
{
    public interface IHistoricalPriceService
    {
        void SaveInDatabaseOperation(string message);
    }
}
