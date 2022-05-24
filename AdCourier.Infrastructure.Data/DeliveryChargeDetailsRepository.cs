using System;
using System.Threading.Tasks;
using AdCourier.Context;
using AdCourier.Domain.Entities.DataModel;
using AdCourier.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using AdCourier.Domain.Entities.ViewModel.GetDeliveryChargeDetails;
using System.Collections.Generic;
using AdCourier.Domain.Entities.BodyModel.DeliveryChargeDetails;
using AdCourier.Domain.Entities.ViewModel.DatabaseViewModel;

namespace AdCourier.Infrastructure.Data
{
    public class DeliveryChargeDetailsRepository : IDeliveryChargeDetailsRepository
    {
        private readonly SqlServerContext _sqlServerContext;
        public DeliveryChargeDetailsRepository(SqlServerContext sqlServerContext)
        {
            _sqlServerContext = sqlServerContext ?? throw new ArgumentNullException(nameof(sqlServerContext));
        }

        public async Task<DeliveryChargeDetails> AddDeliveryChargeDetails(DeliveryChargeDetails deliveryChargeDetails)
        {
            await _sqlServerContext.DeliveryChargeDetails.AddAsync(deliveryChargeDetails);
            await _sqlServerContext.SaveChangesAsync();
            return deliveryChargeDetails;
        }


        public async Task<IEnumerable<GetDeliveryChargeDetailsViewModel>> DeliveryChargeDetailsAreaWise_test(DeliveryChargeDetailsBodyModel deliveryChargeDetailsBodyModel)
        {
            var response = (from details in _sqlServerContext.DeliveryChargeDetails_test.AsNoTracking()
                            join wrange in _sqlServerContext.WeightRange.AsNoTracking() on details.WeightRangeId equals wrange.Id
                            join deliveryRange in _sqlServerContext.DeliveryRange.AsNoTracking() on details.DeliveryRangeId equals deliveryRange.Id
                            where details.DistrictId == deliveryChargeDetailsBodyModel.DistrictId
                            && details.ThanaId == deliveryChargeDetailsBodyModel.ThanaId
                            && details.AreaId == deliveryChargeDetailsBodyModel.AreaId
                            let districtName = _sqlServerContext.Districts.Where(x => x.DistrictId == details.DistrictId && x.AreaType == 2).Select(y => new Districts { District = y.District, DistrictBng = y.DistrictBng }).FirstOrDefault() ?? new Districts()
                            let thanaName = _sqlServerContext.Districts.Where(x => x.DistrictId == details.ThanaId && x.AreaType == 3).Select(y => new Districts { District = y.District, DistrictBng = y.DistrictBng }).FirstOrDefault() ?? new Districts()
                            let areaName = _sqlServerContext.Districts.Where(x => x.DistrictId == details.AreaId && x.AreaType == 4).Select(y => new Districts { District = y.District, DistrictBng = y.DistrictBng }).FirstOrDefault() ?? new Districts()
                            select new GetDeliveryChargeDetailsViewModel
                            {

                                Id = details.Id,
                                IsOpenBox = details.IsOpenBox,
                                DistrictId = details.DistrictId,
                                ThanaId = details.ThanaId,
                                AreaId = details.AreaId,
                                AreaNameBng = areaName.DistrictBng,
                                AreaNameEng = areaName.District,
                                CourierDeliveryCharge = details.CourierDeliveryCharge,
                                DistrictBng = districtName.DistrictBng,
                                DistrictEng = districtName.District,
                                ThanaNameBng = thanaName.DistrictBng,
                                ThanaNameEng = thanaName.District,
                                WeightRangeId = wrange.Id,
                                Type = wrange.Type,
                                Weight = wrange.Weight,
                                DeliveryRangeId = deliveryRange.Id,
                                Name = deliveryRange.Name,
                                Day = deliveryRange.Day,
                                Ranking = deliveryRange.Ranking,
                                DayType = deliveryRange.DayType,
                                OnImageLink = deliveryRange.OnImageLink,
                                OffImageLink = deliveryRange.OffImageLink,
                                ShowHide = deliveryRange.ShowHide,
                                DeliveryAlertMessage = deliveryRange.DeliveryAlertMessage,
                                LoginHours = deliveryRange.LoginHours,
                                DateAdvance = deliveryRange.DateAdvance//,
                                //CityDeliveryCharge = details.CityDeliveryCharge
                            });
            return await response.ToListAsync();
        }

