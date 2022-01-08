using System.ComponentModel;

namespace CtrlInvest.CrossCutting.Enums
{
    public enum ETransactionType
    {
        [Description("Income")]
        INCOME = 0,
        [Description("Expense")]
        EXPENSE = 1,
        [Description("Transfer")]
        TRANSFER = 2,
        [Description("Investiment")]
        INVESTIMENT = 3
    }

    public enum EPaymentMethod
    {
        [Description("Cash")]
        Cash = 0,
        [Description("Credit card")]
        CREDIT_CARD = 1,
        [Description("Deferred payment")]
        DEFERRED_PAYMENT = 2
    }

    public enum ETypeDeal
    {
        [Description("Buy")]
        Buy = 0,
        [Description("Sell")]
        Sell = 1        
    }

    public enum ETypeMarket

    {
        [Description("odd lot market")]
        ODD_LOT_MARKET = 0,
        [Description("cash market ")]
        CASH_MARKET = 1
    }
}
