using AdCourier.Context;
using AdCourier.Domain.Entities.ViewModel.ReturnProducts;
using AdCourier.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AdCourier.Domain.Entities.ViewModel.CourierOrder;
using Microsoft.AspNetCore.Http;
using AdCourier.Domain.Entities.BodyModel.CourierOrder;

namespace AdCourier.Infrastructure.Data
{
    public class ReturnRepository : IReturnRepository
    {
        private readonly SqlServerContext _sqlServerContext;
        //private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IOrderRepository _orderRepository;
        public ReturnRepository(SqlServerContext sqlServerContext, IOrderRepository orderRepository) //IHttpContextAccessor httpContextAccessor,
        {
            _sqlServerContext = sqlServerContext ?? throw new ArgumentNullException(nameof(sqlServerContext));
            //_httpContextAccessor = httpContextAccessor;
            _orderRepository = orderRepository;
        }
        public async Task<ReturnProductsViewModel> GetAllReturnProducts()
        {
            var totalOrderCount = await _sqlServerContext.CourierOrders.Where(g => g.Status == 19).CountAsync();
            try
            {
                var loadCourierOrderBodyModel = new LoadCourierOrderBodyModel
                {
                    Index = 0,
                    Count = totalOrderCount,
                    Status = 19,
                    FromDate = new DateTime(2001, 1, 1),
                    ToDate = new DateTime(2001, 1, 1),
                    OrderIds = "",
                    PodNumber = "",
                    CourierUserId = -1
                };
                var returnOrders = await _orderRepository.RetriveOrderList_Admin(loadCourierOrderBodyModel);

                var merchantListData =  returnOrders.CourierOrderViewModel.GroupBy(g => g.UserInfo.CourierUserId).Select(s => new ReturnMerchantDetails
                {
                    UserId = s.FirstOrDefault().UserInfo.CourierUserId,
                    MerchantName = s.FirstOrDefault().UserInfo.UserName,
                    CompanyName = s.FirstOrDefault().UserInfo.CompanyName,
                    MerchantMobile = s.FirstOrDefault().UserInfo.Mobile,
                    ReturnCollectorId = _sqlServerContext.CollectorAssign.Where(c => c.CourierUserId == s.FirstOrDefault().UserInfo.CourierUserId && c.AssignType == "return").Select(g => g.CollectorId).FirstOrDefault(),
                    ReturnCollectorName = _sqlServerContext.Collectors.Where(c => c.CollectorId == _sqlServerContext.CollectorAssign.Where(ca => ca.CourierUserId == s.FirstOrDefault().UserInfo.CourierUserId && ca.AssignType == "return").Select(g => g.CollectorId).FirstOrDefault()).Select(cn => cn.CollectorName).FirstOrDefault(),
                    TotalReturnProductCount = s.Count(),
                    ReturnProductDetails = s.ToList()
                }).OrderByDescending(u => u.TotalReturnProductCount);
                var responseData = new ReturnProductsViewModel
                {
                    TotalReturnMerchantCount = merchantListData.Count(),
                    TotalReturnProductCount = totalOrderCount,
                    ReturnMerchantDetails = await Task.FromResult(merchantListData)
                };
                return responseData;
            }
            catch (Exception ex)
            {

                throw;
            } 
        }
        public async Task<ReturnProductsViewModel> GetAllReturnProductsReport(string statusIds)
        {
            int[] statusList = statusIds.Split(',').Select(Int32.Parse).ToArray();
            var totalOrderCount = await _sqlServerContext.CourierOrders.Where(g => statusList.Contains(g.Status)).CountAsync();
            try
            {
                var loadCourierOrderBodyModel = new LoadCourierOrderBodyModel
                {
                    Index = 0,
                    Count = totalOrderCount,
                    StatusList = statusList,
                    FromDate = new DateTime(2001, 1, 1),
                    ToDate = new DateTime(2001, 1, 1),
                    OrderIds = "",
                    PodNumber = "",
                    CourierUserId = -1
                };
                var returnOrders = await _orderRepository.RetriveOrderList_Admin(loadCourierOrderBodyModel);

                var merchantListData = returnOrders.CourierOrderViewModel.GroupBy(g => g.UserInfo.CourierUserId).Select(s => new ReturnMerchantDetails
                {
                    UserId = s.FirstOrDefault().UserInfo.CourierUserId,
                    MerchantName = s.FirstOrDefault().UserInfo.UserName,
                    CompanyName = s.FirstOrDefault().UserInfo.CompanyName,
                    MerchantMobile = s.FirstOrDefault().UserInfo.Mobile,
                    ReturnCollectorId = 0, // _sqlServerContext.CollectorAssign.Where(c => c.CourierUserId == s.FirstOrDefault().UserInfo.CourierUserId && c.AssignType == "return").Select(g => g.CollectorId).FirstOrDefault(),
                    ReturnCollectorName = "", //_sqlServerContext.Collectors.Where(c => c.CollectorId == _sqlServerContext.CollectorAssign.Where(ca => ca.CourierUserId == s.FirstOrDefault().UserInfo.CourierUserId && ca.AssignType == "return").Select(g => g.CollectorId).FirstOrDefault()).Select(cn => cn.CollectorName).FirstOrDefault(),
                    TotalReturnProductCount = s.Count(),
                    ReturnProductDetails = s.ToList()
                }).OrderByDescending(u => u.TotalReturnProductCount);
                var responseData = new ReturnProductsViewModel
                {
                    TotalReturnMerchantCount = merchantListData.Count(),
                    TotalReturnProductCount = totalOrderCount,
                    ReturnMerchantDetails = await Task.FromResult(merchantListData)
                };
                return responseData;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
