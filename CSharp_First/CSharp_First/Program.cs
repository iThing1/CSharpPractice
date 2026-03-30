using System;

class Calculator
{
    static void Main()
    {

        Console.Write("첫 번째 숫자를 입력하세요: ");
        string inputFirst = Console.ReadLine();
        if (!double.TryParse(inputFirst, out double numFirst))
        {
            Console.WriteLine("오류: 올바른 숫자가 아닙니다.\n");
            return;
        }

        Console.Write("연산자를 입력하세요 (+, -, *, /): ");
        string selectedOp = Console.ReadLine();

        Console.Write("두 번째 숫자를 입력하세요: ");
        string inputSecond = Console.ReadLine() ?? "";
        if (!double.TryParse(inputSecond, out double numSecond))
        {
            Console.WriteLine("오류: 올바른 숫자가 아닙니다.\n");
            return;
        }

    }
}