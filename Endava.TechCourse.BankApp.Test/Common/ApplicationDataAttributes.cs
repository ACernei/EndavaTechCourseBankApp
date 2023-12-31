using AutoFixture.AutoNSubstitute;
using EntityFrameworkCore.AutoFixture.Core;
using EntityFrameworkCore.AutoFixture.Sqlite;

namespace Endava.TechCourse.BankApp.Test.Common;

public class ApplicationDataAttribute : InlineAutoDataAttribute
{
    public ApplicationDataAttribute(params object[] arguments)
        : base(() => new Fixture()
                .Customize(new CompositeCustomization(
                    new AutoNSubstituteCustomization(),
                    new SqliteCustomization
                    {
                        AutoOpenConnection = true,
                        OmitDbSets = true,
                        OnCreate = OnCreateAction.EnsureCreated
                    })),
            arguments)
    {
    }
}
