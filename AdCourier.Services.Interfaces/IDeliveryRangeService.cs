using AdCourier.Domain.Entities.BodyModel.CourierOrder;
using AdCourier.Domain.Entities.DataModel;
using AdCourier.Domain.Entities.ViewModel.DatabaseViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdCourier.Services.Interfaces
{
    public interface IDeliveryRangeService
    {
        Task<int> AddNumnerInGroup(List<OwnPhoneBook> ownPhoneBook);
        Task<List<PhoneBookGroup>> AddPhoneBookGroup(List<PhoneBookGroup> phoneBookGroup);
        Task<List<OwnPhoneBook>> AddOwnPhoneBook(List<OwnPhoneBook> ownPhoneBook);
        Task<DeliveryRange> AddDeliveryRange(DeliveryRange deliveryRange);
        Task<IEnumerable<DeliveryRange>> GetDeliveryRange();
        Task<IEnumerable<CollectionTimeSlotViewModel>> GetCollectionTimeSlot();
        Task<DeliveryRange> UpdateDeliveryRange(int id, DeliveryRange deliveryRange);
        Task<List<CollectionTimeSlotViewModel>> GetCollectionTimeSlotByTime(RequestBodyModel request);
    }
}
