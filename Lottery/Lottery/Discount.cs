using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lottery20
{
    // Discount-objekten ärver från den abstrakta klassen "Discount". De har metoder som returnerar en double som ticket-objekten sedan använder för att minska sitt 
    // pris med rätt värde. 
    abstract class Discount
    {
        internal abstract double ApplyDiscount();
    }
    class AgeDiscount : Discount
    {
        internal double ticketPrice;
        internal AgeDiscount(double price)
        {
            this.ticketPrice = price;
        }
        internal override double ApplyDiscount()
        {
            return ticketPrice - ticketPrice;
        }

    }
    class SubscriberDiscount : Discount
    {
        internal double ticketPrice;
        internal double discountValue = 2;

        internal SubscriberDiscount(double price)
        {
            this.ticketPrice = price;
        }
        internal override double ApplyDiscount()
        {
            return ticketPrice - discountValue;
        }
    }
}
