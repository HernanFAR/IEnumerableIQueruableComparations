using EntityProjections_NetCore.Repository;
using System.Linq;
using static System.Console;

namespace EntityProjections_NetCore
{
   
    class Program
    {
        static void Main(string[] args)
        {
            using var context = new Context();

            WriteLine($"Proyectando entidades de tipo {{{typeof(Category).Name}}} a tipo {{{typeof(ProjectionClass).Name}}}...");

            var projectedNonRelated = context.Categories
                .Where(e => e.Id % 2 == 0)
                .Select(e => new ProjectionClass 
                { 
                    Name = e.Name 
                })
                .ToList();

            WriteLine($"Logrado");

            WriteLine($"Proyectando entidades de tipo {{{typeof(Category).Name}}} a tipo {{{typeof(Product).Name}}}...");

            var projectedRelatedByBd = context.Categories
                .Where(e => e.Id % 2 == 0)
                .Select(e => new Product 
                { 
                    Name = e.Name 
                })
                .ToList();

            WriteLine($"Logrado");
        }
    }
}
