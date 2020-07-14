using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyNIBSS.Core.Models;
using MyNIBSS.Data.Repositories;
using MyNIBSS.Logic;
using MyNIBSS.ViewModels;

namespace MyNIBSS.Controllers
{
    //[SessionRestrictLogic]
    public class CustomerAccountController : Controller
    {
        BaseRepository<CustomerAccount> baserepo = new BaseRepository<CustomerAccount>(new ApplicationDbContext());
        ApplicationDbContext _context = new ApplicationDbContext();
        CustomerAccountLogic customerAccountLogic = new CustomerAccountLogic();
        //FinancialReportLogic frLogic = new FinancialReportLogic();
        //BusinessLogic busLogic = new BusinessLogic();
        //LoanCustAcctRepository loanRepo = new LoanCustAcctRepository();
        //LoanCustAcctLogic loanLogic = new LoanCustAcctLogic();

        [AdminRoleRestrictLogic]
        //GET: CustomerAccount/Create
        public ActionResult Create()
        {

            return View();
        }

        //POST: CustomerAccount/Create
        //[AdminRoleRestrictLogic]
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "acctName,customerAccount,accType")] CustomerAccountViewModels model)
        //{
        //    //model.Branches = _context.Branches.ToList();
        //    //model.Customers = _context.Customers.ToList();
            
        //        if (model != null && model.id == 0 && model.accType!=0 && model.customerAccount.customerId!=0 && model.customerAccount.branchId!=0)
        //        {
        //            //Assigning the values gotten from the create form to the CustomerAccount model
        //            CustomerAccount customerAccount = new CustomerAccount();
        //            customerAccount.customerId = model.customerAccount.customerId;
        //            customerAccount.branchId = model.customerAccount.branchId;
        //            customerAccount.acctName = model.customerAccount.acctName;
        //            customerAccount.accType = model.accType.ToString();
        //            customerAccount.acctbalance = 0;
        //            customerAccount.createdAt = DateTime.Now;
        //            customerAccount.isLinked = false;
        //            customerAccount.status = "Opened";
        //            customerAccount.dailyInterestAccrued = 0;
        //            customerAccount.acctNumber = customerAccountLogic.GenerateAccountNumber(customerAccount.accType, customerAccount.customerId);
                    
        //            //Adding customerAccount info to memory
        //            _context.CustomerAccounts.Add(customerAccount);
        //            try
        //            {
        //                //Saving customerAccount info to the database
        //                _context.SaveChanges();
        //                TempData["Message"] = "Success";
        //                return RedirectToAction("Index", "CustomerAccount");
        //            }
        //            catch (DbEntityValidationException ex)
        //            {
        //                var errorMessages = ex.EntityValidationErrors
        //                    .SelectMany(x => x.ValidationErrors)
        //                    .Select(x => x.ErrorMessage);

        //                var fullErrorMessage = string.Join("; ", errorMessages);
        //                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

        //                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
        //            }
        //        }
                
        //    else
        //    {
        //        //ViewBag.Message = "Kindly fill all the fields";
        //        ViewBag.Message = "Empty";
        //        return View(model); /*new { message="Create not successful" });*/
        //        //return View("Create", "CustomerAccount", model);
        //    }


        //}
        
        //GET: CustomerAccount/Details/{id}
        [AdminRoleRestrictLogic]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            CustomerAccount customerAccount = baserepo.Get(id);
            if (customerAccount == null)
            {
                return HttpNotFound();
            }
            return View(customerAccount);
        }

        // GET: CustomerAccount/Edit/{id}
        [AdminRoleRestrictLogic]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            CustomerAccount customerAccount = _context.CustomerAccounts.Find(id);
            if (customerAccount == null)
            {
                return HttpNotFound();
            }

            CustomerAccountViewModels model = new CustomerAccountViewModels() { id = customerAccount.id/*, Branches = _context.Branches.ToList()*/ };

            return View(model);


        }

        //Post: CustomerAccount/Edit/{id}
        [AdminRoleRestrictLogic]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id")] CustomerAccountViewModels model)
        {
            CustomerAccount customerAccount = _context.CustomerAccounts.Find(model.id);
            //model.Branches = _context.Branches.ToList();
            //model.Customers = _context.Customers.ToList();

                try
                {

                    //Adding customerAccount info to memory
                    _context.Entry(customerAccount).State = EntityState.Modified;

                    //Updating customerAccount info to the database
                    _context.SaveChanges();
                TempData["Message"] = "Success";
                    return RedirectToAction("Index", "CustomerAccount");
                }
                catch (DbEntityValidationException ex)
                {
                    ViewBag.Message = "Error";

                    var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                    var fullErrorMessage = string.Join("; ", errorMessages);
                    var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                    throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);

                }
            

        }

        //GET: CustomerAccount/Close/{id}
        [AdminRoleRestrictLogic]
        [HttpGet]
        public ActionResult Close(int? id)
        {
            customerAccountLogic.Closed(id);
            return RedirectToAction("Index", "CustomerAccount");
        }

        //GET: CustomerAccount/Open/{id}
        [AdminRoleRestrictLogic]
        [HttpGet]
        public ActionResult Open(int? id)
        {
            customerAccountLogic.Opened(id);
            return RedirectToAction("Index","CustomerAccount");
        }


        //GET: CustomerAccount/Post/{id}
        //[TellerRoleRestrictLogic]
        [HttpGet]
        public ActionResult Post(int? id)
        {
            return RedirectToAction
                ("Create","TellerPosting",new { id });
        }

        // GET: CustomerAccount
        //[SessionRestrictLogic]
        public ActionResult Index()
        {
            var customerAccount = baserepo.GetAll();
            return View(customerAccount.ToList());
            //return View(_context.CustomerAccounts.ToList());
        }

        //GET: CustomerAccount/OpenLoanAccount
        [AdminRoleRestrictLogic]
        public ActionResult OpenLoanAccount(int? id)
        {
            TempData["message"] = "";
            return View();
        }

       
        

    }
}

    