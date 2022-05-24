using AdCourier.Context;
using AdCourier.Domain.Entities.DataModel;
using AdCourier.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using EFCore.BulkExtensions;
using AdCourier.Domain.Entities;
using Microsoft.Extensions.Options;
using System.Data.SqlClient;
using Dapper;
using System.Data;
using AdCourier.Domain.Entities.ViewModel.GetDeliveryChargeDetails;
using AdCourier.Domain.Entities.ViewModel.Offer;
using AdCourier.Domain.Entities.ViewModel.DeliveryManAssign;
using AdCourier.Domain.Entities.ViewModel.DatabaseViewModel;
using AdCourier.Domain.Entities.BodyModel.DeliveryChargeDetails;
using AdCourier.Domain.Entities.BodyModel.CourierOrder;
using AdCourier.Domain.Entities.ViewModel.CourierOrder;
using AdCourier.Domain.Entities.ViewModel.BondhuApp;
using AdCourier.Domain.Entities.ViewModel.OrderRequest;
using AdCourier.Domain.Entities.ViewModel.CourierUsers;
using AdCourier.Domain.Entities.BodyModel.Permission;
using AdCourier.Services.Interfaces;
using AdCourier.Domain.Entities.ViewModel.Poh;

namespace AdCourier.Infrastructure.Data
{
    public class WeightRangeRepository : IWeightRangeRepository
    {
        private readonly SqlServerContext _sqlServerContext;
        private readonly ConnectionStringList _connectionStrings;
        private readonly ISmsEmailService _smsEmailService;
        private readonly IFirebaseCloudService _firebaseCloudService;
        public WeightRangeRepository(SqlServerContext sqlServerContext, IOptions<ConnectionStringList> connectionStrings, ISmsEmailService smsEmailService, IFirebaseCloudService firebaseCloudService)
        {
            _sqlServerContext = sqlServerContext ?? throw new ArgumentNullException(nameof(sqlServerContext));
            _connectionStrings = connectionStrings.Value;
            _smsEmailService = smsEmailService;
            _firebaseCloudService = firebaseCloudService;
        }
        public async Task<DbActionBn> AddDbActionBn(DbActionBn dbActionBn)
        {
            await _sqlServerContext.DbActionBn.AddAsync(dbActionBn);
            await _sqlServerContext.SaveChangesAsync();
            return dbActionBn;
        }

        public async Task<OrderAssignTrack> AddOrderAssignTrack(OrderAssignTrack orderAssignTrack)
        {
            await _sqlServerContext.OrderAssignTrack.AddAsync(orderAssignTrack);
            await _sqlServerContext.SaveChangesAsync();
            return orderAssignTrack;
        }
        public async Task<PickupLocations> AddPickupLocations(PickupLocations pickupLocations)
        {

            var courierUsers = _sqlServerContext.CourierUsers.FirstOrDefault(item => item.CourierUserId == pickupLocations.CourierUserId);

            if (courierUsers != null)
            {
                courierUsers.DistrictId = pickupLocations.DistrictId;
                courierUsers.ThanaId = pickupLocations.ThanaId;
                await _sqlServerContext.SaveChangesAsync();
            }

            var pickupLocationsData = _sqlServerContext.PickupLocations.FirstOrDefault(item => item.CourierUserId == pickupLocations.CourierUserId
            && item.IsActive.Equals(false)
            && item.DistrictId.Equals(pickupLocations.DistrictId)
            && item.ThanaId.Equals(pickupLocations.ThanaId));
            if (pickupLocationsData != null)
            {
                pickupLocationsData.DistrictId = pickupLocations.DistrictId;
                pickupLocationsData.ThanaId = pickupLocations.ThanaId;
                pickupLocationsData.Latitude = pickupLocations.Latitude;
                pickupLocationsData.Longitude = pickupLocations.Longitude;
                pickupLocationsData.PickupAddress = pickupLocations.PickupAddress;
                pickupLocationsData.IsActive = pickupLocations.IsActive;
                await _sqlServerContext.SaveChangesAsync();
            }
            else
            {

                await _sqlServerContext.PickupLocations.AddAsync(pickupLocations);
                await _sqlServerContext.SaveChangesAsync();
                return pickupLocations;
            }

            return pickupLocations;
        }

        public async Task<CollectionTimeSlot> AddCollectionTimeSlot(CollectionTimeSlot collectionTimeSlot)
        {
            await _sqlServerContext.CollectionTimeSlot.AddAsync(collectionTimeSlot);
            await _sqlServerContext.SaveChangesAsync();
            return collectionTimeSlot;
        }
        public async Task<WeightRange> AddWeightRange(WeightRange weightRange)
        {
            await _sqlServerContext.WeightRange.AddAsync(weightRange);
            await _sqlServerContext.SaveChangesAsync();
            return weightRange;
        }

        public async Task<IEnumerable<Hub>> GetAllHubs()
        {
            IQueryable<Hub> data = from w in _sqlServerContext.Hub.AsNoTracking()
                                   where w.IsActive == true
                                   select w;
            return await data.ToListAsync();
        }

        public async Task<IEnumerable<dynamic>> GetPickupLocations(int courierUserId)
        {
            //IQueryable<PickupLocations> data = from w in _sqlServerContext.PickupLocations.AsNoTracking()
            //                                   where w.CourierUserId == courierUserId
            //                                   select w;


            IQueryable<dynamic> data = from w in _sqlServerContext.PickupLocations.AsNoTracking()
                                       where w.CourierUserId == courierUserId
                                       && w.IsActive.Equals(true)
                                       orderby w.Id descending
                                       select new
                                       {
                                           w.Id,
                                           w.DistrictId,
                                           DistrictName = (_sqlServerContext.Districts.Where(x => x.DistrictId.Equals(w.DistrictId)).Select(x => x.DistrictBng)).FirstOrDefault(),
                                           DistrictNameEng = (_sqlServerContext.Districts.Where(x => x.DistrictId.Equals(w.DistrictId)).Select(x => x.District)).FirstOrDefault(),
                                           w.ThanaId,
                                           ThanaName = (_sqlServerContext.Districts.Where(x => x.DistrictId.Equals(w.ThanaId)).Select(x => x.DistrictBng)).FirstOrDefault(),
                                           ThanaNameEng = (_sqlServerContext.Districts.Where(x => x.DistrictId.Equals(w.ThanaId)).Select(x => x.District)).FirstOrDefault(),
                                           w.PickupAddress,
                                           w.Mobile,
                                           w.CourierUserId,
                                           w.Longitude,
                                           w.Latitude,
                                           CollectionTimeSlotId = (_sqlServerContext.Districts.Where(x => x.DistrictId.Equals(w.ThanaId)).Select(x => x.CollectionTimeSlotId)).FirstOrDefault()
                                       };
            return await data.ToListAsync();
        }
        public async Task<IEnumerable<dynamic>> GetPickupLocationsWithAcceptedOrderCount(int courierUserId)
        {
            //IQueryable<PickupLocations> data = from w in _sqlServerContext.PickupLocations.AsNoTracking()
            //                                   where w.CourierUserId == courierUserId
            //                                   select w;


            IQueryable<PickupLocationViewModel> data = from w in _sqlServerContext.PickupLocations.AsNoTracking()
                                                       where w.CourierUserId == courierUserId
                                                       select new PickupLocationViewModel
                                                       {
                                                           Id = w.Id,
                                                           DistrictId = w.DistrictId,
                                                           DistrictName = (_sqlServerContext.Districts.Where(x => x.DistrictId.Equals(w.DistrictId)).Select(x => x.DistrictBng)).FirstOrDefault(),
                                                           DistrictNameEng = (_sqlServerContext.Districts.Where(x => x.DistrictId.Equals(w.DistrictId)).Select(x => x.District)).FirstOrDefault(),
                                                           ThanaId = w.ThanaId,
                                                           ThanaName = (_sqlServerContext.Districts.Where(x => x.DistrictId.Equals(w.ThanaId)).Select(x => x.DistrictBng)).FirstOrDefault(),
                                                           ThanaNameEng = (_sqlServerContext.Districts.Where(x => x.DistrictId.Equals(w.ThanaId)).Select(x => x.District)).FirstOrDefault(),
                                                           PickupAddress = w.PickupAddress,
                                                           CourierUserId = w.CourierUserId,
                                                           Longitude = w.Longitude,
                                                           Latitude = w.Latitude,
                                                           AcceptedOrderCount = (_sqlServerContext.CourierOrders.Where(x => x.Status.Equals(41) && x.MerchantId.Equals(courierUserId) && x.CollectAddressThanaId.Equals(w.ThanaId)).Count())
                                                       };
            return await data.OrderByDescending(x => x.AcceptedOrderCount).ToListAsync();
        }
        public async Task<List<PriceListViewModel>> GetPriceList(int districtId, int deliveryRangeId)
        {
            using (var connection = new SqlConnection(_connectionStrings.MsSqlConnection))
            {
                connection.Open();

                var parameter = new DynamicParameters();
                parameter.Add(name: "@districtId", value: districtId, dbType: DbType.Int32);
                parameter.Add(name: "@deliveryRangeId", value: deliveryRangeId, dbType: DbType.Int32);

                var data = await connection.QueryAsync<PriceListViewModel>(
                        sql: @"[DT].[USP_GetPriceList]",
                        param: parameter,
                        commandType: CommandType.StoredProcedure);

                return data.ToList();
            }
        }

        public async Task<IEnumerable<DbActionBn>> GetDbActionBn()
        {
            IQueryable<DbActionBn> data = from w in _sqlServerContext.DbActionBn.AsNoTracking()
                                          select w;
            return await data.ToListAsync();
        }

