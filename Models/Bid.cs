using System;
using System.Collections.Generic;

namespace Auction_proj.Models
{
    public class Bid : BaseEntity
    {
        public int Userid { get; set; }
        public User User { get; set; }
        public int Auctionid { get; set; }
        public Auction Auction { get; set; }
        public int Bidid { get; set; }
        public int Amount { get; set; }
    }
}