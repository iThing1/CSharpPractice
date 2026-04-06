using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace CSharp_First
{
    // 4. Stack
    // 후입선출(LIFO, Last-In First-Out) 방식
    // UI창 열고 닫기
    public abstract class UIPopUp
    {
        public abstract void Show();
    }

    public class MapUI : UIPopUp
    {
        public override void Show() => Console.WriteLine("나는 지도창");
    }

    public class StatusUI : UIPopUp
    {
        public override void Show() => Console.WriteLine("나는 상태창");
    }

    public class InventoryUI : UIPopUp
    {
        public override void Show() => Console.WriteLine("나는 인벤토리");
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Stack<UIPopUp> uiStack = new Stack<UIPopUp>();

            UIPopUp inventory = new InventoryUI();
            UIPopUp status = new StatusUI();
            UIPopUp map = new MapUI();

            Console.WriteLine("==== UI 창 열기 ====");
            uiStack.Push(inventory);
            inventory.Show();
            uiStack.Push(status);
            status.Show();
            uiStack.Push(map);
            map.Show();

            Console.WriteLine();
            Console.WriteLine("==== 현재 맨 위의 창 확인 ====");
            if (uiStack.Count > 0)
            {
                uiStack.Peek().Show();
                Console.WriteLine();
            }

            Console.WriteLine("뒤로가기 버튼 클릭!!");
            uiStack.Pop();
            Console.WriteLine();

            Console.WriteLine("==== 현재 활성화된 창 ====");
            uiStack.Peek().Show();
        }
    }
}

