using Microsoft.IdentityModel.Tokens;
using System.Text;
namespace MYRestApiServer.Core.Options
{
    public class KeyGen
    {
        public static SymmetricSecurityKey GetSymmetricSecurityKey(string key) 
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        }
    }
}
