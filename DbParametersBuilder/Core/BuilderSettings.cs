using System;

namespace DbParametersBuilder.Core {
    public static class BuilderSettings {
        internal static BuilderDataFactory BuilderFactory = new BuilderDataFactory();
        internal static bool SkipNullParameters = false;
        public static void SetBuilderFactory(BuilderDataFactory builderDataFactory) {
            BuilderFactory = builderDataFactory ?? throw new ArgumentNullException(nameof(builderDataFactory));
        }
        public static void SetSkipNullParameters(bool skip) {
            SkipNullParameters = skip;
        }
    }
}
