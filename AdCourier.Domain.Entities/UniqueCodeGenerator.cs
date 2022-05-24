using System;
using System.Collections.Generic;
using System.Text;

namespace AdCourier.Domain.Entities
{
    public static class UniqueCodeGenerator
    {
        public static string GetUniqueCode(bool isCharaterLowerCaseInCouponCode,
            int minNumberForRandomNumberGenerator, int maxNumberForRandomNumberGenerator)
        {
            var couponCodeStringBuilder = new StringBuilder();
            var randomNumberGenerator = new Random(Guid.NewGuid().GetHashCode());
            couponCodeStringBuilder.Append(DateTime.Now.ToString("MMddyy"));
            if (isCharaterLowerCaseInCouponCode)
            {
                couponCodeStringBuilder.Append(Convert.ToChar(randomNumberGenerator.Next(97, 122)));
                couponCodeStringBuilder.Append(Convert.ToChar(randomNumberGenerator.Next(97, 122)));
            }
            else
            {
                couponCodeStringBuilder.Append(Convert.ToChar(randomNumberGenerator.Next(65, 90)));
                couponCodeStringBuilder.Append(Convert.ToChar(randomNumberGenerator.Next(65, 90)));
            }
            couponCodeStringBuilder.Append(
                randomNumberGenerator.Next(minNumberForRandomNumberGenerator, maxNumberForRandomNumberGenerator));
            return couponCodeStringBuilder.ToString();
        }
    }
}
