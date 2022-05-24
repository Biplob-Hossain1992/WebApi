
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AdCourier.Domain.Entities.ViewModel.BondhuApp;

namespace AdCourier.Services
{
    public class SelfDeliveryService
    {
        public string ColorCode1 = "#45BE59";
        public string ColorCode2 = "#322E7F";
        public string ColorCode3 = "#F15B2D";
        public string ColorCode4 = "#c32148";
        public string ColorCode5 = "#A9A9A9";

        public List<ActionData> PodWiseActionData(int StatusId)
        {
            var data = new List<ActionData>();
            if (StatusId == 32 || StatusId == 1 || StatusId == 11 || StatusId == 1003 || StatusId == 1011)
            {
                data.Add(new ActionData
                {
                    ActionType = 1,
                    ActionMessage = "বুঝে পেয়েছি",
                    UpdateStatus = 440,
                    StatusMessage = "DeliveryMan Accept To Delivery Product",
                    ColorCode = ColorCode1
                });
            }

            return data;
        }
        public List<ActionData> PodWiseCustomerActionData(int StatusId, string PaymentType)
        {
            var data = new List<ActionData>();

            if (StatusId == 440)
            {
                data.Add(new ActionData
                {
                    ActionType = 1,
                    ActionMessage = "ডেলিভারি দেয়া হয়েছে",
                    UpdateStatus = 442,
                    StatusMessage = "Delivery Done",
                    ColorCode = ColorCode3
                });
                data.Add(new ActionData
                {
                    ActionType = 2,
                    ActionMessage = "ডেলিভারি নিতে আগ্রহী নয়",
                    UpdateStatus = 443,
                    StatusMessage = "The customer is not interested",
                    ColorCode = ColorCode2
                });

            }
            if (StatusId == 442 && (PaymentType.Trim().ToLower() == "mpd" || PaymentType.Trim().ToLower() == "mpc"))
            {
                data.Add(new ActionData
                {
                    ActionType = 1,
                    ActionMessage = "পেমেন্ট করুন",
                    UpdateStatus = 446,
                    StatusMessage = "Payment Done",
                    ColorCode = ColorCode3,
                    Icon = "http://static.ajkerdeal.com/images/appimages/bkash.svg",
                    IsPaymentType = 1
                });

            }
            return data;
        }

        public List<ActionData> PodCouponWiseCustomerActionData(int StatusId)
        {
            var data = new List<ActionData>();

            //if (StatusId == 440)
            //{
            //    data.Add(new ActionData
            //    {
            //        ActionType = 1,
            //        ActionMessage = "ডেলিভারি দেয়া হয়েছে",
            //        UpdateStatus = 442,
            //        StatusMessage = "Delivery Done",
            //        ColorCode = "#45BE59"
            //    });
            //    data.Add(new ActionData
            //    {
            //        ActionType = 2,
            //        ActionMessage = "ডেলিভারি নিতে আগ্রহী নয়",
            //        UpdateStatus = 443,
            //        StatusMessage = "The customer is not interested",
            //        ColorCode = "#ECECEC"
            //    });

            //}

            return data;
        }

        public SourceMessage CustomerSourceMessage(int StatusId, string PaymentType, string CollectionAddress, int CouponPrice, int SourceDealPrice, int DeliveryManCommission, int deliveryTimePayment, int deliveryCharge, string orderId)
        {
            SourceMessage model = new SourceMessage();
            string Commission = ToBanglaDigit(Convert.ToString(DeliveryManCommission));
            string PayAmountToAd = ToBanglaDigit(Convert.ToString(CouponPrice - DeliveryManCommission));
            string ProductCouponPrice = ToBanglaDigit(Convert.ToString(CouponPrice));
            string TotalPayment = ToBanglaDigit(Convert.ToString(deliveryTimePayment));

            if (orderId.ToLower().Contains("dt") == true)
            {

                if ((StatusId == 40) && PaymentType.ToLower() == "delivery taka collection")
                {
                    // model.Message = "<b>ডেলিভারির সময় পেমেন্টঃ</b> <font color='#F15B2D'><b>" + TotalPayment + " টাকা </b></font>";
                }
                else
                {
                    // model.Message = "<b>ডেলিভারির সময় পেমেন্টঃ</b> প্রয়োজন নেই";
                }

            }
            else
            {


            }
            return model;
        }

        public CustomerMessage CustomerMessage(int StatusId, string PaymentType, string CollectionAddress, int CouponPrice, int SourceDealPrice, int DeliveryManCommission, int CouponDeliveryManCommission, int deliveryTimePayment, int DeliveryCharge)
        {
            int DeliveryCommission = CouponDeliveryManCommission == 0 ? DeliveryManCommission : CouponDeliveryManCommission;
            string Commission = ToBanglaDigit(Convert.ToString(DeliveryCommission));
            string TotalPrice = ToBanglaDigit(Convert.ToString(deliveryTimePayment));
            CustomerMessage model = new CustomerMessage();
            if (StatusId == 440)
            {
                if (PaymentType.ToLower() == "mpd" && PaymentType.ToLower() == "mpc")
                {
                    model.Message = "<b>ডেলিভারির সময় পেমেন্টঃ</b> <font color='#F15B2D'><b>" + TotalPrice + "  টাকা</b></font>";
                }
                else //if (StatusId == 549 && PaymentType.ToLower() != "mpd")
                {
                    model.Message = "<b>ডেলিভারির সময় পেমেন্টঃ</b> প্রয়োজন নেই";
                }

            }
            if (StatusId == 441)
            {
                model.Message = "<b>ডেলিভারির কমিশনঃ</b> <font color='#F15B2D'><b>" + Commission + " টাকা </b></font>";
                model.Instructions = "";
            }
            return model;
        }

        public CustomerMessage CustomerMessageForPodCoupons(int StatusId, string PaymentType, string CollectionAddress, int CouponPrice, int SourceDealPrice, int DeliveryManCommission, int CouponDeliveryManCommission)
        {
            int DeliveryCommission = CouponDeliveryManCommission == 0 ? DeliveryManCommission : CouponDeliveryManCommission;
            string Commission = ToBanglaDigit(Convert.ToString(DeliveryCommission));
            CustomerMessage model = new CustomerMessage();

            return model;
        }

