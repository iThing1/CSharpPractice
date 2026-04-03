using System;
using System.Collections.Generic;
using System.Threading;

namespace CSharp_First
{
    
    public enum CreatureRank
    {
        None,
        Normal,
        Rare,
        Unique,
        Legendary,
        Ancient,
    }

    public class FantasticCreature
    {
        public string Name { get; protected set; } = "";
        public CreatureRank Rank { get; protected set; }
        public int Level { get; protected set; }

        public virtual void LevelUp()
        {
            Level++;
            Console.WriteLine($"====================================");
            Console.WriteLine($"         !!! Level Up !!!            ");
            Console.WriteLine($"====================================");
            Console.WriteLine($"[{Name}]의 레벨이 올라 {Level}이 되었다!");
        }

    }

    public class GoldenGoblin : FantasticCreature
    {
        public GoldenGoblin()
        {
            Name = "황금 고블린";
            Rank = CreatureRank.Normal;
            Level = 1;
        }

        public override void LevelUp()
        {
            base.LevelUp();
            Console.WriteLine($"[{Name}]의 황금빛이 더욱 빛나기 시작했다!");
        }
    }

    public class CrystalDragon : FantasticCreature
    {
        public int CrystalCount { get; private set; }
        public CrystalDragon()
        {
            Name = "크리스탈 드래곤";
            Rank = CreatureRank.Legendary;
            Level = 5;
            CrystalCount = 0;
        }
        public override void LevelUp()
        {
            base.LevelUp();
            CrystalCount += Level;
            Console.WriteLine($"[{Name}]에게서 크리스탈이 떨어져 나온다!!");
            DropCrystal();
        }

        public void Roar()
        {
             Console.WriteLine($"[{Name}]이(가) 포효했다! 주변이 진동한다!");
        }

        public void DropCrystal()
        {
            if (CrystalCount > 0)
            {
                // 레벨업 시 떨어지진 크리스탈 조각 갯수를 확정 뽑기의 재화로 사용할 수 있도록 구현해보는 것도
                Console.WriteLine($"[System]크리스탈이 {CrystalCount}개 떨어졌습니다.");
            }
        }
    }

    public class GameManager
    {
        private int _creatureCount = 2;
        public List<FantasticCreature> CreaturePool = new List<FantasticCreature>();
        public List<FantasticCreature> PartyList = new List<FantasticCreature>();
        
        public GameManager()
        {
            CreaturePool.Add(new GoldenGoblin());
            CreaturePool.Add(new CrystalDragon());
        }

        public void RunningGame()
        {
            bool isRunning = true;
            while (isRunning)
            {
                GameUtility.ShowMainMenu();
                int number = GameUtility.CheckInputIsNumber(Console.ReadLine() ?? "", 0, 4);
                {
                    if (number == 0)
                    {
                        isRunning = false;
                        continue;
                    }

                    if (number == -1) continue;

                    switch (number)
                    {
                        case 1:
                            Console.WriteLine("뽑고 싶은 횟수를 입력하세요");
                            int count = GameUtility.CheckInputIsNumber(Console.ReadLine() ?? "", 1, 10);
                            if (count == -1) continue;
                            JoinRandomCreatures(count);
                            break;
                        case 2:
                            Console.WriteLine("크리쳐를 선택하세요:");
                            GameUtility.ShowCreaturePool(CreaturePool);
                            int choice = GameUtility.CheckInputIsNumber(Console.ReadLine() ?? "", 0, CreaturePool.Count);
                            if (choice == -1) continue;
                            JoinSpecificCreature(CreaturePool[choice - 1]);

                            break;
                        case 3:
                            GameUtility.ShowCreaturePool(CreaturePool);
                            break;
                        case 4:
                            GameUtility.ShowPartyInfo(PartyList);
                            if (GameUtility.IsPartyEmpty(PartyList)) continue;
                            Console.WriteLine("1. 특정 레벨값으로 찾기\n2. 등급으로 찾기\n3. 이름으로 찾기");
                            int category = GameUtility.CheckInputIsNumber(Console.ReadLine() ?? "", 1, 3);
                            if (category == -1) continue;
                            ShowFindMenu(category);
                            break;
                    }
                }
                Console.WriteLine("계속 하려면 아무 키나 누르세요...");
                Console.ReadKey();
            }
        }

