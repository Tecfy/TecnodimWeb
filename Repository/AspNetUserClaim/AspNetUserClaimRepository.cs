using DataEF.DataAccess;
using Model.In;
using Model.Out;
using Model.VM;
using System.Collections.Generic;
using System.Linq;

namespace Repository
{
    public class AspNetUserClaimRepository
    {
        public bool Delete(string userId)
        {
            using (var db = new DBContext())
            {
                List<AspNetUserClaims> aspNetUserClaims = db.AspNetUserClaims.Where(x => x.UserId == userId).ToList();

                foreach (var item in aspNetUserClaims)
                {
                    db.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                    db.SaveChanges();
                }
            }

            return true;
        }

        public AspNetUserClaimOut GetById(int userId)
        {
            AspNetUserClaimOut aspNetUserClaimOut = new AspNetUserClaimOut();

            using (var db = new DBContext())
            {
                aspNetUserClaimOut.result = db.AspNetUserClaims
                                              .Select(x => new AspNetUserClaimVM()
                                              {
                                                  Id = x.Id,
                                                  UserId = x.UserId,
                                                  ClaimType = x.ClaimType,
                                                  ClaimValue = x.ClaimValue
                                              })
                                              .OrderBy(x => x.ClaimType)
                                              .ToList();
            }

            return aspNetUserClaimOut;
        }

        public AspNetUserClaimOut Insert(AspNetUserClaimIn aspNetUserClaimIn)
        {
            AspNetUserClaimOut aspNetUserClaimOut = new AspNetUserClaimOut();

            using (var db = new DBContext())
            {
                AspNetUserClaims aspNetUserClaim = new AspNetUserClaims
                {
                    UserId = aspNetUserClaimIn.UserId,
                    ClaimType = aspNetUserClaimIn.ClaimType,
                    ClaimValue = aspNetUserClaimIn.ClaimValue
                };

                db.AspNetUserClaims.Add(aspNetUserClaim);
                db.SaveChanges();
            }

            return aspNetUserClaimOut;
        }
    }
}
