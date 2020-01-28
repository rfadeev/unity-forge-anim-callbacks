[![license](https://img.shields.io/github/license/rfadeev/unity-forge-anim-callbacks.svg)](https://github.com/rfadeev/unity-forge-anim-callbacks/blob/master/LICENSE.md)
[![openupm](https://img.shields.io/npm/v/com.rfadeev.unityforge.animcallbacks?label=openupm&registry_uri=https://package.openupm.com)](https://openupm.com/packages/com.rfadeev.unityforge.animcallbacks/)

# Unity Forge Anim Callbacks
Runtime callbacks for Unity animation clips used in Animator and Animation components.

## Motivation
While Unity animation events provide ability to call method from specific time point of animation clip, there is no Unity API for binding such method at runtime from code. So it was decided to implement such way to add callbacks to Unity animation clips.

## Installation

### Install via OpenUPM

The package is available on the [openupm registry](https://openupm.com). It's recommended to install it via [openupm-cli](https://github.com/openupm/openupm-cli).

```
openupm add com.rfadeev.unityforge.animcallbacks
```

### Install via Git

Project supports Unity Package Manager. To install project as Git package do following:
1. Close Unity project and open the `Packages/manifest.json` file.
2. Update `dependencies` to have `com.rfadeev.unityforge.animcallbacks` package:
```json
{
  "dependencies": {
    "com.rfadeev.unityforge.animcallbacks": "https://github.com/rfadeev/unity-forge-anim-callbacks.git#upm"
  }
}
```
3. Open Unity project.

Alternatively, using upm branch add this repository as submodule under `Assets` folder or download it and put to `Assets` folder of your Unity project. 

## Usage
Import `UnityForge.AnimCallbacks` namespace to be able to use extensions for callbacks. Both Animator and Animation extension methods have same names:
* `AddClipStartCallback` - to add callback for start of animation clip.
* `AddClipEndCallback` - to add callback for end of animation clip.
* `AddClipCallback` - to add callback for given timeline position of animation clip.
* `RemoveClipStartCallback` - to remove callback for start of animation clip.
* `RemoveClipEndCallback` - to remove callback for end of animation clip.
* `RemoveClipCallback` - to remove callback for given timeline position of animation clip.

Several callbacks can be added at the same position of animation clip timeline. Callbacks are called in order they were added.
Note that if using anonymous functions, you need to store delegate instance for correct removal of callback via `RemoveClip*` methods.

### Animator
For Animator's animation clip callbacks layer index and clip name are required to add callback. To add callback at given timeline position, position parameter representing time in seconds from clip start is required.
```csharp
var animator = GetComponent<Animator>();
var layerIndex = 0;
var clipName = "AnimatorClipName";

animator.AddClipStartCallback(layerIndex, clipName, () =>
{
    Debug.LogFormat("Clip \"{0}\": started", clipName);
});
animator.AddClipEndCallback(layerIndex, clipName, () =>
{
    Debug.LogFormat("Clip \"{0}\": ended", clipName);
});
animator.AddClipCallback(layerIndex, clipName, 0.5f, () =>
{
    Debug.LogFormat("Clip \"{0}\": callback at 0.5f seconds after start", clipName);
});
```

To remove callback same parameters are required as in complementary add callback method.
```csharp
private void AddExampleCallback()
{
    animator.AddClipStartCallback(layerIndex, clipName, LogStart);
}

private void RemoveExampleCallback()
{
    animator.RemoveClipStartCallback(layerIndex, clipName, LogStart);
}

private void LogStart()
{
    Debug.LogFormat("Clip \"{0}\": started", clipName);
}
```

Find more Animator examples [here](https://github.com/rfadeev/unity-forge-anim-callbacks/tree/master/Packages/com.rfadeev.unityforge.animcallbacks/Samples/Animator).

### Animation
For Animation's animation clip callbacks clip name is required to add callback. To add callback at given timeline position, position parameter representing time in seconds from clip start is required.
```csharp
var animation = GetComponent<Animation>();
var clipName = "AnimationClipName";

animation.AddClipStartCallback(clipName, () =>
{
    Debug.LogFormat("Clip \"{0}\": started", clipName);
});
animation.AddClipEndCallback(clipName, () =>
{
    Debug.LogFormat("Clip \"{0}\": ended", clipName);
});
animation.AddClipCallback(clipName, 0.5f, () =>
{
    Debug.LogFormat("Clip \"{0}\": callback at 0.5f seconds after start", clipName);
});
```

To remove callback same parameters are required as in complementary add callback method.
```csharp
private void AddExampleCallback()
{
    animation.AddClipStartCallback(clipName, LogStart);
}

private void RemoveExampleCallback()
{
    animation.RemoveClipStartCallback(clipName, LogStart);
}

private void LogStart()
{
    Debug.LogFormat("Clip \"{0}\": started", clipName);
}
```

Find more Animation examples [here](https://github.com/rfadeev/unity-forge-anim-callbacks/tree/master/Packages/com.rfadeev.unityforge.animcallbacks/Samples/Animation).

## Caveats
Callbacks are implemented via adding Unity animation events to the animation clip and `AnimationEventReceiver` component to the same object Animator or Animation is attached. Following should be taken into account when using callbacks:
* Since Unity animation event calls all components which have method with the name from animation event, attaching component which has method named `OnTimelineEventRaised` on the same object which adds callback at runtime, can have undesired consequences since these component's method will be called.
* Using `AnimationEventReceiver` directly (in editor or from user code) can result in not desired calls if callbacks are populated from user code or if `OnTimelineEventRaised` method is used in animation event added directly (in editor or from user code).
* Runtime representation of `AnimationClip` is persistent while application is running. So if callback is added, but not removed, coressponding animation event will be present in animation clip even if Animation or Animator component is destroyed.
* Negative animator state speed does not trigger start and end animation events hence no callbacks are called.
