using CtrlInvest.Services.StocksExchanges;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CtrlInvest.Receive.HistoricalData
{
    public class RequestQueue
    {
        private BlockingCollection<String> _queue = new BlockingCollection<String>();
        private ConcurrentQueue<String> _storageQueue = new ConcurrentQueue<String>();
        private IHistoricalPriceService _historicalPriceService;
        private IHistoricalEarningService _historicalEarningService;
        private RequestQueue() { }

        public void Launch(IHistoricalPriceService historicalPriceService, IHistoricalEarningService historicalEarningService)
        {
            _historicalPriceService = historicalPriceService;
            _historicalEarningService = historicalEarningService;

            Task.Factory.StartNew(StartConsumingTask);
            Task.Factory.StartNew(StartPersistingTask);
        }

        public void Add(String request) => _queue.Add(request);
        public void AddRange(List<String> requests) => requests.ForEach(r => Add(r));

        private void StartConsumingTask()
        {
            Parallel.ForEach(_queue.GetConsumingEnumerable(), new ParallelOptions { MaxDegreeOfParallelism = 5 }, EnqueueSaveRequest);
        }

        private void EnqueueSaveRequest(String request)
        {
            _storageQueue.Enqueue(request);
        }

        private async Task StartPersistingTask()
        {
            while (true)
            {
                if (_storageQueue.Count > 1000)
                {
                    var requestChunck = new List<String>();
                    String req;
                    for (var i = 0; i < 1000; i++)
                    {
                        if (_storageQueue.TryDequeue(out req))
                            requestChunck.Add(req);
                    }

                    try
                    {
                        _historicalPriceService.SaveRangeInDatabaseOperation(requestChunck);
                    }
                    catch
                    {
                        AddRange(requestChunck);
                    }
                }
                else
                {
                    await Task.Delay(100);
                }
            }
        }

        private static RequestQueue _instance = new RequestQueue();
        public static RequestQueue Instance => _instance;
    }
}