        public void JoinRandomCreatures(int number)
        {
            Console.Clear();
            Console.WriteLine("=======크리쳐 랜덤 뽑기!!========");
            Random randNum = new Random();        
            for (int i = 0; i < number; i++)
            {
                Console.WriteLine($"{i + 1}회차 뽑기는....");
                Thread.Sleep(1200);
                int choice = randNum.Next(0, _creatureCount);
                FantasticCreature? newCreature = null;
                switch(choice)
                {
                    case 0:
                        newCreature = new GoldenGoblin();
                        break;
                    case 1:
                        newCreature = new CrystalDragon();
                        break;
                }

                if (newCreature != null)
                {
                    PartyList.Add(newCreature);
                    Console.WriteLine($"[System] {newCreature.Name}이(가) 파티에 합류했습니다!");
                }         
            }
            Console.WriteLine("뽑기가 완료되었습니다.");
        }

        public void JoinSpecificCreature(FantasticCreature creature)
        {     
            Console.WriteLine("=======크리쳐 선택권 사용!!========");
            PartyList.Add(creature);
            Console.WriteLine($"[System] {creature.Name}이(가) 파티에 합류했습니다!");
        }

        public void ShowFindMenu(int number)
        {
            switch (number)
            {
                case 1:
                    Console.WriteLine("찾고 싶은 레벨값을 입력하세요:");
                    int level = GameUtility.CheckInputIsNumber(Console.ReadLine() ?? "", 1, 100);
                    if (level == -1) return;
                    FindSpecificCreature(level);
                    break;
                case 2:
                    Console.WriteLine("찾고 싶은 등급을 입력하세요:");
                    Console.WriteLine("0: None | 1: Normal | 2: Rare | 3: Unique | 4: Legendary | 5: Ancient");
                    int rankNum = GameUtility.CheckInputIsNumber(Console.ReadLine() ?? "", 0, 5);
                    if (rankNum == -1) return;
                    CreatureRank rank = (CreatureRank)rankNum;
                    FindSpecificCreature(rank);
                    break;
                case 3:
                    Console.WriteLine("찾고 싶은 이름을 입력하세요:");
                    string name = Console.ReadLine() ?? "";
                    if (string.IsNullOrEmpty(name))
                    {
                        Console.WriteLine("[System] 이름을 입력해주세요.");
                        Console.ReadKey();
                        return;
                    }
                    FindSpecificCreature(name);
                    break;
                default:
                    Console.WriteLine("[System] 잘못된 행동입니다.");
                    Thread.Sleep(1000);
                    break;
            }
        }

        public void FindSpecificCreature(string name)
        {
            List<FantasticCreature> result = PartyList.FindAll(c => c.Name == name);
            if (result.Count > 0)
            {
                Console.WriteLine($"[System] {name}이(가) 파티에 존재합니다!");
                GameUtility.ShowPartyInfo(result);
                return;
            }
            Console.WriteLine($"[System] {name}이(가) 파티에 존재하지 않습니다.");
        }

        public void FindSpecificCreature(CreatureRank rank)
        {
            List<FantasticCreature> result = PartyList.FindAll(c => c.Rank == rank);
            if (result.Count > 0)
            {
                Console.WriteLine($"[System] {rank}이(가) 파티에 존재합니다!");
                GameUtility.ShowPartyInfo(result);
                return;
            }
            Console.WriteLine($"[System] {rank}이(가) 파티에 존재하지 않습니다.");
        }

