// See https://aka.ms/new-console-template for more information


using GameStore.Services;
using Spectre.Console;

VideojuegoService servicio = new VideojuegoService();

// Datos iniciales de prueba
servicio.Registrar("FIFA", "PlayStation", 2500m);
servicio.Registrar("Minecraft", "PC", 1800m);
servicio.Registrar("God of War", "PlayStation", 3200m);

string opcion = "";

while (opcion != "Salir")
{
    AnsiConsole.Clear();

    opcion = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
            .Title("[bold yellow]GAMESTORE - GESTIÓN DE VIDEOJUEGOS[/]")
            .PageSize(10)
            .AddChoices(
                "Listar videojuegos",
                "Registrar videojuego",
                "Eliminar videojuego",
                "Salir"
            )
    );

    switch (opcion)
    {
        case "Listar videojuegos":
        {
            AnsiConsole.Clear();

            var videojuegos = servicio.Listar();

            if (videojuegos.Count == 0)
            {
                AnsiConsole.MarkupLine(
                    "[yellow]No hay videojuegos registrados.[/]"
                );
            }
            else
            {
                var tabla = new Table();

                tabla.Title("[bold green]Catálogo de videojuegos[/]");
                tabla.Border(TableBorder.Rounded);

                tabla.AddColumn("ID");
                tabla.AddColumn("Nombre");
                tabla.AddColumn("Plataforma");
                tabla.AddColumn(
                    new TableColumn("Precio (RD$)").RightAligned()
                );

                foreach (var videojuego in videojuegos)
                {
                    tabla.AddRow(
                        videojuego.Id.ToString(),
                        videojuego.Nombre,
                        videojuego.Plataforma,
                        videojuego.Precio.ToString("N2")
                    );
                }

                AnsiConsole.Write(tabla);
            }

            AnsiConsole.WriteLine();
            AnsiConsole.MarkupLine(
                "[grey]Presiona una tecla para continuar...[/]"
            );
            Console.ReadKey(true);
            break;
        }

        case "Registrar videojuego":
        {
            AnsiConsole.Clear();
            AnsiConsole.MarkupLine(
                "[bold green]REGISTRAR VIDEOJUEGO[/]"
            );

            string nombre = AnsiConsole.Ask<string>(
                "Nombre del videojuego:"
            );

            string plataforma = AnsiConsole.Ask<string>(
                "Plataforma:"
            );

            decimal precio = AnsiConsole.Ask<decimal>(
                "Precio en RD$:"
            );

            if (string.IsNullOrWhiteSpace(nombre) ||
                string.IsNullOrWhiteSpace(plataforma) ||
                precio < 0)
            {
                AnsiConsole.MarkupLine(
                    "[red]Los datos no son válidos.[/]"
                );
            }
            else
            {
                servicio.Registrar(
                    nombre.Trim(),
                    plataforma.Trim(),
                    precio
                );

                AnsiConsole.MarkupLine(
                    "[green]Videojuego registrado correctamente.[/]"
                );
            }

            AnsiConsole.WriteLine();
            AnsiConsole.MarkupLine(
                "[grey]Presiona una tecla para continuar...[/]"
            );
            Console.ReadKey(true);
            break;
        }

        case "Eliminar videojuego":
        {
            AnsiConsole.Clear();
            AnsiConsole.MarkupLine(
                "[bold red]ELIMINAR VIDEOJUEGO[/]"
            );

            var videojuegos = servicio.Listar();

            if (videojuegos.Count == 0)
            {
                AnsiConsole.MarkupLine(
                    "[yellow]No hay videojuegos para eliminar.[/]"
                );
            }
            else
            {
                var tabla = new Table();
                tabla.Border(TableBorder.Rounded);

                tabla.AddColumn("ID");
                tabla.AddColumn("Nombre");
                tabla.AddColumn("Plataforma");

                foreach (var videojuego in videojuegos)
                {
                    tabla.AddRow(
                        videojuego.Id.ToString(),
                        videojuego.Nombre,
                        videojuego.Plataforma
                    );
                }

                AnsiConsole.Write(tabla);

                int id = AnsiConsole.Ask<int>(
                    "Introduce el ID del videojuego que deseas eliminar:"
                );

                bool eliminado = servicio.Eliminar(id);

                if (eliminado)
                {
                    AnsiConsole.MarkupLine(
                        "[green]Videojuego eliminado correctamente.[/]"
                    );
                }
                else
                {
                    AnsiConsole.MarkupLine(
                        "[red]No existe un videojuego con ese ID.[/]"
                    );
                }
            }

            AnsiConsole.WriteLine();
            AnsiConsole.MarkupLine(
                "[grey]Presiona una tecla para continuar...[/]"
            );
            Console.ReadKey(true);
            break;
        }

        case "Salir":
        {
            AnsiConsole.MarkupLine(
                "[yellow]Gracias por utilizar GameStore. ¡Hasta pronto![/]"
            );
            break;
        }
    }
}
