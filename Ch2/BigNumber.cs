using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ch2
{
    // Длинное число
    class BigNumber
    {
        // Список цифр
        private List<byte> digits = new List<byte>();
        // Длина числа
        public int Size => digits.Count;

        // Конструктор для создания больших чисел коллекцией
        public BigNumber(List<byte> bytes)
        {
            digits = bytes.ToList();
            RemoveNulls();
        }

        // Конструктор для создания больших чисел из строки
        public BigNumber(string s)
        {
            foreach (var c in s.Reverse())
            {
                digits.Add(Convert.ToByte(c.ToString()));
            }

            RemoveNulls();
        }

        // Конструктор для создания больших чисел из инта
        public BigNumber(int x)
        {
            digits.AddRange(GetBytes((uint)Math.Abs(x)));
        }

        // Метод для преобразования числа в список цифр
        private List<byte> GetBytes(uint num)
        {
            List<byte> bytes = new List<byte>();
            while (num > 0)
            {
                bytes.Add((byte)(num % 10));
                num /= 10;
            }

            return bytes;
        }

        // Метод для удаления лидирующих нулей длинного числа>
        private void RemoveNulls()
        {
            for (int i = digits.Count - 1; i > 0; i--)
            {
                if (digits[i] == 0)
                {
                    digits.RemoveAt(i);
                }
                else
                {
                    break;
                }
            }
        }

        // Получение цифры по индексу
        // Если индекс больше длины возвращает 0
        public byte GetByte(int i) => i < Size ? digits[i] : (byte)0;

        //установка цифры по индексу
        public void SetByte(int i, byte b)
        {
            while (digits.Count <= i)
            {
                digits.Add(0);
            }

            digits[i] = b;
        }

        // Метод для получения больших чисел в степени
        public static BigNumber Exp(byte val, int exp)
        {
            var bigInt = new BigNumber(0);
            bigInt.SetByte(exp, val);
            bigInt.RemoveNulls();
            return bigInt;
        }

        // Преобразование длинного числа в строку
        public override string ToString()
        {
            var s = new StringBuilder("");

            for (int i = digits.Count - 1; i >= 0; i--)
            {
                s.Append(Convert.ToString(digits[i]));
            }

            return s.ToString();
        }

        // Сравнение по длине
        private static int CompareSize(BigNumber a, BigNumber b)
        {
            if (a.Size < b.Size)
            {
                return -1;
            }
            else if (a.Size > b.Size)
            {
                return 1;
            }

            return CompareDigits(a, b);
        }

        // Сравнение по цифрам, если размере одинаковый
        private static int CompareDigits(BigNumber a, BigNumber b)
        {
            int maxLength = Math.Max(a.Size, b.Size);
            for (int i = maxLength; i >= 0; i--)
            {
                if (a.GetByte(i) < b.GetByte(i))
                {
                    return -1;
                }
                else if (a.GetByte(i) > b.GetByte(i))
                {
                    return 1;
                }
            }

            return 0;
        }

        // Перегрузка операторов сравнения
        public static bool operator <(BigNumber a, BigNumber b) => CompareSize(a, b) < 0;

        public static bool operator >(BigNumber a, BigNumber b) => CompareSize(a, b) > 0;

        public static bool operator <=(BigNumber a, BigNumber b) => CompareSize(a, b) <= 0;

        public static bool operator >=(BigNumber a, BigNumber b) => CompareSize(a, b) >= 0;

        public static bool operator ==(BigNumber a, BigNumber b) => CompareSize(a, b) == 0;

        public static bool operator !=(BigNumber a, BigNumber b) => CompareSize(a, b) != 0;

        // Сложение
        public static BigNumber operator +(BigNumber a, BigNumber b)
        {
            // Используется метод вычисления в столбик
            List<byte> digits = new List<byte>();

            int maxLength = Math.Max(a.Size, b.Size);

            byte t = 0;
            for (int i = 0; i < maxLength; i++)
            {
                byte sum = (byte)(a.GetByte(i) + b.GetByte(i) + t);
                if (sum > 10)
                {
                    sum -= 10;
                    t = 1;
                }
                else
                {
                    t = 0;
                }
                digits.Add(sum);
            }

            if (t > 0)
            {
                digits.Add(t);
            }

            return new BigNumber(digits);
        }

        // Вычитание
        public static BigNumber operator -(BigNumber a, BigNumber b)
        {
            // Вычисление разности больших чисел реализуется подобно сложению 
            // При этом числа сравниваются и от большего вычитается меньшее
            List<byte> digits = new List<byte>();

            BigNumber max = new BigNumber(0);
            BigNumber min = new BigNumber(0);

            // Сравниваем числа
            int compare = CompareSize(a, b);

            switch (compare)
            {
                case -1:
                    min = a;
                    max = b;
                    break;
                case 0:
                    // Если числа равны возвращаем 0
                    return new BigNumber(0);
                case 1:
                    min = b;
                    max = a;
                    break;
            }

            // Из большего вычитаем меньшее
            int maxLength = Math.Max(a.Size, b.Size);

            int t = 0;
            for (int i = 0; i < maxLength; i++)
            {
                int s = max.GetByte(i) - min.GetByte(i) - t;
                if (s < 0)
                {
                    s += 10;
                    t = 1;
                }
                else
                {
                    t = 0;
                }

                digits.Add((byte)s);
            }

            return new BigNumber(digits);
        }

        public static BigNumber operator *(BigNumber a, BigNumber b)
        {
            // Каждая цифра числа перемножается на цифру другого с последующим суммированием
            BigNumber retValue = new BigNumber(0);

            for (var i = 0; i < a.Size; i++)
            {
                for (int j = 0, carry = 0; (j < b.Size) || (carry > 0); j++)
                {
                    var cur = retValue.GetByte(i+j) + a.GetByte(i) * b.GetByte(j) + carry;
                    retValue.SetByte(i + j, (byte)(cur % 10));
                    carry = cur / 10;
                }
            }

            return retValue;
        }

        // Деление
        public static BigNumber operator /(BigNumber a, BigNumber b)
        {
            var retValue = new BigNumber(0);
            var curValue = new BigNumber(0);

            for (var i = a.Size - 1; i >= 0; i--)
            {
                curValue += Exp(a.GetByte(i), i);

                var x = 0;
                var l = 0;
                var r = 10;
                while (l <= r)
                {
                    var m = (l + r) / 2;
                    var cur = b * Exp((byte)m, i);
                    if (cur <= curValue)
                    {
                        x = m;
                        l = m + 1;
                    }
                    else
                    {
                        r = m - 1;
                    }
                }

                retValue.SetByte(i, (byte)(x % 10));
                var t = b * Exp((byte)x, i);
                curValue = curValue - t;
            }

            retValue.RemoveNulls();

            return retValue;
        }

        // Вычисление остатка от деления
        public static BigNumber operator %(BigNumber a, BigNumber b)
        {
            // Деление одного большого числа на другое реализовано путем поиска наибольшего числа, 
            // результат умножения которого на второе число, ближе всего к первому
            BigNumber retValue = new BigNumber(0);

            for (int i = a.Size - 1; i >= 0; i--)
            {
                retValue += Exp(a.GetByte(i), i);

                int x = 0;
                int l = 0;
                int r = 10;

                while (l <= r)
                {
                    int m = (l + r) >> 1;
                    var cur = b * Exp((byte)m, i);
                    if (cur <= retValue)
                    {
                        x = m;
                        l = m + 1;
                    }
                    else
                    {
                        r = m - 1;
                    }
                }

                retValue -= b * Exp((byte)x, i);
            }

            retValue.RemoveNulls();

            return retValue;
        }
    }
}
