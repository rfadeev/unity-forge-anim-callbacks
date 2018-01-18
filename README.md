[![license](https://img.shields.io/github/license/rfadeev/unity-forge-anim-callbacks.svg)](https://github.com/rfadeev/unity-forge-anim-callbacks/blob/master/LICENSE.md)

# Unity Forge Anim Callbacks
Runtime callbacks for Unity animation clip start and end.

## Motivation
While Unity animation events provide ability to call method from specific frame of animation clip, there is no Unity API for binding such method at runtime from code. So it was decided to implement such way to add callbacks to Unity animation clips. Current version supports only callbacks for animation clip start and animation clip end.

## Usage
Import `UnityForge.AnimCallbacks` namespace to be able to use extensions for callbacks. For animator's animation clip callbacks layer index and clip name are required to add callback:
```csharp
var animator = GetComponent<Animator>();
var layerIndex = 0;
var clipName = "MyClipName";
animator.OnClipStart(layerIndex, clipName, () => Debug.LogFormat("{0} clip started", clipName));
animator.OnClipEnd(layerIndex, clipName, () => Debug.LogFormat("{0} clip ended", clipName));
```

Find more examples [here](https://github.com/rfadeev/unity-forge-anim-callbacks/tree/master/Source/Examples).

## Caveats
Callbacks are implemented via adding Unity animation events to the animation clip and `AnimationEventReceiver` component to the same object `Animator` is attached. Following should be taken into account when using callbacks:
* Since Unity animation event calls all components which have method with the name from animation event, attaching component which have methods named `OnStartEventRaised` or `OnEndEventRaised` on the same object which adds callback at runtime, can have undesired consequences since these component's method will be called.
* Using `AnimationEventReceiver` directly (in editor or from code) can result in not desired calls if callbacks are populated from user code.
* Added animation events exist till the application exit since there is no [AnimationClip API](https://docs.unity3d.com/ScriptReference/AnimationClip.html) to remove animation event (there is no `RemoveEvent` method, only `AddEvent`)
* Negative animator state speed does not trigger start and end animation events hence no callbacks are called.
