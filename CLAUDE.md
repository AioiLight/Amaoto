# CLAUDE.md - Amaoto Repository Guide

## Project Overview

**Amaoto** (アマオト) is a C# game programming library that wraps [DXLib](https://dxlib.xsrv.jp/) — a Windows DirectX game development library — to provide a more developer-friendly .NET API. It targets Windows game developers using .NET Framework 4.7.2.

- **Language**: C# (.NET Framework 4.7.2)
- **Output**: Class library (`Amaoto.dll`)
- **Platform**: Windows x64 only
- **License**: MIT
- **Primary audience**: Japanese-speaking game developers (README and code comments are in Japanese)

---

## Repository Structure

```
Amaoto/
├── Amaoto.sln                   # Visual Studio solution
├── README.md                    # Project documentation (Japanese)
├── LICENSE                      # MIT license
├── Amaoto/                      # Main class library project
│   ├── Amaoto.csproj            # MSBuild project file (.NET 4.7.2, x64)
│   ├── DxLibWDotNet.dll         # DXLib .NET wrapper (local binary dependency)
│   ├── packages.config          # NuGet packages (Newtonsoft.Json 13.0.1)
│   ├── Properties/
│   │   └── AssemblyInfo.cs      # Assembly version (currently 1.3.0.0)
│   ├── Animation/               # Easing/animation classes
│   ├── GUI/                     # GUI framework (layout, controls)
│   └── *.cs                     # Core library source files
```

### Core Source Files

