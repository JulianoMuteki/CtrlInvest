using System;

namespace CtrlInvest.ImportHistorical
{
    class Program
    {
        static void Main(string[] args)
        {
            Context context;
            // Three contexts following different strategies
            context = new Context(new ConcreteStrategyA());
            context.ImportHistoricalByDates();
            // Wait for user
            Console.ReadKey();
        }
    }
}
