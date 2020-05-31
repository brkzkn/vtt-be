using VacationTracking.Data;
using Xunit;

namespace VacationTracking.Test.Context
{
    [CollectionDefinition(nameof(VacationTrackingContext))]
    public class VacationTrackingDbContextCollection : ICollectionFixture<VacationTrackingDbContextFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}
