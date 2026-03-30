using System;
using System.Text;

class Calculator
{
    static void Main()
    {
        bool isEnd = false;
        StringBuilder resultgroup = new StringBuilder();

        resultgroup.AppendLine("------ [계산 로그 기록] ------");
        Console.WriteLine("==========================");
        Console.WriteLine("|     C# 미니 계산기      |");
        Console.WriteLine("==========================");
        Console.WriteLine("Q: 종료    /    L: 로그 보기\n");
       
        while (!isEnd)
        {      
            Console.Write("첫 번째 숫자를 입력하세요: ");
            string inputFirst = Console.ReadLine().ToLower();
            if (inputFirst == "q") break;
            if (inputFirst == "l")
            {
                Console.WriteLine($"\n{resultgroup.ToString()}");
                continue;
            }

            if (!double.TryParse(inputFirst, out double numFirst))
            {
                Console.WriteLine("오류: 올바른 숫자가 아닙니다.\n");
                continue;
            }  

            Console.Write("두 번째 숫자를 입력하세요: ");
            string inputSecond = Console.ReadLine().ToLower();
            if (inputSecond == "q") break;
            if (inputSecond == "l")
            {
                Console.WriteLine($"\n{resultgroup.ToString()}");
                continue;
            }
            if (!double.TryParse(inputSecond, out double numSecond))
            {
                Console.WriteLine("오류: 올바른 숫자가 아닙니다.\n");
                continue;
            }

            Console.Write("연산자를 입력하세요 (+, -, *, /): ");
            string selectedOp = Console.ReadLine();
            if (selectedOp.ToLower() == "q") break;

            double result = 0;
            bool success = true;
            switch (selectedOp)
            {
                case "+": result = numFirst + numSecond; break;
                case "-": result = numFirst - numSecond; break;
                case "*": result = numFirst * numSecond; break;
                case "/":
                    if (numSecond != 0) result = numFirst / numSecond;
                    else Console.WriteLine("오류: 0으로 나눌 수 없습니다.\n");
                    success = false;
                    break;
                default:
                    Console.WriteLine("오류: 지원하지 않는 연산자입니다.\n"); 
                    success = false;
                    break;
            }

            if (success)
            {
                string logEntry = $"{numFirst} {selectedOp} {numSecond} = {result}";
                Console.WriteLine($"결과: {result}");
                resultgroup.AppendLine(logEntry);
            }
        }
        Console.WriteLine("\n계산기를 종료합니다...");
    }
}