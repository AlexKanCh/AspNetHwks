using System;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Pcf.Grpc.Contracts;

namespace Pcf.GivingToCustomer.Integration.Service;

public class PromoCodeGrpcService : PromoCodeService.PromoCodeServiceBase
{   
    public override Task<Empty> CreatePromoCode(PromoCodeRequest request, ServerCallContext context)
    {
        Console.WriteLine($"Creating promo code: {request.Code}");
        Console.WriteLine($"Partner ID: {request.PartnerId}, Preference ID: {request.PreferenceId}");
        //Вызов доходит, сюда надо перенести обработку - перенести в сервис и вызывать,
        //это сделано в другом заданни, 2 раза делать не хочется.


        return Task.FromResult(new Empty());
    }
}