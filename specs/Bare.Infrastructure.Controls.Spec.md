# Bare.Infrastructure.Controls - Spécification Initiale

## Objectif
Fournir des contrôles de base de rendu texte au-dessus de `Bare.Primitive.UI`.

## Portage en cours (BareTextUI / BareSync)
- Porter les primitives de composition écran de `BareTextUI` vers une API infra stable.
- Extraire des briques génériques de `BareSync` pour éviter le couplage app dans la future réintégration.

## Surface API MVP
- `TextSurface` : buffer 2D de caractères.
  - `Fill`
  - `SetText`
  - `GetLine`
  - `RenderTo`
- `InfrastructureControlsIdentity` : identifiant constant.

## Surface API enrichie
- `ISurfaceRegion` / `SurfaceRegion`
  - Contrat de région rendable dans une surface.
  - Gestion des bornes (`Left/Top/Width/Height`) avec clamp minimal.
- `TextRegion`
  - Région textuelle alimentée par des consommateurs (`ITextRegionConsumerCollection`).
  - Stockage de lignes + rendu paddé/clippé dans `TextSurface`.
- `SurfaceCanvas`
  - Composition de plusieurs régions dans une seule surface finale.
- `BackgroundSurface`
  - Région de fond (fill) + overlay de régions enfants.
- `ITextRegionConsumer` / `ITextRegionConsumerCollection`
  - Pipeline de production de contenu textuel, inspiré du modèle `ViewFieldConsumer` de BareTextUI.
- `CappedQueue<T>`
  - File bornée thread-safe avec éviction FIFO et événements `NewItem` / `NewItems`.
- `RenderCappedQueue<T>`
  - Adaptateur de queue bornée vers `ITextRegionConsumerCollection`.
- `TextRegionOutputBase`
  - Helpers de formatage temps + utilitaires de nettoyage de lignes/ranges.

## Contraintes
- Rendu déterministe, testable en mémoire.
- Gestion minimale du clipping horizontal.
- Composants portables Windows/Linux (aucun appel console direct dans les contrôles).
- Composants découplés des domaines applicatifs (pas de dépendance BareSync/BareGridCrawler).
