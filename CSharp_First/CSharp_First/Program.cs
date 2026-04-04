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
        Count   // 개선 필요3의 해결책으로 추가. enum에 포함된 크리쳐의 등급의 수를 CreatureRank.Counting으로 표현.
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
            Rank = CreatureRank.Ancient;
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
            Console.WriteLine($"[{Name}]가 어둠 속에 더 깊이 숨어듭니다. (회피율: +{EvasionRate}%)");
        }

        public void Assassinate()
        {
            Console.WriteLine($"[{Name}]가 적의 뒤를 잡아 치명적인 일격을 날립니다!");
        }
    }

    public class StormWeaver : FantasticCreature
    {
        public int StaticCharge { get; private set; }

        public StormWeaver()
        {
            Name = "폭풍을 잣는 자";
            Rank = CreatureRank.Legendary;
            Level = 7;
            StaticCharge = 0;
        }

        public override void LevelUp()
        {
            base.LevelUp();
            StaticCharge += 12;
            Console.WriteLine($"[{Name}]의 주변 공기가 떨리며 전기가 흐릅니다. (현재 충전: {StaticCharge}V)");
        }

        public void ThunderStrike()
        {
            if (StaticCharge >= 36)
            {
                Console.WriteLine($"[{Name}]이 벼락의 실타래를 풀어헤칩니다! 거대한 낙뢰가 전장을 강타합니다.");
                StaticCharge -= 36;
            }
            else
            {
                Console.WriteLine($"[System] 아직 대기의 에너지가 충분히 모이지 않았습니다. (필요: 36V)");
            }
        }
    }

    public class GameManager
    {
        private readonly int MIN_LEVEL = 1;
        private readonly int MAX_LEVEL = 100;

        public GameState currentState { get; private set; } = GameState.None;
        public List<FantasticCreature> CreaturePool = new List<FantasticCreature>();
        public List<FantasticCreature> PartyList = new List<FantasticCreature>();

        // ================ 게임 동작 메서드 ============================================
        public void InitGame()
        {
            currentState = GameState.Start;
            CreaturePool.Clear();
            Console.WriteLine("데이터 로딩중...");

            // 개선 필요2 - 크리쳐 데이터 추가
            CreaturePool.Add(new GoldenGoblin());
            CreaturePool.Add(new AncientSlime());
            CreaturePool.Add(new CrystalDragon());
            CreaturePool.Add(new EternalPhoenix());
            CreaturePool.Add(new ShadowStalker());  
            CreaturePool.Add(new StormWeaver());

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
                        GameUtility.ShowList(CreaturePool, "전체 크리쳐 목록");
                        int choice = GameUtility.CheckInputIsNumber(Console.ReadLine() ?? "", 1, CreaturePool.Count);
                        if (choice != -1) JoinSpecificCreature(CreaturePool[choice - 1]);
                        break;
                    case 3:
                        GameUtility.ShowList(CreaturePool, "전체 크리쳐 목록");
                        break;
                    case 4:
                        if (GameUtility.IsPartyEmpty(PartyList))
                            Console.WriteLine("[System] 파티가 비어있습니다. 먼저 크리쳐 뽑기를 진행하세요");
                        else ShowPartyMenu();
                            break;
                    case 5:
                        Console.WriteLine("개발자 메뉴입니다.");
                        if (GameUtility.IsPartyEmpty(PartyList))
                        {
                            Console.WriteLine("[System] 테스트할 파티원이 없습니다.");
                        }
                        else
                        {
                            ShowDeveloperTestMenu();
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

        // ================ 뽑기 관련 메서드 ============================================
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
                else if (choice == 5)
                    newCreature = new StormWeaver();

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
            else if (creature is StormWeaver)
                newCreature = new StormWeaver();

            if (newCreature != null)
            {
                PartyList.Add(newCreature);
                GameUtility.GetCreatureRankToColor(creature.Rank);
                Console.WriteLine($"{creature.Name}이(가) 파티에 합류했습니다!");
            }              
        }

        // ================ 메뉴 UI ============================================
        public void ShowPartyMenu()
        {
            while (true)
            {
                Console.Clear();
                GameUtility.ShowList(PartyList, "내 파티 정보");
                Console.WriteLine("[ 1. 검색 | 2. 정렬 | 0. 닫기 ]");
                Console.WriteLine("===================================");

                int choice = GameUtility.CheckInputIsNumber(Console.ReadLine() ?? "", 0, 2);
                if (choice == 0) break;

                if (choice == 1) ShowSearchMenu();
                else if (choice == 2) ShowSortMenu();
            }
        }

        private void ShowSearchMenu()
        {
            Console.Clear();
            Console.WriteLine("[검색 기준 선택] (1.레벨 2.등급 3.이름 0.취소)");
            int type = GameUtility.CheckInputIsNumber(Console.ReadLine() ?? "", 0, 3);
            if (type <= 0) return;

            switch (type)
            {
                case 1:
                    Console.Write("찾을 최소 레벨: ");
                    int level = GameUtility.CheckInputIsNumber(Console.ReadLine() ?? "", MIN_LEVEL, MAX_LEVEL);
                    if (level != -1) GameUtility.FindSpecificCreature(PartyList, level);
                    break;
                case 2:
                    Console.WriteLine("0. None | 1. Normal | 2. Rare | 3. Unique | 4. Legendary | 5. Ancient |");
                    int rank = GameUtility.CheckInputIsNumber(Console.ReadLine() ?? "", 0, 5);
                    if (rank != -1) GameUtility.FindSpecificCreature(PartyList, (CreatureRank)rank);
                    break;
                case 3:
                    Console.Write("찾을 이름: ");
                    string name = Console.ReadLine() ?? "";
                    if (name is not null or "") GameUtility.FindSpecificCreature(PartyList, name);
                    break;
            }
            Console.WriteLine("아무 키나 누르면 파티 메뉴로 돌아갑니다.");
            Console.ReadKey();
        }

        private void ShowSortMenu()
        {
            Console.Clear();
            Console.WriteLine("[정렬 기준 선택] (1.레벨순 2.등급순 3.이름순 0.취소)");
            int sortType = GameUtility.CheckInputIsNumber(Console.ReadLine() ?? "", 0, 3);
            if (sortType <= 0) return;

            GameUtility.SortList(PartyList, sortType);
            Console.WriteLine("[System] 정렬이 완료되었습니다. (아무 키나 누르세요)");
            Console.ReadKey();
        }

        // // ================ 테스트 메뉴 ============================================
        public void ShowDeveloperTestMenu()
        {
            Console.Clear();
            Console.WriteLine("==========<< 개발자 테스트 모드 >>==========");
            GameUtility.ShowList(PartyList, "내 파티 목록");
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
            else if (creature is StormWeaver weaver) weaver.ThunderStrike();
            else if (creature is GoldenGoblin)
                Console.WriteLine("[System] 황금 고블린은 고유 액티브 스킬이 없습니다.");
            else
                Console.WriteLine("[System] 정의되지 않은 고유 기능입니다.");
        }
    }

    // ================ 유틸리티 함수 ============================================
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

        // ================ 검색 기능 ============================================
        public static void FindSpecificCreature(List<FantasticCreature> list, string name)
        {
            List<FantasticCreature> result = list.FindAll(c => c.Name == name);
            if (result.Count > 0)
            {
                Console.WriteLine($"[System] {name}이(가) 파티에 존재합니다!");
                GameUtility.ShowList(result, "크리쳐 목록");
                return;
            }
            Console.WriteLine($"[System] {name}이(가) 파티에 존재하지 않습니다.");
        }

        public static void  FindSpecificCreature(List<FantasticCreature> list, CreatureRank rank)
        {
            List<FantasticCreature> result = list.FindAll(c => c.Rank == rank);
            if (result.Count > 0)
            {
                Console.WriteLine($"[System] {rank}이(가) 파티에 존재합니다!");
                GameUtility.ShowList(result, "크리쳐 목록");
                return;
            }
            Console.WriteLine($"[System] {rank}이(가) 파티에 존재하지 않습니다.");
        }

        public static void FindSpecificCreature(List<FantasticCreature> list, int level)
        {
            List<FantasticCreature> result = list.FindAll(c => c.Level >= level);
            if (result.Count > 0)
            {
                Console.WriteLine($"[System] 레벨 {level}이상인 몬스터가 파티에 존재합니다!");
                GameUtility.ShowList(result, "크리쳐 목록");
                return;
            }
            Console.WriteLine($"[System] 레벨 {level}이상인 몬스터가 파티에 존재하지 않습니다.");
        }

        // 개선 필요1 - 정렬기능 추가(해결 완료)
        // ================ 정렬 기능  ============================================
        public static void SortList(List<FantasticCreature> list, int num)
        {
            if (num == 1) SortByLevel(list);
            else if (num == 2) SortByRank(list);
            else if (num == 3) SortByName(list);
        }

        private static void SortByLevel(List<FantasticCreature> list)
        {
            if (IsPartyEmpty(list)) return;
            list.Sort((a, b) => b.Level.CompareTo(a.Level));
        }

        private static void SortByRank(List<FantasticCreature> list)
        {
            if (!IsPartyEmpty(list)) return;
            list.Sort((a, b) => b.Rank.CompareTo(a.Rank));
        }

        private static void SortByName(List<FantasticCreature> list)
        {
            if (IsPartyEmpty(list)) return;
            list.Sort((a, b) => a.Name.CompareTo(b.Name));
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

        // 개선 필요4 - 해결 완료
        // 아래의 ShowCreaturePool과 ShowPartyInfo 메서드는 출력 형식이 거의 동일
        // 들어오는 매개변수 list가 CreaturePool인지 PartyList인지를 판단할 수 있다면 하나의 메서드로 통합해서 사용할 수 있을 것으로 보임
        public static void ShowList(List<FantasticCreature> list, string title)
        {
            if (IsPartyEmpty(list))
            {
                Console.WriteLine($"[System] {title} 목록이 비어 있습니다.");
                return;
            }

            Console.WriteLine($"==========<< {title} >>==========");
            for (int i = 0; i < list.Count; i++)
            {
                GetCreatureRankToColor(list[i].Rank);
                Console.WriteLine($"\n[{i + 1}] <{list[i].Name}> Lv.{list[i].Level}");
            }
            Console.WriteLine("=====================================");
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
