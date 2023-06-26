using HSEPractice2.Areas.Identity.Data;
using HSEPractice2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using System.Net.NetworkInformation;
using static HSEPractice2.Constants;
using Microsoft.AspNetCore.Identity;
using OfficeOpenXml;

namespace HSEPractice2.Controllers
{
    public class TrainController : Controller
    {
        private readonly WaterparkContext _db;

        public TrainController(WaterparkContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index([FromServices] UserManager<ApplicationUser> userManager)
        {
            var currentUsername = User.Identity.Name;

            var currentUser = await userManager.FindByNameAsync(currentUsername);

            //Получить по фамилии айдишник сотрудника
            int userId = _db.Staff.FirstOrDefault(s => s.EmployeeSurname == currentUser.LastName
                                                  && s.EmployeeName == currentUser.FirstName
                                                  && s.EmployeePatronymic == currentUser.Patronymic).EmployeeId;


            bool hasAccess = User.IsInRole(Constants.Roles.Administrator);
            ViewBag.HasAccess = hasAccess;
            bool hasAccess2 = User.IsInRole(Constants.Roles.Instructor);
            ViewBag.HasAccess2 = hasAccess2;

            IEnumerable<Workout> objWorkoutList = _db.Workouts.OrderByDescending(s => s.DateOfWorkout).ToList();
            IEnumerable<CurrentWorkout> objCurList = _db.GetWorkouts().OrderByDescending(s => s.WorkoutDate).ToList();

            var data = new Tuple<IEnumerable<Workout>, IEnumerable<CurrentWorkout>, int>(objWorkoutList, objCurList, userId);

            return View(data);
        }

		[Authorize(Roles = $"{Constants.Roles.Administrator}")]
		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{Constants.Roles.Administrator}")]
        public IActionResult Create([Bind($"{nameof(Workout.WorkoutId)},{nameof(Workout.DateOfWorkout)}, {nameof(Workout.WorkoutType)}, {nameof(Workout.Duration)}, {nameof(Workout.CurrentWorkouts)}")] Workout obj)
        {
            // Преобразование локального времени в UTC
            obj.DateOfWorkout = obj.DateOfWorkout.ToUniversalTime();
            Console.WriteLine(obj);
            if (ModelState.IsValid)
            {
                _db.Workouts.Add(obj); // добавить в базу данных
                _db.SaveChanges();
                TempData["success"] = "Тренировка успешно создана!";
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        //GET ACTION METHOD
        [Authorize(Roles = $"{Constants.Roles.Administrator}")]
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var workoutFromDb = _db.Workouts.Find(id);

            if (workoutFromDb == null)
            {
                return NotFound();
            }
            return View(workoutFromDb);
        }

        // POST ACTION METHOD
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{Constants.Roles.Administrator}")]
        public IActionResult Edit([Bind($"{nameof(Workout.WorkoutId)},{nameof(Workout.DateOfWorkout)}, {nameof(Workout.WorkoutType)}, {nameof(Workout.Duration)}, {nameof(Workout.CurrentWorkouts)}")] Workout obj)
        {
            // Преобразование локального времени в UTC
            obj.DateOfWorkout = obj.DateOfWorkout.ToUniversalTime();
            Console.WriteLine(obj);
            if (ModelState.IsValid)
            {
                _db.Workouts.Update(obj); 
                _db.SaveChanges();
                TempData["success"] = "Тренировка успешно изменена!";
                return RedirectToAction("Index");
            }
            return View(obj);
        }

		// GET ACTION METHOD
		[HttpGet]
		[Authorize(Roles = $"{Constants.Roles.Administrator}")]
		public IActionResult Delete(int? id)
		{
			if (id == null || id == 0)
			{
				return NotFound();
			}
			var workoutFromDb = _db.Workouts.Find(id);

			if (workoutFromDb == null)
			{
				return NotFound();
			}
			return View(workoutFromDb);
		}

		// POST ACTION METHOD
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = $"{Constants.Roles.Administrator}")]
		public IActionResult Delete(Workout obj)
		{
			var workoutFromDb = _db.Workouts.Find(obj.WorkoutId);

			if (workoutFromDb == null)
			{
				return NotFound();
			}
			_db.Workouts.Remove(workoutFromDb);
			_db.SaveChanges();
			TempData["success"] = "Тренировка успешно удалена!";
			return RedirectToAction("Index");
		}

