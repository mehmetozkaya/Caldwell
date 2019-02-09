# Caldwell

When scaffolding existing db from EF Core, you should change incremental as below way on Configure methods at Context class.

entity.Property(e => e.Id).ForSqlServerUseSequenceHiLo("catalog_hilo").IsRequired(); // .ValueGeneratedNever();
