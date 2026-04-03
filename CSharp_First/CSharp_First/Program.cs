using System;
using System.Collections.Generic;
using System.Threading;

namespace CSharp_First
{
    public enum GameState
    {
        None = 0,
        Start,
        Running,
        End,
    }

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

    public class AncientSlime : FantasticCreature
    {
        public int Size { get; private set; }

        public AncientSlime()
        {
            Name = "고대 왕 슬라임";
            Rank = CreatureRank.Unique;
            Level = 1;
            Size = 10;
        }

        public override void LevelUp()
        {
            base.LevelUp();
            Size *= 2;
            Console.WriteLine($"[{Name}]의 몸집이 거대해졌습니다! (현재 크기: {Size})");
        }

        public void Split()
        {
            if (Size > 20)
                Console.WriteLine($"[{Name}]이 여러 마리의 작은 슬라임으로 분열하여 공격을 회피합니다!");
            else
                Console.WriteLine($"[{Name}]이 몸을 부풀려 위협합니다.");
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

    public class EternalPhoenix : FantasticCreature
    {
        public int FireEssence { get; private set; }

        public EternalPhoenix()
        {
            Name = "영원의 피닉스";
            Rank = CreatureRank.Ancient; // 고대 등급
            Level = 10;
            FireEssence = 0;
        }

        public override void LevelUp()
        {
            base.LevelUp();
            FireEssence += 5;
            Console.WriteLine($"[{Name}]의 깃털이 붉게 타오르며 불꽃의 정수를 얻었습니다!");
        }

        public void Rebirth()
        {
            if (FireEssence > 50)
            {
                Console.WriteLine($"[{Name}]가 잿더미 속에서 다시 부활합니다! 모든 상처가 치유됩니다.");
                FireEssence -= 50;
                return;
            }
            else if (FireEssence > 25)
            {
                Console.WriteLine($"[{Name}]가 불꽃의 힘을 파티원에게 부여합니다.");
                FireEssence -= 25;
                return;
            }
            Console.WriteLine($"불꽃의 정수가 부족하여 스킬이 취소되었습니다.");
            Console.WriteLine($"현재 보유한 불꽃의 정수: {FireEssence}");

        }
    }

    public class ShadowStalker : FantasticCreature
    {
        public double EvasionRate { get; private set; }

        public ShadowStalker()
        {
            Name = "그림자 추적자";
            Rank = CreatureRank.Rare;
            Level = 3;
            EvasionRate = 10;
        }

        public override void LevelUp()
        {
            base.LevelUp();
            EvasionRate += 2.5;
            Console.WriteLine($"[{Name}]가 어둠 속에 더 깊이 숨어듭니다. (회피율: {EvasionRate}%)");
        }

        public void Assassinate()
        {
            Console.WriteLine($"[{Name}]가 적의 뒤를 잡아 치명적인 일격을 날립니다!");
        }
    }

    public class GameManager
    {
        // 개선 필요1
        private int _creatureCount = 5; //CreaturePool.Count를 어떻게 쓰면 좋을까 고민하다가 일단 임시로 설정
        public GameState currentState { get; private set; } = GameState.None;
        public List<FantasticCreature> CreaturePool = new List<FantasticCreature>();
        public List<FantasticCreature> PartyList = new List<FantasticCreature>();

        public void InitGame()
        {
            currentState = GameState.Start;
            CreaturePool.Clear();
            Console.WriteLine("데이터 로딩중...");

            // 개선 필요2 - 뽑기 로직에서 문제점 발생
            // AI추천: 리플랙션 or 팩토리패턴(근데 모르겠음, 좀 더 공부해봐야할듯)
            CreaturePool.Add(new GoldenGoblin());
            CreaturePool.Add(new AncientSlime());
            CreaturePool.Add(new CrystalDragon());
            CreaturePool.Add(new EternalPhoenix());
            CreaturePool.Add(new ShadowStalker());  

            currentState = GameState.Running;

            RunningGame();

            EndGame();
        }

        public void RunningGame()
        {
            while (currentState == GameState.Running)
            {
                GameUtility.ShowMainMenu();
                int number = GameUtility.CheckInputIsNumber(Console.ReadLine() ?? "", 0, 5);
                if (number == -1) continue;
                if (number == 0)
                {
                    currentState = GameState.End;
                    return;
                }

                switch (number)
                {
                    case 1:
                        Console.WriteLine("뽑고 싶은 횟수를 입력하세요(최대 10회)");
                        int count = GameUtility.CheckInputIsNumber(Console.ReadLine() ?? "", 1, 10);
                        if (count != -1) JoinRandomCreatures(count);
                        break;
                    case 2:
                        Console.WriteLine("크리쳐를 선택하세요:");
                        GameUtility.ShowCreaturePool(CreaturePool);
                        int choice = GameUtility.CheckInputIsNumber(Console.ReadLine() ?? "", 1, CreaturePool.Count);
                        if (choice != -1) JoinSpecificCreature(CreaturePool[choice - 1]);
                        break;
                    case 3:
                        GameUtility.ShowCreaturePool(CreaturePool);
                        break;
                    case 4:
                        GameUtility.ShowPartyInfo(PartyList);
                        if (!GameUtility.IsPartyEmpty(PartyList))
                        {
                            Console.WriteLine("1. 특정 레벨값으로 찾기\n2. 등급으로 찾기\n3. 이름으로 찾기\n4. 메뉴로 돌아가기");
                            int category = GameUtility.CheckInputIsNumber(Console.ReadLine() ?? "", 1, 4);
                            if (category != -1) ShowFindMenu(category);
                        }
                        break;
                    case 5:
                        Console.WriteLine("개발자 메뉴입니다.");
                        if (GameUtility.IsPartyEmpty(PartyList))
                        {
                            Console.WriteLine("[System] 테스트할 파티원이 없습니다.");
                        }
                        else
                        {
                            DeveloperTestMenu();
                        }
                        break;
                }
                Console.WriteLine("계속 하려면 아무 키나 누르세요...");
                Console.ReadKey();
            }          
        }

        public void EndGame()
        {
            Console.Clear();
            Console.WriteLine("뽑기 시뮬레이터 종료....");
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
                int choice = randNum.Next(0, CreaturePool.Count);
                FantasticCreature? newCreature = null;
                // 개선 필요2
                // 몬스터 클래스가 추가될 때마다 if문을 추가해야 하는 문제점 발생
                if (choice == 0)
                    newCreature = new GoldenGoblin();
                else if (choice == 1)
                    newCreature = new CrystalDragon();
                else if (choice == 2)
                    newCreature = new AncientSlime();
                else if (choice == 3)
                    newCreature = new EternalPhoenix();
                else if (choice == 4)
                    newCreature = new ShadowStalker();

                if (newCreature != null)
                {
                    PartyList.Add(newCreature);
                    GameUtility.GetCreatureRankToColor(newCreature.Rank);
                    Console.WriteLine($"{newCreature.Name}이(가) 파티에 합류했습니다!");
                }         
            }
            Console.WriteLine("뽑기가 완료되었습니다.");
        }

        public void JoinSpecificCreature(FantasticCreature creature)
        {     
            Console.WriteLine("=======크리쳐 선택권 사용!!========");
            // 개선 필요2
            // 마찬가지로 몬스터 클래스가 추가될 때마다 if문을 추가해야 하는 문제점 발생
            FantasticCreature? newCreature = null;
            if (creature is GoldenGoblin)
                newCreature = new GoldenGoblin();
            else if (creature is AncientSlime)
                newCreature = new AncientSlime();
            else if (creature is CrystalDragon)
                newCreature = new CrystalDragon();
            else if (creature is EternalPhoenix)
                newCreature = new EternalPhoenix();
            else if (creature is ShadowStalker)
                newCreature = new ShadowStalker();

            if (newCreature != null)
            {
                PartyList.Add(newCreature);
                GameUtility.GetCreatureRankToColor(creature.Rank);
                Console.WriteLine($"{creature.Name}이(가) 파티에 합류했습니다!");
            }              
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
                case 4:
                    break;
                default:
                    Console.WriteLine("[System] 잘못된 행동입니다.");
                    break;
            }
        }

