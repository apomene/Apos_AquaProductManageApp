
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using static Apos_AquaProductManageApp.Model.FishFarmModel;

namespace Apos_AquaProductManageApp.DBContext
{    
    public class FishFarmDbContext : DbContext
    {
        public DbSet<Cage> Cages { get; set; }
        

    }

    public class CageService
    {
        public List<Cage> GetAllCages()
        {
            using var db = new FishFarmDbContext();
            return db.Cages.ToList();
        }

        public void AddCage(string name, bool isActive)
        {
            using var db = new FishFarmDbContext();
            db.Cages.Add(new Cage { Name = name, IsActive = isActive });
            db.SaveChanges();
        }

        public void DeleteCage(int id)
        {
            using var db = new FishFarmDbContext();
            var cage = db.Cages.Find(id);
            if (cage != null)
            {
                db.Cages.Remove(cage);
                db.SaveChanges();
            }
        }

        public void UpdateCage(int id, string name, bool isActive)
        {
            using var db = new FishFarmDbContext();
            var cage = db.Cages.Find(id);
            if (cage != null)
            {
                cage.Name = name;
                cage.IsActive = isActive;
                db.SaveChanges();
            }
        }
    }

}
