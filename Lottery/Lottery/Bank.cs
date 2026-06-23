using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lottery20
{
    // Till SwissBank's swissBankAccount överförs 25% av intäkterna från spelade lottorader. 
    public abstract class Bank
    {
        static double bankAccount;
    }
    public class SwissBank : Bank
    {
        public static double swissBankAccount;
    }
}
