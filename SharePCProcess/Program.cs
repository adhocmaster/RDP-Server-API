using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading;

namespace SharePC
{
    class Program
    {
        static void Main(string[] args)
        {
           
           
            SharePC pc1 = new SharePC();
            while (true) {
                String a1 = Console.ReadLine();
                if (a1 == "1")
                {
                    pc1.triggerResolutionSwitch();
                    
                }
                else {
                    continue;
                }
            }
           


            String invitation = pc1.GetControlString();

            Console.WriteLine(invitation);
            
            //String line1 = Console.ReadLine();

            //pc1.destroy();

            
            //String invitation2 = pc1.GetControlString();
            //Console.WriteLine(invitation2);

        

            //String line2 = Console.ReadLine();

            //pc1.destroy();
            
            String line3 = Console.ReadLine();

        }
    }
}

            pc1.triggerResolutionSwitch();
           /* String invitation = pc1.GetControlString();
            Console.WriteLine(invitation);
            String line3 = Console.ReadLine();*/