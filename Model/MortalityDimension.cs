using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apos_AquaProductManageApp.Model
{
    public enum MortalityDimension
    {
        Cage,
        Year,
        Month
    }

    public class MortalityPivotKey
    {
        public int? CageId { get; set; }
        public int? Year { get; set; }
        public int? Month { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is not MortalityPivotKey other) return false;
            return CageId == other.CageId && Year == other.Year && Month == other.Month;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(CageId, Year, Month);
        }
    }

}
