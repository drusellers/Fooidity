﻿namespace Fooidity
{
    using System;
    using System.Runtime.Serialization;
    using System.Text;


    /// <summary>
    /// A code feature id is generated based on the type and is used to identify a code feature
    /// outside of the compiled code base (such as in a configuration file, database, etc).
    /// </summary>
    [Serializable]
    public class CodeFeatureId :
        TypeUrn
    {
        public CodeFeatureId(Type type)
            : base(GetCodeFeatureId(type))
        {
        }

        public CodeFeatureId(string uriString)
            : base(uriString)
        {
            if (!Scheme.Equals("urn", StringComparison.OrdinalIgnoreCase))
                throw new FormatException("A CodeFeatureId must be a URN");
        }

        protected CodeFeatureId(SerializationInfo serializationInfo, StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        {
        }

        public CodeFeatureId(Uri uri)
            : base(uri.ToString())
        {
            if (!Scheme.Equals("urn", StringComparison.OrdinalIgnoreCase))
                throw new FormatException("A CodeFeatureId must be a URN");
        }

        static string GetCodeFeatureId(Type type)
        {
            var sb = new StringBuilder("urn:feature:");

            return FormatId(sb, type, true);
        }
    }
}