using Microsoft.OData.Edm;
using Microsoft.OData.UriParser;
using ODLTests;

var castFilterQueries = new[]
{
    "cast(FavoriteColor, edm.string) eq 'blue'",
    "cast(FavoriteColor, 'edm.string') eq 'blue'",
    "cast(FavoriteColor, Edm.String) eq 'blue'",
    "cast(FavoriteColor, 'Edm.String') eq 'blue'",
};

foreach (var filterQuery in castFilterQueries)
{
    CastFunctionWorksWithNoSingleQuotesOnType(filterQuery);
}

static void CastFunctionWorksWithNoSingleQuotesOnType(string filterQuery)
{
    try
    {
        FilterClause filter = ParseFilter(filterQuery, HardCodedTestModel.TestModel, HardCodedTestModel.GetPersonType(), HardCodedTestModel.GetPeopleSet());
    }
    catch(Exception e)
    {
        Console.WriteLine(filterQuery);
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(e.Message);
        Console.ResetColor();
    }
}

static FilterClause ParseFilter(string text, IEdmModel edmModel, IEdmType edmType, IEdmNavigationSource edmEntitySet = null)
{
    return new ODataQueryOptionParser(edmModel, edmType, edmEntitySet, new Dictionary<string, string>() { { "$filter", text } }) { Resolver = new ODataUriResolver() { EnableCaseInsensitive = false } }.ParseFilter();
}