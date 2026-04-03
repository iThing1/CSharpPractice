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

        public void DisplayCreatureInfo()
        {
            Console.WriteLine($"Name: {Name}");
            Console.WriteLine($"Rank: {Rank}");
            Console.WriteLine($"Level: {Level}");
        }

        public virtual void LevelUp()
        {
            Level++;
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

    internal class Program
    { 
        static void Main(string[] args)
        {
            FantasticCreature goblin = new GoldenGoblin();
            goblin.DisplayCreatureInfo();
            goblin.LevelUp();
        }
    }
}
