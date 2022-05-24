using System;
using System.Collections.Generic;
using System.Text;

namespace AdCourier.Domain.Entities.ViewModel.DatabaseViewModel
{
    public class BorrowerBusinessDataViewModel
    {
        public int CourierUserId { get; set; }
        public int NoOfMaxProductSellingMonth { get; set; } = 0;
        public decimal AvgAmountOfMaxSellingMonth { get; set; } = 0;
        public int NoOfRegularProductSellingMonth { get; set; } = 0;
        public decimal AvgAmountOfRegularSellingMonth { get; set; } = 0;
        public int NoOfLowestProductSellingMonth { get; set; } = 0;
        public decimal AvgAmountOfLowestSellingMonth { get; set; } = 0;
        public int NoOfMaxProductPurchasedMonth { get; set; } = 0;
        public decimal AvgAmountOfMaxPurchasedMonth { get; set; } = 0;
        public int NoOfRegularProductPurchasedMonth { get; set; } = 0;
        public decimal AvgAmountOfRegularPurchasedMonth { get; set; } = 0;
        public int NoOfLowestProductPurchasedMonth { get; set; } = 0;
        public decimal AvgAmountOfLowestPurchasedMonth { get; set; } = 0;
        public decimal BusinessRent { get; set; } = 0;
        public decimal SalaryOfEmployee { get; set; } = 0;
        public decimal TransportationCost { get; set; } = 0;
        public decimal UtilityCost { get; set; } = 0;
        public decimal RepairCost { get; set; } = 0;
        public decimal PackagingCost { get; set; } = 0;
        public decimal BusinessTax { get; set; } = 0;
        public decimal OthersExpense { get; set; } = 0;
        public decimal DebotrsOwingAmount { get; set; } = 0;
        public decimal DebotrsOwingAmountPercentageOnSale { get; set; } = 0;
        public decimal CreditorsOwingAmount { get; set; } = 0;
        public decimal CreditorsOwingAmountPercentageOnPurchase { get; set; } = 0;
        public string BankShortCode { get; set; } = "";
        public string AccountType { get; set; } = "";
        public decimal CashInHand { get; set; } = 0;
        public decimal CashAtBank { get; set; } = 0;
        public string ChecqueBookPhotoUrl { get; set; } = "";
        public string AccountNumber { get; set; } = "";
        public string AccountName { get; set; } = "";
        public string RoutingNumber { get; set; } = "";
        public string BkashNumber { get; set; } = "";
        public string MobileBankingAccountType { get; set; } = "";
    }
}
