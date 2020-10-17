using System;
using System.Diagnostics;
using System.Threading;
<<<<<<< HEAD
using src.CitizenLibrary;
=======
>>>>>>> DK-Branch

namespace src
{
    public class testmain
    {
        public static void main(string[] args)
        {
<<<<<<< HEAD
            Building b = new Building("yeet", 0, 20, 20, 0, 0);
            Supermarket s = new Supermarket("yeet", 0, 20 ,20, 0, 20);
            Hospital h = new Hospital("yeet", 0, 20, 20, 0, 20, false);
            
            Citizen c = new Citizen();
            Citizen d = new Citizen();
            Citizen e = new Citizen();

            b.enterBuilding(c);
            b.enterBuilding(d);
            b.enterBuilding(e);

            b.exitBuilding(c);
            Console.WriteLine("c has exited the building");
            b.exitBuilding(e);
            Console.WriteLine("d has exited the building");
            b.exitBuilding(d);
            Console.WriteLine("e has exited the building");
=======
            
>>>>>>> DK-Branch
        }
    }
}