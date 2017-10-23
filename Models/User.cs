using System;
using System.Collections.Generic;

namespace Auction_proj.Models
{
    public class User : BaseEntity
    {
        public string UserName { get; set; }
        public int Userid { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public int Wallet { get; set; }
        public List<Bid> Bids { get; set; }
        public User(){
            Bids = new List<Bid>();
            Wallet = 1000;

        }

    }
}