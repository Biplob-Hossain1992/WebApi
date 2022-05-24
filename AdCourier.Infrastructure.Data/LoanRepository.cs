using AdCourier.Context;
using AdCourier.Domain.Entities;
using AdCourier.Domain.Entities.DataModel;
using AdCourier.Domain.Entities.ViewModel.DatabaseViewModel;
using AdCourier.Domain.Entities.ViewModel.Other;
using AdCourier.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AdCourier.Infrastructure.Data
{
    public class LoanRepository : ILoanRepository
    {
        private readonly SqlServerContext _sqlServerContext;
        private readonly ConnectionStringList _connectionStrings;
        public LoanRepository(SqlServerContext sqlServerContext, IOptions<ConnectionStringList> connectionStrings)
        {
            _sqlServerContext = sqlServerContext ?? throw new ArgumentNullException(nameof(sqlServerContext));
            _connectionStrings = connectionStrings.Value;
        }

        public async Task<LoanSurvey> AddLoanSurvey(LoanSurvey loadSurvey)
        {
            await _sqlServerContext.LoanSurvey.AddAsync(loadSurvey);
            await _sqlServerContext.SaveChangesAsync();
            return loadSurvey;
        }

        public async Task<IEnumerable<dynamic>> GetLoanSurveyByCourierUser(int CourierUserId)
        {
            var loanSurveys = await (from loan in _sqlServerContext.LoanSurvey.AsNoTracking()
                               join courierLoan in _sqlServerContext.CouriersWithLoanSurvey
                               on loan.LoanSurveyId equals courierLoan.LoanSurveyId into lc
                               from _courier in lc.DefaultIfEmpty()
                               join applicantFamMem in _sqlServerContext.LoanApplicantFamilyMember.AsNoTracking()
                               on loan.LoanSurveyId equals applicantFamMem.LoanSurveyId into groupJoin
                               from subApplicantFamMem in groupJoin.DefaultIfEmpty()
                               where loan.CourierUserId.Equals(CourierUserId)
                               select new 
                               {
                                   LoanSurveyId = loan.LoanSurveyId,
                                   CourierUserId = loan.CourierUserId,
                                   Gender = loan.Gender,
                                   TradeLicenseImageUrl = loan.TradeLicenseImageUrl,
                                   InterestedAmount = loan.InterestedAmount,
                                   TransactionAmount = loan.TransactionAmount,
                                   IsBankAccount = loan.IsBankAccount,
                                   IsLocalShop = loan.IsLocalShop,
                                   ApplicationDate = loan.ApplicationDate,
                                   MonthlyTotalCodAmount = loan.MonthlyTotalCodAmount,
                                   GuarantorMobile = loan.GuarantorMobile,
                                   GuarantorName = loan.GuarantorName,
                                   MonthlyTotalAverageSale = loan.MonthlyTotalAverageSale,
                                   MerchantName = loan.MerchantName,
                                   LoanAmount = loan.LoanAmount,
                                   BankName = loan.BankName,
                                   Age = loan.Age,
                                   BasketValue = loan.BasketValue,
                                   CardHolder = loan.CardHolder,
                                   CardLimit = loan.CardLimit,
                                   LoanEmi = loan.LoanEmi,
                                   HasCreditCard = loan.HasCreditCard,
                                   HasTin = loan.HasTin,
                                   EduLevel = loan.EduLevel,
                                   RepayType = loan.RepayType,
                                   MonthlyOrder = loan.MonthlyOrder,
                                   MonthlyExp = loan.MonthlyExp,
                                   Recommend = loan.Recommend,
                                   RelationMarchent = loan.RelationMarchent,
                                   ShopOwnership = loan.ShopOwnership,
                                   TinNumber = loan.TinNumber,
                                   HomeOwnership = loan.HomeOwnership,
                                   Married = loan.Married,
                                   FamMem = loan.FamMem,
                                   HasTradeLicense = loan.HasTradeLicense,
                                   TradeLicenseNo = loan.TradeLicenseNo,
                                   TradeLicenseExpireDate = loan.TradeLicenseExpireDate,
                                   CompanyBankAccNo = loan.CompanyBankAccNo,
                                   CompanyBankAccName = loan.CompanyBankAccName,
                                   AnnualTotalIncome = loan.AnnualTotalIncome,
                                   DateOfBirth = loan.DateOfBirth,
                                   NidNo = loan.NidNo,
                                   OthersIncome = loan.OthersIncome,
                                   ReqTenorMonth = loan.ReqTenorMonth,
                                   ResidenceLocation = loan.ResidenceLocation,
                                   HasPreviousLoan = loan.HasPreviousLoan,
                                   BankStatementUrl = loan.BankStatementUrl,
                                   Comments = loan.Comments,
                                   BusinessStartDate = loan.BusinessStartDate,
                                   NidImageUrl = loan.NidImageUrl,
                                   NidBackImageUrl = loan.NidBackImageUrl,
                                   TinImageUrl = loan.TinImageUrl,
                                   CibUploadedFormUrl = loan.CibUploadedFormUrl,
                                   PresentAddHouseNo = loan.PresentAddHouseNo,
                                   PresentAddRoadNo = loan.PresentAddRoadNo,
                                   PresentAddRoadName = loan.PresentAddRoadName,
                                   PresentAddArea = loan.PresentAddArea,
                                   PresentAddPostOffice = loan.PresentAddPostOffice,
                                   PresentAddDistrictId = loan.PresentAddDistrictId,
                                   PresentAddThanaId = loan.PresentAddThanaId,
                                   HouseOwner = loan.HouseOwner,
                                   IsOwner = loan.IsOwner,
                                   DurationOfLiving = loan.DurationOfLiving,
                                   PermanentAddHouseNo = loan.PermanentAddHouseNo,
                                   PermanentAddRoadNo = loan.PermanentAddRoadNo,
                                   PermanentAddRoadName = loan.PermanentAddRoadName,
                                   PermanentAddArea = loan.PermanentAddArea,
                                   PermanentAddPostOffice = loan.PermanentAddPostOffice,
                                   PermanentAddDistrictId = loan.PermanentAddDistrictId,
                                   PermanentAddThanaId = loan.PermanentAddThanaId,
                                   FatherName = loan.FatherName,
                                   MotherName = loan.MotherName,
                                   SpouseName = loan.SpouseName,
                                   IsLoanDue = loan.IsLoanDue,
                                   Experience = loan.Experience,
                                   Occupation = loan.Occupation,
                                   PermanentAddHouseOwnerName = loan.PermanentAddHouseOwnerName,
                                   CreditCardNumber = loan.CreditCardNumber,
                                   ApplicantPhotoUrl = loan.ApplicantPhotoUrl,
                                   LoanRepayType = loan.LoanRepayType,
                                   OtherIncomeSource = loan.OtherIncomeSource,
                                   CourierWithLoanSurvey = new
                                   {
                                       CouriersWithLoanSurveyId = _courier == null ? 0 : _courier.CouriersWithLoanSurveyId,
                                       CourierId = _courier == null ? 0 : _courier.CourierId,
                                       CourierName = _courier == null ? "" : (_courier.CourierName ?? ""),
                                       LoanSurveyId = _courier == null ? 0 : _courier.LoanSurveyId
                                   },
                                   ApplicantFamMem = new
                                   {
                                       LoanSurveyId = subApplicantFamMem == null ? 0 : subApplicantFamMem.LoanSurveyId,
                                       FamilyMemberId = subApplicantFamMem == null ? 0 : subApplicantFamMem.FamilyMemberId,
                                       CourierUserId = subApplicantFamMem == null ? 0 : subApplicantFamMem.CourierUserId,
                                       FamilyMemberName = subApplicantFamMem == null ? "" : (subApplicantFamMem.FamilyMemberName ?? ""),
                                       Relation = subApplicantFamMem == null ? "" : (subApplicantFamMem.Relation ?? ""),
                                       Age = subApplicantFamMem == null ? 0 : subApplicantFamMem.Age,
                                       Education = subApplicantFamMem == null ? "" : (subApplicantFamMem.Education ?? ""),
                                       IsMarried = subApplicantFamMem == null ? false : subApplicantFamMem.IsMarried,
                                       Occupation = subApplicantFamMem == null ? "" : (subApplicantFamMem.Occupation ?? ""),
                                   }
                               }).OrderByDescending(o => o.LoanSurveyId).ToListAsync();

            var returnLoanSurveyData = loanSurveys.GroupBy(g => g.CourierUserId)
                                .Select(s => new
                                {
                                    LoanSurveyId = s.FirstOrDefault().LoanSurveyId,
                                    Gender = s.FirstOrDefault().Gender,
                                    TradeLicenseImageUrl = s.FirstOrDefault().TradeLicenseImageUrl,
                                    InterestedAmount = s.FirstOrDefault().InterestedAmount,
                                    TransactionAmount = s.FirstOrDefault().TransactionAmount,
                                    IsBankAccount = s.FirstOrDefault().IsBankAccount,
                                    IsLocalShop = s.FirstOrDefault().IsLocalShop,
                                    ApplicationDate = s.FirstOrDefault().ApplicationDate,
                                    MonthlyTotalCodAmount = s.FirstOrDefault().MonthlyTotalCodAmount,
                                    GuarantorMobile = s.FirstOrDefault().GuarantorMobile,
                                    GuarantorName = s.FirstOrDefault().GuarantorName,
                                    MonthlyTotalAverageSale = s.FirstOrDefault().MonthlyTotalAverageSale,
                                    MerchantName = s.FirstOrDefault().MerchantName,
                                    LoanAmount = s.FirstOrDefault().LoanAmount,
                                    BankName = s.FirstOrDefault().BankName,
                                    Age = s.FirstOrDefault().Age,
                                    BasketValue = s.FirstOrDefault().BasketValue,
                                    CardHolder = s.FirstOrDefault().CardHolder,
                                    CardLimit = s.FirstOrDefault().CardLimit,
                                    LoanEmi = s.FirstOrDefault().LoanEmi,
                                    HasCreditCard = s.FirstOrDefault().HasCreditCard,
                                    HasTin = s.FirstOrDefault().HasTin,
                                    EduLevel = s.FirstOrDefault().EduLevel,
                                    RepayType = s.FirstOrDefault().RepayType,
                                    MonthlyOrder = s.FirstOrDefault().MonthlyOrder,
                                    MonthlyExp = s.FirstOrDefault().MonthlyExp,
                                    Recommend = s.FirstOrDefault().Recommend,
                                    RelationMarchent = s.FirstOrDefault().RelationMarchent,
                                    ShopOwnership = s.FirstOrDefault().ShopOwnership,
                                    TinNumber = s.FirstOrDefault().TinNumber,
                                    HomeOwnership = s.FirstOrDefault().HomeOwnership,
                                    Married = s.FirstOrDefault().Married,
                                    FamMem = s.FirstOrDefault().FamMem,
                                    HasTradeLicense = s.FirstOrDefault().HasTradeLicense,
                                    TradeLicenseNo = s.FirstOrDefault().TradeLicenseNo,
                                    TradeLicenseExpireDate = s.FirstOrDefault().TradeLicenseExpireDate,
                                    CompanyBankAccNo = s.FirstOrDefault().CompanyBankAccNo,
                                    CompanyBankAccName = s.FirstOrDefault().CompanyBankAccName,
                                    AnnualTotalIncome = s.FirstOrDefault().AnnualTotalIncome,
                                    DateOfBirth = s.FirstOrDefault().DateOfBirth,
                                    NidNo = s.FirstOrDefault().NidNo,
                                    OthersIncome = s.FirstOrDefault().OthersIncome,
                                    ReqTenorMonth = s.FirstOrDefault().ReqTenorMonth,
                                    ResidenceLocation = s.FirstOrDefault().ResidenceLocation,
                                    HasPreviousLoan = s.FirstOrDefault().HasPreviousLoan,
                                    BankStatementUrl = s.FirstOrDefault().BankStatementUrl,
                                    Comments = s.FirstOrDefault().Comments,
                                    BusinessStartDate = s.FirstOrDefault().BusinessStartDate,
                                    NidImageUrl = s.FirstOrDefault().NidImageUrl,
                                    NidBackImageUrl = s.FirstOrDefault().NidBackImageUrl,
                                    TinImageUrl = s.FirstOrDefault().TinImageUrl,
                                    CibUploadedFormUrl = s.FirstOrDefault().CibUploadedFormUrl,
                                    PresentAddHouseNo = s.FirstOrDefault().PresentAddHouseNo,
                                    PresentAddRoadNo = s.FirstOrDefault().PresentAddRoadNo,
                                    PresentAddRoadName = s.FirstOrDefault().PresentAddRoadName,
                                    PresentAddArea = s.FirstOrDefault().PresentAddArea,
                                    PresentAddPostOffice = s.FirstOrDefault().PresentAddPostOffice,
                                    PresentAddDistrictId = s.FirstOrDefault().PresentAddDistrictId,
                                    PresentAddThanaId = s.FirstOrDefault().PresentAddThanaId,
                                    HouseOwner = s.FirstOrDefault().HouseOwner,
                                    IsOwner = s.FirstOrDefault().IsOwner,
                                    DurationOfLiving = s.FirstOrDefault().DurationOfLiving,
                                    PermanentAddHouseNo = s.FirstOrDefault().PermanentAddHouseNo,
                                    PermanentAddRoadNo = s.FirstOrDefault().PermanentAddRoadNo,
                                    PermanentAddRoadName = s.FirstOrDefault().PermanentAddRoadName,
                                    PermanentAddArea = s.FirstOrDefault().PermanentAddArea,
                                    PermanentAddPostOffice = s.FirstOrDefault().PermanentAddPostOffice,
                                    PermanentAddDistrictId = s.FirstOrDefault().PermanentAddDistrictId,
                                    PermanentAddThanaId = s.FirstOrDefault().PermanentAddThanaId,
                                    FatherName = s.FirstOrDefault().FatherName,
                                    MotherName = s.FirstOrDefault().MotherName,
                                    SpouseName = s.FirstOrDefault().SpouseName,
                                    IsLoanDue = s.FirstOrDefault().IsLoanDue,
                                    Experience = s.FirstOrDefault().Experience,
                                    Occupation = s.FirstOrDefault().Occupation,
                                    PermanentAddHouseOwnerName = s.FirstOrDefault().PermanentAddHouseOwnerName,
                                    CreditCardNumber = s.FirstOrDefault().CreditCardNumber,
                                    ApplicantPhotoUrl = s.FirstOrDefault().ApplicantPhotoUrl,
                                    LoanRepayType = s.FirstOrDefault().LoanRepayType,
                                    OtherIncomeSource = s.FirstOrDefault().OtherIncomeSource,
                                    CourierWithLoanSurvey = s.Where(w => w.CourierWithLoanSurvey.LoanSurveyId == s.FirstOrDefault().CourierWithLoanSurvey.LoanSurveyId)
                                    .GroupBy(g => g.CourierWithLoanSurvey.CouriersWithLoanSurveyId)
                                    .Select(r => new
                                    {
                                        CouriersWithLoanSurveyId = r.FirstOrDefault().CourierWithLoanSurvey.CouriersWithLoanSurveyId,
                                        CourierId = r.FirstOrDefault().CourierWithLoanSurvey.CourierId,
                                        CourierName = r.FirstOrDefault().CourierWithLoanSurvey.CourierName
                                    }).ToList(),
                                    ApplicantFamMem = s.Where(w => w.ApplicantFamMem.LoanSurveyId == s.FirstOrDefault().LoanSurveyId)
                                    .GroupBy(g => g.ApplicantFamMem.FamilyMemberId)
                                    .Select(r => new
                                    {
                                        FamilyMemberId = r.FirstOrDefault().ApplicantFamMem.FamilyMemberId,
                                        CourierUserId = r.FirstOrDefault().ApplicantFamMem.CourierUserId,
                                        FamilyMemberName = r.FirstOrDefault().ApplicantFamMem.FamilyMemberName,
                                        Relation = r.FirstOrDefault().ApplicantFamMem.Relation,
                                        Age = r.FirstOrDefault().ApplicantFamMem.Age,
                                        Education = r.FirstOrDefault().ApplicantFamMem.Education,
                                        IsMarried = r.FirstOrDefault().ApplicantFamMem.IsMarried,
                                        Occupation = r.FirstOrDefault().ApplicantFamMem.Occupation
                                    }).ToList()
                                });
            return returnLoanSurveyData;
        }

        public async Task<dynamic> GetCibFormCourierUserInfo(int CourierUserId)
        {
            var userInfo = await (from user in _sqlServerContext.CourierUsers
                                  join loan in _sqlServerContext.LoanSurvey
                                  on user.CourierUserId equals loan.CourierUserId into userLoanJoin
                                  from subLoan in userLoanJoin.DefaultIfEmpty()
                                  where user.CourierUserId == CourierUserId
                                  select new
                                  {
                                      CourierUserId = user.CourierUserId,
                                      CompanyName = user.CompanyName,
                                      UserName = user.UserName,
                                      Gender = user.Gender,
                                      Mobile = user.Mobile,
                                      TinNumber = userLoanJoin == null ? "" : (subLoan.TinNumber ?? ""),
                                      DateOfBirth = subLoan.DateOfBirth,
                                      NidNo = userLoanJoin == null ? "" : (subLoan.NidNo ?? ""),
                                      AmountOfLoanApplied = userLoanJoin == null ? 0 : (subLoan.InterestedAmount == 0 ? 0 : subLoan.InterestedAmount)
                                  }).OrderByDescending(o => o.CourierUserId).ToListAsync();

            var returnedUser = userInfo.GroupBy(g => g.CourierUserId).Select(s => new
            {
                CourierUserId = s.LastOrDefault().CourierUserId,
                CompanyName = s.LastOrDefault().CompanyName,
                UserName = s.LastOrDefault().UserName,
                Gender = s.LastOrDefault().Gender,
                Mobile = s.LastOrDefault().Mobile,
                TinNumber = s.LastOrDefault().TinNumber,
                DateOfBirth = s.LastOrDefault().DateOfBirth,
                NidNo = s.LastOrDefault().NidNo,
                AmountOfLoanApplied = s.LastOrDefault().AmountOfLoanApplied
            });
            return returnedUser;
        }

        public async Task<int> UpdateLoanSurvey(int loanSurveyId, LoanSurvey loanSurvey)
        {
            var entity = await _sqlServerContext.LoanSurvey.FirstOrDefaultAsync(l => l.LoanSurveyId == loanSurveyId);

            if (entity != null)
            {
                entity.MerchantName = loanSurvey.MerchantName;
                entity.Gender = loanSurvey.Gender;
                entity.TradeLicenseImageUrl = loanSurvey.TradeLicenseImageUrl;
                entity.InterestedAmount = loanSurvey.InterestedAmount;
                entity.TransactionAmount = loanSurvey.TransactionAmount;
                entity.IsBankAccount = loanSurvey.IsBankAccount;
                entity.IsLocalShop = loanSurvey.IsLocalShop;
                entity.ApplicationDate = loanSurvey.ApplicationDate;
                entity.MonthlyTotalCodAmount = loanSurvey.MonthlyTotalCodAmount;
                entity.GuarantorMobile = loanSurvey.GuarantorMobile;
                entity.GuarantorName = loanSurvey.GuarantorName;
                entity.MonthlyTotalAverageSale = loanSurvey.MonthlyTotalAverageSale;
                entity.LoanAmount = loanSurvey.LoanAmount;
                entity.BankName = loanSurvey.BankName;
                entity.Age = loanSurvey.Age;
                entity.BasketValue = loanSurvey.BasketValue;
                entity.CardHolder = loanSurvey.CardHolder;
                entity.CardLimit = loanSurvey.CardLimit;
                entity.LoanEmi = loanSurvey.LoanEmi;
                entity.HasCreditCard = loanSurvey.HasCreditCard;
                entity.HasTin = loanSurvey.HasTin;
                entity.EduLevel = loanSurvey.EduLevel;
                entity.RepayType = loanSurvey.RepayType;
                entity.MonthlyOrder = loanSurvey.MonthlyOrder;
                entity.MonthlyExp = loanSurvey.MonthlyExp;
                entity.Recommend = loanSurvey.Recommend;
                entity.RelationMarchent = loanSurvey.RelationMarchent;
                entity.ShopOwnership = loanSurvey.ShopOwnership;
                entity.TinNumber = loanSurvey.TinNumber;
                entity.HomeOwnership = loanSurvey.HomeOwnership;
                entity.Married = loanSurvey.Married;
                entity.FamMem = loanSurvey.FamMem;
                entity.HasTradeLicense = loanSurvey.HasTradeLicense;
                entity.TradeLicenseNo = loanSurvey.TradeLicenseNo;
                entity.TradeLicenseExpireDate = loanSurvey.TradeLicenseExpireDate;
                entity.CompanyBankAccNo = loanSurvey.CompanyBankAccNo;
                entity.CompanyBankAccName = loanSurvey.CompanyBankAccName;
                entity.AnnualTotalIncome = loanSurvey.AnnualTotalIncome;
                entity.DateOfBirth = loanSurvey.DateOfBirth;
                entity.NidNo = loanSurvey.NidNo;
                entity.OthersIncome = loanSurvey.OthersIncome;
                entity.ReqTenorMonth = loanSurvey.ReqTenorMonth;
                entity.ResidenceLocation = loanSurvey.ResidenceLocation;
                entity.HasPreviousLoan = loanSurvey.HasPreviousLoan;
                entity.LenderType = loanSurvey.LenderType;
                entity.BankStatementUrl = loanSurvey.BankStatementUrl;
                entity.BusinessStartDate = loanSurvey.BusinessStartDate;
                entity.NidImageUrl = loanSurvey.NidImageUrl;
                entity.NidBackImageUrl = loanSurvey.NidBackImageUrl;
                entity.TinImageUrl = loanSurvey.TinImageUrl;
                entity.PresentAddHouseNo = loanSurvey.PresentAddHouseNo;
                entity.PresentAddRoadNo = loanSurvey.PresentAddRoadNo;
                entity.PresentAddRoadName = loanSurvey.PresentAddRoadName;
                entity.PresentAddArea = loanSurvey.PresentAddArea;
                entity.PresentAddPostOffice = loanSurvey.PresentAddPostOffice;
                entity.PresentAddDistrictId = loanSurvey.PresentAddDistrictId;
                entity.PresentAddThanaId = loanSurvey.PresentAddThanaId;
                entity.HouseOwner = loanSurvey.HouseOwner;
                entity.IsOwner = loanSurvey.IsOwner;
                entity.DurationOfLiving = loanSurvey.DurationOfLiving;
                entity.PermanentAddHouseNo = loanSurvey.PermanentAddHouseNo;
                entity.PermanentAddRoadNo = loanSurvey.PermanentAddRoadNo;
                entity.PermanentAddRoadName = loanSurvey.PermanentAddRoadName;
                entity.PermanentAddArea = loanSurvey.PermanentAddArea;
                entity.PermanentAddPostOffice = loanSurvey.PermanentAddPostOffice;
                entity.PermanentAddDistrictId = loanSurvey.PermanentAddDistrictId;
                entity.PermanentAddThanaId = loanSurvey.PermanentAddThanaId;
                entity.FatherName = loanSurvey.FatherName;
                entity.MotherName = loanSurvey.MotherName;
                entity.SpouseName = loanSurvey.SpouseName;
                entity.IsLoanDue = loanSurvey.IsLoanDue;
                entity.Experience = loanSurvey.Experience;
                entity.Occupation = loanSurvey.Occupation;
                entity.PermanentAddHouseOwnerName = loanSurvey.PermanentAddHouseOwnerName;
                entity.CreditCardNumber = loanSurvey.CreditCardNumber;
                entity.ApplicantPhotoUrl = loanSurvey.ApplicantPhotoUrl;
                entity.LoanRepayType = loanSurvey.LoanRepayType;
                entity.OtherIncomeSource = loanSurvey.OtherIncomeSource;

                _sqlServerContext.LoanSurvey.Update(entity);
                return await _sqlServerContext.SaveChangesAsync();
            }

            return 0;
        }

        public async Task<int> UpdateCourierWithLoanSurvey(int loanSurveyId, List<CouriersWithLoanSurvey> couriersWithLoanSurveys)
        {
            var entity = await _sqlServerContext.CouriersWithLoanSurvey.Where(c => c.LoanSurveyId == loanSurveyId).ToListAsync();
            _sqlServerContext.CouriersWithLoanSurvey.RemoveRange(entity);
            await _sqlServerContext.SaveChangesAsync();

            _sqlServerContext.CouriersWithLoanSurvey.AddRange(couriersWithLoanSurveys);
            await _sqlServerContext.SaveChangesAsync();

            return 1;
        }

        public async Task<IEnumerable<DanaToDtMatchColumnViewModel>> GetDanaColumnValue(int CourierUserId)
        {
            int[] confirmedOrder = new int[] { 0, 2, 29 };
            var data = await (from courierUser in _sqlServerContext.CourierUsers
                              join orders in _sqlServerContext.CourierOrders
                              on courierUser.CourierUserId equals orders.MerchantId into leftJoin1

                              from j1 in leftJoin1.DefaultIfEmpty()

                              //join category in _sqlServerContext.Category
                              //on courierUser.CategoryId equals category.CategoryId into leftJoin2

                              //from j2 in leftJoin2.DefaultIfEmpty()

                              //join subcategory in _sqlServerContext.SubCategory
                              //on courierUser.SubCategoryId equals subcategory.SubCategoryId into leftJoin3

                              //from j3 in leftJoin3.DefaultIfEmpty()

                              where courierUser.CourierUserId.Equals(CourierUserId)
                              && j1.OrderDate >= DateTime.Now.AddMonths(-6)
                              && j1.OrderDate < DateTime.Now.AddDays(1)
                              && !confirmedOrder.Contains(j1.Status)
                              select new
                              {
                                  MerchantId = courierUser.CourierUserId,
                                  ActualPackagePrice = j1.ActualPackagePrice,
                                  CollectionAmount = j1.CollectionAmount,
                                  CourierOrdersId = j1 == null ? "" : (j1.CourierOrdersId ?? ""),
                                  DeliveryCharge = j1.DeliveryCharge,
                                  CodCharge = j1.CodCharge,
                                  JoinDate = courierUser.JoinDate,
                                  //BusinessType = j2 == null ? "" : (j2.CategoryNameEng ?? ""),
                                  //TypeOfProductSale = j3 == null ? "" : (j3.SubCategoryNameEng ?? ""),
                                  Review = courierUser.MerchantReview,
                                  Recommendation = courierUser.Recommendation
                              }).ToListAsync();

            var dataSixMonths = data.GroupBy(g => g.MerchantId).Select(s => new
            {
                AvgBasketValue = s.Sum(m => m.ActualPackagePrice == 0 ? m.CollectionAmount : m.ActualPackagePrice) / s.Count(),
                CommissionShare = s.Sum(d => d.DeliveryCharge) / s.Count(),
                RelationShip = (DateTime.Now - s.FirstOrDefault().JoinDate).Days,
                //BusinessType = s.FirstOrDefault().BusinessType,
                //TypeOfProductSale = s.FirstOrDefault().TypeOfProductSale,
                MonthlyAvgStockAmt = s.Count() / 6,
                ReviewLastSixMonths = s.FirstOrDefault().Review,
                Recommendation = s.FirstOrDefault().Recommendation
            });

            var dataOneYear = await (from courierUser in _sqlServerContext.CourierUsers
                               join orders in _sqlServerContext.CourierOrders
                               on courierUser.CourierUserId equals orders.MerchantId into leftJoin1

                               from j1 in leftJoin1.DefaultIfEmpty()

                               where courierUser.CourierUserId.Equals(CourierUserId)
                               && j1.OrderDate >= DateTime.Now.AddMonths(-12)
                               && j1.OrderDate < DateTime.Now.AddDays(1)
                               && !confirmedOrder.Contains(j1.Status)
                               select new
                               {
                                   MerchantId = courierUser.CourierUserId,
                                   CourierOrdersId = j1 == null ? "" : (j1.CourierOrdersId ?? ""),
                                   CollectionAmount = j1.CollectionAmount,
                                   ActualPackagePrice = j1.ActualPackagePrice
                               }).GroupBy(g => g.MerchantId).Select(s => new
                               {
                                   TotalOrder = s.Count(),
                                   MonthlyAvgOrder = s.Count() / 12,
                                   TotalSales = s.Sum(m => m.ActualPackagePrice == 0 ? m.CollectionAmount : m.ActualPackagePrice),
                                   AvgMonthlySales = s.Sum(m => m.ActualPackagePrice == 0 ? m.CollectionAmount : m.ActualPackagePrice) / 12
                               }).ToListAsync();

            List<DanaToDtMatchColumnViewModel> matchColumnNameResult = new List<DanaToDtMatchColumnViewModel>();

            if (dataSixMonths.Count() == 0 || dataOneYear.Count() == 0)
            {
                return matchColumnNameResult;
            }

            matchColumnNameResult.Add(new DanaToDtMatchColumnViewModel
            {
                DanaProperty = "basket_value",
                DtProperty = "",
                Value = dataSixMonths.FirstOrDefault().AvgBasketValue.ToString("0.00")
            });
            matchColumnNameResult.Add(new DanaToDtMatchColumnViewModel
            {
                DanaProperty = "last_review",
                DtProperty = "",
                Value = dataSixMonths.FirstOrDefault().ReviewLastSixMonths.ToString()
            });
            matchColumnNameResult.Add(new DanaToDtMatchColumnViewModel
            {
                DanaProperty = "recommend",
                DtProperty = "",
                Value = dataSixMonths.FirstOrDefault().Recommendation.ToString()
            });
            matchColumnNameResult.Add(new DanaToDtMatchColumnViewModel
            {
                DanaProperty = "commission_share",
                DtProperty = "",
                Value = dataSixMonths.FirstOrDefault().CommissionShare.ToString("0.00")
            });
            matchColumnNameResult.Add(new DanaToDtMatchColumnViewModel
            {
                DanaProperty = "relation_marchent",
                DtProperty = "",
                Value = dataSixMonths.FirstOrDefault().RelationShip.ToString()
            });
            matchColumnNameResult.Add(new DanaToDtMatchColumnViewModel
            {
                DanaProperty = "last_year_sale",
                DtProperty = "",
                Value = dataOneYear.FirstOrDefault().TotalSales.ToString("0.00")
            });
            matchColumnNameResult.Add(new DanaToDtMatchColumnViewModel
            {
                DanaProperty = "avg_monthly_sale",
                DtProperty = "",
                Value = dataOneYear.FirstOrDefault().AvgMonthlySales.ToString("0.00")
            });
            matchColumnNameResult.Add(new DanaToDtMatchColumnViewModel
            {
                DanaProperty = "monthly_order",
                DtProperty = "",
                Value = dataOneYear.FirstOrDefault().MonthlyAvgOrder.ToString()
            });
            matchColumnNameResult.Add(new DanaToDtMatchColumnViewModel
            {
                DanaProperty = "average_inventory",
                DtProperty = "",
                Value = dataSixMonths.FirstOrDefault().MonthlyAvgStockAmt.ToString()
            });

            return matchColumnNameResult;
        }

        public async Task<int> UpdateLoanApplicantsComment(int LoanSurveyId, string Comments)
        {
            var loanApplicant = await _sqlServerContext.LoanSurvey.FirstOrDefaultAsync(x => x.LoanSurveyId == LoanSurveyId);

            if (loanApplicant != null)
            {
                loanApplicant.Comments = Comments;
                _sqlServerContext.LoanSurvey.Update(loanApplicant);
                return await _sqlServerContext.SaveChangesAsync();
            }

            return 0;
        }

        public async Task<CibFormData> AddCibFormData(CibFormData formData)
        {
            //var entity = await _sqlServerContext.CibFormData.FirstOrDefaultAsync(data => data.CourierUserId.Equals(formData.CourierUserId));

            //if(entity != null)
            //{
            //    _sqlServerContext.CibFormData.Remove(entity);
            //    await _sqlServerContext.SaveChangesAsync();
            //}

            _sqlServerContext.CibFormData.Add(formData);
            await _sqlServerContext.SaveChangesAsync();

            return formData;
        }

        public async Task<List<CibFormOtherLoan>> AddCibFormOtherLoan (int cibFormDataId, List<CibFormOtherLoan> otherLoans)
        {
            //var entity = await _sqlServerContext.CibFormOtherLoan.FirstOrDefaultAsync(data => data.CibFormDataId.Equals(cibFormDataId));

            //if (entity != null)
            //{
            //    _sqlServerContext.CibFormOtherLoan.RemoveRange(otherLoans);
            //    await _sqlServerContext.SaveChangesAsync();
            //}

            _sqlServerContext.CibFormOtherLoan.AddRange(otherLoans);
            await _sqlServerContext.SaveChangesAsync();

            return otherLoans;
        }

        public async Task<List<LoanApplicantFamilyMember>> AddLoanApplicantFamilyMember(int LoanSurveyId, List<LoanApplicantFamilyMember> LoanApplicantFamilyMember)
        {
            var entity = await _sqlServerContext.LoanApplicantFamilyMember.Where(data => data.LoanSurveyId.Equals(LoanSurveyId)).ToListAsync();

            if (entity != null)
            {
                _sqlServerContext.LoanApplicantFamilyMember.RemoveRange(entity);
                await _sqlServerContext.SaveChangesAsync();
            }

            _sqlServerContext.LoanApplicantFamilyMember.AddRange(LoanApplicantFamilyMember);
            await _sqlServerContext.SaveChangesAsync();

            return LoanApplicantFamilyMember;
        }

        public async Task<List<LoanApplicantFamilyMember>> UpdateLoanApplicantFamilyMember(int LoanSurveyId, List<LoanApplicantFamilyMember> LoanApplicantFamilyMember)
        {
            var entity = await _sqlServerContext.LoanApplicantFamilyMember.Where(data => data.LoanSurveyId.Equals(LoanSurveyId)).ToListAsync();

            if (entity != null)
            {
                _sqlServerContext.LoanApplicantFamilyMember.RemoveRange(entity);
                await _sqlServerContext.SaveChangesAsync();
            }

            _sqlServerContext.LoanApplicantFamilyMember.AddRange(LoanApplicantFamilyMember);
            await _sqlServerContext.SaveChangesAsync();

            return LoanApplicantFamilyMember;
        }

        public async Task<BorrowerBusinessDataViewModel> GetBorrowerBusinessData(int courierUserId)
        {
            var data = await (from borrowerData in _sqlServerContext.BorrowerBusinessData.AsNoTracking()
                              join user in _sqlServerContext.CourierUsers.AsNoTracking()
                              on borrowerData.CourierUserId equals user.CourierUserId into groupJoin
                              from subUserGroup in groupJoin.DefaultIfEmpty()
                              where borrowerData.CourierUserId.Equals(courierUserId)
                              select new BorrowerBusinessDataViewModel
                              {
                                  CourierUserId = borrowerData.CourierUserId,
                                  NoOfMaxProductSellingMonth = borrowerData.NoOfMaxProductSellingMonth,
                                  AvgAmountOfMaxSellingMonth = borrowerData.AvgAmountOfMaxSellingMonth,
                                  NoOfRegularProductSellingMonth = borrowerData.NoOfRegularProductSellingMonth,
                                  AvgAmountOfRegularSellingMonth = borrowerData.AvgAmountOfRegularSellingMonth,
                                  NoOfLowestProductSellingMonth = borrowerData.NoOfLowestProductSellingMonth,
                                  AvgAmountOfLowestSellingMonth = borrowerData.AvgAmountOfLowestSellingMonth,
                                  NoOfMaxProductPurchasedMonth = borrowerData.NoOfMaxProductPurchasedMonth,
                                  AvgAmountOfMaxPurchasedMonth = borrowerData.AvgAmountOfMaxPurchasedMonth,
                                  NoOfRegularProductPurchasedMonth = borrowerData.NoOfRegularProductPurchasedMonth,
                                  AvgAmountOfRegularPurchasedMonth = borrowerData.AvgAmountOfRegularPurchasedMonth,
                                  NoOfLowestProductPurchasedMonth = borrowerData.NoOfLowestProductPurchasedMonth,
                                  AvgAmountOfLowestPurchasedMonth = borrowerData.AvgAmountOfLowestPurchasedMonth,
                                  BusinessRent = borrowerData.BusinessRent,
                                  SalaryOfEmployee = borrowerData.SalaryOfEmployee,
                                  TransportationCost = borrowerData.TransportationCost,
                                  UtilityCost = borrowerData.UtilityCost,
                                  RepairCost = borrowerData.RepairCost,
                                  PackagingCost = borrowerData.PackagingCost,
                                  BusinessTax = borrowerData.BusinessTax,
                                  OthersExpense = borrowerData.OthersExpense,
                                  DebotrsOwingAmount = borrowerData.DebotrsOwingAmount,
                                  DebotrsOwingAmountPercentageOnSale = borrowerData.DebotrsOwingAmountPercentageOnSale,
                                  CreditorsOwingAmount = borrowerData.CreditorsOwingAmount,
                                  CreditorsOwingAmountPercentageOnPurchase = borrowerData.CreditorsOwingAmountPercentageOnPurchase,
                                  BankShortCode = borrowerData.BankShortCode,
                                  AccountType = borrowerData.AccountType,
                                  CashInHand = borrowerData.CashInHand,
                                  CashAtBank = borrowerData.CashAtBank,
                                  ChecqueBookPhotoUrl = borrowerData.ChecqueBookPhotoUrl,
                                  AccountName = subUserGroup == null ? "" : subUserGroup.AccountName,
                                  AccountNumber = subUserGroup == null ? "" : subUserGroup.AccountNumber,
                                  RoutingNumber = subUserGroup == null ? "" : subUserGroup.RoutingNumber,
                                  MobileBankingAccountType = "Bkash",
                                  BkashNumber = subUserGroup == null ? "" : subUserGroup.BkashNumber
                              }).FirstOrDefaultAsync();
            return data;
        }

        public async Task<BorrowerBusinessData> AddBorrowerBusinessData(BorrowerBusinessData borrowerBusinessData)
        {
            await _sqlServerContext.BorrowerBusinessData.AddAsync(borrowerBusinessData);
            await _sqlServerContext.SaveChangesAsync();
            return borrowerBusinessData;
        }

        public async Task<BorrowerBusinessDataViewModel> UpdateBorrowerBusinessData(int courierUserId, BorrowerBusinessDataViewModel borrowerBusinessData)
        {
            var entity = await _sqlServerContext.BorrowerBusinessData.FirstOrDefaultAsync(b => b.CourierUserId.Equals(courierUserId));

            if (entity != null)
            {
                entity.NoOfMaxProductSellingMonth = borrowerBusinessData.NoOfMaxProductSellingMonth;
                entity.AvgAmountOfMaxSellingMonth = borrowerBusinessData.AvgAmountOfMaxSellingMonth;
                entity.NoOfRegularProductSellingMonth = borrowerBusinessData.NoOfRegularProductSellingMonth;
                entity.AvgAmountOfRegularSellingMonth = borrowerBusinessData.AvgAmountOfRegularSellingMonth;
                entity.NoOfLowestProductSellingMonth = borrowerBusinessData.NoOfLowestProductSellingMonth;
                entity.AvgAmountOfLowestSellingMonth = borrowerBusinessData.AvgAmountOfLowestSellingMonth;
                entity.NoOfMaxProductPurchasedMonth = borrowerBusinessData.NoOfMaxProductPurchasedMonth;
                entity.AvgAmountOfMaxPurchasedMonth = borrowerBusinessData.AvgAmountOfMaxPurchasedMonth;
                entity.NoOfRegularProductPurchasedMonth = borrowerBusinessData.NoOfRegularProductPurchasedMonth;
                entity.AvgAmountOfRegularPurchasedMonth = borrowerBusinessData.AvgAmountOfRegularPurchasedMonth;
                entity.NoOfLowestProductPurchasedMonth = borrowerBusinessData.NoOfLowestProductPurchasedMonth;
                entity.AvgAmountOfLowestPurchasedMonth = borrowerBusinessData.AvgAmountOfLowestPurchasedMonth;
                entity.BusinessRent = borrowerBusinessData.BusinessRent;
                entity.SalaryOfEmployee = borrowerBusinessData.SalaryOfEmployee;
                entity.TransportationCost = borrowerBusinessData.TransportationCost;
                entity.UtilityCost = borrowerBusinessData.UtilityCost;
                entity.RepairCost = borrowerBusinessData.RepairCost;
                entity.PackagingCost = borrowerBusinessData.PackagingCost;
                entity.BusinessTax = borrowerBusinessData.BusinessTax;
                entity.OthersExpense = borrowerBusinessData.OthersExpense;
                entity.DebotrsOwingAmount = borrowerBusinessData.DebotrsOwingAmount;
                entity.DebotrsOwingAmountPercentageOnSale = borrowerBusinessData.DebotrsOwingAmountPercentageOnSale;
                entity.CreditorsOwingAmount = borrowerBusinessData.CreditorsOwingAmount;
                entity.CreditorsOwingAmountPercentageOnPurchase = borrowerBusinessData.CreditorsOwingAmountPercentageOnPurchase;
                entity.BankShortCode = borrowerBusinessData.BankShortCode;
                entity.AccountType = borrowerBusinessData.AccountType;
                entity.CashInHand = borrowerBusinessData.CashInHand;
                entity.CashAtBank = borrowerBusinessData.CashAtBank;
                entity.ChecqueBookPhotoUrl = borrowerBusinessData.ChecqueBookPhotoUrl;

                _sqlServerContext.BorrowerBusinessData.Update(entity);
                await _sqlServerContext.SaveChangesAsync();
            }

            return borrowerBusinessData;
        }

        public async Task<LoanStatusViewModel> UpdateLoanStatus(LoanStatusViewModel loanStatus, int lenderUserId)
        {
            LoanStatus status = new LoanStatus();
            status.LoanSurveyId = loanStatus.LoanApplicationId;
            status.StatusCode = loanStatus.StatusCode;
            status.Status = loanStatus.Status;
            status.Comment = loanStatus.Comment;
            status.LenderUserId = lenderUserId;


            _sqlServerContext.LoanStatus.Add(status);
            await _sqlServerContext.SaveChangesAsync();
            return loanStatus;
        }

        public async Task<LoanDisbursement> UpdateLoanDisbursementDT(LoanDisbursement loanDisbursement)
        {
            if (loanDisbursement.DisbursementId == 0)
            {
                _sqlServerContext.LoanDisbursement.Add(loanDisbursement);
                await _sqlServerContext.SaveChangesAsync();
            }
            else
            {
                _sqlServerContext.LoanDisbursement.Update(loanDisbursement);
                await _sqlServerContext.SaveChangesAsync();
            }

            return loanDisbursement;
        }

        public async Task<LoanDisbursementHistoryViewModel> UpdateLoanDisbursementHistoryDT(LoanDisbursementHistoryViewModel loanDisbursementHistory, int lenderUserId)
        {
            LoanDisbursementHistory DisbursementHistory = new LoanDisbursementHistory();
            DisbursementHistory.RefTransactionID = loanDisbursementHistory.RefTransactionID;
            DisbursementHistory.PaymentDate = loanDisbursementHistory.PaymentDate;
            DisbursementHistory.PaymentAmount = loanDisbursementHistory.PaymentAmount;
            DisbursementHistory.PaymentMethod = loanDisbursementHistory.PaymentMethod;
            DisbursementHistory.PaymentTransactionID = loanDisbursementHistory.PaymentTransactionID;
            DisbursementHistory.LenderUserId = lenderUserId;

            _sqlServerContext.LoanDisbursementHistory.Add(DisbursementHistory);
            await _sqlServerContext.SaveChangesAsync();

            return loanDisbursementHistory;
        }

        public async Task<RepaymentStatusViewModel> UpdateLoanRepaymentStatusDT(RepaymentStatusViewModel repaymentStatus, int lenderUserId)
        {
            RepaymentStatus repayment = new RepaymentStatus()
            {
                RepaymentDate = repaymentStatus.RepaymentDate,
                Amount = repaymentStatus.Amount,
                PaymentMethod = repaymentStatus.PaymentMethod,
                RefTransactionID = repaymentStatus.RefTransactionID,
                PaymentTransactionID = repaymentStatus.PaymentTransactionID,
                StatusCode = repaymentStatus.StatusCode,
                Status = repaymentStatus.Status,
                LenderUserId = lenderUserId
            };

            _sqlServerContext.RepaymentStatus.Add(repayment);
            await _sqlServerContext.SaveChangesAsync();
            return repaymentStatus;
        }

        public async Task<LoanStatusViewModel> GetLoanStatus(int LoanSurveyId, int CourierUserId)
        {
            var entity = await _sqlServerContext.LoanStatus.FirstOrDefaultAsync(l => l.LoanSurveyId.Equals(LoanSurveyId));

            if (entity != null)
            {
                var loanStatus = new LoanStatusViewModel
                {
                    LoanApplicationId = entity.LoanSurveyId,
                    Status = entity.Status,
                    Comment = entity.Comment
                };
                return loanStatus;
            }

            return null;
        }

        private int ConvertBengaliNumberToEnglishNumber(string bengaliNumbers)
        {
            if (bengaliNumbers != "")
            {
                char[] bengaliNumberCharArr = bengaliNumbers.Split('-')[1].ToCharArray();
                string[] convertedDigitArr = new string[bengaliNumberCharArr.Length];
                for (int i = 0; i < bengaliNumberCharArr.Length; i++)
                {
                    char digit = (char)(bengaliNumberCharArr[i] - '১' + '1');
                    convertedDigitArr[i] = digit.ToString();
                }
                int numberToReturn = Convert.ToInt32(string.Join("", convertedDigitArr));
                return numberToReturn;
            }
            else
            {
                return 0;
            }
        }

        private byte[] GetImage(string url)
        {
            Stream stream = null;
            byte[] buf;
            WebProxy myProxy = new WebProxy();
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);

            HttpWebResponse response = (HttpWebResponse)req.GetResponse();
            stream = response.GetResponseStream();

            using (BinaryReader br = new BinaryReader(stream))
            {
                int len = (int)(response.ContentLength);
                buf = br.ReadBytes(len);
                br.Close();
            }

            stream.Close();
            response.Close();

            return (buf);
        }

        public String ConvertImageURLToBase64(String url)
        {
            StringBuilder _sb = new StringBuilder();

            Byte[] _byte = this.GetImage(url);

            _sb.Append(Convert.ToBase64String(_byte, 0, _byte.Length));

            return _sb.ToString();
        }

        public async Task<dynamic> GetLoanApplicationInformationShakti(int courierUserId)
        {
            var loanApplication = await _sqlServerContext.LoanSurvey.Where(l => l.CourierUserId.Equals(courierUserId)).OrderByDescending(o => o.LoanSurveyId).FirstOrDefaultAsync();

            var monthlyOrder = await (from orders in _sqlServerContext.CourierOrders
                                                 where orders.MerchantId == courierUserId
                                                 && orders.IsDownloaded == true
                                                 group orders by new
                                                 {
                                                     Month = orders.OrderDate.Month,
                                                     Year = orders.OrderDate.Year
                                                 } into orderDateGroup
                                                 select new
                                                 {
                                                     Month = orderDateGroup.Key.Month,
                                                     MonthName = "",
                                                     Year = orderDateGroup.Key.Year,
                                                     TotalCollectionAmount = orderDateGroup.Sum(o => o.CollectionAmount)
                                                 }).OrderByDescending(o => o.Year).ThenByDescending(o => o.Month).ToListAsync();

            List<MonthlyOrderTransaction> monthlyOrderTransaction = new List<MonthlyOrderTransaction>();
            foreach (var item in monthlyOrder)
            {
                string month = "";
                if(item.Month.ToString().Length < 2)
                {
                    month = String.Concat("0", item.Month);
                }
                else
                {
                    month = item.Month.ToString();
                }
                string yymm = String.Concat(month, item.Year);

                monthlyOrderTransaction.Add(new MonthlyOrderTransaction
                {
                    YYYYMM = yymm,
                    TRANSACTIONAMOUNT = item.TotalCollectionAmount
                });
            }

            decimal avgTransactionAmountPerMonth = 0;
            if (monthlyOrderTransaction.Count() != 0)
            {
                avgTransactionAmountPerMonth = monthlyOrderTransaction.Average(a => a.TRANSACTIONAMOUNT);
            }

            var data = await (from _loan in _sqlServerContext.LoanSurvey
                              join _merchant in _sqlServerContext.CourierUsers
                              on _loan.CourierUserId equals _merchant.CourierUserId

                              join _presentThana in _sqlServerContext.Districts
                              on _loan.PresentAddThanaId equals _presentThana.DistrictId into presentAddThanaGroupJoin
                              from subPresentThana in presentAddThanaGroupJoin.DefaultIfEmpty()

                              join _presentDistrict in _sqlServerContext.Districts
                              on _loan.PresentAddDistrictId equals _presentDistrict.DistrictId into presentAddDistrictGroupJoin
                              from subPresentDistrict in presentAddDistrictGroupJoin.DefaultIfEmpty()

                              join _permanentThana in _sqlServerContext.Districts
                              on _loan.PermanentAddThanaId equals _permanentThana.DistrictId into permanentAddThanaGroupJoin
                              from subPermanentThana in permanentAddThanaGroupJoin.DefaultIfEmpty()

                              join _permanentDistrict in _sqlServerContext.Districts
                              on _loan.PermanentAddDistrictId equals _permanentDistrict.DistrictId into permanentAddDistrictGroupJoin
                              from subPermanentDistrict in permanentAddDistrictGroupJoin.DefaultIfEmpty()

                              join _businessData in _sqlServerContext.BorrowerBusinessData
                              on _loan.CourierUserId equals _businessData.CourierUserId into businessDataGroupJoin
                              from subBusinessData in businessDataGroupJoin.DefaultIfEmpty()

                              join _courierUser in _sqlServerContext.CourierUsers
                              on _loan.CourierUserId equals _courierUser.CourierUserId

                              join _familyMember in _sqlServerContext.LoanApplicantFamilyMember
                              on _loan.LoanSurveyId equals _familyMember.LoanSurveyId into familyMemberGroupJoin
                              from subFamilyMember in familyMemberGroupJoin.DefaultIfEmpty()

                              where _loan.CourierUserId.Equals(courierUserId)
                              select new
                              {
                                  Partner_Application_Id = _loan.LoanSurveyId.ToString(),
                                  Nid_No = _loan.NidNo,
                                  Applicant_Name = _merchant.UserName,
                                  Org_Name = _merchant.CompanyName,
                                  Mobile_NO = _merchant.Mobile,
                                  Date_of_Birth = _loan.DateOfBirth,
                                  Gender = _loan.Gender.ToLower() == "male" ? "M" : "F",
                                  Loan_Amount = _loan.InterestedAmount,
                                  Present_Address = _merchant.Address,
                                  Business_Address = _merchant.Address,
                                  Experience = _loan.Experience,
                                  Education = _loan.EduLevel,
                                  Spouse_Name = _loan.SpouseName,
                                  Fathers_Name = _loan.FatherName,
                                  Mothers_Name = _loan.MotherName,
                                  Occupation = _loan.Occupation,
                                  Present_House_No = _loan.PresentAddHouseNo,
                                  Present_Road_No = _loan.PresentAddRoadNo,
                                  Present_Road_Name = _loan.PresentAddRoadName,
                                  House_Owner = _loan.HouseOwner,
                                  Present_Moholla = _loan.PresentAddArea,
                                  Present_Post_Office = _loan.PresentAddPostOffice,
                                  Present_Thana = presentAddThanaGroupJoin == null ? "" : (subPresentThana.District ?? ""),
                                  Present_District = presentAddDistrictGroupJoin == null ? "" : (subPresentDistrict.District ?? ""),
                                  Is_House_Owner = _loan.IsOwner == false ? 0 : 1,
                                  Present_Duration_LivesHere = _loan.DurationOfLiving,
                                  Permanent_House_No = _loan.PermanentAddHouseNo,
                                  Permanent_Road_No = _loan.PermanentAddRoadNo,
                                  Permanent_Road_Name = _loan.PermanentAddRoadName,
                                  Permanent_House_Owner = _loan.PermanentAddHouseOwnerName,
                                  Permanent_Moholla = _loan.PermanentAddArea,
                                  Permanent_Post_Office = _loan.PermanentAddPostOffice,
                                  Permanent_Thana = permanentAddThanaGroupJoin == null ? "" : (subPermanentThana.District ?? ""),
                                  Permanent_District = permanentAddDistrictGroupJoin == null ? "" : (subPermanentDistrict.District ?? ""),
                                  Request_Amount = _loan.InterestedAmount,
                                  Loan_Duration = _loan.ReqTenorMonth,
                                  Repayment_Method = _loan.LoanRepayType.ToLower() == "monthly" ? "M" : "W",
                                  Avg_Good_Sales_Per_Month = businessDataGroupJoin == null ? 0 : subBusinessData.AvgAmountOfMaxSellingMonth,
                                  No_Of_Good_Months = businessDataGroupJoin == null ? 0 : subBusinessData.NoOfMaxProductSellingMonth,
                                  Avg_Bad_Sales_Per_Month = businessDataGroupJoin == null ? 0 : subBusinessData.AvgAmountOfLowestSellingMonth,
                                  No_Of_Bad_Months = businessDataGroupJoin == null ? 0 : subBusinessData.NoOfLowestProductSellingMonth,
                                  Avg_Regular_Sales_Per_Month = businessDataGroupJoin == null ? 0 : subBusinessData.AvgAmountOfRegularSellingMonth,
                                  No_Of_Regular_Months = businessDataGroupJoin == null ? 0 : subBusinessData.NoOfRegularProductSellingMonth,
                                  Avg_Good_Cost_Per_Month = businessDataGroupJoin == null ? 0 : subBusinessData.AvgAmountOfMaxPurchasedMonth,
                                  No_Of_Good_Months_Cost = businessDataGroupJoin == null ? 0 : subBusinessData.NoOfMaxProductPurchasedMonth,
                                  Avg_Bad_Cost_Per_Month = businessDataGroupJoin == null ? 0 : subBusinessData.AvgAmountOfLowestPurchasedMonth,
                                  No_Of_Bad_Months_Cost = businessDataGroupJoin == null ? 0 : subBusinessData.NoOfLowestProductPurchasedMonth,
                                  Avg_Regular_Cost_Per_Month = businessDataGroupJoin == null ? 0 : subBusinessData.AvgAmountOfRegularPurchasedMonth,
                                  No_Of_Regular_Months_Cost = businessDataGroupJoin == null ? 0 : subBusinessData.NoOfRegularProductPurchasedMonth,
                                  Avg_Transaction_Amt_Per_Month = avgTransactionAmountPerMonth,
                                  Rent_Of_Business = businessDataGroupJoin == null ? 0 : subBusinessData.BusinessRent,
                                  Salary_Of_Employees = businessDataGroupJoin == null ? 0 : subBusinessData.SalaryOfEmployee,
                                  Transportation_Cost = businessDataGroupJoin == null ? 0 : subBusinessData.TransportationCost,
                                  Utility_Cost = businessDataGroupJoin == null ? 0 : subBusinessData.UtilityCost,
                                  Repair_Cost = businessDataGroupJoin == null ? 0 : subBusinessData.RepairCost,
                                  Packaging_Cost = businessDataGroupJoin == null ? 0 : subBusinessData.PackagingCost,
                                  Tax_Charges = businessDataGroupJoin == null ? 0 : subBusinessData.BusinessTax,
                                  Other_Expense = businessDataGroupJoin == null ? 0 : subBusinessData.OthersExpense,
                                  Owed_By_Debtors = businessDataGroupJoin == null ? 0 : subBusinessData.DebotrsOwingAmount,
                                  Owed_By_Creditors = businessDataGroupJoin == null ? 0 : subBusinessData.CreditorsOwingAmount,
                                  Other_Loan_Monthly_Installment = ConvertBengaliNumberToEnglishNumber(_loan.LoanEmi),
                                  Other_Loan_Amount = _loan.LoanAmount,
                                  Other_Loan_Name = _loan.BankName,
                                  Account_No = _courierUser.AccountNumber,
                                  Account_Name = _courierUser.AccountName,
                                  Bank_Code = businessDataGroupJoin == null ? "" : (subBusinessData.BankShortCode ?? ""),
                                  Routing_Number = _courierUser.RoutingNumber,
                                  Mobile_Account_No = _courierUser.BkashNumber,
                                  Cash_In_Hand = businessDataGroupJoin == null ? 0 : subBusinessData.CashInHand,
                                  Cash_At_Bank = businessDataGroupJoin == null ? 0 : subBusinessData.CashAtBank,
                                  Credit_Card_No = _loan.CreditCardNumber,
                                  Credit_Card_Limit = _loan.CardLimit != "" ? Convert.ToInt32(_loan.CardLimit) : 0,
                                  Nid_Front_Doc = _loan.NidImageUrl,
                                  Nid_Back_Doc = _loan.NidBackImageUrl,
                                  Applicant_Photo_Doc = _loan.ApplicantPhotoUrl,
                                  Check_Book_Leaf_Doc = businessDataGroupJoin == null ? "" : (subBusinessData.ChecqueBookPhotoUrl ?? ""),
                                  Source = _loan.OtherIncomeSource,
                                  MonthlyIncome = _loan.OthersIncome,
                                  PartnerFamilyMemberList = new
                                  {
                                      FMNAME = familyMemberGroupJoin == null ? "" : (subFamilyMember.FamilyMemberName ?? ""),
                                      RELATION = familyMemberGroupJoin == null ? "" : (subFamilyMember.Relation ?? ""),
                                      Age = familyMemberGroupJoin == null ? 0 : (subFamilyMember == null ? 0 : subFamilyMember.Age),
                                      EDUCATION = familyMemberGroupJoin == null ? "" : (subFamilyMember.Education ?? ""),
                                      ISMARRIED = familyMemberGroupJoin == null ? 0 : (subFamilyMember == null ? 0 : subFamilyMember.IsMarried == false ? 0 : 1),
                                      OCCUPATION = familyMemberGroupJoin == null ? "" : (subFamilyMember.Occupation ?? ""),
                                      Partner_Application_id = familyMemberGroupJoin == null ? "0" : (subFamilyMember == null ? "0" : _loan.LoanSurveyId.ToString())
                                  },
                                  Partner_Other_Income_Model = new
                                  {
                                      Source = _loan.OtherIncomeSource,
                                      Monthly_Income = _loan.OthersIncome,
                                      Partner_Application_Id = "0"//_loan.LoanSurveyId.ToString()
                                  }
                              }).OrderByDescending(o => o.Partner_Application_Id).ToListAsync();

            var dataToReturn = data.GroupBy(g => g.Partner_Application_Id).Select(s => new ShaktiLoanInfoViewModel
            {
                Partner_Application_Id = s.FirstOrDefault().Partner_Application_Id,
                Nid_No = s.FirstOrDefault().Nid_No,
                Applicant_Name = s.FirstOrDefault().Applicant_Name,
                Org_Name = s.FirstOrDefault().Org_Name,
                Mobile_NO = s.FirstOrDefault().Mobile_NO,
                Date_of_Birth = s.FirstOrDefault().Date_of_Birth,
                Gender = s.FirstOrDefault().Gender,
                Loan_Amount = s.FirstOrDefault().Loan_Amount,
                Present_Address = s.FirstOrDefault().Present_Address,
                Business_Address = s.FirstOrDefault().Business_Address,
                Experience = s.FirstOrDefault().Experience,
                Education = s.FirstOrDefault().Education,
                Spouse_Name = s.FirstOrDefault().Spouse_Name,
                Fathers_Name = s.FirstOrDefault().Fathers_Name,
                Mothers_Name = s.FirstOrDefault().Mothers_Name,
                Occupation = s.FirstOrDefault().Occupation,
                Present_House_No = s.FirstOrDefault().Present_House_No,
                Present_Road_No = s.FirstOrDefault().Present_Road_No,
                Present_Road_Name = s.FirstOrDefault().Present_Road_Name,
                House_Owner = s.FirstOrDefault().House_Owner,
                Present_Moholla = s.FirstOrDefault().Present_Moholla,
                Present_Post_Office = s.FirstOrDefault().Present_Post_Office,
                Present_Thana = s.FirstOrDefault().Present_Thana,
                Present_District = s.FirstOrDefault().Present_District,
                Is_House_Owner = s.FirstOrDefault().Is_House_Owner,
                Present_Duration_LivesHere = s.FirstOrDefault().Present_Duration_LivesHere,
                Permanent_House_No = s.FirstOrDefault().Permanent_House_No,
                Permanent_Road_No = s.FirstOrDefault().Permanent_Road_No,
                Permanent_Road_Name = s.FirstOrDefault().Permanent_Road_Name,
                Permanent_House_Owner = s.FirstOrDefault().Permanent_House_Owner,
                Permanent_Moholla = s.FirstOrDefault().Permanent_Moholla,
                Permanent_Post_Office = s.FirstOrDefault().Permanent_Post_Office,
                Permanent_Thana = s.FirstOrDefault().Permanent_Thana,
                Permanent_District = s.FirstOrDefault().Permanent_District,
                Request_Amount = s.FirstOrDefault().Request_Amount,
                Loan_Duration = s.FirstOrDefault().Loan_Duration,
                Repayment_Method = s.FirstOrDefault().Repayment_Method,
                Avg_Good_Sales_Per_Month = s.FirstOrDefault().Avg_Good_Sales_Per_Month,
                No_Of_Good_Months = s.FirstOrDefault().No_Of_Good_Months,
                Avg_Bad_Sales_Per_Month = s.FirstOrDefault().Avg_Bad_Sales_Per_Month,
                No_Of_Bad_Months = s.FirstOrDefault().No_Of_Bad_Months,
                Avg_Regular_Sales_Per_Month = s.FirstOrDefault().Avg_Regular_Sales_Per_Month,
                No_Of_Regular_Months = s.FirstOrDefault().No_Of_Regular_Months,
                Avg_Good_Cost_Per_Month = s.FirstOrDefault().Avg_Good_Cost_Per_Month,
                No_Of_Good_Months_Cost = s.FirstOrDefault().No_Of_Good_Months_Cost,
                Avg_Bad_Cost_Per_Month = s.FirstOrDefault().Avg_Bad_Cost_Per_Month,
                No_Of_Bad_Months_Cost = s.FirstOrDefault().No_Of_Bad_Months_Cost,
                Avg_Regular_Cost_Per_Month = s.FirstOrDefault().Avg_Regular_Cost_Per_Month,
                No_Of_Regular_Months_Cost = s.FirstOrDefault().No_Of_Regular_Months_Cost,
                Avg_Transaction_Amt_Per_Month = s.FirstOrDefault().Avg_Transaction_Amt_Per_Month,
                Rent_Of_Business = s.FirstOrDefault().Rent_Of_Business,
                Salary_Of_Employees = s.FirstOrDefault().Salary_Of_Employees,
                Transportation_Cost = s.FirstOrDefault().Transportation_Cost,
                Utility_Cost = s.FirstOrDefault().Utility_Cost,
                Repair_Cost = s.FirstOrDefault().Repair_Cost,
                Packaging_Cost = s.FirstOrDefault().Packaging_Cost,
                Tax_Charges = s.FirstOrDefault().Tax_Charges,
                Other_Expense = s.FirstOrDefault().Other_Expense,
                Owed_By_Debtors = s.FirstOrDefault().Owed_By_Debtors,
                Owed_By_Creditors = s.FirstOrDefault().Owed_By_Creditors,
                Other_Loan_Monthly_Installment = s.FirstOrDefault().Other_Loan_Monthly_Installment,
                Other_Loan_Amount = s.FirstOrDefault().Other_Loan_Amount,
                Other_Loan_Name = s.FirstOrDefault().Other_Loan_Name,
                Account_No = s.FirstOrDefault().Account_No,
                Account_Name = s.FirstOrDefault().Account_Name,
                Bank_Code = s.FirstOrDefault().Bank_Code,
                Routing_Number = s.FirstOrDefault().Routing_Number,
                Mobile_Account_No = s.FirstOrDefault().Mobile_Account_No,
                Cash_In_Hand = s.FirstOrDefault().Cash_In_Hand,
                Cash_At_Bank = s.FirstOrDefault().Cash_At_Bank,
                Credit_Card_No = s.FirstOrDefault().Credit_Card_No,
                Credit_Card_Limit = s.FirstOrDefault().Credit_Card_Limit,
                Nid_Front_Doc = s.FirstOrDefault().Nid_Front_Doc,
                Nid_Back_Doc = s.FirstOrDefault().Nid_Back_Doc,
                Applicant_Photo_Doc = s.FirstOrDefault().Applicant_Photo_Doc,
                Check_Book_Leaf_Doc = s.FirstOrDefault().Check_Book_Leaf_Doc,
                Source = s.FirstOrDefault().Source,
                MonthlyIncome = s.FirstOrDefault().MonthlyIncome,
                PartnerFamilyMemberList = s.Select(f => new PartnerFamilyMember
                {
                    FMNAME = f.PartnerFamilyMemberList.FMNAME,
                    RELATION = f.PartnerFamilyMemberList.RELATION,
                    Age = f.PartnerFamilyMemberList.Age,
                    EDUCATION = f.PartnerFamilyMemberList.EDUCATION,
                    ISMARRIED = f.PartnerFamilyMemberList.ISMARRIED,
                    OCCUPATION = f.PartnerFamilyMemberList.OCCUPATION,
                    Partner_Application_id = f.PartnerFamilyMemberList.Partner_Application_id
                }).ToList(),
                Partner_Other_Income_Model = s.Select(i => new PartnerOtherIncome
                {
                    Source = i.Partner_Other_Income_Model.Source,
                    Monthly_Income = i.Partner_Other_Income_Model.Monthly_Income,
                    Partner_Application_Id = i.Partner_Other_Income_Model.Partner_Application_Id
                }).ToList(),
                Merchant_Monthly_Transaction = monthlyOrderTransaction
            }).ToList();

            foreach (var item in dataToReturn)
            {
                if (item.Nid_Front_Doc != "")
                {
                    item.Nid_Front_Doc = ConvertImageURLToBase64(item.Nid_Front_Doc);
                }
                if (item.Nid_Back_Doc != "")
                {
                    item.Nid_Back_Doc = ConvertImageURLToBase64(item.Nid_Back_Doc);
                }
                if (item.Applicant_Photo_Doc != "")
                {
                    item.Applicant_Photo_Doc = ConvertImageURLToBase64(item.Applicant_Photo_Doc);
                }
                if (item.Check_Book_Leaf_Doc != "")
                {
                    item.Check_Book_Leaf_Doc = ConvertImageURLToBase64(item.Check_Book_Leaf_Doc);
                }
            }

            return dataToReturn;
        }

        public async Task<LoanApprovalPayment> AddLoanApprovalPayment(LoanApprovalPayment loanApprovalPayment)
        {
            _sqlServerContext.LoanApprovalPayment.Add(loanApprovalPayment);
            await _sqlServerContext.SaveChangesAsync();
            return loanApprovalPayment;
        }

        public async Task<dynamic> UpdateLoanApprovalPaymentDT(LoanApprovalPaymentViewModel loanApprovalPayment)
        {
            var loanApprovalPaymentEntity = await _sqlServerContext.LoanApprovalPayment.FirstOrDefaultAsync(l => l.LoanSurveyId.Equals(loanApprovalPayment.LoanApplicationId));

            if(loanApprovalPaymentEntity != null)
            {
                if(loanApprovalPayment.AcquiredAmount == 25)
                {
                    loanApprovalPaymentEntity.DTAcquiredAmount = loanApprovalPayment.AcquiredAmount;
                    loanApprovalPaymentEntity.ThirdPartyAcquiredAmount = loanApprovalPayment.AcquiredAmount;

                    _sqlServerContext.LoanApprovalPayment.Update(loanApprovalPaymentEntity);
                    await _sqlServerContext.SaveChangesAsync();

                    return loanApprovalPaymentEntity;
                }
                return "your amount is not correct";
            }
            return "Update loan approval payment failed";
        }

        public async Task<IEnumerable<LoanApprovedStatusViewModel>> GetMerchantLoanApprovedStatus(int courierUserId)
        {
            var loanEntity = await _sqlServerContext.LoanSurvey.Where(l => l.CourierUserId.Equals(courierUserId)).OrderByDescending(o => o.LoanSurveyId).FirstOrDefaultAsync();
            List<LoanApprovedStatusViewModel> loanApprovedStatus = new List<LoanApprovedStatusViewModel>();

            if (loanEntity != null)
            {
                loanApprovedStatus = await (from _loan in _sqlServerContext.LoanSurvey
                                        join _loanStatus in _sqlServerContext.LoanStatus
                                        on _loan.LoanSurveyId equals _loanStatus.LoanSurveyId

                                        join _lenderUser in _sqlServerContext.LenderUser
                                        on _loanStatus.LenderUserId equals _lenderUser.LenderUserId

                                        where _loan.LoanSurveyId.Equals(loanEntity.LoanSurveyId)
                                        select new LoanApprovedStatusViewModel
                                        {
                                            LoanApplicationId = _loan.LoanSurveyId,
                                            StatusCode = _loanStatus.StatusCode,
                                            Status = _loanStatus.Status,
                                            Comment = _loanStatus.Comment,
                                            CommentDate = _loanStatus.CommentDate,
                                            LenderUserName = _lenderUser.LenderName
                                        }).ToListAsync();

                var data = await _sqlServerContext.LoanDisbursement.Where(l => l.LoanSurveyId.Equals(loanEntity.LoanSurveyId)).FirstOrDefaultAsync();

                if (data != null)
                {
                    foreach (var item in loanApprovedStatus)
                    {
                        item.DisbursementDate = data.DisbursementDate;
                        item.EmiAmount = data.EmiAmount;
                    }
                }
            }

            return loanApprovedStatus;
        }

        public async Task<IEnumerable<LoanSurveyViewModel>> GetApprovedLoanApplicantsData()
        {
            var approvedLoanApplicantsList = await _sqlServerContext.LoanStatus.Where(l => l.StatusCode.Equals("00002")).Select(s => s.LoanSurveyId).Distinct().ToListAsync();
            List<LoanSurveyViewModel> loanSurveyData = new List<LoanSurveyViewModel>();

            var allData = await (from _loan in _sqlServerContext.LoanSurvey
                                 join _merchant in _sqlServerContext.CourierUsers on _loan.CourierUserId equals _merchant.CourierUserId

                                 join _loanCourier in _sqlServerContext.CouriersWithLoanSurvey
                                 on _loan.LoanSurveyId equals _loanCourier.LoanSurveyId into lc
                                 from _courier in lc.DefaultIfEmpty()

                                 join _thana in _sqlServerContext.Districts
                                 on _merchant.ThanaId equals _thana.DistrictId into merchantThanaJoin
                                 from subMerchantThana in merchantThanaJoin.DefaultIfEmpty()

                                 join presentDistrict in _sqlServerContext.Districts
                                 on _loan.PresentAddDistrictId equals presentDistrict.DistrictId into presentDistrictJoin
                                 from subPresentDistrict in presentDistrictJoin.DefaultIfEmpty()

                                 join presentThana in _sqlServerContext.Districts
                                 on _loan.PresentAddThanaId equals presentThana.DistrictId into presentThanaJoin
                                 from subPresentThana in presentThanaJoin.DefaultIfEmpty()

                                 join permanentDistrict in _sqlServerContext.Districts
                                 on _loan.PermanentAddDistrictId equals permanentDistrict.DistrictId into permanentDistrictJoin
                                 from subPermanentDistrict in permanentDistrictJoin.DefaultIfEmpty()

                                 join permanentThana in _sqlServerContext.Districts
                                 on _loan.PermanentAddThanaId equals permanentThana.DistrictId into permanentThanaJoin
                                 from subPermanentThana in presentThanaJoin.DefaultIfEmpty()

                                 join _loanStatus in _sqlServerContext.LoanStatus
                                 on _loan.LoanSurveyId equals _loanStatus.LoanSurveyId into loanStatusJoin
                                 from subLoanStatus in loanStatusJoin.DefaultIfEmpty()

                                 where approvedLoanApplicantsList.Contains(_loan.LoanSurveyId)
                                 && _loan.CourierUserId != 1
                                 select new
                                 {
                                     CourierUserId = _loan.CourierUserId,
                                     LoanSurveyId = _loan.LoanSurveyId,
                                     MerchantName = _merchant.CompanyName,
                                     TradeLicenseImageUrl = _loan.TradeLicenseImageUrl,
                                     InterestedAmount = _loan.InterestedAmount,
                                     TransactionAmount = _loan.TransactionAmount,
                                     IsBankAccount = _loan.IsBankAccount,
                                     IsLocalShop = _loan.IsLocalShop,
                                     ApplicationDate = _loan.ApplicationDate,
                                     MonthlyTotalCodAmount = _loan.MonthlyTotalCodAmount,
                                     GuarantorName = _loan.GuarantorName,
                                     GuarantorMobile = _loan.GuarantorMobile,
                                     MonthlyTotalAverageSale = _loan.MonthlyTotalAverageSale,
                                     LoanAmount = _loan.LoanAmount,
                                     BankName = _loan.BankName,
                                     Gender = _loan.Gender,
                                     Age = _loan.Age,
                                     BasketValue = _loan.BasketValue,
                                     CardHolder = _loan.CardHolder,
                                     CardLimit = _loan.CardLimit,
                                     LoanEmi = _loan.LoanEmi,
                                     HasCreditCard = _loan.HasCreditCard,
                                     HasTin = _loan.HasTin,
                                     EduLevel = _loan.EduLevel,
                                     RepayType = _loan.RepayType,
                                     MonthlyOrder = _loan.MonthlyOrder,
                                     MonthlyExp = _loan.MonthlyExp,
                                     Recommend = _loan.Recommend,
                                     RelationMarchent = _loan.RelationMarchent,
                                     ShopOwnership = _loan.ShopOwnership,
                                     TinNumber = _loan.TinNumber,
                                     HomeOwnership = _loan.HomeOwnership,
                                     Married = _loan.Married,
                                     FamMem = _loan.FamMem,
                                     HasTradeLicense = _loan.HasTradeLicense,
                                     TradeLicenseNo = _loan.TradeLicenseNo,
                                     TradeLicenseExpireDate = _loan.TradeLicenseExpireDate,
                                     CompanyBankAccNo = _loan.CompanyBankAccNo,
                                     CompanyBankAccName = _loan.CompanyBankAccName,
                                     AnnualTotalIncome = _loan.AnnualTotalIncome,
                                     DateOfBirth = _loan.DateOfBirth,
                                     NidNo = _loan.NidNo,
                                     OthersIncome = _loan.OthersIncome,
                                     ReqTenorMonth = _loan.ReqTenorMonth,
                                     ResidenceLocation = _loan.ResidenceLocation,
                                     CollectionAmountAvg = 0,
                                     HasPreviousLoan = _loan.HasPreviousLoan,
                                     LenderType = _loan.LenderType,
                                     BankStatementUrl = _loan.BankStatementUrl,
                                     Comments = _loan.Comments,
                                     BusinessStartDate = _loan.BusinessStartDate,
                                     NidImageUrl = _loan.NidImageUrl,
                                     NidBackImageUrl = _loan.NidBackImageUrl,
                                     TinImageUrl = _loan.TinImageUrl,
                                     CibUploadedFormUrl = _loan.CibUploadedFormUrl,
                                     Address = _merchant.Address,
                                     ThanaId = _merchant.ThanaId,
                                     ThanaName = merchantThanaJoin == null ? "" : (subMerchantThana.District ?? ""),
                                     FatherName = _loan.FatherName,
                                     MotherName = _loan.MotherName,
                                     SpouseName = _loan.SpouseName,
                                     IsLoanDue = _loan.IsLoanDue,
                                     PresentAddHouseNo = _loan.PresentAddHouseNo,
                                     PresentAddRoadNo = _loan.PresentAddRoadNo,
                                     PresentAddRoadName = _loan.PresentAddRoadName,
                                     PresentAddArea = _loan.PresentAddArea,
                                     PresentAddPostOffice = _loan.PresentAddPostOffice,
                                     PresentAddDistrictId = _loan.PresentAddDistrictId,
                                     PresentDistrictName = presentDistrictJoin == null ? "" : (subPresentDistrict.District ?? ""),
                                     PresentAddThanaId = _loan.PresentAddThanaId,
                                     PresentThanaName = presentThanaJoin == null ? "" : (subPresentThana.District ?? ""),
                                     HouseOwner = _loan.HouseOwner,
                                     IsOwner = _loan.IsOwner,
                                     DurationOfLiving = _loan.DurationOfLiving,
                                     PermanentAddHouseNo = _loan.PermanentAddHouseNo,
                                     PermanentAddRoadNo = _loan.PermanentAddRoadNo,
                                     PermanentAddRoadName = _loan.PermanentAddRoadName,
                                     PermanentAddArea = _loan.PermanentAddArea,
                                     PermanentAddPostOffice = _loan.PermanentAddPostOffice,
                                     PermanentAddDistrictId = _loan.PermanentAddDistrictId,
                                     PermanentDistrictName = permanentDistrictJoin == null ? "" : (subPermanentDistrict.District ?? ""),
                                     PermanentAddThanaId = _loan.PermanentAddThanaId,
                                     PermanentThanaName = permanentThanaJoin == null ? "" : (subPermanentThana.District ?? ""),
                                     Experience = _loan.Experience,
                                     Occupation = _loan.Occupation,
                                     PermanentAddHouseOwnerName = _loan.PermanentAddHouseOwnerName,
                                     CreditCardNumber = _loan.CreditCardNumber,
                                     ApplicantPhotoUrl = _loan.ApplicantPhotoUrl,
                                     LoanRepayType = _loan.LoanRepayType,
                                     OtherIncomeSource = _loan.OtherIncomeSource,
                                     CouriersWithLoanSurveyViewModel = new
                                     {
                                         CouriersWithLoanSurveyId = _courier == null ? 0 : _courier.CouriersWithLoanSurveyId,
                                         CourierId = _courier == null ? 0 : _courier.CourierId,
                                         CourierName = _courier == null ? "" : (_courier.CourierName ?? ""),
                                         LoanSurveyId = _courier == null ? 0 : _courier.LoanSurveyId
                                     },
                                     LoanStatus = new
                                     {
                                         LoanStatusId = loanStatusJoin == null ? 0 : subLoanStatus.LoanStatusId,
                                         LoanSurveyId = loanStatusJoin == null ? 0 : subLoanStatus.LoanSurveyId,
                                         StatusCode = loanStatusJoin == null ? "" : (subLoanStatus.StatusCode ?? ""),
                                         Status = loanStatusJoin == null ? "" : (subLoanStatus.Status ?? ""),
                                         Comment = loanStatusJoin == null ? "" : (subLoanStatus.Comment ?? ""),
                                         CommentDate = loanStatusJoin == null ? DateTime.Now : subLoanStatus.CommentDate
                                     }
                                 }).OrderByDescending(o => o.LoanSurveyId).ToListAsync();

            loanSurveyData = allData.GroupBy(g => g.CourierUserId).Select(s => new LoanSurveyViewModel
            {
                LoanSurveyId = s.FirstOrDefault().LoanSurveyId,
                CourierUserId = s.FirstOrDefault().CourierUserId,
                MerchantName = s.FirstOrDefault().MerchantName,
                TradeLicenseImageUrl = s.FirstOrDefault().TradeLicenseImageUrl,
                InterestedAmount = s.FirstOrDefault().InterestedAmount,
                TransactionAmount = s.FirstOrDefault().TransactionAmount,
                IsBankAccount = s.FirstOrDefault().IsBankAccount,
                IsLocalShop = s.FirstOrDefault().IsLocalShop,
                ApplicationDate = s.FirstOrDefault().ApplicationDate,
                MonthlyTotalCodAmount = s.FirstOrDefault().MonthlyTotalCodAmount,
                GuarantorName = s.FirstOrDefault().GuarantorName,
                GuarantorMobile = s.FirstOrDefault().GuarantorMobile,
                MonthlyTotalAverageSale = s.FirstOrDefault().MonthlyTotalAverageSale,
                LoanAmount = s.FirstOrDefault().LoanAmount,
                BankName = s.FirstOrDefault().BankName,
                Gender = s.FirstOrDefault().Gender,
                Age = s.FirstOrDefault().Age,
                BasketValue = s.FirstOrDefault().BasketValue,
                CardHolder = s.FirstOrDefault().CardHolder,
                CardLimit = s.FirstOrDefault().CardLimit,
                LoanEmi = s.FirstOrDefault().LoanEmi,
                HasCreditCard = s.FirstOrDefault().HasCreditCard,
                HasTin = s.FirstOrDefault().HasTin,
                EduLevel = s.FirstOrDefault().EduLevel,
                RepayType = s.FirstOrDefault().RepayType,
                MonthlyOrder = s.FirstOrDefault().MonthlyOrder,
                MonthlyExp = s.FirstOrDefault().MonthlyExp,
                Recommend = s.FirstOrDefault().Recommend,
                RelationMarchent = s.FirstOrDefault().RelationMarchent,
                ShopOwnership = s.FirstOrDefault().ShopOwnership,
                TinNumber = s.FirstOrDefault().TinNumber,
                HomeOwnership = s.FirstOrDefault().HomeOwnership,
                Married = s.FirstOrDefault().Married,
                FamMem = s.FirstOrDefault().FamMem,
                HasTradeLicense = s.FirstOrDefault().HasTradeLicense,
                TradeLicenseNo = s.FirstOrDefault().TradeLicenseNo,
                TradeLicenseExpireDate = s.FirstOrDefault().TradeLicenseExpireDate,
                CompanyBankAccNo = s.FirstOrDefault().CompanyBankAccNo,
                CompanyBankAccName = s.FirstOrDefault().CompanyBankAccName,
                AnnualTotalIncome = s.FirstOrDefault().AnnualTotalIncome,
                DateOfBirth = s.FirstOrDefault().DateOfBirth,
                NidNo = s.FirstOrDefault().NidNo,
                OthersIncome = s.FirstOrDefault().OthersIncome,
                ReqTenorMonth = s.FirstOrDefault().ReqTenorMonth,
                ResidenceLocation = s.FirstOrDefault().ResidenceLocation,
                CollectionAmountAvg = s.FirstOrDefault().CollectionAmountAvg,
                HasPreviousLoan = s.FirstOrDefault().HasPreviousLoan,
                LenderType = s.FirstOrDefault().LenderType,
                BankStatementUrl = s.FirstOrDefault().BankStatementUrl,
                Comments = s.FirstOrDefault().Comments,
                BusinessStartDate = s.FirstOrDefault().BusinessStartDate,
                NidImageUrl = s.FirstOrDefault().NidImageUrl,
                NidBackImageUrl = s.FirstOrDefault().NidBackImageUrl,
                TinImageUrl = s.FirstOrDefault().TinImageUrl,
                CibUploadedFormUrl = s.FirstOrDefault().CibUploadedFormUrl,
                Address = s.FirstOrDefault().Address,
                ThanaId = s.FirstOrDefault().ThanaId,
                ThanaName = s.FirstOrDefault().ThanaName,
                FatherName = s.FirstOrDefault().FatherName,
                MotherName = s.FirstOrDefault().MotherName,
                SpouseName = s.FirstOrDefault().SpouseName,
                IsLoanDue = s.FirstOrDefault().IsLoanDue,
                PresentAddHouseNo = s.FirstOrDefault().PresentAddHouseNo,
                PresentAddRoadNo = s.FirstOrDefault().PresentAddRoadNo,
                PresentAddRoadName = s.FirstOrDefault().PresentAddRoadName,
                PresentAddArea = s.FirstOrDefault().PresentAddArea,
                PresentAddPostOffice = s.FirstOrDefault().PresentAddPostOffice,
                PresentAddDistrictId = s.FirstOrDefault().PresentAddDistrictId,
                PresentDistrictName = s.FirstOrDefault().PresentDistrictName,
                PresentAddThanaId = s.FirstOrDefault().PresentAddThanaId,
                PresentThanaName = s.FirstOrDefault().PresentThanaName,
                HouseOwner = s.FirstOrDefault().HouseOwner,
                IsOwner = s.FirstOrDefault().IsOwner,
                DurationOfLiving = s.FirstOrDefault().DurationOfLiving,
                PermanentAddHouseNo = s.FirstOrDefault().PermanentAddHouseNo,
                PermanentAddRoadNo = s.FirstOrDefault().PermanentAddRoadNo,
                PermanentAddRoadName = s.FirstOrDefault().PermanentAddRoadName,
                PermanentAddArea = s.FirstOrDefault().PermanentAddArea,
                PermanentAddPostOffice = s.FirstOrDefault().PermanentAddPostOffice,
                PermanentAddDistrictId = s.FirstOrDefault().PermanentAddDistrictId,
                PermanentDistrictName = s.FirstOrDefault().PermanentDistrictName,
                PermanentAddThanaId = s.FirstOrDefault().PermanentAddThanaId,
                PermanentThanaName = s.FirstOrDefault().PermanentThanaName,
                Experience = s.FirstOrDefault().Experience,
                Occupation = s.FirstOrDefault().Occupation,
                PermanentAddHouseOwnerName = s.FirstOrDefault().PermanentAddHouseOwnerName,
                CreditCardNumber = s.FirstOrDefault().CreditCardNumber,
                ApplicantPhotoUrl = s.FirstOrDefault().ApplicantPhotoUrl,
                LoanRepayType = s.FirstOrDefault().LoanRepayType,
                OtherIncomeSource = s.FirstOrDefault().OtherIncomeSource,
                LoanApprovedStatusViewModel = s.Where(k => k.LoanStatus.LoanSurveyId == s.FirstOrDefault().LoanSurveyId)
                .GroupBy(g => g.LoanStatus.LoanStatusId)
                .Select(a => new LoanApprovedStatusViewModel
                {
                    LoanApplicationId = a.FirstOrDefault().LoanSurveyId,
                    Status = a.FirstOrDefault().LoanStatus.Status,
                    StatusCode = a.FirstOrDefault().LoanStatus.StatusCode,
                    Comment = a.FirstOrDefault().LoanStatus.Comment,
                    CommentDate = a.FirstOrDefault().LoanStatus.CommentDate
                }).ToList(),
                CouriersWithLoanSurveyViewModel = s.Where(w => w.LoanSurveyId == s.FirstOrDefault().LoanSurveyId)
                .GroupBy(g => g.CouriersWithLoanSurveyViewModel.CouriersWithLoanSurveyId)
                .Select(r => new CouriersWithLoanSurveyViewModel
                {
                    CouriersWithLoanSurveyId = r.FirstOrDefault().CouriersWithLoanSurveyViewModel.CouriersWithLoanSurveyId,
                    CourierId = r.FirstOrDefault().CouriersWithLoanSurveyViewModel.CourierId,
                    CourierName = r.FirstOrDefault().CouriersWithLoanSurveyViewModel.CourierName,
                    LoanSurveyId = r.FirstOrDefault().CouriersWithLoanSurveyViewModel.LoanSurveyId
                }).ToList()
            }).ToList();

            var array = loanSurveyData.Select(x => x.CourierUserId).ToArray();

            var data = _sqlServerContext.CourierOrders.Where(x => array.Contains(x.MerchantId)
                && x.OrderDate.Date >= DateTime.Now.Date.AddMonths(-3)
                && x.OrderDate.Date < DateTime.Now.Date.AddDays(1)
                && x.IsDownloaded == true)
                .GroupBy(g => g.MerchantId).Select(s => new
                {
                    MerchantId = s.Key,
                    TotalCollectionAmount = s.Sum(c => c.CollectionAmount),
                    Months = s.GroupBy(g => g.OrderDate.Month).Select(grouped => new
                    {
                        Month = grouped.Key
                    }).Count()
                }).ToList();

            foreach (var item in loanSurveyData)
            {
                var totalCollectionAmount = data.Where(w => w.MerchantId.Equals(item.CourierUserId)).Count() > 0 ? data.Where(w => w.MerchantId.Equals(item.CourierUserId)).FirstOrDefault().TotalCollectionAmount : 0;

                int month = data.Where(w => w.MerchantId.Equals(item.CourierUserId)).Count() > 0 ? data.Where(w => w.MerchantId.Equals(item.CourierUserId)).FirstOrDefault().Months : 0;

                int dynamicMonth = month == 0 ? 3 : (month < 3 ? month : 3);

                item.CollectionAmountAvg = totalCollectionAmount / dynamicMonth;
            }

            return loanSurveyData;
        }

        public async Task<LoanDisbursement> GetLoanDisbursementsInfo(int LoanSurveyId)
        {
            var data = await _sqlServerContext.LoanDisbursement.Where(l => l.LoanSurveyId.Equals(LoanSurveyId)).FirstOrDefaultAsync();
            return data;
        }

        public async Task<IEnumerable<LoanDisbursementHistory>> GetLoanDisbursementsHistoryInfo(string TransactionId)
        {
            var data = await _sqlServerContext.LoanDisbursementHistory.Where(l => l.RefTransactionID.Equals(TransactionId)).ToListAsync();
            return data;
        }

        public async Task<dynamic> GetDanaCustomInformation(int CourierUserId)
        {
            var danaColumnData = await GetDanaColumnValue(CourierUserId);
            string recommendDana = danaColumnData.Where(x => x.DanaProperty.Equals("recommend")).Select(s => s.Value).FirstOrDefault();
            string lastReviewDana = danaColumnData.Where(x => x.DanaProperty.Equals("last_review")).Select(s => s.Value).FirstOrDefault();
            string recommend = "", lastReview = "";
            if (recommendDana == "1")
            {
                recommend = "Silver";
            }
            else if (recommendDana == "1")
            {
                recommend = "Gold";
            }
            else if (recommendDana == "1")
            {
                recommend = "Platinum";
            }
            else
            {
                recommend = "Not Reviewed Yet";
            }

            if(lastReviewDana == "0")
            {
                lastReview = "Not Reviewed Yet";
            }
            else
            {
                lastReview = lastReviewDana;
            }

            List<DanaCustomInformationViewModel> data = new List<DanaCustomInformationViewModel>();

            data = await (from _loan in _sqlServerContext.LoanSurvey
                       where _loan.CourierUserId.Equals(CourierUserId)
                       select new DanaCustomInformationViewModel
                       {
                           LoanSurveyId = _loan.LoanSurveyId,
                           CourierUserId = _loan.CourierUserId,
                           age = _loan.Age,
                           basket_value = danaColumnData.Where(x => x.DanaProperty.Equals("basket_value")).Select(s => s.Value).FirstOrDefault(),
                           avg_monthly_sale = danaColumnData.Where(x => x.DanaProperty.Equals("avg_monthly_sale")).Select(s => s.Value).FirstOrDefault(),
                           has_bank_ac = _loan.IsBankAccount == true ? "হ্যাঁ" : "না",
                           bank_statement = _loan.BankStatementUrl,
                           business_age = danaColumnData.Where(x => x.DanaProperty.Equals("relation_marchent")).Select(s => s.Value).FirstOrDefault(),
                           business_location = "",
                           business_type = "",
                           call_sms_ratio = "",
                           commission_share = danaColumnData.Where(x => x.DanaProperty.Equals("commission_share")).Select(s => s.Value).FirstOrDefault(),
                           credit = "",
                           loan_emi = _loan.HasPreviousLoan == true ? _loan.LoanEmi : "",
                           debit = "",
                           deposit = "",
                           edu_level = _loan.EduLevel,
                           fund_inward = "",
                           fund_outward = "",
                           gender = _loan.Gender,
                           handset_brand = "",
                           has_current_loan = _loan.HasPreviousLoan == true ? "হ্যাঁ" : "না",
                           has_physical_shop = _loan.IsLocalShop == true ? "হ্যাঁ" : "না",
                           home_ownership = _loan.HomeOwnership,
                           interest_credit = "",
                           lender_type = _loan.LenderType,
                           loan_amount = "",
                           repay_type = _loan.LoanRepayType,
                           married = _loan.Married,
                           mobile_recharge = "",
                           average_inventory = danaColumnData.Where(x => x.DanaProperty.Equals("average_inventory")).Select(s => s.Value).FirstOrDefault(),
                           monthly_order = danaColumnData.Where(x => x.DanaProperty.Equals("monthly_order")).Select(s => s.Value).FirstOrDefault(),
                           monthly_exp = danaColumnData.Where(x => x.DanaProperty.Equals("avg_monthly_sale")).Select(s => s.Value).FirstOrDefault(),
                           monthly_income = _loan.MonthlyTotalCodAmount.ToString(),
                           call_receive = "",
                           call_made = "",
                           fam_mem = _loan.FamMem,
                           miss_call = "",
                           payment_card = "",
                           purchase_card = "",
                           recommend = recommend,
                           relation_marchent = danaColumnData.Where(x => x.DanaProperty.Equals("relation_marchent")).Select(s => s.Value).FirstOrDefault(),
                           residence_location = _loan.ResidenceLocation,
                           last_review = lastReview,
                           shop_ownership = _loan.ShopOwnership,
                           smartphone_ownership = "",
                           sms_in = "",
                           sms_out = "",
                           tax_doc = "",
                           last_year_sale = danaColumnData.Where(x => x.DanaProperty.Equals("last_year_sale")).Select(s => s.Value).FirstOrDefault(),
                           trade_license = _loan.TradeLicenseImageUrl,
                           type_product_sale = "",
                           utility_bill = "",
                           withdrawal = ""
                       }).OrderByDescending(o => o.LoanSurveyId).ToListAsync();

            //calculatedValue = _loan.HasPreviousLoan == true ? _loan.LoanEmi != "" ? (Convert.ToDecimal(ConvertBengaliNumberToEnglishNumber(_loan.LoanEmi)) / Convert.ToDecimal(danaColumnData.Where(x => x.DanaProperty.Equals("avg_monthly_sale")).Select(s => s.Value).FirstOrDefault()) * 100) : 0 : 0

            if (data.Count() > 1)
            {
                int loanId = 0;
                foreach (var item in data)
                {
                    if (item.has_current_loan == "হ্যাঁ" && item.loan_emi != "")
                    {
                        loanId = item.LoanSurveyId;
                    }
                }

                if(loanId != 0)
                {
                    data = data.Where(x => x.LoanSurveyId == loanId).ToList();
                }
            }

            var groupedData = data.GroupBy(g => g.CourierUserId).Select(s => new DanaCustomInformationViewModel
            {
                age = s.FirstOrDefault().age,
                basket_value = s.FirstOrDefault().basket_value,
                avg_monthly_sale = s.FirstOrDefault().avg_monthly_sale,
                has_bank_ac = s.FirstOrDefault().has_bank_ac,
                bank_statement = s.FirstOrDefault().bank_statement,
                business_age = s.FirstOrDefault().business_age,
                business_location = s.FirstOrDefault().business_location,
                business_type = s.FirstOrDefault().business_type,
                call_sms_ratio = s.FirstOrDefault().call_sms_ratio,
                commission_share = s.FirstOrDefault().commission_share,
                credit = s.FirstOrDefault().credit,
                loan_emi = s.FirstOrDefault().loan_emi,
                debit = s.FirstOrDefault().debit,
                deposit = s.FirstOrDefault().deposit,
                edu_level = s.FirstOrDefault().edu_level,
                fund_inward = s.FirstOrDefault().fund_inward,
                fund_outward = s.FirstOrDefault().fund_outward,
                gender = s.FirstOrDefault().gender,
                handset_brand = s.FirstOrDefault().handset_brand,
                has_current_loan = s.FirstOrDefault().has_current_loan,
                has_physical_shop = s.FirstOrDefault().has_physical_shop,
                home_ownership = s.FirstOrDefault().home_ownership,
                interest_credit = s.FirstOrDefault().interest_credit,
                lender_type = s.FirstOrDefault().lender_type,
                loan_amount = s.FirstOrDefault().loan_amount,
                repay_type = s.FirstOrDefault().repay_type,
                married = s.FirstOrDefault().married,
                mobile_recharge = s.FirstOrDefault().mobile_recharge,
                average_inventory = s.FirstOrDefault().average_inventory,
                monthly_order = s.FirstOrDefault().monthly_order,
                monthly_exp = s.FirstOrDefault().monthly_exp,
                monthly_income = s.FirstOrDefault().monthly_income,
                call_receive = s.FirstOrDefault().call_receive,
                call_made = s.FirstOrDefault().call_made,
                fam_mem = s.FirstOrDefault().fam_mem,
                miss_call = s.FirstOrDefault().miss_call,
                payment_card = s.FirstOrDefault().payment_card,
                purchase_card = s.FirstOrDefault().purchase_card,
                recommend = s.FirstOrDefault().recommend,
                relation_marchent = s.FirstOrDefault().relation_marchent,
                residence_location = s.FirstOrDefault().residence_location,
                last_review = s.FirstOrDefault().last_review,
                shop_ownership = s.FirstOrDefault().shop_ownership,
                smartphone_ownership = s.FirstOrDefault().smartphone_ownership,
                sms_in = s.FirstOrDefault().sms_in,
                sms_out = s.FirstOrDefault().sms_out,
                tax_doc = s.FirstOrDefault().tax_doc,
                last_year_sale = s.FirstOrDefault().last_year_sale,
                trade_license = s.FirstOrDefault().trade_license,
                type_product_sale = s.FirstOrDefault().type_product_sale,
                utility_bill = s.FirstOrDefault().utility_bill,
                withdrawal = s.FirstOrDefault().withdrawal
            }).ToList();

            foreach (var item in groupedData)
            {
                if (item.has_current_loan == "হ্যাঁ" && item.loan_emi != "")
                {
                    item.emi_sales_percent = Decimal.Round(Convert.ToDecimal(ConvertBengaliNumberToEnglishNumber(item.loan_emi)) / Convert.ToDecimal(danaColumnData.Where(x => x.DanaProperty.Equals("avg_monthly_sale")).Select(s => s.Value).FirstOrDefault()) * 100, 2, MidpointRounding.AwayFromZero);
                }
            }

            return groupedData;

        }

        public async Task<IEnumerable<LoanDisbursementInfoViewModel>> GetMerchantLoanDisbursementInfo(string StatusCode)
        {
            DateTime date = DateTime.Today;
            var firstDay = new DateTime(date.Year, date.Month, 1);
            var lastDay = firstDay.AddMonths(1).AddDays(-1);

            var testOrderStatus = new int[] { 0, 2, 29 };

            var codData = await (from _loanStatus in _sqlServerContext.LoanStatus
                          join _loanSurvey in _sqlServerContext.LoanSurvey
                          on _loanStatus.LoanSurveyId equals _loanSurvey.LoanSurveyId

                          join _order in _sqlServerContext.CourierOrders.Where(o => o.OrderDate >= firstDay && o.OrderDate <= lastDay & !testOrderStatus.Contains(o.Status))
                          on _loanSurvey.CourierUserId equals _order.MerchantId into orderJoin
                          from subOrders in orderJoin.DefaultIfEmpty()

                          where _loanStatus.StatusCode.Contains(StatusCode)
                          select new
                          {
                              CourierUserId = _loanSurvey.CourierUserId,
                              CodAmount = orderJoin == null ? 0 : (subOrders == null ? 0 : subOrders.CollectionAmount),
                          }).ToListAsync();

            var codList = codData.GroupBy(g => g.CourierUserId)
                .Select(s => new
                {
                    CourierUserId = s.FirstOrDefault().CourierUserId,
                    CodAmount = s.Sum(m => m.CodAmount)
                });

            var data = await (from _loanStatus in _sqlServerContext.LoanStatus
                       join _loanDisbursement in _sqlServerContext.LoanDisbursement
                       on _loanStatus.LoanSurveyId equals _loanDisbursement.LoanSurveyId into statusDisbursementJoin
                       from subLoanDisbursement in statusDisbursementJoin.DefaultIfEmpty()

                       join _loanSurvey in _sqlServerContext.LoanSurvey
                       on _loanStatus.LoanSurveyId equals _loanSurvey.LoanSurveyId

                       join _courierUser in _sqlServerContext.CourierUsers
                       on _loanSurvey.CourierUserId equals _courierUser.CourierUserId

                       join _lenderUser in _sqlServerContext.LenderUser
                       on _loanStatus.LenderUserId equals _lenderUser.LenderUserId

                       where _loanStatus.StatusCode.Contains(StatusCode)
                       select new LoanDisbursementInfoViewModel
                       {
                           CourierUserId = _courierUser.CourierUserId,
                           StatusCode = _loanStatus.StatusCode,
                           LoanApprovalAmount = statusDisbursementJoin == null ? 0 : (subLoanDisbursement == null ? 0 : subLoanDisbursement.DisbursementAmount),
                           EmiAmount = statusDisbursementJoin == null ? 0 : (subLoanDisbursement == null ? 0 : subLoanDisbursement.EmiAmount),
                           TenorMonth = statusDisbursementJoin == null ? 0 : (subLoanDisbursement == null ? 0 : subLoanDisbursement.RequiredTenorMonth),
                           CompanyName = _courierUser.CompanyName,
                           UserName = _courierUser.UserName,
                           LenderUserName = _lenderUser.LenderName,
                           CodAmount = 0
                       }).ToListAsync();

            var loanStatusData = data.GroupBy(g => g.CourierUserId)
                .Select(s => new LoanDisbursementInfoViewModel
                {
                    CourierUserId = s.FirstOrDefault().CourierUserId,
                    StatusCode = s.FirstOrDefault().StatusCode,
                    LoanApprovalAmount = s.FirstOrDefault().LoanApprovalAmount,
                    EmiAmount = s.FirstOrDefault().EmiAmount,
                    TenorMonth = s.FirstOrDefault().TenorMonth,
                    CompanyName = s.FirstOrDefault().CompanyName,
                    UserName = s.FirstOrDefault().UserName,
                    LenderUserName = s.FirstOrDefault().LenderUserName,
                    CodAmount = s.FirstOrDefault().CodAmount
                }).ToList();

            foreach (var item in loanStatusData)
            {
                item.CodAmount = codList.Where(c => c.CourierUserId == item.CourierUserId).FirstOrDefault().CodAmount;
            }

            return loanStatusData;
        }
    }
}
