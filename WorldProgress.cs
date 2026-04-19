// WorldProgress.cs
using System.Collections.Generic;

namespace TurnBasedRPG
{
    public class WorldProgress
    {
        public List<MapPoint> MapPoints { get; set; } = new List<MapPoint>();
        
        // Quick access to furthest progress
        public int HighestUnlockedIndex { get; set; } = 0;

        public WorldProgress()
        {
            // Initialize default points (you can expand this later)
            MapPoints.Add(new MapPoint(0, "Point A - The Starting Glade", "A") { IsUnlocked = true });
            MapPoints.Add(new MapPoint(1, "Point B - The Shadowed Path", "B"));
            MapPoints.Add(new MapPoint(2, "Point C - The Crystal Ruins", "C"));
            MapPoints.Add(new MapPoint(3, "Point D - The Storm Peaks", "D"));
        }

        public MapPoint GetPoint(int index)
        {
            return MapPoints.Find(p => p.Index == index);
        }

        public bool UnlockPoint(int index)
        {
            var point = GetPoint(index);
            if (point != null && !point.IsUnlocked)
            {
                point.IsUnlocked = true;
                if (index > HighestUnlockedIndex)
                    HighestUnlockedIndex = index;
                return true;
            }
            return false;
        }
    }
}
