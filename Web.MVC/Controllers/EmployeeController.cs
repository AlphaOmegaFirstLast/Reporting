using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using Bussiness.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Reporting.data;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Web.MVC.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ReportContext _reportContext;

        public EmployeeController(ReportContext reportingContext)
        {
            _reportContext = reportingContext;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            var employees = _reportContext.Employees;
            return View(employees);
        }

        public IActionResult List()
        {
            var employees = _reportContext.Employees.Include(e => e.Department);
            return View(employees);
        }

        public ActionResult Create()
        {
            var Id = 0;
            ViewBag.Departments =
                _reportContext.Departments.Select(
                    d => new SelectListItem() { Text = d.Name, Value = d.Id.ToString() }).ToList();

            return View(new Employee());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Employee emp)
        {

            if (emp.BirthDate >= emp.GraduationDate)  //this should go to javascript validation
            {
                ModelState.AddModelError(string.Empty, "Graduation Date must be greater than Birthdate.");
            }
            if (ModelState.IsValid)  //this has nothing to do w the model Employee- it doesnt check its rules
            {
                _reportContext.Employees.Add(emp);
                try
                {
                    await _reportContext.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    ModelState.AddModelError(string.Empty, e.Message);
                    throw;
                }
            }

            return View(emp);
        }

        public async Task<ActionResult> Edit(int id)
        {
            var emp = await _reportContext.Employees.FirstOrDefaultAsync(e => e.Id == id);
            if (emp == null)
            {
                // throw new ArgumentNullException(nameof(emp));
                return this.NotFound();

            }
            GetDepartmentsList();
            return View(emp);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Update(int id, Employee emp)
        {
            try
            {
                emp.Id = id;

                _reportContext.Employees.Attach(emp);  // one query on DB (find and update)
                var entry = _reportContext.Entry(emp);
                entry.State = EntityState.Modified;  // all properties will be updated according to model's properties of the view. view must have all properties
                                                     // entry.Property(e=>e.BirthDate).IsModified = false; // in case u dont want to modify the field in Db
                                                     // or use update to replace these 3 lines if all properties are modified 
                await _reportContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (DbUpdateException e)
            {
                ModelState.AddModelError(string.Empty, "cannot update entity " + e.Message);
            }

            return View(emp);
        }

        public async Task<ActionResult> Delete(int id, bool? retry)  //optional parameter
        {
            var emp = await _reportContext.Employees.FirstOrDefaultAsync(e => e.Id == id);
            if (emp == null)
            {
                // throw new ArgumentNullException(nameof(emp));
                return this.NotFound();

            }
            ViewBag.Retry = retry ?? false;   // if null set it to a value otherwise it remains with the same value
            return View(emp);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var emp = await _reportContext.Employees.FirstOrDefaultAsync(e => e.Id == id);
                _reportContext.Employees.Remove(emp);
                await _reportContext.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                return RedirectToAction("Delete", new { id = id, retry = true });
            }
            return RedirectToAction("Index");

        }


        public void GetDepartmentsList()
        {
            ViewBag.Departments = _reportContext.Departments.Select(d => new SelectListItem() { Text = d.Name, Value = d.Id.ToString() }).ToList();
        }

 
    }
}