        public List<ActionData> CustomerActionData(int StatusId, string orderId, string type)
        {
            var data = new List<ActionData>();

            if (orderId.ToLower().Contains("dt") == true)
            {
                // return start

                if (StatusId == 60 && type == "return") //নতুন অর্ডার
                {
                    data.Add(new ActionData
                    {
                        ActionType = 1,
                        ActionMessage = "অ্যাকসেপ্ট",
                        UpdateStatus = 38,
                        StatusMessage = "Return Accepted by Delivery Man",
                        ColorCode = ColorCode1
                    });
                }


                // collection start
                else if (StatusId == 40 && type == "collection") //নতুন অর্ডার
                {
                    data.Add(new ActionData
                    {
                        ActionType = 1,
                        ActionMessage = "অ্যাকসেপ্ট",
                        UpdateStatus = 41,
                        StatusMessage = "Delivery Accepted by Delivery Man",
                        ColorCode = ColorCode1
                    });
                }

                else if (StatusId == 41 && type == "collection")
                {
                    data.Add(new ActionData
                    {
                        ActionType = 2,
                        ActionMessage = "ক্যানসেল",
                        UpdateStatus = 40,
                        StatusMessage = "Order confirm for collection",
                        ColorCode = ColorCode5,
                        PopUpDialog = 2
                    });
                    data.Add(new ActionData
                    {
                        ActionType = 2,
                        ActionMessage = "মার্চেন্টকে পাওয়া যাচ্ছে না",
                        UpdateStatus = 43,
                        StatusMessage = "Merchant is not available",
                        ColorCode = ColorCode2,
                        PopUpDialog = 1
                    });
                }

                // collection and delivery start
                else if (StatusId == 40 && type == "collectionanddelivery") //নতুন অর্ডার
                {
                    data.Add(new ActionData
                    {
                        ActionType = 1,
                        ActionMessage = "অ্যাকসেপ্ট",
                        UpdateStatus = 41,
                        StatusMessage = "Delivery Accepted by Delivery Man",
                        ColorCode = ColorCode1
                    });
                }
                else if (StatusId == 41 && type == "collectionanddelivery")
                {
                    data.Add(new ActionData
                    {
                        ActionType = 2,
                        ActionMessage = "ক্যানসেল",
                        UpdateStatus = 40,
                        StatusMessage = "Order confirm for collection",
                        ColorCode = ColorCode5,
                        PopUpDialog = 2
                    });
                    data.Add(new ActionData
                    {
                        ActionType = 2,
                        ActionMessage = "মার্চেন্টকে পাওয়া যাচ্ছে না",
                        UpdateStatus = 43,
                        StatusMessage = "Merchant is not available",
                        ColorCode = ColorCode2,
                        PopUpDialog = 1
                    });
                }

                else if (StatusId == 53 && type == "delivery") //নতুন অর্ডার
                {
                    //data.Add(new ActionData // action changed by Biplob according to Rafiq bhai (2021-10-28)
                    //{
                    //    ActionType = 1,
                    //    ActionMessage = "অ্যাকসেপ্ট",
                    //    UpdateStatus = 41,
                    //    StatusMessage = "Delivery Accepted by Delivery Man",
                    //    ColorCode = ColorCode1
                    //});
                    data.Add(new ActionData
                    {
                        ActionType = 1,
                        ActionMessage = "কালেক্ট করা হয়েছে",
                        UpdateStatus = 54,
                        StatusMessage = "Order has been collected from AD Hub",
                        ColorCode = ColorCode1,
                        CollectionPointAvailable = 1
                    });
                    data.Add(new ActionData
                    {
                        ActionType = 2,
                        ActionMessage = "ক্যানসেল",
                        UpdateStatus = 53,
                        StatusMessage = "Order confirm for delivery",
                        ColorCode = ColorCode5
                    });
                }

            }
            else
            {

                //return start
                if (StatusId == 297 && type == "return") //নতুন অর্ডার
                {
                    data.Add(new ActionData
                    {
                        ActionType = 1,
                        ActionMessage = "অ্যাকসেপ্ট",
                        UpdateStatus = 363,
                        StatusMessage = "Return Accepted by Delivery Man",
                        ColorCode = ColorCode1
                    });
                }

                //collection and delivery start
                if (StatusId == 333 && type == "collectionanddelivery" || StatusId == 555 && type == "collectionanddelivery"
                    || StatusId == 15 && type == "collectionanddelivery" || StatusId == 26 && type == "collectionanddelivery") //নতুন অর্ডার
                {
                    data.Add(new ActionData
                    {
                        ActionType = 1,
                        ActionMessage = "অ্যাকসেপ্ট",
                        UpdateStatus = 334,
                        StatusMessage = "Delivery Accepted by Delivery Man",
                        ColorCode = ColorCode1
                    });
                }
                else if (StatusId == 334 && type == "collectionanddelivery")
                {
                    data.Add(new ActionData
                    {
                        ActionType = 2,
                        ActionMessage = "ক্যানসেল",
                        UpdateStatus = 333,
                        StatusMessage = "Order has been assigned collection point from rider",
                        ColorCode = ColorCode5,
                        PopUpDialog = 2
                    });
                    data.Add(new ActionData
                    {
                        ActionType = 2,
                        ActionMessage = "মার্চেন্টকে পাওয়া যাচ্ছে না",
                        UpdateStatus = 338,
                        StatusMessage = "Merchant is not available",
                        ColorCode = ColorCode2,
                        PopUpDialog = 1
                    });
                }

                // collection start

                else if (StatusId == 333 && type == "collection" || StatusId == 555 && type == "collection"
                    || StatusId == 15 && type == "collection" || StatusId == 26 && type == "collection") //নতুন অর্ডার
                {
                    data.Add(new ActionData
                    {
                        ActionType = 1,
                        ActionMessage = "অ্যাকসেপ্ট",
                        UpdateStatus = 334,
                        StatusMessage = "Delivery Accepted by Delivery Man",
                        ColorCode = ColorCode1
                    });
                }
                else if (StatusId == 334 && type == "collection")
                {
                    data.Add(new ActionData
                    {
                        ActionType = 2,
                        ActionMessage = "ক্যানসেল",
                        UpdateStatus = 333,
                        StatusMessage = "Order has been assigned collection point from rider",
                        ColorCode = ColorCode5,
                        PopUpDialog = 2
                    });
                    data.Add(new ActionData
                    {
                        ActionType = 2,
                        ActionMessage = "মার্চেন্টকে পাওয়া যাচ্ছে না",
                        UpdateStatus = 338,
                        StatusMessage = "Merchant is not available",
                        ColorCode = ColorCode2,
                        PopUpDialog = 1
                    });
                }

                // delivery
                else if (StatusId == 358 && type == "delivery") //নতুন অর্ডার
                {
                    data.Add(new ActionData
                    {
                        ActionType = 1,
                        ActionMessage = "অ্যাকসেপ্ট",
                        UpdateStatus = 334,
                        StatusMessage = "Delivery Accepted by Delivery Man",
                        ColorCode = ColorCode1
                    });
                }
            }

            return data;
        }


        public ActionModel GetActionModel(int status)
        {
            var data = new ActionModel();
            if (status.Equals(0))
            {
                data.ButtonName = "নতুন অর্ডার";
            }
            else if (status.Equals(44))
            {
                data.ButtonName = "কালেক্ট করা হয়েছে";
            }
            else if (status.Equals(41))
            {
                data.ButtonName = "অ্যাকসেপ্ট";
            }

            return data;
        }
        public List<ActionModel> SetActionModel(int status)
        {
            var data = new List<ActionModel>();
            if (status.Equals(0))
            {
                data.Add(new ActionModel
                {
                    ButtonName = "অ্যাকসেপ্ট",
                    StatusUpdate = 41,
                    StatusMessage = "Delivery Accepted by Delivery Man",
                    ColorCode = ColorCode1
                });
            }
            else if (status.Equals(41))
            {
                data.Add(new ActionModel
                {
                    ButtonName = "কালেক্ট করবো",
                    StatusUpdate = 44,
                    StatusMessage = "Order has been collected from collection point",
                    ColorCode = ColorCode1
                });
            }

            return data;
        }

