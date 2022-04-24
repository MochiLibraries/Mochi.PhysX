using System;

public static class SnippetUtils
{
    public static uint getNbPhysicalCores()
        //TODO: This is not actually the same getNbPhysicalCores since it includes SMT threads.
        => (uint)Environment.ProcessorCount;
}
