using System;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Net.Client;
using Pcf.ReceivingFromPartner.Core.Abstractions.Gateways;
using Pcf.ReceivingFromPartner.Core.Domain;


namespace Pcf.ReceivingFromPartner.Integration
{
    public class GivingPromoCodeToCustomerGateway
        : IGivingPromoCodeToCustomerGateway
    {
        //private readonly PromoCodeService.PromoCodeServiceClient _client;
        public GivingPromoCodeToCustomerGateway(string grpcServerUrl)
        {
            //var channel = GrpcChannel.ForAddress(grpcServerUrl);
            //_client = new PromoCodeService.PromoCodeServiceClient(channel);
        }

        public async Task GivePromoCodeToCustomer(PromoCode promoCode)
        {
            //var request = new PromoCodeRequest()
            //{
            //    PartnerId = promoCode.Partner.Id,
            //    BeginDate = promoCode.BeginDate.ToShortDateString(),
            //    EndDate = promoCode.EndDate.ToShortDateString(),
            //    PreferenceId = promoCode.PreferenceId,
            //    PromoCode = promoCode.Code,
            //    ServiceInfo = promoCode.ServiceInfo,
            //    PartnerManagerId = promoCode.PartnerManagerId
            //};

            //try
            //{
            //    await _client.CreatePromoCodeAsync(request);

            //    Console.WriteLine("Promo code created successfully.");
            //}
            //catch (RpcException ex)
            //{
            //    Console.WriteLine($"gRPC error: {ex.Status.Detail}");
            //}
        }
    }
}