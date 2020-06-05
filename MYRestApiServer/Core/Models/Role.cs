using System.Collections.Generic;

namespace MYRestApiServer.Core.Models
{
    public class Role
    {
        public int ID { get; set; }
        public string RoleName { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
    }
}
