using CRUD_MVC.Data;
using CRUD_MVC.Models.Domain;
using CRUD_MVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRUD_MVC.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly MVCDbContext _mvcDbContext;
        public EmployeeController(MVCDbContext mvcDbContext)
        {
            _mvcDbContext = mvcDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var employees = await _mvcDbContext.Employees.ToListAsync();

            return View(employees);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddEmployeeViewModel addEmployeeRequest)
        {
            var new_employee = new Employee()
            {
                Id = Guid.NewGuid(),
                Name = addEmployeeRequest.Name,
                Email = addEmployeeRequest.Email,
                DateOfBirth = addEmployeeRequest.DateOfBirth,
                Salary = addEmployeeRequest.Salary,
                Department = addEmployeeRequest.Department
            };

            await _mvcDbContext.Employees.AddAsync(new_employee);
            await _mvcDbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> View(Guid Id) 
        {
            var employee = await _mvcDbContext.Employees.FirstOrDefaultAsync(x => x.Id == Id);

            if (employee != null)
            {
                var viewModel = new EditViewModel
                {
                    Id = employee.Id,
                    Name = employee.Name,
                    Email = employee.Email,
                    DateOfBirth = employee.DateOfBirth,
                    Salary = employee.Salary,
                    Department = employee.Department
                };

                return await Task.Run(() => View("View", viewModel));
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> View(EditViewModel editViewModel)
        {
            var employee = await _mvcDbContext.Employees.FindAsync(editViewModel.Id);

            if (employee != null)
            {
                employee.Name = editViewModel.Name;
                employee.Email = editViewModel.Email;
                employee.Salary = editViewModel.Salary;
                employee.DateOfBirth = editViewModel.DateOfBirth; 
                employee.Department = editViewModel.Department;

                await _mvcDbContext.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(EditViewModel editViewModel)
        {
            var employee = await _mvcDbContext.Employees.FindAsync(editViewModel.Id);

            if(employee != null)
            {
                _mvcDbContext.Employees.Remove(employee);  

                await _mvcDbContext.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }
    }
}
