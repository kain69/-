using System;

namespace Ch2
{
    class Program
    {
        static void Main(string[] args)
        {
            BigNumber bn1 = new BigNumber(100);
            BigNumber bn2 = new BigNumber(50);
            Console.WriteLine("A: " + bn1 + " B: " + bn2);

            string res;
            res = (bn1 + bn2).ToString();
            Console.WriteLine("Сумма: " + res);

            res = (bn1 - bn2).ToString();
            Console.WriteLine("Разность: " + res);

            res = (bn1 * bn2).ToString();
            Console.WriteLine("Умножение: " + res);

            res = (bn1 / bn2).ToString();
            Console.WriteLine("Деление: " + res);

            res = (bn1 % bn2).ToString();
            Console.WriteLine("Деление по модулю: " + res);

            bool bRes = bn1 > bn2;
            Console.WriteLine("Сравнение: " + bRes);

            Console.WriteLine(res);

            Console.ReadLine();
        }
    }
}
