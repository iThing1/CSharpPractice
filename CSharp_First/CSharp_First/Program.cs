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

        double result = 0;
        bool success = true;
        switch (selectedOp)
        {
            case "+": result = numFirst + numSecond; break;
            case "-": result = numFirst - numSecond; break;
            case "*": result = numFirst * numSecond; break;
            case "/":
                if (numSecond != 0) result = numFirst / numSecond;
                else Console.WriteLine("오류: 0으로 나눌 수 없습니다.\n"); success = false;
                break;
            default:
                Console.WriteLine("오류: 지원하지 않는 연산자입니다.\n"); success = false;
                break;
        }

        if (success)
        {
            Console.WriteLine($"결과: {numFirst} {selectedOp} {numSecond} = {result}");
        }
    }
}