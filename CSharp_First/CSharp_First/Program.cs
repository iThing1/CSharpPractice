using System;
using System.Collections.Generic;

namespace CSharp_First
{
    // 자료구조의 간단한 사용법에 대한 코드
    public class Item
    {
        public int mID;
        public string mName;

        public Item(int id, string n)
        {
            mID = id;
            mName = n;
        }
        public override string ToString()
        {
            return $"[{mID}] {mName}";
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            // 1. List
            // 단순 나열, 인덱스 접근시에 사용: CPU 캐시 효율이 가장 좋음(가장 빠름)
            List<Item> inventory = new List<Item>();
            // 생성된 아이템을 인벤토리에 추가
            Item redPotion = new Item(100, "빨간 포션");
            inventory.Add(redPotion);

            // 인벤토리 내에서 생성도 가능
            inventory.Add(new Item(200, "주황 포션"));
            inventory.Add(new Item(300, "하얀 포션"));
            Console.WriteLine("====아이템 생성 후 인벤토리====");
            ShowInventory(inventory);
            Console.WriteLine();

            // 인벤토리 내에 삽입
            inventory.Insert(1, new Item(400, "초록 포션"));
            Console.WriteLine("====초록 포션 추가 후 인벤토리====");
            ShowInventory(inventory);
            Console.WriteLine();
            // 인벤토리 내의 아이템 삭제
            if (inventory.Contains(redPotion))
            {
                inventory.Remove(redPotion);
                Console.WriteLine("====빨간 포션 삭제 후 인벤토리====");
                ShowInventory(inventory);
                Console.WriteLine();
            }

            inventory.RemoveAt(0);
            Console.WriteLine("====인덱스0번 삭제 후 인벤토리====");
            ShowInventory(inventory);
            Console.WriteLine();

            inventory.Clear();
            Console.WriteLine("====데이터 삭제 후 인벤토리====");
            ShowInventory(inventory);
            Console.WriteLine();

        }
        // 매우 매우 매우 매우 중요
        // 제네릭 문법을 활용하여 인벤토리 목록을 보여주는 메서드
        // 이후 아이템을 상속받는 Weapon, Potion, Material 등등의 리스트도 함께 처리가능
        // 아이템이 아닌 Monster가 들어온다면? 즉시 에러 발생(박싱&언박싱이 없음)
        // 재사용성 극대화, 타입 안정성 확보, 유지보수 편함
        public static void ShowInventory<T>(List<T> list) where T : Item
        {
            if (list.Count == 0)
            {
                Console.WriteLine("인벤토리가 비어 있습니다."); return;
            }

            foreach (T item in list)
            {
                Console.WriteLine($"아이템: {item.mName} (ID: {item.mID})");
            }
        }
    }
}
