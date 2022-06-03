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

        private bool isRunPersistingTask = true;
        private RequestQueue() { }

        public void Launch(IHistoricalPriceService historicalPriceService)
        {
            _historicalPriceService = historicalPriceService;

            Task.Factory.StartNew(StartConsumingTask);
            Task.Factory.StartNew(StartPersistingTask);
        }

        public void Launch(IHistoricalEarningService historicalEarningService)
        {
            _historicalEarningService = historicalEarningService;

            Task.Factory.StartNew(StartConsumingTask);
            Task.Factory.StartNew(StartPersistingTask);
        }

        public void Add(String request) => _queue.Add(request);
        public void AddRange(List<String> requests) => requests.ForEach(r => Add(r));

        private void StartConsumingTask()
        {
            Parallel.ForEach(_queue.GetConsumingEnumerable(), new ParallelOptions { MaxDegreeOfParallelism = 1 }, EnqueueSaveRequest);
        }

        private void EnqueueSaveRequest(String request)
        {
            _storageQueue.Enqueue(request);
        }

        private async Task StartPersistingTask()
        {
            while (isRunPersistingTask)
            {
                int checkCount = GetCountCalculate();
                if (_storageQueue.Count >= checkCount && _storageQueue.Count > 0)
                {
                    var requestChunck = new List<String>();
                    String req;
                    for (var i = 0; i <= checkCount; i++)
                    {
                        if (_storageQueue.TryDequeue(out req))
                            requestChunck.Add(req);
                    }

                    try
                    {
                        if (_historicalPriceService != null)
                        {
                            await _historicalPriceService.SaveRangeInDatabaseOperation(requestChunck); 
                        }

                        else
                            _historicalEarningService.SaveRangeInDatabaseOperation(requestChunck);
                    }
                    catch (Exception ex)
                    {
                        AddRange(requestChunck);
                    }
                }
                else
                {
                    await Task.Delay(5000);
                }
            }
        }

        private int GetCountCalculate()
        {
            int count = 1000;
            if (_storageQueue.Count > 10000)
            {
                count = 2000;
            }
            else if (_storageQueue.Count > 1000 && _storageQueue.Count < 10000)
            {
                count = 1000;
            }
            else if (_storageQueue.Count < 1000 && _storageQueue.Count > 500)
            {
                count = 500;
            }
            else
                count = _storageQueue.Count;

            return count;
        }

        public void StopLaunch()
        {
            isRunPersistingTask = false;
        }

        private static RequestQueue _instance = new RequestQueue();
        public static RequestQueue Instance => _instance;
    }
}
