//#define Vojta


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cuni.Arithmetics.FixedPoint
{

#if Vojta
    public abstract class FixedNumber
    {
        public abstract int ShiftAmount { get; }
    }

    public class Q24_8 : FixedNumber
    {
        public override int ShiftAmount { get => 8; }
    }
    public class Q16_16 : FixedNumber
    {
        public override int ShiftAmount { get => 16; }
    }
    public class Q8_24 : FixedNumber
    {
        public override int ShiftAmount { get => 24; }
    }


    public class Fixed<T> where T : FixedNumber, new()
    {

        private static int ShiftAmount;
        private int value;

        public Fixed(int value)
        {
            ShiftAmount = (new T()).ShiftAmount;

            // owerflow detection
            if (value >= (1 << (31 - ShiftAmount)))
            {
                //Console.WriteLine("Overflow detected. This value is to high: " + value + " maximum is : " + ((1 << (31 - ShiftAmount))-1));
                throw new OverflowException("This value is to high: " + value);
            }
            if (value < (1 << (31 - ShiftAmount)) * (-1))
            {
                //Console.WriteLine("Overflow detected. This value is to low: " + value + " maximum is : " + ((1 << (31 - ShiftAmount))));
                throw new OverflowException("This value is to low: " + value);
            }
            this.value = value << ShiftAmount;
        }

        public Fixed<T> Add(Fixed<T> x)
        {
            int tmp = value + x.value;
            return new Fixed<T>(0) { value = tmp };
        }

        public Fixed<T> Subtract(Fixed<T> x)
        {
            int tmp = value - x.value;
            return new Fixed<T>(0) { value = tmp };
        }

        public Fixed<T> Multiply(Fixed<T> x)
        {
            Int64 tmp = (Int64)value * (Int64)x.value;
            tmp = tmp >> ShiftAmount;
            return new Fixed<T>(0) { value = (int)tmp };
        }

        public Fixed<T> Divide(Fixed<T> x)
        {
            Int64 tmpa = (Int64)value << ShiftAmount;
            if (x.value == 0)
            {
                throw new DivideByZeroException();
            }
            double tmp = (double)tmpa / x.value;
            for (int i = 0; i < ShiftAmount; i++)
            {
                tmp = 2 * tmp;
            }
            Int64 tmpb = Convert.ToInt64(tmp);
            tmp = tmpb >> ShiftAmount;
            return new Fixed<T>(0) { value = (int)tmp };

        }


        public override string ToString()
        {
            double tmp = value;
            for (int i = 0; i < ShiftAmount; i++)
            {
                tmp = tmp / 2;
            }
            return (tmp).ToString();
        }
    }


   #else

     public abstract class QFormat
    {
        public abstract int GetNumOfFractionalBits();
    }

    public class Q24_8 : QFormat
    {
        public override int GetNumOfFractionalBits() => 8;
    }
    public class Q16_16 : QFormat
    {
        public override int GetNumOfFractionalBits() => 16;
    }
    public class Q8_24 : QFormat
    {
        public override int GetNumOfFractionalBits() => 24;
    }


    public struct Fixed<T> where T : QFormat, new()
    {
        private static int FractionalBitsCount;
        static Fixed()
        {
            FractionalBitsCount = (new T()).GetNumOfFractionalBits();
        } 

        private int theNumber;

        public Fixed(int number)
        {
            theNumber = number << FractionalBitsCount;
        }

        public Fixed<T> Add(Fixed<T> num)
        {
            return new Fixed<T>() { theNumber = theNumber + num.theNumber };
        }

        public Fixed<T> Subtract(Fixed<T> num)
        {
            return new Fixed<T>() { theNumber = theNumber - num.theNumber };
        }

        public Fixed<T> Multiply(Fixed<T> num)
        {
            long result = ((long)theNumber * (long)num.theNumber) >> FractionalBitsCount;

            return new Fixed<T>() { theNumber = (int)result };
        }

        public Fixed<T> Divide(Fixed<T> num)
        {
            long result = ((long)theNumber << FractionalBitsCount) / num.theNumber;

            return new Fixed<T>() { theNumber = (int)result };
        }

        public static implicit operator Fixed<T>(int num) => new Fixed<T>(num);

        public override string ToString() => ((double)theNumber / (1 << FractionalBitsCount)).ToString();
    }
#endif

}
