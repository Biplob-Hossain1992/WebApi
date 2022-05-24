using AdCourier.Domain.Entities.BodyModel.CourierOrder;
using AdCourier.Domain.Entities.DataModel;
using AdCourier.Domain.Entities.ViewModel.DatabaseViewModel;
using AdCourier.Domain.Interfaces;
using AdCourier.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdCourier.Services
{
    public class DeliveryRangeService : IDeliveryRangeService
    {
        private readonly IDeliveryRangeRepository _deliveryRangeRepository;
        public DeliveryRangeService(IDeliveryRangeRepository deliveryRangeRepository)
        {
            _deliveryRangeRepository = deliveryRangeRepository;
        }
        public async Task<DeliveryRange> AddDeliveryRange(DeliveryRange deliveryRange)
        {
            return await _deliveryRangeRepository.AddDeliveryRange(deliveryRange);
        }

        public async Task<List<PhoneBookGroup>> AddPhoneBookGroup(List<PhoneBookGroup> phoneBookGroup)
        {
            return await _deliveryRangeRepository.AddPhoneBookGroup(phoneBookGroup);
        }
        public async Task<List<OwnPhoneBook>> AddOwnPhoneBook(List<OwnPhoneBook> ownPhoneBook)
        {
            return await _deliveryRangeRepository.AddOwnPhoneBook(ownPhoneBook);
        }

        public async Task<IEnumerable<CollectionTimeSlotViewModel>> GetCollectionTimeSlot()
        {
            return await _deliveryRangeRepository.GetCollectionTimeSlot();
        }

        public async Task<List<CollectionTimeSlotViewModel>> GetCollectionTimeSlotByTime(RequestBodyModel request)
        {
            return await _deliveryRangeRepository.GetCollectionTimeSlotByTime(request);
        }

        public async Task<IEnumerable<DeliveryRange>> GetDeliveryRange()
        {
            return await _deliveryRangeRepository.GetDeliveryRange();
        }

        public async Task<DeliveryRange> UpdateDeliveryRange(int id, DeliveryRange deliveryRange)
        {
            return await _deliveryRangeRepository.UpdateDeliveryRange(id, deliveryRange);
        }

        public async Task<int> AddNumnerInGroup(List<OwnPhoneBook> ownPhoneBook)
        {
            return await _deliveryRangeRepository.AddNumnerInGroup(ownPhoneBook);
        }
    }
}
