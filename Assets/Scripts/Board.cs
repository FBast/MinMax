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

    public void ConvertHandyMatrix(Pieces[,] handyMatrix) {
        Matrix = new Piece[handyMatrix.GetLength(0), handyMatrix.GetLength(1)];
        for (int i = 0; i < handyMatrix.GetLength(0); i++) {
            for (int j = 0; j < handyMatrix.GetLength(1); j++) {
                switch (handyMatrix[i,j]) {
                    case Pieces.None:
                        break;
                    case Pieces.WhiteChessPawn:
                        Matrix[j,i] = new ChessPawn(new Coordinate(j, i), PlayerColor.White);
                        break;
                    case Pieces.BlackChessPawn:
                        Matrix[j,i] = new ChessPawn(new Coordinate(j, i), PlayerColor.Black);
                        break;
                    case Pieces.WhiteChessRook:
                        Matrix[j,i] = new ChessRook(new Coordinate(j, i), PlayerColor.White);
                        break;
                    case Pieces.BlackChessRook:
                        Matrix[j,i] = new ChessRook(new Coordinate(j, i), PlayerColor.Black);
                        break;
                    case Pieces.WhiteChessKnight:
                        Matrix[j,i] = new ChessKnight(new Coordinate(j, i), PlayerColor.White);
                        break;
                    case Pieces.BlackChessKnight:
                        Matrix[j,i] = new ChessKnight(new Coordinate(j, i), PlayerColor.Black);
                        break;
                    case Pieces.WhiteChessBishop:
                        Matrix[j,i] = new ChessBishop(new Coordinate(j, i), PlayerColor.White);
                        break;
                    case Pieces.BlackChessBishop:
                        Matrix[j,i] = new ChessBishop(new Coordinate(j, i), PlayerColor.Black);
                        break;
                    case Pieces.WhiteChessQueen:
                        Matrix[j,i] = new ChessQueen(new Coordinate(j, i), PlayerColor.White);
                        break;
                    case Pieces.BlackChessQueen:
                        Matrix[j,i] = new ChessQueen(new Coordinate(j, i), PlayerColor.Black);
                        break;
                    case Pieces.WhiteChessKing:
                        Matrix[j,i] = new ChessKing(new Coordinate(j, i), PlayerColor.White);
                        break;
                    case Pieces.BlackChessKing:
                        Matrix[j,i] = new ChessKing(new Coordinate(j, i), PlayerColor.Black);
                        break;
                    case Pieces.WhiteCheckersMen:
                        Matrix[j,i] = new CheckersMen(new Coordinate(j, i), PlayerColor.White);
                        break;
                    case Pieces.BlackCheckersMen:
                        Matrix[j,i] = new CheckersMen(new Coordinate(j, i), PlayerColor.Black);
                        break;
                    case Pieces.WhiteCheckersKing:
                        Matrix[j,i] = new CheckersKing(new Coordinate(j, i), PlayerColor.White);
                        break;
                    case Pieces.BlackCheckersKing:
                        Matrix[j,i] = new CheckersKing(new Coordinate(j, i), PlayerColor.Black);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
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