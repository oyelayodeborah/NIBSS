using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyNIBSS.Core.Models;
using MyNIBSS.Data.Repositories;

namespace MyNIBSS.Controllers
{
    public class FinancialInstitutionController : Controller
    {
        FinancialInstitutionRepository Repo = new FinancialInstitutionRepository();


        public ActionResult Add()
        {
            //var _context = new ApplicationDbContext();
            //FinancialInstitution model = new FinancialInstitution();
            //model.SinkNodes = _context.SinkNodes.ToList();
            //if (model.SinkNodes.Count() == 0)
            //{
            //    model.SinkNodes = new List<SinkNode>();
            //}

            //return View(model);
            return View();
        }

        [HttpPost]
        public ActionResult Add(FinancialInstitution model)
        {
            try
            {
                //check uniqueness of name and code
                if (!(Repo.isUniqueCode(model.InstitutionCode)))
                {
                    ViewBag.Message = "FinancialInstitution's name must be unique";
                    if (!(Repo.isUniqueName(model.Name)))
                    {
                        ViewBag.Message = "FinancialInstitution already exist";

                    }
                    return View();
                }

                model.DateCreated = DateTime.Now;
                model.DateModified = DateTime.Now;
                Repo.Save(model);
                return RedirectToAction("Index", new { message = "Successfully added FinancialInstitution!" });
            }
            catch (Exception ex)
            {
                return View(new { message = "FinancialInstitution was not successful" });
            }
        }
        public ActionResult Edit(int? id)
        {
            var _context = new ApplicationDbContext();
            if (id == null)
            {
                return HttpNotFound();
            }

            FinancialInstitution financialinstitution = _context.FinancialInstitutions.Find(id);
            if (financialinstitution == null)
            {
                return HttpNotFound();
            }
            return View(financialinstitution);


        }
        
        [HttpPost]
        public ActionResult Edit(FinancialInstitution model)
        {
            var _context = new ApplicationDbContext();
            model.DateModified = DateTime.Now;

            try
            {

                var newModel = Repo.Get(model.Id);

                //check uniqueness of name and code
                if (!(Repo.isUniqueCode(newModel.InstitutionCode, model.InstitutionCode)))
                {
                    ViewBag.Message = "FinancialInstitution's name must be unique";
                    if (!(Repo.isUniqueName(newModel.Name, model.Name)))
                    {
                        ViewBag.Message = "FinancialInstitution's name must be unique";
                        return View();
                    }
                    return View();
                }
                newModel.Name = model.Name;
                newModel.InstitutionCode = model.InstitutionCode;
                newModel.Port = model.Port;
                newModel.DateModified = model.DateModified;
                Repo.Update(newModel);
                ViewBag.Message = "Updated";
                return RedirectToAction("Index", new { message = "FinancialInstitution was successfully updated" });
            }
            catch (Exception ex)
            {
                //ErrorLogger.Log("Message= " + ex.Message + "\nInner Exception= " + ex.InnerException + "\n");
                return View(new { message = "Error updating FinancialInstitution" });
            }
        }

        public ActionResult Index()
        {
            var getList = Repo.GetAll();
            return View(getList == null ? new List<FinancialInstitution>() : getList);
        }
    }
}