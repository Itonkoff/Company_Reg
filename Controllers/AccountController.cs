using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Company_Reg.Constants;
using Company_Reg.Database;
using Company_Reg.Dtos;
using Company_Reg.Helpers;
using Company_Reg.Models;
using LinqToDB;
using Microsoft.AspNetCore.Mvc;

namespace Company_Reg.Controllers
{
    [Route("Payments")]
    public class AccountController : Controller
    {
        [HttpGet("{userid}", Name = "GetPayment")]
        public IActionResult GetPayments(string userid)
        {
            using(var db = new db())
            {
                var payments = (from payment in db.payments
                                where payment.UserId == userid
                                select payment).ToList();

                if(payments != null || payments.Count > 0)
                {
                    PaymentsHelper helper = new PaymentsHelper(payments);
                    double balance = helper.Balance;
                    PaymentsResponseDto response = new PaymentsResponseDto();
                    response.AccountBalance = balance;
                    response.Payments = payments;
                    return Ok(response);
                }
            }
            return NotFound();
        }


        [HttpGet("Credit/{creditId}", Name = "GetCredit")]
        public IActionResult GetCredit(string creditId)
        {
            using (var db = new db())
            {
                var credit = (from credi in db.credits
                              where credi.CreditId == creditId
                              select credi).FirstOrDefault();

                if (credit != null)
                {
                    return Ok(credit);
                }
            }
            return NotFound();
        }


        [HttpGet("Credits/{userid}")]
        public IActionResult GetCredits(string userid)
        {
            using(var db = new db())
            {
                ServiceConstants serviceConstants = new ServiceConstants();
                var credits = (from credi in db.credits
                               where credi.UserId == userid
                               && credi.ApplicationRef == null
                               select credi).ToList();

                CreditsDto creditCounts = new CreditsDto();

                if(credits != null || credits.Count > 0)
                {
                    foreach(var credit in credits)
                    {
                        if (credit.Service.Equals(serviceConstants.NAMESEARCH))
                        {
                            if (credit.ApplicationRef == null)
                                creditCounts.NameSearch++;
                        }

                        if (credit.Service.Equals(serviceConstants.PVTLIMITEDENTITY))
                        {
                            if (credit.ApplicationRef == null)
                                creditCounts.PvtLimitedCompany++;
                        }
                    }
                    return Ok(creditCounts);
                }
            }
            return NotFound();
        }



        [HttpPost("RecordPayment")]
        public IActionResult RecordPayment([FromBody]Payment payment)
        {
            using (var db = new db())
            {
                if (payment != null)
                {
                    payment.Date = DateTime.Now.ToString();
                    if (db.Insert(payment) == 1)
                        return Ok(payment);
                }
            }
            return BadRequest();
        }



        [HttpPost("PurchaseCredits")]
        public IActionResult PurchaseCredits([FromBody]CreditPurchaseDto purchaseDetails)
        {
            using(var db = new db())
            {
                var constants = new ServiceConstants();
                var payments = (from payment in db.payments 
                                where payment.UserId==purchaseDetails.UserID 
                                select payment).ToList();

                if(payments == null)
                {
                    return NotFound("You have no enough topup to fund this purchase");
                }

                PaymentsHelper helper = new PaymentsHelper(payments);

                if (helper.CanPurchaseNameSearch(purchaseDetails.NumberOfCredits))
                {
                    Payment payment = new Payment();
                    payment.UserId = purchaseDetails.UserID;
                    payment.PaymentId = Guid.NewGuid().ToString();
                    payment.Date = DateTime.Now.ToString();
                    payment.Description = purchaseDetails.Service + " Credit Purchase";
                    if (purchaseDetails.Service.Equals(constants.NAMESEARCH))
                        payment.AmountDr = helper.getTotalNameSearchPrice(purchaseDetails.NumberOfCredits);
                    if (purchaseDetails.Service.Equals(constants.PVTLIMITEDENTITY))
                        payment.AmountDr = helper.getTotalPvtPrice(purchaseDetails.NumberOfCredits);

                    List<Credit> credits = new List<Credit>();

                    if (db.Insert(payment) == 1){
                        for (var i = 0; i < purchaseDetails.NumberOfCredits; i++)
                        {
                            Credit credit = new Credit();
                            credit.UserId = purchaseDetails.UserID;
                            credit.CreditId = Guid.NewGuid().ToString();
                            credit.PaymentId = payment.PaymentId;
                            credit.Service = purchaseDetails.Service;
                            DateTime expDate = DateTime.Now.AddDays(30);
                            credit.ExpiryDate = expDate.ToString();
                            if(db.Insert(credit) == 1)
                            {
                                credits.Add(credit);
                                continue;
                            }
                            else
                            {
                                return BadRequest(payment);
                            }
                        }
                        payments.Add(payment);
                        CreditPurchaseResponseDto response = new CreditPurchaseResponseDto();
                        response.payment = payment;
                        response.credits = credits;
                        return Ok(response);
                    }
                }
            }
            return BadRequest("You have no enough topup to fund this purchase");
        }

        [HttpPost("UpdateCredit")]
        public IActionResult UpdateCredit([FromBody]UpdateCreditDto creditUpdate)
        {
            using(var db = new db())
            {
                ServiceConstants constants = new ServiceConstants();
                var credits = (from cred in db.credits
                              where cred.UserId == creditUpdate.UserId
                              && cred.Service == creditUpdate.Service
                              select cred).ToList();

                if (credits != null && credits.Count > 0)
                {
                    Credit credit = credits.FirstOrDefault();
                    credit.ApplicationRef = creditUpdate.ApplicationRef;
                    if(db.Update(credit) == 1)
                    {
                        if(creditUpdate.Service.Equals(constants.NAMESEARCH)) 
                        {
                            var searchInfor = (from searchInfo in db.SearchInfo
                                               where searchInfo.SearchRef == creditUpdate.ApplicationRef
                                               select searchInfo).FirstOrDefault();
                            searchInfor.Payment = "Paid";
                            if(db.Update(searchInfor) == 1)
                            {
                                return Ok(credit);
                            }
                        }

                        if (creditUpdate.Service.Equals(constants.PVTLIMITEDENTITY))
                        {
                            var companyInfo = (from coInfo in db.CompanyInfo
                                               where coInfo.Application_Ref == creditUpdate.ApplicationRef
                                               select coInfo).FirstOrDefault();
                            companyInfo.Payment = "Paid";
                            if (db.Update(companyInfo) == 1)
                            {
                                return Ok(credit);
                            }
                        }
                    }
                }
            }
            return NotFound();
        }
    }
}
