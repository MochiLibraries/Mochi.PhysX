namespace SnippetCommon;

public unsafe static class Globals
{
    //BIOQUIRK: Might be an interesting idea to have the semantics of this enforced by an analyzer.
    // This dummy function is here for now so that static_cast usage in the C++ snippets is reflected in our ports.
    public static T* static_cast<T>(void* ptr)
        where T : unmanaged
        => (T*)ptr;
}
