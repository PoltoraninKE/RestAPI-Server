using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MYRestApiServer.Core.Models
{
    public class Details
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public byte[] UserPhoto { get; set; }
        public User User { get; set; }
        public int? UserID { get; set; }
    }
}