        public void FindSpecificCreature(int level)
        {
            List<FantasticCreature> result = PartyList.FindAll(c => c.Level >= level);
            if (result.Count > 0)
            {
                Console.WriteLine($"[System] 레벨 {level}이상인 몬스터가 파티에 존재합니다!");
                GameUtility.ShowPartyInfo(result);
                return;
            }
            Console.WriteLine($"[System] 레벨 {level}이상인 몬스터가 파티에 존재하지 않습니다.");
        } 
    }

    public static class GameUtility
    {
        public static bool IsPartyEmpty(List<FantasticCreature> list)
        {
            if (list.Count == 0 || list == null)
            {
                return true;
            }
            return false;
        }

        public static void GetCreatureRankToColor(CreatureRank rank)
        {
            switch (rank)
            {
                case CreatureRank.None:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case CreatureRank.Normal:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case CreatureRank.Rare:
                    Console.ForegroundColor = ConsoleColor.Blue;
                    break;
                case CreatureRank.Unique:
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    break;
                case CreatureRank.Legendary:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case CreatureRank.Ancient:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
            }
            Console.Write($"[{rank.ToString()}]");
            Console.ResetColor();
        }

        public static int CheckInputIsNumber(string input, int min, int max)
        {
            if (int.TryParse(input, out int number))
            {
                if (number < min || number > max)
                {
                    Console.WriteLine("[System] 올바른 숫자를 입력해주세요.");
                    Thread.Sleep(1000);
                    return -1;
                }
                else return number;
            }   
            else
            {
                Console.WriteLine("[System] 숫자를 입력해주세요.");
                Thread.Sleep(1000);
                return -1;
            }
        }

        public static void ShowMainMenu()
        {
            Console.Clear();
            Console.WriteLine("===================================");
            Console.WriteLine("===================================");
            Console.WriteLine("1. 크리쳐 랜덤 뽑기\n2. 크리쳐 확정 뽑기\n3. 전체 크리쳐 목록 보기\n4. 내 파티 보기\n0: 종료");
            Console.WriteLine("===================================");
            Console.WriteLine("===================================");
        }

        public static void ShowCreaturePool(List<FantasticCreature> list)
        {
            if (list.Count == 0)
            {
                Console.WriteLine("목록이 비어 있습니다.");
                return;
            }

            for (int i = 0; i < list.Count; i++)
            {   
                Console.WriteLine("==========<< 크리쳐 목록 >>==========");
                Console.WriteLine($"[{i + 1}] ");
                GetCreatureRankToColor(list[i].Rank);
                Console.WriteLine($"{list[i].Name} | Level: {list[i].Level}");
                Console.WriteLine("------------------------------------");
            }
        }

        public static void ShowPartyInfo(List<FantasticCreature> list)
        {
            if (IsPartyEmpty(list))
            {
                Console.WriteLine("[System] 파티에 크리쳐가 존재하지 않습니다.");
                Thread.Sleep(1000); 
                return;
            }

            Console.WriteLine("==========<< 파티 정보 >>==========");
            for (int i = 0; i < list.Count; i++)
            {
                GetCreatureRankToColor(list[i].Rank);
                Console.WriteLine($"[{i + 1}] Name: {list[i].Name}\nLevel: {list[i].Level}\n");
            }
        }

        // 현재 사용하지 않는 메서드
        public static void ShowCreatureInfo(FantasticCreature creature)
        {
            Console.WriteLine($"==========<< 크리쳐 정보 >>==========");
            GetCreatureRankToColor(creature.Rank);
            Console.WriteLine($"Name: {creature.Name}\nLevel: {creature.Level}\n");
        }

    }

    internal class Program
    {
        static void Main(string[] args)
        {   
            GameManager gm = new GameManager();

            gm.RunningGame();
            Console.WriteLine("게임이 종료되었습니다. 감사합니다!");  

        }
    }
}