        public async Task<List<GetDeliveryChargeDetailsViewModel>> DeliveryChargeDetailsAreaWise(DeliveryChargeDetailsBodyModel deliveryChargeDetailsBodyModel)
        {
            var response = (from details in _sqlServerContext.DeliveryChargeDetails.AsNoTracking()
                            join wrange in _sqlServerContext.WeightRange.AsNoTracking() on details.WeightRangeId equals wrange.Id
                            join deliveryRange in _sqlServerContext.DeliveryRange.AsNoTracking() on details.DeliveryRangeId equals deliveryRange.Id
                            where details.DistrictId == deliveryChargeDetailsBodyModel.DistrictId
                            && details.ThanaId == deliveryChargeDetailsBodyModel.ThanaId
                            && details.AreaId == deliveryChargeDetailsBodyModel.AreaId
                            && details.ServiceType.Equals(deliveryChargeDetailsBodyModel.ServiceType)
                            && deliveryRange.IsActive.Equals(true)
                            && details.IsActive.Equals(true)
                            //let districtName = _sqlServerContext.Districts.Where(x => x.DistrictId == details.DistrictId && x.AreaType == 2).Select(y => new Districts { District = y.District, DistrictBng = y.DistrictBng }).FirstOrDefault() ?? new Districts()
                            //let thanaName = _sqlServerContext.Districts.Where(x => x.DistrictId == details.ThanaId && x.AreaType == 3).Select(y => new Districts { District = y.District, DistrictBng = y.DistrictBng }).FirstOrDefault() ?? new Districts()
                            //let areaName = _sqlServerContext.Districts.Where(x => x.DistrictId == details.AreaId && x.AreaType == 4).Select(y => new Districts { District = y.District, DistrictBng = y.DistrictBng }).FirstOrDefault() ?? new Districts()
                            select new GetDeliveryChargeDetailsViewModel
                            {

                                Id = details.Id,
                                IsOpenBox = details.IsOpenBox,
                                DistrictId = details.DistrictId,
                                ThanaId = details.ThanaId,
                                AreaId = details.AreaId,
                                //AreaNameBng = areaName.DistrictBng,
                                //AreaNameEng = areaName.District,
                                CourierDeliveryCharge = details.CourierDeliveryCharge,
                                //DistrictBng = districtName.DistrictBng,
                                //DistrictEng = districtName.District,
                                //ThanaNameBng = thanaName.DistrictBng,
                                //ThanaNameEng = thanaName.District,
                                WeightRangeId = wrange.Id,
                                Type = deliveryRange.Type,
                                Weight = wrange.Weight,
                                DeliveryRangeId = deliveryRange.Id,
                                Name = deliveryRange.Name,
                                Day = deliveryRange.Day,
                                Ranking = deliveryRange.Ranking,
                                DayType = deliveryRange.DayType,
                                OnImageLink = deliveryRange.OnImageLink,
                                OffImageLink = deliveryRange.OffImageLink,
                                ShowHide = deliveryRange.ShowHide,
                                DeliveryAlertMessage = deliveryRange.DeliveryAlertMessage,
                                LoginHours = deliveryRange.LoginHours,
                                DateAdvance = deliveryRange.DateAdvance,
                                //CityDeliveryCharge = details.CityDeliveryCharge,
                                DeliveryCharge = deliveryRange.CourierDeliveryCharge,
                                ExtraDeliveryCharge = details.CourierDeliveryCharge - deliveryRange.CourierDeliveryCharge,
                                ExtraCollectionCharge = wrange.ExtraCollectionCharge
                            });
            return await response.ToListAsync();
        }

