using Mochi.PhysX;
using Mochi.PhysX.Infrastructure;

namespace SnippetCommon;

// These are really only here for ease of porting the snippets and making them better match their C++ counterparts.
// We could auto-generate them, but I don't think this pattern necessarily makes sense for C#
unsafe partial class Globals
{
    public static void PX_RELEASE<TBase>(ref TBase* x)
        where TBase : unmanaged, IPxBase
    {
        if (x != null)
        {
            ((PxBase*)x)->release();
            x = null;
        }
    }

    public static void PX_RELEASE(ref PxBatchQuery* x)
    {
        if (x != null)
        {
            x->release();
            x = null;
        }
    }

    public static void PX_RELEASE(ref PxCooking* x)
    {
        if (x != null)
        {
            x->release();
            x = null;
        }
    }

    public static void PX_RELEASE(ref PxDefaultCpuDispatcher* x)
    {
        if (x != null)
        {
            x->release();
            x = null;
        }
    }

    public static void PX_RELEASE(ref PxFoundation* x)
    {
        if (x != null)
        {
            x->release();
            x = null;
        }
    }

    public static void PX_RELEASE(ref PxPhysics* x)
    {
        if (x != null)
        {
            x->release();
            x = null;
        }
    }

    public static void PX_RELEASE(ref PxPvd* x)
    {
        if (x != null)
        {
            x->release();
            x = null;
        }
    }

    public static void PX_RELEASE(ref PxPvdTransport* x)
    {
        if (x != null)
        {
            x->release();
            x = null;
        }
    }

    public static void PX_RELEASE(ref PxScene* x)
    {
        if (x != null)
        {
            x->release();
            x = null;
        }
    }

    public static void PX_RELEASE(ref PxVehicleDrivableSurfaceToTireFrictionPairs* x)
    {
        if (x != null)
        {
            x->release();
            x = null;
        }
    }
}
