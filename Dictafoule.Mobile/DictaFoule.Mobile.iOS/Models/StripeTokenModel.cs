using System;
namespace DictaFoule.Mobile.iOS.Models
{
    public class StripeTokenModel
    {
        public string Name {get; set;}
        public string Number { get; set; }
        public int ExpirationYear { get; set; }
        public int ExpirationMonth { get; set; }
        public string Cvc { get; set; }
    }
}