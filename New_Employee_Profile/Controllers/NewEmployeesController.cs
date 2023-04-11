using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using New_Employee_Profile.Context;
using New_Employee_Profile.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using System.Drawing.Imaging;

namespace New_Employee_Profile.Views
{
    public class NewEmployeesController : Controller
    {
        private readonly DataContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public NewEmployeesController(DataContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: NewEmployees
        public async Task<IActionResult> Index(int p = 1)
        {
            int pageSize = 3;
            ViewBag.PageNumber = p;
            ViewBag.PageRange = pageSize;
            ViewBag.TotalPages = (int)Math.Ceiling((decimal)_context.NewEmployees.Count() / pageSize);

            return View(await _context.NewEmployees.OrderByDescending(p => p.Id)
                                                                            .Include(p => p.Department)
                                                                            .Include(p => p.EmpType)
                                                                            .Include(p => p.Role)
                                                                            .Include(p => p.Section)
                                                                            .Skip((p - 1) * pageSize)
                                                                            .Take(pageSize)
                                                                            .ToListAsync());
            //stock code 
            //return _context.NewEmployees != null ? 
            //              View(await _context.NewEmployees.ToListAsync()) :
            //              Problem("Entity set 'DataContext.NewEmployees'  is null.");
        }

        // GET: NewEmployees/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null || _context.NewEmployees == null)
            {
                return NotFound();
            }

            var newEmployee = await _context.NewEmployees
                .Include(n => n.Department)
                .Include(n => n.EmpType)
                .Include(n => n.Role)
                .Include(n => n.Section)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (newEmployee == null)
            {
                return NotFound();
            }

            return View(newEmployee);
        }


        // GET: NewEmployees/Profile/5
        public async Task<IActionResult> Profile(long? id)
        {
            if (id == null || _context.NewEmployees == null)
            {
                return NotFound();
            }

            var newEmployee = await _context.NewEmployees
                .Include(n => n.Department)
                .Include(n => n.EmpType)
                .Include(n => n.Role)
                .Include(n => n.Section)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (newEmployee == null)
            {
                return NotFound();
            }

            return View(newEmployee);
        }



       



        // GET: NewEmployees/Create
        public IActionResult Create()
        {
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name");
            ViewData["EmpTypeId"] = new SelectList(_context.EmpTypes, "Id", "Name");
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Name");
            ViewData["SectionId"] = new SelectList(_context.Sections, "Id", "Name");
            return View();
        }

        // POST: NewEmployees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( NewEmployee newEmployee)
        {
            //if (ModelState.IsValid)
            //{
            //    _context.Add(newEmployee);
            //    await _context.SaveChangesAsync();
            //    return RedirectToAction(nameof(Index));
            //}
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name", newEmployee.DepartmentId);
            ViewData["EmpTypeId"] = new SelectList(_context.EmpTypes, "Id", "Name", newEmployee.EmpTypeId);
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Name", newEmployee.RoleId);
            ViewData["SectionId"] = new SelectList(_context.Sections, "Id", "Name", newEmployee.SectionId);

            if (ModelState.IsValid)
            {
                newEmployee.Slug = newEmployee.FName.ToLower().Replace(" ", "-") +"-"+ newEmployee.LName.ToLower().Replace(" ", "-");

                var slug = await _context.NewEmployees.FirstOrDefaultAsync(p => p.Slug == newEmployee.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "The newEmployee already exists.");
                    return View(newEmployee);
                }

                if (newEmployee.ImageUpload != null)
                {
                    string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/newEmployees");
                    //string imageName = Guid.NewGuid().ToString() + "_" + newEmployee.ImageUpload.FileName;
                    string ImgExtension = Path.GetExtension(newEmployee.ImageUpload.FileName);
                    string imageName = newEmployee.FName + " " + newEmployee.LName + ImgExtension;

                    string filePath = Path.Combine(uploadsDir, imageName);

                    //string ImagePath = Path.Combine(uploadsDir, imageName);

                    FileStream fs = new FileStream(filePath, FileMode.Create);
                    await newEmployee.ImageUpload.CopyToAsync(fs);
                    fs.Close();

                    newEmployee.ImageName = imageName;
                    //newEmployee.ImageURL = "~/wwwroot/media/newEmployees/" + imageName ;
    }

                _context.Add(newEmployee);
                await _context.SaveChangesAsync();

                TempData["Success"] = "The New Employee has been created!";

                return RedirectToAction("Index");

            }


            return View(newEmployee);
        }

        // GET: NewEmployees/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null || _context.NewEmployees == null)
            {
                return NotFound();
            }

            var newEmployee = await _context.NewEmployees.FindAsync(id);
            if (newEmployee == null)
            {
                return NotFound();
            }


            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name", newEmployee.DepartmentId);
            ViewData["EmpTypeId"] = new SelectList(_context.EmpTypes, "Id", "Name", newEmployee.EmpTypeId);
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Name", newEmployee.RoleId);
            ViewData["SectionId"] = new SelectList(_context.Sections, "Id", "Name", newEmployee.SectionId);
            return View(newEmployee);
        }

        // POST: NewEmployees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, NewEmployee newEmployee)
        {
            if (id != newEmployee.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                newEmployee.Slug = newEmployee.FName.ToLower().Replace(" ", "-") + "-" + newEmployee.LName.ToLower().Replace(" ", "-");

                var slug = await _context.NewEmployees.FirstOrDefaultAsync(p => p.Slug == newEmployee.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "The newEmployee already exists.");
                    return View(newEmployee);
                }

                if (newEmployee.ImageUpload != null)
                {
                    string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/newEmployees");
                    //string imageName = Guid.NewGuid().ToString() + "_" + newEmployee.ImageUpload.FileName;
                    string ImgExtension = Path.GetExtension(newEmployee.ImageUpload.FileName);
                    string imageName = newEmployee.FName + " " + newEmployee.LName + ImgExtension;

                    string filePath = Path.Combine(uploadsDir, imageName);

                    //string ImagePath = Path.Combine(uploadsDir, imageName);

                    FileStream fs = new FileStream(filePath, FileMode.Create);
                    await newEmployee.ImageUpload.CopyToAsync(fs);
                    fs.Close();

                    newEmployee.ImageName = imageName;
                    //newEmployee.ImageURL = "~/wwwroot/media/newEmployees/" + imageName;
                }

                try
                {
                    _context.Update(newEmployee);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "The New Employee has been edited!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NewEmployeeExists(newEmployee.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name", newEmployee.DepartmentId);
            ViewData["EmpTypeId"] = new SelectList(_context.EmpTypes, "Id", "Name", newEmployee.EmpTypeId);
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Name", newEmployee.RoleId);
            ViewData["SectionId"] = new SelectList(_context.Sections, "Id", "Name", newEmployee.SectionId);
            return View(newEmployee);
        }

        // GET: NewEmployees/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null || _context.NewEmployees == null)
            {
                return NotFound();
            }

            var newEmployee = await _context.NewEmployees
                .Include(n => n.Department)
                .Include(n => n.EmpType)
                .Include(n => n.Role)
                .Include(n => n.Section)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (newEmployee == null)
            {
                return NotFound();
            }

            return View(newEmployee);
        }

        // POST: NewEmployees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            if (_context.NewEmployees == null)
            {
                return Problem("Entity set 'DataContext.NewEmployees'  is null.");
            }
            var newEmployee = await _context.NewEmployees.FindAsync(id);
            if (newEmployee != null)
            {
                _context.NewEmployees.Remove(newEmployee);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NewEmployeeExists(long id)
        {
          return (_context.NewEmployees?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
