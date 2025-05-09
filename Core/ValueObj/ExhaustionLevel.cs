using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ValueObj
{
    public record struct ExhaustionLevel
    {
        private const int MINLEVEL = 0;
        private const int MAXLEVEL = 100;
        private const double MINPROGRESSION = 0;
        private const double MAXPROGRESSION = 7.5;

        public double Level { get; init; }
        public double Progression { get; init; }

        public ExhaustionLevel() { }
        public ExhaustionLevel(double progression)
        {
            Level = MINLEVEL;
            Progression = progression;
            try
            {
                ValidatePr();
            }
            catch (Exception ex)
            {
                throw new Exception("Error creating ExhaustionLevel", ex);
            }
        }

        //public ExhaustionLevel(double level, double progression)
        //{
        //    Level = level;
        //    Progression = progression;
        //    try
        //    {
        //        ValidateLv();
        //        ValidatePr();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Error creating ExhaustionLevel", ex);
        //    }
        //}

        private void ValidatePr()
        {
            if(Progression < MINPROGRESSION || Progression > MAXPROGRESSION)
            {
                throw new ArgumentException("Invalid Progression");
            }
        }

        private void ValidateLv()
        {
            if (Level < MINLEVEL || Level > MAXLEVEL)
            {
                throw new ArgumentException("Invalid Level");
            }
        }

        public void Reduce(double am)
        {
            this = this with { Level = Level - am };
            ValidateLv();
        }
    }
}