        // 오버로딩을 사용해 같은 이름의 메서드로 다양한 검색 기능 구현
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

        public void DeveloperTestMenu()
        {
            Console.Clear();
            Console.WriteLine("==========<< 개발자 테스트 모드 >>==========");
            GameUtility.ShowPartyInfo(PartyList);
            Console.WriteLine("테스트할 크리쳐 번호를 선택하세요 (0: 취소):");

            int index = GameUtility.CheckInputIsNumber(Console.ReadLine() ?? "", 0, PartyList.Count);
            if (index <= 0) return;

            FantasticCreature target = PartyList[index - 1];
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"[ {target.Name} 테스트 중... ]");
                Console.WriteLine("1. 레벨업 테스트");
                Console.WriteLine("2. 고유 스킬 테스트");
                Console.WriteLine("0. 나가기");

                int testAction = GameUtility.CheckInputIsNumber(Console.ReadLine() ?? "", 0, 2);
                if (testAction == 0) break;

                if (testAction == 1) target.LevelUp();
                else if (testAction == 2) ExecuteUniqueSkill(target);

                Console.WriteLine("\n아무 키나 누르면 테스트 메뉴로 돌아갑니다...");
                Console.ReadKey();
            }
        }

        private void ExecuteUniqueSkill(FantasticCreature creature)
        {
            Console.WriteLine($"==== {creature.Name} 고유 기능 실행 ====");

            // 개선 필요2
            // 형변환을 이용해 고유 기능을 호출
            if (creature is AncientSlime slime) slime.Split();
            else if (creature is CrystalDragon dragon) dragon.Roar();
            else if (creature is EternalPhoenix phoenix) phoenix.Rebirth();
            else if (creature is ShadowStalker stalker) stalker.Assassinate();
            else if (creature is GoldenGoblin)
                Console.WriteLine("황금 고블린은 고유 액티브 스킬이 없습니다.");
            else
                Console.WriteLine("정의되지 않은 고유 기능입니다.");
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

        // 개선 필요3
        // 게임의 메뉴가 늘어날 때마다 CheckInputIsNumber 메서드의 매개변수로 들어가는 max값을 일일이 수정해줘야 하는 문제점 발생
        public static int CheckInputIsNumber(string input, int min, int max)
        {
            if (int.TryParse(input, out int number))
            {
                if (number < min || number > max)
                {
                    Console.WriteLine("[System] 올바른 숫자를 입력해주세요.");
                    return -1;
                }
                else return number;
            }   
            else
            {
                Console.WriteLine("[System] 숫자를 입력해주세요.");
                return -1;
            }
        }

        public static void ShowMainMenu()
        {
            Console.Clear();
            Console.WriteLine("<뽑기 시뮬레이터 Fantastic Version>");
            Console.WriteLine("===================================");
            Console.WriteLine("===================================");
            Console.WriteLine("1. 크리쳐 랜덤 뽑기\n2. 크리쳐 확정 뽑기\n3. 전체 크리쳐 목록 보기\n4. 내 파티 보기\n5. 개발자 메뉴\n0. 종료");
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
            Console.Clear();
            Console.WriteLine("=========<< 크리쳐 목록 >>=========");
            for (int i = 0; i < list.Count; i++)
            {   
                GetCreatureRankToColor(list[i].Rank);
                Console.WriteLine($"\n[{i + 1}] <{list[i].Name}> Lv.{list[i].Level}");
            }
            Console.WriteLine("===================================");
        }

        public static void ShowPartyInfo(List<FantasticCreature> list)
        {
            if (IsPartyEmpty(list))
            {
                Console.WriteLine("[System] 파티에 크리쳐가 존재하지 않습니다.");
                return;
            }

            Console.WriteLine("==========<< 파티 정보 >>==========");
            for (int i = 0; i < list.Count; i++)
            {
                GetCreatureRankToColor(list[i].Rank);
                Console.WriteLine($"[{i + 1}] <{list[i].Name}> Lv.{list[i].Level}\n");
            }
        }

        // 현재 사용하지 않는 메서드
        public static void ShowCreatureInfo(FantasticCreature creature)
        {
            Console.WriteLine($"==========<< 크리쳐 정보 >>==========");
            GetCreatureRankToColor(creature.Rank);
            Console.WriteLine($"<{creature.Name}> Lv.{creature.Level}");
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
