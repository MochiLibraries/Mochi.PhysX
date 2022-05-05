using System.Runtime.CompilerServices;

namespace SnippetCommon;

public unsafe static partial class Globals
{
    //BIOQUIRK: Might be an interesting idea to have the semantics of this enforced by an analyzer.
    // This dummy function is here for now so that static_cast usage in the C++ snippets is reflected in our ports.
    public static T* static_cast<T>(void* ptr)
        where T : unmanaged
        => (T*)ptr;

    //BIOQUIRK: Similar idea but for readonly byrefs.
    public static ref readonly TTo static_cast<TFrom, TTo>(in TFrom t)
        where TFrom : unmanaged
        where TTo : unmanaged
        => ref Unsafe.As<TFrom, TTo>(ref Unsafe.AsRef(t));
}
