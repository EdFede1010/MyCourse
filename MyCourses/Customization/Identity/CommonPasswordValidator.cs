using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace MyCourses.Customization.Identity
{
    public class CommonPasswordValidator<TUser> : IPasswordValidator<TUser> where TUser : class
    {
        private readonly string[] commons;
        
        public CommonPasswordValidator()
        {
            this.commons = new[]
            {
                "password", "abc", "123", "qwerty"
            };
        }
        public Task<IdentityResult> ValidateAsync(UserManager<TUser> manager, TUser user, string password)
        {
            IdentityResult result;
            if (commons.Any(common => password.Contains(common, System.StringComparison.CurrentCultureIgnoreCase)))
            {
                result = IdentityResult.Failed(new IdentityError { Description = "Password troppo facile" });
            }
            else
            {
                result = IdentityResult.Success;
            }
            return Task.FromResult(result);
        }
    }
}