        public async Task<int> GetMerchantCollectionCharge(int id)
        {
            decimal collectionCharge = 5;
            var courierUsers = await _sqlServerContext.CourierUsers.FirstOrDefaultAsync(m => m.CourierUserId.Equals(id));

            //var extraCharge = await _sqlServerContext.ExtraCharge.FirstOrDefaultAsync();

            int orders = _sqlServerContext.CourierOrders.Count(x => x.MerchantId.Equals(id) && x.OrderDate.Year == DateTime.Now.Year
            && x.OrderDate.Month == DateTime.Now.Month
            && x.OrderDate.Day == DateTime.Now.Day);

            if (orders == 0)
            {
                collectionCharge = courierUsers.FirstCollectionCharge;
            }
            else if (orders > 0)
            {
                collectionCharge = courierUsers.CollectionCharge;
            }

            return Convert.ToInt32(collectionCharge);
        }
        public bool GetMerchantCredit(int id)
        {
            int[] collectionArray = new int[] { 15, 24 };

            //int[] chargeArray = new int[] { 25, 29, 31, 56, 0, 1, 2, 4, 5, 6 };
            int[] chargeArray = new int[] { 15, 16, 17, 18, 19, 20, 21, 22, 24, 26, 27, 28, 29, 30, 38, 39 };


            var credit = (from m in _sqlServerContext.CourierUsers.AsNoTracking()
                          where m.CourierUserId == id
                          select m.Credit + m.AdvancePayment).FirstOrDefault();

            var TotalCollectionAmount = from o in _sqlServerContext.CourierOrders.AsNoTracking()
                                        where o.MerchantId == id
                                        && collectionArray.Contains(o.Status)
                                        select o.CollectionAmount;

            var TotalServiceCharge = from o in _sqlServerContext.CourierOrders.AsNoTracking()
                                     join s in _sqlServerContext.CourierOrderStatus.AsNoTracking()
                                     on o.Status equals s.StatusId
                                     where o.MerchantId == id
                                     //&& o.CollectionAmount == 0
                                     //&& s.FulfillmentStatusGroup.ToLower() != "deliverybondu"
                                     && chargeArray.Contains(s.StatusId)
                                     select o.BreakableCharge + o.CodCharge + o.DeliveryCharge
                                       + o.ReturnCharge + o.CollectionCharge + o.PackagingCharge;

            if (TotalServiceCharge.Sum() > (TotalCollectionAmount.Sum() + credit))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public async Task<List<WeightRange>> GetWeightRange()
        {
            IQueryable<WeightRange> data = from w in _sqlServerContext.WeightRange.AsNoTracking()
                                           where w.WeightNumber > 0
                                           select w;
            return await data.ToListAsync();
        }

        public async Task<dynamic> GetReturnOrders(int merchantId, int index, int count)
        {
            int ReturnOrderAccepted = 21;

            var data = _sqlServerContext.CourierOrders.Where(x => x.MerchantId.Equals(merchantId)
            && x.Status.Equals(ReturnOrderAccepted))
                .GroupBy(x => x.OrderDate.Date)
            .Select(g => new
            {
                Date = g.Key.Date,
                Name = "রিটার্ন প্রোডাক্ট",
                Order = g.Count(),
                Orders = g.ToList()
            })
            .OrderByDescending(i => i.Date)
            .Skip(index).Take(count);

            return await data.ToListAsync();
        }


        public async Task<dynamic> GetSurveyQuestion()
        {

            var subSurveyAnswer = _sqlServerContext.SubSurveyAnswer.ToList();

            var query = _sqlServerContext.SurveyQuestion
               .Join(_sqlServerContext.SurveyAnswer,
                  question => question.SurveyQuestionId,
                  answer => answer.SurveyQuestionId,
                  (question, answer) => new { Question = question, Answer = answer })
               .Where(questionAndanswer => questionAndanswer.Question.IsActive == true)
               .GroupBy(g => g.Question.SurveyQuestionId)
               .Select(g => new
               {
                   SurveyQuestionId = g.Key,
                   QuestionName = g.FirstOrDefault().Question.QuestionName,
                   IsMultipleAnswer = g.FirstOrDefault().Question.IsMultipleAnswer,
                   Ordering = g.FirstOrDefault().Question.Ordering,
                   ImageUrl = g.FirstOrDefault().Question.ImageUrl,
                   SurveyAnswerViewModel = g.Where(x => x.Answer.IsActive.Equals(true)).Select(x => new
                   {
                       SurveyAnswerId = x.Answer.SurveyAnswerId,
                       SurveyQuestionId = x.Answer.SurveyQuestionId,
                       AnswerName = x.Answer.AnswerName,
                       IsActive = x.Answer.IsActive,
                       SurveyRedirectNextQuestionId = x.Answer.SurveyRedirectNextQuestionId,
                       SubSurveyAnswerViewModel = subSurveyAnswer.Where(sa => sa.SurveyAnswerId.Equals(x.Answer.SurveyAnswerId))

                   })
               })
               .OrderBy(i => i.Ordering);

            return await query.ToListAsync();
        }

        public async Task<dynamic> DanaMatchColumn(string CourierUserId)
        {
            string[] Slug_Name = 
            { 
                "has_bank_ac","card_holder","card_limit",
                "has_creditcard","has_tin","edu_level",
                "monthly_exp","monthly_income","shop_ownership",
                "tin_number","trade_license","home_ownership",
                "married","fam_mem","residence_location"
            };

            string[] Dt_Name =
            {
                "IsBankAccount","CardHolder","CardLimit",
                "HasCreditCard","HasTin","EduLevel",
                "MonthlyExp","TransactionAmount","ShopOwnership",
                "TinNumber","TradeLicenseImageUrl","HomeOwnership",
                "Married","FamMem", "ResidenceLocation"
            };

            List<DanaToDtMatchColumnViewModel> danaToDts = new List<DanaToDtMatchColumnViewModel>();

            for (int i = 0; i < Slug_Name.Count(); i++)
            {
                danaToDts.Add(new DanaToDtMatchColumnViewModel
                {
                    DanaProperty = Slug_Name[i],
                    DtProperty = Dt_Name[i]
                });
            }

            string matchColumns = string.Join(",", Dt_Name);
            List<DanaToDtMatchColumnViewModel> matchColumnNameResult = new List<DanaToDtMatchColumnViewModel>();

            using (var connection = new SqlConnection(_connectionStrings.MsSqlConnection))
            {
                var paramter = new DynamicParameters();
                paramter.Add(name: "@courierUserId", value: CourierUserId, dbType: DbType.String);
                paramter.Add(name: "@matchColumns", value: matchColumns, dbType: DbType.String);

                matchColumnNameResult = (await connection.QueryAsync<DanaToDtMatchColumnViewModel>(
                    sql: @"[DT].[Usp_MatchColumnName]",
                    param: paramter,
                    commandType: CommandType.StoredProcedure
                    )).ToList();
            }

            var returnDanaProperty = (from first in danaToDts
                                      join second in matchColumnNameResult
                                      on first.DtProperty equals second.DtProperty
                                      select new DanaToDtMatchColumnViewModel
                                      {
                                          DanaProperty = first.DanaProperty,
                                          DtProperty = second.DtProperty,
                                          Value = second.Value
                                      }).ToList();
            return returnDanaProperty;
        }

        public async Task<int> UpdateDeliveryBonduAssignMultiple(List<DeliveryBonduAssign> deliveryBonduAssign)
        {
            int deliveryManUserId = (from m in deliveryBonduAssign
                                     select m.DeliveryManUserId).FirstOrDefault();

            int updatedBy = (from m in deliveryBonduAssign
                             select m.UpdatedBy).FirstOrDefault();

            var orderIds = (from d in deliveryBonduAssign
                            select d.OrderId).ToArray();

            var entity = await _sqlServerContext.DeliveryBonduAssign.AsNoTracking()
                .Where(x => orderIds.Contains(x.OrderId))
                .BatchUpdateAsync(x => new DeliveryBonduAssign
                {
                    DeliveryManUserId = deliveryManUserId,
                    UpdatedOn = DateTime.Now,
                    UpdatedBy = updatedBy
                });

            return entity;
        }

        public async Task<Collectors> UpdateTemporaryCollectors(int id, Collectors collectors)
        {
            var entity = await _sqlServerContext.Collectors.FirstOrDefaultAsync(item => item.CollectorId == id);
            if (entity != null)
            {
                entity.IsTemporary = collectors.IsTemporary;

                // Update entity in DbSet
                _sqlServerContext.Collectors.Update(entity);

                // Save changes in database
                await _sqlServerContext.SaveChangesAsync();
            }

            return entity;
        }
        public async Task<DeliveryUsers> UpdateNowOfflineRiders(int id, DeliveryUsers rider)
        {
            var entity = await _sqlServerContext.DeliveryUsers.FirstOrDefaultAsync(item => item.Id == id);
            if (entity != null)
            {
                entity.IsNowOffline = rider.IsNowOffline;

                // Update entity in DbSet
                _sqlServerContext.DeliveryUsers.Update(entity);

                // Save changes in database
                await _sqlServerContext.SaveChangesAsync();
            }

            return entity;
        }

        public async Task<DeliveryUsers> UpdateIsPermanentRider(int id, DeliveryUsers rider)
        {
            var entity = await _sqlServerContext.DeliveryUsers.FirstOrDefaultAsync(item => item.Id == id);
            if (entity != null)
            {
                entity.IsPermanentRider = rider.IsPermanentRider;

                // Update entity in DbSet
                _sqlServerContext.DeliveryUsers.Update(entity);

                // Save changes in database
                await _sqlServerContext.SaveChangesAsync();
            }

            return entity;
        }
        public async Task<DeliveryUsers> UpdateRiderTypeOfDeliveryBondhu(int id, DeliveryUsers rider)
        {
            var entity = await _sqlServerContext.DeliveryUsers.FirstOrDefaultAsync(item => item.Id == id);
            if (entity != null)
            {
                entity.RiderType = rider.RiderType;

                // Update entity in DbSet
                _sqlServerContext.DeliveryUsers.Update(entity);

                // Save changes in database
                await _sqlServerContext.SaveChangesAsync();
            }

            return entity;
        }
        public async Task<WeightRange> UpdateWeightRange(int id, WeightRange weightRange)
        {
            var entity = await _sqlServerContext.WeightRange.FirstOrDefaultAsync(item => item.Id == id);
            if (entity != null)
            {
                entity.Weight = weightRange.Weight;
                entity.Type = weightRange.Type;
                entity.WeightNumber = weightRange.WeightNumber;
                entity.ExpressTypeCourierDeliveryCharge = weightRange.ExpressTypeCourierDeliveryCharge;
                entity.RegularTypeCourierDeliveryCharge = weightRange.RegularTypeCourierDeliveryCharge;

                // Update entity in DbSet
                _sqlServerContext.WeightRange.Update(entity);

                // Save changes in database
                await _sqlServerContext.SaveChangesAsync();
            }

            return entity;
        }

        public async Task<DeliveryRange> UpdateImageLink(DeliveryRange deliveryRange)
        {
            var entity = await _sqlServerContext.DeliveryRange.FirstOrDefaultAsync(item => item.Id == deliveryRange.Id);
            if (entity != null)
            {
                if (deliveryRange.OnImageLink != "")
                {
                    entity.OnImageLink = deliveryRange.OnImageLink;
                }


                if (deliveryRange.OffImageLink != "")
                {
                    entity.OffImageLink = deliveryRange.OffImageLink;
                }

                // Update entity in DbSet
                _sqlServerContext.DeliveryRange.Update(entity);

                // Save changes in database
                await _sqlServerContext.SaveChangesAsync();
            }

            return entity;

        }


        public async Task<PickupLocations> UpdatePickupLocations(int id, PickupLocations pickupLocations)
        {
            var entity = await _sqlServerContext.PickupLocations.FirstOrDefaultAsync(item => item.Id == id);
            if (entity != null)
            {
                if (pickupLocations.DistrictId > 0)
                {
                    entity.DistrictId = pickupLocations.DistrictId;
                }
                if (pickupLocations.ThanaId > 0)
                {
                    entity.ThanaId = pickupLocations.ThanaId;
                }

                if (pickupLocations.AreaId > 0)
                {
                    entity.AreaId = pickupLocations.AreaId;
                }
                if (!String.IsNullOrEmpty(pickupLocations.PickupAddress))
                {
                    entity.PickupAddress = pickupLocations.PickupAddress;
                }
                if (pickupLocations.Latitude != "")
                {
                    entity.Latitude = pickupLocations.Latitude;
                }
                if (pickupLocations.Longitude != "")
                {
                    entity.Longitude = pickupLocations.Longitude;
                }

                if (!String.IsNullOrEmpty(pickupLocations.Mobile))
                {
                    entity.Mobile = pickupLocations.Mobile;
                }
                // Update entity in DbSet
                _sqlServerContext.PickupLocations.Update(entity);

                // Save changes in database
                await _sqlServerContext.SaveChangesAsync();
            }

            return entity;
        }

        public async Task<CourierUsers> UpdatePaymentCycle(CourierUsers users)
        {
            var entity = await _sqlServerContext.CourierUsers.FirstOrDefaultAsync(item => item.CourierUserId == users.CourierUserId);
            if (entity != null)
            {
                entity.BkashNumber = users.BkashNumber;
                entity.PreferredPaymentCycle = users.PreferredPaymentCycle;
                entity.PreferredPaymentCycleDate = DateTime.Now;

                _sqlServerContext.CourierUsers.Update(entity);

                //Changes are updated and Saved in Database
                await _sqlServerContext.SaveChangesAsync();
            }
            return entity;

        }


        public async Task<LocationAssign> UpdateLocationAssign(int id, LocationAssign locationAssign)
        {
            var entity = await _sqlServerContext.LocationAssign.FirstOrDefaultAsync(item => item.LocationAssignId == id);
            if (entity != null)
            {
                entity.DistrictId = locationAssign.DistrictId;
                entity.ThanaId = locationAssign.ThanaId;
                entity.AreaId = locationAssign.AreaId;
                entity.CollectorId = locationAssign.CollectorId;
                entity.DeliveryUserId = locationAssign.DeliveryUserId;
                entity.AdDefaultAssign = locationAssign.AdDefaultAssign;
                entity.DtDefaultAssign = locationAssign.DtDefaultAssign;
                entity.ZoneId = locationAssign.ZoneId;
                entity.UpdatedBy = locationAssign.UpdatedBy;
                entity.UpdatedOn = DateTime.Now;
                // Update entity in DbSet
                _sqlServerContext.LocationAssign.Update(entity);

                // Save changes in database
                await _sqlServerContext.SaveChangesAsync();

                var insertEntity = await _sqlServerContext.LocationAssign.AsNoTracking().Where(b => b.LocationAssignId.Equals(id)).FirstOrDefaultAsync();

                var locationAssignHistory =  new LocationAssignHistory()
                {
                    DeliveryUserId = insertEntity.DeliveryUserId,
                    CollectorId = insertEntity.CollectorId,
                    DistrictId = insertEntity.DistrictId,
                    ThanaId = insertEntity.ThanaId,
                    AreaId = insertEntity.AreaId,
                    DtDefaultAssign = insertEntity.DtDefaultAssign,
                    AdDefaultAssign = insertEntity.AdDefaultAssign,
                    ZoneId = insertEntity.ZoneId,
                    InsertedBy = insertEntity.InsertedBy,
                    UpdatedBy = insertEntity.UpdatedBy
                };

                await _sqlServerContext.LocationAssignHistory.AddAsync(locationAssignHistory);
                await _sqlServerContext.SaveChangesAsync();
                
            }

            return entity;
        }

        public async Task<DbActionBn> UpdateDbActionBn(int id, DbActionBn dbActionBn)
        {
            var entity = await _sqlServerContext.DbActionBn.FirstOrDefaultAsync(item => item.Id == id);
            if (entity != null)
            {
                entity.ActionType = dbActionBn.ActionType;
                entity.ActionMessage = dbActionBn.ActionMessage;
                entity.UpdateStatus = dbActionBn.UpdateStatus;
                entity.StatusMessage = dbActionBn.StatusMessage;
                entity.ColorCode = dbActionBn.ColorCode;
                entity.Icon = dbActionBn.Icon;
                entity.IsPaymentType = dbActionBn.IsPaymentType;
                entity.ProjectType = dbActionBn.ProjectType;
                // Update entity in DbSet
                _sqlServerContext.DbActionBn.Update(entity);

                // Save changes in database
                await _sqlServerContext.SaveChangesAsync();
            }

            return entity;
        }

        public async Task<OfferInformationViewModel> GetOfferInformation(int orderid, string offercode)
        {
            var res = from o in _sqlServerContext.CourierOrders
                      join m in _sqlServerContext.CourierUsers on o.MerchantId equals m.CourierUserId
                      where o.Id == orderid
                      && o.OfferCode == offercode
                      select new OfferInformationViewModel
                      {
                          CourierOrdersId = o.CourierOrdersId,
                          CustomerName = o.CustomerName,
                          Mobile = o.Mobile,
                          MerchantMobile = m.Mobile,
                          Address = o.Address,
                          OrderDate = o.OrderDate,
                          CollectionAmount = o.CollectionAmount,
                          CollectionName = o.CollectionName,
                          DeliveryCharge = o.DeliveryCharge,
                          CompanyName = m.CompanyName
                      };

            return await res.FirstOrDefaultAsync();
        }

        public async Task<Hub> GetHubsByPickupLocation(PickupLocations pickupLocation)
        {

            var hub = _sqlServerContext.LocationAssign
                .Join(_sqlServerContext.DeliveryZone, la => la.ZoneId, dz => dz.ZoneId, (la, dz) => new { la, dz })
                .Join(_sqlServerContext.Hub, ladz => ladz.dz.HubId, h => h.Id, (ladz, h) => new { ladz, h })
                .Where(w => w.ladz.la.DistrictId.Equals(pickupLocation.DistrictId) && w.ladz.la.ThanaId.Equals(pickupLocation.ThanaId))
                .Select(m => new Hub
                {
                    Id = m.h.Id,
                    Name = m.h.Name,
                    Value = m.h.Value,
                    HubAddress = m.h.HubAddress,
                    Longitude = m.h.Longitude,
                    Latitude = m.h.Latitude,
                    HubMobile = m.h.HubMobile,
                    IsActive = m.h.IsActive
                });

            return await hub.FirstOrDefaultAsync();
        }

        public async Task<PickupLocations> UpdatePickupLocationsForLatLong(PickupLocations pickupLocations)
        {
            var entity = await _sqlServerContext.PickupLocations
                .FirstOrDefaultAsync(item => item.DistrictId == pickupLocations.DistrictId
                && item.ThanaId == pickupLocations.ThanaId
                && item.IsActive == true
                && item.CourierUserId == pickupLocations.CourierUserId);

            if (entity != null)
            {
                if (pickupLocations.Latitude != "")
                {
                    entity.Latitude = pickupLocations.Latitude;
                }
                if (pickupLocations.Longitude != "")
                {
                    entity.Longitude = pickupLocations.Longitude;
                }
                // Update entity in DbSet
                _sqlServerContext.PickupLocations.Update(entity);

                // Save changes in database
                await _sqlServerContext.SaveChangesAsync();
            }

            return entity;
        }

        public async Task<int> UpdatePriceWithWeight(DeliveryChargeDetailsBodyModel deliveryChargeDetailsBodyModel)
        {

            int startIndex = 3;
            int endIndex = deliveryChargeDetailsBodyModel.CourierOrderId.Length - 3;
            int id = Convert.ToInt32(deliveryChargeDetailsBodyModel.CourierOrderId.Substring(startIndex, endIndex));

            var entity = new DeliveryChargeDetails();

            if (deliveryChargeDetailsBodyModel.DistrictId > 0 && deliveryChargeDetailsBodyModel.ThanaId > 0 && deliveryChargeDetailsBodyModel.AreaId > 0 && deliveryChargeDetailsBodyModel.DeliveryRangeId > 0)
            {
                entity = await _sqlServerContext.DeliveryChargeDetails
                .FirstOrDefaultAsync(item => item.DistrictId == deliveryChargeDetailsBodyModel.DistrictId
                && item.ThanaId == deliveryChargeDetailsBodyModel.ThanaId
                && item.AreaId == deliveryChargeDetailsBodyModel.AreaId
                && item.WeightRangeId == deliveryChargeDetailsBodyModel.WeightRangeId
                && item.DeliveryRangeId == deliveryChargeDetailsBodyModel.DeliveryRangeId);
            }

            else if (deliveryChargeDetailsBodyModel.DistrictId > 0 && deliveryChargeDetailsBodyModel.ThanaId > 0)
            {
                entity = await _sqlServerContext.DeliveryChargeDetails
                .FirstOrDefaultAsync(item => item.DistrictId == deliveryChargeDetailsBodyModel.DistrictId
                && item.ThanaId == deliveryChargeDetailsBodyModel.ThanaId
                && item.WeightRangeId == deliveryChargeDetailsBodyModel.WeightRangeId);
            }
            else if (deliveryChargeDetailsBodyModel.DistrictId > 0)
            {
                entity = await _sqlServerContext.DeliveryChargeDetails
                .FirstOrDefaultAsync(item => item.DistrictId == deliveryChargeDetailsBodyModel.DistrictId
                && item.WeightRangeId == deliveryChargeDetailsBodyModel.WeightRangeId);
            }

            if (entity != null)
            {
                var entityOrder = _sqlServerContext.CourierOrders
                .FirstOrDefault(item => item.Id.Equals(id));

                entityOrder.WeightRangeId = entity.WeightRangeId;
                entityOrder.DeliveryCharge = entity.CourierDeliveryCharge + deliveryChargeDetailsBodyModel.ExtraCollectionCharge;

                // Update entity in DbSet
                _sqlServerContext.CourierOrders.Update(entityOrder);

                // Save changes in database
                return await _sqlServerContext.SaveChangesAsync();
            }

            return 0;
        }

        public async Task<List<Category>> GetAssignCourierUserCategory(AssignCourierUserCategory assignCourierUserCategory)
        {
            var query = _sqlServerContext.AssignCourierUserCategory
                .Where(x => x.CourierUserId.Equals(assignCourierUserCategory.CourierUserId))
                .Join(_sqlServerContext.Category,
                usercategory => usercategory.CategoryId,
                category => category.CategoryId,
                (usercategory, category) => new Category
                {
                    CategoryId = category.CategoryId,
                    CategoryNameBng = category.CategoryNameBng,
                    CategoryNameEng = category.CategoryNameEng
                });

            return await query.ToListAsync();
        }

        public async Task<List<Category>> GetDtCategories(bool isActive)
        {

            return await _sqlServerContext.Category.Where(x => x.IsActive.Equals(isActive)).ToListAsync();
            //return data.ToList();
        }

        public async Task<List<Category>> GetCategoriesForAdmin(bool isActive)
        {
            if(!isActive)
                return await _sqlServerContext.Category.ToListAsync();
            else
                return await _sqlServerContext.Category.Where(x => x.IsActive.Equals(isActive)).ToListAsync();
        }
        public async Task<List<SubCategory>> GetSubCategories(bool isActive)
        {
            return await _sqlServerContext.SubCategory.Where(x => x.IsActive.Equals(isActive)).ToListAsync();
        }
        public async Task<List<SubCategory>> GetSubCategoryById(bool isActive, int categoryId)
        {
            return await _sqlServerContext.SubCategory.Where(x => x.IsActive.Equals(isActive) && x.CategoryId.Equals(categoryId)).ToListAsync();
        }
        public async Task<List<Districts>> GetServiceDistricts(int[] rangeId)
        {


            var districtIds = _sqlServerContext.DeliveryChargeDetails
                .Where(d => rangeId.Contains(d.DeliveryRangeId) && d.IsActive.Equals(true))
                .Select(x => x.DistrictId).Distinct().ToArray();

            var thanaIds = _sqlServerContext.DeliveryChargeDetails
                .Where(d => rangeId.Contains(d.DeliveryRangeId) && d.IsActive.Equals(true))
                .Select(x => x.ThanaId).Distinct().ToArray();

            var areaIds = _sqlServerContext.DeliveryChargeDetails
                .Where(d => rangeId.Contains(d.DeliveryRangeId) && d.IsActive.Equals(true))
                .Select(x => x.AreaId).Distinct().ToArray();


            var allIds = districtIds.Union(thanaIds).Union(areaIds);

            var data = await _sqlServerContext.Districts.Where(x => allIds.Contains(x.DistrictId)
                && x.IsActive.Equals(true)
                //&& x.DistrictId < 20000
                )
                .Select(z => new Districts
                {
                    DistrictId = z.DistrictId,
                    District = z.District,
                    DistrictBng = z.DistrictBng,
                    DistrictPriority = z.DistrictPriority,
                    ParentId = z.ParentId,
                    IsCity = z.IsCity,
                    IsActiveForCorona = z.IsActiveForCorona,
                    NextDayAlertMessage = z.NextDayAlertMessage

                }).OrderBy(o => o.DistrictPriority).ToListAsync();

            return data;

        }

        public async Task<List<CourierOrdersViewModel>> GetSABookingReport(RequestBodyModel request)
        {
            var data = from a in _sqlServerContext.CourierOrders.AsNoTracking()
                       join b in _sqlServerContext.Couriers.AsNoTracking() on a.CourierId equals b.CourierId
                       join c in _sqlServerContext.Districts.AsNoTracking() on a.DistrictId equals c.DistrictId

                       join d in _sqlServerContext.Districts.AsNoTracking() on a.ThanaId equals d.DistrictId into ps1
                       from subd in ps1.DefaultIfEmpty()
                       join e in _sqlServerContext.Districts.AsNoTracking() on a.AreaId equals e.DistrictId into ps2
                       from sube in ps2.DefaultIfEmpty()
                       join f in _sqlServerContext.CourierOrderStatus.AsNoTracking() on a.Status equals f.StatusId
                       join h in _sqlServerContext.CourierOrderStatusHistory.AsNoTracking() on a.CourierOrdersId equals h.CourierOrderId

                       where
                            //a.InvoiceCourier == "sa" && a.InvoiceNumber != "" &&
                            b.CourierId == (request.CourierId == -1 ? b.CourierId : request.CourierId)
                            && a.DeliveryRangeId == (request.DeliveryRangeId == -1 ? a.DeliveryRangeId : request.DeliveryRangeId)
                            && a.DistrictId == (request.DistrictId == 0 ? a.DistrictId : request.DistrictId)
                            && a.Status == (request.StatusId == -1 ? a.Status : request.StatusId)

                            && h.Status == 36 && (h.Comment == "Sent To Hub-" + request.HubName || h.Comment == "Sent To Hub-36-" + request.HubName)
                            && h.PostedOn >= request.FromDate && h.PostedOn < request.ToDate.AddDays(1)

                       select new CourierOrdersViewModel
                       {
                           Id = a.Id,
                           PodNumber = a.PodNumber,
                           InvoiceNumber = a.InvoiceNumber,
                           PaymentType = a.PaymentType,
                           CollectionAmount = a.CollectionAmount,
                           Mobile = a.Mobile,
                           CustomerName = a.CustomerName,
                           Couriers = new CouriersViewModel
                           {
                               CourierName = b.CourierName
                           },
                           CourierOrderStatus = new OrderStatusViewModel
                           {
                               StatusNameEng = f.StatusNameEng
                           },
                           DistrictsViewModel = new DistrictsViewModel
                           {
                               District = c.District,
                               Thana = subd.District,
                               Area = sube.District,
                               EdeshMobileNo = c.EDeshMobileNo,
                               TigerMobileNo = c.TigerMobileNo
                           }

                       };
            return await data.Distinct().ToListAsync();
        }


        public async Task<IEnumerable<dynamic>> GetDetailedSAReport(RequestBodyModel request)
        {
            //var allDistricts = await _sqlServerContext.Districts.AsNoTracking().Where(x => x.ParentId == 0 && x.IsActive == true).ToListAsync();

            var context = await (from co in _sqlServerContext.CourierOrders.AsNoTracking()
                                 join d in _sqlServerContext.Districts.AsNoTracking() on co.DistrictId equals d.DistrictId
                                 join _orderStatus in _sqlServerContext.CourierOrderStatus.AsNoTracking() on co.Status equals _orderStatus.StatusId
                                 join c in _sqlServerContext.Couriers.AsNoTracking() on co.CourierId equals c.CourierId
                                 join h in _sqlServerContext.CourierOrderStatusHistory.AsNoTracking() on co.CourierOrdersId equals h.CourierOrderId

                                 where h.Status == 36 && h.Comment == "Sent To Hub-SA হাব"
                                 && h.PostedOn >= request.FromDate && h.PostedOn < request.ToDate.AddDays(1)
                                 && co.CourierId == (request.CourierId == 0 ? co.CourierId : request.CourierId)

                                 select new CourierOrdersViewModel
                                 {
                                     Id = co.Id,
                                     PodNumber = co.PodNumber,
                                     InvoiceNumber = co.InvoiceNumber,
                                     PaymentType = co.PaymentType,
                                     CollectionAmount = co.CollectionAmount,
                                     Status = co.Status,
                                     Couriers = new CouriersViewModel
                                     {
                                         CourierName = c.CourierName
                                     },
                                     CourierOrderStatus = new OrderStatusViewModel
                                     {
                                         StatusNameEng = _orderStatus.StatusNameEng
                                     },
                                     DistrictsViewModel = new DistrictsViewModel
                                     {
                                         DistrictId = d.DistrictId,
                                         District = d.District,
                                         EdeshMobileNo = d.EDeshMobileNo,
                                         TigerMobileNo = d.TigerMobileNo
                                     }

                                 }).Distinct().ToListAsync();

            var report = context.GroupBy(x => x.DistrictsViewModel.DistrictId).
            Select(y => new
            {
                DistrictId = y.Key,
                District = y.FirstOrDefault().DistrictsViewModel.District,
                SentToHub = new
                {
                    TotalCount = y.Count(),
                    CourierOrders = y
                },
                InvoiceEmpty = new
                {
                    TotalCount = y.Where(z => z.InvoiceNumber == "").Count(),
                    CourierOrders = y.Where(z => z.InvoiceNumber == "")
                },
                NotRecieved = new
                {
                    TotalCount = y.Where(z => z.Status != 37).Count(),
                    CourierOrders = y.Where(z => z.Status != 37 && z.Comment != "Received From Hub-" + y.FirstOrDefault().DistrictsViewModel.District)
                }
            }).OrderBy(a => a.District).ToList();

            return report;
        }

        public async Task<int> UpdateBulkStatus(List<CourierOrders> request)
        {
            int startIndex = 3;
            var ids = request.Select(s => Convert.ToInt32(s.CourierOrdersId.Substring(startIndex, s.CourierOrdersId.Length - startIndex))).ToArray();

            var data = _sqlServerContext.CourierOrders.Where(x => ids.Contains(x.Id));

            foreach (var item in data)
            {
                var requestData = request.Where(x=> x.CourierOrdersId.Equals("DT-"+item.Id)).FirstOrDefault();

                item.Comment = requestData.Comment;
                item.Status = requestData.Status;
                item.IsConfirmedBy = requestData.IsConfirmedBy;
                item.HubName = requestData.HubName;
                item.UpdatedBy = requestData.UpdatedBy;
                item.UpdatedOn = DateTime.Now;
                item.ReAttemptCharge = requestData.ReAttemptCharge > 0 ? requestData.ReAttemptCharge : item.ReAttemptCharge;

                _sqlServerContext.CourierOrders.Update(item);

            }
            var entity = await _sqlServerContext.SaveChangesAsync();

            //var comment = request.Select(x => x.Comment).FirstOrDefault();
            //var status = request.Select(x => x.Status).FirstOrDefault();
            //var isConfirmberBy = request.Select(x => x.IsConfirmedBy).FirstOrDefault();
            //var hubName = request.Select(x => x.HubName).FirstOrDefault();
            //var updatedBy = request.Select(x => x.UpdatedBy).FirstOrDefault();
            //var reAttemptCharge = request.Select(x => x.ReAttemptCharge).FirstOrDefault();

            //var entity = await _sqlServerContext.CourierOrders.AsNoTracking().Where(x => ids.Contains(x.Id))
            //                    .BatchUpdateAsync(x => new CourierOrders
            //                    {
            //                        Status = status,
            //                        Comment = comment,
            //                        IsConfirmedBy = isConfirmberBy,
            //                        HubName = hubName,
            //                        UpdatedBy = updatedBy,
            //                        UpdatedOn = DateTime.Now,
            //                        ReAttemptCharge = reAttemptCharge > 0 ? reAttemptCharge : 0
            //                    });


            var entry = _sqlServerContext.CourierOrders.AsNoTracking().Where(x => ids.Contains(x.Id));


            var courierOrderStatusHistory = entry.Select(x => new CourierOrderStatusHistory()
            {
                CourierOrderId = x.CourierOrdersId,
                IsConfirmedBy = x.IsConfirmedBy,
                OrderDate = x.OrderDate,
                Status = x.Status,
                PostedBy = x.UpdatedBy,
                MerchantId = x.MerchantId,
                Comment = x.Comment,
                PodNumber = x.PodNumber,
                CourierId = x.CourierId,
                HubName = x.HubName,
                CourierDeliveryManName = x.CourierDeliveryManName,
                CourierDeliveryManMobile = x.CourierDeliveryManMobile,
                PostedOn = x.UpdatedOn
            }).ToList();

            await _sqlServerContext.BulkInsertAsync(courierOrderStatusHistory);


            return entity;
        }

        public async Task<List<Districts>> GetServiceDistricts(int deliveryRangeId)
        {
            if (deliveryRangeId == 17)
            {
                int[] nextDayArray = new int[] { 14, 17 };

                var districtIds = _sqlServerContext.DeliveryChargeDetails
                    .Where(d => nextDayArray.Contains(d.DeliveryRangeId) && d.IsActive.Equals(true))
                    .Select(x => x.DistrictId).Distinct().ToArray();

                var thanaIds = _sqlServerContext.DeliveryChargeDetails
                    .Where(d => nextDayArray.Contains(d.DeliveryRangeId) && d.IsActive.Equals(true))
                    .Select(x => x.ThanaId).Distinct().ToArray();

                var areaIds = _sqlServerContext.DeliveryChargeDetails
                    .Where(d => nextDayArray.Contains(d.DeliveryRangeId) && d.IsActive.Equals(true))
                    .Select(x => x.AreaId).Distinct().ToArray();


                var allIds = districtIds.Union(thanaIds).Union(areaIds);

                var data = await _sqlServerContext.Districts.Where(x => allIds.Contains(x.DistrictId)
                    && x.IsActive.Equals(true)
                    //&& x.DistrictId < 20000
                    )
                    .Select(z => new Districts
                    {
                        DistrictId = z.DistrictId,
                        District = z.District,
                        DistrictBng = z.DistrictBng,
                        DistrictPriority = z.DistrictPriority,
                        ParentId = z.ParentId,
                        IsCity = z.IsCity,
                        IsActiveForCorona = z.IsActiveForCorona,
                        NextDayAlertMessage = z.NextDayAlertMessage

                    }).OrderBy(o => o.DistrictPriority).ToListAsync();

                return data;

            }
            else
            {

                var districtIds = _sqlServerContext.DeliveryChargeDetails
                    .Where(d => d.DeliveryRangeId.Equals(deliveryRangeId) && d.IsActive.Equals(true))
                    .Select(x => x.DistrictId).Distinct().ToArray();

                var thanaIds = _sqlServerContext.DeliveryChargeDetails
                    .Where(d => d.DeliveryRangeId.Equals(deliveryRangeId) && d.IsActive.Equals(true))
                    .Select(x => x.ThanaId).Distinct().ToArray();

                var areaIds = _sqlServerContext.DeliveryChargeDetails
                    .Where(d => d.DeliveryRangeId.Equals(deliveryRangeId) && d.IsActive.Equals(true))
                    .Select(x => x.AreaId).Distinct().ToArray();


                var allIds = districtIds.Union(thanaIds).Union(areaIds);


                var data = await _sqlServerContext.Districts.Where(x => allIds.Contains(x.DistrictId)
                    && x.IsActive.Equals(true)
                    //&& x.DistrictId < 20000
                    )
                    .Select(z => new Districts
                    {
                        DistrictId = z.DistrictId,
                        District = z.District,
                        DistrictBng = z.DistrictBng,
                        DistrictPriority = z.DistrictPriority,
                        ParentId = z.ParentId,
                        IsCity = z.IsCity,
                        IsActiveForCorona = z.IsActiveForCorona

                    }).OrderBy(o => o.DistrictPriority).ToListAsync();

                return data;
            }

        }


        public async Task<IEnumerable<DeliveredReturnedCountModel>> GetDeliveredReturnedCount(LoadCourierOrderBodyModel bodyModel)
        {
            using (var connection = new SqlConnection(_connectionStrings.MsSqlConnection))
            {
                connection.Open();

                var parameter = new DynamicParameters();
                parameter.Add(name: "@FromDate", value: bodyModel.FromDate, dbType: DbType.DateTime);
                parameter.Add(name: "@ToDate", value: bodyModel.ToDate, dbType: DbType.DateTime);
                parameter.Add(name: "@MerchantId", value: bodyModel.CourierUserId, dbType: DbType.Int32);

                var data = await connection.QueryAsync<DeliveredReturnedCountModel>(
                        sql: @"[DT].[USP_GetDeliveredReturnedCount]",
                        param: parameter,
                        commandType: CommandType.StoredProcedure);

                return data.ToList();
            }
        }

        public async Task<IEnumerable<DeliveredReturnedDetailsViewModel>> GetDeliveredReturnedCountWiseDetails(RequestBodyModel bodyModel)
        {
            using (var connection = new SqlConnection(_connectionStrings.MsSqlConnection))
            {
                connection.Open();
                var parameter = new DynamicParameters();

                parameter.Add(name: "@FromDate", value: bodyModel.FromDate, dbType: DbType.DateTime);
                parameter.Add(name: "@ToDate", value: bodyModel.ToDate, dbType: DbType.DateTime);
                parameter.Add(name: "@MerchantId", value: bodyModel.MerchantId, dbType: DbType.Int32);
                parameter.Add(name: "@Type", value: bodyModel.Type, dbType: DbType.String);

                var data = await connection.QueryAsync<DeliveredReturnedDetailsViewModel>(
                        sql: @"[DT].[USP_GetDeliveredReturnedCountWiseDetails]",
                        param: parameter,
                        commandType: CommandType.StoredProcedure);

                return data.ToList();
            }
        }

        public async Task<IEnumerable<BondhuAppMismatchDataViewModel>> BondhuAppMismatchData(RequestBodyModel bodyModel)
        {
            using (var connection = new SqlConnection(_connectionStrings.MsSqlConnection))
            {
                connection.Open();

                var parameter = new DynamicParameters();
                parameter.Add(name: "@FromDate", value: bodyModel.FromDate, dbType: DbType.DateTime);
                parameter.Add(name: "@ToDate", value: bodyModel.ToDate, dbType: DbType.DateTime);

                var data = await connection.QueryAsync<BondhuAppMismatchDataViewModel>(
                        sql: @"[DT].[USP_BondhuAppMismatchData]",
                        param: parameter,
                        commandType: CommandType.StoredProcedure);

                return data.ToList();
            }
        }

        public async Task<CourierOrdersViewModel> GetAcceptedCourierOrders(int courierUserId)
        {
            var slot = await _sqlServerContext.CollectionTimeSlot.Where(x => DateTime.Now.TimeOfDay >= x.StartTime.Value
            && DateTime.Now.TimeOfDay <= x.EndTime.Value
            && x.IsActive == true).FirstOrDefaultAsync();

            var orders = from _courierOrders in _sqlServerContext.CourierOrders.AsNoTracking()
                         join _deliveryman in _sqlServerContext.DeliveryUsers.AsNoTracking() on _courierOrders.UpdatedBy equals _deliveryman.Id
                         join _collectionTimeSlot in _sqlServerContext.CollectionTimeSlot.AsNoTracking() on _courierOrders.CollectionTimeSlotId equals _collectionTimeSlot.CollectionTimeSlotId

                         where _courierOrders.Status == 41
                         && _courierOrders.IsConfirmedBy.Equals("deliveryman")
                         && _courierOrders.MerchantId == courierUserId
                         && _collectionTimeSlot.CollectionTimeSlotId == slot.CollectionTimeSlotId
                         //&& _courierOrders.OrderDate.Date == DateTime.Now.Date
                         orderby _courierOrders.Id descending
                         select new CourierOrdersViewModel
                         {
                             CourierOrdersId = "DT-" + _courierOrders.Id,
                             RiderAcceptDate = _courierOrders.UpdatedOn,
                             DeliveryUsersViewModel = new DeliveryUsersViewModel
                             {
                                 Name = _deliveryman.Name
                             },
                             CollectionTimeSlot = new CollectionTimeSlotViewModel
                             {
                                 StartTime = _collectionTimeSlot.StartTime,
                                 EndTime = _collectionTimeSlot.EndTime
                             }
                         };

            return await orders.FirstOrDefaultAsync();
        }

        public async Task<bool> SlotChangeSMSandNotification(List<CourierOrders> orders)
        {

            var timeSlot = _sqlServerContext.CollectionTimeSlot.AsNoTracking().Where(x => x.IsActive == true).ToList();

            var orderIds = (from order in orders
                            select order.Id).ToArray();

            var list = (from _courierOrders in _sqlServerContext.CourierOrders.AsNoTracking()
                        join _collectionTimeSlot in _sqlServerContext.CollectionTimeSlot.AsNoTracking() on _courierOrders.CollectionTimeSlotId equals _collectionTimeSlot.CollectionTimeSlotId
                        //join order in orders on _courierOrders.Id equals order.Id
                        join _courierUsers in _sqlServerContext.CourierUsers.AsNoTracking() on _courierOrders.MerchantId equals _courierUsers.CourierUserId

                        where orderIds.Contains(_courierOrders.Id)
                        select new
                        {
                            CourierOrdersId = "DT-" + _courierOrders.Id,
                            _courierOrders.MerchantId,
                            CourierUsers = new CourierUsersViewModel
                            {
                                Mobile = _courierUsers.Mobile,
                                FirebaseToken = _courierUsers.FirebaseToken
                            },
                            CurrentCollectionTimeSlot = new
                            {
                                EndTime = _collectionTimeSlot.EndTime
                            },
                            NextCollectionTimeSlot = _collectionTimeSlot.EndTime >= new TimeSpan(21, 0, 0) &&
                            _collectionTimeSlot.EndTime <= new TimeSpan(23, 50, 0) ? timeSlot.Where(y => y.CollectionTimeSlotId == 1).Select(k => new
                            {
                                EndTime = k.EndTime,
                                NextDay = true
                            }).FirstOrDefault()
                            : timeSlot.Where(x => x.StartTime == _collectionTimeSlot.EndTime).Select(k => new
                            {
                                EndTime = k.EndTime,
                                NextDay = false
                            }).FirstOrDefault()
                        }).ToList();


            var courierOrders = list.GroupBy(x => x.MerchantId).Select(y => new
            {
                CourierOrdersId = y.Select(z => z.CourierOrdersId).ToList(),
                CourierUsers = y.FirstOrDefault().CourierUsers,
                CurrentCollectionTimeSlot = y.FirstOrDefault().CurrentCollectionTimeSlot,
                NextCollectionTimeSlot = y.FirstOrDefault().NextCollectionTimeSlot
            }).ToList();

            var smsBody = "Sorry, we could not collect parcel(dt-code) within slot-endTime. It will be collected within nextSlot-endTime";
            var smsList = new List<SendMobileBodyModel>();
            foreach (var courierObj in courierOrders)
            {
                var notification = courierObj.NextCollectionTimeSlot.NextDay == true ?
                    smsBody.Replace("dt-code", string.Join(", ", courierObj.CourierOrdersId)).Replace("slot-endTime", new DateTime(courierObj.CurrentCollectionTimeSlot.EndTime.Value.Ticks).ToString("hh:mm tt"))
                    .Replace("nextSlot-endTime", "next day " + new DateTime(courierObj.NextCollectionTimeSlot.EndTime.Value.Ticks).ToString("hh:mm tt"))
                    :
                    smsBody.Replace("dt-code", string.Join(", ", courierObj.CourierOrdersId)).Replace("slot-endTime", new DateTime(courierObj.CurrentCollectionTimeSlot.EndTime.Value.Ticks).ToString("hh:mm tt")).Replace("nextSlot-endTime", "today " + new DateTime(courierObj.NextCollectionTimeSlot.EndTime.Value.Ticks).ToString("hh:mm tt"));

                var smsModel = new SendMobileBodyModel
                {
                    numbers = new string[] { courierObj.CourierUsers.Mobile },
                    text = notification,
                    type = 0,
                    datacoding = 0
                };

                smsList.Add(smsModel);

                if (courierObj.CourierUsers.FirebaseToken != "")
                {
                    var orderStatus = new CourierOrderStatus()
                    {
                        NotificationType = 0,
                        ServiceType = "collection",
                        Title = "Sorry, We could not collect parcel",
                        Description = notification
                    };

                    await _firebaseCloudService.SendNotificationDeliveryBondhu(courierObj.CourierUsers.FirebaseToken, orderStatus);
                }
            }
            var smsResponse = await _smsEmailService.SmsSend(smsList);

            return smsResponse;
        }

        public async Task<List<CourierOrdersViewModel>> GetCollectionSlotWiseOrders(RequestBodyModel request)
        {

            var courierOrders = from _courierOrders in _sqlServerContext.CourierOrders.AsNoTracking()
                                join _courierUsers in _sqlServerContext.CourierUsers.AsNoTracking() on _courierOrders.MerchantId equals _courierUsers.CourierUserId
                                join _deliveryUser in _sqlServerContext.DeliveryUsers.AsNoTracking() on _courierOrders.DeliveryUserId equals _deliveryUser.Id into orderUsers
                                from _deliveryUser in orderUsers.DefaultIfEmpty()
                                    //join _collectDistrict in _sqlServerContext.Districts.AsNoTracking() on _courierOrders.CollectAddressDistrictId equals _collectDistrict.DistrictId into collectDistrict
                                    //from _collectDistrict in collectDistrict.DefaultIfEmpty()
                                join _collectThana in _sqlServerContext.Districts.AsNoTracking() on _courierOrders.CollectAddressThanaId equals _collectThana.DistrictId into collectThana
                                from _collectThana in collectThana.DefaultIfEmpty()

                                where _courierOrders.CollectionTimeSlotId == request.CollectionTimeSlotId
                                && _courierOrders.Status == request.StatusId
                                && _courierOrders.CollectionTime.HasValue
                                && _courierOrders.CollectionTime.Value.Date >= request.FromDate.Date
                                && _courierOrders.CollectionTime.Value.Date < request.ToDate.Date.AddDays(1)
                                && _courierOrders.MerchantId != 1
                                && _courierOrders.OfficeDrop == false
                                orderby _courierOrders.MerchantId
                                select new CourierOrdersViewModel
                                {
                                    Id = _courierOrders.Id,
                                    MerchantId = _courierOrders.MerchantId,
                                    CollectAddressDistrictId = _courierOrders.CollectAddressDistrictId,
                                    CollectAddressThanaId = _courierOrders.CollectAddressThanaId,
                                    PaymentType = _courierOrders.PaymentType,
                                    CollectionTime = _courierOrders.CollectionTime,
                                    CourierUsers = new CourierUsersViewModel
                                    {
                                        CompanyName = _courierUsers.CompanyName
                                    },
                                    DeliveryUsersViewModel = new DeliveryUsersViewModel
                                    {
                                        Id = _deliveryUser.Id.ToString() == null ? 0 : _deliveryUser.Id,
                                        Name = _deliveryUser.Name == null ? string.Empty : _deliveryUser.Name
                                    },
                                    DistrictsViewModel = new DistrictsViewModel
                                    {
                                        Thana = _collectThana.District == null ? string.Empty : _collectThana.District,
                                        ThanaId = _collectThana.DistrictId.ToString() == null ? 0 : _collectThana.DistrictId
                                    }
                                };


            return await courierOrders.ToListAsync();
        }

        public async Task<IEnumerable<dynamic>> GetRiderWiseCollectionReport(RequestBodyModel request)
        {

            var orders = await (from _history in _sqlServerContext.CourierOrderStatusHistory.AsNoTracking()

                                join _courierOrder in _sqlServerContext.CourierOrders.AsNoTracking() on _history.CourierOrderId equals _courierOrder.CourierOrdersId
                                join _rider in _sqlServerContext.DeliveryUsers.AsNoTracking() on _history.PostedBy equals _rider.Id

                                where _history.Status == 44
                                && _history.IsConfirmedBy == "deliveryman"
                                && _history.MerchantId != 1
                                && _courierOrder.CollectAddressDistrictId == (request.DistrictId != 14 ?
                                (_courierOrder.CollectAddressDistrictId == 14 ? -1 : _courierOrder.CollectAddressDistrictId) : request.DistrictId)
                                && _history.PostedOn >= request.FromDate
                                && _history.PostedOn < request.ToDate.AddDays(1)
                                //group _courierOrders by _courierOrders.DeliveryUserId into grp
                                select new
                                {
                                    DeliveryUserId = _history.PostedBy,
                                    DeliveryUserName = _rider.Name,
                                    CourierOrders = _history.CourierOrderId,
                                    Merchant = _history.MerchantId
                                }).Distinct().ToListAsync();


            var report = orders.GroupBy(x => x.DeliveryUserId).Select(y => new
            {
                DeliveryUserName = y.FirstOrDefault().DeliveryUserName,
                OrderCount = y.Count(),
                MerchantCount = y.Select(x => x.Merchant).Distinct().Count()
            }).OrderByDescending(y => y.OrderCount);

            return report;
        }

        public async Task<IEnumerable<dynamic>> GetPackagedWiseOrders(RequestBodyModel request)
        {
            var orders = await (from _history in _sqlServerContext.CourierOrderStatusHistory.AsNoTracking()
                                join _courierOrders in _sqlServerContext.CourierOrders.AsNoTracking() on _history.CourierOrderId equals _courierOrders.CourierOrdersId
                                join _admin in _sqlServerContext.Users.AsNoTracking() on _history.PostedBy equals _admin.UserId

                                where _history.IsConfirmedBy == "admin"
                                && _history.Status == 8
                                && _history.PostedOn >= request.FromDate
                                && _history.PostedOn < request.ToDate.AddDays(1)

                                select new
                                {
                                    UserName = _admin.UserName,
                                    UserId = _admin.UserId,
                                    HubName = _history.HubName,
                                    CourierOrderId = _history.CourierOrderId,
                                    OrderDate = _courierOrders.OrderDate,
                                    CustomerName = _courierOrders.CustomerName,
                                    PaymentType = _courierOrders.PaymentType
                                }).Distinct().ToListAsync();


            var report = orders.GroupBy(x => new { x.UserId, x.HubName }).Select(y => new
            {
                UserName = y.FirstOrDefault().UserName,
                HubName = y.FirstOrDefault().HubName,
                CourierOrderCount = y.Select(z => z.CourierOrderId).Distinct().Count(),
                Orders = y.Select(z => new
                {
                    CourierOrdersId = z.CourierOrderId,
                    OrderDate = z.OrderDate,
                    CustomerName = z.CustomerName,
                    PaymentType = z.PaymentType
                }).Distinct().ToList()

            }).OrderByDescending(a => a.CourierOrderCount).ToList();


            return report;
        }


        public async Task<List<Vouchers>> AddVoucher(List<Vouchers> vouchers)
        {
            await _sqlServerContext.Vouchers.AddRangeAsync(vouchers);
            await _sqlServerContext.SaveChangesAsync();
            return vouchers;
        }

        public async Task<IEnumerable<Vouchers>> GetAllVouchers()
        {

            return await _sqlServerContext.Vouchers.ToListAsync();
            //return data.ToList();
        }

        public async Task<List<PhoneBookGroup>> GetMyPhoneBookGroup(int courierUserId)
        {
            return await _sqlServerContext.PhoneBookGroup.Where(g => g.CourierUserId.Equals(courierUserId)).ToListAsync();
        }

        public async Task<CourierUsers> UpdateTelesalesStatus(int userId, CourierUsersViewModel courierUsers)
        {
            var entity = await _sqlServerContext.CourierUsers.FirstOrDefaultAsync(item => item.CourierUserId == userId);
            if (entity != null)
            {
                entity.TeleSales = courierUsers.TeleSales; //courierUsers.IsTeleSales;
                entity.TeleSalesDate = DateTime.Now;

                _sqlServerContext.CourierUsers.Update(entity);

                await _sqlServerContext.SaveChangesAsync();

                if (courierUsers.TeleSaleCourierUsers.Count() == 0)
                {
                    var telesalesCourierEntity = await _sqlServerContext.TeleSaleCourierUsers.Where(d => d.CourierUserId.Equals(userId)).ToListAsync();

                    _sqlServerContext.TeleSaleCourierUsers.RemoveRange(telesalesCourierEntity);
                    await _sqlServerContext.SaveChangesAsync();
                }
                else if(courierUsers.TeleSaleCourierUsers.Count() != 0)
                {
                    var telesalesCourierEntity = await _sqlServerContext.TeleSaleCourierUsers.Where(d => d.CourierUserId.Equals(userId)).ToListAsync();

                    _sqlServerContext.TeleSaleCourierUsers.RemoveRange(telesalesCourierEntity);
                    await _sqlServerContext.SaveChangesAsync();

                    _sqlServerContext.TeleSaleCourierUsers.AddRange(courierUsers.TeleSaleCourierUsers);
                    await _sqlServerContext.SaveChangesAsync();
                }
            }

            return entity;

        }

        public async Task<AcquisitionLeadManagement> AddAcquisitionLead(AcquisitionLeadManagement acquisitionLead)
        {

            await _sqlServerContext.AcquisitionLeadManagement.AddAsync(acquisitionLead);
            await _sqlServerContext.SaveChangesAsync();
            return acquisitionLead;
        }

        public async Task<List<CourierUsersViewModel>> GetDistrictwiseCourierUserInfo(bool isInsideDhaka, string companyName)
        {
            if(isInsideDhaka)
            {
                var data = await (from _courierUsers in _sqlServerContext.CourierUsers.AsNoTracking()
                                  join _pickupLocation in _sqlServerContext.PickupLocations.AsNoTracking() on _courierUsers.CourierUserId equals _pickupLocation.CourierUserId

                                  where _courierUsers.IsActive == true
                                  && _pickupLocation.IsActive == true
                                  && _pickupLocation.DistrictId == 14
                                  && EF.Functions.Like(_courierUsers.CompanyName, "%" + companyName + "%")
                                  select new CourierUsersViewModel
                                  {
                                      CourierUserId = _courierUsers.CourierUserId,
                                      CompanyName = _courierUsers.CompanyName,
                                      Mobile = _courierUsers.Mobile,
                                  }).Distinct().ToListAsync();

                return data;
            }
            else
            {
                var data = await (from _courierUsers in _sqlServerContext.CourierUsers.AsNoTracking()
                                  join _pickupLocation in _sqlServerContext.PickupLocations.AsNoTracking() on _courierUsers.CourierUserId equals _pickupLocation.CourierUserId

                                  where _courierUsers.IsActive == true
                                  && _pickupLocation.IsActive == true
                                  && _pickupLocation.DistrictId != 14
                                  && EF.Functions.Like(_courierUsers.CompanyName, "%" + companyName + "%")
                                  select new CourierUsersViewModel
                                  {
                                      CourierUserId = _courierUsers.CourierUserId,
                                      CompanyName = _courierUsers.CompanyName,
                                      Mobile = _courierUsers.Mobile,
                                  }).Distinct().ToListAsync();

                return data;
            }
        }

        public async Task<int> UpdateUserProfile(int userId, Users users)
        {
            var entity = await _sqlServerContext.Users.FirstOrDefaultAsync(item => item.UserId == userId);

            if (entity != null)
            {
                entity.FullName = users.FullName;
                entity.Passwrd = users.Passwrd;
                entity.PersonalEmail = users.PersonalEmail;
                entity.Mobile = users.Mobile;
                entity.BloodGroup = users.BloodGroup;
                entity.Address = users.Address;
                entity.Gender = users.Gender;

                _sqlServerContext.Users.Update(entity);
                return await _sqlServerContext.SaveChangesAsync();
            }

            return 0;
        }

        public async Task<AdminUsersViewModel> GetUser(int userId)
        {
            var user = from _user in _sqlServerContext.Users
                       where _user.UserId == userId
                       select new AdminUsersViewModel
                       {
                           UserName = _user.UserName,
                           FullName = _user.FullName,
                           Passwrd = _user.Passwrd,
                           AdminType = _user.AdminType,
                           Email = _user.PersonalEmail,
                           Mobile = _user.Mobile,
                           BloodGroup = _user.BloodGroup,
                           Address = _user.Address,
                           Gender = _user.Gender
                       };
            return await user.FirstOrDefaultAsync();
        }

        public async Task<List<DeliveryRange>> GetDTDeliveryChargeInfo(RequestBodyModel request)
        {
            
            var data = await (from _deliveryChargeDetails in _sqlServerContext.DeliveryChargeDetails.AsNoTracking()
                       join _deliveryRange in _sqlServerContext.DeliveryRange.AsNoTracking() on _deliveryChargeDetails.DeliveryRangeId equals _deliveryRange.Id

                       where _deliveryChargeDetails.DistrictId == request.DistrictId
                       && _deliveryChargeDetails.ThanaId == request.ThanaId
                       && _deliveryChargeDetails.AreaId == request.AreaId
                       && _deliveryChargeDetails.WeightRangeId == 2
                       && _deliveryChargeDetails.IsActive == true
                       && _deliveryChargeDetails.ServiceType == (request.DistrictId == 14 ? "citytocity" : "alltoall")

                       select new DeliveryRange
                       {
                           Id = _deliveryRange.Id,
                           Name = _deliveryRange.Name,
                           Type = _deliveryRange.Type,
                           CourierDeliveryCharge = (_deliveryRange.Type == "regular" ? Convert.ToDecimal(0) : _deliveryChargeDetails.CourierDeliveryCharge),
                           OnImageLink = _deliveryRange.OnImageLink,
                           OffImageLink = _deliveryRange.OffImageLink
                       }).ToListAsync();

            
            return data;
        }

        public async Task<SMSPurchase> AddSmsPurchase(SMSPurchase request)
        {
            await _sqlServerContext.SMSPurchase.AddRangeAsync(request);
            var entry = await _sqlServerContext.SaveChangesAsync();

            var courierUsersInfoContext = await _sqlServerContext.CourierUsers.Where(x => x.CourierUserId.Equals(request.CourierUserId)).FirstOrDefaultAsync();

            if(courierUsersInfoContext != null && entry > 0)
            {
                courierUsersInfoContext.CustomerSMSLimit = courierUsersInfoContext.CustomerSMSLimit + request.BuySmsCount;

                 _sqlServerContext.CourierUsers.Update(courierUsersInfoContext);
                await _sqlServerContext.SaveChangesAsync();
            }

            return request;
        }

        public async Task<List<GetPurchasedSMSInfoViewModel>> GetPurchasedSMSInfo(int courierUserId)
        {
            using(var con = new SqlConnection(_connectionStrings.MsSqlConnection))
            {
                con.Open();

                var param = new DynamicParameters();
                param.Add(name: "@courierUserId", value: courierUserId, dbType: DbType.Int32);

                var data = await con.QueryAsync<GetPurchasedSMSInfoViewModel>(sql: "[DT].[USP_GetPurchasedSMSInfo]", commandType: CommandType.StoredProcedure,
                    param: param);

                return data.ToList();
            }

        }

        public async Task<Users> UpdateAdUserSalaryAmount(Users user)
        {
            var entity = await _sqlServerContext.Users.Where(u => u.UserId == user.UserId).FirstOrDefaultAsync();

            if (entity != null)
            {
                entity.SalaryAmount = user.SalaryAmount;
                _sqlServerContext.Users.Update(entity);
                await _sqlServerContext.SaveChangesAsync();
            }

            return entity;
        }

        public async Task<IEnumerable<dynamic>> GetDatewiseVoucherInfo(RequestBodyModel request)
        {

            var data = await (from _courierOrders in _sqlServerContext.CourierOrders.AsNoTracking()
                       join _courierUsers in _sqlServerContext.CourierUsers.AsNoTracking() on _courierOrders.MerchantId equals _courierUsers.CourierUserId
                       join _deliveryRange in _sqlServerContext.DeliveryRange.AsNoTracking() on _courierOrders.VoucherDeliveryRangeId equals _deliveryRange.Id

                       where _courierOrders.VoucherCode != String.Empty
                       && _courierOrders.VoucherDeliveryRangeId != 0
                       && _courierOrders.OrderDate.Date >= request.FromDate.Date
                       && _courierOrders.OrderDate < request.ToDate.AddDays(1).Date

                       select new
                       {
                           CompanyName = _courierUsers.CompanyName + " (" + _courierUsers.CourierUserId + ")",
                           CourierOrdersId = "DT-" + _courierOrders.Id,
                           VoucherCode = _courierOrders.VoucherCode,
                           ServiceType = _deliveryRange.Name + " " + _deliveryRange.Day + " " + _deliveryRange.DayType,
                           OrderDate = _courierOrders.OrderDate
                       }).OrderByDescending(x=> x.OrderDate ).ToListAsync();


            return data;
        }

        public async Task<int> AddLenderUser(LenderUser lenderUser)
        {
            _sqlServerContext.LenderUser.Add(lenderUser);
            return await _sqlServerContext.SaveChangesAsync();
        }

        public async Task<int> AddUserLocationAssign(UserLocationAssign userLocationAssign)
        {
            _sqlServerContext.UserLocationAssign.Add(userLocationAssign);
            return await _sqlServerContext.SaveChangesAsync();
        }

        public async Task<List<LenderCourierUserAssignment>> AssignLenderUserToCourierUser(List<LenderCourierUserAssignment> lenderCourierUserAssignments)
        {
            await _sqlServerContext.LenderCourierUserAssignment.AddRangeAsync(lenderCourierUserAssignments);
            await _sqlServerContext.SaveChangesAsync();
            return lenderCourierUserAssignments;
        }

        public async Task<List<LenderCourierUserAssignment>> UnAssignLenderUserToCourierUser(List<LenderCourierUserAssignment> lenderCourierUserAssignments)
        {
            var assignments = lenderCourierUserAssignments.Select(s => s.AssignmentId);
            var entity = _sqlServerContext.LenderCourierUserAssignment.Where(x => assignments.Contains(x.AssignmentId));
            _sqlServerContext.LenderCourierUserAssignment.RemoveRange(entity);
            await _sqlServerContext.SaveChangesAsync();
            return lenderCourierUserAssignments;
        }

        public async Task<List<LenderUser>> GetLenderUsers()
        {
            return await (from l in _sqlServerContext.LenderUser.AsNoTracking()
                          select l).ToListAsync();
        }

        public async Task<IEnumerable<dynamic>> GetLenderWiseAssignedCourierUsers(int lenderUserId)
        {
            var data = await (from loan in _sqlServerContext.LoanSurvey.Where(l => l.CourierUserId != 1)
                              join assignedUsers in _sqlServerContext.LenderCourierUserAssignment.Where(l => l.LenderUserId == lenderUserId)
                              on loan.CourierUserId equals assignedUsers.CourierUserId into join1

                              from j1 in join1.DefaultIfEmpty()
                              join courierUser in _sqlServerContext.CourierUsers
                              on loan.CourierUserId equals courierUser.CourierUserId
                              select new
                              {
                                  AssignmentId = j1 == null ? 0 : j1.AssignmentId,
                                  LoanCourierUserId = loan.CourierUserId,
                                  AssignedCourierUserId = j1 == null ? 0 : j1.CourierUserId,
                                  LoanCourierUserName = courierUser.CompanyName
                              }).GroupBy(g => g.LoanCourierUserId).Select(s => new
                              {
                                  AssignmentId = s.FirstOrDefault().AssignmentId,
                                  LoanCourierUserId = s.FirstOrDefault().LoanCourierUserId,
                                  AssignedCourierUserId = s.FirstOrDefault().AssignedCourierUserId,
                                  LoanCourierUserName = s.FirstOrDefault().LoanCourierUserName
                              }).ToListAsync();
            return data;
        }

        public async Task<int> UpdatePoHOrders(CourierOrders request, string type)
        {
            var courierOrders = await _sqlServerContext.CourierOrders.Where(x => x.Id == request.Id).FirstOrDefaultAsync();
            int updatedCount = 0;
            if(courierOrders != null)
            {
                if(type == "verify")
                {
                    courierOrders.PaymentServiceTypeVerify = "verify";
                }
                else if(type == "unverify")
                {
                    courierOrders.PaymentServiceTypeVerify = String.Empty;
                }
                else if(type == "merchantverify")
                {
                    courierOrders.PaymentServiceTypeMerchantVerify = "verify";
                }
                else if(type == "merchantunverify")
                {
                    courierOrders.PaymentServiceTypeMerchantVerify = String.Empty;
                }
                else if(type == "remove")
                {
                    courierOrders.PaymentServiceType = 0;
                    courierOrders.PaymentServiceCharge = 0;
                    courierOrders.PaymentServiceTypeVerify = String.Empty;
                    courierOrders.PaymentServiceTypeMerchantVerify = String.Empty;

                    //cod charge calculation
                    var extraCharge = await _sqlServerContext.ExtraCharge.FirstOrDefaultAsync();
                    var courierUsersInfo = await _sqlServerContext.CourierUsers.Where(x => x.CourierUserId.Equals(courierOrders.MerchantId)).FirstOrDefaultAsync();

                    if(courierOrders.DistrictId == 14) //Inside Dhaka
                    {
                        if(courierUsersInfo.CodChargeTypeFlag == 1) //Taka
                        {
                            if(courierUsersInfo.CodChargeDhaka < 0)
                            {
                                courierOrders.CodCharge = (extraCharge.CodChargeDhaka == null ? 0 : Convert.ToDecimal(extraCharge.CodChargeDhaka));
                            }
                            else
                            {
                                courierOrders.CodCharge = (courierUsersInfo.CodChargeDhaka == null? 0 : Convert.ToDecimal(courierUsersInfo.CodChargeDhaka));
                            }
                        }
                        else //Percentage
                        {
                            if (courierUsersInfo.CodChargePercentageDhaka < 0)
                            {
                                courierOrders.CodCharge = (extraCharge.CodChargeDhakaPercentage / Convert.ToDecimal(100)) * courierOrders.CollectionAmount;
                            }
                            else
                            {
                                courierOrders.CodCharge = (Convert.ToDecimal(courierUsersInfo.CodChargePercentageDhaka) / Convert.ToDecimal(100)) * courierOrders.CollectionAmount;
                            }
                        }
                    }
                    else //OutsideDhaka
                    {
                        //courierOrders.CodCharge = Convert.ToDecimal(extraCharge.CodChargePercentage / Convert.ToDecimal(100)) * courierOrders.CollectionAmount;
                        if (courierUsersInfo.CodChargeTypeOutsideFlag == 1) //Taka
                        {
                            if (courierUsersInfo.CodChargeOutsideDhaka < 0)
                            {
                                courierOrders.CodCharge = (extraCharge.CodChargeOutsideDhaka == null ? 0 : Convert.ToDecimal(extraCharge.CodChargeOutsideDhaka));
                            }
                            else
                            {
                                courierOrders.CodCharge = (courierUsersInfo.CodChargeOutsideDhaka == null ? 0 : Convert.ToDecimal(courierUsersInfo.CodChargeOutsideDhaka));
                            }
                        }
                        else //Percentage
                        {
                            if (courierUsersInfo.CodChargePercentageOutsideDhaka < 0)
                            {
                                courierOrders.CodCharge = (extraCharge.CodChargePercentage / Convert.ToDecimal(100)) * courierOrders.CollectionAmount;
                            }
                            else
                            {
                                courierOrders.CodCharge = (Convert.ToDecimal(courierUsersInfo.CodChargePercentageOutsideDhaka) / Convert.ToDecimal(100)) * courierOrders.CollectionAmount;
                            }
                        }
                    }
                    

                }

                _sqlServerContext.CourierOrders.Update(courierOrders);
                updatedCount = await _sqlServerContext.SaveChangesAsync();

            }

            return updatedCount;
        }

        public async Task<int> UpdateUserLocationAssign(int userLocationAssignId, UserLocationAssign userLocationAssign)
        {
            var entity = await _sqlServerContext.UserLocationAssign.FirstOrDefaultAsync(u => u.Id == userLocationAssignId);

            if (entity != null)
            {
                entity.DistrictId = userLocationAssign.DistrictId;
                entity.ThanaId = userLocationAssign.ThanaId;
                entity.AreaId = userLocationAssign.AreaId;
                entity.UserId = userLocationAssign.UserId;
                entity.UserType = userLocationAssign.UserType;

                _sqlServerContext.UserLocationAssign.Update(entity);
                return await _sqlServerContext.SaveChangesAsync();
            }

            return 0;
        }

        public async Task<dynamic> UpdateReferencewisePayment(PaymentReference request, string paymentFrom)
        {
            //var reference = await _sqlServerContext.PaymentReference.Where(x => x.PaymentReferenceId == request.PaymentReferenceId && x.CourierId == request.CourierId).FirstOrDefaultAsync();


            var reference = await (from _paymentReference in _sqlServerContext.PaymentReference
                        join _paymentReferenceDetals in _sqlServerContext.PaymentReferenceDetails on _paymentReference.PaymentReferenceId equals _paymentReferenceDetals.PaymentReferenceId
                        join paidPRD in _sqlServerContext.PaymentReferenceDetails on new {a= _paymentReference.PaymentReferenceId, b= "payment" } equals new {a = paidPRD.PaymentReferenceId, b= paidPRD.Type } into grpPRD
                                    from _paidPRD in grpPRD.DefaultIfEmpty()

                        where _paymentReference.PaymentReferenceId == request.PaymentReferenceId
                        && _paymentReference.CourierId == request.CourierId
                        && _paidPRD.Type != "payment"
                        select _paymentReference).Distinct().FirstOrDefaultAsync();

            if(reference != null)
            {
                reference.TransactionId = request.TransactionId;
                reference.Type = request.Type;
                reference.IsConfirmedBy = request.IsConfirmedBy;
                reference.PostedBy = request.PostedBy;
                reference.UpdatedOn = DateTime.Now;

                _sqlServerContext.PaymentReference.Update(reference);
                var updateCount = await _sqlServerContext.SaveChangesAsync();


                var refDetails = await _sqlServerContext.PaymentReferenceDetails.Where(x => x.PaymentReferenceId == reference.PaymentReferenceId)
                    .Select(y => new PaymentReferenceDetails
                    {
                        OrderId = y.OrderId,
                        Status = y.Status,
                        PodNumber = y.PodNumber,
                        CourierId = y.CourierId,
                        PaymentFrom = paymentFrom,
                        PostedBy = request.PostedBy,
                        AdminType = request.IsConfirmedBy,
                        Type = "payment",
                        PostedOn = DateTime.Now,
                        PaymentReferenceId = y.PaymentReferenceId,
                        CollectionAmount = y.CollectionAmount
                    }).ToListAsync();


                await _sqlServerContext.BulkInsertAsync(refDetails);


                var response = new
                {
                    isValid = true,
                    message = "ReferenceId is valid",
                    UpdateCount = updateCount,
                    PaymentReferenceDetails = refDetails
                };

                return response;
            }
            else
            {
                var response = new
                {
                    isValid = false,
                    message = "ReferenceId is not valid"
                };

                return response;
            }
            
        }

        public async Task<dynamic> GetPaymentReferenceReport(RequestBodyModel request)
        {
            int[] userIds = { request.UserId, 82 };
            var paymentReferenceData = (await (from _pr in _sqlServerContext.PaymentReference.AsNoTracking()
                                               join _prd in _sqlServerContext.PaymentReferenceDetails.AsNoTracking() on new { a = _pr.PaymentReferenceId, b = _pr.Type } equals new { a = _prd.PaymentReferenceId, b = _prd.Type }
                                               join _orders in _sqlServerContext.PaymentReferenceDetails.AsNoTracking() on new { a = _pr.PaymentReferenceId, b = "reference" } equals new { a = _orders.PaymentReferenceId, b = _orders.Type } into orders
                                               from _requestOrders in orders.DefaultIfEmpty()
                                               join _couriers in _sqlServerContext.Couriers.AsNoTracking() on _pr.CourierId equals _couriers.CourierId

                                               where _pr.CourierId == request.CourierId
                                               //&& _pr.PostedBy == request.UserId
                                               && userIds.Contains(_pr.PostedBy)
                                               && _requestOrders.PodNumber != null
                                               && _requestOrders.PostedOn.Date >= request.FromDate.Date
                                               && _requestOrders.PostedOn.Date < request.ToDate.Date.AddDays(1)
                                               select new
                                               {
                                                   PaymentReferenceId = _pr.PaymentReferenceId,
                                                   CourierName = _couriers.CourierName,
                                                   TotalCollectionAmount = _pr.TotalCollectionAmount,
                                                   TransactionId = _pr.TransactionId,
                                                   UpdatedOn = _pr.UpdatedOn,
                                                   PRType = _pr.Type,
                                                   OrderId = _prd.OrderId,
                                                   PodNumber = _prd.PodNumber,
                                                   PRDType = _prd.Type,
                                                   CollectionAmount = _prd.CollectionAmount,
                                                   PostedOn = _prd.PostedOn
                                               }).Distinct().ToListAsync())
                                               .GroupBy(x => x.PaymentReferenceId)
                                               .Select(y => new
                                               {
                                                   PaymentReferenceId = y.Key,
                                                   CourierName = y.FirstOrDefault().CourierName,
                                                   TotalCollectionAmount = y.FirstOrDefault().TotalCollectionAmount,
                                                   TransactionId = y.FirstOrDefault().TransactionId,
                                                   UpdatedOn = y.FirstOrDefault().UpdatedOn.ToString("dd/MM/yyyy hh: mm tt"),
                                                   PRType = y.FirstOrDefault().PRType,
                                                   OrderDetails = new
                                                   {
                                                       TotalOrders = y.Where(x => x.PRDType == y.FirstOrDefault().PRType).Count(),
                                                       OrderList = y.Where(x=> x.PRDType == y.FirstOrDefault().PRType).Select(j=> new 
                                                       {
                                                           OrderId = j.OrderId,
                                                           PodNumber = j.PodNumber,
                                                           CollectionAmount = j.CollectionAmount,
                                                           PostedOn = j.PostedOn.ToString("dd/MM/yyyy hh: mm tt"),
                                                           Type = j.PRDType
                                                       }).Distinct().ToList() 

                                                   }
                                               });



            return paymentReferenceData.ToList();
        }

        public async Task<List<UserLocationAssignViewModel>> GetUserLocationAssign()
        {
            var data = await (from userLocation in _sqlServerContext.UserLocationAssign
                        join district in _sqlServerContext.Districts.Where(d => d.IsActive.Equals(true))
                        on userLocation.DistrictId equals district.DistrictId
                        join thana in _sqlServerContext.Districts.Where(t => t.IsActive.Equals(true))
                        on userLocation.ThanaId equals thana.DistrictId
                        join area in _sqlServerContext.Districts.Where(a => a.IsActive.Equals(true))
                        on userLocation.AreaId equals area.DistrictId into areaJoin

                        from j1 in areaJoin.DefaultIfEmpty()
                        join user in _sqlServerContext.Users
                        on userLocation.UserId equals user.UserId
                        select new UserLocationAssignViewModel
                        {
                            Id = userLocation.Id,
                            DistrictId = userLocation.DistrictId,
                            DistrictName = district.District,
                            DistrictNameBng = district.DistrictBng,
                            ThanaId = userLocation.ThanaId,
                            ThanaName = thana.District,
                            ThanaNameBng = thana.DistrictBng,
                            AreaId = userLocation.AreaId,
                            AreaName = j1 == null ? "" : j1.District,
                            AreaNameBng = j1 == null ? "" : j1.DistrictBng,
                            UserId = userLocation.UserId,
                            FullName = user.FullName,
                            UserType = userLocation.UserType
                        }).ToListAsync();
            return data;
        }

        public async Task<PohViewModel> GetPohApplicable(string mobile, int courierUserId)
        {
            using (var connection = new SqlConnection(_connectionStrings.MsSqlConnection))
            {
                connection.Open();

                var parameter = new DynamicParameters();
                parameter.Add(name: "@Mobile", value: mobile, dbType: DbType.String);
                parameter.Add(name: "@CourierUserId", value: courierUserId, dbType: DbType.Int32);

                var data = await connection.QueryAsync<PohViewModel>(
                        sql: @"[DT].[PohApplicable]",
                        param: parameter,
                        commandType: CommandType.StoredProcedure);

                return data.FirstOrDefault();
            }
        }

        public async Task<dynamic> GetPohOrderStatuswise(RequestBodyModel request)
        {
            var report = await (from _courierOrders in _sqlServerContext.CourierOrders.AsNoTracking()
                                join _h in _sqlServerContext.CourierOrderStatusHistory.AsNoTracking() on
                                new { orders = _courierOrders.CourierOrdersId, status = 10 } equals new { orders = _h.CourierOrderId, status = _h.Status } into subTable
                                from _history in subTable.DefaultIfEmpty()

                                join _courierUsers in _sqlServerContext.CourierUsers on _courierOrders.MerchantId equals _courierUsers.CourierUserId
                                join _status in _sqlServerContext.CourierOrderStatus on _courierOrders.Status equals _status.StatusId

                                where _courierOrders.PaymentServiceType == 1
                                && _courierOrders.PaymentServiceCharge > 0
                                && String.IsNullOrEmpty(_history.Status.ToString())
                                && _courierOrders.OrderDate.Date >= request.FromDate.Date
                                && _courierOrders.OrderDate.Date < request.ToDate.AddDays(1).Date

                                select new
                                {
                                    CourierOrdersId = "DT-" + _courierOrders.Id,
                                    CurrentStatus = _status.StatusNameEng,
                                    CompanyName = _courierUsers.CompanyName,
                                    CollectionAmount = _courierOrders.CollectionAmount,
                                    CustomerName = _courierOrders.CustomerName,
                                    Address = _courierOrders.Address
                                }).ToListAsync();

            return report;
        }

        public async Task<dynamic> GetPohOrderwiseReport(RequestBodyModel request)
        {
            var orderInfo = await (from _courierOrders in _sqlServerContext.CourierOrders.AsNoTracking()
                             join _merchant in _sqlServerContext.CourierUsers.AsNoTracking() on _courierOrders.MerchantId equals _merchant.CourierUserId
                             join _status in _sqlServerContext.CourierOrderStatus.AsNoTracking() on _courierOrders.Status equals _status.StatusId

                             where _courierOrders.PaymentServiceType == 1
                             && _courierOrders.PaymentServiceCharge > 0
                             && _courierOrders.MerchantId == (request.MerchantId > 0 ? request.MerchantId : _courierOrders.MerchantId)
                             & _courierOrders.OrderDate.Date >= request.FromDate.Date
                             && _courierOrders.OrderDate.Date < request.ToDate.AddDays(1).Date

                             select new
                             {
                                 OrderId = "DT-" + _courierOrders.Id,
                                 MerchantName = _merchant.CompanyName,
                                 CollectionAmount = _courierOrders.CollectionAmount,
                                 DeliveryCharge = _courierOrders.DeliveryCharge,
                                 CodCharge = _courierOrders.CodCharge,
                                 CollectionCharge = _courierOrders.CollectionCharge,
                                 BreakableCharge = _courierOrders.BreakableCharge,
                                 ReturnCharge = _courierOrders.ReturnCharge,
                                 PackagingCharge = _courierOrders.PackagingCharge,
                                 PohCharge = _courierOrders.PaymentServiceCharge,
                                 TotalCharge = (_courierOrders.DeliveryCharge + _courierOrders.CodCharge + _courierOrders.CollectionCharge + _courierOrders.BreakableCharge + _courierOrders.ReturnCharge + _courierOrders.PackagingCharge + _courierOrders.PaymentServiceCharge),
                                 MerchantPayable = (_courierOrders.CollectionAmount - (_courierOrders.DeliveryCharge + _courierOrders.CodCharge + _courierOrders.CollectionCharge + _courierOrders.BreakableCharge + _courierOrders.ReturnCharge + _courierOrders.PackagingCharge + _courierOrders.PaymentServiceCharge)),
                                 CurrentStatus = _status.StatusNameEng
                             }).Distinct().ToListAsync();

            return orderInfo;
        }
    }
}
