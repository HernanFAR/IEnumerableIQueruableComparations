using EntityProjections_NetCore.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace EntityProjections_NetCore
{
   
    class Program
    {
        static void Main(string[] args)
        {
            using var context = new Context();

            var projected = context.Categories
                .Where(e => e.Id % 2 == 0)
                .Select(e => new ProjectionClass 
                { 
                    Name = e.Name 
                })
                .ToList();
        }
    }
}
