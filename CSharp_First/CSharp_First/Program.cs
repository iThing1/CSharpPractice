using System;
using System.Collections.Generic;

namespace CSharp_First
{
    // 2. Dictionary
    // key 값으로 빠르게 해당 데이터에 접근, List보다 메모리 많이 먹음.
    // ex) 도감 시스템
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
            Dictionary<int, Item> itemBook = new Dictionary<int, Item>();

            Item redPotion = new Item(100, "빨강 포션");
            Item bluePotion = new Item(200, "파란 포션");
            Item yellowPotion = new Item(300, "노란 포션");

            // Add: 새로운 Key와 데이터를 추가
            // 이미 있는 Key를 입력하면 에러 발생
            itemBook.Add(1, redPotion);
            itemBook.Add(2, bluePotion);
            //itemBook.Add(2, yellowPotion);    <-- 에러 발생!!
            itemBook.Add(3, yellowPotion);

            Console.WriteLine("==== 아이템 등록 완료 ====");
            ShowItemInfo(itemBook);
            Console.WriteLine();

            // ContainsKey & Remove: 키 존재 여부 확인 후 삭제
            if (itemBook.ContainsKey(1))
            {
                Console.WriteLine("ID 1번(빨강 포션)을 도감에서 삭제.");
                itemBook.Remove(1);
                ShowItemInfo(itemBook);
                Console.WriteLine();
            }

            // 3. TryGetValue: 안전하고 빠른 데이터 검색
            // 'out Item potion'에 검색 결과가 담김
            bool isFound = itemBook.TryGetValue(2, out Item? potion);
            if (potion != null && isFound)
            {
                Console.WriteLine($"아이템 찾기 성공: {potion.ToString()}");
            }
            else
            {
                Console.WriteLine("아이템을 찾을 수 없습니다.");
            }
        }

        // 제네릭 문법을 활용한 딕셔너리 출력 메서드
        public static void ShowItemInfo<T>(Dictionary<int, T> dict) where T : Item
        {
            if (dict == null || dict.Count == 0)
            {
                Console.WriteLine("도감이 비어 있습니다.");
                return;
            }

            foreach (var pair in dict)
            {
                Console.WriteLine($"도감 번호: {pair.Key} | 정보: {pair.Value}");
            }
        }
    }
}
