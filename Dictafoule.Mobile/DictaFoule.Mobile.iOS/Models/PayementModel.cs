using System;
namespace DictaFoule.Mobile.iOS.Models
{
    public class PayementModel
    {
        public string Token { get; set; }
        public decimal Amount { get; set; }
        public int IdProject { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
