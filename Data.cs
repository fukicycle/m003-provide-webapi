using m003_provide_api.Controllers;

namespace m003_provide_api
{
    public sealed class Data
    {
        public readonly SupplierDTO[] Suppliers = new SupplierDTO[] {
            new SupplierDTO(1, "Kaneko central center"),
            new SupplierDTO(2, "Kimbara hyper center"),
            new SupplierDTO(3, "Oyaizu super center"),
            new SupplierDTO(4, "Yamamoto capital supplier") };

        public readonly IngredientDTO[] Ingredients =
        {
            new IngredientDTO(1, "flour"),
            new IngredientDTO(2,"solt"),
            new IngredientDTO(3, "sugar"),
            new IngredientDTO(4, "butter"),
            new IngredientDTO(5, "milk"),
            new IngredientDTO(6, "egg"),
            new IngredientDTO(7, "strawberry"),
            new IngredientDTO(8, "chocolate"),
            new IngredientDTO(9, "fresh cream"),
            new IngredientDTO(10, "cocoa"),
            new IngredientDTO(11, "baking soda"),
            new IngredientDTO(12, "lemon"),
            new IngredientDTO(13, "gelatin"),
            new IngredientDTO(14, "baking powder"),
            new IngredientDTO(15, "water")
        };

        public readonly List<SupplierIngredientDTO> SupplierIngredients = new List<SupplierIngredientDTO>();

        public Data()
        {
            SupplierIngredients.Clear();
            foreach (SupplierDTO supplier in Suppliers)
            {
                foreach (IngredientDTO ingredientDTO in Ingredients)
                {
                    int v = Random.Shared.Next(3, 10);
                    decimal price = v * 0.001m;
                    int lt = 10 - v;
                    SupplierIngredients.Add(new SupplierIngredientDTO(ingredientDTO.Id, price, supplier.Id, supplier.Name, lt));
                }
            }
        }
    }
}
