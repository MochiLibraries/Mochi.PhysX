#if RENDER_SNIPPET
using static SnippetHelloWorldRender;

renderLoop();
#else
using static SnippetHelloWorld;

const int frameCount = 100;
initPhysics(false);
for (int i = 0; i < frameCount; i++)
    stepPhysics(false);
cleanupPhysics(false);
#endif
