using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Auction_proj.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Auction_proj.Controllers
{
    public class HomeController : Controller
    {
        private AuctionContext _context;
    
        public HomeController(AuctionContext context)
        {
            _context = context;
        }
        // GET: /Home/
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [Route("Register")]
        public IActionResult Register(RegisterViewModel model)
        {
            if(ModelState.IsValid){
                User n = _context.Users.Where(x => x.UserName == model.UserName).SingleOrDefault();
                if(n == null){
                User NewUser = new User();
                    NewUser.FirstName = model.FirstName;
                    NewUser.LastName = model.LastName;
                    NewUser.UserName = model.UserName;
                    NewUser.Password = model.Password;
                    _context.Users.Add(NewUser);
                    _context.SaveChanges();
                    HttpContext.Session.SetInt32("Userid", NewUser.Userid);
                    return Redirect($"Dash/{NewUser.Userid}");
                }else{
                    TempData["Error"] = "UserName Already Taken";
                    return View("Index");
                }
            }else{
                return View("Index");
            }
        }
        [HttpPost]
        [Route("Login")]
        public IActionResult Login(string UserName, string Password){
             User p = _context.Users.Where(x => x.UserName == UserName).SingleOrDefault();
            if(p != null && p.Password == Password){
              HttpContext.Session.SetInt32("Userid", p.Userid);
              return Redirect($"Dash/{p.Userid}");
            }else{
                TempData["Error"] = "Problem with your UserName or Password";
                return View("Index");
            } 
        }
        [HttpGet]
        [Route("Dash/{Userid}")]
        public IActionResult Dash(int Userid){
            int? Int = HttpContext.Session.GetInt32("Userid");
            User Cur = _context.Users.Include(b => b.Bids).ThenInclude(w => w.Auction).Where(c => c.Userid == Userid).SingleOrDefault();
            List<Auction> W = _context.Auctions.Include(t => t.Bids).ThenInclude(u => u.User).Where(g => g.StartingBid != 0).OrderBy(k => k.EndDate).ToList();
            if(Int != Cur.Userid){
                return RedirectToAction("Index", "Home");
            }else{
                foreach(var x in W){
                    if(DateTime.Now > x.EndDate){
                        User Add = _context.Users.Where(l => l.UserName == x.Creator).SingleOrDefault();
                        foreach(var y in x.Bids){
                            if(y.Amount == x.StartingBid){
                                User Minus = _context.Users.Where(p => p.Userid == y.Userid).SingleOrDefault();
                                Add.Wallet += x.StartingBid;
                                Minus.Wallet -= x.StartingBid;
                                _context.Bids.Remove(y);
                                _context.Auctions.Remove(x);
                                _context.SaveChanges();
                            }
                        }
                    }
                }    
                List<Auction> K = _context.Auctions.Include(t => t.Bids).ThenInclude(u => u.User).Where(g => g.StartingBid != 0).OrderBy(k => k.EndDate).ToList();
                ViewBag.User = Cur;
                ViewBag.W = K;
                return View("Dash");
            }
        }
        [HttpGet]
        [Route("/Logout")]
        public IActionResult Logout(){
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
    }
}
