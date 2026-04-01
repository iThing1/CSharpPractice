using System;
using System.Collections.Specialized;

namespace CSharp_First
{
    public abstract class InteractableObject
    {
        public string Name { get; protected set; }
        public string Description { get; protected set; }

        protected InteractableObject(string name, string desc)
        { 
            Name = name;
            Description = desc;
        }

        public abstract void Interact();
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

    internal class Program
    {
        static void Main(string[] args)
        {
          
        }
    }
}