        public async Task<List<GetDeliveryChargeDetailsViewModel>> DeliveryChargeMerchantDetailsAreaWise(DeliveryChargeDetailsBodyModel deliveryChargeDetailsBodyModel)
        {
            var response = (from details in _sqlServerContext.DeliveryChargeMerchantDetails.AsNoTracking()
                            join wrange in _sqlServerContext.WeightRange.AsNoTracking() on details.WeightRangeId equals wrange.Id
                            join deliveryRange in _sqlServerContext.DeliveryRange.AsNoTracking() on details.DeliveryRangeId equals deliveryRange.Id
                            where details.DistrictId == deliveryChargeDetailsBodyModel.DistrictId
                            && details.ThanaId == deliveryChargeDetailsBodyModel.ThanaId
                            && details.AreaId == deliveryChargeDetailsBodyModel.AreaId
                            && details.ServiceType.Equals(deliveryChargeDetailsBodyModel.ServiceType)
                            && deliveryRange.IsActive.Equals(true)
                            && details.IsActive.Equals(true)

                            select new GetDeliveryChargeDetailsViewModel
                            {

                                Id = details.Id,
                                IsOpenBox = details.IsOpenBox,
                                DistrictId = details.DistrictId,
                                ThanaId = details.ThanaId,
                                AreaId = details.AreaId,
                                CourierDeliveryCharge = details.CourierDeliveryCharge,
                                WeightRangeId = wrange.Id,
                                Type = deliveryRange.Type,
                                Weight = wrange.Weight,
                                DeliveryRangeId = deliveryRange.Id,
                                Name = deliveryRange.Name,
                                Day = deliveryRange.Day,
                                Ranking = deliveryRange.Ranking,
                                DayType = deliveryRange.DayType,
                                OnImageLink = deliveryRange.OnImageLink,
                                OffImageLink = deliveryRange.OffImageLink,
                                ShowHide = deliveryRange.ShowHide,
                                DeliveryAlertMessage = deliveryRange.DeliveryAlertMessage,
                                LoginHours = deliveryRange.LoginHours,
                                DateAdvance = deliveryRange.DateAdvance,
                                DeliveryCharge = deliveryRange.CourierDeliveryCharge,
                                ExtraDeliveryCharge = details.CourierDeliveryCharge - deliveryRange.CourierDeliveryCharge,
                                ExtraCollectionCharge = wrange.ExtraCollectionCharge
                            });
            return await response.ToListAsync();
        }

