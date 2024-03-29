﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using CourseManagementSystem.Models;
using System.Data.Entity;
using System.Net.Mail;

namespace CourseManagementSystem.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        public AccountController()
            : this(new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext())))
        {
        }

        public AccountController(UserManager<ApplicationUser> userManager)
        {
            UserManager = userManager;
        }
        private async Task AddUserToRoleAsync(ApplicationUser user, string role)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var result = await userManager.AddToRoleAsync(user.Id, role);
        }

        public UserManager<ApplicationUser> UserManager { get; private set; }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindAsync(model.UserName, model.Password);
                if (user != null && user.IsConfirmed)
                {
                    await SignInAsync(user, model.RememberMe);
                    return RedirectToLocal(returnUrl);
                }
                else
                {
                    ModelState.AddModelError("", "Invalid username or password.");
                }
            }

            
            return View(model);
        }

        private string CreateToken()
        {
            return ShortGuid.NewShortGuid();
        }

        private void SendEmail(string mail, string subject, string body)
        {

            SmtpClient ss = new SmtpClient("smtp.gmail.com", 587);
            ss.EnableSsl = true;
            ss.Timeout = 10000;
            ss.DeliveryMethod = SmtpDeliveryMethod.Network;
            ss.UseDefaultCredentials = false;
            ss.Credentials = new System.Net.NetworkCredential("course.management.system.help@gmail.com", "80624921905");


            MailMessage message = new MailMessage();
            //Setting From , To
            message.From = new MailAddress("course.management.system.help@gmail.com", "Course");
            message.To.Add(new MailAddress(mail));
            message.Subject = subject;
            message.Body = body;

            ss.Send(message);

        }

        private void SendEmailConfirmation(string to, string username, string confirmationToken)
        {
            string link = Url.Action("RegisterConfirmation", "Account", new { Id = confirmationToken }, "http");
            SendEmail(to, "Confirmation of registration", link);
        }


        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {

                string confirmationToken = CreateToken();
                var user = new ApplicationUser() { UserName = model.UserName, Email = model.Email, FirstName = model.FirstName, LastName = model.LastName, ConfirmationToken = confirmationToken, IsConfirmed = false };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await AddUserToRoleAsync(user, model.Role);
                    SendEmailConfirmation(model.Email, model.UserName, confirmationToken);
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    AddErrors(result);
                }
            }

            
            return View(model);
        }

        private bool ConfirmAccount(string confirmationToken)
        {
            ApplicationUser user = db.Users.First(u => u.ConfirmationToken == confirmationToken);
            if (user != null)
            {
                user.IsConfirmed = true;
                DbSet<ApplicationUser> dbSet = db.Set<ApplicationUser>();
                dbSet.Attach(user);
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();

                return true;
            }
            return false;
        }

        [AllowAnonymous]
        public ActionResult RegisterConfirmation(string Id)
        {
            if (ConfirmAccount(Id))
            {
                return RedirectToAction("Login");
            }
            return RedirectToAction("Login");
        }

        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost, AllowAnonymous, ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(string UserName)
        {

            var user = UserManager.FindByName(UserName);
            if (user == null)
            {
                TempData["Message"] = "User Not exist.";
            }
            else
            {
                // генерируем маркер пароля
                ResetToken token = new ResetToken() { Token = CreateToken(), UserName = UserName };

                db.ResetToken.Add(token);
                db.SaveChanges();
                // создаем урл с маркером пароля
                var resetLink = Url.Action("ResetPassword", "Account", new { un = UserName, rt = token.Token }, "http");
                // получим e-mail прользователя
                var email = db.Users.Where(x => x.UserName == UserName).Select(x => x.Email).FirstOrDefault();
                // отсылаем email
                string subject = "Change Password";
                string body = "To change the password, follow the link " + resetLink;
                try
                {
                    SendEmail(email, subject, body);
                    TempData["Message"] = "A message with a password reset link has been sent by e-mail.";
                }
                catch (Exception ex)
                {
                    TempData["Message"] = "Error occured while sending email." + ex.Message;
                }
            }
            return View();
        }

        [AllowAnonymous]
        public ActionResult ResetPassword(string un, string rt)
        {
            
            ApplicationUser userProfile = db.Users.FirstOrDefault(x => x.UserName.Equals(un));
            ResetToken resetToken = db.ResetToken.FirstOrDefault(x => x.Token.Equals(rt));
            if (userProfile == null || resetToken == null || !(resetToken.UserName == userProfile.UserName))
            {
                return RedirectToAction("BadLink");
            }

            string newpassword = new Random(8).Next(99999999).ToString();

            if (!(UserManager.RemovePassword(UserManager.FindByName(un).Id) == IdentityResult.Success))
            {
                return RedirectToAction("BadLink");
            }
            UserManager.AddPassword(UserManager.FindByName(un).Id, newpassword);

            db.ResetToken.Remove(resetToken);
            db.SaveChanges();

            // send an email with a new password
            string subject = "New password";
            string body = "New password to access the system: " + newpassword;
            try
            {
                SendEmail(userProfile.Email, subject, body);
                ViewBag.Message ="A password letter has been sent.";
            }
            catch (Exception ex)
            {
                ViewBag.Message = "An error occurred while sending the email." + ex.Message;
            }

            return View();
        }

        //
        // POST: /Account/Disassociate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Disassociate(string loginProvider, string providerKey)
        {
            ManageMessageId? message = null;
            IdentityResult result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }
            return RedirectToAction("Manage", new { Message = message });
        }

        //
        // GET: /Account/Manage

        public ActionResult Manage(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been successfully changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Password set successfully."
                : message == ManageMessageId.RemoveLoginSuccess ?"The external login has been deleted successfully."
                : message == ManageMessageId.Error ? "An error has occurred. Check the spelling of the data."
                : message == ManageMessageId.ChangeDataSuccess ?"Data successfully changed."
                : "";
            ViewBag.HasLocalPassword = HasPassword();
            ViewBag.ReturnUrl = Url.Action("Manage");
            return View();
        }

        //
        // POST: /Account/Manage
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<ActionResult> Manage(ManageUserViewModel model)
        {
            bool hasPassword = HasPassword();
            ViewBag.HasLocalPassword = hasPassword;
            ViewBag.ReturnUrl = Url.Action("Manage");
            if (hasPassword)
            {
                if (ModelState.IsValid)
                {
                    IdentityResult result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
                    }
                    else
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.Error });
                    }
                }
            }
            else
            {
                // User does not have a password so remove any validation errors caused by a missing OldPassword field
                ModelState state = ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }

                if (ModelState.IsValid)
                {
                    IdentityResult result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
                    }
                    else
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.Error });
                    }
                }
            }

            //The appearance of this message indicates an error; redisplay of form
            return View(model);
        }

        [Authorize]
        [AllowAnonymous]
        public ActionResult Usr(string profile)
        {
            //var course = db.Courses.Where(l => l.Author.UserName == profile);
            try
            {
                var courseMarks = db.CourseMark.Where(l => l.course.Author.UserName == profile);
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
                var role = userManager.FindByName(profile).Roles.First().RoleId.ToString();
                var firstName = userManager.FindByName(profile).FirstName.ToString();
                var lastName = userManager.FindByName(profile).LastName.ToString();
                var email = userManager.FindByName(profile).Email.ToString();
                ViewBag.loginProfile = profile;
                ViewBag.profileRole = role;
                ViewBag.profileFirstName = firstName;
                ViewBag.profileLastName = lastName;
                ViewBag.profileEmail = email;
                return View(courseMarks.ToList());
            }
            catch
            {
                return null;
            }
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Запрос перенаправления к внешнему поставщику входа
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var user = await UserManager.FindAsync(loginInfo.Login);
            if (user != null)
            {
                await SignInAsync(user, isPersistent: false);
                return RedirectToLocal(returnUrl);
            }
            else
            {
                // If the user does not have an account, then prompt the user to create an account
                ViewBag.ReturnUrl = returnUrl;
                ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { UserName = loginInfo.DefaultUserName });
            }
        }

        //
        // POST: /Account/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // Request a redirect to the external login provider to link a login for the current user
            return new ChallengeResult(provider, Url.Action("LinkLoginCallback", "Account"), User.Identity.GetUserId());
        }

        //
        // GET: /Account/LinkLoginCallback
        public async Task<ActionResult> LinkLoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
            if (loginInfo == null)
            {
                return RedirectToAction("Manage", new { Message = ManageMessageId.Error });
            }
            var result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
            if (result.Succeeded)
            {
                return RedirectToAction("Manage");
            }
            return RedirectToAction("Manage", new { Message = ManageMessageId.Error });
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Manage");
            }

            if (ModelState.IsValid)
            {
                
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser() { UserName = model.UserName };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInAsync(user, isPersistent: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult RemoveAccountList()
        {
            var linkedAccounts = UserManager.GetLogins(User.Identity.GetUserId());
            ViewBag.ShowRemoveButton = HasPassword() || linkedAccounts.Count > 1;
            return (ActionResult)PartialView("_RemoveAccountPartial", linkedAccounts);
        }



        [ChildActionOnly]
        public ActionResult ChangeUserData()
        {

            return (ActionResult)PartialView("_ChangeUserDataPartial");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangeUserData(UserDataViewModel model)
        {
            if (ModelState.IsValid)
            {

                var user = UserManager.FindById(User.Identity.GetUserId());
                user.Email = model.Email;
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                var result = await UserManager.UpdateAsync(user);

                return RedirectToAction("Manage", new { Message = ManageMessageId.ChangeDataSuccess });
            }
            else
                return RedirectToAction("Manage", new { Message = ManageMessageId.Error });
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing && UserManager != null)
            {
                UserManager.Dispose();
                UserManager = null;
            }
            base.Dispose(disposing);
        }

        #region 
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private async Task SignInAsync(ApplicationUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            var identity = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            Error,
            ChangeDataSuccess
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        private class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties() { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }


        }
        #endregion
    }
}