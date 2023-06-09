﻿using Auto.Models;
using Auto.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Diagnostics;
using System.Runtime.Intrinsics.Arm;

namespace Auto.Controllers
{
  public class HomeController : Controller
  {
    private readonly ILogger<HomeController> _logger;
    private readonly AddObject _addobj;
    public readonly IConfiguration _configuration;

    public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
    {
      _logger = logger;
      _configuration = configuration;
      _addobj = new AddObject(configuration);
    }

    public IActionResult Client()
    {      
      var client = _addobj.Context.Clients.Include(p => p.Magazines).ToList();
      return View(client.ToList());
    }

    public IActionResult Auto()
    {      
      var auto = _addobj.Context.Autos.Include(p => p.Magazines).ToList();
      return View(auto.ToList());
    }
    

    [HttpGet]
    public IActionResult AddAuto()
    {
      return View();
    }

    [HttpPost]
    public IActionResult CreateAuto([FromForm] string model, [FromForm] int year, [FromForm] decimal price)
    {
      var auto = new Auto.Models.Auto { AutoModel = model, Year = year, Price = price };
      _addobj.Add(_configuration, auto);
      return RedirectToAction("Auto");
    }

    public IActionResult DeleteAuto(int id, string name)
    {
      _addobj.Delete(_configuration, name, id);
      return RedirectToAction("Auto");
    }
    
    [HttpGet]
    public IActionResult AddClient()
    {
      return View();
    }

    [HttpPost]
    public IActionResult CreateClient([FromForm] string fname, [FromForm] string sname)
    {
      var client = new Client { ClientName = fname, ClientSname = sname };
      _addobj.Add(_configuration, client);
      return RedirectToAction("Client");
    }

    public IActionResult DeleteClient(int id, string name)
    {
      _addobj.Delete(_configuration, name, id);
      return RedirectToAction("Client");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
      return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
  }
}