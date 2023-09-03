using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CreditcardApi.Models
{
    public class CreditCard
    {
        public int paymentdetailid { get; set; }
        public string cardownername { get; set; }
        public string cardnumber { get; set; }
        public string expirationdate { get; set; }
        public string securitycode { get; set; }
    }
}