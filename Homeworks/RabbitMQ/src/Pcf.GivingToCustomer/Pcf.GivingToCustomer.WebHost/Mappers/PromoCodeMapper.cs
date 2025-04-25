using Pcf.GivingToCustomer.Core.Domain;
using Pcf.GivingToCustomer.WebHost.Models;
using System;
using System.Collections.Generic;

namespace Pcf.GivingToCustomer.WebHost.Mappers
{
    public class PromoCodeMapper
    {
        public static PromoCode MapFromModel(GivePromoCodeRequest request, Preference preference, IEnumerable<Customer> customers)
        {

            var promocode = new PromoCode();
            promocode.Id = request.PromoCodeId;

            promocode.PartnerId = request.PartnerId;
            promocode.Code = request.PromoCode;
            promocode.ServiceInfo = request.ServiceInfo;

            promocode.BeginDate = DateTime.Parse(request.BeginDate);
            promocode.EndDate = DateTime.Parse(request.EndDate);

            promocode.Preference = preference;
            promocode.PreferenceId = preference.Id;

            promocode.Customers = new List<PromoCodeCustomer>();

            foreach (var item in customers)
            {
                promocode.Customers.Add(new PromoCodeCustomer()
                {

                    CustomerId = item.Id,
                    Customer = item,
                    PromoCodeId = promocode.Id,
                    PromoCode = promocode
                });
            };

            return promocode;
        }

        public static PromoCode MapFromModel(GivePromoCodeRequest request)
            => new()
            {
                Id = request.PromoCodeId,
                PartnerId = request.PartnerId,
                Code = request.PromoCode,
                ServiceInfo = request.ServiceInfo,
                PreferenceId = request.PreferenceId
            };

        public static PromoCodeShortResponse MapToShortResponse(PromoCode entity)
            => new()
            {
                Id = entity.Id,
                Code = entity.Code,
                BeginDate = entity.BeginDate.ToString("yyyy-MM-dd"),
                EndDate = entity.EndDate.ToString("yyyy-MM-dd"),
                PartnerId = entity.PartnerId,
                ServiceInfo = entity.ServiceInfo
            };
    }
}
