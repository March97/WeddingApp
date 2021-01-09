using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;
using WeddingApp.API.Dtos;

namespace WeddingApp.API.Controllers
{
    [Route("create-checkout-session")]
    [ApiController]
    public class CheckoutApiController : Controller
    {
        [HttpPost]
        public ActionResult Create(PaymentDto paymentDto)
        {
            var domain = "http://localhost:4200/payment";
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string>
                {
                  "card",
                },
                LineItems = new List<SessionLineItemOptions>
                {
                  new SessionLineItemOptions
                  {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                      UnitAmount = paymentDto.cost * 100,
                      Currency = "pln",
                      ProductData = new SessionLineItemPriceDataProductDataOptions
                      {
                        Name = "Rezerwacja nr: " + paymentDto.id,
                      },
                    },
                    Quantity = 1,
                  },
                },
                Mode = "payment",
                SuccessUrl = domain + "?success=true&id=" + paymentDto.id,
                CancelUrl = "http://localhost:4200/reservations",
            };
            var service = new SessionService();
            Session session = service.Create(options);
            return Json(new { id = session.Id });
        }
    }
}