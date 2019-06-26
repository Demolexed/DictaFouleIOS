using System;
using UIKit;

namespace DictaFoule.Mobile.iOS.Business
{
    public class Base
    {
        /// <summary>
        /// Unique phone Identifier 
        /// </summary>
        /// <value>The GUID.</value>
        protected string Guid { get; set; }

        public Base()
        {
            var nsUid = UIDevice.CurrentDevice.IdentifierForVendor;
            this.Guid = nsUid.AsString();
        }
    }
}
