using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdCourier.Domain.Entities.BodyModel.DeliveryChargeDetails;
using AdCourier.Domain.Entities.DataModel;
using AdCourier.Domain.Entities.ViewModel.DatabaseViewModel;
using AdCourier.Domain.Entities.ViewModel.GetDeliveryChargeDetails;
using AdCourier.Domain.Interfaces;
using AdCourier.Services.Interfaces;
using StackExchange.Redis.Extensions.Core.Abstractions;

namespace AdCourier.Services
{
    public class DeliveryChargeDetailsService : IDeliveryChargeDetailsService
    {
        private readonly IDeliveryChargeDetailsRepository _deliveryChargeDetailsRepository;
        private readonly IRedisCacheClient _redis;
        public DeliveryChargeDetailsService(IDeliveryChargeDetailsRepository deliveryChargeDetailsRepository, IRedisCacheClient redis)
        {
            _deliveryChargeDetailsRepository = deliveryChargeDetailsRepository;
            _redis = redis;
        }

        public async Task<DeliveryChargeDetails> AddDeliveryChargeDetails(DeliveryChargeDetails deliveryChargeDetails)
        {
            return await _deliveryChargeDetailsRepository.AddDeliveryChargeDetails(deliveryChargeDetails);
        }

        public async Task<IEnumerable<AreaWiseChargeDetailsViewModel>> DeliveryChargeDetailsAreaWise_test(DeliveryChargeDetailsBodyModel deliveryChargeDetailsBodyModel)
        {
            var data = await _deliveryChargeDetailsRepository.DeliveryChargeDetailsAreaWise_test(deliveryChargeDetailsBodyModel);
            var response = data.GroupBy(q => q.WeightRangeId).Select(w => new AreaWiseChargeDetailsViewModel
            {
                Weight = data.Where(g => g.WeightRangeId == w.Key).Select(x => x.Weight).FirstOrDefault(),
                IsOpenBox = data.Where(g => g.WeightRangeId == w.Key).Select(x => x.IsOpenBox).FirstOrDefault(),
                WeightRangeId = w.Key,
                WeightRangeWiseData = data.Where(v => v.WeightRangeId == w.Key).Select(j => new WeightRangeWiseData
                {
                    DeliveryRangeId = j.DeliveryRangeId,
                    WeightRangeId = j.WeightRangeId,
                    ChargeAmount = j.CourierDeliveryCharge,
                    //CityDeliveryCharge = j.CityDeliveryCharge,
                    Days = j.Day,
                    DeliveryType = j.Name,
                    Ranking = j.Ranking,
                    DayType = j.DayType,
                    OnImageLink = j.OnImageLink,
                    OffImageLink = j.OffImageLink,
                    ShowHide = j.ShowHide,
                    DeliveryAlertMessage = j.DeliveryAlertMessage,
                    LoginHours = j.LoginHours,
                    DateAdvance = j.DateAdvance

                }).OrderBy(o => o.Ranking).ToList(),
                DeliveryTypeModel = data.Where(r => r.WeightRangeId == w.Key).Select(y => new DeliveryTypeModel
                {

                    DeliveryType = y.Name,
                    DeliveryDayChargeModel = data.Where(u => u.WeightRangeId == y.WeightRangeId && u.DeliveryRangeId == y.DeliveryRangeId).Select(f => new DeliveryDayChargeModel
                    {

                        WeightRangeId = f.WeightRangeId,
                        DeliveryType = f.Name,
                        ChargeAmount = f.CourierDeliveryCharge,
                        Days = f.Day,
                        DayType = f.DayType

                    }).ToList()
                }).ToList()
            });

            return response.OrderBy(r => r.WeightRangeId);
        }
        public async Task<IEnumerable<AreaWiseChargeDetailsViewModel>> DeliveryChargeDetailsAreaWise(DeliveryChargeDetailsBodyModel deliveryChargeDetailsBodyModel)
        {
            //string key = deliveryChargeDetailsBodyModel.DistrictId.ToString() + deliveryChargeDetailsBodyModel.ThanaId.ToString();

            //if (await _redis.Db1.Database.KeyExistsAsync(key) == true)
            //{
            //    var data = await _redis.Db1.GetAsync<IEnumerable<GetDeliveryChargeDetailsViewModel>>(key);

            //    var response = data.GroupBy(q => q.WeightRangeId).Select(w => new AreaWiseChargeDetailsViewModel
            //    {
            //        Weight = data.Where(g => g.WeightRangeId == w.Key).Select(x => x.Weight).FirstOrDefault(),
            //        IsOpenBox = data.Where(g => g.WeightRangeId == w.Key).Select(x => x.IsOpenBox).FirstOrDefault(),
            //        WeightRangeId = w.Key,
            //        WeightRangeWiseData = data.Where(v => v.WeightRangeId == w.Key).Select(j => new WeightRangeWiseData
            //        {
            //            DeliveryRangeId = j.DeliveryRangeId,
            //            WeightRangeId = j.WeightRangeId,
            //            ChargeAmount = j.CourierDeliveryCharge,
            //            Days = j.Day,
            //            DeliveryType = j.Name,
            //            Ranking = j.Ranking,
            //            DayType = j.DayType,
            //            OnImageLink = j.OnImageLink,
            //            OffImageLink = j.OffImageLink,
            //            ShowHide = j.ShowHide,
            //            DeliveryAlertMessage = j.DeliveryAlertMessage,
            //            LoginHours = j.LoginHours,
            //            DateAdvance = j.DateAdvance

            //        }).OrderBy(o => o.Ranking).ToList(),
            //        DeliveryTypeModel = data.Where(r => r.WeightRangeId == w.Key).Select(y => new DeliveryTypeModel
            //        {

            //            DeliveryType = y.Name,
            //            DeliveryDayChargeModel = data.Where(u => u.WeightRangeId == y.WeightRangeId && u.DeliveryRangeId == y.DeliveryRangeId).Select(f => new DeliveryDayChargeModel
            //            {

            //                WeightRangeId = f.WeightRangeId,
            //                DeliveryType = f.Name,
            //                ChargeAmount = f.CourierDeliveryCharge,
            //                Days = f.Day

            //            }).ToList()
            //        }).ToList()
            //    });

            //    return response.OrderBy(r => r.WeightRangeId);
            //}
            //else
            //{

            var data = await _deliveryChargeDetailsRepository.DeliveryChargeDetailsAreaWise(deliveryChargeDetailsBodyModel);

            //bool added = await _redis.Db1.AddAsync(key, data, DateTimeOffset.Now.AddHours(12));
            var response = data.GroupBy(q => q.WeightRangeId).Select(w => new AreaWiseChargeDetailsViewModel
                {
                    Weight = data.Where(g => g.WeightRangeId == w.Key).Select(x => x.Weight).FirstOrDefault(),
                    IsOpenBox = data.Where(g => g.WeightRangeId == w.Key).Select(x => x.IsOpenBox).FirstOrDefault(),
                    WeightRangeId = w.Key,
                    WeightRangeWiseData = data.Where(v => v.WeightRangeId == w.Key).Select(j => new WeightRangeWiseData
                    {
                        DeliveryRangeId = j.DeliveryRangeId,
                        WeightRangeId = j.WeightRangeId,
                        ChargeAmount = j.CourierDeliveryCharge,
                        Type =j.Type,
                        //CityDeliveryCharge = j.CityDeliveryCharge,
                        Days = j.Day,
                        DeliveryType = j.Name,
                        Ranking = j.Ranking,
                        DayType = j.DayType,
                        OnImageLink = j.OnImageLink,
                        OffImageLink = j.OffImageLink,
                        ShowHide = j.ShowHide,
                        DeliveryAlertMessage=j.DeliveryAlertMessage,
                        LoginHours = j.LoginHours,
                        DateAdvance = j.DateAdvance,
                        DeliveryCharge = j.DeliveryCharge,
                        ExtraDeliveryCharge = j.ExtraDeliveryCharge,
                        ExtraCollectionCharge = j.ExtraCollectionCharge

                    }).OrderBy(o => o.Ranking).ToList(),
                    DeliveryTypeModel = data.Where(r => r.WeightRangeId == w.Key).Select(y => new DeliveryTypeModel
                    {

                        DeliveryType = y.Name,
                        DeliveryDayChargeModel = data.Where(u => u.WeightRangeId == y.WeightRangeId && u.DeliveryRangeId == y.DeliveryRangeId).Select(f => new DeliveryDayChargeModel
                        {

                            WeightRangeId = f.WeightRangeId,
                            DeliveryType = f.Name,
                            ChargeAmount = f.CourierDeliveryCharge,
                            Days = f.Day,
                            DayType = f.DayType

                        }).ToList()
                    }).ToList()
                });

                return response.OrderBy(r => r.WeightRangeId);
            //}
        }

