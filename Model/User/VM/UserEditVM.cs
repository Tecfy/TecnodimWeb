using System.Collections.Generic;

namespace Model.VM
{
    public class UserEditVM
    {
        public int UserId { get; set; }

        public string AspNetUserId { get; set; }

        public string RoleId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public List<string> Claims { get; set; }

        public List<string> Units { get; set; }
    }
}
