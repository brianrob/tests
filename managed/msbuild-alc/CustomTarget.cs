using Microsoft.Build.Framework;
using Microsoft.Build.Shared;
using Microsoft.Build.Utilities;
using System.Runtime.Loader;

namespace msbuild_alc
{
    public class CustomTarget : Task
    {
        public override bool Execute()
        {
            var thisLoadContext = AssemblyLoadContext.GetLoadContext(typeof(CustomTarget).Assembly);

            Log.LogWarning($"AssemblyLoadContext name: \"{thisLoadContext.GetType().Name}\" for current executing assembly {typeof(CustomTarget).Assembly.GetName().Name}");

            return !Log.HasLoggedErrors;
        }
    }
}