#if RENDER_SNIPPET
using static SnippetArticulationRender;

renderLoop();
#else
using static SnippetArticulation;

const uint frameCount = 100;
initPhysics(false);
for (uint i = 0; i < frameCount; i++)
    stepPhysics(false);
cleanupPhysics(false);
#endif

return 0;
