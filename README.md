# Treasure Hunter - 2D Action Platformer Game (In Progress)

## 📌 Project Overview
A personal project focused on implementing clean code, scalable architecture, system performance optimization, and enhancing the overall "Game-feel" for a 2D action-platformer game.

## 🛠️ Architecture & Core Technologies
The project is designed with a clean modular structure to easily scale and integrate new gameplay features:

*   **Finite State Machine (FSM):** Manages complex states for both the Player (Idle, Run, Jump, Fall, Attack, Dash) and Enemy AI. Decoupling the logic of each state makes the codebase highly extensible, easier to maintain, and completely avoids messy, nested IF-ELSE conditions.
*   **Object Pooling System:** Implemented for frequently instantiated/destroyed objects such as enemies, visual effects (VFX), and collectible items (Treasures). This drastically reduces the overhead of constant `Instantiate` / `Destroy` calls, optimizes memory usage, and prevents runtime spikes caused by Garbage Collection.
*   **Cinemachine Integration:** Configured smooth camera follow, dead zones, camera boundary constraints (Camera Confiner), and real-time screen shake (Camera Shake) to enhance impact and responsiveness during combat or taking damage.

---
## Game Features
<!-- Video 1 -->
<video src="https://github.com/user-attachments/assets/7899aa8f-8045-4e32-84c0-c40fc2709497" width="100%" autoplay loop muted></video>

<!-- Video 2 -->
<video src="https://github.com/user-attachments/assets/4ad687b5-5e59-4493-874d-e9a084267121" width="100%" autoplay loop muted></video>

<!-- Video 3 -->
<video src="https://github.com/user-attachments/assets/84ea5ffc-706a-472d-9887-514c7378ebc0" width="100%" autoplay loop muted></video>

<!-- Video 4 -->
<video src="https://github.com/user-attachments/assets/afb9cc43-4073-4fe7-843f-3554f1779ff3" width="100%" autoplay loop muted></video>

<!-- Video 5 -->
<video src="https://github.com/user-attachments/assets/e987a511-8459-4545-b3de-1ab10637bcd5" width="100%" autoplay loop muted></video>

<!-- Video 6 -->
<video src="https://github.com/user-attachments/assets/df741ede-1072-4f9b-807c-b6a0900a5621" width="100%" autoplay loop muted></video>

<!-- Video 7 -->
<video src="https://github.com/user-attachments/assets/695f9b76-ee44-475a-bc63-d81bb51f5c1c" width="100%" autoplay loop muted></video>
