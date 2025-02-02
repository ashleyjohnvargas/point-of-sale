﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using POS1.Models;

public class ProfileController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<ProfileController> _logger;
    public ProfileController(ApplicationDbContext context, ILogger<ProfileController> logger)
    {
        _context = context;
        _logger = logger;
    }

    // Display the profile page
    //[Authorize]
    public IActionResult ProfilePage()
    {

        if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserId")))
        {
            return RedirectToAction("LoginPage", "Login");
        }

        // Retrieve the logged-in user's ID from session (as string) and convert to int
        var userIdString = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
        {
            // If the session doesn't contain a valid UserId, redirect to login page
            return RedirectToAction("LoginPage", "Login");
        }

        // Log the UserId to the console (this will output to the console/logs)
        _logger.LogInformation($"Logged-in UserId: {userId}");


        // Fetch user profile and user details
        var profile = _context.UserProfiles.FirstOrDefault(p => p.Id == userId);
        var user = _context.Users.FirstOrDefault(u => u.Id == userId);

        if (profile == null)
        {
            // Create a default profile for new users
            profile = new Profile
            {
                FullName = "",
                Email = "",
                PhoneNumber = "",
                Address = ""
            };
            _context.UserProfiles.Add(profile);
            _context.SaveChanges();
        }
        return View(profile);
    }
    //[Authorize]
    public IActionResult EditProfile()
    {
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserId")))
        {
            return RedirectToAction("LoginPage", "Login");
        }
        // Retrieve the logged-in user's ID from session (as string) and convert to int
        var userIdString = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
        {
            // If the session doesn't contain a valid UserId, redirect to login page
            return RedirectToAction("LoginPage", "Login");
        }
        var profile = _context.UserProfiles.FirstOrDefault(p => p.Id == userId);
        return View(profile);
    }

    // Update profile
    [HttpPost]
    public IActionResult UpdateProfile(Profile model)
    {
        if (!ModelState.IsValid)
        {
            return View("EditProfile", model);
        }

        // Retrieve the logged-in user's ID from session (as string) and convert to int
        var userIdString = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
        {
            // If the session doesn't contain a valid UserId, redirect to login page
            return RedirectToAction("LoginPage", "Login");
        }

        var profile = _context.UserProfiles.FirstOrDefault(p => p.Id == userId);
        var user = _context.Users.FirstOrDefault(u => u.Id == userId);

        if (profile != null && user != null)
        {
            // Update profile information
            profile.FullName = user.FullName;
            profile.PhoneNumber = model.PhoneNumber;
            profile.Address = model.Address;
            profile.Email = model.Email;

            // Update email if changed
            //user.FullName = model.FullName;
            user.Email = model.Email;

            //HttpContext.Session.SetString("UserFullName", user.FullName);

            _context.SaveChanges();
            TempData["SuccessMessage"] = "Profile updated successfully!";
        }

        return RedirectToAction("ProfilePage");
    }

    // Display the ChangePasswordPage
   // [Authorize]
    public IActionResult ChangePasswordPage()
    {
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserId")))
        {
            return RedirectToAction("LoginPage", "Login");
        }
        return View();
    }

   
    // Change or Update the password
    [HttpPost]
    public IActionResult ChangePassword(string currentPassword, string newPassword, string confirmPassword)
    {
        // Retrieve the logged-in user's ID from session (as string) and convert to int
        var userIdString = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
        {
            return RedirectToAction("LoginPage", "Login");
        }

        var user = _context.Users.FirstOrDefault(u => u.Id == userId);
        if (user == null)
        {
            TempData["ErrorMessage"] = "User not found.";
            return RedirectToAction("ChangePasswordPage");
        }

        // Check if the current password matches the hashed password (using BCrypt)
        if (!BCrypt.Net.BCrypt.Verify(currentPassword, user.Password))  // Compare hashed password
        {
            TempData["ErrorMessage"] = "Current password is incorrect.";
            return RedirectToAction("ChangePasswordPage");
        }

        // Check if the new password and confirm password match
        if (newPassword != confirmPassword)
        {
            TempData["ErrorMessage"] = "New password and confirm password do not match.";
            return RedirectToAction("ChangePasswordPage");
        }

        // Hash the new password before saving it to the database
        user.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);  // Hash the new password


        _context.SaveChanges();

        TempData["SuccessMessage"] = "Password changed successfully!";
        return RedirectToAction("ProfilePage");
    }

}