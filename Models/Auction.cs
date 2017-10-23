using System;
using System.Collections.Generic;

namespace Auction_proj.Models
{
    public class Auction : BaseEntity
    {
        public int Auctionid { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public int StartingBid { get; set; }
        public DateTime EndDate { get; set; }
        public string Creator { get; set; }
        public List<Bid> Bids { get; set; }
        public Auction(){
            Bids = new List<Bid>();
        }
    }
}