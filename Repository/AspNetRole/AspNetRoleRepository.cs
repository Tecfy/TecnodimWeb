using DataEF.DataAccess;
using Model.Out;
using Model.VM;
using System.Linq;

namespace Repository
{
    public class AspNetRoleRepository
    {
        RegisterEventRepository registerEventRepository = new RegisterEventRepository();

        public AspNetRolesOut GetRoles()
        {
            AspNetRolesOut aspNetRolesOut = new AspNetRolesOut();

            using (var db = new DBContext())
            {
                aspNetRolesOut.result = db.AspNetRoles
                                          .Select(x => new AspNetRolesVM()
                                          {
                                              RoleId = x.Id,
                                              Name = x.Name
                                          })
                                          .OrderBy(x => x.Name)
                                          .ToList();
            }

            return aspNetRolesOut;
        }
    }
}
