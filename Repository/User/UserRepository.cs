using DataEF.DataAccess;
using Model.In;
using Model.Out;
using Model.VM;
using System;
using System.Linq;

namespace Repository
{
    public partial class UserRepository
    {
        RegisterEventRepository registerEventRepository = new RegisterEventRepository();

        public bool Delete(int userId)
        {
            using (var db = new DBContext())
            {
                Users user = db.Users.Find(userId);
                user.Active = false;
                user.DeletedDate = DateTime.Now;

                db.Entry(user).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }

            return true;
        }

        public UsersOut GetAll(UsersIn usersIn)
        {
            UsersOut usersOut = new UsersOut();

            using (var db = new DBContext())
            {
                usersOut.totalCount = db.Users.Count(x => x.Active == true && x.DeletedDate == null);

                usersOut.result = db.Users
                                   .Where(x => x.Active == true && x.DeletedDate == null)
                                   .Select(x => new UsersVM()
                                   {
                                       UserId = x.UserId,
                                       Role = x.AspNetUsers.AspNetUserRoles.FirstOrDefault().AspNetRoles.Name,
                                       Name = x.FirstName + " " + x.LastName,
                                       Email = x.AspNetUsers.Email,
                                       CreatedDate = x.CreatedDate,
                                   })
                                   .OrderBy(usersIn.sort, !usersIn.sortdirection.Equals("asc"))
                                   .Skip((usersIn.currentPage.Value - 1) * usersIn.qtdEntries.Value)
                                   .Take(usersIn.qtdEntries.Value)
                                   .ToList();
            }

            return usersOut;
        }

        public UserOut GetById(UserIn userIn)
        {
            UserOut userOut = new UserOut();

            using (var db = new DBContext())
            {
                userOut.result = db.Users
                                   .Where(x => x.Active == true && x.DeletedDate == null && x.UserId == userIn.UserId)
                                   .Select(x => new UserVM()
                                   {
                                       UserId = x.UserId,
                                       Role = x.AspNetUsers.AspNetUserRoles.FirstOrDefault().AspNetRoles.Name,
                                       Name = x.FirstName + " " + x.LastName,
                                       Email = x.AspNetUsers.Email,
                                   }).FirstOrDefault();
            }

            return userOut;
        }

        public UserEditOut GetEditById(UserIn userIn)
        {
            UserEditOut userOut = new UserEditOut();

            using (var db = new DBContext())
            {
                userOut.result = db.Users
                                   .Where(x => x.Active == true && x.DeletedDate == null && x.UserId == userIn.UserId)
                                   .Select(x => new UserEditVM()
                                   {
                                       UserId = x.UserId,
                                       AspNetUserId = x.AspNetUserId,
                                       RoleId = x.AspNetUsers.AspNetUserRoles.FirstOrDefault().RoleId,
                                       FirstName = x.FirstName,
                                       LastName = x.LastName,
                                       Email = x.AspNetUsers.Email,
                                   }).FirstOrDefault();
            }

            return userOut;
        }

        public UserOut Insert(UserCreateIn userCreateVM)
        {
            UserOut userOut = new UserOut();

            using (var db = new DBContext())
            {
                using (var scope = new System.Transactions.TransactionScope())
                {


                }
            }

            return userOut;
        }

        public UserOut Update(UserEditIn userEditVM)
        {
            throw new NotImplementedException();
        }
    }
}
