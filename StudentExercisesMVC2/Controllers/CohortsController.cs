using System;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using StudentExercisesMVC2.Models;
using StudentExercisesMVC2.Repositories;

namespace StudentExercisesMVC2.Controllers
{
    public class CohortsController : Controller
    {
        private readonly IConfiguration _config;

        public CohortsController(IConfiguration config)
        {
            _config = config;
        }

        public SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }
        // GET: Cohorts
        public ActionResult Index()
        {

            var cohorts = CohortRepository.GetCohorts();
            return View(cohorts);
        }

        // GET: Cohorts/Details/5
        public ActionResult Details(int id)
        {
            //encapsulation StudentRepository.GetStudent
            var cohort = CohortRepository.GetCohort(id);
            return View(cohort);
        }

        // GET: Cohorts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Cohorts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Cohort cohort, IFormCollection collection)
        {
            CohortRepository.CreateCohort(cohort);
            return RedirectToAction(nameof(Index));
        }

        // GET: Cohorts/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Cohorts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Cohorts/Delete/5

        public ActionResult DeleteConfirm(int id)
        {
            var cohort = CohortRepository.GetCohort(id);
            return View(cohort);
        }

        // POST: Cohorts/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}