using Bare.Infrastructure.Controls;
using Bare.Primitive.UI;

var output = new ConsoleUiOutput();
var surface = new TextSurface(Math.Max(10, output.Width), 3);

surface.Fill(' ');
surface.SetText(0, 0, "Bare.Infrastructure.Controls POC");
surface.SetText(0, 1, "TextSurface rendering");
surface.SetText(0, 2, "Press Enter to exit");

output.Clear();
surface.RenderTo(output);

_ = Console.ReadLine();
