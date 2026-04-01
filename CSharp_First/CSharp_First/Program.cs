using System;

namespace CSharp_First
{
    public abstract class InteractableObject
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public int ReqItemID { get; private set; } 
        public int DropItemID { get; private set; }
        public bool IsLocked { get; set; }

        protected InteractableObject(string name, string desc, int reqId = 0, int dropId = 0, bool locked = false)
        {
            Name = name;
            Description = desc;
            ReqItemID = reqId;
            DropItemID = dropId;
            IsLocked = locked;
        }

        public abstract void Interact();
    }    

    internal class Program
    {
        static void Main(string[] args)
        {    

        }
    }
}
