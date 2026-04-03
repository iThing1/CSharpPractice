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
        public List<FantasticCreature> PartyList = new List<FantasticCreature>();
        
        public void JoinRandomCreatures(int number)
        {
            Random randNum = new Random();
            
            for (int i = 0; i < number; i++)
            {
                int choice = randNum.Next(0, _creatureCount);
                FantasticCreature newCreature = null;
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

        public void DisplayCreatureInfo(List<FantasticCreature> list)
        {
            Console.WriteLine("\n==========<< 몬스터 정보 >>==========");
            for (int i = 0; i < list.Count; i++)
            {
                Console.WriteLine($"[{i + 1}] Name: {list[i].Name}\nRank: {list[i].Rank} | Level: {list[i].Level}\n");
            }
        }

    }

    internal class Program
    {
        static void Main(string[] args)
        {   
            GameManager gm = new GameManager();
            gm.JoinRandomCreatures(3);
            gm.DisplayCreatureInfo(gm.PartyList);

            gm.PartyList[0].LevelUp();
            gm.DisplayCreatureInfo(gm.PartyList);
        }
    }
}