        public List<ActionData> SetActionDataModel(int StatusId, string orderId, int flag, string PaymentType, string type, int CollectionAmount)
        {
            var data = new List<ActionData>();

            if (orderId.ToLower().Contains("dt") == true)
            {
                // return start

                if (StatusId == 38 && type == "return") // অ্যাকসেপ্ট
                {
                    data.Add(new ActionData
                    {
                        ActionType = 1,
                        ActionMessage = "মার্চেন্টের হাতে হস্তান্তর",
                        UpdateStatus = 39,
                        StatusMessage = "Return hand over to merchant",
                        ColorCode = ColorCode1,
                        CollectionPointAvailable = 1
                    });

                    data.Add(new ActionData
                    {
                        ActionType = 2,
                        ActionMessage = "মার্চেন্ট গ্রহন করেনি",
                        UpdateStatus = 22,
                        StatusMessage = "Return order reject by Merchant",
                        ColorCode = ColorCode4,
                        CollectionPointAvailable = 1
                    });

                    data.Add(new ActionData
                    {
                        ActionType = 2,
                        ActionMessage = "মার্চেন্টকে পাওয়া যাচ্ছে না",
                        UpdateStatus = 66,
                        StatusMessage = "Return Merchant is unreachable",
                        ColorCode = ColorCode2,
                        CollectionPointAvailable = 1
                    });

                    data.Add(new ActionData
                    {
                        ActionType = 2,
                        ActionMessage = "রাইডারের কোনো প্রচেষ্টা নেই",
                        UpdateStatus = 67,
                        StatusMessage = "Not Attempt Rider",
                        ColorCode = ColorCode3,
                        CollectionPointAvailable = 1
                    });
                }
                    // collection start

                else if (StatusId == 41 && type == "collection") // অ্যাকসেপ্ট
                {

                    data.Add(new ActionData
                    {
                        ActionType = 1,
                        ActionMessage = "কালেক্ট করা হয়েছে",
                        UpdateStatus = 44,
                        StatusMessage = "Order has been collected from collection point",
                        ColorCode = ColorCode1,
                        CollectionPointAvailable = 1
                    });

                    data.Add(new ActionData
                    {
                        ActionType = 2,
                        ActionMessage = "মার্চেন্টের কাছে প্রোডাক্ট নাই",
                        UpdateStatus = 55,
                        StatusMessage = "Product is not available",
                        ColorCode = ColorCode4,
                        PopUpDialog = 1
                    });
                    data.Add(new ActionData
                    {
                        ActionType = 1,
                        ActionMessage = "মার্চেন্ট হোল্ড করেছেন",
                        UpdateStatus = 57,
                        StatusMessage = "Merchant On Hold",
                        ColorCode = ColorCode2,
                        CollectionPointAvailable = 1
                    });
                    data.Add(new ActionData
                    {
                        ActionType = 2,
                        ActionMessage = "রাইডারের কোনো প্রচেষ্টা নেই",
                        UpdateStatus = 67,
                        StatusMessage = "Not Attempt Rider",
                        ColorCode = ColorCode3,
                        CollectionPointAvailable = 1
                    });
                }

                //else if (StatusId == 44 && type == "collection") // কালেক্ট করা হয়েছে
                //{
                //    data.Add(new ActionData
                //    {
                //        ActionType = 1,
                //        ActionMessage = "হাবে ডেলিভারি দেয়া হয়েছে",
                //        UpdateStatus = 51,
                //        StatusMessage = "Delivered to AD hub",
                //        ColorCode = ColorCode3
                //    });
                //}
                else if ((StatusId == 57 && type == "collection")
                    || StatusId == 55 && type == "collection") 
                {
                    data.Add(new ActionData
                    {
                        ActionType = 1,
                        ActionMessage = "কালেক্ট করা হয়েছে",
                        UpdateStatus = 44,
                        StatusMessage = "Order has been collected from collection point",
                        ColorCode = ColorCode1,
                        CollectionPointAvailable = 1
                    });
                }
                else if (StatusId == 43 && type == "collection") //কালেক্ট করা সম্ভব নয়
                {
                    data.Add(new ActionData
                    {
                        ActionType = 1,
                        ActionMessage = "কালেক্ট করা হয়েছে",
                        UpdateStatus = 44,
                        StatusMessage = "Order has been collected from collection point",
                        ColorCode = ColorCode1,
                        CollectionPointAvailable = 1
                    });
                    data.Add(new ActionData
                    {
                        ActionType = 2,
                        ActionMessage = "মার্চেন্টের কাছে প্রোডাক্ট নাই",
                        UpdateStatus = 55,
                        StatusMessage = "Product is not available",
                        ColorCode = ColorCode4
                    });
                }

                // collection start

                // collection and delivery start

                else if ((StatusId == 57 && type == "collectionanddelivery")
                        || StatusId == 55 && type == "collectionanddelivery")
                {
                    data.Add(new ActionData
                    {
                        ActionType = 1,
                        ActionMessage = "কালেক্ট করা হয়েছে",
                        UpdateStatus = 44,
                        StatusMessage = "Order has been collected from collection point",
                        ColorCode = ColorCode1,
                        CollectionPointAvailable = 1
                    });
                }
                else if (StatusId == 41 && type == "collectionanddelivery") // অ্যাকসেপ্ট
                {
                    data.Add(new ActionData
                    {
                        ActionType = 1,
                        ActionMessage = "কালেক্ট করা হয়েছে",
                        UpdateStatus = 44,
                        StatusMessage = "Order has been collected from collection point",
                        ColorCode = ColorCode1,
                        CollectionPointAvailable = 1
                    });
                    data.Add(new ActionData
                    {
                        ActionType = 2,
                        ActionMessage = "মার্চেন্টের কাছে প্রোডাক্ট নাই",
                        UpdateStatus = 55,
                        StatusMessage = "Product is not available",
                        ColorCode = ColorCode4,
                        PopUpDialog = 1
                    });
                    data.Add(new ActionData
                    {
                        ActionType = 1,
                        ActionMessage = "মার্চেন্ট হোল্ড করেছেন",
                        UpdateStatus = 57,
                        StatusMessage = "Merchant On Hold",
                        ColorCode = ColorCode2,
                        CollectionPointAvailable = 1
                    });
                }

                else if (StatusId == 44 && type == "collectionanddelivery") // কালেক্ট করা হয়েছে
                {
                    data.Add(new ActionData
                    {
                        ActionType = 1,
                        ActionMessage = "ডেলিভারি দেয়া হয়েছে",
                        UpdateStatus = CollectionAmount > 0 ? 15 : 45,
                        StatusMessage = "Order Delivered by Delivery Man",
                        ColorCode = ColorCode3
                    });
                    data.Add(new ActionData
                    {
                        ActionType = 2,
                        ActionMessage = "কাস্টমার আগ্রহী নয়",
                        UpdateStatus = 42,
                        StatusMessage = "Customer not interested",
                        ColorCode = ColorCode2
                    });
                    data.Add(new ActionData
                    {
                        ActionType = 2,
                        ActionMessage = "কাস্টমার হোল্ড করেছেন",
                        UpdateStatus = 47,
                        StatusMessage = "Product hold by delivery bondhu",
                        ColorCode = ColorCode4
                    });
                }
                else if (StatusId == 43 && type == "collectionanddelivery")
                {
                    data.Add(new ActionData
                    {
                        ActionType = 1,
                        ActionMessage = "কালেক্ট করা হয়েছে",
                        UpdateStatus = 44,
                        StatusMessage = "Order has been collected from collection point",
                        ColorCode = ColorCode1,
                        CollectionPointAvailable = 1
                    });
                }

                else if (StatusId == 45 && type == "collectionanddelivery" && CollectionAmount > 0)
                {
                    data.Add(new ActionData
                    {
                        ActionType = 1,
                        ActionMessage = "পেমেন্ট করুন",
                        UpdateStatus = 49,
                        StatusMessage = "Payment Received From Rider",
                        ColorCode = ColorCode3,
                        Icon = "http://static.ajkerdeal.com/images/appimages/bkash.svg",
                        IsPaymentType = 1
                    });
                }

                // collection and delivery end

                //delivery start
                else if ((StatusId == 57 && type == "delivery")
                                || StatusId == 55 && type == "delivery")
                {
                    data.Add(new ActionData
                    {
                        ActionType = 1,
                        ActionMessage = "কালেক্ট করা হয়েছে",
                        UpdateStatus = 44,
                        StatusMessage = "Order has been collected from collection point",
                        ColorCode = ColorCode1,
                        CollectionPointAvailable = 1
                    });
                }
                else if (StatusId == 41 && type == "delivery") // অ্যাকসেপ্ট
                {
                    data.Add(new ActionData
                    {
                        ActionType = 1,
                        ActionMessage = "কালেক্ট করা হয়েছে",
                        UpdateStatus = 54,
                        StatusMessage = "Order has been collected from AD Hub",
                        ColorCode = ColorCode1,
                        CollectionPointAvailable = 1
                    });
                    data.Add(new ActionData
                    {
                        ActionType = 2,
                        ActionMessage = "ক্যানসেল",
                        UpdateStatus = 53,
                        StatusMessage = "Order confirm for delivery",
                        ColorCode = ColorCode5
                    });
                    data.Add(new ActionData
                    {
                        ActionType = 2,
                        ActionMessage = "রাইডারের কোনো প্রচেষ্টা নেই",
                        UpdateStatus = 67,
                        StatusMessage = "Not Attempt Rider",
                        ColorCode = ColorCode3,
                        CollectionPointAvailable = 1
                    });
                }
                else if (StatusId == 54 && type == "delivery") // কালেক্ট করা হয়েছে
                {
                    data.Add(new ActionData
                    {
                        ActionType = 1,
                        ActionMessage = "ডেলিভারি দেয়া হয়েছে",
                        UpdateStatus = CollectionAmount > 0 ? 15 : 45,
                        StatusMessage = "Order Delivered by Delivery Man",
                        ColorCode = ColorCode3
                    });
                    data.Add(new ActionData
                    {
                        ActionType = 2,
                        ActionMessage = "কাস্টমারকে পাওয়া যাচ্ছে না",
                        UpdateStatus = 33,
                        StatusMessage = "Customer Unreachable",
                        ColorCode = ColorCode4
                    });
                    data.Add(new ActionData
                    {
                        ActionType = 2,
                        ActionMessage = "কাস্টমার আগ্রহী নয়",
                        UpdateStatus = 42,
                        StatusMessage = "Customer not interested",
                        ColorCode = ColorCode2
                    });
                    data.Add(new ActionData
                    {
                        ActionType = 2,
                        ActionMessage = "কাস্টমার হোল্ড করেছেন",
                        UpdateStatus = 47,
                        StatusMessage = "Product hold by delivery bondhu",
                        ColorCode = ColorCode4
                    });
                }
                else if (StatusId == 45 && type == "delivery" && CollectionAmount > 0)
                {
                    data.Add(new ActionData
                    {
                        ActionType = 1,
                        ActionMessage = "পেমেন্ট করুন",
                        UpdateStatus = 49,
                        StatusMessage = "Payment Received From Rider",
                        ColorCode = ColorCode3,
                        Icon = "http://static.ajkerdeal.com/images/appimages/bkash.svg",
                        IsPaymentType = 1
                    });
                }
                //delivery end

            }
            else
            {

                // return start

                if (StatusId == 363 && type == "return") // অ্যাকসেপ্ট
                {
                    data.Add(new ActionData
                    {
                        ActionType = 1,
                        ActionMessage = "মার্চেন্টের হাতে হস্তান্তর",
                        UpdateStatus = 364,
                        StatusMessage = "Return hand over to merchant",
                        ColorCode = ColorCode1,
                        CollectionPointAvailable = 1
                    });
                    data.Add(new ActionData
                    {
                        ActionType = 2,
                        ActionMessage = "মার্চেন্ট গ্রহন করেনি",
                        UpdateStatus = 295,
                        StatusMessage = "Return order reject by Merchant",
                        ColorCode = ColorCode4,
                        CollectionPointAvailable = 1
                    });

                    data.Add(new ActionData
                    {
                        ActionType = 2,
                        ActionMessage = "মার্চেন্টকে পাওয়া যাচ্ছে না",
                        UpdateStatus = 365,
                        StatusMessage = "Return Merchant is unreachable",
                        ColorCode = ColorCode2,
                        CollectionPointAvailable = 1
                    });
                }

                else if (StatusId == 334 && type == "collectionanddelivery") // অ্যাকসেপ্ট
                {
                    data.Add(new ActionData
                    {
                        ActionType = 1,
                        ActionMessage = "কালেক্ট করা হয়েছে",
                        UpdateStatus = 335,
                        StatusMessage = "Order has been collected from collection point",
                        ColorCode = ColorCode1,
                        CollectionPointAvailable = 1
                    });
                    data.Add(new ActionData
                    {
                        ActionType = 2,
                        ActionMessage = "মার্চেন্টের কাছে প্রোডাক্ট নাই",
                        UpdateStatus = 356,
                        StatusMessage = "Product is not available",
                        ColorCode = ColorCode4,
                        PopUpDialog = 1
                    });
                    data.Add(new ActionData
                    {
                        ActionType = 1,
                        ActionMessage = "মার্চেন্ট হোল্ড করেছেন",
                        UpdateStatus = 361,
                        StatusMessage = "Merchant On Hold",
                        ColorCode = ColorCode2,
                        CollectionPointAvailable = 1
                    });
                }

                else if ((StatusId == 356 && type == "collectionanddelivery")
                || StatusId == 361 && type == "collectionanddelivery")
                {
                    data.Add(new ActionData
                    {
                        ActionType = 1,
                        ActionMessage = "কালেক্ট করা হয়েছে",
                        UpdateStatus = 335,
                        StatusMessage = "Order has been collected from collection point",
                        ColorCode = ColorCode1,
                        CollectionPointAvailable = 1
                    });
                }

                else if (StatusId == 338 && type == "collectionanddelivery") 
                {
                    data.Add(new ActionData
                    {
                        ActionType = 1,
                        ActionMessage = "কালেক্ট করা হয়েছে",
                        UpdateStatus = 335,
                        StatusMessage = "Order has been collected from collection point",
                        ColorCode = ColorCode1,
                        CollectionPointAvailable = 1
                    });
                }

                else if (StatusId == 335 && type == "collectionanddelivery") // কালেক্ট করা হয়েছে
                {
                    data.Add(new ActionData
                    {
                        ActionType = 1,
                        ActionMessage = "ডেলিভারি দেয়া হয়েছে",
                        UpdateStatus = 340,
                        StatusMessage = "Order Delivered by Delivery Man",
                        ColorCode = ColorCode3
                    });
                    data.Add(new ActionData
                    {
                        ActionType = 2,
                        ActionMessage = "কাস্টমার আগ্রহী নয়",
                        UpdateStatus = 341,
                        StatusMessage = "Customer not interested",
                        ColorCode = ColorCode2
                    });
                    data.Add(new ActionData
                    {
                        ActionType = 2,
                        ActionMessage = "কাস্টমার হোল্ড করেছেন",
                        UpdateStatus = 360,
                        StatusMessage = "Product hold by delivery bondhu",
                        ColorCode = ColorCode4
                    });
                }

                // collection start

                else if ((StatusId == 356 && type == "collection")
                            || StatusId == 361 && type == "collection")
                {
                    data.Add(new ActionData
                    {
                        ActionType = 1,
                        ActionMessage = "কালেক্ট করা হয়েছে",
                        UpdateStatus = 335,
                        StatusMessage = "Order has been collected from collection point",
                        ColorCode = ColorCode1,
                        CollectionPointAvailable = 1
                    });
                }
                else if(StatusId == 334 && type == "collection") // অ্যাকসেপ্ট
                {
                    data.Add(new ActionData
                    {
                        ActionType = 1,
                        ActionMessage = "কালেক্ট করা হয়েছে",
                        UpdateStatus = 335,
                        StatusMessage = "Order has been collected from collection point",
                        ColorCode = ColorCode1,
                        CollectionPointAvailable = 1
                    });
                    data.Add(new ActionData
                    {
                        ActionType = 2,
                        ActionMessage = "মার্চেন্টের কাছে প্রোডাক্ট নাই",
                        UpdateStatus = 356,
                        StatusMessage = "Product is not available",
                        ColorCode = ColorCode4,
                        PopUpDialog = 1
                    });
                    data.Add(new ActionData
                    {
                        ActionType = 1,
                        ActionMessage = "মার্চেন্ট হোল্ড করেছেন",
                        UpdateStatus = 361,
                        StatusMessage = "Merchant On Hold",
                        ColorCode = ColorCode2,
                        CollectionPointAvailable = 1
                    });
                }

                else if (StatusId == 338 && type == "collection")
                {
                    data.Add(new ActionData
                    {
                        ActionType = 1,
                        ActionMessage = "কালেক্ট করা হয়েছে",
                        UpdateStatus = 335,
                        StatusMessage = "Order has been collected from collection point",
                        ColorCode = ColorCode1,
                        CollectionPointAvailable = 1
                    });
                }

                else if (StatusId == 335 && type == "collection") // কালেক্ট করা হয়েছে
                {
                    data.Add(new ActionData
                    {
                        ActionType = 1,
                        ActionMessage = "হাবে ডেলিভারি দেয়া হয়েছে",
                        UpdateStatus = 357,
                        StatusMessage = "Delivered to AD hub",
                        ColorCode = ColorCode3
                    });
                }

                // delivery

                else if ((StatusId == 356 && type == "delivery")
                                || StatusId == 361 && type == "delivery")
                {
                    data.Add(new ActionData
                    {
                        ActionType = 1,
                        ActionMessage = "কালেক্ট করা হয়েছে",
                        UpdateStatus = 335,
                        StatusMessage = "Order has been collected from collection point",
                        ColorCode = ColorCode1,
                        CollectionPointAvailable = 1
                    });
                }
                else if (StatusId == 334 && type == "delivery") // অ্যাকসেপ্ট
                {
                    data.Add(new ActionData
                    {
                        ActionType = 1,
                        ActionMessage = "কালেক্ট করা হয়েছে",
                        UpdateStatus = 335,
                        StatusMessage = "Order has been collected from AD Hub",
                        ColorCode = ColorCode1,
                        CollectionPointAvailable = 1
                    });
                    data.Add(new ActionData
                    {
                        ActionType = 2,
                        ActionMessage = "ক্যানসেল",
                        UpdateStatus = 333,
                        StatusMessage = "Order has been assigned for delivery will be Collected from AD Hub",
                        ColorCode = ColorCode5
                    });
                }

                else if (StatusId == 359 && type == "delivery") // কালেক্ট করা হয়েছে
                {
                    data.Add(new ActionData
                    {
                        ActionType = 1,
                        ActionMessage = "ডেলিভারি দেয়া হয়েছে",
                        UpdateStatus = 340,
                        StatusMessage = "Order Delivered by Delivery Man",
                        ColorCode = ColorCode3
                    });
                    data.Add(new ActionData
                    {
                        ActionType = 2,
                        ActionMessage = "কাস্টমার আগ্রহী নয়",
                        UpdateStatus = 341,
                        StatusMessage = "Customer not interested",
                        ColorCode = ColorCode2
                    });
                    data.Add(new ActionData
                    {
                        ActionType = 2,
                        ActionMessage = "কাস্টমার হোল্ড করেছেন",
                        UpdateStatus = 360,
                        StatusMessage = "Product hold by delivery bondhu",
                        ColorCode = ColorCode4
                    });
                }
            }
            return data;
        }
        public List<ActionData> SetActionData(int StatusId, string orderId, int flag, string PaymentType)
        {
            var data = new List<ActionData>();
            //New Action Button
            if (orderId.ToLower().Contains("dt") == true)
            {
                if (StatusId == 40)
                {
                    data.Add(new ActionData
                    {
                        ActionType = 1,
                        ActionMessage = "অ্যাকসেপ্ট",
                        UpdateStatus = 41,
                        StatusMessage = "Delivery Accepted by Delivery Man",
                        ColorCode = ColorCode1
                    });

                    //data.Add(new ActionData
                    //{
                    //    ActionType = 2,
                    //    ActionMessage = "কাস্টমার আগ্রহী নয়",
                    //    UpdateStatus = 42,
                    //    StatusMessage = "Customer not interested",
                    //    ColorCode = ColorCode2
                    //});
                    //data.Add(new ActionData
                    //{
                    //    ActionType = 2,
                    //    ActionMessage = "মার্চেন্টকে পাওয়া যাচ্ছে না",
                    //    UpdateStatus = 43,
                    //    StatusMessage = "Merchant is not available",
                    //    ColorCode = ColorCode2
                    //});
                    //data.Add(new ActionData
                    //{
                    //    ActionType = 2,
                    //    ActionMessage = "মার্চেন্টের কাছে প্রোডাক্ট নাই",
                    //    UpdateStatus = 55,
                    //    StatusMessage = "Product is not available",
                    //    ColorCode = ColorCode4
                    //});
                }
                //else if (StatusId == 40 && flag == 1)
                //{
                //    data.Add(new ActionData
                //    {
                //        ActionType = 1,
                //        ActionMessage = "কালেক্ট করা হয়েছে",
                //        UpdateStatus = 44,
                //        StatusMessage = "Order has been collected from collection point",
                //        ColorCode = ColorCode1,
                //        CollectionPointAvailable = 1
                //    });
                //    data.Add(new ActionData
                //    {
                //        ActionType = 2,
                //        ActionMessage = "মার্চেন্টকে পাওয়া যাচ্ছে না",
                //        UpdateStatus = 43,
                //        StatusMessage = "Merchant is not available",
                //        ColorCode = ColorCode2
                //    });
                //    data.Add(new ActionData
                //    {
                //        ActionType = 2,
                //        ActionMessage = "মার্চেন্টের কাছে প্রোডাক্ট নাই",
                //        UpdateStatus = 55,
                //        StatusMessage = "Product is not available",
                //        ColorCode = ColorCode4
                //    });
                //}

            }


            else
            {
                if (StatusId == 555 )
                {
                    data.Add(new ActionData
                    {
                        ActionType = 1,
                        ActionMessage = "অ্যাকসেপ্ট",
                        UpdateStatus = 545,
                        StatusMessage = "Delivery Accepted by Delivery Man",
                        ColorCode = ColorCode1
                    });
                }
                if (StatusId == 333)
                {
                    data.Add(new ActionData
                    {
                        ActionType = 1,
                        ActionMessage = "অ্যাকসেপ্ট",
                        UpdateStatus = 334,
                        StatusMessage = "Delivery Accepted by Delivery Man",
                        ColorCode = ColorCode1
                    });
                }
                if (StatusId == 545 && flag==0)
                {

                    data.Add(new ActionData
                    {
                        ActionType = 2,
                        ActionMessage = "কাস্টমার আগ্রহী নয়",
                        UpdateStatus = 541,
                        StatusMessage = "The customer is not interested in taking delivery",
                        ColorCode = ColorCode2
                    });
                    data.Add(new ActionData
                    {
                        ActionType = 2,
                        ActionMessage = "অর্ডারটি বাতিল করুন",
                        UpdateStatus = 542,
                        StatusMessage = "Order Canceled By Delivery Bondhu",
                        ColorCode = ColorCode3
                    });
                    data.Add(new ActionData
                    {
                        ActionType = 2,
                        ActionMessage = "মার্চেন্টকে পাওয়া যাচ্ছে না",
                        UpdateStatus = 548,
                        StatusMessage = "Merchant is not available",
                        ColorCode = ColorCode2
                    });
                    data.Add(new ActionData
                    {
                        ActionType = 2,
                        ActionMessage = "মার্চেন্টের কাছে প্রোডাক্ট নাই",
                        UpdateStatus = 556,
                        StatusMessage = "Product is not available",
                        ColorCode = ColorCode4
                    });

                }
                if (StatusId == 545 && flag == 1)
                {
                    data.Add(new ActionData
                    {
                        ActionType = 1,
                        ActionMessage = "কালেক্ট করা হয়েছে",
                        UpdateStatus = 549,
                        StatusMessage = "The order has been collected from Bazar",
                        ColorCode = ColorCode1,
                        CollectionPointAvailable = 1
                    });
                    data.Add(new ActionData
                    {
                        ActionType = 2,
                        ActionMessage = "অর্ডারটি বাতিল করুন",
                        UpdateStatus = 542,
                        StatusMessage = "Order Canceled By Delivery Bondhu",
                        ColorCode = ColorCode3
                    });
                    data.Add(new ActionData
                    {
                        ActionType = 2,
                        ActionMessage = "মার্চেন্টকে পাওয়া যাচ্ছে না",
                        UpdateStatus = 548,
                        StatusMessage = "Merchant is not available",
                        ColorCode = ColorCode2
                    });
                    data.Add(new ActionData
                    {
                        ActionType = 2,
                        ActionMessage = "মার্চেন্টের কাছে প্রোডাক্ট নাই",
                        UpdateStatus = 556,
                        StatusMessage = "Product is not available",
                        ColorCode = ColorCode3
                    });

                }
                if (StatusId == 334 && flag==0)
                {
                    //data.Add(new ActionData
                    //{
                    //    ActionType = 1,
                    //    ActionMessage = "অ্যাকসেপ্ট",
                    //    UpdateStatus = 334,
                    //    StatusMessage = "Delivery Accepted by Delivery Man",
                    //    ColorCode = ColorCode1
                    //});
                    data.Add(new ActionData
                    {
                        ActionType = 2,
                        ActionMessage = "কাস্টমার আগ্রহী নয়",
                        UpdateStatus = 341,
                        StatusMessage = "Customer not interested",
                        ColorCode = ColorCode2
                    });
                    data.Add(new ActionData
                    {
                        ActionType = 2,
                        ActionMessage = "অর্ডারটি বাতিল করুন",
                        UpdateStatus = 342,
                        StatusMessage = "Order Canceled By Delivery Bondhu",
                        ColorCode = ColorCode3
                    });
                    data.Add(new ActionData
                    {
                        ActionType = 2,
                        ActionMessage = "মার্চেন্টকে পাওয়া যাচ্ছে না",
                        UpdateStatus = 338,
                        StatusMessage = "Merchant is not available",
                        ColorCode = ColorCode2
                    });
                    data.Add(new ActionData
                    {
                        ActionType = 2,
                        ActionMessage = "মার্চেন্টের কাছে প্রোডাক্ট নাই",
                        UpdateStatus = 356,
                        StatusMessage = "Product is not available",
                        ColorCode = ColorCode4
                    });
                }
                if (StatusId == 334 && flag == 1)
                {
                    data.Add(new ActionData
                    {
                        ActionType = 1,
                        ActionMessage = "কালেক্ট করা হয়েছে",
                        UpdateStatus = 335,
                        StatusMessage = "Order has been collected from collection point",
                        ColorCode = ColorCode1,
                        CollectionPointAvailable = 1
                    });
                    data.Add(new ActionData
                    {
                        ActionType = 2,
                        ActionMessage = "অর্ডারটি বাতিল করুন",
                        UpdateStatus = 342,
                        StatusMessage = "Order Canceled By Delivery Bondhu",
                        ColorCode = ColorCode3
                    });
                    data.Add(new ActionData
                    {
                        ActionType = 2,
                        ActionMessage = "মার্চেন্টকে পাওয়া যাচ্ছে না",
                        UpdateStatus = 338,
                        StatusMessage = "Merchant is not available",
                        ColorCode = ColorCode2
                    });
                    data.Add(new ActionData
                    {
                        ActionType = 2,
                        ActionMessage = "মার্চেন্টের কাছে প্রোডাক্ট নাই",
                        UpdateStatus = 356,
                        StatusMessage = "Product is not available ",
                        ColorCode = ColorCode4
                    });
                }
            }
            //New Action Button End

            if (orderId.ToLower().Contains("dt") == true)
            {
                if (StatusId == 41) // অ্যাকসেপ্ট
                {
                    data.Add(new ActionData
                    {
                        ActionType = 1,
                        ActionMessage = "কালেক্ট করা হয়েছে",
                        UpdateStatus = 44,
                        StatusMessage = "Order has been collected from collection point",
                        ColorCode = ColorCode1,
                        CollectionPointAvailable = 1
                    });
                    data.Add(new ActionData
                    {
                        ActionType = 2,
                        ActionMessage = "ক্যানসেল",
                        UpdateStatus = 40,
                        StatusMessage = "Order has been assigned collection point from rider",
                        ColorCode = ColorCode2
                    });
                    data.Add(new ActionData
                    {
                        ActionType = 2,
                        ActionMessage = "মার্চেন্টকে পাওয়া যাচ্ছে না",
                        UpdateStatus = 43,
                        StatusMessage = "Merchant is not available",
                        ColorCode = ColorCode2
                    });
                    data.Add(new ActionData
                    {
                        ActionType = 2,
                        ActionMessage = "মার্চেন্টের কাছে প্রোডাক্ট নাই",
                        UpdateStatus = 55,
                        StatusMessage = "Product is not available",
                        ColorCode = ColorCode4
                    });
                }

                else if (StatusId == 42) // কাস্টমার আগ্রহী নয়
                {
                    data.Add(new ActionData
                    {
                        ActionType = 1,
                        ActionMessage = "ডেলিভারি দেয়া হয়েছে",
                        UpdateStatus = 45,
                        StatusMessage = "Order Delivered by Delivery Man",
                        ColorCode = ColorCode3
                    });
                    data.Add(new ActionData
                    {
                        ActionType = 2,
                        ActionMessage = "মার্চেন্টকে রিটার্ন দেয়া হয়েছে",
                        UpdateStatus = 21,
                        StatusMessage = "Return order accepted by merchant",
                        ColorCode = ColorCode2
                    });
                }

                else if (StatusId == 44) // কালেক্ট করা হয়েছে
                {
                    data.Add(new ActionData
                    {
                        ActionType = 1,
                        ActionMessage = "ডেলিভারি দেয়া হয়েছে",
                        UpdateStatus = 45,
                        StatusMessage = "Order Delivered by Delivery Man",
                        ColorCode = ColorCode3
                    });
                    data.Add(new ActionData
                    {
                        ActionType = 2,
                        ActionMessage = "কাস্টমার আগ্রহী নয়",
                        UpdateStatus = 42,
                        StatusMessage = "Customer not interested",
                        ColorCode = ColorCode2
                    });
                    data.Add(new ActionData
                    {
                        ActionType = 2,
                        ActionMessage = "হোল্ড করুন",
                        UpdateStatus = 47,
                        StatusMessage = "Product hold by delivery bondhu",
                        ColorCode = ColorCode4
                    });
                }

                else if (StatusId == 43) //কালেক্ট করা সম্ভব নয়
                {
                    data.Add(new ActionData
                    {
                        ActionType = 1,
                        ActionMessage = "বাজার থেকে কালেক্ট করতে ইচ্ছুক",
                        UpdateStatus = 46,
                        StatusMessage = "Wanted to collect from the market",
                        ColorCode = ColorCode2
                    });
                    data.Add(new ActionData
                    {
                        ActionType = 2,
                        ActionMessage = "মার্চেন্টকে পাওয়া যাচ্ছে না",
                        UpdateStatus = 43,
                        StatusMessage = "Merchant is not available",
                        ColorCode = ColorCode2
                    });
                    data.Add(new ActionData
                    {
                        ActionType = 2,
                        ActionMessage = "মার্চেন্টের কাছে প্রোডাক্ট নাই",
                        UpdateStatus = 55,
                        StatusMessage = "Product is not available",
                        ColorCode = ColorCode4
                    });
                }
                else if (StatusId == 47)
                {
                    data.Add(new ActionData
                    {
                        ActionType = 1,
                        ActionMessage = "ডেলিভারি দেয়া হয়েছে",
                        UpdateStatus = 45,
                        StatusMessage = "Order Delivered by Delivery Man",
                        ColorCode = ColorCode3
                    });
                }
            }

            else
            {

            

                //if (StatusId==555)
                //{
                //    data.Add(new ActionData
                //    {
                //        ActionType = 1,
                //        ActionMessage = "অ্যাকসেপ্ট",
                //        UpdateStatus = 545,
                //        StatusMessage = "Delivery Accepted by Delivery Man",
                //        ColorCode = "#45BE59"
                //    });

                //}

            //if (StatusId == 545)
            //{
            //    data.Add(new ActionData
            //    {
            //        ActionType = 1,
            //        ActionMessage = "কালেক্ট করা হয়েছে",
            //        UpdateStatus = 549,
            //        StatusMessage = "The order has been collected from Bazar",
            //        ColorCode = ColorCode1,
            //        CollectionPointAvailable = 1
            //    });
            //    data.Add(new ActionData
            //    {
            //        ActionType = 2,
            //        ActionMessage = "মার্চেন্টকে পাওয়া যাচ্ছে না",
            //        UpdateStatus = 548,
            //        StatusMessage = "Merchant is not available",
            //        ColorCode = ColorCode2
            //    });
            //    data.Add(new ActionData
            //    {
            //        ActionType = 2,
            //        ActionMessage = "মার্চেন্টের কাছে প্রোডাক্ট নাই",
            //        UpdateStatus = 556,
            //        StatusMessage = "Product is not available",
            //        ColorCode = ColorCode4
            //    });

            //    }

            if (StatusId == 549)
            {
                data.Add(new ActionData
                {
                    ActionType = 1,
                    ActionMessage = "ডেলিভারি দেয়া হয়েছে",
                    UpdateStatus = 540,
                    StatusMessage = "Order Delivered by Delivery Man",
                    ColorCode = ColorCode3
                });
                data.Add(new ActionData
                {
                    ActionType = 2,
                    ActionMessage = "কাস্টমার আগ্রহী নয়",
                    UpdateStatus = 541,
                    StatusMessage = "The customer is not interested in taking delivery",
                    ColorCode = ColorCode2
                });

            }

            //if (StatusId == 333)
            //{
            //    data.Add(new ActionData
            //    {
            //        ActionType = 1,
            //        ActionMessage = "অ্যাকসেপ্ট",
            //        UpdateStatus = 334,
            //        StatusMessage = "Delivery Accepted by Delivery Man",
            //        ColorCode = "#45BE59"
            //    });
            //    data.Add(new ActionData
            //    {
            //        ActionType = 2,
            //        ActionMessage = "কাস্টমার আগ্রহী নয়",
            //        UpdateStatus = 341,
            //        StatusMessage = "Customer not interested",
            //        ColorCode = "#ECECEC"
            //    });
            //    data.Add(new ActionData
            //    {
            //        ActionType = 2,
            //        ActionMessage = "ষ্টক শেষ",
            //        UpdateStatus = 336,
            //        StatusMessage = "Stockout by the merchant",
            //        ColorCode = "#ECECEC"
            //    });

            //}


            //if (StatusId == 334)
            //{
            //    data.Add(new ActionData
            //    {
            //        ActionType = 1,
            //        ActionMessage = "কালেক্ট করা হয়েছে",
            //        UpdateStatus = 335,
            //        StatusMessage = "Order has been collected from collection point",
            //        ColorCode = ColorCode1,
            //        CollectionPointAvailable = 1
            //    });
            //    data.Add(new ActionData
            //    {
            //        ActionType = 2,
            //        ActionMessage = "মার্চেন্টকে পাওয়া যাচ্ছে না",
            //        UpdateStatus = 338,
            //        StatusMessage = "Merchant is not available",
            //        ColorCode = ColorCode2
            //    });
            //    data.Add(new ActionData
            //    {
            //        ActionType = 2,
            //        ActionMessage = "মার্চেন্টের কাছে প্রোডাক্ট নাই",
            //        UpdateStatus = 356,
            //        StatusMessage = "Product is not available",
            //        ColorCode = ColorCode4
            //    });


            //    }

            if (StatusId == 338)
            {
                data.Add(new ActionData
                {
                    ActionType = 1,
                    ActionMessage = "বাজার থেকে কালেক্ট করতে ইচ্ছুক",
                    UpdateStatus = 337,
                    StatusMessage = "Wanted to collect from the market",
                    ColorCode = ColorCode2
                });
                data.Add(new ActionData
                {
                    ActionType = 2,
                    ActionMessage = "মার্চেন্টকে পাওয়া যাচ্ছে না",
                    UpdateStatus = 338,
                    StatusMessage = "Merchant is not available",
                    ColorCode = ColorCode2
                });
                data.Add(new ActionData
                {
                    ActionType = 2,
                    ActionMessage = "মার্চেন্টের কাছে প্রোডাক্ট নাই",
                    UpdateStatus = 356,
                    StatusMessage = "Product is not available",
                    ColorCode = ColorCode4
                });


                }

            if (StatusId == 337)
            {
                data.Add(new ActionData
                {
                    ActionType = 1,
                    ActionMessage = "কালেক্ট করা হয়েছে",
                    UpdateStatus = 335,
                    StatusMessage = "Wanted to collect from the market",
                    ColorCode = ColorCode1,
                    CollectionPointAvailable = 1
                });
                data.Add(new ActionData
                {
                    ActionType = 2,
                    ActionMessage = "মার্চেন্টকে পাওয়া যাচ্ছে না",
                    UpdateStatus = 338,
                    StatusMessage = "Merchant is not available",
                    ColorCode = ColorCode2
                });
                data.Add(new ActionData
                {
                    ActionType = 2,
                    ActionMessage = "মার্চেন্টের কাছে প্রোডাক্ট নাই",
                    UpdateStatus = 356,
                    StatusMessage = "Product is not available ",
                    ColorCode = ColorCode4
                });

            }

            if (StatusId == 335)
            {
                data.Add(new ActionData
                {
                    ActionType = 1,
                    ActionMessage = "ডেলিভারি দেয়া হয়েছে",
                    UpdateStatus = 340,
                    StatusMessage = "Order Delivered by Delivery Man",
                    ColorCode = ColorCode3
                });
                data.Add(new ActionData
                {
                    ActionType = 2,
                    ActionMessage = "কাস্টমার আগ্রহী নয়",
                    UpdateStatus = 341,
                    StatusMessage = "Customer not interested",
                    ColorCode = ColorCode2
                });

            }

            if (StatusId == 45 && (PaymentType.Trim().ToLower() == "mpd" || PaymentType.Trim().ToLower() == "mpc"))
            {
                data.Add(new ActionData
                {
                    ActionType = 1,
                    ActionMessage = "পেমেন্ট করুন",
                    UpdateStatus = 46,
                    StatusMessage = "Payment Done",
                    ColorCode = ColorCode3,
                    Icon = "http://static.ajkerdeal.com/images/appimages/bkash.svg",
                    IsPaymentType = 1
                });

            }
            if (StatusId == 340 && (PaymentType.Trim().ToLower() == "mpd" || PaymentType.Trim().ToLower() == "mpc"))
            {
                data.Add(new ActionData
                {
                    ActionType = 1,
                    ActionMessage = "পেমেন্ট করুন",
                    UpdateStatus = 346,
                    StatusMessage = "Payment Done",
                    ColorCode = ColorCode3,
                    Icon = "http://static.ajkerdeal.com/images/appimages/bkash.svg",
                    IsPaymentType = 1
                });

            }
                if (StatusId == 540 && (PaymentType.Trim().ToLower() == "mpd" || PaymentType.Trim().ToLower() == "mpc"))
                {
                    data.Add(new ActionData
                    {
                        ActionType = 1,
                        ActionMessage = "পেমেন্ট করুন",
                        UpdateStatus = 546,
                        StatusMessage = "Payment Done",
                        ColorCode = ColorCode3,
                        Icon = "http://static.ajkerdeal.com/images/appimages/bkash.svg",
                        IsPaymentType = 1
                    });

                }
            }

            return data;
        }