        public async Task<IEnumerable<AreaWiseChargeDetailsViewModel>> DeliveryChargeMerchantDetailsAreaWise(DeliveryChargeDetailsBodyModel deliveryChargeDetailsBodyModel)
        {
            var data = await _deliveryChargeDetailsRepository.DeliveryChargeMerchantDetailsAreaWise(deliveryChargeDetailsBodyModel);

            var response = data.GroupBy(q => q.WeightRangeId).Select(w => new AreaWiseChargeDetailsViewModel
            {
                Weight = data.Where(g => g.WeightRangeId == w.Key).Select(x => x.Weight).FirstOrDefault(),
                IsOpenBox = data.Where(g => g.WeightRangeId == w.Key).Select(x => x.IsOpenBox).FirstOrDefault(),
                WeightRangeId = w.Key,
                WeightRangeWiseData = data.Where(v => v.WeightRangeId == w.Key).Select(j => new WeightRangeWiseData
                {
                    DeliveryRangeId = j.DeliveryRangeId,
                    WeightRangeId = j.WeightRangeId,
                    ChargeAmount = j.CourierDeliveryCharge,
                    Type = j.Type,
                    Days = j.Day,
                    DeliveryType = j.Name,
                    Ranking = j.Ranking,
                    DayType = j.DayType,
                    OnImageLink = j.OnImageLink,
                    OffImageLink = j.OffImageLink,
                    ShowHide = j.ShowHide,
                    DeliveryAlertMessage = j.DeliveryAlertMessage,
                    LoginHours = j.LoginHours,
                    DateAdvance = j.DateAdvance,
                    DeliveryCharge = j.DeliveryCharge,
                    ExtraDeliveryCharge = j.ExtraDeliveryCharge,
                    ExtraCollectionCharge = j.ExtraCollectionCharge

                }).OrderBy(o => o.Ranking).ToList(),
                DeliveryTypeModel = data.Where(r => r.WeightRangeId == w.Key).Select(y => new DeliveryTypeModel
                {

                    DeliveryType = y.Name,
                    DeliveryDayChargeModel = data.Where(u => u.WeightRangeId == y.WeightRangeId && u.DeliveryRangeId == y.DeliveryRangeId).Select(f => new DeliveryDayChargeModel
                    {

                        WeightRangeId = f.WeightRangeId,
                        DeliveryType = f.Name,
                        ChargeAmount = f.CourierDeliveryCharge,
                        Days = f.Day,
                        DayType = f.DayType

                    }).ToList()
                }).ToList()
            });

            return response.OrderBy(r => r.WeightRangeId);
            //}
        }

