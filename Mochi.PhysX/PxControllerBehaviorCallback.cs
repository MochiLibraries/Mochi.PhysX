using Mochi.PhysX.Infrastructure;
using System.Runtime.CompilerServices;

namespace Mochi.PhysX;

unsafe partial struct PxControllerBehaviorCallback
{
    // These two overloads end up conflicting when Biohazrd emits them as inheritance-via-generic trampolines
    // As such we manually implement them with one as an extension method so they don't conflict.
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public PxControllerBehaviorFlags getBehaviorFlags<TController>(in TController controller)
        where TController : unmanaged, IPxController
        => getBehaviorFlags(in Unsafe.As<TController, PxController>(ref Unsafe.AsRef(in controller)));
}

public static class PxControllerBehaviorCallbackExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static PxControllerBehaviorFlags getBehaviorFlags<TObstacle>(this ref PxControllerBehaviorCallback callback, in TObstacle obstacle)
        where TObstacle : unmanaged, IPxObstacle
        => callback.getBehaviorFlags(in Unsafe.As<TObstacle, PxObstacle>(ref Unsafe.AsRef(in obstacle)));
}
