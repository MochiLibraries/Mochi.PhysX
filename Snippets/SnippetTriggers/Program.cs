#if RENDER_SNIPPET
using static SnippetTriggersRender;

renderLoop();
#else
using static SnippetTriggers;

initPhysics(false);
for (int i = 0; i < 250; i++)
    stepPhysics(false);
cleanupPhysics(false);
#endif
