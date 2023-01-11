using System;
using System.Collections.Generic;
using Chess;
using JetBrains.Annotations;

public struct Board : ICloneable {

    public int Row;
    public int Column;
    [ItemCanBeNull] public Piece[,] Matrix;

    public Board(int row, int column) {
        Row = row;
        Column = column;
        Matrix = new Piece[Row, Column];
    }

    public Piece GetPiece(Coordinate coordinate) {
        return Matrix[coordinate.Row, coordinate.Column];
    }

    public IEnumerable<Piece> GetPieces(PlayerColor? playerColor = null) {
        foreach (Piece piece in Matrix) {
            if (piece == null) continue;
            if (playerColor == null) yield return piece;
            if (piece.Player == playerColor) yield return piece;
        }
    }
    
    public IEnumerable<T> GetPieces<T>(PlayerColor? playerColor = null) where T : Piece {
        foreach (Piece piece in Matrix) {
            if (piece is not T castedPiece) continue;
            if (playerColor == null) yield return castedPiece;
            if (castedPiece.Player == playerColor) yield return castedPiece;
        }
    }
    
    public bool OccupiedCoordinate(Coordinate coordinate, PlayerColor? playerColor = null) {
        Piece piece = Matrix[coordinate.Row, coordinate.Column];
        if (playerColor == null) return piece != null;
        return piece is King || piece?.Player == playerColor;
    }
    
    public bool ValidCoordinate(Coordinate coordinate) {
        return coordinate.Row >= 0 && coordinate.Row < Matrix.GetLength(0) && 
               coordinate.Column >= 0 && coordinate.Column < Matrix.GetLength(1);
    }

    public int Evaluate(PlayerColor player, PlayerColor opponent) {
        int value = 0;
        if (ChessRules.CheckMate(this, player, opponent)) {
            value = int.MinValue;
        }
        else {
            foreach (Piece piece in Matrix) {
                if (piece == null) continue;
                value += piece.Value * (player == piece.Player ? 1 : -1);
            }
            //value += ChessRules.Check(this, player, opponent) ? -5 : 0;
            value += ChessRules.Draw(this, player) ? -10 : 0;
        }
        return value;
    }
    
    public void ConvertHandyMatrix(Pieces[,] handyMatrix) {
        Matrix = new Piece[handyMatrix.GetLength(0), handyMatrix.GetLength(1)];
        for (int i = 0; i < handyMatrix.GetLength(0); i++) {
            for (int j = 0; j < handyMatrix.GetLength(1); j++) {
                switch (handyMatrix[j, i]) {
                    case Pieces.None:
                        break;
                    case Pieces.WhiteChessPawn:
                        Matrix[i, j] = new Pawn(new Coordinate(i, j), PlayerColor.White);
                        break;
                    case Pieces.BlackChessPawn:
                        Matrix[i, j] = new Pawn(new Coordinate(i, j), PlayerColor.Black);
                        break;
                    case Pieces.WhiteChessRook:
                        Matrix[i, j] = new Rook(new Coordinate(i, j), PlayerColor.White);
                        break;
                    case Pieces.BlackChessRook:
                        Matrix[i, j] = new Rook(new Coordinate(i, j), PlayerColor.Black);
                        break;
                    case Pieces.WhiteChessKnight:
                        Matrix[i, j] = new Knight(new Coordinate(i, j), PlayerColor.White);
                        break;
                    case Pieces.BlackChessKnight:
                        Matrix[i, j] = new Knight(new Coordinate(i, j), PlayerColor.Black);
                        break;
                    case Pieces.WhiteChessBishop:
                        Matrix[i, j] = new Bishop(new Coordinate(i, j), PlayerColor.White);
                        break;
                    case Pieces.BlackChessBishop:
                        Matrix[i, j] = new Bishop(new Coordinate(i, j), PlayerColor.Black);
                        break;
                    case Pieces.WhiteChessQueen:
                        Matrix[i, j] = new Queen(new Coordinate(i, j), PlayerColor.White);
                        break;
                    case Pieces.BlackChessQueen:
                        Matrix[i, j] = new Queen(new Coordinate(i, j), PlayerColor.Black);
                        break;
                    case Pieces.WhiteChessKing:
                        Matrix[i, j] = new King(new Coordinate(i, j), PlayerColor.White);
                        break;
                    case Pieces.BlackChessKing:
                        Matrix[i, j] = new King(new Coordinate(i, j), PlayerColor.Black);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
    
    public object Clone() {
        Board board = new Board(Row, Column);
        for (int i = 0; i < Matrix.GetLength(0); i++) {
            for (int j = 0; j < Matrix.GetLength(1); j++) {
                board.Matrix[i, j] = (Piece) Matrix[i, j]?.Clone();
            }
        }
        return board;
    }

}