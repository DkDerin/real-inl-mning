using EcommerceWeb.Models;
using EcommerceWeb.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EcommerceWeb.Controllers
{
    public class UserListController : Controller
    {

        ApplicationDbContext context;
        //Added
        public UserListController()
        {
            context = new ApplicationDbContext();
        }

        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        // GET: UserList
        public ActionResult Index()
        {
            using (var db = new ApplicationDbContext())
            {
                var model = (from users in db.Users
                             select new
                             {
                                 UserId = users.Id,
                                 Email = users.Email,
                                 RoleNames = (from userRole in users.Roles
                                              join role in db.Roles on userRole.RoleId
                                              equals role.Id
                                              select role.Name).ToList()
                             }).ToList().Select(p => new UserIndexViewModel()
                             {
                                 UserId = p.UserId,
                                 Email = p.Email,
                                 UserRoles = string.Join(",", p.RoleNames)
                             });
                return View(model);
            }
        }


        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(string id)
        {
            var model = new UserCRUDViewModel();
            var user = UserManager.FindById(id);
            using (var db = new ApplicationDbContext())
            {
                model.UserId = user.Id;
                model.Email = user.Email;
                model.UserName = user.UserName;
                model.UserRoles = UserManager.GetRoles(user.Id).SingleOrDefault();
                model.AvailableUsers = new List<SelectListItem>();

                foreach (var item in db.Roles)
                {
                    model.AvailableUsers.Add(new SelectListItem { Value = item.Name, Text = item.Name });
                }

                return View(model);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(UserCRUDViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            using (var db = new ApplicationDbContext())
            {
                var user = UserManager.FindById(model.UserId);
                user.Id = model.UserId;

                UserManager.RemoveFromRole(user.Id, model.UserRoles);
                UserManager.AddToRole(user.Id, model.UserDropDown);

                db.SaveChanges();
                return RedirectToAction("Index");
            }
        }



    }
}