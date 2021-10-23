using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;

namespace JeuDeLaVie
{
    public struct Coords
    {
        public readonly int x;
        public readonly int y;
        public Coords(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public override string ToString()
        {
            return $"({x}, {y})";
        }
    }

    public class Cell
    {
        public bool isAlive { get; private set; }

        private bool nextState;

        public Cell()
        {
            isAlive = false;
        }
        public Cell(bool state)
        {
            isAlive = state;
        }

        public void ComeAlive()
        {
            nextState = true;
        }

        public void PassAway()
        {
            nextState = false;
        }

        public void Update()
        {
            isAlive = nextState;
        }
    }

    public class Grid
    {
        private int n;
        public Cell[,] TabCells;

        public Grid(int nbCells, List<Coords> AliveCellCords)
        {
            n = nbCells;
            TabCells = new Cell[n, n];
            for (int ord = 0; ord < n; ord++)
            {
                for (int abs = 0; abs < n; abs++)
                {
                    Coords coords = new Coords(abs, ord);
                    bool isAlive = false;
                    if (AliveCellCords.Contains(coords))
                    {
                        AliveCellCords.Remove(coords);
                        isAlive = true;
                    }

                    TabCells[coords.y, coords.x] = new Cell(isAlive);
                }
            }
        }

        public int GetNbAliveNeighboor(Coords coords)
        {
            int x = coords.x;
            int y = coords.y;
            int sum = 0;

            int[,] listNeighboor = new int[,]
            {
                {x - 1, y - 1}, {x, y - 1}, {x + 1, y - 1},
                {x - 1, y}, {x + 1, y},
                {x - 1, y + 1}, {x, y + 1}, {x + 1, y + 1}
            };

            for (int loop = 0; loop < 8; loop++)
            {
                int X = listNeighboor[loop,0];
                int Y = listNeighboor[loop,1];
                if(X < n && Y < n && X >= 0 && Y >= 0)
                    sum += Convert.ToInt32(TabCells[Y, X].isAlive);
            }

        return sum;
        }

        public List<Coords> GetCoordsCellsAlive()
        {
            List<Coords> coordsList = new List<Coords>();
            for (int ord = 0; ord < n; ord++)
                for (int abs = 0; abs < n; abs++)
                    if (TabCells[ord, abs].isAlive)
                        coordsList.Add(new Coords(abs, ord));

            return coordsList;
        }

        private string[] AffichageTexteCell(Cell cell)
        {
            string[] strings = new string[] {"+---","| "};
            if (cell.isAlive)
                strings[1] += "X ";
            else
                strings[1] += "  ";
            return strings;
        }
        
        public void DisplayGrid()
        {
            string[,] affichage = new string[n,3];
            for (int ord = 0; ord < n; ord++)
                for (int abs = 0; abs < n; abs++)
                {
                    string[] strings = AffichageTexteCell(TabCells[ord, abs]);
                    for (int loop = 0; loop < 2; loop++)
                        affichage[ord, loop] += strings[loop];
                    if (abs == n - 1)
                    {
                        affichage[ord, 0] += "+";
                        affichage[ord, 1] += "|";

                    }
                    if (ord == n - 1)
                    {
                        affichage[ord, 2] += "+---";
                        if (abs == n - 1)
                            affichage[ord, 2] += "+";
                    }
                }
            

            foreach (string ligne in affichage)
            {
                if(ligne != null)
                    Console.WriteLine(ligne);
            }
        }

        public void UpdateGrid()
        {
            for (int ord = 0; ord < n; ord++)
            {
                for (int abs = 0; abs < n; abs++)
                {
                    Coords coords = new Coords(abs, ord);
                    int nbNeighboor = GetNbAliveNeighboor(coords);
                    Cell cell = TabCells[coords.y, coords.x];
                    if (!cell.isAlive)
                    {
                        if (nbNeighboor == 3)
                        {
                            cell.ComeAlive();
                            //Console.Write("Apparition vie"); //Debug
                        }
                        else
                        {
                            cell.PassAway();
                            //Console.Write("Rien de spécial"); //Debug
                        }
                    }
                    else if (nbNeighboor != 2 && nbNeighboor != 3)
                    {
                        cell.PassAway();
                        //Console.Write("Mort de la cellule"); //Debug
                    }
                    else
                    {
                        cell.ComeAlive();
                        //Console.Write("La cellule survit"); //Debug
                    }
                    //Console.WriteLine($" nb Parents = {nbNeighboor} {coords}"); //Debug
                }
            }

            foreach (Cell cell in TabCells)
            {
                cell.Update();
            }
        }
    }

    public class Game
    {
        private int n;
        private int iter;
        public Grid grid;
        private readonly List<Coords> AliveCellsCoords = new List<Coords>();

        public Game(int nbCells, int nbIterations, int nbAlive)
        {
            this.n = nbCells;
            this.iter = nbIterations;
            Random random = new();
            for (int loop = 0; loop < nbAlive; loop++)
            {
                Coords coords = new Coords(random.Next(n), random.Next(n));
                if (!AliveCellsCoords.Contains(coords))
                    AliveCellsCoords.Add(coords);
                else
                    loop--;
            }
            grid = new Grid(n, AliveCellsCoords);
        }

        public void RunGameConsole()
        {
            for (int loop = 0; loop < iter; loop++)
            {
                if (grid.GetCoordsCellsAlive().Any())
                {
                    grid.DisplayGrid();
                    grid.UpdateGrid();
                    Console.ReadLine();
                    Console.WriteLine(grid.GetCoordsCellsAlive());
                }
            }
            grid.DisplayGrid();
        }
    }
    
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Nombre cellules ? ");
            int nbCell = Int32.Parse(Console.ReadLine());
            
            Console.Write("Nombre itérations ? ");
            int nbIter = Int32.Parse(Console.ReadLine());

            Console.Write("Nombre cellules vivantes ? ");
            int nbAlive = Int32.Parse(Console.ReadLine());
            
            Game game = new Game(nbCell, nbIter, nbAlive);
            game.RunGameConsole();
        }
    }
}