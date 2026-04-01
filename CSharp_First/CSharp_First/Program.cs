using System;
using System.Collections.Generic;
using System.Threading;

namespace CSharp_First
{
    public enum ItemData
    {
        None = 0,
        OldKey = 100,
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
    // 
    public static class Item
    {
        public static string GetItemName(ItemData item)
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

        public abstract void Interact(Player player, GameManager gameMgr);
    }

    public interface IItemDropable
    {
        public void DropItem();
    }

    public interface ILockable
    {
        public void Unlock();
    }
    // =======================================================================
    // ============= 물건 목록 ===================================================
    // =======================================================================
    public class SafeBox : InteractableObject, IItemDropable, ILockable
    {
        public ItemData ReqItemID { get; private set; }
        public ItemData DropItemID { get; private set; }
        public bool IsLocked { get; private set; } = true;

        public SafeBox() : base ("금고", "단단히 잠겨져있는 금고이다.")
        {
            ReqItemID = ItemData.OldKey;
            DropItemID = ItemData.Note;
        }   

        public override void Interact(Player player, GameManager gameMgr)
        {
            Console.WriteLine($"[System]{Name}을(를) 조사합니다.");
            if (IsLocked)
            {
                Console.WriteLine($"{Item.GetItemName(ReqItemID)}가 필요할 것 같다.");
                if (player.HasItem(ReqItemID))
                {
                    Unlock();
                    player.AddItem(DropItemID);
                    player.Inventory.Remove(ReqItemID);
                    Console.WriteLine($"[System]{Item.GetItemName(ReqItemID)}이/가 사라졌습니다.");
                }
                else 
                    Console.WriteLine("하지만 열쇠가 없다. 다른 곳을 더 찾아보자.");
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
            Console.WriteLine($"금고 안에서 [{Item.GetItemName(DropItemID)}]을(를) 발견했다!");
        }
    }

    public class Coat : InteractableObject, IItemDropable
    {
        private ItemData dropItemID = ItemData.OldKey;
        private bool hasItem = true;

        public Coat() : base("코트", "벽에 걸려있는 낡은 코트이다. 주머니가 두툼해 보인다.") { }
        public override void Interact(Player player, GameManager gameMgr)
        {
            Console.WriteLine($"[System]{Name}을(를) 조사합니다.");

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
            Console.WriteLine($"[System]코트 주머니에서 [{Item.GetItemName(dropItemID)}]을(를) 획득했습니다!");
        }
    }
    public class DoorLock : InteractableObject
    {
        private string password = "1111"; // 탈출 비밀번호
        private bool isSolved = false;

        public DoorLock() : base("전자 도어락", "단단히 잠긴 철문에 달려있는 도어락이다. 숫자 패드가 빛나고 있다.") { }

        public override void Interact(Player player, GameManager gameMgr)
        {
            Console.WriteLine($"[System]{Name}을(를) 조사합니다.");
            if (isSolved)
            {
                Console.WriteLine("이미 잠금이 해제되어 문이 열려 있습니다.");
                return;
            }

            Console.Write("비밀번호 4자리를 입력하세요 (0: 취소): ");
            string input = Console.ReadLine() ?? "";

            if (input == "0") return;

            if (input == password)
            {
                isSolved = true;
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("<<<<< 삐비빅! 잠금이 해제되었습니다! >>>>>");
                Console.ResetColor();
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
        public string Name { get; private set; }
        public List<InteractableObject> objectList { get; set; } = new List<InteractableObject>();

        public Room(string name)
        {
            Name = name;
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

        public void ChangeState(GameState state) => CurrentState = state;

        public void InitGame()
        {
            ShowTitle();
            ShowStory();

            mRooms = new Room[4];
            mRooms[0] = new Room("방문");
            mRooms[1] = new Room("옷걸이대가 있는 벽");
            mRooms[2] = new Room("창문이 있는 벽");
            mRooms[3] = new Room("침대가 있는 쪽");

            // 여기서부터 공간안에 들어갈 물체를 생성 및 배치
            mRooms[0].objectList.Add(new DoorLock());
            mRooms[1].objectList.Add(new Coat());
            mRooms[2].objectList.Add(new SafeBox());

            Console.WriteLine("게임 시작");
            ChangeState(GameState.Running);
        }

        public void RunGame()
        {
            IsRunning = true;
            while (IsRunning)
            {
                if (CurrentState == GameState.Exit)
                {
                    IsRunning = false;
                    break; 
                }

                Console.Clear();
                Console.WriteLine($"--- 현재 위치: {mRooms[mCurrentRoomIndex].Name} ---");
                Console.WriteLine("무엇을 할까?");
                Console.WriteLine("1. 이동  2. 조사  3. 인벤토리  (0. 종료)");
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
                        {
                            List<InteractableObject> currentObjects = mRooms[mCurrentRoomIndex].objectList;
                            if (currentObjects.Count == 0)
                            {
                                Console.WriteLine("여기는 조사할 만한 것이 없다.");
                                break;
                            }

                            Console.Clear();
                            Console.WriteLine("눈에 들어오는 몇개의 물건이 있다.");
                            for (int i = 0; i < currentObjects.Count; i++)
                            {
                                Console.WriteLine($"{i + 1}. {currentObjects[i].Name}");
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
                                    currentObjects[targetIndex].Interact(Player, this);
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
                        if (Player.Inventory.Count == 0) Console.WriteLine("비어 있음");
                        else
                        {
                            for (int i = 0; i < Player.Inventory.Count; i++)
                            {
                                ItemData item = Player.Inventory[i];
                                Console.WriteLine($"- {Item.GetItemName(item)}");
                            }
                        }
                        break;
                    case 0:
                        IsRunning = false;
                        ChangeState(GameState.Exit);            
                        break;
                }

                if (CurrentState == GameState.Exit) break;
                Console.WriteLine("\n(계속하려면 아무 키나 누르세요)");
                Console.ReadKey();
            }
            ExitGame();
        }

        public void ExitGame()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("========================================");
            Console.WriteLine("          CONGRATULATIONS!              ");
            Console.WriteLine("========================================");
            Console.ResetColor();
            SlowSay("\n당신은 모든 수수께끼를 풀고 방을 탈출했습니다.");
            Thread.Sleep(1000);
            SlowSay("탁 트인 하늘을 보며 당신은 안도의 한숨을 내쉽니다.");
            Thread.Sleep(1000);
            Console.WriteLine("\n게임을 종료하려면 아무 키나 누르세요...");
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
            SlowSay("\n심호흡을 한 당신은 이제...\n");
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
            gm.RunGame();

        }
    }
}
