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
        private const double HIGHRISK = 70;
        private const int CHANGEDAY = 10;

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
            if(Level > HIGHRISK)
            {
                throw new Exception("High risk of exhaustion");
            }

            if (Level < MINLEVEL || Level > MAXLEVEL)
            {
                throw new ArgumentOutOfRangeException("Invalid Level");
            }
        }

        public void Reduce(double am)
        {
            this = this with { Level = Level - am };
            ValidateLv();
        }

        public void Progress()
        {
            this = this with { Level = Level + Progression };
            try
            {
                ValidateLv();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw new ArgumentOutOfRangeException("Error creating ExhaustionLevel", ex);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("High risk", ex);
            }
        }

        public void GoodDay()
        {
            this = this with
            {
                Level = Math.Max(0, Level - CHANGEDAY)
            };
        }

        public void BadDay()
        {
            this = this with
            {
                Level = Math.Min(100, Level + CHANGEDAY)
            };
        }
    }
}
