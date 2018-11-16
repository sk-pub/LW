using System;

namespace LinnworksTestP3
{
	class Program
	{
		static void Main(string[] args)
		{
			var width = 3;
			Console.Write($"Enter width of the world [{width}]:");
			if (int.TryParse(Console.ReadLine(), out int userWidth))
			{
				width = userWidth;
			}

			var height = 2;
			Console.Write($"Enter height of the world [{height}]:");
			if (int.TryParse(Console.ReadLine(), out int userHeight))
			{
				height = userHeight;
			}

			var world = new World(width, height);

			Console.WriteLine();
			Console.WriteLine($"Hello World {width} x {height}!");
			world.PrintCells();
			Console.WriteLine();

			var startX = 0;
			Console.Write($"Enter start location X [{startX}]:");
			if (int.TryParse(Console.ReadLine(), out int userStartX))
			{
				startX = userStartX;
			}

			var startY = 0;
			Console.Write($"Enter start location Y [{startY}]:");
			if (int.TryParse(Console.ReadLine(), out int userStartY))
			{
				startY = userStartY;
			}

			var endX = width - 1;
			Console.Write($"Enter end location X [{endX}]:");
			if (int.TryParse(Console.ReadLine(), out int userEndX))
			{
				endX = userEndX;
			}

			var endY = height - 1;
			Console.Write($"Enter end location Y [{endY}]:");
			if (int.TryParse(Console.ReadLine(), out int userEndY))
			{
				endY = userEndY;
			}

			var startLoc = new Location(startX, startY);
			var endLoc = new Location(endX, endY);

			Console.WriteLine();
			Console.WriteLine("Looking for the shortest way...");
			Console.WriteLine();

			var shortestWay = world.FindShortestWay(startLoc, endLoc);

			if (shortestWay == null)
			{
				Console.WriteLine("There is no way between this locations");
			}
			else
			{
				for (var i = 0; i < shortestWay.Length - 1; i++)
				{
					Console.Write($"({shortestWay[i].X},{shortestWay[i].Y})->");
				}
				Console.WriteLine($"({shortestWay[shortestWay.Length - 1].X},{shortestWay[shortestWay.Length - 1].Y})");
			}

			Console.WriteLine();
			Console.WriteLine("Press any key...");
			Console.ReadKey();
		}
	}
}
