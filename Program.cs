using System;

namespace KG_6
{
    class Program
    {
        static void Main(string[] args)
        {
            using (Game g = Game.Create())
            {
                g.Run();
            }
        }
    }
}
