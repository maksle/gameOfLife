using System;
using System.Timers;


namespace GameOfLife
{

    class Cell
    {
        private int _state;

        public Cell()
        {
            _state = 0;
        }

        public Cell(int val)
        {
            _state = val;
        }

        public int State
        {
            get
            {
                return _state;
            }
            set
            {
                _state = value;
            }
        }

        public void Toggle()
        {
            if (_state == 1)
            {
                _state = 0;
            }
            else
            {
                _state = 1;
            }
        }

		public override string ToString() 
		{
			if (_state == 1) {
				return ".";
			} else {
				return " ";
			}
		}

		public void Draw()
		{
			Console.Write (this.ToString ());
		}
    }

    class Grid
    {
        private Cell[,] _grid;
		private int _size;

        public Grid(int size)
        {
			_size = size;
            _grid = new Cell[size, size];
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    _grid[x,y] = new Cell();
                }
            }
        }

		public int Size 
		{
			get { return _size; }
		}

		public void setStartPosition(int[] position)
		{
			int pos = _size/2;
			for (int i = 0; i < position.Length; i++) 
			{
				_grid [2, pos].State = i;
				pos++;
			}
		}

		public Cell getCell(int x, int y)
		{
			return _grid [x, y];
		}

		public Cell getCell(int[] coords) 
		{
			int x = coords [0];
			int y = coords [1];
			return _grid [x, y];
		}

		public void Draw()
		{
			try { 
				Console.Clear ();
			} catch (Exception) {
				// do nothing
			}
			for (int x = 0; x < _size; x++)
			{
				for (int y = 0; y < _size; y++)
				{
					getCell (x, y).Draw ();
				}
				Console.Write ("\n");
			}
		}
    }

	class Rules 
	{
		private Grid _grid;

		public Rules(Grid g)
		{
			_grid = g;
		}

		public int getNumLiveNeighbors(int x, int y)
		{
			int alive = 
				_grid.getCell(x - 1, y - 1).State 
				+ _grid.getCell(x - 1, y).State 
				+ _grid.getCell(x - 1, y + 1).State
				+ _grid.getCell(x, y - 1).State
				+ _grid.getCell(x, y + 1).State
				+ _grid.getCell(x + 1, y - 1).State
				+ _grid.getCell(x + 1, y).State
				+ _grid.getCell(x + 1, y + 1).State;
			return alive;
		}

		public int applyToCell(int x, int y) 
		{
			int liveNeighbors = getNumLiveNeighbors (x, y);
			if (_grid.getCell (x, y).State == 1) {
				if (liveNeighbors < 2) {
					return 0;
				} else if (liveNeighbors > 3) {
					return 0;
				} else {
					return 1;
				}
			}
			else { // state is 0
				if (liveNeighbors == 3) {
					return 1;
				}
				else {
					return 0;
				}
			}
		}	

		public Grid applyToGrid() 
		{
			int gsize = _grid.Size;
			Grid output = new Grid (gsize);
			for (int x = 0; x < gsize; x++) {
				for (int y = 0; y < gsize; y++) {
					output.getCell(x, y).State = applyToCell (x, y);
				}
			}
			return output;
		}
	}

    class Play
    {
		private static Timer loopTimer;
		private Grid grid;

		public void Init() 
		{
			grid = new Grid (100);
			int[] start = {1,1,1,1};
			grid.setStartPosition (start);
			Loop();
		}

		public void Loop() 
		{
			loopTimer = new Timer (1000);
			loopTimer.Elapsed += displayNewGen;
			loopTimer.Enabled = true;
		}

		private static void displayNewGen (object source, ElapsedEventArgs e) 
		{

		}

		public void Stop() 
		{
			loopTimer.Stop ();
		}

        static void Main(string[] args)
        {
			Console.WriteLine ("Hi there");
        }
    }

}