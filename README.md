# 🗡️ Asaki - GMTK Game Jam 2026

![Unity Version](https://img.shields.io/badge/Unity-6-lightgrey)
![C#](https://img.shields.io/badge/Language-C%23-blue)
![Event](https://img.shields.io/badge/Event-GMTK%20Game%20Jam%202026-critical)

**Asaki** is a game developed as a submission for the **GMTK Game Jam 2026**. Built under the pressure of a tight jam deadline, this project showcases rapid prototyping, collaborative game design, and the implementation of engaging core mechanics using Unity 6.

🎮 **[Play Asaki on Itch.io!](https://krishna-tyagi.itch.io/asaki)**

---

## 🌟 Concept & Theme

- **Jam Theme:** Built to Scale (Countdown)
- **Our Interpretation:** We took the concept of a "Countdown" and tied it directly to the state of the game world. As the timer ticks down, the environment dynamically reacts and changes, increasing the tension and altering the level layout. If the player fails to complete the objective before the countdown reaches zero, the world collapses and is completely destroyed. 

## 🎮 Gameplay Highlights

- **Dynamic Environment:** The world actively shifts and deteriorates based on the countdown timer, forcing the player to adapt their platforming strategies on the fly.
- **Smart Enemy AI:** Enemy patrol movement and edge-detection are powered by **Raycast2D**. By firing invisible physics rays (that "ray thingie"!), enemies intelligently detect the edges of platforms to turn around and avoid falling off, as well as spot the player in their line of sight.
- **Race Against Time:** A frantic core loop where every second dictates the physical reality of the level around you. 

---

## 🛠️ Development & Technologies

This game was built from scratch during the jam weekend using:
- **Game Engine:** Unity 6 (URP 2D)
- **Language:** C#
- **Version Control:** Git & GitHub
- **Key Systems Implemented:**
  - Timer-based world state management system.
  - `Physics2D.Raycast` for dynamic AI decision-making and obstacle detection.
  - Unity Event system for modular game logic and destruction triggers.

---

## 👥 The Team

**Asaki** was brought to life by a team of dedicated developers and friends:

- **Ishitva Pandey** ([@IshitvaPandeyDev](https://github.com/IshitvaPandeyDev)) - **Scripter**
- **Krishna Tyagi** ([@Krishna-Tyagi](https://krishna-tyagi.itch.io/)) - **Level Designer**
- **Kanishka Vats** ([@glitchcorewitch](https://itch.io/profile/glitchcorewitch)) - **Background and Asset Creator**

---

## 🚀 Installation & Setup (Source Code)

If you'd like to explore the source code or run the project locally in Unity:

1. Clone this repository:
   ```bash
   git clone [Insert Repository Link Here]
