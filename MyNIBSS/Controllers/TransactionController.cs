using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MyNIBSS.Core.Models;
using MyNIBSS.Data.Repositories;

namespace MyNIBSS.Controllers
{
    public class TransactionController:Controller
    {
        TransactionRepository Repo = new TransactionRepository();
        public ActionResult Index()
        {
            var getList = Repo.GetAll();
            return View(getList.Count() == 0 ? new List<Transaction>() : getList);
        }
    }
}