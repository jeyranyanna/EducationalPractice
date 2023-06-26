using HSEPractice2.Areas.Identity.Data;
using HSEPractice2.Areas.Identity.Pages.Account.Manage;
using HSEPractice2.Core.Repositories;
using HSEPractice2.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using static HSEPractice2.Constants;

namespace HSEPractice2.Controllers
{
    public class UserController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public UserController(IUnitOfWork unitOfWork, SignInManager<ApplicationUser> signInManager)
        {
            _unitOfWork = unitOfWork;
            _signInManager = signInManager;
        }

        [Authorize(Roles = $"{Constants.Roles.Administrator}")]
        public IActionResult Index()
        {
            var users = _unitOfWork.User.GetUsers().OrderBy(u => u.LastName).ToList(); 
            return View(users);
        }


        [Authorize(Roles = $"{Constants.Roles.Administrator}")]
        public async Task<IActionResult> Edit(string id)
        {
            var user = _unitOfWork.User.GetUser(id);
            var roles = _unitOfWork.Role.GetRoles();

            var userRoles = await _signInManager.UserManager.GetRolesAsync(user);

            var roleItems = roles.Select(role => 
                new SelectListItem(
                    role.Name, 
                    role.Id,
                    userRoles.Any(ur => ur.Contains(role.Name)))).ToList();

            var vm = new EditUserViewModel
            {
                User = user,
                Roles = roleItems
            }; 

            return View(vm);
        }

        [HttpPost]
        [Authorize(Roles = $"{Constants.Roles.Administrator}")]
        public async Task<IActionResult> OnPostAsync(EditUserViewModel data)
        {
            var user = _unitOfWork.User.GetUser(data.User.Id);
            if (user == null)
            {
                return NotFound();
            }

            var userRolesInDb = await _signInManager.UserManager.GetRolesAsync(user);
            var rolesToAdd = new List<string>();
            var rolesToDelete = new List<string>();

            foreach (var role in data.Roles)
            {
                var assignedInDb = userRolesInDb.FirstOrDefault(ur => ur == role.Text);
                if (role.Selected)
                {
                    if (assignedInDb == null)
                    {
                        // Добавить роль
                        rolesToAdd.Add(role.Text);
                    }
                }
                else
                {
                    if(assignedInDb != null)
                    {
                        // Удалить роль 
                        rolesToDelete.Add(role.Text);
                    }
                }
            }
            if (rolesToAdd.Any())
            {
                await _signInManager.UserManager.AddToRolesAsync(user, rolesToAdd);
            }

            if (rolesToDelete.Any())
            {
                await _signInManager.UserManager.RemoveFromRolesAsync(user, rolesToDelete);
            }

            user.FirstName = data.User.FirstName;
            user.LastName = data.User.LastName;
            user.Email = data.User.Email;
            user.Patronymic = data.User.Patronymic;
            user.UserName = data.User.UserName;
            user.NormalizedUserName = data.User.UserName.ToUpper();

            _unitOfWork.User.UpdateUser(user);

            return RedirectToAction("Edit", new {id = user.Id});
        }


        [Authorize(Roles = $"{Constants.Roles.Administrator}")]
        public async Task<IActionResult> Delete()
        {
            return View();
        }

        [Authorize(Roles = $"{Constants.Roles.Administrator}")]
        [HttpPost]
        public async Task<IActionResult> Delete(string id, [FromServices] UserManager<ApplicationUser> userManager)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userManager.GetUserId(User)}'.");
            }


            //if (!await userManager.CheckPasswordAsync(user, model.Input.Password))
            //{
            //    ModelState.AddModelError(string.Empty, "Incorrect password.");
            //    return View();
            //}


            var result = await userManager.DeleteAsync(user);
            var userId = await userManager.GetUserIdAsync(user);
            if (!result.Succeeded)
            {
                TempData["Error"] = "Не удалось удалить пользователя!";
                throw new InvalidOperationException($"Unexpected error occurred deleting user.");
            }

            //await _signInManager.SignOutAsync();
            TempData["Success"] = "Пользователь успешно удален!";

            //_logger.LogInformation("User with ID '{UserId}' deleted themselves.", userId);

            return Redirect("~/User");
        }
    }
}
