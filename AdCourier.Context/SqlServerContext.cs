using AdCourier.Domain.Entities.DataModel;
using AdCourier.Domain.Entities.ProcedureDataModel;
using Microsoft.EntityFrameworkCore;

namespace AdCourier.Context
{
    public class SqlServerContext : DbContext
    {
        public SqlServerContext(DbContextOptions<SqlServerContext> options) : base(options)
        {

        }

        public DbSet<CourierOrders> CourierOrders { set; get; }
        public DbSet<CourierUsers> CourierUsers { set; get; }
        public DbSet<DeliveryChargeDetails> DeliveryChargeDetails { set; get; }
        public DbSet<DeliveryChargeMerchantDetails> DeliveryChargeMerchantDetails { set; get; }
        public DbSet<ChangeDeliveryChargeDetailsLog> ChangeDeliveryChargeDetailsLog { set; get; }
        public DbSet<DeliveryRange> DeliveryRange { set; get; }
        public DbSet<WeightRange> WeightRange { set; get; }
        public DbSet<Districts> Districts { set; get; }
        public DbSet<CourierOrderStatusHistory> CourierOrderStatusHistory { set; get; }
        public DbSet<CourierOrderStatus> CourierOrderStatus { set; get; }
        public DbSet<ExtraCharge> ExtraCharge { set; get; }
        public DbSet<Users> Users { set; get; }
        public DbSet<StatusGroup> StatusGroup { get; set; }
        public DbSet<Couriers> Couriers { get; set; }
        public DbSet<Settings> Settings { get; set; }
        public DbSet<Permission> Permission { get; set; }
        public DbSet<Collectors> Collectors { get; set; }
        public DbSet<DeliveryUsers> DeliveryUsers { get; set; }
        public DbSet<CollectorAssign> CollectorAssign { get; set; }
        public DbSet<DeliveryBonduAssign> DeliveryBonduAssign { get; set; }
        public DbSet<PackagingChargeRange> PackagingChargeRange { get; set; }
        public DbSet<CourierAssign> CourierAssign { get; set; }
        public DbSet<GenerateLink> GenerateLink { get; set; }
        public DbSet<Hub> Hub { get; set; }
        public DbSet<Coupons> Coupons { get; set; }
        public DbSet<Payments> Payments { get; set; }
        public DbSet<UserProfile> UserProfile { get; set; }
        public DbSet<Customers> Customers { get; set; }
        public DbSet<Deals> Deals { get; set; }
        public DbSet<DeliveryZoneLocation> DeliveryZoneLocation { get; set; }
        public DbSet<DeliveryZone> DeliveryZone { get; set; }
        public DbSet<PickupLocations> PickupLocations { get; set; }
        public DbSet<LocationAssign> LocationAssign { get; set; }
        public DbSet<DbActionBn> DbActionBn { get; set; }
        public DbSet<CollectionTimeSlot> CollectionTimeSlot { get; set; }
        public DbSet<Referrer> Referrer { get; set; }
        public DbSet<Referee> Referee { get; set; }
        public DbSet<MerchantVisited> MerchantVisited { get; set; }
        public DbSet<MerchantCalled> MerchantCalled { get; set; }

        public DbSet<OrderAssignTrack> OrderAssignTrack { get; set; }
        public DbSet<OrderRequest> OrderRequest { get; set; }

        public DbSet<BkashPayment> BkashPayment { get; set; }

        public DbSet<SurveyQuestion> SurveyQuestion { get; set; }
        public DbSet<SurveyAnswer> SurveyAnswer { get; set; }
        public DbSet<SubSurveyAnswer> SubSurveyAnswer { get; set; }
        public DbSet<SurveyQuestionAnswerLog> SurveyQuestionAnswerLog { get; set; }

        public DbSet<Category> Category { get; set; }
        public DbSet<SubCategory> SubCategory { get; set; }
        public DbSet<AssignCourierUserCategory> AssignCourierUserCategory { get; set; }

        public DbSet<DeliveryChargeDetails_test> DeliveryChargeDetails_test { set; get; }
        public DbSet<LoanSurvey> LoanSurvey { get; set; }
        public DbSet<CibFormData> CibFormData { get; set; }
        public DbSet<CibFormOtherLoan> CibFormOtherLoan { get; set; }
        public DbSet<LoanApplicantFamilyMember> LoanApplicantFamilyMember { get; set; }
        public DbSet<LenderUser> LenderUser { get; set; }
        public DbSet<BorrowerBusinessData> BorrowerBusinessData { get; set; }
        public DbSet<LoanStatus> LoanStatus { get; set; }
        public DbSet<LoanDisbursement> LoanDisbursement { get; set; }
        public DbSet<LoanDisbursementHistory> LoanDisbursementHistory { get; set; }
        public DbSet<RepaymentStatus> RepaymentStatus { get; set; }
        public DbSet<TeleSaleCourierUsers> TeleSaleCourierUsers { get; set; }
        public DbSet<LenderCourierUserAssignment> LenderCourierUserAssignment { get; set; }
        public DbSet<LoanApprovalPayment> LoanApprovalPayment { get; set; }
        public DbSet<CustomerSMS> CustomerSMS { get; set; }
        public DbSet<CouriersWithLoanSurvey> CouriersWithLoanSurvey { get; set; }
        public DbSet<Vouchers> Vouchers { get; set; }
        public DbSet<UserLocationAssign> UserLocationAssign { get; set; }
        public DbSet<OwnPhoneBook> OwnPhoneBook { get; set; }
        public DbSet<PhoneBookGroup> PhoneBookGroup { get; set; }
        public DbSet<SMSPurchase> SMSPurchase { get; set; }
        public DbSet<PriceAndOrderTypeHistory> PriceAndOrderTypeHistory { get; set; }

        public DbSet<PaymentReference> PaymentReference { get; set; }
        public DbSet<PaymentReferenceDetails> PaymentReferenceDetails { get; set; }
        public DbSet<PaymentMail> PaymentMail { get; set; }
        public DbSet<AccountingHit> AccountingHit { get; set; }

        

        //initial db set for procedure

        public DbSet<OrderModel> OrderModel { get; set; }
        public DbSet<MerchantOrder> MerchantOrder { get; set; }
        public DbSet<CollectorReceived> CollectorReceived { get; set; }

        public DbSet<AcquisitionLeadManagement> AcquisitionLeadManagement { get; set; }
        public DbSet<MerchantInfoUpdate> MerchantInfoUpdate { get; set; }
        public DbSet<LocationAssignHistory> LocationAssignHistory { get; set; }

        public DbSet<CustomComment> CustomComment { get; set; }
        public DbSet<RedxPop> RedxPop { get; set; }
        public DbSet<CourierLocation> CourierLocation { get; set; }
        public DbSet<SACodCharges> SACodCharges { get; set; }
        public DbSet<PohScore> PohScore { get; set; }
    }
}
