using Microsoft.AspNetCore.Mvc;
using Pcf.GivingToCustomer.Core.Domain;
using Pcf.GivingToCustomer.Core.Exceptions;
using Pcf.GivingToCustomer.Core.Services;
using Pcf.GivingToCustomer.WebHost.Mappers;
using Pcf.GivingToCustomer.WebHost.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pcf.GivingToCustomer.WebHost.Controllers
{
    /// <summary>
    /// Промокоды
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PromocodesController
        : ControllerBase
    {
        private readonly IPromoCodeService _promoCodeService;

        public PromocodesController(IPromoCodeService promoCodeService)
        {
            _promoCodeService = promoCodeService;
        }

        /// <summary>
        /// Получить все промокоды
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<List<PromoCodeShortResponse>>> GetPromocodesAsync()
        {
            var promocodes = await _promoCodeService.GetAllAsync();

            var response = promocodes.Select(x => new PromoCodeShortResponse()
            {
                Id = x.Id,
                Code = x.Code,
                BeginDate = x.BeginDate.ToString("yyyy-MM-dd"),
                EndDate = x.EndDate.ToString("yyyy-MM-dd"),
                PartnerId = x.PartnerId,
                ServiceInfo = x.ServiceInfo
            }).ToList();

            return Ok(response);
        }

        /// <summary>
        /// Создать промокод и выдать его клиентам с указанным предпочтением
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GivePromoCodesToCustomersWithPreferenceAsync(GivePromoCodeRequest request)
        {
            try
            {
                PromoCode promoCode = PromoCodeMapper.MapFromModel(request);
                await _promoCodeService.GivePromoCodeToCustomersWithPreferenceAsync(promoCode);

                return CreatedAtAction(nameof(GetPromocodesAsync), new { }, null);
            }
            catch (PreferenceNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}