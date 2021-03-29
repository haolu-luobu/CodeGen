using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using NSwag;
using NSwag.CodeGeneration.OperationNameGenerators;

namespace code_gen
{
    public class CustomSingleClientFromPathSegmentsOperationNameGenerator : IOperationNameGenerator
    {
        /// <summary>Gets a value indicating whether the generator supports multiple client classes.</summary>
        public bool SupportsMultipleClients { get; } = true;

        /// <summary>Gets the client name for a given operation (may be empty).</summary>
        /// <param name="document">The Swagger document.</param>
        /// <param name="path">The HTTP path.</param>
        /// <param name="httpMethod">The HTTP method.</param>
        /// <param name="operation">The operation.</param>
        /// <returns>The client name.</returns>
        public virtual string GetClientName(
            OpenApiDocument document,
            string path,
            string httpMethod,
            OpenApiOperation operation)
        {
            return string.Empty;
        }

        /// <summary>Gets the client name for a given operation (may be empty).</summary>
        /// <param name="document">The Swagger document.</param>
        /// <param name="path">The HTTP path.</param>
        /// <param name="httpMethod">The HTTP method.</param>
        /// <param name="operation">The operation.</param>
        /// <returns>The client name.</returns>
        public virtual string GetOperationName(
            OpenApiDocument document,
            string path,
            string httpMethod,
            OpenApiOperation operation)
        {
            string operationName = ConvertPathToName(path);
            if (document.Paths.SelectMany(pair => pair.Value.ActualPathItem.Select(p => new
                {
                    Path = pair.Key.Trim(new char[1] {'/'}),
                    HttpMethod = p.Key,
                    Operation = p.Value
                })).Where(op =>
                    this.GetClientName(document, op.Path, op.HttpMethod, op.Operation) ==
                    this.GetClientName(document, path, httpMethod, operation) &&
                    ConvertPathToName(op.Path) == operationName)
                .ToList()
                .Count > 1)
                operationName = CapitalizeFirst(httpMethod)+operationName;
            return operationName;
        }

        /// <summary>Converts the path to an operation name.</summary>
        /// <param name="path">The HTTP path.</param>
        /// <returns>The operation name.</returns>
        public static string ConvertPathToName(string path)
        {
            string str = ((IEnumerable<string>) Regex.Replace(path, "\\{.*?\\}", "").Split('/', '-', '_'))
                .Where<string>((Func<string, bool>) (part => !part.Contains("{") && !string.IsNullOrWhiteSpace(part)))
                .Aggregate<string, string>("",
                    (Func<string, string, string>) ((current, part) =>
                        current + CapitalizeFirst(part)));
            if (string.IsNullOrEmpty(str))
                str = "Index";
            return str.Replace("V1", "");
        }

        /// <summary>Capitalizes first letter.</summary>
        /// <param name="name">The name to capitalize.</param>
        /// <returns>Capitalized name.</returns>
        internal static string CapitalizeFirst(string name)
        {
            if (string.IsNullOrEmpty(name))
                return string.Empty;
            string lowerInvariant = name.ToLowerInvariant();
            return char.ToUpperInvariant(lowerInvariant[0]).ToString() +
                   (lowerInvariant.Length > 1 ? lowerInvariant.Substring(1) : "");
        }
    }
}