| File | Purpose |
|------|---------|
| `Amaoto.cs` | Static init/teardown (`Init()`, `End()`), main loop helpers |
| `Texture.cs` | Core texture class (rotation, scale, opacity, anchor points) |
| `VirtualScreen.cs` | Offscreen render target |
| `FontRender.cs` | Custom font rendering (not DXLib's built-in) |
| `AnimateTexture.cs` | Frame-based texture animation |
| `AtlasTexture.cs` | Sprite atlas/sheet support |
| `ResizeableBoxTexture.cs` | 9-slice texture for resizable UI |
| `TextureMask.cs` | Texture masking |
| `Key.cs` | Keyboard input state (static) |
| `Mouse.cs` | Mouse position and button state (static) |
| `Input.cs` | Text input field (wraps DXLib input) |
| `Sound.cs` | Audio playback (load, play, volume, pitch) |
| `Movie.cs` | Video playback |
| `Scene.cs` | Base class for game scenes |
| `SceneManager.cs` | Scene lifecycle management (add/remove/insert) |
| `Counter.cs` | High-precision timer using DXLib performance counter |
| `FPSCounter.cs` | FPS measurement |
| `ConfigManager.cs` | JSON config save/load (via Newtonsoft.Json, camelCase) |
| `Logger.cs` | Fluent logging wrapper around DXLib error logging |
| `DXLibUtil.cs` | Blend mode and drawing utilities (static) |
| `AmaotoUtil.cs` | General utility functions (static) |
| `LayoutBuilder.cs` | Fluent layout building helper |
| `Filter.cs` | Graphics filter operations |
| `IPlayable.cs` | Interface for Sound and Movie |
| `ITextureReturnable.cs` | Interface for objects returning textures |

### Animation Module (`Animation/`)

| File | Purpose |
|------|---------|
| `Animator.cs` | Abstract base class for all animations |
| `Sequencer.cs` | Chains multiple Animator instances in order |
| `Linear.cs` | Linear interpolation |
| `EaseIn.cs` | Ease-in curve |
| `EaseOut.cs` | Ease-out curve |
| `EaseInOut.cs` | Ease-in-out curve |
| `EaseInBack.cs` | Ease-in with overshoot |
| `EaseOutBack.cs` | Ease-out with overshoot |
| `FadeIn.cs` | Opacity fade-in |
| `FadeOut.cs` | Opacity fade-out |
| `Blank.cs` | No-op / delay placeholder |

### GUI Module (`GUI/`)

| File | Purpose |
|------|---------|
| `DrawPart.cs` | Abstract base class for all GUI elements |
| `Container.cs` | Holds and manages child `DrawPart` elements |
| `Row.cs` / `Column.cs` | Horizontal/vertical layout containers |
| `Left.cs` / `Right.cs` / `Center.cs` | Alignment wrappers |
| `Button.cs` | Clickable button with animation support |
| `CheckBox.cs` | Toggle checkbox |
| `NumericUpDown.cs` | Numeric spinner control |
| `Image.cs` | Static image display |
| `Tab.cs` | Tabbed panel container |
| `Scroller.cs` | Scrollable content area |
| `GUINormalize.cs` | GUI coordinate normalization |
| `MouseClickEventArgs.cs` | Event args for mouse click events |

---

## Build System

### Requirements

- **Visual Studio 2016+** (or MSBuild 15+)
- **.NET Framework 4.7.2** SDK
- **Windows** (DXLib is Windows-only)
- NuGet package restore (handled automatically by VS)

### Build Commands

```bash
# Build Release (x64)
msbuild Amaoto.sln /p:Configuration=Release;Platform=x64

# Build Debug (x64)
msbuild Amaoto.sln /p:Configuration=Debug;Platform=x64
```

### Output Locations

- **Debug**: `Amaoto/bin/x64/Debug/Amaoto.dll`
- **Release**: `Amaoto/bin/x64/Release/Amaoto.dll` + `Amaoto.xml` (XML docs)

### NuGet Dependency

- `Newtonsoft.Json 13.0.1` — used in `ConfigManager.cs` for JSON serialization with camelCase naming strategy

---

## Code Conventions

### Naming

- **Classes, public properties, methods**: PascalCase
- **Private fields**: camelCase
- **Enum values**: PascalCase
- **Identifiers and comments**: Japanese is common; English is acceptable
- XML documentation comments (`/// <summary>`) on all public members

### Design Patterns in Use

- **Static utility classes**: `Key`, `Mouse`, `Input`, `Amaoto`, `ConfigManager`, `DXLibUtil`, `Logger`
- **Abstract base classes**: `Animator` (animations), `DrawPart` (GUI elements)
- **Interfaces for polymorphism**: `IPlayable` (Sound/Movie), `ITextureReturnable`
- **Fluent interface**: `Logger` supports method chaining
- **Event-driven**: GUI components expose `OnMouseDown`, `OnMouseUp` C# events
- **Disposable resources**: `Texture`, `Sound`, `VirtualScreen` implement `IDisposable`
- **Composition**: GUI containers hold lists of child `DrawPart` elements

### Error Handling

- DXLib init failure throws `Exception("DXLib initialize failed")`
- DXLib return code `-1` indicates failure; check return values before use
- Use null-coalescing operators for defensive null checks

### Project-Specific Notes

- `AllowUnsafeBlocks` is enabled in the project file (required for some DXLib interop)
- The library is **x64 only** — do not change platform targets
- `DxLibWDotNet.dll` is a local binary (not on NuGet); it must be present in the project directory
- `ConfigManager` serializes with `CamelCasePropertyNamesContractResolver` — property names in JSON are camelCase regardless of C# naming

---

## Testing

There is currently **no automated test suite**. The library is validated manually by building consumer game projects against it. When adding new features:

1. Build the library successfully (no compiler errors or warnings)
2. Verify the XML documentation compiles without errors in Release mode
3. Manually test in a DXLib-based game project if behavioral changes are made

---

## Common Development Tasks

### Adding a New Animation Easing

1. Create a new file in `Animation/` (e.g., `BounceOut.cs`)
2. Inherit from `Animation.Animator`
3. Override the required abstract methods
4. Follow the pattern of existing easings (e.g., `EaseOut.cs`)

### Adding a New GUI Control

1. Create a new file in `GUI/` (e.g., `Slider.cs`)
2. Inherit from `GUI.DrawPart`
3. Implement `Update()` and `Draw()` methods
4. Add mouse event handling following `Button.cs` as a reference
5. Register child elements through the `Container` pattern if needed

### Adding a New Core Feature

1. Create a new `.cs` file in `Amaoto/` (root)
2. Use namespace `Amaoto`
3. Add XML documentation to all public members
4. If wrapping DXLib functionality, check return codes for `-1` failure

### Modifying ConfigManager

`ConfigManager` uses `Newtonsoft.Json` with `CamelCasePropertyNamesContractResolver`. All config classes should use standard PascalCase C# properties — serialized JSON keys will be camelCase automatically.

---

## What to Avoid

- **Do not change the target platform from x64** — DXLib requires 64-bit
- **Do not remove `AllowUnsafeBlocks`** — required for DXLib pointer interop
- **Do not add test projects using GUI/Windows forms test runners** — DXLib has a Windows message loop that conflicts
- **Do not rename or move `DxLibWDotNet.dll`** — the project references it by relative path
- **Do not upgrade Newtonsoft.Json past 13.x** without verifying .NET 4.7.2 compatibility

---

## Git Workflow

- Main development branch: `master`
- The repository is mirrored at `https://github.com/AioiLight/Amaoto`
- Commit messages are typically in Japanese (follow existing style when contributing)
- No CI/CD pipelines are configured; all validation is manual
