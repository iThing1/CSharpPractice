using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp_First
{
    // 3. Queue
    // 선입선출(FIFO, First-In First-Out) 방식
    // ex) 채팅 시스템 + stringBuilder 활용
    public class ChattingBox
    {
        public StringBuilder chatLog = new StringBuilder();
        private Queue<string> messageQueue = new Queue<string>();

        // 메세지 입력 메서드: Enqueue
        public void ReceiveMessage(string user, string msg)
        {
            string fullMsg = $"[{user}]: {msg}";
            messageQueue.Enqueue(fullMsg);
            chatLog.AppendLine(fullMsg);
        }

        // 메시지 출력 및 대기열에서 제거: Dequeue
        public void ProcessNextMessage()
        {
            if (messageQueue.Count > 0)
            {
                string m = messageQueue.Dequeue();
                Console.WriteLine($">>> 화면 출력: {m}");
            }
            else
            {
                Console.WriteLine("[System] 처리할 메시지가 없습니다.");
            }
        }

        // Peek: 대기열의 맨 앞 데이터를 제거하지 않고 확인만 함
        public void CheckNextMessage()
        {
            if (messageQueue.Count > 0)
            {
                string next = messageQueue.Peek();
                Console.WriteLine($"[System] 다음에 처리될 메시지: {next}");
            }
            else
            {
                Console.WriteLine("[System] 대기 중인 메시지가 없습니다.");
            }
        }

        public void ShowChattingLog()
        {
            Console.WriteLine("\n======== 세계 채팅 로그 ========");
            string log = chatLog.ToString();
            Console.WriteLine(string.IsNullOrEmpty(log) ? "(로그가 비어있습니다)" : log);
            Console.WriteLine("================================");
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            {
                ChattingBox worldChat = new ChattingBox();
                string myCharacter = "자료구조는 어려워";  // 테스트를 위해 추가

                Console.WriteLine("====== 실시간 세계 채팅 시스템 ======");
                Console.WriteLine("1: 메시지 입력 | 2: 메시지 처리(출력) | 3: 다음 메시지 미리보기 | 4: 전체 로그 보기 | 0: 종료");

                while (true)
                {
                    Console.Write("\n입력 > ");
                    string? input = Console.ReadLine();

                    if (input == "0") break;

                    switch (input)
                    {
                        case "1":
                            Console.Write("메시지 입력: ");
                            string? msg = Console.ReadLine();
                            if (!string.IsNullOrWhiteSpace(msg)) worldChat.ReceiveMessage(myCharacter, msg);
                            Console.WriteLine("-> 메시지가 큐에 등록되었습니다.");
                            break;
                        case "2":
                            worldChat.ProcessNextMessage();
                            break;
                        case "3":
                            worldChat.CheckNextMessage();
                            break;
                        case "4":
                            worldChat.ShowChattingLog();
                            break;
                        default:
                            Console.WriteLine("잘못된 입력입니다.");
                            break;
                    }
                }
                Console.WriteLine("프로그램을 종료합니다.");
            }
        }
    }
}