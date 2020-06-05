using System.Collections.Generic;


namespace MYRestApiServer.Core.Models
{
    public class User
    {
        public int ID { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }    
        public Details Details { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
    }
}