        public async Task<IEnumerable<GetDeliveryChargeDetailsViewModel>> DeliveryChargeDetailsSearchWise(DeliveryChargeDetailsSearchModel deliveryChargeDetailsSearch)
        {

            IQueryable<GetDeliveryChargeDetailsViewModel> response;

            if (deliveryChargeDetailsSearch.WeightId > 0)
            {
                response = (from details in _sqlServerContext.DeliveryChargeDetails.AsNoTracking()
                            join wrange in _sqlServerContext.WeightRange.AsNoTracking() on details.WeightRangeId equals wrange.Id
                            join deliveryRange in _sqlServerContext.DeliveryRange.AsNoTracking() on details.DeliveryRangeId equals deliveryRange.Id
                            where details.DistrictId == deliveryChargeDetailsSearch.DistrictId && details.ThanaId == deliveryChargeDetailsSearch.ThanaId && details.WeightRangeId == deliveryChargeDetailsSearch.WeightId
                            let districtName = _sqlServerContext.Districts.Where(x => x.DistrictId == details.DistrictId && x.AreaType == 2).Select(y => new Districts { District = y.District, DistrictBng = y.DistrictBng }).FirstOrDefault() ?? new Districts()
                            let thanaName = _sqlServerContext.Districts.Where(x => x.DistrictId == details.ThanaId && x.AreaType == 3).Select(y => new Districts { District = y.District, DistrictBng = y.DistrictBng }).FirstOrDefault() ?? new Districts()
                            let areaName = _sqlServerContext.Districts.Where(x => x.DistrictId == details.AreaId && x.AreaType == 4).Select(y => new Districts { District = y.District, DistrictBng = y.DistrictBng }).FirstOrDefault() ?? new Districts()
                            select new GetDeliveryChargeDetailsViewModel
                            {
                                Id = details.Id,
                                DistrictId = details.DistrictId,
                                ThanaId = details.ThanaId,
                                AreaId = details.AreaId,
                                AreaNameBng = areaName.DistrictBng,
                                AreaNameEng = areaName.District,
                                CourierDeliveryCharge = details.CourierDeliveryCharge,
                                DistrictBng = districtName.DistrictBng,
                                DistrictEng = districtName.District,
                                ThanaNameBng = thanaName.DistrictBng,
                                ThanaNameEng = thanaName.District,
                                WeightRangeId = wrange.Id,
                                Type = wrange.Type,
                                Weight = wrange.Weight,
                                DeliveryRangeId = deliveryRange.Id,
                                Name = deliveryRange.Name,
                                Day = deliveryRange.Day
                            });
            }
            else if (deliveryChargeDetailsSearch.DeliveryTypeId > 0)
            {
                response = (from details in _sqlServerContext.DeliveryChargeDetails.AsNoTracking()
                            join wrange in _sqlServerContext.WeightRange.AsNoTracking() on details.WeightRangeId equals wrange.Id
                            join deliveryRange in _sqlServerContext.DeliveryRange.AsNoTracking() on details.DeliveryRangeId equals deliveryRange.Id
                            where details.DistrictId == deliveryChargeDetailsSearch.DistrictId && details.ThanaId == deliveryChargeDetailsSearch.ThanaId && details.WeightRangeId == deliveryChargeDetailsSearch.WeightId && details.DeliveryRangeId == deliveryChargeDetailsSearch.DeliveryTypeId
                            let districtName = _sqlServerContext.Districts.Where(x => x.DistrictId == details.DistrictId && x.AreaType == 2).Select(y => new Districts { District = y.District, DistrictBng = y.DistrictBng }).FirstOrDefault() ?? new Districts()
                            let thanaName = _sqlServerContext.Districts.Where(x => x.DistrictId == details.ThanaId && x.AreaType == 3).Select(y => new Districts { District = y.District, DistrictBng = y.DistrictBng }).FirstOrDefault() ?? new Districts()
                            let areaName = _sqlServerContext.Districts.Where(x => x.DistrictId == details.AreaId && x.AreaType == 4).Select(y => new Districts { District = y.District, DistrictBng = y.DistrictBng }).FirstOrDefault() ?? new Districts()
                            select new GetDeliveryChargeDetailsViewModel
                            {
                                Id = details.Id,
                                DistrictId = details.DistrictId,
                                ThanaId = details.ThanaId,
                                AreaId = details.AreaId,
                                AreaNameBng = areaName.DistrictBng,
                                AreaNameEng = areaName.District,
                                CourierDeliveryCharge = details.CourierDeliveryCharge,
                                DistrictBng = districtName.DistrictBng,
                                DistrictEng = districtName.District,
                                ThanaNameBng = thanaName.DistrictBng,
                                ThanaNameEng = thanaName.District,
                                WeightRangeId = wrange.Id,
                                Type = wrange.Type,
                                Weight = wrange.Weight,
                                DeliveryRangeId = deliveryRange.Id,
                                Name = deliveryRange.Name,
                                Day = deliveryRange.Day
                            });
            }
            else
            {
                response = (from details in _sqlServerContext.DeliveryChargeDetails.AsNoTracking()
                            join wrange in _sqlServerContext.WeightRange.AsNoTracking() on details.WeightRangeId equals wrange.Id
                            join deliveryRange in _sqlServerContext.DeliveryRange.AsNoTracking() on details.DeliveryRangeId equals deliveryRange.Id
                            where details.DistrictId == deliveryChargeDetailsSearch.DistrictId && details.ThanaId == deliveryChargeDetailsSearch.ThanaId
                            let districtName = _sqlServerContext.Districts.Where(x => x.DistrictId == details.DistrictId && x.AreaType == 2).Select(y => new Districts { District = y.District, DistrictBng = y.DistrictBng }).FirstOrDefault() ?? new Districts()
                            let thanaName = _sqlServerContext.Districts.Where(x => x.DistrictId == details.ThanaId && x.AreaType == 3).Select(y => new Districts { District = y.District, DistrictBng = y.DistrictBng }).FirstOrDefault() ?? new Districts()
                            let areaName = _sqlServerContext.Districts.Where(x => x.DistrictId == details.AreaId && x.AreaType == 4).Select(y => new Districts { District = y.District, DistrictBng = y.DistrictBng }).FirstOrDefault() ?? new Districts()
                            select new GetDeliveryChargeDetailsViewModel
                            {
                                Id = details.Id,
                                DistrictId = details.DistrictId,
                                ThanaId = details.ThanaId,
                                AreaId = details.AreaId,
                                AreaNameBng = areaName.DistrictBng,
                                AreaNameEng = areaName.District,
                                CourierDeliveryCharge = details.CourierDeliveryCharge,
                                DistrictBng = districtName.DistrictBng,
                                DistrictEng = districtName.District,
                                ThanaNameBng = thanaName.DistrictBng,
                                ThanaNameEng = thanaName.District,
                                WeightRangeId = wrange.Id,
                                Type = wrange.Type,
                                Weight = wrange.Weight,
                                DeliveryRangeId = deliveryRange.Id,
                                Name = deliveryRange.Name,
                                Day = deliveryRange.Day
                            });

            }

            var responseData=await Task.FromResult(await response.ToListAsync());
            return responseData;

        }
        public async Task<IEnumerable<GetDeliveryChargeDetailsViewModel>> GetDeliveryChargeDetails()
        {

            var response = await (from details in _sqlServerContext.DeliveryChargeDetails.AsNoTracking()
                                  join wrange in _sqlServerContext.WeightRange.AsNoTracking() on details.WeightRangeId equals wrange.Id
                                  join deliveryRange in _sqlServerContext.DeliveryRange.AsNoTracking() on details.DeliveryRangeId equals deliveryRange.Id
                                  let districtName = _sqlServerContext.Districts.Where(x => x.DistrictId == details.DistrictId && x.AreaType == 2).Select(y => new Districts { District = y.District, DistrictBng = y.DistrictBng }).FirstOrDefault() ?? new Districts()
                                  let thanaName = _sqlServerContext.Districts.Where(x => x.DistrictId == details.ThanaId && x.AreaType == 3).Select(y => new Districts { District = y.District, DistrictBng = y.DistrictBng }).FirstOrDefault() ?? new Districts()
                                  let areaName = _sqlServerContext.Districts.Where(x => x.DistrictId == details.AreaId && x.AreaType == 4).Select(y => new Districts { District = y.District, DistrictBng = y.DistrictBng }).FirstOrDefault() ?? new Districts()
                                  select new GetDeliveryChargeDetailsViewModel
                                  {

                                      Id = details.Id,
                                      DistrictId = details.DistrictId,
                                      ThanaId = details.ThanaId,
                                      AreaId = details.AreaId,
                                      AreaNameBng = areaName.DistrictBng,
                                      AreaNameEng = areaName.District,
                                      CourierDeliveryCharge = details.CourierDeliveryCharge,
                                      DistrictBng = districtName.DistrictBng,
                                      DistrictEng = districtName.District,
                                      ThanaNameBng = thanaName.DistrictBng,
                                      ThanaNameEng = thanaName.District,
                                      WeightRangeId = wrange.Id,
                                      Type = wrange.Type,
                                      Weight = wrange.Weight,
                                      DeliveryRangeId = deliveryRange.Id,
                                      Name = deliveryRange.Name,
                                      Day = deliveryRange.Day
                                  }).ToListAsync();


            return response;

        }

