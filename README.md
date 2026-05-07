# 🐣 Easter Hunt

A Unity-based 3D Easter egg hunting game where the player explores environments to collect hidden Easter eggs while avoiding hostile enemies. Built as a fun seasonal project, the game features player abilities, interactive objects, enemy AI, and a save system.

---

## 🎮 Gameplay

- Explore 3D environments to find and collect hidden Easter eggs
- Avoid or outsmart hostile AI enemies that patrol and chase
- Unlock and use special **Abilities** collected throughout the world
- Interact with objects such as **Doors** and collectables
- Progress is saved between sessions

---

## 🗂️ Project Structure

```
Assets/
├── Animations/         # Animation clips and controllers
├── Dev/                # Development/debug assets
├── Kevin Iglesias/     # Third-party character animation assets (Kevin Iglesias)
├── Models/             # 3D models
├── Prefabs/            # Reusable prefabs (e.g. Projectile)
├── Rukha93/            # Third-party assets (Rukha93)
├── Scenes/             # Unity scenes
├── ScriptableObjects/  # Data-driven ScriptableObjects
├── Scripts/
│   ├── AI/
│   │   ├── EnemyMovement.cs     # Handles enemy patrol and navigation
│   │   └── HostileAI.cs         # Hostile enemy state machine and chase logic
│   ├── Player/
│   │   ├── Ability.cs           # Base ability class
│   │   ├── AbilityCollectable.cs # Pickup logic for ability items
│   │   ├── CollectEggs.cs       # Egg collection and counter logic
│   │   ├── Interactor.cs        # Player interaction system
│   │   ├── NumberGenerator.cs   # Utility: random number generation
│   │   └── PlayerController.cs  # Player movement and input handling
│   ├── Saving/                  # Save/load system
│   ├── AnimatedCube.cs          # Animated cube helper script
│   └── Door.cs                  # Door interaction and animation
├── Settings/           # Unity render/project settings assets
└── InputSystem_Actions # Unity Input System action maps
Packages/               # Unity package manifest
ProjectSettings/        # Unity project settings
```

---

## 🛠️ Built With

- **Engine:** [Unity](https://unity.com/) (with the new Input System)
- **Language:** C#
- **Third-party Assets:**
  - [Kevin Iglesias](https://www.keviniglesias.com/) — Character animations
  - Rukha93 — Environment/visual assets

---

## 🚀 Getting Started

### Prerequisites

- Unity **2021.3 LTS** or newer (check `ProjectSettings/ProjectVersion.txt` for the exact version)
- Git LFS (recommended for large binary assets)

### Installation

1. Clone the repository:
   ```bash
   git clone https://github.com/SSpiderlxx/EasterHunt.git
   ```
2. Open the project in **Unity Hub**.
3. Open the main scene from `Assets/Scenes/`.
4. Press **Play** to run the game in the editor.

---

## 🎯 Features

| Feature | Description |
|---------|-------------|
| 🥚 Egg Collection | Hunt for Easter eggs scattered across the map |
| 🧠 Enemy AI | Enemies patrol and chase the player using `HostileAI.cs` |
| ⚡ Abilities | Collect special ability pickups to gain new powers |
| 🚪 Interactive Doors | Doors that open via the interaction system |
| 💾 Save System | Persistent save/load functionality |
| 🎮 New Input System | Unity Input System for controller and keyboard support |

---

## 📁 Key Scripts

### Player
- **`PlayerController.cs`** — Manages movement, jumping, and input via Unity's new Input System
- **`CollectEggs.cs`** — Tracks egg pickups and updates the UI counter
- **`Interactor.cs`** — Handles proximity-based interactions with world objects
- **`Ability.cs` / `AbilityCollectable.cs`** — Base class and pickup for player abilities

### AI
- **`HostileAI.cs`** — State machine controlling idle, patrol, and chase behaviours
- **`EnemyMovement.cs`** — NavMesh-based movement for enemies

### World
- **`Door.cs`** — Animated door with trigger-based or interaction-based opening
- **`Saving/`** — Serialises and deserialises player progress to disk

---

## 🤝 Third-Party Licences

This project uses assets from third-party creators. Please respect their individual licence terms:
- **Kevin Iglesias** animations — see `Assets/Kevin Iglesias/`
- **Rukha93** assets — see `Assets/Rukha93/`

---

## 👤 Author

**Leon Twemlow**
- GitHub: [@SSpiderlxx](https://github.com/SSpiderlxx)
- Email: [leontwemlow@gmail.com](mailto:leontwemlow@gmail.com)
- Website: [twemlow.dev](https://twemlow.dev)

---

*Happy hunting! 🐰*
