[![license](https://img.shields.io/github/license/rfadeev/unity-forge-anim-callbacks.svg)](https://github.com/rfadeev/unity-forge-anim-callbacks/blob/master/LICENSE.md)

# Unity Forge Anim Callbacks
Runtime callbacks for Unity animation clips used in Animator and Animation components.

## Motivation
While Unity animation events provide ability to call method from specific time point of animation clip, there is no Unity API for binding such method at runtime from code. So it was decided to implement such way to add callbacks to Unity animation clips.

## Usage
Import `UnityForge.AnimCallbacks` namespace to be able to use extensions for callbacks. Both Animator and Animation extension methods have same names:
* `AddClipStartCallback` - to add callback for start of animation clip.
* `AddClipEndCallback` - to add callback for end of animation clip.
* `AddClipCallback` - to add callback for given timeline position of animation clip.

Several callbacks can be added at the same position of animation clip timeline. Callbacks are called in order they were added.

### Animator
For Animator's animation clip callbacks layer index and clip name are required to add callback. To add callback at given timeline position, position parameter representing time in seconds from clip start, is required.
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

### Animation
For Animation's animation clip callbacks clip name is required to add callback. To add callback at given timeline position, position parameter representing time in seconds from clip start, is required.
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

Find more examples [here](https://github.com/rfadeev/unity-forge-anim-callbacks/tree/master/Source/Examples).

## Caveats
Callbacks are implemented via adding Unity animation events to the animation clip and `AnimationEventReceiver` component to the same object Animator or Animation is attached. Following should be taken into account when using callbacks:
* Since Unity animation event calls all components which have method with the name from animation event, attaching component which has method named `OnTimelineEventRaised` on the same object which adds callback at runtime, can have undesired consequences since these component's method will be called.
* Using `AnimationEventReceiver` directly (in editor or from user code) can result in not desired calls if callbacks are populated from user code or if `OnTimelineEventRaised` method is used in animation event added directly (in editor or from user code).
* Added animation events exist till the application exit since there is no [AnimationClip API](https://docs.unity3d.com/ScriptReference/AnimationClip.html) to remove animation event (there is no `RemoveEvent` method, only `AddEvent`)
* Negative animator state speed does not trigger start and end animation events hence no callbacks are called.