        public SourceMessage SourceMessage(int StatusId, string PaymentType, string CollectionAddress, int CouponPrice, int SourceDealPrice, int DeliveryManCommission, int ProductQtn, string orderId, int DeliveryCharge)
        {

            SourceMessage model = new SourceMessage();
            string Commission = ToBanglaDigit(Convert.ToString(DeliveryManCommission));
            string PayAmountToAd = ToBanglaDigit(Convert.ToString(CouponPrice - DeliveryManCommission));
            string ProductCouponPrice = ToBanglaDigit(Convert.ToString((CouponPrice * ProductQtn)+DeliveryCharge));

            if (orderId.ToLower().Contains("dt") == true)
            {
                if (StatusId == 41)
                {
                    //model.Message = "<b>কালেকশন পয়েন্টঃ</b> " + CollectionAddress;
                    model.Status = "আপনি পার্সেলটি ডেলিভারির জন্য কালেক্ট করেছেন।";
                    if (PaymentType.ToLower() == "delivery taka collection")
                    {
                        model.IsPay = 1;
                    }
                    else
                    {
                        model.IsPay = 0;
                    }
                }
                else if (StatusId == 47)
                {
                    model.Status = "কাস্টমার হোল্ড করেছেন";
                }
                else if (StatusId == 42)
                {
                    model.Status = "কাস্টমার আগ্রহী নয়";
                }

                else if(StatusId == 44)
                {
                    
                    model.Status = "আপনি প্রোডাক্টটি ডেলিভারির জন্য কালেক্ট করেছেন।";
                    //if (PaymentType.ToLower() == "mpd" || PaymentType.ToLower() == "mpc")
                    //{
                    //    model.Message = "<b>পেমেন্ট গ্রহণ করুনঃ </b> <font color='#F15B2D'><b>" + ProductCouponPrice + " টাকা </b></font>"   /*"<br/><b>কালেকশন সময় পেমেন্টঃ</b> প্রয়োজন নেই"*/;
                    //    model.Instructions = "রিসিভ করা টাকা থেকে কমিশন <font color='#F15B2D'><b>" + ProductCouponPrice + " টাকা</b></font> পেমেন্ট হিসেবে রেখে বাকি <font color='#F15B2D'><b>" + PayAmountToAd + " টাকা</b></font> আজকেরডিল-কে বিকাশ করুন (বিকাশ নাম্বারঃ 01833319529)";
                    //}
                    //else if (PaymentType.ToLower() != "mpd" && PaymentType.ToLower() == "mpc")
                    //{
                    //    model.Instructions = "কাস্টমার থেকে পাওয়া পুরো <font color='#F15B2D'><b>" + ProductCouponPrice + " টাকা</b></font> আপনি রেখে দিন। <br/>এখান থেকে আপনি <font color='#F15B2D'><b>" + Commission + " টাকা</b></font> কমিশন আয় করেছেন!";
                    //}
                }
                else if (StatusId == 45)
                {
                    model.Status = "ডেলিভারি সম্পন্ন করা হয়েছে";
                }
                else if (StatusId == 15)
                {
                    model.Status = "<font color='#E86324'><b>প্রোডাক্টটি কাস্টমারকে ডেলিভারি করা হয়েছে</b></font>";
                }
                else if (StatusId == 49)
                {
                    model.Status = "<font color='#E86324'><b>রাইডার থেকে পেমেন্ট রিসিভড হয়েছে</b></font>";
                }
                //45,15,49
                else if (StatusId == 51)
                {
                    model.Status = "পার্সেল হাবে ডেলিভারি হয়েছে";
                }
                else if (StatusId == 52)
                {
                    model.Status = "<font color='#E86324'><b>পার্সেল হাবে রিসিভ হয়েছে</b></font>";
                }
                else if(StatusId == 55)
                {
                    model.Status = "মার্চেন্টের কাছে প্রোডাক্ট নাই";
                }
                else if (StatusId == 48)
                {
                    model.Status = "কাস্টমার হোল্ড প্রোডাক্ট এডমিন রিসিভড করেছে";
                }

            }
            else
            {

                if (StatusId== 334 || StatusId == 335)
                {
                    model.Status = "আপনি পার্সেলটি ডেলিভারির জন্য কালেক্ট করেছেন।";
                }
                else if (StatusId == 340)
                {
                    model.Status = "ডেলিভারি সম্পন্ন করা হয়েছে";
                }
                else if (StatusId == 356)
                {
                    model.Status = "মার্চেন্টের কাছে প্রোডাক্ট নাই";
                }
                else if (StatusId == 341)
                {
                    model.Status = "কাস্টমার আগ্রহী নয়";
                }
                else if (StatusId == 357)
                {
                    model.Status = "পার্সেল হাবে ডেলিভারি হয়েছে";
                }

                else if (StatusId == 360)
                {
                    model.Status = "কাস্টমার হোল্ড করেছেন";
                }

            }

            return model;
        }

