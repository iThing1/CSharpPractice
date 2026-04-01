using System;

namespace CSharp_First
{
    public enum ItemData
    {
        None = 0,
        OldKey = 100,
        Knife = 200,
        Note = 300,
    }

    public class Item
    {
        public string GetItemName(ItemData item)
        {
            switch (item)
            {
                case ItemData.OldKey: 
                    return "낡은 열쇠";
                case ItemData.Knife: 
                    return "칼";
                case ItemData.Note:
                    return "1111이 적힌 쪽지";
                default:
                    return "None";
            }
        }
    }
    public abstract class InteractableObject
    {
        public string Name { get; protected set; }
        public string Description { get; protected set; }

        protected InteractableObject(string name, string desc)
        { 
            Name = name;
            Description = desc;
        }

        public abstract void Interact(Player player);
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

    public interface IItemDropable
    {
        public void DropItem();
    }

    public interface ILockable
    {
        public void Unlock();
    }

    public class SafeBox : InteractableObject, IItemDropable, ILockable
    {
        public ItemData ReqItemID { get; private set; }
        public ItemData DropItemID { get; private set; }
        public bool IsLocked { get; private set; } = true;

        private Item itemMgr = new Item();

        public SafeBox() : base ("금고", "단단히 잠겨져있는 금고이다.")
        {
            ReqItemID = ItemData.OldKey;
            DropItemID = ItemData.Note;
        }   

        public override void Interact(Player player)
        {
            Console.WriteLine($"[{Name}]을(를) 조사합니다.");
            if (IsLocked)
            {
                Console.WriteLine($"{itemMgr.GetItemName(ReqItemID)}가 필요할 것 같다.");
                if (player.HasItem(ReqItemID))
                {
                    Unlock(); 
                    player.Inventory.Remove(ReqItemID);
                    Console.WriteLine($"[{itemMgr.GetItemName(ReqItemID)}]가 사라졌습니다.");
                }
                else Console.WriteLine("하지만 열쇠가 없다. 다른 곳을 더 찾아보자.");
                return;
            }
            Console.WriteLine("이미 열려있는 금고이다.");
        }

        public void Unlock()
        {
            if (IsLocked)
            {
                IsLocked = false;
                Console.WriteLine("덜컥! 금고가 열렸습니다.");

                DropItem();
            }
        }

        public void DropItem()
        {
            string droppedItemName = itemMgr.GetItemName(DropItemID);
            Console.WriteLine($"금고 안에서 [{droppedItemName}]을(를) 발견했습니다!");
        }
    }

    public class Player
    {
        public List<ItemData> Inventory { get; private set; } = new List<ItemData>();

        public bool HasItem(ItemData item)
        {
            return Inventory.Contains(item);
        }

        public void AddItem(ItemData item)
        {
            Inventory.Add(item);
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Player player = new Player();
            SafeBox safe = new SafeBox();

            Console.WriteLine("=== 시나리오 1: 빈손으로 금고 조사 ===");
            safe.Interact(player);

            Console.WriteLine("\n--------------------------------------");
            Console.WriteLine($">> 시스템: {ItemData.OldKey}를 획득했습니다.");
            Console.WriteLine("--------------------------------------");

            Console.WriteLine("\n=== 시나리오 2: 열쇠를 가지고 금고 조사 ===");
            player.AddItem(ItemData.OldKey);
            safe.Interact(player);

            Console.WriteLine("\n=== 시나리오 3: 이미 열린 금고 재조사 ===");
            safe.Interact(player);

            Console.ReadKey();
        }
    }
}
