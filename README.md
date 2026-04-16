# Bare.Infrastructure.Controls

Low-level text surface primitives for character-grid rendering — the compositing layer of the Bare ecosystem.

## Status

Early-stage library. Public, usable, but the API surface is intentionally small and may evolve.

## Purpose

Provides buffer-based text surfaces, rectangular regions, and a canvas compositor that allow higher-level UI code to build screen layouts as character grids and render them to an `IUiOutput` in a single pass. This is the rendering substrate that `Bare.Infrastructure.UI` builds its interactive components on top of.

## What is inside

### Surface and buffer

| Type | Kind | Description |
|---|---|---|
| `TextSurface` | sealed class | Fixed-size `char[,]` buffer with `Fill`, `SetText`, `GetLine`, and `RenderTo(IUiOutput)`. Handles clipping and negative-offset writes. |
| `SurfaceCanvas` | sealed class | Compositor: owns a list of `ISurfaceRegion` instances, builds a merged `TextSurface` via `BuildSurface()`, and renders the result to `IUiOutput`. |

### Region abstractions

| Type | Kind | Description |
|---|---|---|
| `ISurfaceRegion` | interface | Positionable rectangle (`Left`, `Top`, `Width`, `Height`) with `RenderTo(TextSurface)` — the basic layout unit. |
| `ITextRegion` | interface | Extends `ISurfaceRegion` with `Lines[]` — a region that carries its own text content. |
| `SurfaceRegion` | class | Mutable `ISurfaceRegion` implementation with position/size setters. |
| `TextRegion` | class | `ITextRegion` implementation with mutable lines. |
| `TextRegionOutputBase` | abstract class | Base for writing text into a `TextRegion` via the `IUiOutput` pattern. |

### Consumers and composition

| Type | Kind | Description |
|---|---|---|
| `ITextRegionConsumer` | interface | Accepts a `TextRegion` and writes content into it. |
| `ITextRegionConsumerCollection` | interface | Manages a collection of `ITextRegionConsumer` instances. |
| `EmptyTextRegionConsumers` | class | Empty collection implementation (null-object pattern). |
| `BackgroundSurface` | class | Renders a background fill into a `TextSurface`. |

### Data structures

| Type | Kind | Description |
|---|---|---|
| `CappedQueue<T>` | class | Thread-safe queue with a configurable `MaxCount`. Oldest items are evicted when the cap is reached. Raises `NewItem` / `NewItems` events. |
| `RenderCappedQueue<T>` | class | Appears to integrate `CappedQueue` with region-based rendering. |

### Identity

| Type | Kind | Description |
|---|---|---|
| `InfrastructureControlsIdentity` | static class | Assembly identity constant (`Name = "Bare.Infrastructure.Controls"`) |

No external NuGet dependencies. References `Bare.Primitive.UI`. Targets .NET 8+ (or .NET 10 when available).

## Design goals

- **Buffer-first rendering**: all layout is computed in-memory on `TextSurface` buffers before being flushed to the terminal. No direct console calls inside controls.
- **Composable regions**: screens are assembled from independent `ISurfaceRegion` pieces via `SurfaceCanvas`, then rendered in one pass.
- **Clipping by construction**: `TextSurface.SetText` and `RenderTo` clip automatically — out-of-bounds writes are silently ignored, never throwing.

## Relationship to other Bare repositories

```
Bare.Primitive.Kernel
  └─ Bare.Primitive.UI
       └─ Bare.Infrastructure.Controls   ← you are here
            └─ Bare.Infrastructure.UI
```

`Bare.Infrastructure.Controls` sits between the Primitive UI contracts and the Infrastructure UI interactive layer. It depends on `Bare.Primitive.UI` for `IUiOutput` and `UiText`. It is referenced by `Bare.Infrastructure.UI`, which adds menus, pagers, and interactive flows on top of these surfaces.

## What it does not try to do

- Provide interactive UI flows (menus, pagers, pickers) — those belong in `Bare.Infrastructure.UI`.
- Handle user input — that is the concern of `Bare.Primitive.UI`'s `IUiInput` / `IUiKeyInput`.
- Know about specific applications or business logic.

## Build / development notes

```sh
dotnet build Bare.Infrastructure.Controls.slnx
dotnet test Bare.Infrastructure.Controls.slnx
```

No special build flags or external tooling required.

## License

AGPL-3.0