using System.Linq;
using Chess;

public static class ChessRules {
    
    public static bool CheckMate(Board board, PlayerColor player, PlayerColor opponent) {
        return Check(board, player, opponent) && Draw(board, player);
    }

    public static bool Check(Board board, PlayerColor player, PlayerColor opponent) {
        King king = board.GetPieces<King>(player).First();
        foreach (Piece piece in board.GetPieces(opponent)) {
            foreach (Coordinate coordinate in piece.BaseMoves(board)) {
                if (king.CurrentCoordinate == coordinate) return true;
            }
        }
        return false;
    }

    public static bool Draw(Board board, PlayerColor player) {
        return board.GetPieces(player).Sum(piece => piece.BaseMoves(board).Count()) == 0;
    }
    
}