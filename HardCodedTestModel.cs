using Microsoft.OData.Edm;

namespace ODLTests
{
    internal class HardCodedTestModel
    {
        internal static readonly IEdmModel _testModel = GetEdmModel();

        public static IEdmModel TestModel
        {
            get { return _testModel; }
        }

        internal static IEdmModel GetEdmModel()
        {
            var model = new EdmModel();

            #region Enum Types
            var colorType = new EdmEnumType("Fully.Qualified.Namespace", "Color", EdmPrimitiveTypeKind.Int32, true);
            colorType.AddMember("Red", new EdmEnumMemberValue(1));
            colorType.AddMember("Blue", new EdmEnumMemberValue(2));
            colorType.AddMember("Yellow", new EdmEnumMemberValue(3));
            model.AddElement(colorType);
            #endregion

            #region Structured Types
            var personType = new EdmEntityType("Fully.Qualified.Namespace", "Person");
            var personId = personType.AddStructuralProperty("ID", EdmPrimitiveTypeKind.Int32, false);
            personType.AddKeys(personId);
            personType.AddStructuralProperty("Name", EdmPrimitiveTypeKind.String);
            personType.AddStructuralProperty("FavoriteColor", new EdmEnumTypeReference(colorType, true));
            model.AddElement(personType);

            var employeeType = new EdmEntityType("Fully.Qualified.Namespace", "Employee", personType);
            employeeType.AddStructuralProperty("WorkEmail", EdmPrimitiveTypeKind.String);
            model.AddElement(employeeType);

            var managerType = new EdmEntityType("Fully.Qualified.Namespace", "Manager", employeeType);
            managerType.AddStructuralProperty("NumberOfReports", EdmPrimitiveTypeKind.Int32);
            model.AddElement(managerType);

            var openEmployeeType = new EdmEntityType("Fully.Qualified.Namespace", "OpenEmployee", employeeType, false, true);
            model.AddElement(openEmployeeType);
            #endregion

            #region Entity Container
            var container = new EdmEntityContainer("Fully.Qualified.Namespace", "Context");
            model.AddElement(container);

            var peopleSet = container.AddEntitySet("People", personType);
            var employeesSet = container.AddEntitySet("Employees", employeeType);
            var managersSet = container.AddEntitySet("Managers", managerType);
            var openEmployeesSet = container.AddEntitySet("OpenEmployees", openEmployeeType);
            #endregion

            return model;
        }

        public static IEdmEntityType GetPersonType()
        {
            return TestModel.FindType("Fully.Qualified.Namespace.Person") as IEdmEntityType;
        }

        public static IEdmEntityTypeReference GetPersonTypeReference()
        {
            return new EdmEntityTypeReference(GetPersonType(), false);
        }

        public static IEdmEntitySet GetPeopleSet()
        {
            return TestModel.FindEntityContainer("Context").FindEntitySet("People");
        }
    }
}
