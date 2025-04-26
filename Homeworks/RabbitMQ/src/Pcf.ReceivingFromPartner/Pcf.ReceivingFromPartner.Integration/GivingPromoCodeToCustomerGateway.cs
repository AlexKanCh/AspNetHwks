using System;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.Extensions.Options;
using Pcf.Grpc.Contracts;
using Pcf.ReceivingFromPartner.Core.Abstractions.Gateways;
using Pcf.ReceivingFromPartner.Core.Domain;
using Pcf.ReceivingFromPartner.Integration.Configuration;


namespace Pcf.ReceivingFromPartner.Integration
{
    public class GivingPromoCodeToCustomerGateway
        : IGivingPromoCodeToCustomerGateway
    {
        private readonly PromoCodeService.PromoCodeServiceClient _client;
        public GivingPromoCodeToCustomerGateway(IOptions<GrpcSettings> options)
        {
            var grpcServerUrl = options.Value.Url;
            var channel = GrpcChannel.ForAddress(grpcServerUrl);
            _client = new PromoCodeService.PromoCodeServiceClient(channel);
        }

        public async Task GivePromoCodeToCustomer(PromoCode promoCode)
        {
            var request = new PromoCodeRequest()
            {
                PartnerId = promoCode.Partner.Id.ToString(),
                BeginDate = new DateTimeOffset(promoCode.BeginDate).ToUnixTimeMilliseconds(),
                EndDate = new DateTimeOffset(promoCode.EndDate).ToUnixTimeMilliseconds(),
                PreferenceId = promoCode.PreferenceId.ToString(),
                Code = promoCode.Code,
                ServiceInfo = promoCode.ServiceInfo,
                PartnerManagerId = promoCode.PartnerManagerId.ToString()
            };

            try
            {
                await _client.CreatePromoCodeAsync(request);

                Console.WriteLine("Promo code created successfully.");
            }
            catch (RpcException ex)
            {
                Console.WriteLine($"gRPC error: {ex.Status.Detail}");
            }
        }
    }
}