PhysX SDK snippets adapted to Mochi.PhysX
=======================================================================================================================

This directory contains select snippets adapted from [the ones included with PhysX SDK](https://github.com/NVIDIAGameWorks/PhysX/tree/c3d5537bdebd6f5cd82fcaf87474b838fe6fd5fa/physx/snippets).

Generally speaking little has been done to make these C#-like, instead preferring to have them closely match the original snippets to make it easier to compare and contrast between the C++ and C# versions.

## License

Note that since much of this code is basically copied straight from the PhysX SDK, it falls under a different license than the rest of this repository. See the notice at the top of relevant files for details.

## Running the snippets

First make sure you have built and generated bindings as described in the main project readme. Once that's done you can build and start the samples using `dotnet` or from Visual Studio.

Each snippet has a description of what it does at the top of their main file. Most snippets render an output, but a handful do not for one reason or another.

All snippets with a visual output can be navigated by pressing the WASD keys and clicking+dragging the mouse on the window. A handful of the snippets have additional controls, see below.

## Notes for specific snippets

### `SnippetArticulation`

The following preprocessor conditionals can be toggled at the top of the snippet:

| Conditional | Description |
|-------------|-------------|
| `USE_REDUCED_COORDINATE_ARTICULATION` | Disabling this will use [`PxArticulation` instead of `PxArticulationReducedCoordinate`](https://gameworksdocs.nvidia.com/PhysX/4.1/documentation/physxguide/Manual/Articulations.html).
| `CREATE_SCISSOR_LIFT` | Disabling this will activate a differnet simulation involving a ball tied to a rope. Enabling requires `USE_REDUCED_COORDINATE_ARTICULATION`.

### `SnippetBVHStructure`

This snippet does not have a renderer. It demonstrates an optimization technique for actors with thousands of shapes. The overly simplistic renderer used for the snippets doesn't support this and fails with an assert.

You can view this sample using [PVD](https://gameworksdocs.nvidia.com/PhysX/4.1/documentation/physxguide/Manual/VisualDebugger.html), although it is not particularly interesting to look at.

### `SnippetHelloGRB` / `SnippetHelloWorld`

Additional controls:

* <kbd>B</kbd> will create an additional stack of boxes
* <kbd>[Space]</kbd> will shoot a ball

### `SnippetJoint`

Additional controls:

* <kbd>[Space]</kbd> will shoot a ball

### `SnippetSerialization`

You can toggle `gUseBinarySerialization` in the main file to choose between XML and binary serialization.

You can use `USE_FILES` in `Program.cs` to toggle between serializing the objects to memory or files. (This is specific to the Mochi variant of the snippet.)

### `SnippetSplitSim`

This snippet can be configured by toggling `OVERLAP_COLLISION_AND_RENDER_WITH_NO_LAG` and `OVERLAP_COLLISION_AND_RENDER_WITH_ONE_FRAME_LAG` in the main snippet file. See the snippet's description for details.

### `SnippetTriggers`

Additional controls:

* <kbd>F1</kbd> - <kbd>F9</kbd> will select between different variants of the snippet.

### `SnippetVehicleContactMod`

Toggle `BLOCKING_SWEEPS` in the main file to change how wheel scene queries are performed.

### `SnippetVehicleScale`

Toggle the index used to initialized `gLengthScale` between `eLengthScaleInches` and `eLengthScaleCentimeters` to use two different vehicle scales.
