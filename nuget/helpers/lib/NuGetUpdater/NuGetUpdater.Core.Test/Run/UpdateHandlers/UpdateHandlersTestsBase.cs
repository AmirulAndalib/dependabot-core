using NuGetUpdater.Core.Run.ApiModel;
using NuGetUpdater.Core.Run.UpdateHandlers;
using NuGetUpdater.Core.Run;
using Xunit;

namespace NuGetUpdater.Core.Test.Run.UpdateHandlers;

public class UpdateHandlersTestsBase : TestBase
{
    protected static JobSource CreateJobSource(string directory, params string[] additionalDirectories) => new JobSource()
    {
        Provider = "github",
        Repo = "test/repo",
        Directories = [directory, .. additionalDirectories],
    };

    protected Task TestAsync(
        Job job,
        IUpdateHandler expectedUpdateHandler,
        object[] expectedApiMessages,
        (string Name, string Contents)[] files,
        MockNuGetPackage[]? packages = null,
        IDiscoveryWorker? discoveryWorker = null,
        IAnalyzeWorker? analyzeWorker = null,
        IUpdaterWorker? updaterWorker = null,
        ExperimentsManager? experimentsManager = null
    )
    {
        // first ensure we're using the correct updater
        var actualUpdateHandler = RunWorker.GetUpdateHandler(job);
        Assert.Equal(actualUpdateHandler.GetType(), expectedUpdateHandler.GetType());

        experimentsManager ??= new ExperimentsManager();
        return EndToEndTests.RunAsync(job, files, discoveryWorker, analyzeWorker, updaterWorker, expectedApiMessages, packages, experimentsManager);
    }
}
