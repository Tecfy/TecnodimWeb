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
                unitsOut.totalCount = db.Units.Count(x => x.Active == true && x.DeletedDate == null);

                unitsOut.result = db.Units
                                   .Where(x => x.Active == true && x.DeletedDate == null)
                                   .Select(x => new UnitsVM()
                                   {
                                       UnityId = x.UnityId,
                                       ExternalId = x.ExternalId,
                                       Name = x.Name,
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
                                       Name = x.Name,
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
                                   .Where(x => x.Active == true && x.DeletedDate == null && x.UnityId == unityIn.UnityId)
                                   .Select(x => new UnityVM()
                                   {
                                       UnityId = x.UnityId,
                                       ExternalId = x.ExternalId,
                                       Name = x.Name,
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
                                   .Where(x => x.Active == true && x.DeletedDate == null && x.UnityId == unityIn.UnityId)
                                   .Select(x => new UnityEditVM()
                                   {
                                       UnityId = x.UnityId,
                                       ExternalId = x.ExternalId,
                                       Name = x.Name,
                                   }).FirstOrDefault();
            }

            return unityEditOut;
        }

        public int? GetByCode(string code)
        {
            int? unityId = null;

            using (var db = new DBContext())
            {
                unityId = db.Units
                            .Where(x => x.Active == true && x.DeletedDate == null && x.ExternalId == code)
                            .Select(x => x.UnityId).FirstOrDefault();
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
                    Name = unityCreateIn.Name
                };

                db.Units.Add(unit);
                db.SaveChanges();

                unityOut.result = db.Units
                                   .Where(x => x.Active == true && x.DeletedDate == null && x.UnityId == unit.UnityId)
                                   .Select(x => new UnityVM()
                                   {
                                       UnityId = x.UnityId,
                                       ExternalId = x.ExternalId,
                                       Name = x.Name,
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

                db.Entry(unit).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                unityOut.result = db.Units
                                  .Where(x => x.Active == true && x.DeletedDate == null && x.UnityId == unit.UnityId)
                                  .Select(x => new UnityVM()
                                  {
                                      UnityId = x.UnityId,
                                      ExternalId = x.ExternalId,
                                      Name = x.Name,
                                  }).FirstOrDefault();
            }

            return unityOut;
        }
    }
}
