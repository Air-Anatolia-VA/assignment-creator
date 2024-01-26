using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Air_Anatolia_VA_Assignment_Creator.Class
{
    internal class Flight
    {
        internal string code;
        internal string flightnum;
        internal string depicao;
        internal string arricao;
        internal string route;
        internal string aircraft;
        internal double distance;
        internal string deptime;
        internal string arrtime;
        internal double flighttime;
        internal string flighttype;
        internal int[] daysofweek;
    }

    enum DaysOfWeek
    {
        Sunday,
        Monday,
        Tuesday,
        Wednesday,
        Thursday,
        Friday,
        Saturday,
    }
}
