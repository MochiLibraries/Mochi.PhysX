using System.Diagnostics;

namespace Mochi.PhysX;

// This type is deprecated in PhysX and is unlikely to change. For the sake of simplicity we just reimplemented it in C# as a generic instead of realizing all of its templates.
// It's really only exposed for the sake of using it with the vehicles module. Batch queries should not be used otherwise as they're only more efficient on the PlayStation 3
// https://github.com/NVIDIAGameWorks/PhysX/issues/89#issuecomment-476512903

/// <summary>Generic struct for receiving results of single query in a batch. Gets templated on hit type PxRaycastHit, PxSweepHit or PxOverlapHit.</summary>
/// <remarks>The batched query feature has been deprecated in PhysX version 3.4</remarks>
public unsafe struct PxBatchQueryResult<THitType>
    where THitType : unmanaged
{
    /// <summary>Holds the closest blocking hit for a single query in a batch. Only valid if <see cref="hasBlock"/> is true.</summary>
    public THitType block;

    /// <summary>
    /// This pointer will either be set to <c>null</c> for 0 <see cref="nbTouches"/> or will point
    /// into the user provided batch query results buffer specified in <see cref="PxBatchQueryDesc"/>.
    /// </summary>
    public THitType* touches;

    /// <summary>Number of touching hits returned by this query, works in tandem with touches pointer.</summary>
    public uint nbTouches;

    /// <summary>Copy of the userData pointer specified in the corresponding query.</summary>
    public void* userData;

    /// <summary>Takes on values from <see cref="PxBatchQueryStatus"/>.</summary>
    public byte queryStatus;

    /// <summary>True if there was a blocking hit.</summary>
    public bool hasBlock;

    /// <summary>pads the struct to 16 bytes.</summary>
    public ushort pad;

    /// <summary>Computes the number of any hits in this result, blocking or touching.</summary>
    public readonly uint getNbAnyHits()
        => nbTouches + (hasBlock? 1u : 0u);

    /// <summary>Convenience iterator used to access any hits in this result, blocking or touching.</summary>
    public readonly THitType getAnyHit(uint index)
    {
        Debug.Assert(index < nbTouches + (hasBlock ? 1u : 0u));
        return index < nbTouches ? touches[index] : block;
    }
}
