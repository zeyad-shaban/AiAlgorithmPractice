// static class MazeElements {
//     public const char Empty = ' ';
//     public const char Player = 'O';
//     public const char Wall = 'X';
//     public const char Explored = '-';
//     public const char Win = 'W';
// }


// class Node<T>(T value, Node<T>? next = null) {
//     public T Value = value;
//     public Node<T>? Next = next;
// }

// class Program {
//     static readonly Dictionary<char, (int, int)> Moves = new() {
//         { 'w', (-1, 0) },
//         { 's', (1, 0) },
//         { 'a', (0, -1) },
//         { 'd', (0, 1) },
//     };


//     static int[] MovePlayer(int[] playerPos, char move) => move == 'N' ? playerPos : [playerPos[0] + Moves[move].Item1, playerPos[1] + Moves[move].Item2];
//     static bool isInBoundary(char[][] Maze, int[] newPos) => (newPos[0] >= 0 && newPos[0] < Maze.Length) && (newPos[1] >= 0 && newPos[1] < Maze[0].Length);


//     static Node<char>? FindPath(char[][] Maze, int[] pos) {
//         if (!isInBoundary(Maze, pos) || Maze[pos[0]][pos[1]] == MazeElements.Wall || Maze[pos[0]][pos[1]] == MazeElements.Explored) return null;
//         else if (Maze[pos[0]][pos[1]] == MazeElements.Win)
//             return new Node<char>('N');

//         Maze[pos[0]][pos[1]] = MazeElements.Explored;
//         Node<char>? result = null;

//         foreach (var move in Moves) {
//             Node<char>? nextAction = FindPath(Maze, MovePlayer(pos, move.Key));
//             if (nextAction != null) {
//                 result = new(move.Key, nextAction);
//                 break;
//             }
//         }
//         return result;
//     }

//     static void Main(string[] args) {
//         char[][] Maze = [
//             [' ', ' ', ' '],
//             [' ', ' ', ' '],
//             [' ', ' ', ' '],
//             ['O', ' ', 'W'],
//         ];
//         // char[][] Maze = [
//         //     ['X', ' ', 'O'],
//         //     [' ', ' ', 'X'],
//         //     [' ', 'X', ' '],
//         //     [' ', 'X', 'W'],
//         // ];
//         // char[][] Maze = [ // interesting case where ai picked the longest path due to depth first search algorithm
//         //     [' ', ' ', 'O', ' '],
//         //     [' ', 'X', ' ', ' '],
//         //     [' ', 'X', 'W', ' '],
//         // ];

//         int[] playerPos = new int[2];
//         int[] winPos = new int[2];
//         for (int i = 0; i < Maze.Length; ++i)
//             for (int j = 0; j < Maze[0].Length; ++j)
//                 if (Maze[i][j] == MazeElements.Player)
//                     playerPos = [i, j];
//                 else if (Maze[i][j] == MazeElements.Win)
//                     winPos = [i, j];

//         Node<char>? path = FindPath(Maze, playerPos);
//         Node<char>? currStep = path;
//         while (currStep != null) {
//             Maze[playerPos[0]][playerPos[1]] = 'O';
//             Console.WriteLine("--------------");
//             for (int i = 0; i < Maze.Length; ++i) {
//                 for (int j = 0; j < Maze[0].Length; ++j)
//                     Console.Write($" | {Maze[i][j]}");

//                 Console.Write("\n--------------\n");
//             }

//             // apply movement
//             int[] newPos = MovePlayer(playerPos, currStep.Value);

//             if (!isInBoundary(Maze, newPos)) {
//                 Console.WriteLine("Out of boundary, AI FAILED");
//                 break;
//             }
//             else if (Maze[newPos[0]][newPos[1]] == 'X') {
//                 Console.WriteLine("There is a wall, AI FAILED");
//                 break;
//             }
//             else {
//                 Maze[playerPos[0]][playerPos[1]] = '-';
//                 playerPos = newPos;
//             }
//             currStep = currStep.Next;

//             Console.WriteLine("Press any key to continue");
//             Console.ReadLine();
//         }
//         if (playerPos.SequenceEqual(winPos)) Console.WriteLine("The AI have found an exit, GREAT!");
//         else Console.WriteLine("NO VALID PATH EXISTS (or the ai is a dummy)");
//     }

// }