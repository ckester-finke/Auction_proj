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
    public class AuctionController : Controller
    {
        private AuctionContext _context;
    
        public AuctionController(AuctionContext context)
        {
            _context = context;
        }
        [HttpGet]
        [Route("NewAuction")]
        public IActionResult NewAuction(){
            int? Int = HttpContext.Session.GetInt32("Userid");
            User Cur = _context.Users.Include(b => b.Bids).ThenInclude(w => w.Auction).Where(c => c.Userid == (int)Int).SingleOrDefault();
            ViewBag.Userid = Cur.Userid;
            return View("NewAuction");
        }
        [HttpPost]
        [Route("Auct")]
        public IActionResult Auct(RegisterAuctModel model){
            int? Int = HttpContext.Session.GetInt32("Userid");
            User Cur = _context.Users.Include(b => b.Bids).ThenInclude(w => w.Auction).Where(c => c.Userid == (int)Int).SingleOrDefault();
            ViewBag.Userid = Cur.Userid;
            if(ModelState.IsValid){
                if(model.StartingBid < 1){
                    TempData["Error"] = "Starting bid must be greater than 0";
                    return View("NewAuction");
                }if(DateTime.Now > model.EndDate){
                    TempData["Error"] = "EndDate Can't be in the past";
                    return View("NewAuction");
                }
                Auction New = new Auction();
                New.ProductName = model.ProductName;
                New.Description = model.Description;
                New.StartingBid = model.StartingBid;
                New.Creator = Cur.UserName;
                New.EndDate = model.EndDate;
                _context.Auctions.Add(New);
                _context.SaveChanges();
                return Redirect($"/Dash/{Cur.Userid}");
            }
            
            return View("NewAuction");
        }
        [HttpGet]
        [Route("/Delete/{Auctionid}")]
        public IActionResult Delete(int auctionid){
            int? Int = HttpContext.Session.GetInt32("Userid");
            User Cur = _context.Users.Include(b => b.Bids).ThenInclude(w => w.Auction).Where(c => c.Userid == (int)Int).SingleOrDefault();
            Auction A = _context.Auctions.Where(d => d.Auctionid == auctionid).SingleOrDefault();
            _context.Auctions.Remove(A);
            _context.SaveChanges();
            return Redirect($"/Dash/{Cur.Userid}");
        }
        [HttpGet]
        [Route("/Product/{Auctionid}")]
        public IActionResult Product(int Auctionid){
            int? Int = HttpContext.Session.GetInt32("Userid");
            User Cur = _context.Users.Include(b => b.Bids).ThenInclude(w => w.Auction).Where(c => c.Userid == (int)Int).SingleOrDefault();
            Auction B = _context.Auctions.Where(h => h.Auctionid == Auctionid).SingleOrDefault();
            double days = (B.EndDate - DateTime.Now).TotalDays;
            ViewBag.Days = days;
            ViewBag.Userid = Cur.Userid;
            ViewBag.B = B;
            return View("Product");
        }
        [HttpPost]
        [Route("NewBid/{Auctionid}")]
        public IActionResult NewBid(int Auctionid, int amount){
            int? Int = HttpContext.Session.GetInt32("Userid");
            Auction B = _context.Auctions.Where(h => h.Auctionid == Auctionid).SingleOrDefault();
            User Cur = _context.Users.Include(b => b.Bids).ThenInclude(w => w.Auction).Where(c => c.Userid == (int)Int).SingleOrDefault();
            if(amount > B.StartingBid){
            Bid L = new Bid();
            L.Userid = Cur.Userid;
            L.Auctionid = B.Auctionid;
            L.Amount = amount;
            B.StartingBid = amount;
            _context.Bids.Add(L);
            _context.SaveChanges();
            return Redirect($"/Product/{B.Auctionid}");
            }else{
                TempData["Error"] = "Bid must be higher then current highest";
                return Redirect($"/Product/{B.Auctionid}");
            }
        }
    }
}