using DataEF.DataAccess;
using Model.In;
using Model.Out;
using Model.VM;
using System;
using System.Linq;

namespace Repository
{
    public partial class UnityRepository
    {
        public bool Delete(int unitId)
        {
            using (var db = new DBContext())
            {
                Units unit = db.Units.Find(unitId);
                unit.Active = false;
                unit.DeletedDate = DateTime.Now;

                db.Entry(unit).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }

            return true;
        }

        public UnitsOut GetAll(UnitsIn unitsIn)
        {
            UnitsOut unitsOut = new UnitsOut();

            using (var db = new DBContext())
            {
                unitsOut.totalCount = db.Units.Count(x => x.DeletedDate == null);

                unitsOut.result = db.Units
                                   .Where(x => x.DeletedDate == null
                                            &&
                                            (
                                                string.IsNullOrEmpty(unitsIn.filter)
                                                ||
                                                (
                                                    x.Name.Contains(unitsIn.filter)
                                                )
                                            )
                                    )
                                   .Select(x => new UnitsVM()
                                   {
                                       UnityId = x.UnityId,
                                       ExternalId = x.ExternalId,
                                       Name = x.Name,
                                       Active = x.Active,
                                       CreatedDate = x.CreatedDate
                                   })
                                   .OrderBy(unitsIn.sort, !unitsIn.sortdirection.Equals("asc"))
                                   .Skip((unitsIn.currentPage.Value - 1) * unitsIn.qtdEntries.Value)
                                   .Take(unitsIn.qtdEntries.Value)
                                   .ToList();
            }

            return unitsOut;
        }

        public UnitsDDLOut GetDDLAll()
        {
            UnitsDDLOut unitsDDLOut = new UnitsDDLOut();

            using (var db = new DBContext())
            {
                unitsDDLOut.result = db.Units
                                   .Where(x => x.Active == true && x.DeletedDate == null)
                                   .Select(x => new UnitsDDLVM()
                                   {
                                       UnityId = x.UnityId,
                                       Name = x.ExternalId + " - " + x.Name,
                                   })
                                   .ToList();
            }

            return unitsDDLOut;
        }

        public UnitsDDLOut GetDDLAllByAspNetUserId(string aspNetUserId)
        {
            UnitsDDLOut unitsDDLOut = new UnitsDDLOut();

            using (var db = new DBContext())
            {
                unitsDDLOut.result = db.UserUnits
                                   .Where(x => x.Units.Active == true && x.Units.DeletedDate == null && x.Users.AspNetUserId == aspNetUserId)
                                   .Select(x => new UnitsDDLVM()
                                   {
                                       UnityId = x.UnityId,
                                       Name = x.Units.ExternalId + " - " + x.Units.Name,
                                   })
                                   .ToList();
            }

            return unitsDDLOut;
        }

        public UnityOut GetById(UnityIn unityIn)
        {
            UnityOut unityOut = new UnityOut();

            using (var db = new DBContext())
            {
                unityOut.result = db.Units
                                   .Where(x => x.DeletedDate == null && x.UnityId == unityIn.UnityId)
                                   .Select(x => new UnityVM()
                                   {
                                       UnityId = x.UnityId,
                                       ExternalId = x.ExternalId,
                                       Name = x.Name,
                                       Active = x.Active
                                   }).FirstOrDefault();
            }

            return unityOut;
        }

        public UnityEditOut GetEditById(UnityIn unityIn)
        {
            UnityEditOut unityEditOut = new UnityEditOut();

            using (var db = new DBContext())
            {
                unityEditOut.result = db.Units
                                   .Where(x => x.DeletedDate == null && x.UnityId == unityIn.UnityId)
                                   .Select(x => new UnityEditVM()
                                   {
                                       UnityId = x.UnityId,
                                       ExternalId = x.ExternalId,
                                       Name = x.Name,
                                       Active = x.Active
                                   }).FirstOrDefault();
            }

            return unityEditOut;
        }

        public int? GetByCode(string unitCode, string unit)
        {
            int? unityId = null;

            using (var db = new DBContext())
            {
                unityId = db.Units
                            .Where(x => x.DeletedDate == null && x.ExternalId == unitCode)
                            .Select(x => x.UnityId).FirstOrDefault();

                if (unityId <= 0 || unityId == null)
                {
                    unityId = Insert(new UnityCreateIn { ExternalId = unitCode, Name = unit }).result.UnityId;
                }
            }

            return unityId;
        }

        public UnityOut Insert(UnityCreateIn unityCreateIn)
        {
            UnityOut unityOut = new UnityOut();

            using (var db = new DBContext())
            {
                Units unit = new Units
                {
                    UnityId = unityCreateIn.UnityId,
                    ExternalId = unityCreateIn.ExternalId,
                    Name = unityCreateIn.Name,
                    Active = unityCreateIn.Active
                };

                db.Units.Add(unit);
                db.SaveChanges();

                unityOut.result = db.Units
                                   .Where(x => x.DeletedDate == null && x.UnityId == unit.UnityId)
                                   .Select(x => new UnityVM()
                                   {
                                       UnityId = x.UnityId,
                                       ExternalId = x.ExternalId,
                                       Name = x.Name,
                                       Active = x.Active
                                   }).FirstOrDefault();
            }

            return unityOut;
        }

        public UnityOut Update(UnityEditIn unityEditIn)
        {
            UnityOut unityOut = new UnityOut();

            using (var db = new DBContext())
            {
                Units unit = db.Units.Find(unityEditIn.UnityId);

                unit.EditedDate = DateTime.Now;
                unit.ExternalId = unityEditIn.ExternalId;
                unit.Name = unityEditIn.Name;
                unit.Active = unityEditIn.Active;

                db.Entry(unit).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                unityOut.result = db.Units
                                  .Where(x => x.DeletedDate == null && x.UnityId == unit.UnityId)
                                  .Select(x => new UnityVM()
                                  {
                                      UnityId = x.UnityId,
                                      ExternalId = x.ExternalId,
                                      Name = x.Name,
                                      Active = x.Active

                                  }).FirstOrDefault();
            }

            return unityOut;
        }
    }
}
