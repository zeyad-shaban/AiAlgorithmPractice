class Program {
    static char BOT = 'X';
    static char PLR = 'O';
    static Dictionary<int, (int, int)> moveMap = new() {
        {0, (0,0)},
        {1, (0,1)},
        {2, (0,2)},
        {3, (1,0)},
        {4, (1,1)},
        {5, (1,2)},
        {6, (2,0)},
        {7, (2,1)},
        {8, (2,2)},
    };


    static bool PlayMove(int pos, char[][] board, char turn) {
        if (!moveMap.ContainsKey(pos)) return false;
        (int, int) posIdx = moveMap[pos];
        if (board[posIdx.Item1][posIdx.Item2] != ' ') return false;

        board[posIdx.Item1][posIdx.Item2] = turn;
        return true;
    }

    static int CheckWinner(char[][] board) {
        // Check rows
        for (int i = 0; i < 3; i++) {
            if (board[i][0] != ' ' && board[i][0] == board[i][1] && board[i][1] == board[i][2]) {
                return board[i][0] == BOT ? 1 : -1;
            }
        }

        // Check columns
        for (int j = 0; j < 3; j++) {
            if (board[0][j] != ' ' && board[0][j] == board[1][j] && board[1][j] == board[2][j]) {
                return board[0][j] == BOT ? 1 : -1;
            }
        }

        // Check diagonals
        if (board[0][0] != ' ' && board[0][0] == board[1][1] && board[1][1] == board[2][2]) {
            return board[0][0] == BOT ? 1 : -1;
        }

        if (board[0][2] != ' ' && board[0][2] == board[1][1] && board[1][1] == board[2][0]) {
            return board[0][2] == BOT ? 1 : -1;
        }

        // No winner
        return 0;
    }


    static void DrawBoard(char[][] board) {
        for (int i = 0; i < 3; i++) {
            for (int j = 0; j < 3; j++) {
                Console.Write(board[i][j]);
                if (j < 2) Console.Write("|");
            }
            Console.WriteLine();
            if (i < 2) Console.WriteLine("-----");
        }
    }

    static bool IsFull(char[][] board) {
        for (int i = 0; i < board.Length; ++i)
            for (int j = 0; j < board[0].Length; ++j)
                if (board[i][j] == ' ') return false;

        return true;
    }

    static (int, int) MinMaxSearch(char[][] board, char turn, int depth, int alpha = int.MinValue, int beta = int.MaxValue) {
        int winner = CheckWinner(board);
        if (winner != 0 || depth <= 0) return (winner, -1);

        int bestScore = turn == BOT ? int.MinValue : int.MaxValue;
        int bestMove = -1;

        for (int i = 0; i < 9; ++i) {
            if (!PlayMove(i, board, turn)) continue;

            (int score, int _) = MinMaxSearch(board, turn == PLR ? BOT : PLR, depth - 1, alpha, beta);
            board[moveMap[i].Item1][moveMap[i].Item2] = ' ';
            if (turn == BOT) {
                alpha = Math.Max(alpha, score);
                if (beta <= alpha) break;

                if (score > bestScore) {
                    bestScore = score;
                    bestMove = i;
                }
            }
            else {
                beta = Math.Min(beta, score);
                if (beta <= alpha) break;

                if (score < bestScore) {
                    bestScore = score;
                    bestMove = i;
                }
            }
        }

        return (bestMove == -1 ? 0 : bestScore, bestMove);
    }

    static void Main(string[] args) {
        char[][] board = [
            [' ', ' ', ' '],
            [' ', ' ', ' '],
            [' ', ' ', ' '],
        ];

        char turn = PLR;
        int winner = 0;
        DrawBoard(board);
        while (winner == 0) {
            Console.WriteLine("==============================");
            Console.WriteLine("================================");
            Console.WriteLine("==================================");
            Console.WriteLine("====================================");
            Console.WriteLine("======================================");
            int pos;
            if (turn == PLR) {
                Console.WriteLine("Enter move: ");
                pos = int.Parse(Console.ReadLine());
            }
            else {
                (int _, pos) = MinMaxSearch(board, turn, 6);
            }

            bool valid = PlayMove(pos, board, turn);
            if (!valid) {
                Console.WriteLine("Invalid position");
                continue;
            }

            DrawBoard(board);

            winner = CheckWinner(board);
            if (IsFull(board)) break;
            turn = turn == PLR ? BOT : PLR;
        }

        Console.WriteLine(winner);
    }
}