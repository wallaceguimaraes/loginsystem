﻿using Microsoft.AspNetCore.Mvc;

namespace LoginSystem.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
