using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Mochi.PhysX;

// PhysX defines PxFixedSizeLookupTable<T> as a template for use with various sizes, but in practice only a size of 8 is ever used
// For the sake of simplicity, this template is manually ported to C# instead of using Biohazrd's template specialization infrastructure.
public unsafe struct PxFixedSizeLookupTable8
{
    private const uint NB_ELEMENTS = 8;

    // PxFixedSizeLookupTable8() omitted as it is basically default C# behavior

    public PxFixedSizeLookupTable8(PxEMPTY empty)
        => Unsafe.SkipInit(out this);

    public PxFixedSizeLookupTable8(ReadOnlySpan<float> dataPairs)
    {
        Debug.Assert(dataPairs.Length < 2 * NB_ELEMENTS);
        Debug.Assert(dataPairs.Length % 2 == 0);
        fixed (float* mDataPairsP = mDataPairs)
        { dataPairs.CopyTo(new Span<float>(mDataPairsP, 2 * (int)NB_ELEMENTS)); }
        mNbDataPairs = (uint)dataPairs.Length / 2;
    }

    public PxFixedSizeLookupTable8(float* dataPairs, uint numDataPairs)
        : this(new ReadOnlySpan<float>(dataPairs, (int)numDataPairs * 2))
    { }

    public PxFixedSizeLookupTable8(in PxFixedSizeLookupTable8 src)
    {
        fixed (float* srcP = src.mDataPairs)
        fixed (float* thisP = this.mDataPairs)
        {
            ReadOnlySpan<float> srcSpan = new(srcP, (int)src.mNbDataPairs * 2);
            Span<float> thisSpan = new(thisP, (int)src.mNbDataPairs * 2);
            srcSpan.CopyTo(thisSpan);
        }

        mNbDataPairs=src.mNbDataPairs;
    }

    // ~PxFixedSizeLookupTable() omitted since it is empty and doesn't make sense for C#.

    // PxFixedSizeLookupTable& operator=(const PxFixedSizeLookupTable& src) omitted as it is default C# behavior (and doesn't make sense for C# anyway)

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void addPair(float x, float y)
    {
        Debug.Assert(mNbDataPairs < NB_ELEMENTS);
        mDataPairs[2*mNbDataPairs+0]=x;
        mDataPairs[2*mNbDataPairs+1]=y;
        mNbDataPairs++;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly float getYVal(float x)
    {
        if(0==mNbDataPairs)
        {
            Debug.Assert(false);
            return 0;
        }

        if(1==mNbDataPairs || x<getX(0))
        {
            return getY(0);
        }

        float x0=getX(0);
        float y0=getY(0);

        for(uint i=1;i<mNbDataPairs;i++)
        {
            float x1=getX(i);
            float y1=getY(i);

            if((x>=x0)&&(x<x1))
            {
                return (y0+(y1-y0)*(x-x0)/(x1-x0));
            }

            x0=x1;
            y0=y1;
        }

        Debug.Assert(x>=getX(mNbDataPairs-1));
        return getY(mNbDataPairs-1);
    }

    public readonly uint getNbDataPairs() => mNbDataPairs;

    public void clear()
    {
        fixed (float* mDataPairsP = mDataPairs)
        { new Span<float>(mDataPairsP, (int)NB_ELEMENTS * 2 * sizeof(float)).Clear(); }
        mNbDataPairs = 0;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly float getX(uint i)
    {
        return mDataPairs[2*i];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly float getY(uint i)
    {
        return mDataPairs[2*i+1];
    }

    public fixed float mDataPairs[(int)(2*NB_ELEMENTS)];
    public uint mNbDataPairs;
    public fixed uint mPad[3];
}
