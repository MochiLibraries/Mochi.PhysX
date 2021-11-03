﻿using Biohazrd;
using Biohazrd.Transformation;
using ClangSharp;
using ClangSharp.Interop;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using ClangType = ClangSharp.Type;

namespace InfectedPhysX.Generator
{
    //TODO: Add sanity checks that the transformations called for during initialization actually happened.
    public sealed class PhysXFlagsEnumTransformation : TransformationBase
    {
        // This transformation almost supports concurrency, but the fact that we remove elements from the sets as they are processed means that it is not
        // It should be relatively easy to adapt this type to support concurrency, it's just a matter of doing it.
        protected override bool SupportsConcurrency => false;

        private readonly HashSet<TranslatedTypedef> FlagsTypedefs = new(ReferenceEqualityComparer.Instance);
        private readonly Dictionary<EnumDecl, (TranslatedTypedef FlagsTypedef, ClangType UnderlyingType)> FlagsEnums = new();
        private readonly HashSet<ClangType> FlagsCanonicalTypes = new();

        //TODO: This transformation has a heavy reliance on Clang stuff. Ideally it should be able to work with what Biohazrd provides alone.
        protected override TranslatedLibrary PreTransformLibrary(TranslatedLibrary library)
        {
            // These should generally be empty
            Debug.Assert(FlagsTypedefs.Count == 0, "The state of this transformaiton should be empty at this point.");
            Debug.Assert(FlagsEnums.Count == 0, "The state of this transformaiton should be empty at this point.");
            Debug.Assert(FlagsCanonicalTypes.Count == 0, "The state of this transformaiton should be empty at this point.");
            FlagsTypedefs.Clear();
            FlagsEnums.Clear();
            FlagsCanonicalTypes.Clear();

            // Run an initial pass through the library to identify PxFlags enums
            foreach (TranslatedDeclaration declaration in library.EnumerateRecursively())
            {
                // Look for typedefs
                if (declaration is not TranslatedTypedef typedef)
                { continue; }

                // Look for typedefs that are template specializations
                if (typedef.UnderlyingType is not ClangTypeReference clangType || clangType.ClangType is not TemplateSpecializationType templateSpecialization)
                { continue; }

                // Get the declaration
                if (library.FindClangCursor(templateSpecialization.Handle.Declaration) is not ClassTemplateSpecializationDecl templateSpecializationDeclaration)
                {
                    Debug.Fail("The declaration for a TemplateSpecializationType is expected to be a ClassTemplateSpecializationDecl.");
                    continue;
                }

                // Look for PxFlags
                if (templateSpecializationDeclaration.Name != "PxFlags")
                { continue; }

                // We expect there to be two template arguments: PxFlags<enumtype, storagetype>
                if (templateSpecializationDeclaration.TemplateArgs.Count != 2)
                {
                    Debug.Fail("PxFlags should always have two template arguments.");
                    continue;
                }

                // Extract the arguments
                TemplateArgument enumArgument = templateSpecializationDeclaration.TemplateArgs[0];
                TemplateArgument storageType = templateSpecializationDeclaration.TemplateArgs[1];

                if (enumArgument.Kind != CXTemplateArgumentKind.CXTemplateArgumentKind_Type)
                {
                    Debug.Fail("The first template argument to PxFlags should be a type argument");
                    continue;
                }

                if (storageType.Kind != CXTemplateArgumentKind.CXTemplateArgumentKind_Type)
                {
                    Debug.Fail("The second template argument to PxFlags should be a type argument");
                    continue;
                }

                // The first argument should be an EnumType with a corresponding EnumDecl
                if (enumArgument.AsType is not EnumType { Decl: EnumDecl enumDecl })
                { continue; }

                // Record the relevant info needed to perform the transformation
                FlagsTypedefs.Add(typedef);
                FlagsEnums.Add(enumDecl, (typedef, storageType.AsType));
                FlagsCanonicalTypes.Add(templateSpecialization.CanonicalType); //TODO: Is this the same for all PxFlags?
            }

            return library;
        }

        protected override TranslatedLibrary PostTransformLibrary(TranslatedLibrary library)
        {
            FlagsTypedefs.Clear();
            FlagsEnums.Clear();
            FlagsCanonicalTypes.Clear();

            return library;
        }

        protected override TransformationResult TransformTypedef(TransformationContext context, TranslatedTypedef declaration)
        {
            // Delete the targeted typedefs
            if (FlagsTypedefs.Remove(declaration))
            { return null; }

            return declaration;
        }

        protected override TransformationResult TransformEnum(TransformationContext context, TranslatedEnum declaration)
        {
            // Update targeted enums
            if (declaration.Declaration is EnumDecl enumDecl && FlagsEnums.Remove(enumDecl, out (TranslatedTypedef FlagsTypedef, ClangType UnderlyingType) enumInfo))
            {
                return declaration with
                {
                    Name = enumInfo.FlagsTypedef.Name,
                    IsFlags = true,
                    UnderlyingType = new ClangTypeReference(enumInfo.UnderlyingType),
                    ReplacedDeclarations = ImmutableArray.Create<TranslatedDeclaration>(enumInfo.FlagsTypedef)
                };
            }

            return declaration;
        }

        protected override TransformationResult TransformFunction(TransformationContext context, TranslatedFunction declaration)
        {
            // Remove the PX_FLAGS_OPERATORS, which we define as static operator overloads that return one of the enumerated PxFlags<,> types.
            if (declaration is { SpecialFunctionKind: SpecialFunctionKind.OperatorOverload, IsInstanceMethod: false, Declaration: FunctionDecl functionDecl } && FlagsCanonicalTypes.Contains(functionDecl.ReturnType.CanonicalType))
            { return null; }

            return declaration;
        }
    }
}
