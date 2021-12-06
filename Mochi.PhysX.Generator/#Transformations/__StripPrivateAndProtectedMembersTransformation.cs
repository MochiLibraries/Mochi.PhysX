using Biohazrd;
using Biohazrd.Transformation;

namespace Mochi.PhysX.Generator
{
    internal sealed class __StripPrivateAndProtectedMembersTransformation : TransformationBase
    {
        protected override TransformationResult TransformFunction(TransformationContext context, TranslatedFunction declaration)
        {
            // Private/protected functions are always stripped
            if (declaration.Accessibility is AccessModifier.Private or AccessModifier.Protected)
            { return null; }

            // Check if any parents are private/protected
            foreach (TranslatedDeclaration parent in context.Parents)
            {
                if (parent.Accessibility is AccessModifier.Private or AccessModifier.Protected)
                { return null; }
            }

            return base.TransformFunction(context, declaration);
        }

        protected override TransformationResult TransformRecord(TransformationContext context, TranslatedRecord declaration)
        {
            // Private/protected records are always stripped
            if (declaration.Accessibility is AccessModifier.Private or AccessModifier.Protected)
            { return null; }

            return base.TransformRecord(context, declaration);
        }

        protected override TransformationResult TransformUndefinedRecord(TransformationContext context, TranslatedUndefinedRecord declaration)
        {
            // Private/protected records are always stripped
            if (declaration.Accessibility is AccessModifier.Private or AccessModifier.Protected)
            { return null; }

            return base.TransformUndefinedRecord(context, declaration);
        }
    }
}
