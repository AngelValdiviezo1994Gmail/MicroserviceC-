using Common.Domain.Entities;
using Common.Domain.Enums;
using Common.Domain.Interfaces;
using Common.Domain.ViewModel;
using Common.Domain.Wrappers;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Module.Academico.Domain.Entities;
using Module.Academico.Infrastructure.Interfaces.Client;
using Newtonsoft.Json;
using NuGet.Protocol;
using System.Web.WebPages;
using WebAPP.Extensions;
using WebAPP.Filters;
using WebAPP.Helpers;
using WebAPP.ViewModel;

namespace Microservice.Client.Controllers.Catalogos
{
    public class GeneroController : Controller
    {
        // GET: GeneroController
        public ActionResult Index()
        {
            return View();
        }

        // GET: GeneroController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: GeneroController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: GeneroController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: GeneroController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: GeneroController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: GeneroController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: GeneroController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
