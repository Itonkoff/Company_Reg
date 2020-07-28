using Company_Reg.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Company_Reg.Helpers
{
    public class PaymentsHelper
    {
        private readonly List<Payment> payments;
        public double Balance;
        double NameSearchPrice = 80.00;
        double PvtEntityPrice = 200.00;

        public PaymentsHelper(List<Payment> payments)
        {
            this.payments = payments;
            CalculateBalance();
        }

        private void CalculateBalance()
        {
            if (payments.Count > 0)
            {
                var sumation = 0.0;
                var subtraction = 0.0;

                foreach (var payments in payments)
                {
                    sumation += payments.AmountCr;
                    subtraction += payments.AmountDr;
                }
                Balance =  sumation - subtraction;
            }
        }

        public bool CanPurchaseNameSearch(int creditsRequested)
        {
            return Balance >= (creditsRequested * NameSearchPrice); 
        }

        public bool CanPurchasePvtEntitySearch(int creditsRequested)
        {
            return Balance >= (creditsRequested * PvtEntityPrice);
        }

        public double getTotalNameSearchPrice(int creditsRequested)
        {
            return creditsRequested * NameSearchPrice;
        }

        public double getTotalPvtPrice(int creditsRequested)
        {
            return creditsRequested * PvtEntityPrice;
        }

    }
}
