using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Practical_20.Interfaces;
using Practical_20.Models;

namespace Practical_20.Controllers
{
	public class StudentController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IRepository<Student> _repository;

		public StudentController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
			_repository = _unitOfWork.GetRepository<Student>();
		}

		public async Task<IActionResult> Index()
		{
			return _repository.GetAll() != null ?
						View(_repository.GetAll().ToList()) :
						Problem("Entity set 'DatabaseContext.Students'  is null.");
		}

		public async Task<IActionResult> Details(int? id)
		{
			if (id == null || _repository.GetAll() == null)
			{
				return NotFound();
			}

			var student = _repository.GetById(id ?? 0);
			if (student == null)
			{
				return NotFound();
			}

			return View(student);
		}

		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("Id,Name,Email")] Student student)
		{
			if (ModelState.IsValid)
			{
				_repository.Insert(student);
				await _unitOfWork.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			return View(student);
		}

		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null || _repository.GetAll() == null)
			{
				return NotFound();
			}

			var student = _repository.GetById(id ?? 0);
			if (student == null)
			{
				return NotFound();
			}
			return View(student);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Email")] Student student)
		{
			if (id != student.Id)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_repository.Update(student);
					await _unitOfWork.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!StudentExists(student.Id))
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
			return View(student);
		}

		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null || _repository.GetAll() == null)
			{
				return NotFound();
			}

			var student = _repository.GetById(id ?? 0);

			if (student == null)
			{
				return NotFound();
			}

			return View(student);
		}

		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			if (_repository.GetAll() == null)
			{
				return Problem("Entity set 'DatabaseContext.Students'  is null.");
			}
			var student = _repository.GetById(id);
			if (student != null)
			{
				_repository.Delete(student);
			}

			await _unitOfWork.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		private bool StudentExists(int id)
		{
			return (_repository.GetAll()?.Any(e => e.Id == id)).GetValueOrDefault();
		}
	}
}
