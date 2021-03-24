using Bogus;
using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace netFramework.Repository
{
    internal class DbContextInitializer : CreateDatabaseIfNotExists<Context>
    {
        private readonly static int _CategoryAmmount = 32;
        private readonly static int _DocumentTypeAmmount = 4;
        private readonly static int _ProductAmmount = 1024;
        private readonly static int _FactureAmmount = 32768;
        private readonly static int _FactureDetailAmmount = 131072;

        protected override void Seed(Context context)
        {
            Randomizer.Seed = new Random(8675309);

            // Poblado de Category
            SeedWithCategory(context);

            // Poblado de DocumentType
            SeedWithDocumentType(context);

            // Poblado de Product
            SeedWithProduct(context);

            // Poblado de Facture
            SeedWithFacture(context);

            // Poblado de Facture
            SeedWithFactureDetail(context);
        }

        private void SeedWithCategory(Context context)
        {
            // Configuracion de Id
            int id = 1;

            // Generador 
            var faker = new Faker<Category>()
                .RuleFor(t => t.Id, _ => id++)
                .RuleFor(t => t.Name, f => f.Lorem.Sentence(25));

            // Agregado
            context.Categories.AddRange(faker.Generate(_CategoryAmmount));
        }

        private void SeedWithDocumentType(Context context)
        {
            // Configuracion de Id
            int id = 1;

            // Generador 
            var faker = new Faker<DocumentType>()
                .RuleFor(t => t.Id, _ => id++)
                .RuleFor(t => t.Name, f => f.Lorem.Sentence(25));

            // Agregado
            context.DocumentTypes.AddRange(faker.Generate(_DocumentTypeAmmount));
        }

        private void SeedWithProduct(Context context)
        {
            int id = 1;

            var faker = new Faker<Product>()
                .RuleFor(t => t.Id, _ => id++)
                .RuleFor(t => t.Name, f => f.Lorem.Sentence(25))
                .RuleFor(t => t.Price, f => f.Finance.Amount())
                .RuleFor(t => t.CategoryId, f => f.Random.Int(min: 1, max: _CategoryAmmount));

            context.Products.AddRange(faker.Generate(_ProductAmmount));
        }

        private void SeedWithFacture(Context context)
        {
            int id = 1;

            var faker = new Faker<Facture>()
                .RuleFor(t => t.Id, _ => id++)
                .RuleFor(t => t.TotalAmmonut, f => f.Finance.Amount())
                .RuleFor(t => t.DocumentTypeId, f => f.Random.Int(min: 1, max: _DocumentTypeAmmount))
                .RuleFor(t => t.BuyDate, f => f.Date.Between(DateTime.Now.AddYears(5), DateTime.Now));

            context.Factures.AddRange(faker.Generate(_FactureAmmount));
        }

        private void SeedWithFactureDetail(Context context)
        {
            int id = 1;

            var faker = new Faker<FactureDetail>()
                .RuleFor(t => t.Id, _ => id++)
                .RuleFor(t => t.FactureId, f => f.Random.Int(min: 1, max: _FactureAmmount))
                .RuleFor(t => t.ProductId, f => f.Random.Int(min: 1, max: _ProductAmmount))
                .RuleFor(t => t.Quantity, f => f.Random.Int(min: 1, max: 64));

            context.FactureDetails.AddRange(faker.Generate(_FactureDetailAmmount));
        }
    }
}