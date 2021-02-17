using System;
using System.Collections.Generic;
using System.Linq;
using Checkers;
using Chess;
using JetBrains.Annotations;

public class Board : ICloneable {

    [ItemCanBeNull] public Piece[,] Matrix = new Piece[8, 8];

    public bool OccupiedCoordinate(Coordinate coordinate, PlayerColor? playerColor = null) {
        if (playerColor == null) return Matrix[coordinate.X, coordinate.Y] != null;
        return Matrix[coordinate.X, coordinate.Y] != null &&
               Matrix[coordinate.X, coordinate.Y].Player == playerColor;
    }
    
    public bool ValidCoordinate(Coordinate coordinate) {
        return coordinate.X >= 0 && coordinate.X < Matrix.GetLength(0) && 
               coordinate.Y >= 0 && coordinate.Y < Matrix.GetLength(1);
    }
        
    public Piece GetPiece(Coordinate coordinate) {
        return Matrix[coordinate.X, coordinate.Y];
    }
    
    public void SetupCheckersBoard() {
        Matrix = new Piece[,] {
            {new CheckersMen(PlayerColor.White, new Coordinate(0, 0)), null, new CheckersMen(PlayerColor.White, new Coordinate(0, 2)), null, new CheckersMen(PlayerColor.White, new Coordinate(0, 4)), null, new CheckersMen(PlayerColor.White, new Coordinate(0, 6)), null},
            {null, new CheckersMen(PlayerColor.White, new Coordinate(1, 1)), null, new CheckersMen(PlayerColor.White, new Coordinate(1, 3)), null, new CheckersMen(PlayerColor.White, new Coordinate(1, 5)), null, new CheckersMen(PlayerColor.White, new Coordinate(1, 7))},
            {new CheckersMen(PlayerColor.White, new Coordinate(2, 0)), null, new CheckersMen(PlayerColor.White, new Coordinate(2, 2)), null, new CheckersMen(PlayerColor.White, new Coordinate(2, 4)), null, new CheckersMen(PlayerColor.White, new Coordinate(2, 6)), null},
            {null, null, null, null, null, null, null, null},
            {null, null, null, null, null, null, null, null},
            {null, new CheckersMen(PlayerColor.Black, new Coordinate(5, 1)), null, new CheckersMen(PlayerColor.Black, new Coordinate(5, 3)), null, new CheckersMen(PlayerColor.Black, new Coordinate(5, 5)), null, new CheckersMen(PlayerColor.Black, new Coordinate(5, 7))},
            {new CheckersMen(PlayerColor.Black, new Coordinate(6, 0)), null, new CheckersMen(PlayerColor.Black, new Coordinate(6, 2)), null, new CheckersMen(PlayerColor.Black, new Coordinate(6, 4)), null, new CheckersMen(PlayerColor.Black, new Coordinate(6, 6)), null},
            {null, new CheckersMen(PlayerColor.Black, new Coordinate(7, 1)), null, new CheckersMen(PlayerColor.Black, new Coordinate(7, 3)), null, new CheckersMen(PlayerColor.Black, new Coordinate(7, 5)), null, new CheckersMen(PlayerColor.Black, new Coordinate(7, 7))}
        };
    }
       
    public void SetupChessBoard() {
        Matrix = new Piece[,] {
            {new ChessRook(PlayerColor.White, new Coordinate(0, 0)), new ChessKnight(PlayerColor.White, new Coordinate(0, 1)), new ChessBishop(PlayerColor.White, new Coordinate(0, 2)), new ChessQueen(PlayerColor.White, new Coordinate(0, 3)), new ChessKing(PlayerColor.White, new Coordinate(0, 4)), new ChessBishop(PlayerColor.White, new Coordinate(0, 5)), new ChessKnight(PlayerColor.White, new Coordinate(0, 6)), new ChessRook(PlayerColor.White, new Coordinate(0, 7))},
            {new ChessPawn(PlayerColor.White, new Coordinate(1, 0)), new ChessPawn(PlayerColor.White, new Coordinate(1, 1)), new ChessPawn(PlayerColor.White, new Coordinate(1, 2)), new ChessPawn(PlayerColor.White, new Coordinate(1, 3)), new ChessPawn(PlayerColor.White, new Coordinate(1, 4)), new ChessPawn(PlayerColor.White, new Coordinate(1, 5)), new ChessPawn(PlayerColor.White, new Coordinate(1, 6)), new ChessPawn(PlayerColor.White, new Coordinate(1, 7))},
            {null, null, null, null, null, null, null, null},
            {null, null, null, null, null, null, null, null},
            {null, null, null, null, null, null, null, null},
            {null, null, null, null, null, null, null, null},
            {new ChessPawn(PlayerColor.Black, new Coordinate(6, 0)), new ChessPawn(PlayerColor.Black, new Coordinate(6, 1)), new ChessPawn(PlayerColor.Black, new Coordinate(6, 2)), new ChessPawn(PlayerColor.Black, new Coordinate(6, 3)), new ChessPawn(PlayerColor.Black, new Coordinate(6, 4)), new ChessPawn(PlayerColor.Black, new Coordinate(6, 5)), new ChessPawn(PlayerColor.Black, new Coordinate(6, 6)), new ChessPawn(PlayerColor.Black, new Coordinate(6, 7))},
            {new ChessRook(PlayerColor.Black, new Coordinate(7, 0)), new ChessKnight(PlayerColor.Black, new Coordinate(7, 1)), new ChessBishop(PlayerColor.Black, new Coordinate(7, 2)), new ChessQueen(PlayerColor.Black, new Coordinate(7, 3)), new ChessKing(PlayerColor.Black, new Coordinate(7, 4)), new ChessBishop(PlayerColor.Black, new Coordinate(7, 5)), new ChessKnight(PlayerColor.Black, new Coordinate(7, 6)), new ChessRook(PlayerColor.Black, new Coordinate(7, 7))}
        };
    }
    
    public IEnumerable<Piece> AvailablePieces(PlayerColor playerColor) {
        return 
            from Piece piece in Matrix 
            where piece != null && piece.Player == playerColor 
            select piece;
    }
        
    public object Clone() {
        Board board = new Board();
        for (int i = 0; i < Matrix.GetLength(0); i++) {
            for (int j = 0; j < Matrix.GetLength(1); j++) {
                if (Matrix[i, j] != null) board.Matrix[i, j] = (Piece) Matrix[i, j].Clone();
            }
        }
        return board;
    }
        
}