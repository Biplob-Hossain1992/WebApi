using AdCourier.Context;
using AdCourier.Domain.Entities.BodyModel.CourierOrder;
using AdCourier.Domain.Entities.DataModel;
using AdCourier.Domain.Entities.ViewModel.InstantCodViewModel;
using AdCourier.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdCourier.Infrastructure.Data
{
    public class InstantCodRepository : IInstantCodRepository
    {
        private readonly SqlServerContext _sqlServerContext;
        public InstantCodRepository(SqlServerContext sqlServerContext)
        {
            _sqlServerContext = sqlServerContext ?? throw new ArgumentNullException(nameof(sqlServerContext));
        }

        public async Task<CourierOrders> UpdateInstantCodOrder(CourierOrders courierOrders)
        {
            var collectionAmount = courierOrders.CollectionAmount;
            var entity = await _sqlServerContext.CourierOrders.AsNoTracking().FirstOrDefaultAsync(item => item.Id == courierOrders.Id);
            var courierName = await _sqlServerContext.CourierLocation.AsNoTracking().FirstOrDefaultAsync(c => c.CourierLocationId == courierOrders.CourierLocationId);
            if (entity != null)
            {
                
                if (courierOrders.CollectionAmount > 0)
                {
                    entity.ActualPackagePrice = courierOrders.CollectionAmount;
                }
                var codCharge = collectionAmount > 0 && collectionAmount <= 1000 ? 20 
                    : collectionAmount >= 1001 && collectionAmount <= 2000 ? 30
                    : collectionAmount >= 2001 && collectionAmount <= 3000 ? 40
                    : collectionAmount >= 3001 && collectionAmount <= 5000 ? 50
                    : collectionAmount >= 5001 && collectionAmount <= 50000 ? 10
                    : collectionAmount > 50000 ? 8
                    : 0;
                entity.CodCharge = codCharge;
                entity.OrderType = "Delivery Taka Collection";
                entity.Status = 40;
                entity.Mobile = courierOrders.Mobile;
                entity.CustomerName = courierOrders.CustomerName;
                entity.OtherMobile = courierOrders.OtherMobile;
                entity.Address = courierOrders.Address;
                entity.DistrictId = courierOrders.DistrictId;
                entity.ThanaId = courierOrders.ThanaId;
                entity.AreaId = courierOrders.AreaId;
                entity.InvoiceCourier = courierName == null ? "" : courierOrders.InvoiceCourier;
                entity.InvoiceNumber = courierOrders.InvoiceNumber;
                entity.CourierLocationId = courierOrders.CourierLocationId;
                entity.CollectAddressDistrictId = courierOrders.CollectAddressDistrictId;
                entity.CollectAddressThanaId = courierOrders.CollectAddressThanaId;
                entity.CollectionAmount = courierOrders.CollectionAmount;
                entity.CollectionName = courierOrders.CollectionName;
                entity.PodNumber = courierOrders.InvoiceNumber;
                if (_sqlServerContext.Entry(entity).State != EntityState.Unchanged)
                {
                    courierOrders = _sqlServerContext.Update(entity).Entity;
                    await _sqlServerContext.SaveChangesAsync();
                }
            }

            return entity;
        }

        public async Task<IEnumerable<dynamic>> GetInstantCodOrders(RequestBodyModel request)
        {

            var courierLocations = await _sqlServerContext.CourierLocation.Where(s => s.IsActive == true).ToListAsync();

            var allDistricts = await _sqlServerContext.Districts.Where(z => z.IsActive == true).Select(s => new
            {
                s.District,
                s.DistrictBng,
                s.DistrictId,
                s.ParentId,
                s.IsActive
            }).ToListAsync();


            var courierUserIds = await _sqlServerContext.CourierOrders.Where(w => w.OrderDate.Date >= request.FromDate.Date
                                && w.OrderDate.Date < request.ToDate.Date.AddDays(1)
                                && w.OrderFrom.Equals("instacod")).Select(s => s.MerchantId).ToArrayAsync();
            //&& w.Id.Equals(955871)).Select(s => s.MerchantId).ToArrayAsync();


            var pickupLocations = await _sqlServerContext.PickupLocations.Where(w => courierUserIds.Contains(w.CourierUserId) && w.IsActive.Equals(true)).ToListAsync();


            var pickupDistricts = (from dis in allDistricts
                                   join pic in pickupLocations on dis.DistrictId equals pic.DistrictId
                                   select new
                                   {
                                       dis.District,
                                       dis.DistrictBng,
                                       dis.DistrictId,
                                       dis.ParentId,
                                       dis.IsActive,
                                       pic.CourierUserId
                                   }).Distinct().ToList();


            var pickupThanas = (from dis in allDistricts
                                join pic in pickupLocations on dis.DistrictId equals pic.ThanaId
                                select new
                                {
                                    dis.District,
                                    dis.DistrictBng,
                                    dis.DistrictId,
                                    dis.ParentId,
                                    dis.IsActive,
                                    pic.CourierUserId
                                }).ToList();


            var instantOrders = await (from data in _sqlServerContext.CourierOrders.AsNoTracking()
                                       where data.OrderDate >= request.FromDate.Date && data.OrderDate.Date < request.ToDate.Date.AddDays(1)

                                       ////&& data.OrderFrom.Equals("desktop site")
                                       && data.OrderFrom.Equals("instacod")
                                       //where data.Id.Equals(955871)
                                       orderby data.Id descending
                                       select new
                                       {
                                           Id = data.Id,
                                           MerchantId = data.MerchantId,
                                           CustomerName = data.CustomerName,
                                           Mobile = data.Mobile,
                                           OtherMobile = data.OtherMobile,
                                           Address = data.Address,
                                           DistrictId = data.DistrictId,
                                           ThanaId = data.ThanaId,
                                           AreaId = data.AreaId,
                                           ActualPackagePrice = data.ActualPackagePrice,
                                           CollectionAmount = data.CollectionAmount,
                                           CollectionName = data.CollectionName,
                                           InvoiceNumber = data.InvoiceNumber,
                                           QuickOrderImageUrl = data.QuickOrderImageUrl,
                                           CollectAddressDistrictId = data.CollectAddressDistrictId,
                                           CollectAddressThanaId = data.CollectAddressThanaId,
                                           Districts = allDistricts.Where(w => w.ParentId.Equals(0)),
                                           Thanas = data.ThanaId != 0 ? allDistricts.Where(z => z.ParentId == data.DistrictId && z.IsActive == true) : null,
                                           Areas = data.AreaId != 0 ? allDistricts.Where(z => z.ParentId == data.ThanaId && z.IsActive == true) : null,
                                           CollectDistricts = pickupDistricts.Where(w => w.CourierUserId.Equals(data.MerchantId)),
                                           CollectThanas = pickupThanas.Where(w => w.CourierUserId.Equals(data.MerchantId)),
                                           CourierLocationId = data.CourierLocationId,
                                           CourierLocations = courierLocations

                                       }).ToListAsync();
            return instantOrders;
        }

        public async Task<dynamic> GetInstantCodCollectionDetails(RequestBodyModel request)
        {
            var statusArray = new int[] { 0, 40, 44 };
            var instantCodCountView = new InstantCodCountView();
            var instantCod = await (
                from orders in _sqlServerContext.CourierOrders.AsNoTracking()
                where
                statusArray.Contains(orders.Status) &&
                orders.OrderFrom.Equals("instacod") &&
                //&&
                //orders.OrderDate.Date >= request.FromDate.Date &&
                //orders.OrderDate.Date < request.ToDate.Date.AddDays(1) &&
                orders.MerchantId == request.MerchantId
                select new
                {
                    Id = orders.Id,
                    CollectionAmount = orders.CollectionAmount,
                    Status = orders.Status,
                }).ToListAsync();

            var UnCollectedOrdersCount = instantCod.Count(x => x.Status == 0 || x.Status == 40);
            var SaReceiptCollectionCount = instantCod.Count(x => x.Status == 44);
            var SaReceiptCollectionAmount = instantCod.Where(x => x.Status == 44).Sum(x => x.CollectionAmount);

            return new InstantCodCountView
            {
                UnCollectedOrdersCount = UnCollectedOrdersCount,
                SaReceiptCollectionCount = SaReceiptCollectionCount,
                SaReceiptCollectionAmount = SaReceiptCollectionAmount,
            };

        }

        public async Task<IEnumerable<CourierLocation>> GetCourierLocations(int isActive)
        {
            if (isActive == 1)
            {
                return await _sqlServerContext.CourierLocation.Where(w => w.IsActive == true).ToListAsync();
            }
            else if (isActive == 2)
            {
                return await _sqlServerContext.CourierLocation.Where(w => w.IsActive == false).ToListAsync();
            }
            else
            {
                return await _sqlServerContext.CourierLocation.ToListAsync();
            }

        }

        public async Task<CourierLocation> AddCourierLocation(CourierLocation courierLocation)
        {
            await _sqlServerContext.CourierLocation.AddRangeAsync(courierLocation);
            await _sqlServerContext.SaveChangesAsync();
            return courierLocation;
        }

        public async Task<bool> CheckInstaCod(RequestBodyModel request)
        {

            var result = await _sqlServerContext.CourierOrders.AnyAsync(c => c.MerchantId == request.MerchantId && c.CollectionName == request.CollectionName);
            return result;
        }
    }
}
