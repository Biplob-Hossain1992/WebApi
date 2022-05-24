using AdCourier.Context;
using AdCourier.Domain.Entities.BodyModel.CourierOrder;
using AdCourier.Domain.Entities.DataModel;
using AdCourier.Domain.Entities.ViewModel.DatabaseViewModel;
using AdCourier.Domain.Interfaces;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdCourier.Infrastructure.Data
{

    public class DeliveryRangeRepository : IDeliveryRangeRepository
    {
        private readonly SqlServerContext _sqlServerContext;
        public DeliveryRangeRepository(SqlServerContext sqlServerContext)
        {
            _sqlServerContext = sqlServerContext ?? throw new ArgumentNullException(nameof(sqlServerContext));
        }

        public async Task<DeliveryRange> AddDeliveryRange(DeliveryRange deliveryRange)
        {
            await _sqlServerContext.DeliveryRange.AddAsync(deliveryRange);
            await _sqlServerContext.SaveChangesAsync();
            return deliveryRange;
        }

        public async Task<List<DeliveryRange>> GetDeliveryRange()
        {
            IQueryable<DeliveryRange> data = from d in _sqlServerContext.DeliveryRange.AsNoTracking()
                                             where d.IsActive == true
                                             select d;
            return await data.ToListAsync();
        }

        public async Task<IEnumerable<CollectionTimeSlotViewModel>> GetCollectionTimeSlot()
        {
            var data = from d in _sqlServerContext.CollectionTimeSlot.AsNoTracking()
                                                  orderby d.Ordering ascending
                                                  where d.IsActive == true
                                                  select new CollectionTimeSlotViewModel
                                                  {
                                                      CollectionTimeSlotId = d.CollectionTimeSlotId,
                                                      StartTime = d.StartTime,
                                                      EndTime = d.EndTime,
                                                      Ordering = d.Ordering,
                                                      OrderLimit = d.OrderLimit,
                                                      IsActive = d.IsActive,
                                                      CutOffTime = d.CutOffTime,
                                                      FormattingStartTime = new DateTime(d.StartTime.Value.Ticks).ToString("hh:mm tt"),
                                                      FormattingEndTime = new DateTime(d.EndTime.Value.Ticks).ToString("hh:mm tt"),
                                                      FormattingCutOffTime = new DateTime(d.CutOffTime.Value.Ticks).ToString("hh:mm tt"),
                                                      SlotName = d.SlotName
                                                  };
            return await data.ToListAsync();
        }

        public async Task<DeliveryRange> UpdateDeliveryRange(int id, DeliveryRange deliveryRange)
        {
            var entity = await _sqlServerContext.DeliveryRange.FirstOrDefaultAsync(item => item.Id == id);
            if (entity != null)
            {
                entity.Name = deliveryRange.Name;
                entity.Day = deliveryRange.Day;
                entity.IsActive = deliveryRange.IsActive;
                entity.DayType = deliveryRange.DayType;
                entity.OffImageLink = deliveryRange.OffImageLink;
                entity.OnImageLink = deliveryRange.OnImageLink;
                entity.CourierDeliveryCharge = deliveryRange.CourierDeliveryCharge;
                entity.Type = deliveryRange.Type;
                // Update entity in DbSet
                _sqlServerContext.DeliveryRange.Update(entity);

                // Save changes in database
                await _sqlServerContext.SaveChangesAsync();
            }

            return entity;
        }

        public async Task<List<CollectionTimeSlotViewModel>> GetCollectionTimeSlotByTime(RequestBodyModel request)
        {
            if(request.RequestDate.Date == DateTime.Now.Date)
            {
                var time = DateTime.Now.TimeOfDay;
                return await _sqlServerContext.CollectionTimeSlot.AsNoTracking().Where(x => !(x.StartTime < time && x.EndTime < time) && x.IsActive.Equals(true)).Select(d => new CollectionTimeSlotViewModel {
                    CollectionTimeSlotId = d.CollectionTimeSlotId,
                    StartTime = d.StartTime,
                    EndTime = d.EndTime,
                    Ordering = d.Ordering,
                    OrderLimit = d.OrderLimit,
                    IsActive = d.IsActive,
                    CutOffTime = d.CutOffTime,
                    FormattingStartTime = new DateTime(d.StartTime.Value.Ticks).ToString("hh:mm tt"),
                    FormattingEndTime = new DateTime(d.EndTime.Value.Ticks).ToString("hh:mm tt"),
                    FormattingCutOffTime = new DateTime(d.CutOffTime.Value.Ticks).ToString("hh:mm tt"),
                    SlotName = d.SlotName
                }).OrderBy(y => y.Ordering).ToListAsync();
            }
            else
            {
                return await _sqlServerContext.CollectionTimeSlot.AsNoTracking().Where(x => x.IsActive == true).Select(d => new CollectionTimeSlotViewModel {
                    CollectionTimeSlotId = d.CollectionTimeSlotId,
                    StartTime = d.StartTime,
                    EndTime = d.EndTime,
                    Ordering = d.Ordering,
                    OrderLimit = d.OrderLimit,
                    IsActive = d.IsActive,
                    CutOffTime = d.CutOffTime,
                    FormattingStartTime = new DateTime(d.StartTime.Value.Ticks).ToString("hh:mm tt"),
                    FormattingEndTime = new DateTime(d.EndTime.Value.Ticks).ToString("hh:mm tt"),
                    FormattingCutOffTime = new DateTime(d.CutOffTime.Value.Ticks).ToString("hh:mm tt"),
                    SlotName = d.SlotName
                }).OrderBy(y => y.Ordering).ToListAsync(); 
            }
        }

        public async Task<List<OwnPhoneBook>> AddOwnPhoneBook(List<OwnPhoneBook> ownPhoneBook)
        {
            await _sqlServerContext.OwnPhoneBook.AddRangeAsync(ownPhoneBook);
            await _sqlServerContext.SaveChangesAsync();
            return ownPhoneBook;
        }

        public async Task<List<PhoneBookGroup>> AddPhoneBookGroup(List<PhoneBookGroup> phoneBookGroup)
        {
            await _sqlServerContext.PhoneBookGroup.AddRangeAsync(phoneBookGroup);
            await _sqlServerContext.SaveChangesAsync();
            return phoneBookGroup;
        }

        public async Task<int> AddNumnerInGroup(List<OwnPhoneBook> ownPhoneBook)
        {
            var mobiles = ownPhoneBook.Select(s => s.Mobile).ToArray();


            var entity = await _sqlServerContext.OwnPhoneBook.AsNoTracking().Where(x => mobiles.Contains(x.Mobile))
                                .BatchUpdateAsync(x => new OwnPhoneBook
                                {
                                    PhoneBookGroupId = ownPhoneBook.FirstOrDefault().PhoneBookGroupId
                                });

            return entity;
        }
    }
}