        public async Task<IEnumerable<GetDeliveryChargeDetailsViewModel>> DeliveryChargeDetailsSearchWise(DeliveryChargeDetailsSearchModel deliveryChargeDetailsSearch)
        {
            return await _deliveryChargeDetailsRepository.DeliveryChargeDetailsSearchWise(deliveryChargeDetailsSearch);
        }
        public async Task<IEnumerable<GetDeliveryChargeDetailsViewModel>> GetDeliveryChargeDetails()
        {
            return await _deliveryChargeDetailsRepository.GetDeliveryChargeDetails();

        }

        public async Task<IEnumerable<SACodChargesViewModel>> GetSACodChargeList()
        {
            return await _deliveryChargeDetailsRepository.GetSACodChargeList();
        }

        public async Task<DeliveryChargeDetails> UpdateDeliveryChargeDetails(int id, DeliveryChargeDetails deliveryChargeDetails)
        {
            return await _deliveryChargeDetailsRepository.UpdateDeliveryChargeDetails(id, deliveryChargeDetails);
        }

        public async Task<DeliveryChargeMerchantDetails> UpdateDeliveryChargeMerchantDetails(int id, DeliveryChargeMerchantDetails deliveryChargeMerchantDetails)
        {
            return await _deliveryChargeDetailsRepository.UpdateDeliveryChargeMerchantDetails(id, deliveryChargeMerchantDetails);
        }
    }
}
