using System;
using System.Collections.Generic;

namespace LinnworksTestP3
{
	public class Location
	{
		public int X;
		public int Y;

		public Location(int x, int y)
		{
			this.X = x;
			this.Y = y;
		}
	}

	public class World
	{
		public class Cell : Location
		{
			// Defines cell passability from 0 (can't go) to 100 (normal passability)
			// The higher is passability, the quicker it is possible to pass the cell
			public byte Passability;

			public Cell(int x, int y, byte passability)
				: base(x, y)
			{
				this.Passability = passability;
			}
		}

		private Cell[,] cells; // World map

		public World(int sizeX, int sizeY)
		{
			var rnd = new Random();

			// Build map and randomly set passability for each cell
			cells = new Cell[sizeX, sizeY];
			for (int x = 0; x < sizeX; x++)
				for (int y = 0; y < sizeY; y++)
					cells[x, y] = new Cell(x, y, (byte)rnd.Next(0, 100));
		}

		// Dijkstra algorithm
		public Location[] FindShortestWay(Location startLoc, Location endLoc)
		{
			var width = cells.GetLength(0);
			var height = cells.GetLength(1);
			var minDistances = new double[width, height];
			for (var i = 0; i < width; i++)
			{
				for (var j = 0; j < height; j++)
				{
					minDistances[i, j] = double.PositiveInfinity;
				}
			}

			var visited = new HashSet<Cell>();
			minDistances[startLoc.X, startLoc.Y] = GetDistance(cells[startLoc.X, startLoc.Y].Passability);

			var loc = startLoc;

			// Find min distance from each location
			while (loc != null)
			{
				var neighbors = GetNeighbors(loc, visited);
				neighbors.Sort((x, y) =>
				{
					var dX = GetDistance(cells[x.X, x.Y].Passability);
					var dY = GetDistance(cells[y.X, y.Y].Passability);

					return dX.CompareTo(dY);
				});

				foreach (var n in neighbors)
				{
					var d = minDistances[loc.X, loc.Y] + GetDistance(cells[n.X, n.Y].Passability);
					if (d < minDistances[n.X, n.Y])
					{
						minDistances[n.X, n.Y] = d;
					}
				}
				visited.Add(cells[loc.X, loc.Y]);

				loc = GetNextLoc(visited, minDistances);
			}

			// Find path
			if (double.IsPositiveInfinity(minDistances[endLoc.X, endLoc.Y]))
				return null;

			var path = new List<Location>();

			visited.Clear();
			loc = endLoc;

			while (loc.X != startLoc.X || loc.Y != startLoc.Y)
			{
				path.Add(loc);

				var neighbors = GetNeighbors(loc, visited);
				var md = minDistances[loc.X, loc.Y] - GetDistance(cells[loc.X, loc.Y].Passability);
				neighbors.Sort((x, y) =>
				{
					var mdX = minDistances[x.X, x.Y];
					var mdY = minDistances[y.X, y.Y];

					return (mdX - md).CompareTo(mdY - md);
				});
				loc = neighbors[0];
			}
			path.Add(startLoc);
			
			path.Reverse();
			return path.ToArray();
		}

		private List<Location> GetNeighbors(Location loc, HashSet<Cell> visited)
		{
			var width = cells.GetLength(0);
			var height = cells.GetLength(1);

			var result = new List<Location>();

			if (loc.Y > 0)
			{
				var cell = cells[loc.X, loc.Y - 1];
				if (!visited.Contains(cell))
				{
					result.Add(cell);
				}
			}
			if (loc.X < width - 1)
			{
				var cell = cells[loc.X + 1, loc.Y];
				if (!visited.Contains(cell))
				{
					result.Add(cell);
				}
			}
			if (loc.Y < height - 1)
			{
				var cell = cells[loc.X, loc.Y + 1];
				if (!visited.Contains(cell))
				{
					result.Add(cell);
				}
			}
			if (loc.X > 0)
			{
				var cell = cells[loc.X - 1, loc.Y];
				if (!visited.Contains(cell))
				{
					result.Add(cell);
				}
			}

			return result;
		}

		private Location GetNextLoc(HashSet<Cell> visited, double[,] minDistances)
		{
			var width = cells.GetLength(0);
			var height = cells.GetLength(1);
			Location result = null;
			var dMin = double.PositiveInfinity;

			for (var i = 0; i < width; i++)
			{
				for (var j = 0; j < height; j++)
				{
					var cell = cells[i, j];
					if (!visited.Contains(cell))
					{
						var d = minDistances[i, j];
						if (d < dMin)
						{
							dMin = d;
							result = cell;
						}
					}
				}
			}

			return result;
		}

		private static double GetDistance(int passability)
		{
			return 100d / passability;
		}

		public void PrintCells()
		{
			var color = Console.ForegroundColor;

			var width = cells.GetLength(0);
			var height = cells.GetLength(1);

			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.Write("   ");
			for (var i = 0; i < width; i++)
			{
				Console.Write($"{i.ToString().PadLeft(3, ' ')}");
			}
			Console.ForegroundColor = ConsoleColor.White;
			Console.WriteLine();

			for (var j = 0; j < height; j++)
			{
				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.Write($"{j.ToString().PadLeft(3, ' ')}");
				Console.ForegroundColor = ConsoleColor.White;

				for (var i = 0; i < width; i++)
				{
					Console.Write($"{cells[i, j].Passability.ToString().PadLeft(3, ' ')}");
				}
				Console.WriteLine();
			}

			Console.ForegroundColor = color;
		}
	}
}
