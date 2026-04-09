# Pacman (Unity) - Zenject DI + GameEvents Flow

This project uses **Zenject** for Dependency Injection and a centralized **`GameEvents`** event-aggregator to decouple gameplay systems.

---

## 1. Architecture Summary

- **Container bootstrap**: `Assets/Scripts/Installers/GameInstaller.cs`
- **Event hub**: `Assets/Scripts/Core/GameEvents.cs`
- **Game orchestration**: `Assets/Scripts/Core/GameManager.cs`
- **UI reaction layer**: `Assets/Scripts/Core/UIManager.cs`
- **Event publishers**:
  - `Assets/Scripts/Core/Ghosts/Ghost.cs`
  - `Assets/Scripts/Core/Tiles/Pellets.cs`
  - `Assets/Scripts/Core/Tiles/PowerPellets.cs`
- **Player movement**:
  - `Assets/Scripts/Core/Player/Pacman.cs`
  - `Assets/Scripts/Core/Player/Movement.cs`

---

## 2. Current DI Bindings (`GameInstaller`)

`GameInstaller` currently binds:

- `GameEvents` as `AsSingle().NonLazy()`
- `GameManager` from scene hierarchy as singleton
- `UIManager` from scene hierarchy as singleton
- `Movement` from scene hierarchy (currently singleton binding)
- `Pellets` from scene hierarchy as cached

Zenject then injects dependencies into MonoBehaviours via `[Inject]` `Construct(...)`.

---

## 3. Runtime Flow (Exact Gameplay Event Flow)

### Startup flow

1. Unity loads scene with `SceneContext`.
2. `GameInstaller.InstallBindings()` registers dependencies.
3. Zenject injects:
   - `GameEvents` into `GameManager`, `Ghost`, `Pellets`, `Movement`, etc.
   - `GameManager` + `GameEvents` into `UIManager`.
4. `GameManager.Start()` subscribes to `GameEvents` and calls `StartNewGame()`.
5. `UIManager.Start()` initializes score/lives/game-over UI from `GameManager`.

### Pellet eaten flow

1. `Pellets.OnTriggerEnter2D()` detects Pacman.
2. `Pellets.Eat()` calls `gameEvents.RaisePelletEaten(this)`.
3. `GameManager.OnPelletEaten(...)` handles:
   - Deactivate pellet
   - Add score
   - If no pellets remain, start next round after delay
4. `GameManager.SetScore(...)` emits `ScoreChanged`.
5. `UIManager` receives `ScoreChanged` and updates `scoreText`.

### Power pellet flow

1. `PowerPellets.Eat()` calls `RaisePowerPelletEaten(this)`.
2. `GameManager.OnPowerPelletEaten(...)`:
   - Enables frightened state on ghosts
   - Reuses pellet scoring flow
   - Schedules ghost multiplier reset after duration

### Ghost collision flow

1. `Ghost.OnCollisionEnter2D()` checks collision with Pacman.
2. If frightened: `RaiseGhostEaten(this)`, else: `RaisePacmanEaten()`.
3. `GameManager` handles event:
   - Ghost eaten -> add multiplied points
   - Pacman eaten -> reduce lives, reset state or game over
4. `GameManager` emits `LivesChanged` / `GameOverChanged`.
5. `UIManager` updates lives/game-over UI.

### Game over flow

1. `GameManager.GameOver()` disables entities and emits `GameOverChanged(true)`.
2. `Movement` listens to `GameOverChanged` and freezes motion.
3. `UIManager.Update()` listens for key press and calls `StartNewGame()`.

---

## 4. Why This DI Design Works

- Removes `FindObjectOfType` runtime coupling.
- Centralizes communication through `GameEvents`.
- Keeps gameplay classes focused on **publishing or handling domain events**.
- Makes components easier to test and refactor independently.

---

## 5. Scene Setup Checklist

- Add `SceneContext` to the scene.
- Add `GameInstaller` to `SceneContext` installers.
- Ensure exactly one `GameManager` and one `UIManager` in scene hierarchy.
- Ensure gameplay objects (`Ghost`, `Pellets`, `PowerPellets`, `Pacman`) are active in hierarchy so Zenject can inject them.

---

## 6. Recommended Next Cleanup

To make DI fully correct and predictable:

1. Avoid binding `Movement` as a global singleton (`AsSingle`) for multi-entity scenes.
2. Prefer per-object local component references (`GetComponent`) for own movement, or identifier-based Zenject bindings.
3. Keep `GameEvents` as the only global singleton event hub.

---
