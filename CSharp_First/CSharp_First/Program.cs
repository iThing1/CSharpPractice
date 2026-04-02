using System;
using System.Collections.Generic;
using System.Threading;

namespace CSharp_First
{
    public enum ItemData
    {
        None = 0,
        OldKey = 100,
        CabinetKey = 101,
        Knife = 200,
        Note = 300,
    }
    public enum GameState
    {
        None,
        Start,
        Running,
        Exit
    }

    // 아이템을 static으로 선언
    // 현재 아이템 클래스는 아이템 이름을 반환하는 기능만 있음.
    public static class Item
    {
        public static string GetItemName(ItemData item)
        {
            switch (item)
            {
                case ItemData.OldKey: 
                    return "낡은 열쇠";
                    case ItemData.CabinetKey:
                    return "캐비닛 열쇠";
                case ItemData.Knife: 
                    return "칼";
                case ItemData.Note:
                    return "1111이 적힌 쪽지";
                default:
                    return "None";
            }
        }
    }
    // 추상 클래스
    // 상호작용 가능한 물체들의 공통된 속성과 기능을 정의
    public abstract class InteractableObject
    {
        public string mName { get; protected set; }
        public string Description { get; protected set; }

        protected InteractableObject(string name, string desc)
        { 
            mName = name;
            Description = desc;
        }
        // virtual로 변경시 공통 로직도 자식에서 변경할 수 있음
        // => Interact를 오버라이드 하고 base.interact()로 공통 로직 호출하는 형태로도 구현 가능
        // 코드 작성시 실수 방지를 위해 virtual이 아닌 그냥 일반 메서드로 구현
        public void Interact(Player player, GameManager gameMgr)
        {
            // 공통 로직은 여기에
            // 조사할 때 마다 출력되는 메시지나 효과 등을 이곳에서 처리
            Console.WriteLine("----------------------------------");
            Console.WriteLine($"[System]{mName}을(를) 조사합니다.");
            Console.WriteLine("----------------------------------\n");
            // 자식 클래스에서 구체적인 상호작용 로직을 구현하도록 추상 메서드 호출
            OnInteract(player, gameMgr);
        }
        protected abstract void OnInteract(Player player, GameManager gameMgr);
    }

    public interface IItemDropable
    {
        public void DropItem();
    }

    public interface ILockable
    {
        public void Unlock();
    }

    // ============= 물건 목록 ===================================================
    public class SafeBox : InteractableObject, IItemDropable, ILockable
    {
        public ItemData mReqItemID { get; private set; }
        public ItemData mDropItemID { get; private set; }
        public bool mIsLocked { get; private set; } = true;

        public SafeBox() : base ("금고", "단단히 잠겨져있는 금고이다.")
        {
            mReqItemID = ItemData.OldKey;
            mDropItemID = ItemData.Note;
        }

        protected override void OnInteract(Player player, GameManager gameMgr)
        {
            if (mIsLocked)
            {
                Console.WriteLine($"{Item.GetItemName(mReqItemID)}가 필요할 것 같다.");
                if (player.HasItem(mReqItemID))
                {
                    Unlock();
                    player.AddItem(mDropItemID);
                    player.Inventory.Remove(mReqItemID);
                    Console.WriteLine("----------------------------------");
                    Console.WriteLine($"[System]{Item.GetItemName(mReqItemID)}이/가 사라졌습니다.");
                    Console.WriteLine("----------------------------------\n");
                }
                else 
                    Console.WriteLine("하지만 열쇠가 없다. 다른 곳을 더 찾아보자.");
                return;
            }
            Console.WriteLine("이미 열려있는 금고이다.");
        }

        public void Unlock()
        {
            if (mIsLocked)
            {
                mIsLocked = false;
                Console.WriteLine("금고가 열렸다!!.");
                DropItem();
            }
        }

        public void DropItem()
        {
            Console.WriteLine($"금고 안에서 [{Item.GetItemName(mDropItemID)}]을(를) 발견했다!");
        }
    }

    public class Cabinet : InteractableObject, ILockable
    {
        public ItemData mReqItemID { get; private set; }
        public bool mIsLocked { get; private set; } = true;

        public Cabinet() : base("책상 캐비넷", "단단하게 잠겨진 책상 캐비넷이다.") 
        { mReqItemID = ItemData.CabinetKey; }

        protected override void OnInteract(Player player, GameManager gameMgr)
        {
            if (mIsLocked)
            {
                Console.WriteLine($"{Item.GetItemName(mReqItemID)}가 필요할 것 같다.");
                if (player.HasItem(mReqItemID))
                {
                    Unlock();
                    player.Inventory.Remove(mReqItemID);
                    Console.WriteLine("----------------------------------");
                    Console.WriteLine($"[System]{Item.GetItemName(mReqItemID)}이/가 사라졌습니다.");
                    Console.WriteLine("----------------------------------\n");
                }
                else
                    Console.WriteLine("하지만 열쇠가 없다. 다른 곳을 더 찾아보자.");
                return;
            }
            Console.WriteLine("이미 조사했던 캐비넷이다.");
        }
        public void Unlock()
        {
            if (mIsLocked)
            {
                mIsLocked = false;
                Console.WriteLine("캐비넷이 열렸다!!.");
                Console.WriteLine("안에 이상한 낙서 같은게 보인다!!");
                Console.WriteLine("[이 글은 영국에서부터 시작하여......]");
                Thread.Sleep(1000);
                Console.WriteLine("글의 뒷부분이 찢겨져있어서 더 이상 읽을 수 없다.");
            }
        }

    }

