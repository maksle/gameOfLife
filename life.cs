using System;
using System.Timers;

namespace GameOfLife
{
  class Cell
  {
    private int _state;

    public Cell ()
    {
      _state = 0;
    }

    public Cell (int val)
    {
      _state = val;
    }

    public int State {
      get {
        return _state;
      }
      set {
        _state = value;
      }
    }

    public void Toggle ()
    {
      if (_state == 1) {
        _state = 0;
      } else {
        _state = 1;
      }
    }

    public override string ToString ()
    {
      if (_state == 1) {
        return "x";
      } else {
        return ".";
      }
    }

    public void Draw ()
    {
      Console.Write (this.ToString ());
    }
  }

  class Grid
  {
    private Cell[,] _grid;
    private int _size;

    public Grid (int size)
    {
      _size = size;
      _grid = new Cell[size, size];
      for (int x = 0; x < size; x++) {
        for (int y = 0; y < size; y++) {
          _grid [x, y] = new Cell ();
        }
      }
    }

    public int Size {
      get { return _size; }
    }

    public void setStartPosition (int[] position)
    {
      int row = _size / 3;

      int longestChain = 0;
      int chain = 0;
      for (int i = 0; i < position.Length; i++) {
        if (position [i] != -1)
          chain++;
        else {
          if (chain > longestChain) {
            longestChain = chain;
          }
          chain = 0;
        }
      }
      if (chain > longestChain) {
        longestChain = chain;
      }
      int initPos = (_size / 2) - (longestChain / 2); // small todo: need to handle negative #s

      int pos = initPos;
      for (int i = 0; i < position.Length; i++) {
        if (position [i] == -1) { // go to next row
          row++;
          pos = initPos;
        } else {
          _grid [row, pos].State = position [i];
          pos++;
        }
      }
    }

    public Cell getCell (int x, int y)
    {
      return _grid [x, y];
    }

    public Cell getCell (int[] coords)
    {
      int x = coords [0];
      int y = coords [1];
      return _grid [x, y];
    }

    public void Draw ()
    {
      for (int x = 0; x < _size; x++) {
        for (int y = 0; y < _size; y++) {
          getCell (x, y).Draw ();
        }
        Console.Write ("\n");
      }
    }
  }

  class Rules
  {
    public static int getNumLiveNeighbors (Grid grid, int x, int y)
    {
      int alive = 0;
      try {
        alive += grid.getCell (x - 1, y - 1).State;
      } catch (IndexOutOfRangeException) {
      }
      try {
        alive += grid.getCell (x - 1, y).State;
      } catch (IndexOutOfRangeException) {
      }
      try {
        alive += grid.getCell (x - 1, y + 1).State;
      } catch (IndexOutOfRangeException) {
      }
      try {
        alive += grid.getCell (x, y - 1).State;
      } catch (IndexOutOfRangeException) {
      }
      try {
        alive += grid.getCell (x, y + 1).State;
      } catch (IndexOutOfRangeException) {
      }
      try {
        alive += grid.getCell (x + 1, y - 1).State;
      } catch (IndexOutOfRangeException) {
      }
      try {
        alive += grid.getCell (x + 1, y).State;
      } catch (IndexOutOfRangeException) {
      }
      try {
        alive += grid.getCell (x + 1, y + 1).State;
      } catch (IndexOutOfRangeException) {
      }
      return alive;
    }

    public static int applyToCell (Grid grid, int x, int y)
    {
      int liveNeighbors = getNumLiveNeighbors (grid, x, y);
      if (grid.getCell (x, y).State == 1) {
        if (liveNeighbors < 2) {
          return 0;
        } else if (liveNeighbors > 3) {
          return 0;
        } else {
          return 1;
        }
      } else { // state is 0
        if (liveNeighbors == 3) {
          return 1;
        } else {
          return 0;
        }
      }
    }

    public static Grid applyToGrid (Grid grid)
    {
      int gsize = grid.Size;
      Grid output = new Grid (gsize);
      for (int x = 0; x < gsize; x++) {
        for (int y = 0; y < gsize; y++) {
          output.getCell (x, y).State = applyToCell (grid, x, y);
        }
      }
      return output;
    }
  }

  class Play
  {
    private static Timer loopTimer;
    private static Grid grid;

    public static void Init (int[] start)
    {
      grid = new Grid (50);
      grid.setStartPosition (start);
      grid.Draw ();
      Loop ();
    }

    private static void Loop ()
    {
      loopTimer = new Timer (1000);
      loopTimer.Elapsed += displayNewGen;
      loopTimer.Enabled = true;
    }

    private static void displayNewGen (Object source, ElapsedEventArgs e)
    {
      try {
        Console.Clear ();
      } catch (Exception) {
        // do nothing
      }
      Console.WriteLine ();
      // Console.WriteLine ("The event was raised at {0}", e.SignalTime);
      grid = Rules.applyToGrid (grid);
      grid.Draw ();
    }

    private static void Stop ()
    {
      loopTimer.Stop ();
    }

    static void Main (string[] args)
    {
      int[] heartByAlina = { 1, 1, 0, 1, 1, -1, 1, 0, 1, 0, 1, -1, 0, 1, 0, 1, -1, 0, 1, 0, 1, -1, 0, 0, 1 };
      int[] glider1 = { 0, 1, -1, 0, 1, 1, -1, 1, 0, 1 };
      int[] rPentomino = { 0, 1, 1, -1, 1, 1, -1, 0, 1};

      Console.WriteLine ("Press the Enter key to exit the program... ");
      Init (rPentomino);
      Console.ReadLine ();
      Console.WriteLine ("Terminating the application...");
    }
  }
}