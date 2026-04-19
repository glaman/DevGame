// MapPoint.cs
namespace TurnBasedRPG
{
    public class MapPoint
    {
        public int Index { get; set; }
        public string Name { get; set; }           // e.g. "Point A", "The Whispering Woods"
        public string Letter { get; set; }         // A, B, C, D...
        public bool IsUnlocked { get; set; }
        
        // Optional: What this point rewards
        public string HeroToUnlock { get; set; }   // Name of helper hero unlocked by completing this point
        public int RecommendedLevel { get; set; }

        public MapPoint(int index, string name, string letter)
        {
            Index = index;
            Name = name;
            Letter = letter;
            IsUnlocked = false;
        }
    }
}