        public async Task<IEnumerable<SACodChargesViewModel>> GetSACodChargeList()
        {
            var entity = await _sqlServerContext.SACodCharges.Select(s => new SACodChargesViewModel
            {
                CodCharge = s.CodCharge,
                MaxAmount = s.MaxAmount,
                MinAmount = s.MinAmount,
                IntervalAmount = s.IntervalAmount
            }).ToListAsync();

            return entity;
        }

        public async Task<DeliveryChargeDetails> UpdateDeliveryChargeDetails(int id, DeliveryChargeDetails deliveryChargeDetails)
        {
            var entity = await _sqlServerContext.DeliveryChargeDetails.FirstOrDefaultAsync(item => item.Id == id);
            if (entity != null)
            {
                entity.DistrictId = deliveryChargeDetails.DistrictId;
                entity.ThanaId = deliveryChargeDetails.ThanaId;
                entity.AreaId = deliveryChargeDetails.AreaId;

                entity.WeightRangeId = deliveryChargeDetails.WeightRangeId;
                entity.DeliveryRangeId = deliveryChargeDetails.DeliveryRangeId;
                entity.CourierDeliveryCharge = deliveryChargeDetails.CourierDeliveryCharge;

                // Update entity in DbSet
                _sqlServerContext.DeliveryChargeDetails.Update(entity);

                // Save changes in database
                await _sqlServerContext.SaveChangesAsync();
            }

            return entity;
        }

        public async Task<DeliveryChargeMerchantDetails> UpdateDeliveryChargeMerchantDetails(int id, DeliveryChargeMerchantDetails deliveryChargeMerchantDetails)
        {
            var entity = await _sqlServerContext.DeliveryChargeMerchantDetails.FirstOrDefaultAsync(item => item.Id == id);
            if (entity != null)
            {
                entity.DistrictId = deliveryChargeMerchantDetails.DistrictId;
                entity.ThanaId = deliveryChargeMerchantDetails.ThanaId;
                entity.AreaId = deliveryChargeMerchantDetails.AreaId;

                entity.WeightRangeId = deliveryChargeMerchantDetails.WeightRangeId;
                entity.DeliveryRangeId = deliveryChargeMerchantDetails.DeliveryRangeId;
                entity.CourierDeliveryCharge = deliveryChargeMerchantDetails.CourierDeliveryCharge;

                // Update entity in DbSet
                _sqlServerContext.DeliveryChargeMerchantDetails.Update(entity);

                // Save changes in database
                await _sqlServerContext.SaveChangesAsync();
            }

            return entity;
        }
    }
}
