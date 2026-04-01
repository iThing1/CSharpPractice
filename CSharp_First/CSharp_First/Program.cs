using System;
using System.Collections.Specialized;

namespace CSharp_First
{
    public abstract class InteractableObject
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public int ReqItemID { get; private set; } 
        public int DropItemID { get; private set; }
        public bool IsLocked { get; set; }

        protected InteractableObject(string name, string desc, int reqId = 0, int dropId = 0, bool locked = false)
        {
            Name = name;
            Description = desc;
            ReqItemID = reqId;
            DropItemID = dropId;
            IsLocked = locked;
        }

        public abstract void Interact();
    }
    public class Room
    {
        public string Name { get; private set; }
        public List<InteractableObject> objectList { get; set; } = new List<InteractableObject>();

        public Room(string name)
        {
            Name = name;
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Room[] room = new Room[4];
            room[0] = new Room("출입구");
            room[1] = new Room("창문쪽");
            room[2] = new Room("침대쪽");
            room[3] = new Room("옷장쪽");

            int currentIndex = 0;
            while (true)
            {
                Console.Clear();
                Console.WriteLine("==============================");
                Console.WriteLine($"     현재 위치: [{room[currentIndex].Name}]");
                Console.WriteLine("==============================");
                Console.WriteLine("이동 방향을 선택하세요");
                Console.WriteLine("1. 왼쪽(<-)  2. 오른쪽(->)  0. 종료");
                Console.Write("입력: ");

                int selected;
                if (!int.TryParse(Console.ReadLine(), out selected))
                {
                    Console.WriteLine("잘못된 입력입니다.");
                    continue;
                }

                if (selected == 0) break;
                if (selected == 2)
                {
                    currentIndex++;
                    if (currentIndex > 3) currentIndex = 0;
                }
                else if (selected == 1)
                {
                    currentIndex--;
                    if (currentIndex < 0) currentIndex = 3;
                }
            }
        }
    }
}
