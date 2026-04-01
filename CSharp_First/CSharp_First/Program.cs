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

    public class GameManager
    {
        private Room[] mRooms;
        private int mCurrentRoomIndex = 0;
        public Player Player { get; private set; }
        public bool IsRunning { get; private set; } = true;

        public GameManager()
        {
            Player = new Player();
            InitGame();
        }

        public void InitGame()
        {
            mRooms = new Room[4];
            mRooms[0] = new Room("방문");
            mRooms[1] = new Room("옷장이 보이는 벽면");
            mRooms[2] = new Room("창문이 있는 벽");
            mRooms[3] = new Room("침대가 있는 쪽");
            
            // 여기서부터 공간안에 들어갈 물체를 생성 및 배치
            // 예시: mRoom[1].ObjectList.Add(new SafeBox());
        }

        public void MoveRoom(int dir)
        {
            // 순환형 이동로직 선택
            int nextIndex = (dir == 1) ?  mCurrentRoomIndex - 1 : mCurrentRoomIndex + 1;
            if (nextIndex > 3) nextIndex = 0;
            else if (nextIndex < 0) nextIndex = 3;
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            
        }
    }
}