    public class Coat : InteractableObject, IItemDropable
    {
        private ItemData dropItemID = ItemData.OldKey;
        private bool hasItem = true;

        public Coat() : base("코트", "벽에 걸려있는 낡은 코트이다.") { }
        protected override void OnInteract(Player player, GameManager gameMgr)
        {
            if (hasItem)
            {
                Console.WriteLine("코트 주머니 속을 뒤져보니 무언가 잡힌다.");
                DropItem();
                player.AddItem(dropItemID);
                hasItem = false;
                return;
            }
            Console.WriteLine("이미 조사를 마친 코트이다. 더 이상 특별한 것은 없다.");
        }

        public void DropItem()
        {
            Console.WriteLine("----------------------------------");
            Console.WriteLine($"[System]코트 주머니에서 [{Item.GetItemName(dropItemID)}]을(를) 획득했습니다!");
            Console.WriteLine("----------------------------------\n");
        }
    }
    public class DoorLock : InteractableObject
    {
        private string mPassword = "1111"; // 탈출 비밀번호
        private bool mIsSolved = false;

        public DoorLock() : base("전자 도어락", "단단히 잠긴 철문에 달려있는 도어락이다.") { }

        protected override void OnInteract(Player player, GameManager gameMgr)
        {
            if (mIsSolved)
            {
                Console.WriteLine("이미 잠금이 해제되어 문이 열려 있습니다.");
                return;
            }

            Console.Write("비밀번호 4자리를 입력하세요 (0: 취소): ");
            string input = Console.ReadLine() ?? "";

            if (input == "0") return;

            if (input == mPassword)
            {
                mIsSolved = true;
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("<<<<< 삐비빅! 잠금이 해제되었습니다! >>>>>");
                Console.ResetColor();
                gameMgr.ChangeExitCondition(mIsSolved);
                gameMgr.ChangeState(GameState.Exit);
                Console.WriteLine("\n(계속하려면 아무 키나 누르세요)");
                Console.ReadKey();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("띠링! 비밀번호가 틀렸다.");
                Console.ResetColor();
            }
        }
    }

    public class Room
    {
        public string mName { get; private set; }
        public List<InteractableObject> mObjectList { get; set; } = new List<InteractableObject>();

        public Room(string name)
        {
            mName = name;
        }
    }

    public class Player
    {
        public List<ItemData> Inventory { get; private set; } = new List<ItemData>();

        public bool HasItem(ItemData item) => Inventory.Contains(item);
        public void AddItem(ItemData item) => Inventory.Add(item);
    }

    public class GameManager
    {
        private GameState _mCurrentState;
        public GameState mCurrentState
        {
            get => _mCurrentState;
            private set
            {
                _mCurrentState = value;
                if (_mCurrentState == GameState.Running)
                {
                    RunGame();
                }
                else if (_mCurrentState == GameState.Exit)
                {
                    ExitGame();
                }
            }
        }

        private Room[] mRooms;
        private int mCurrentRoomIndex = 0;
        public Player mPlayer { get; private set; }
        public bool mIsRunning { get; private set; } = false;
        public bool mIsCompleted { get; private set; } = false;

        public GameManager()
        {
            mPlayer = new Player();
            mCurrentState = GameState.None;
        }

        public void ChangeState(GameState state) => mCurrentState = state;
        public void ChangeExitCondition(bool IsCompleted) => mIsCompleted = true;
        public void InitGame()
        {      
            ShowTitle();
            // ShowStory();
            mRooms = new Room[4];
            mRooms[0] = new Room("방문");
            mRooms[1] = new Room("옷걸이대가 있는 벽");
            mRooms[2] = new Room("창문이 있는 벽");
            mRooms[3] = new Room("침대가 있는 쪽");

            // 여기서부터 공간안에 들어갈 물체를 생성 및 배치
            mRooms[0].mObjectList.Add(new DoorLock());
            mRooms[1].mObjectList.Add(new Coat());
            mRooms[1].mObjectList.Add(new Cabinet());
            mRooms[2].mObjectList.Add(new SafeBox());

            Console.WriteLine("게임 시작");
            ChangeState(GameState.Running);
        }

