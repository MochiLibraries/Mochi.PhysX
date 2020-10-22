using System.IO;

namespace InfectedPhysX.Sample
{
    internal unsafe static class TextWriterEx
    {
        public static void WriteAnsi(this TextWriter writer, byte* stringPointer)
        {
            if (stringPointer == null)
            {
                writer.Write("<null>");
                return;
            }

            while (*stringPointer != 0)
            {
                writer.Write((char)*stringPointer);
                stringPointer++;
            }
        }
    }
}
