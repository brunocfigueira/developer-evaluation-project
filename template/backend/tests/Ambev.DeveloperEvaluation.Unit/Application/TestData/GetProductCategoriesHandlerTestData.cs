using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData;

public static class GetProductCategoriesHandlerTestData
{
    private static readonly Faker faker = new();

    public static List<string> GenerateCategories(int count = 3)
    {
        return Enumerable.Range(0, count)
            .Select(_ => faker.Commerce.Categories(1)[0])
            .Distinct()
            .ToList();
    }
}
