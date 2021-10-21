using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

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
            
            for (int Y = y - 1; Y <= Y + 1; Y++)
            {
                for (int X = x-1; X <= x+1; X++)
                {
                    sum += Convert.ToInt32(TabCells[X, Y].isAlive);
                }
            }

            return sum - Convert.ToInt32(TabCells[x, y].isAlive);
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
            List<Coords> coordsList = GetCoordsCellsAlive();
            foreach (Coords coords in coordsList)
            {
                int nbNeighboor = GetNbAliveNeighboor(coords);
                Cell cell = TabCells[coords.y, coords.x];
                if (!cell.isAlive)
                {
                    if (nbNeighboor == 3)
                        cell.ComeAlive();
                }
                else if (!(nbNeighboor == 2 || nbNeighboor == 3))
                    cell.PassAway();
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Grid grid = new Grid(2,new List<Coords>{new Coords(0,0)});
            grid.DisplayGrid();
        }
    }
}