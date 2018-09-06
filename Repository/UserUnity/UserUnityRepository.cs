using DataEF.DataAccess;
using Model.In;
using Model.Out;
using Model.VM;
using System.Collections.Generic;
using System.Linq;

namespace Repository
{
    public partial class UserUnityRepository
    {
        public bool DeleteUnits(int userId)
        {
            using (var db = new DBContext())
            {
                List<UserUnits> userUnits = db.UserUnits.Where(x => x.UserId == userId).ToList();

                foreach (var item in userUnits)
                {
                    db.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                    db.SaveChanges();
                }
            }

            return true;
        }

        public UserUnityOut Insert(UserUnityCreateIn userUnityCreateIn)
        {
            UserUnityOut userUnityOut = new UserUnityOut();

            using (var db = new DBContext())
            {
                UserUnits userUnit = new UserUnits
                {
                    UnityId = userUnityCreateIn.UnityId,
                    UserId = userUnityCreateIn.UserId
                };

                db.UserUnits.Add(userUnit);
                db.SaveChanges();

                userUnityOut.result = db.UserUnits
                                   .Where(x => x.UnityId == userUnit.UnityId && x.UserId == userUnit.UserId)
                                   .Select(x => new UserUnityVM()
                                   {
                                       UnityId = x.UnityId,
                                       UserId = x.UserId,
                                   }).FirstOrDefault();
            }

            return userUnityOut;
        }
    }
}