        public void RunGame()
        {
            mIsRunning = true; 
            while (mIsRunning)
            {
                if (mCurrentState == GameState.Exit)
                {
                    mIsRunning = false;
                    break; 
                }

                Console.Clear();
                Console.WriteLine($"--- 현재 위치: [{mRooms[mCurrentRoomIndex].mName}] ---");
                Console.WriteLine("무엇을 할까?");
                Console.WriteLine("1. 이동  2. 조사  3. 인벤토리  (0. 종료)");
                if (!int.TryParse(Console.ReadLine(), out int selectNum))
                {
                    Console.WriteLine("숫자만 입력 가능합니다.");
                    Console.ReadKey();
                    continue;
                }
                // 피드백 내용 반영
                if (selectNum < 0 || selectNum > 3)
                {
                    Console.WriteLine("잘못된 번호입니다.");
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
                        {
                            List<InteractableObject> currentObjects = mRooms[mCurrentRoomIndex].mObjectList;
                            if (currentObjects.Count == 0)
                            {
                                Console.WriteLine("여기는 조사할 만한 것이 없다.");
                                break;
                            }

                            Console.Clear();
                            Console.WriteLine("눈에 들어오는 몇개의 물건이 있다.");
                            for (int i = 0; i < currentObjects.Count; i++)
                            {
                                Console.WriteLine($"{i + 1}. {currentObjects[i].mName} : {currentObjects[i].Description}");
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
                                    currentObjects[targetIndex].Interact(mPlayer, this);
                                }
                                else
                                {
                                    Console.WriteLine("목록에 없는 번호입니다.");
                                }
                            }
                            else Console.WriteLine("숫자만 입력해 주세요.");
                        }
                        break;
                    case 3:
                        Console.WriteLine("--- 인벤토리 목록 ---");
                        if (mPlayer.Inventory.Count == 0) Console.WriteLine("비어 있음");
                        else
                        {
                            for (int i = 0; i < mPlayer.Inventory.Count; i++)
                            {
                                ItemData item = mPlayer.Inventory[i];
                                Console.WriteLine($"- {Item.GetItemName(item)}");
                            }
                        }
                        break;
                    case 0:
                        mIsRunning = false;
                        ChangeState(GameState.Exit);            
                        break;
                }

                if (mCurrentState == GameState.Exit)
                {
                    break;
                }
                Console.WriteLine("\n(계속하려면 아무 키나 누르세요)");
                Console.ReadKey();
            }
        }

        public void ExitGame()
        {
            if (mIsCompleted)
            {
                Console.Clear();
                Console.WriteLine("========================================");
                Console.WriteLine("        ?? [CONGRATULATIONS] ??           ");
                Console.WriteLine("========================================");
                SlowSay("\n당신은 모든 수수께끼를 풀고 방을 탈출했습니다.\n");
                Thread.Sleep(1000);
                SlowSay("하지만 당신의 눈앞에 보이는 건 거대한 잠겨진 문과");
                SlowSay("얼룩진 피로 장식된 바닥과 벽 뿐입니다....\n");
                Thread.Sleep(1000);
                SlowSay("당신은 이제......?");
                Thread.Sleep(1000);
            }
            Console.WriteLine("\nThe End");
            Console.ReadKey();
        }

        public void MoveRoom(string dir)
        {
            // 순환형 이동로직 선택
            int change = dir == "left" ? -1 : dir == "right" ? 1 : 0;
            mCurrentRoomIndex = (mCurrentRoomIndex + change + mRooms.Length) % mRooms.Length; // 음수 방지
        }

        private void ShowTitle()
        {
            Console.Clear();
            Console.WriteLine("========================================");
            Console.WriteLine("                                        ");
            Console.WriteLine("             ROOM ESCAPE                ");
            Console.WriteLine("                                        ");
            Console.WriteLine("========================================");
            Console.ResetColor();
            Console.WriteLine("\n    [아무 키나 누르면 시작합니다]");
            Console.ReadKey();
        }

        private void ShowStory()
        {
            Console.Clear();
            Console.WriteLine("[Prologue]");
            Console.WriteLine("----------------------------------------");
            SlowSay("낯선 방에서 눈을 뜬 당신.");
            Thread.Sleep(1000);
            SlowSay("사방이 막혀있는 공간은 폐쇄공포증을 가진 당신을 압박해옵니다.\n");
            Thread.Sleep(2000);
            SlowSay("점차 이 상황에 대한 불안감을 느낀 당신은 방을 둘러봅니다");
            Thread.Sleep(1000);
            SlowSay("여기저기 수상한 물건들이 흩뿌려져 있습니다.");
            Thread.Sleep(1000);
            SlowSay("어쩌면 이 상황을 벗어나는 데에 저런 물건들이 필요할지 모릅니다");
            Thread.Sleep(1500);
            SlowSay("\n심호흡을 한 당신은 이제......\n");
            Thread.Sleep(1500);
            Console.WriteLine("----------------------------------------");
            Console.WriteLine("\n[Press Any Key To START]");
            Console.ReadKey();
        }

        private void SlowSay(string message, int speed = 40)
        {
            foreach (char c in message)
            {
                Console.Write(c);
                Thread.Sleep(speed);
            }
            Console.WriteLine();
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
