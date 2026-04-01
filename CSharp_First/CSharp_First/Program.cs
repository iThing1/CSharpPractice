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
            Console.WriteLine($"[System]{Name}을(를) 조사합니다.");
            if (IsLocked)
            {
                Console.WriteLine($"{itemMgr.GetItemName(ReqItemID)}가 필요할 것 같다.");
                if (player.HasItem(ReqItemID))
                {
                    Unlock(); 
                    player.Inventory.Remove(ReqItemID);
                    Console.WriteLine($"[System]{itemMgr.GetItemName(ReqItemID)}이/가 사라졌습니다.");
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
                Console.WriteLine("금고가 열렸다!!.");

                DropItem();
            }
        }

        public void DropItem()
        {
            string droppedItemName = itemMgr.GetItemName(DropItemID);
            Console.WriteLine($"금고 안에서 [{droppedItemName}]을(를) 발견했다!");
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
        public enum GameState {
            None,
            Start, 
            Running, 
            Exit 
        }
        public GameState CurrentState { get; private set; }

        private Room[] mRooms;
        private int mCurrentRoomIndex = 0;
        public Player Player { get; private set; }
        public bool IsRunning { get; private set; } = false;

        public GameManager()
        {
            Player = new Player();
            CurrentState = GameState.None;
        }

        public void ChangeState(GameState state)
        {
            CurrentState = state;
            switch (state)
            {
                case GameState.Start:
                    InitGame(); break;
                case GameState.Running:
                    RunGame(); break;
                case GameState.Exit: 
                    ExitGame();break;
            }
        }

        public void InitGame()
        {
            Console.WriteLine("Loading......");
            mRooms = new Room[4];
            mRooms[0] = new Room("방문");
            mRooms[1] = new Room("옷장이 보이는 벽면");
            mRooms[2] = new Room("창문이 있는 벽");
            mRooms[3] = new Room("침대가 있는 쪽");
            
            // 여기서부터 공간안에 들어갈 물체를 생성 및 배치
            mRooms[1].objectList.Add(new SafeBox());

            Console.WriteLine("게임 시작");
            ChangeState(GameState.Running);
        }

        public void RunGame()
        {
            IsRunning = true;
            while (IsRunning)
            {
                Console.Clear();
                Console.WriteLine($"--- 현재 위치: {mRooms[mCurrentRoomIndex].Name} ---");
                Console.WriteLine("무엇을 할까?");
                Console.WriteLine("1. 이동  2. 조사  0. 종료");
                if (!int.TryParse(Console.ReadLine(), out int selectNum))
                {
                    Console.WriteLine("숫자만 입력 가능합니다.");
                    Console.ReadKey();
                    continue;
                }

                switch (selectNum)
                {
                    case 1: // 이동 로직
                        Console.WriteLine("1. 왼쪽  2. 오른쪽");
                        if (int.TryParse(Console.ReadLine(), out int dirNum))
                        {
                            if (dirNum == 1) MoveRoom("left");
                            else if (dirNum == 2) MoveRoom("right");
                        }
                        break;

                    case 2: // 조사 로직
                        var currentObjects = mRooms[mCurrentRoomIndex].objectList;
                        if (currentObjects.Count == 0)
                        {
                            Console.WriteLine("여기는 조사할 만한 것이 없다.");
                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine("눈에 들어오는 몇개의 물건이 있다.");
                            for (int i = 0; i < currentObjects.Count; i++)
                            {
                                Console.WriteLine($"{i+1}. {currentObjects[i].Name}");
                            }
                            Console.WriteLine("----------------------------------");
                            Console.Write("어떤 것을 조사해볼까?  (0: 뒤로가기) ");
                            if (int.TryParse(Console.ReadLine(), out int inputNum))
                            {
                                if (inputNum == 0) break;

                                int targetIndex = inputNum - 1;
                                if (targetIndex >= 0 && targetIndex < currentObjects.Count)
                                {
                                    Console.Clear();
                                    currentObjects[targetIndex].Interact(Player);
                                    break;
                                }
                                Console.WriteLine("목록에 없는 번호입니다.");
                                return;
                            }
                            Console.WriteLine("숫자만 입력해 주세요.");
                        }
                        break;
                    case 0:
                        ChangeState(GameState.Exit);
                        IsRunning = false;
                        return;
                }
                Console.WriteLine("\n(계속하려면 아무 키나 누르세요)");
                Console.ReadKey();
            }       
        }

        public void ExitGame()
        {

        }

        public void MoveRoom(string dir)
        {
            int dirToInt = 0;
            if (dir == "left") { dirToInt = 1; }
            else if (dir == "right") { dirToInt = 2; }
            // 순환형 이동로직 선택
            mCurrentRoomIndex = (dirToInt == 1) ? mCurrentRoomIndex - 1 : mCurrentRoomIndex + 1;
            if (mCurrentRoomIndex > 3) mCurrentRoomIndex = 0;
            else if (mCurrentRoomIndex < 0) mCurrentRoomIndex = 3;
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            GameManager gm = new GameManager();
            gm.InitGame();


        }
    }
}
