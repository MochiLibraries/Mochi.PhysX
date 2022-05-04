using Biohazrd;
using Biohazrd.Transformation;
using System.Collections.Immutable;
using System.Linq;

namespace Mochi.PhysX.Generator
{
    /// <summary>Transforms a PhysX-style scoped enum into a modern scoped enum.</summary>
    /// <remarks>
    /// In PhysX, enums are scoped by placing them into an empty struct:
    /// <code>
    /// struct PxExampleEnum
    /// {
    ///     enum Enum
    ///     {
    ///         eEXAMPLE_ONE,
    ///         eEXAMPLE_TWO,
    ///         eEXAMPLE_THREE
    ///     };
    /// };
    /// </code>
    ///
    /// This transform effectively replaces the scoping struct with the enum its self:
    /// <code>
    /// enum class PxExampleEnum
    /// {
    ///     eEXAMPLE_ONE,
    ///     eEXAMPLE_TWO,
    ///     eEXAMPLE_THREE
    /// };
    /// </code>
    /// </remarks>
    public sealed class PhysXEnumTransformation : TransformationBase
    {
        protected override TransformationResult TransformRecord(TransformationContext context, TranslatedRecord declaration)
        {
            // A PhysX enum record is one that has no members except a single EnumDeclaration.
            if (declaration.Members.Count == 1 && declaration.TotalMemberCount == 1 && declaration.Members[0] is TranslatedEnum enumDeclaration)
            {
                return enumDeclaration with
                {
                    Name = declaration.Name,
                    TranslateAsLooseConstants = false
                };
            }

            // A handful of types (such as PxVehicleDifferential4WData) are more complicated and still have a nested name-irrelevant enum
            // Handle those types by creating aliases to their enum constants
            int i = -1;
            foreach (TranslatedDeclaration member in declaration.Members)
            {
                i++;

                if (member is TranslatedEnum { Name: "Enum", TranslateAsLooseConstants: false } nestedEnum)
                {
                    ImmutableList<TranslatedDeclaration>.Builder membersBuilder = declaration.Members.ToBuilder();
                    membersBuilder.InsertRange(i + 1, nestedEnum.Values.Select(v => new EnumAliasDeclaration(v)));

                    return declaration with
                    {
                        Members = membersBuilder.ToImmutable()
                    };
                }
            }

            return declaration;
        }
    }
}
