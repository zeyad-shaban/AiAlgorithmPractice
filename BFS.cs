class Node<T>(T value, Node<T>? next = null) {
    public T Value = value;
    public Node<T>? Next = next;
}

static class MazeElements {
    public const char Empty = ' ';
    public const char Player = 'O';
    public const char Wall = 'X';
    public const char Explored = '-';
    public const char Win = 'W';
}

class Program {
    static readonly Dictionary<char, (int, int)> Moves = new() {
        { 'w', (-1, 0) },
        { 's', (1, 0) },
        { 'a', (0, -1) },
        { 'd', (0, 1) },
    };


    static int[] MovePlayer(int[] playerPos, char move) => move == 'N' ? playerPos : [playerPos[0] + Moves[move].Item1, playerPos[1] + Moves[move].Item2];
    static bool isInBoundary(char[][] Maze, int[] pos) => (pos[0] >= 0 && pos[0] < Maze.Length) && (pos[1] >= 0 && pos[1] < Maze[0].Length);

    static void Main(string[] args) {
        // char[][] Maze = [
        //     [' ', ' ', ' '],
        //     [' ', ' ', ' '],
        //     ['O', ' ', 'W'],
        // ];
        // char[][] Maze = [
        //     ['X', ' ', 'O'],
        //     [' ', ' ', 'X'],
        //     [' ', 'X', ' '],
        //     [' ', 'X', 'W'],
        // ];
        char[][] Maze = [ // interesting case where ai picked the longest path due to depth first search algorithm
            [' ', ' ', 'O', ' '],
            [' ', 'X', ' ', ' '],
            [' ', 'X', 'W', ' '],
        ];

        int[] playerPos = new int[2];
        int[] winPos = new int[2];
        for (int i = 0; i < Maze.Length; ++i)
            for (int j = 0; j < Maze[0].Length; ++j)
                if (Maze[i][j] == MazeElements.Player)
                    playerPos = [i, j];
                else if (Maze[i][j] == MazeElements.Win)
                    winPos = [i, j];

        Queue<int[]> queue = [];
        queue.Enqueue(playerPos);

        Dictionary<(int, int), (char, (int, int))> cameFrom = [];

        int[] thisPos = playerPos;
        while (queue.Count > 0) {
            thisPos = queue.Dequeue();
            if (!isInBoundary(Maze, thisPos) || Maze[thisPos[0]][thisPos[1]] == MazeElements.Wall || Maze[thisPos[0]][thisPos[1]] == MazeElements.Explored)
                continue;
            else if (Maze[thisPos[0]][thisPos[1]] == MazeElements.Win)
                break;

            foreach (var move in Moves) {
                int[] nextMove = MovePlayer(thisPos, move.Key);
                if (!cameFrom.ContainsKey((nextMove[0], nextMove[1]))) cameFrom[(nextMove[0], nextMove[1])] = (move.Key, (thisPos[0], thisPos[1]));
                queue.Enqueue(nextMove);
            }

            Maze[thisPos[0]][thisPos[1]] = MazeElements.Explored;
        }

        Stack<char> path = [];
        while (!thisPos.SequenceEqual(playerPos)) {
            // path.Push(Dictionary[thisPos]);
            var fromVal = cameFrom[(thisPos[0], thisPos[1])];
            path.Push(fromVal.Item1);
            thisPos = [fromVal.Item2.Item1, fromVal.Item2.Item2];
        }

        char currStep;
        while (true) {
            Maze[playerPos[0]][playerPos[1]] = 'O';
            Console.WriteLine("--------------");
            for (int i = 0; i < Maze.Length; ++i) {
                for (int j = 0; j < Maze[0].Length; ++j)
                    Console.Write($" | {Maze[i][j]}");

                Console.Write("\n--------------\n");
            }

            // apply movement
            if (!path.TryPop(out currStep)) break;
            int[] newPos = MovePlayer(playerPos, currStep);

            if (!isInBoundary(Maze, newPos)) {
                Console.WriteLine("Out of boundary, AI FAILED");
                break;
            }
            else if (Maze[newPos[0]][newPos[1]] == 'X') {
                Console.WriteLine("There is a wall, AI FAILED");
                break;
            }
            else {
                Maze[playerPos[0]][playerPos[1]] = '-';
                playerPos = newPos;
            }

            Console.WriteLine("Press any key to continue");
            Console.ReadLine();
        }
        if (playerPos.SequenceEqual(winPos)) Console.WriteLine("The AI have found an exit, GREAT!");
        else Console.WriteLine("NO VALID PATH EXISTS (or the ai is a dummy)");
    }

}