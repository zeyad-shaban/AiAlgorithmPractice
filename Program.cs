using System.Diagnostics;

class Program {
    public static void ReverseMove(int col, char[][] board) {
        for (int j = 0; j < board.Length; ++j) {
            if (board[j][col] == Elems.player || board[j][col] == Elems.bot) {
                board[j][col] = ' ';
                break;
            }
        }
    }
    public static (int, int) MinMaxSearch(int turn, int depth, char[][] board, int alpha = int.MinValue, int beta = int.MaxValue) {// returns value, move
        int winner = Game.CheckWinner(board);
        if (winner != 0) return (winner, -1);
        if (depth <= 0) return (0, -1);

        int bestScore = int.MaxValue * turn;
        int bestMove = -1;

        for (int i = 0; i < board.Length; ++i) {
            bool valid = Game.DropAtPos(board, i, turn);
            if (!valid) continue;

            (int score, int _) = MinMaxSearch(turn * -1, depth - 1, board, alpha, beta);
            ReverseMove(i, board);

            if (turn == -1) {
                // max
                alpha = Math.Max(alpha, score);
                if (score > bestScore) (bestScore, bestMove) = (score, i);
                if (beta <= alpha) break;
            }
            else {
                // min
                beta = Math.Min(beta, score);
                if (score < bestScore) (bestScore, bestMove) = (score, i);
                if (beta <= alpha) break;
            }
        }
        return (bestMove == -1 ? 0 : bestScore, bestMove);
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
            int move;

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
                (int _, move) = MinMaxSearch(turn, 12, board);
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
/*
HUMANS DEFEATED LIST:
    TeamTrojj
    Dad
    The Creator
    ???
*/