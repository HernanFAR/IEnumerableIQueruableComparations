using IQueryableVsIEnumerable_NetCore.Repository;
using System;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using static System.Console;

namespace IQueryableVsIEnumerable_NetCore
{
    class Program
    {
        static void Main(string[] args)
        {
            MeasureFacturesOneCondition();
            WriteLine("----------------------------------------------------");
            MeasureFacturesTwoCondition();
            WriteLine("----------------------------------------------------");

            MeasureFacturesDetailOneCondition();
            WriteLine("----------------------------------------------------");
            MeasureFacturesDetailTwoCondition();
            WriteLine("----------------------------------------------------");

            MeasureFacturesDetailTwoConditionWithOr();
            WriteLine("----------------------------------------------------");

            ReadLine();
        }

        private static void MeasureFacturesOneCondition()
        {
            int documentTypeId = 1;

            // Verificando con IEnumerable
            var elapse = MeasureEnumerable<Facture>(out int count,
                e => e.DocumentTypeId == documentTypeId
            );

            Console.WriteLine($"IEnumerable tardo: {elapse}ms en cargar {count} entidades de tipo " +
                $"{GetName<Facture>()}, con una condicion\n");

            // Verificando con IQueryable
            elapse = MeasureQuery<Facture>(out count,
                e => e.DocumentTypeId == documentTypeId
            );

            Console.WriteLine($"IQueryable tardo: {elapse}ms en cargar {count} entidades de tipo " +
                $"{GetName<Facture>()}, con una condicion\n");
        }

        private static void MeasureFacturesTwoCondition()
        {
            int documentTypeId = 1;
            decimal minimumPrice = 1000m;

            // Verificando con IEnumerable
            var elapse = MeasureEnumerable<Facture>(out int count,
                e => e.DocumentTypeId == documentTypeId,
                e => e.TotalAmmonut > minimumPrice
            );

            Console.WriteLine($"IEnumerable tardo: {elapse}ms en cargar {count} entidades de tipo " +
                $"{GetName<Facture>()}, con dos condiciones\n");

            // Verificando con IQueryable
            elapse = MeasureQuery<Facture>(out count,
                e => e.DocumentTypeId == documentTypeId,
                e => e.TotalAmmonut > minimumPrice
            );

            Console.WriteLine($"IQueryable tardo: {elapse}ms en cargar {count} entidades de tipo " +
                $"{GetName<Facture>()}, con dos condiciones\n");
        }

        private static void MeasureFacturesDetailOneCondition()
        {
            int minQuantity = 15;

            // Verificando con IEnumerable
            var elapse = MeasureEnumerable<FactureDetail>(out int count,
                e => e.Quantity == minQuantity
            );

            Console.WriteLine($"IEnumerable tardo: {elapse}ms en cargar {count} entidades de tipo " +
                $"{GetName<FactureDetail>()}, con una condicion\n");

            // Verificando con IQueryable
            elapse = MeasureQuery<FactureDetail>(out count,
                e => e.Quantity == minQuantity
            );

            Console.WriteLine($"IQueryable tardo: {elapse}ms en cargar {count} entidades de tipo " +
                $"{GetName<FactureDetail>()}, con una condicion\n");
        }

        private static void MeasureFacturesDetailTwoCondition()
        {
            int minQuantity = 15;
            int[] validProducts = new int[] { 2, 5, 10, 23, 254 };

            // Verificando con IEnumerable
            var elapse = MeasureEnumerable<FactureDetail>(out int count,
                e => e.Quantity == minQuantity,
                e => validProducts.Contains(e.ProductId)
            );

            Console.WriteLine($"IEnumerable tardo: {elapse}ms en cargar {count} entidades de tipo " +
                $"{GetName<FactureDetail>()}, con dos condiciones\n");

            // Verificando con IQueryable
            elapse = MeasureQuery<FactureDetail>(out count,
                e => e.Quantity == minQuantity,
                e => validProducts.Contains(e.ProductId)
            );

            Console.WriteLine($"IQueryable tardo: {elapse}ms en cargar {count} entidades de tipo " +
                $"{GetName<FactureDetail>()}, con dos condiciones\n");
        }

        private static void MeasureFacturesDetailTwoConditionWithOr()
        {
            int minQuantity = 15;
            int[] validProducts = new int[] { 2, 5, 10, 23, 254 };
            int[] validFactures = new int[] { 1, 3, 5, 7, 11 };

            // Verificando con IEnumerable
            var elapse = MeasureEnumerable<FactureDetail>(out int count,
                e => e.Quantity == minQuantity,
                e => validProducts.Contains(e.ProductId) || validFactures.Contains(e.FactureId)
            );

            Console.WriteLine($"IEnumerable tardo: {elapse}ms en cargar {count} entidades de tipo " +
                $"{GetName<FactureDetail>()}, con dos condiciones, una opcional\n");

            // Verificando con IQueryable
            elapse = MeasureQuery<FactureDetail>(out count,
                e => e.Quantity == minQuantity,
                e => validProducts.Contains(e.ProductId) || validFactures.Contains(e.FactureId)
            );

            Console.WriteLine($"IQueryable tardo: {elapse}ms en cargar {count} entidades de tipo " +
                $"{GetName<FactureDetail>()}, con dos condiciones, una opcional\n");
        }

        private static string GetName<T>() => typeof(T).Name;

        static long MeasureEnumerable<T>(out int count, params Func<T, bool>[] action)
            where T : class
        {
            var stopWatch = new Stopwatch();

            using (var context = new Context())
            {
                //Inicializando contexto
                context.Categories.Find(1);

                stopWatch.Start();

                var query = context.GetAllIEnumerable<T>();

                action.ToList().ForEach(e => query = query.Where(e));

                count = query.ToList().Count;

                stopWatch.Stop();
            }

            return stopWatch.ElapsedMilliseconds;
        }

        static long MeasureQuery<T>(out int count, params Expression<Func<T, bool>>[] action)
            where T : class
        {
            var stopWatch = new Stopwatch();

            using (var context = new Context())
            {
                //Inicializando contexto
                context.Categories.Find(1);

                stopWatch.Start();

                var query = context.GetAllIQueryable<T>();

                action.ToList().ForEach(e => query = query.Where(e));

                count = query.ToList().Count;

                stopWatch.Stop();
            }

            return stopWatch.ElapsedMilliseconds;
        }
    }
}
