using UnityEngine;
using System.Collections.Generic;

namespace Powerups.Grenade
{

    public static class IntExtensions
    {
        public static Dictionary<int, int> SquaresCache = new Dictionary<int, int>();

        public static Dictionary<int, int> CubesCache = new Dictionary<int, int>();
        public static int SquareRoot(this int value)
        {
            if (SquaresCache.ContainsKey(value)) return SquaresCache[value]; 
            
            var sqrt  = (int)Mathf.Sqrt(value);
            SquaresCache[value] = sqrt;
            return sqrt;
        }
        public static int CubeRoot(this int value)
        {
            if (CubesCache.ContainsKey(value)) return CubesCache[value];

            var cbrt = (int)Mathf.Pow(value, (1 / 3f));
            CubesCache[value] = cbrt;
            return cbrt; 
        }
       
    }
    public static class NumberAlgorithms
    {
        public static bool IsPerfectSquare(int Number)
        {
            if (Number > 0)
            {
                int sqrt = Number.SquareRoot();
                return sqrt * sqrt == Number;
            }
            return false;
        }

        public static  bool IsPerfectCube(int Number)
        {
            if (Number > 0)
            {
                int cbrt = Number.CubeRoot();

                return cbrt * cbrt * cbrt == Number;
            }

            return false;
        }


        private static Dictionary<int, bool> PrimeCache = new Dictionary<int, bool>();
        public static bool IsPrime(int Number)
        {
            if (PrimeCache.ContainsKey(Number)) return PrimeCache[Number];
            if (Number == 0 || Number == 1 || Number == 2)
            {
                PrimeCache[Number] = true;
                return true; //this should only happen on 2.. 
            }


            for (int i = 2; i <= Number / 2 + 1; ++i)
            {

                if (Number % i == 0)
                {
                    PrimeCache[Number] = false;
                    return false;
                }
            }
            PrimeCache[Number] = true;
            return true;
        }
    }
}