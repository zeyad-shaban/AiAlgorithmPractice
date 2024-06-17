class Game {
    public static bool DropAtPos(char[][] board, int col, int turn) {
        int i = 0;
        for (; i < board.Length && board[i][col] != Elems.player && board[i][col] != Elems.bot; ++i) ;

        if (i - 1 >= 0) board[i - 1][col] = (turn == -1 ? Elems.bot : Elems.player);

        return i - 1 >= 0;
    }

    public static int CheckWinner(char[][] board) {
        // check row
        for (int i = 0; i < board.Length; ++i) {
            char dom = board[i][0];
            if (dom != Elems.bot && dom != Elems.player) continue;

            int j = 1;
            for (; j < board[0].Length && board[i][j] == dom; ++j) ;
            if (j == board[0].Length) return dom == Elems.player ? -10 : 10;
        }

        // check column
        for (int col = 0; col < board[0].Length; ++col) {
            char dom = board[0][col];
            if (dom != Elems.bot && dom != Elems.player) continue;

            int row = 1;
            for (; row < board.Length && board[row][col] == dom; ++row) ;
            if (row == board.Length) return dom == Elems.player ? -10 : 10;
        }

        // diag check top left down right
        char domDiag = board[0][0];
        if (domDiag != Elems.player && domDiag != Elems.bot) return 0;
        int iDiag = 1;
        for (; iDiag < board.Length && board[iDiag][iDiag] == domDiag; ++iDiag) ;
        if (iDiag == board.Length) return domDiag == Elems.player ? -10 : 10;

        // diag check top right down left
        iDiag = board.Length - 1;
        domDiag = board[iDiag][board.Length - 1 - iDiag];
        for (; iDiag >= 0 && board[iDiag][board.Length - 1 - iDiag] == domDiag; --iDiag) ;
        if (iDiag == -1) return domDiag == Elems.player ? -10 : 10;

        return 0;
    }
}