using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MSReport.Core
{
    public static class Utility
    {
        private static string GetFullEmbeddedResourceName(string embeddedResourceName, Type anyTypeDefineInResourceAssembly)
        {
            embeddedResourceName = embeddedResourceName?.Trim();
            var allResourceNames = Assembly.GetAssembly(anyTypeDefineInResourceAssembly)?.GetManifestResourceNames().ToList();
            var targetNames = allResourceNames?.Where(o => o.Equals(embeddedResourceName)).ToList();
            if (targetNames == null || targetNames.Count == 0)
            {
                targetNames = allResourceNames?.Where(o => o.EndsWith("." + embeddedResourceName)).ToList();
            }

            if (targetNames == null || targetNames.Count == 0)
            {
                return null;
            }

            if (targetNames.Count > 1)
            {
                throw new ArgumentException(string.Format("Multiple resource found with name of '{0}',(naming example: 'FolderName.fileName.png')", embeddedResourceName));
            }

            return targetNames.FirstOrDefault();
        }

        public static byte[] ToBytes(Stream stream)
        {
            if (stream != null)
            {
                using (stream)
                {
                    if (stream.CanSeek)
                    {
                        stream.Seek(0, SeekOrigin.Begin);
                    }

                    using (var reader = new BinaryReader(stream))
                    {
                        return reader.ReadBytes((int)stream.Length);
                    }
                }
            }
            else
            {
                return null;
            }
        }

        public static byte[] ToBytes(string embeddedResourceName, Type anyTypeDefineInResourceAssembly)
        {
            var stream = ToStream(embeddedResourceName, anyTypeDefineInResourceAssembly);
            return ToBytes(stream);
        }

        public static Stream ToStream(string embeddedResourceName, Type anyTypeDefineInResourceAssembly)
        {
            var fullEmbeddedResourceName = GetFullEmbeddedResourceName(embeddedResourceName, anyTypeDefineInResourceAssembly);
            return Assembly.GetAssembly(anyTypeDefineInResourceAssembly).GetManifestResourceStream(fullEmbeddedResourceName);
        }

        public static Stream ToStream(string embeddedResourceName, Type anyTypeDefineInResourceAssembly, out string fullEmbeddedResourceName)
        {
            fullEmbeddedResourceName = GetFullEmbeddedResourceName(embeddedResourceName, anyTypeDefineInResourceAssembly);
            return ToStream(embeddedResourceName, anyTypeDefineInResourceAssembly);
        }
    }
}
