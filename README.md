# EntityFrameworkTests

Proyecto creado para verificar ciertos tematicas que me causaron incertidumbre respecto a EntityFramework (Core)

## Duda 1: IEnumerable vs IQueryable
<details>
  <summary>Ver detalle</summary>
  
  ## ¿Da IQuerayble una mejor performance que usar un IEnumerable cuando haces consultas mediante EF (Core)

  Proyectos relacionados:
  - IQueryableVsIEnumerable_Net_Seeder
  - IQueryableVsIEnumerable_NetCore

  ### Conclusiones

  IQueryable efectivamente da mas performance que usar un IEnumerable. 

  Esto pasa porque la interface IQueryable internamente genera una consulta a medida que se le van a acumulando metodos, es decir:

  ```c#
  /*
    Esto genera un equivalente de: 
    Select * from Categories;
  */
  var entities = DbContext.Categories
      .ToList();

  /* 
    Esto genera un equivalente de: 
    Select * from Product 
    Where Product.Proce > 100;
  */
  var entities = DbContext.Entity
      .Where(e => e.Price > 100)
      .ToList();

  /* 
    Esto genera un equivalente de: 
    Select top (1) * from Product 
    Where Product.Proce > 100;
  */
  var entities = DbContext.Entity
      .Where(e => e.Id == 25)
      .SingleOrDefault();
  ```

  Y asi sucesivamente. Puedes ver el detalle de las funciones generadas al final (Work In Progress)
  
</details>

## Duda 2: Proyecciones de entre entidades de EF (Core)

> Work in progress

## Duda 3: Agregado de entidades relacionadas en contextos diferentes (Modo desconectado) EF (Core)

> Work in progress

## Duda 4: Uso de SplitQuery para performance EF Core

> Work in progress

## Codigo SQL generado por C# con LINQ To Entities
<details>
  <summary>Ver consultas</summary>
  
  > Codigo C#
  
  ```c#
  var entities = DbContext.Entity
      .Where(e => e.Id == 25)
      .SingleOrDefault();
  ```
  
  > Codigo SQL (No exacto, genera uno similar)
  
  ```sql
  Select top (1) * from Product 
  Where Product.Proce > 100;
  ```
  
</details>
