class Game {
    public static bool DropAtPos(char[][] board, int col, int turn) {
        int i = 0;
        for (; i < board.Length && board[i][col] != Elems.player && board[i][col] != Elems.bot; ++i) ;

        if (i - 1 >= 0) board[i - 1][col] = (turn == -1 ? Elems.bot : Elems.player);

        return i - 1 >= 0;
    }

    public static int CheckWinner(char[][] board) {
        // Check rows for connected four
        for (int row = 0; row < board.Length; row++) {
            for (int col = 0; col <= board[0].Length - 4; col++) {
                char first = board[row][col];
                if (first == ' ') continue;  // Skip empty cells

                bool isWinningSequence = true;
                for (int k = 1; k < 4; k++) {
                    if (board[row][col + k] != first) {
                        isWinningSequence = false;
                        break;
                    }
                }
                if (isWinningSequence) return (first == Elems.player) ? -10 : 10;
            }
        }

        // Check columns for connected four
        for (int col = 0; col < board[0].Length; col++) {
            for (int row = 0; row <= board.Length - 4; row++) {
                char first = board[row][col];
                if (first == ' ') continue;  // Skip empty cells

                bool isWinningSequence = true;
                for (int k = 1; k < 4; k++) {
                    if (board[row + k][col] != first) {
                        isWinningSequence = false;
                        break;
                    }
                }
                if (isWinningSequence) return (first == Elems.player) ? -10 : 10;
            }
        }

        // Check diagonals (both directions) for connected four
        for (int row = 0; row <= board.Length - 4; row++) {
            for (int col = 0; col <= board[0].Length - 4; col++) {
                // Top-left to bottom-right diagonal
                char first = board[row][col];
                if (first != ' ') {
                    bool isWinningSequence = true;
                    for (int k = 1; k < 4; k++) {
                        if (board[row + k][col + k] != first) {
                            isWinningSequence = false;
                            break;
                        }
                    }
                    if (isWinningSequence) return (first == Elems.player) ? -10 : 10;
                }

                // Top-right to bottom-left diagonal
                first = board[row][col + 3];
                if (first != ' ') {
                    bool isWinningSequence = true;
                    for (int k = 1; k < 4; k++) {
                        if (board[row + k][col + 3 - k] != first) {
                            isWinningSequence = false;
                            break;
                        }
                    }
                    if (isWinningSequence) return (first == Elems.player) ? -10 : 10;
                }
            }
        }

        return 0;  // No winner found
    }
}