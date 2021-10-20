using System;

namespace JeuDeLaVie
{
    struct Coords
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
        private bool isAlive;
        private bool nextState;

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
    
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
}