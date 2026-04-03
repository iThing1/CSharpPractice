using System;

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

        public void JoinRandomCreatures(int number)
        {
            Console.Clear();
            Console.WriteLine("=======크리쳐 랜덤 뽑기!!========");
            Random randNum = new Random();        
            for (int i = 0; i < number; i++)
            {
                Console.WriteLine($"{i + 1}회차 뽑기는....");
                Thread.Sleep(2000);
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
        }

        public void JoinSpecificCreature(FantasticCreature creature)
        {     
            Console.WriteLine("=======크리쳐 선택권 사용!!========");
            PartyList.Add(creature);
            Console.WriteLine($"[System] {creature.Name}이(가) 파티에 합류했습니다!");
        }

        public void ShowCreaturePool()
        {
            Console.WriteLine("==========<< 전체 크리쳐 목록 >>==========");
            for (int i = 0; i < CreaturePool.Count; i++)
            {
                Console.WriteLine($"[{i + 1}] Name: {CreaturePool[i].Name}\nRank: {CreaturePool[i].Rank} | Level: {CreaturePool[i].Level}\n");
            }
        }

        public void ShowPartyInfo(List<FantasticCreature> list)
        {
            Console.WriteLine("==========<< 파티 정보 >>==========");
            for (int i = 0; i < list.Count; i++)
            {
                Console.WriteLine($"[{i + 1}] Name: {list[i].Name}\nRank: {list[i].Rank} | Level: {list[i].Level}\n");
            }
        }

        public void ShowCreatureInfo(FantasticCreature creature)
        {
            Console.WriteLine($"==========<< 크리쳐 정보 >>==========");
            Console.WriteLine($"Name: {creature.Name}\nRank: {creature.Rank} | Level: {creature.Level}\n");
        }

        public void FindSpecificCreature(string name)
        {
            FantasticCreature? creature = PartyList.Find(c => c.Name == name);
            if (creature != null)
            {
                Console.WriteLine($"[System] {name}이(가) 파티에 존재합니다!");
                ShowCreatureInfo(creature);
                return;
            }
            Console.WriteLine($"[System] {name}이(가) 파티에 존재하지 않습니다.");
        }

        public void FindSpecificCreature(CreatureRank rank)
        {
            FantasticCreature? creature = PartyList.Find(c => c.Rank == rank);
            if (creature != null)
            {
                Console.WriteLine($"[System] {rank}이(가) 파티에 존재합니다!");
                ShowCreatureInfo(creature);
                return;
            }
            Console.WriteLine($"[System] {rank}이(가) 파티에 존재하지 않습니다.");
        }

        public void FindSpecificCreature(int level)
        {
            FantasticCreature? creature = PartyList.Find(c => c.Level >= level);
            if (creature != null)
            {
                Console.WriteLine($"[System] 레벨 {level}이상인 몬스터가 파티에 존재합니다!");
                ShowCreatureInfo(creature);
                return;
            }
            Console.WriteLine($"[System] 레벨 {level}이상인 몬스터가 파티에 존재하지 않습니다.");
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {   
            GameManager gm = new GameManager();
        }
    }
}