		[Authorize(Roles = $"{Constants.Roles.Instructor}, {Constants.Roles.Administrator}")]
		public IActionResult CreateCur()
        {
            ViewBag.Trains = new SelectList(_db.Workouts.OrderByDescending(s => s.WorkoutId), "WorkoutId", "WorkoutId");
            var cursh = new CurrentWorkout();
            cursh.WorkoutDateNavigation = new Workout();
            return View(cursh);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{Constants.Roles.Instructor}, {Constants.Roles.Administrator}")]
        public async Task<IActionResult> CreateCurAsync(
        [Bind($"{nameof(CurrentWorkout.CurrentWorkoutId)},{nameof(CurrentWorkout.WorkoutDate)}, {nameof(CurrentWorkout.Trainer)}, {nameof(CurrentWorkout.WorkoutDateNavigation)}, {nameof(CurrentWorkout.TrainerNavigation)}")] CurrentWorkout obj,
        [FromServices] UserManager<ApplicationUser> userManager)
        {
            if (ModelState.IsValid)
            {
                var currentUsername = User.Identity.Name;

                var currentUser = await userManager.FindByNameAsync(currentUsername);

                //Получить по фамилии айдишник сотрудника
                int i = _db.Staff.FirstOrDefault(s => s.EmployeeSurname == currentUser.LastName && s.EmployeeName == currentUser.FirstName).EmployeeId;
                obj.Trainer = i;

                //Проверить есть ли уже такая запись в базе данных
                if (_db.GetWorkouts().Any(s => s.WorkoutDate == obj.WorkoutDate && s.Trainer == obj.Trainer))
                {
                    ModelState.AddModelError("WorkoutDate", "Запись о данной тренировке для данного сотрудника уже существует.");
                    TempData["error"] = "Текущая тренировка НЕ создана!";
                    ViewBag.Trains = new SelectList(_db.Workouts.OrderByDescending(s => s.WorkoutId), "WorkoutId", "WorkoutId");
                    var workout = new CurrentWorkout();
                    return View(workout);
                }

                _db.CurrentWorkouts.Add(obj); // добавить в базу данных
                _db.SaveChanges();
                TempData["success"] = "Текущая тренировка успешно создана!";
                return RedirectToAction("Index");
            }
            return View(obj);
        }


		//GET ACTION METHOD
		[Authorize(Roles = $"{Constants.Roles.Instructor}, {Constants.Roles.Administrator}")]
		public IActionResult EditCur(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var curworkFromDb = _db.GetWorkouts().Find(s => s.CurrentWorkoutId == id);
            ViewBag.Trains = new SelectList(_db.Workouts.OrderByDescending(s => s.WorkoutId), "WorkoutId", "WorkoutId");

            if (curworkFromDb == null)
            {
                return NotFound();
            }
            return View(curworkFromDb);
        }

        
        // POST ACTION METHOD
        [HttpPost]
        [ValidateAntiForgeryToken]
		[Authorize(Roles = $"{Constants.Roles.Instructor}, {Constants.Roles.Administrator}")]
		public async Task<IActionResult> EditCurAsync(
        [Bind($"{nameof(CurrentWorkout.CurrentWorkoutId)},{nameof(CurrentWorkout.WorkoutDate)}, {nameof(CurrentWorkout.Trainer)}, {nameof(CurrentWorkout.WorkoutDateNavigation)}, {nameof(CurrentWorkout.TrainerNavigation)}")] CurrentWorkout obj,
        [FromServices] UserManager<ApplicationUser> userManager)
        {
			if (ModelState.IsValid)
			{
				//Проверить есть ли уже такая запись в базе данных
				if (_db.GetWorkouts().Any(s => s.Trainer == obj.Trainer && s.WorkoutDate == obj.WorkoutDate && s.CurrentWorkoutId != obj.CurrentWorkoutId))
				{
					ModelState.AddModelError("WorkoutDate", "Запись о данной тренировке для данного сотрудника уже существует.");
					TempData["error"] = "Текущая тренировка НЕ изменена!";
					ViewBag.Trains = new SelectList(_db.Workouts.OrderByDescending(s => s.WorkoutId), "WorkoutId", "WorkoutId");
					var workout = new CurrentWorkout();
					return View(workout);
				}

				var existingEntity = _db.CurrentWorkouts.Find(obj.CurrentWorkoutId);
				if (existingEntity != null)
				{
					// Обновляем существующую сущность новыми значениями
					existingEntity.WorkoutDate = obj.WorkoutDate;
					existingEntity.Trainer = obj.Trainer;

					_db.SaveChanges();
					TempData["success"] = "Текущая тренировка успешно изменена!";
					return RedirectToAction("Index");
				}
				else
				{
					ModelState.AddModelError("", "Запись не найдена.");
					return View(obj);
				}
			}
			return View(obj);
		}


		// GET ACTION METHOD
		[HttpGet]
        [Authorize(Roles = $"{Constants.Roles.Instructor}, {Constants.Roles.Administrator}")]
        public IActionResult DeleteCur(int? id)
		{
			List<CurrentWorkout> curWList = _db.GetWorkouts().OrderBy(s => s.WorkoutDateNavigation.DateOfWorkout).ToList();
			if (id == null || id == 0)
			{
				return NotFound();
			}
			var curwFromDb = curWList.Find(s => s.CurrentWorkoutId == id);

			if (curwFromDb == null)
			{
				return NotFound();
			}
			return View(curwFromDb);
		}


		// POST ACTION METHOD
		[HttpPost]
		[ValidateAntiForgeryToken]
        [Authorize(Roles = $"{Constants.Roles.Instructor}, {Constants.Roles.Administrator}")]
        public IActionResult DeleteCur(CurrentWorkout obj)
		{
			var curwFromDb = _db.CurrentWorkouts.Find(obj.CurrentWorkoutId);

			if (curwFromDb == null)
			{
				return NotFound();
			}
			_db.CurrentWorkouts.Remove(curwFromDb); // удалить из базы данных
			_db.SaveChanges();
			TempData["success"] = "Тренировка успешно удалена!";
			return RedirectToAction("Index");
		}
	}
}
