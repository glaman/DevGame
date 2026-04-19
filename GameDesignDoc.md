# Turn-Based RPG - Game Design Document

**Project:** Turn-Based RPG (Working Title TBD)  
**Date:** April 12, 2026  
**Current Phase:** Core Systems Design – Stats, Classes, Abilities, and Combat Rules  
**Engine:** MonoGame DesktopGL (C# .NET 9) – Cross-platform (Linux primary development)

---

## 1. Game Overview

- **Genre**: Turn-based tactical RPG with exploration, party management, and equipment-driven progression.
- **Core Loop**: Main Hub (Dashboard) → Map → Battle → Rewards → Dashboard.
- **Player Role**: The leader ("You") who customizes their entire moveset through equipment.
- **Helpers**: Recruited allies with strong class identities and fixed abilities.
- **Key Rules**:
  - Helpers have exactly **2 abilities** + Basic Attack + Defend.
  - Player has **4 equipment slots** that grant abilities + Basic Attack + Defend.
  - Battles use **round-based turns** with speed-influenced random initiative.
  - Cooldowns are measured in full battle rounds.
  - Basic Attack is class-specific.
  - Defend / Stand Ground is always available.

---

## 2. Major Systems Status

| System                    | Status          | Key Decisions Locked In                                                                 | Still Needs Work                  |
|---------------------------|-----------------|------------------------------------------------------------------------------------------|-----------------------------------|
| Save System               | Fully Designed  | 8 slots, JSON, event-based auto-save, **no mid-battle saves**                            | Implementation                    |
| Hero Hierarchy            | Fully Designed  | Entity → Hero → PlayerHero / HelperHero                                                  | Implementation                    |
| Class System              | Fully Designed  | 5 starting classes with distinct identities                                              | Implementation                    |
| Stats System              | Fully Designed  | Primary: Might, Finesse, Wit, Vigor<br>Secondary: Speed, Strike, Arcane, Hardiness, Resolve | Implementation                    |
| Battle Round & Turn Order | Fully Designed  | Speed-based random initiative, show current + next 2–3 units                             | Implementation                    |
| Ability System            | Fully Designed  | Helpers: 2 abilities + Basic + Defend<br>Player: 4 equipment abilities + Basic + Defend | Implementation                    |
| Cooldown Mechanics        | Fully Designed  | Round-based, current round does **not** count, decreases at end of each full round       | Implementation                    |
| Ability Outcomes          | Fully Designed  | Hybrid (flat + percentage), class-based multipliers, role distance penalties             | Implementation                    |
| Buffs & Debuffs           | Designed        | Effects with chance to stick, duration in rounds, stat modifications                     | Implementation                    |
| Equipment System          | Fully Designed  | Player-only, 4 slots                                                                         | Implementation                    |
| Inventory & Consumables   | Fully Designed  | Shared inventory                                                                             | Implementation                    |
| Map & Progression         | Fully Designed  | Unlockable Points                                                                            | Implementation                    |
| Main Hub (Dashboard)      | Fully Designed  | Single large overview screen with panels                                                     | Implementation                    |
| Screen Flow & Navigation  | Fully Designed  | Smart ESC behavior, history limited to 2 previous screens                                    | Implementation                    |
| Art Style                 | Defined         | Bold black line art + cel-shading + hand-drawn 3D depth                                      | Asset creation                    |

---

## 3. Stats System

### Primary Stats
- **Might** – Raw physical power and presence
- **Finesse** – Agility, precision, timing, and accuracy
- **Wit** – Cleverness, insight, magical aptitude, and tactical thinking
- **Vigor** – Endurance, life force, and overall resilience

### Secondary Stats
- **Speed** – How quickly a unit acts in combat (major factor in turn order)
- **Strike** – Overall physical attack power
- **Arcane** – Magical and ability power
- **Hardiness** – Overall toughness and damage reduction
- **Resolve** – Resistance to status effects and debuffs

---

## 4. Classes & Identity

| Class            | Primary Strength      | Key Secondary Stats             | Playstyle Flavor                          |
|------------------|-----------------------|---------------------------------|-------------------------------------------|
| **Brawler**      | Might                 | Strike, Hardiness               | Close-range brute, high damage tank       |
| **Sharpshooter** | Finesse               | Speed, Strike                   | Ranged precision, critical hits           |
| **Healer**       | Vigor + Wit           | Hardiness, Resolve              | Support, healing, team sustain            |
| **Magician**     | Wit                   | Arcane, Resolve                 | Spellcasting, area control, burst damage  |
| **Jack-of-all**  | Balanced              | Balanced                        | Versatile, adaptable                      |

---

## 5. Combat Actions & Abilities

Every unit has access to these actions on their turn:

- **Basic Attack** — Always available, no cooldown. **Class-specific version**.
- **Signature Ability** — Mid-level ability with **2–4 round cooldown**.
- **Ultimate / Heavy Hitter** — Powerful tide-swinging ability with **5+ round cooldown**.
- **Defend / Stand Ground** — Always available. Reduces incoming damage this round.

**Cooldown Rule:**
- Cooldown starts **after** the ability is used.
- The round in which the ability is used **does not** count toward the cooldown.
- Cooldown decreases by 1 at the **end of every full round**.

**Ability Outcome System (Hybrid):**
- Any class can attempt any type of ability.
- Effectiveness is heavily influenced by role distance from the class’s primary strength.
- Direct damage mostly uses **flat numbers**.
- Healing and major support abilities often use **percentage-based** effects.
- Harsh penalties for off-role actions (e.g., Brawler healing is severely reduced).

---

## 6. Buffs & Debuffs (Effects System)

Abilities can apply pure effects or combine them with damage/healing.

**Effect Properties:**
- Name (e.g., Strengthen, Slow, Poison)
- Target (Self, Ally, Enemy, All Allies, etc.)
- Stat Modified (Might, Finesse, Wit, Vigor, Speed, Strike, Arcane, Hardiness, Resolve, etc.)
- Value (flat or percentage)
- Duration (in rounds)
- Chance to Apply (percentage)

**Duration Rule:**
- Effects tick down at the **end of every full round**.

---

## 7. Screen Flow & Navigation

**Main Screens:**
- Title Screen
- Player Profile Screen (create new hero or view current profile)
- Main Hub (Dashboard)
- Map Screen
- Battle Screen
- Equipment Screen
- Hero Gallery Screen

**Dev Navigation (available on all screens):**
- T = Title
- D = Dashboard / Main Hub
- M = Map
- B = Battle
- E = Equipment
- H = Hero Gallery
- ESC = Go back to previous screen (or Exit if on Title or Main Hub)

**Smart ESC Behavior:**
- On Title or Main Hub → Exit game
- On any other screen → Return to previous screen (history limited to 2 previous screens)

---

## 8. Save System

- **Number of Slots**: 8
- **Format**: JSON (during development)
- **Slot Naming**: Automatically uses the PlayerHero’s name
- **Auto-Save Triggers**: After battle ends, equipment changes, new map point, new hero, inventory changes
- **Important Rule**: Never save during an active battle

---

## 9. Development & Debugging Features

- Robust error trapping with clear logging
- Live debug interface with real-time stat display and tweaking
- Command-line arguments: `--debug`, `--show-stats`
- Hot-reloading support

---

## 10. Art Asset Requirements

- Bold black line art with comic book / hand-drawn ink feel
- Cel-shading with vibrant colors and strategic lighting
- Strong rim lighting for pseudo-3D depth

---

## 11. Recommended Next Steps

1. Create example abilities for each class using the hybrid system
2. Define leveling and stat growth rules
3. Expand Player Profile Screen for new character creation
4. Implement battle system with turn order and effects
5. Continue building and testing screens with navigation

---

**Last Updated:** April 12, 2026  
*This is a living document. Update it as decisions are made and implementation progresses.*

---

This version now fully includes:
- The original Major Systems Status Table (restored and updated)
- All the stats we finalized (including Hardiness)
- The ability system with correct counts for Helpers vs Player
- The exact cooldown rule you described
- The hybrid outcome system with role distance penalties
- Buffs & debuffs / effects system

Would you like me to expand any section right now (e.g., add example abilities, leveling rules, or more detail to the Player Profile Screen)?

Or shall we continue refining something specific?

Let me know how you'd like to proceed!
