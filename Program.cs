using System.Diagnostics;

class Program {
    public static (int, int) MinMaxSearch(int turn, int depth, char[][] board) {// returns value, move
        int winner = Game.CheckWinner(board);
        if (winner != 0) return (winner, -1);
        if (depth <= 0) return (0, -1);

        int bestVal = int.MaxValue * turn;
        int bestMove = -1;

        for (int i = 0; i < board.Length; ++i) {
            bool valid = Game.DropAtPos(board, i, turn);
            if (!valid) continue;

            (int val, int _) = MinMaxSearch(turn * -1, depth - 1, board);
            for (int j = 0; j < board.Length; ++j) {
                if (board[j][i] == Elems.player || board[j][i] == Elems.bot) {
                    board[j][i] = ' ';
                    break;
                }
            }

            if ((turn == -1 && val > bestVal) || (turn == 1 && val < bestVal)) {
                bestVal = val;
                bestMove = i;
            }
        }
        return (bestMove == -1 ? 0 : bestVal, bestMove);
    }

    static char[][] board = [
        [' ', ' ', ' ', ' ', ' ', ' '],
        [' ', ' ', ' ', ' ', ' ', ' '],
        [' ', ' ', ' ', ' ', ' ', ' '],
        [' ', ' ', ' ', ' ', ' ', ' '],
        [' ', ' ', ' ', ' ', ' ', ' '],
        [' ', ' ', ' ', ' ', ' ', ' '],
    ];

    static void Main(string[] args) {

        int winner = 0;
        int turn = 1; // -1 for bot, 1 for player
        while (winner == 0) {
            Console.Write("\n================================\n");
            Console.Write("\n=================================\n");
            Console.Write("\n==================================\n");
            int move = -1;
            if (turn == 1) {
                Console.Write("row: ");
                if (!int.TryParse(Console.ReadLine(), out move) || move - 1 >= board[0].Length) {
                    Console.WriteLine("Please enter a valid number");
                    continue;
                }
                --move;
            }
            else {
                var watch = Stopwatch.StartNew();
                (int _, move) = MinMaxSearch(turn, 10, board);
                watch.Stop();
                Console.WriteLine($"Best move found in {watch.ElapsedMilliseconds / 1000}secs");
            }

            if (move < 0 || move >= board.Length) break;

            bool didPlay = Game.DropAtPos(board, move, turn);
            if (!didPlay) {
                Console.WriteLine("Invalid position");
                continue;
            }

            for (int i = 0; i < board.Length; ++i) {
                for (int j = 0; j < board[0].Length; ++j) {
                    Console.Write($" | {board[i][j]}");
                }
                Console.Write("\n---------------\n");
            }

            winner = Game.CheckWinner(board);
            turn *= -1;
        }

        Console.WriteLine(winner);
    }
}