        public int CalculateDeliveryManCommission(int statusId, string PaymentType, int DistrictId)
        {
            int defaultCommission = 50;
            int returnCommission = 0;
            if (PaymentType.ToLower() == "mpd")
            {
                if (DistrictId == 14)
                {
                    returnCommission = 60;
                }
                else
                {
                    returnCommission = 70;
                }
            }
            else
            {
                if (DistrictId == 14)
                {
                    returnCommission = 50;
                }
                else
                {
                    returnCommission = 55;
                }
            }
            return returnCommission;
        }
        public static string ToEnglishDigit(string banglaString)
        {
            if (string.IsNullOrEmpty(banglaString))
            {
                return null;
            }
            return banglaString.Replace('০', '0')
                .Replace('১', '1')
                .Replace('২', '2')
                .Replace('৩', '3')
                .Replace('৪', '4')
                .Replace('৫', '5')
                .Replace('৬', '6')
                .Replace('৭', '7')
                .Replace('৮', '8')
                .Replace('৯', '9');
        }

        public static string ToBanglaDigit(string englishString)
        {
            if (string.IsNullOrEmpty(englishString))
            {
                return null;
            }
            return englishString.Replace('0', '০')
                .Replace('1', '১')
                .Replace('2', '২')
                .Replace('3', '৩')
                .Replace('4', '৪')
                .Replace('5', '৫')
                .Replace('6', '৬')
                .Replace('7', '৭')
                .Replace('8', '৮')
                .Replace('9', '৯');
        }
    }
}