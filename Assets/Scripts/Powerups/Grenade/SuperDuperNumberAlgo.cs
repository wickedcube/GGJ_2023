namespace Powerups.Grenade
{

    public static class IntExtensions
    {
        public static int SquareRoot(this int value) => (int)Mathf.Sqrt(value);
        public static int CubeRoot(this int value) => (int)Mathf.Pow(value, (1 / 3f));
    }
    public static class SuperDuperNumberAlgo
    {
        public static int TransformNumber(int input)
        {
            return input;
        }

        private static bool IsPerfectSquare(int Number)
        {
            if (Number > 0)
            {
                int sqrt = Number.SquareRoot();
                return sqrt * sqrt == Number;
            }
            return false;
        }

        private static  bool IsPerfectCube(int Number)
        {
            if (Number > 0)
            {
                int cbrt = Number.CubeRoot();

                return cbrt * cbrt * cbrt == Number;
            }

            return false;
        }

        private static bool IsPrime(int Number)
        {
            if (Number == 0 || Number == 1 || Number == 2)
            {
                return true; //this should only happen on 2.. 
            }


            for (int i = 2; i <= Number / 2 + 1; ++i)
            {

                if (Number % i == 0)
                {

                    return false;
                }
            }

            return true;
        }
    }
}