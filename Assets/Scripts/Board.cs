﻿using System;
using System.Collections.Generic;
using System.Linq;
using Checkers;
using Chess;
using JetBrains.Annotations;

public class Board : ICloneable {

    [ItemCanBeNull] public Piece[,] Matrix = new Piece[8, 8];

    public bool OccupiedCoordinate(Coordinate coordinate, PlayerColor? playerColor = null) {
        if (playerColor == null) return Matrix[coordinate.Row, coordinate.Column] != null;
        return Matrix[coordinate.Row, coordinate.Column] != null &&
               Matrix[coordinate.Row, coordinate.Column].Player == playerColor;
    }
    
    public bool ValidCoordinate(Coordinate coordinate) {
        return coordinate.Row >= 0 && coordinate.Row < Matrix.GetLength(0) && 
               coordinate.Column >= 0 && coordinate.Column < Matrix.GetLength(1);
    }
        
    public Piece GetPiece(Coordinate coordinate) {
        return Matrix[coordinate.Row, coordinate.Column];
    }

    public void ConvertHandyMatrix(Pieces[,] handyMatrix) {
        Matrix = new Piece[handyMatrix.GetLength(0), handyMatrix.GetLength(1)];
        for (int i = 0; i < handyMatrix.GetLength(0); i++) {
            for (int j = 0; j < handyMatrix.GetLength(1); j++) {
                switch (handyMatrix[j, i]) {
                    case Pieces.None:
                        break;
                    case Pieces.WhiteChessPawn:
                        Matrix[i, j] = new ChessPawn(new Coordinate(i, j), PlayerColor.White);
                        break;
                    case Pieces.BlackChessPawn:
                        Matrix[i, j] = new ChessPawn(new Coordinate(i, j), PlayerColor.Black);
                        break;
                    case Pieces.WhiteChessRook:
                        Matrix[i, j] = new ChessRook(new Coordinate(i, j), PlayerColor.White);
                        break;
                    case Pieces.BlackChessRook:
                        Matrix[i, j] = new ChessRook(new Coordinate(i, j), PlayerColor.Black);
                        break;
                    case Pieces.WhiteChessKnight:
                        Matrix[i, j] = new ChessKnight(new Coordinate(i, j), PlayerColor.White);
                        break;
                    case Pieces.BlackChessKnight:
                        Matrix[i, j] = new ChessKnight(new Coordinate(i, j), PlayerColor.Black);
                        break;
                    case Pieces.WhiteChessBishop:
                        Matrix[i, j] = new ChessBishop(new Coordinate(i, j), PlayerColor.White);
                        break;
                    case Pieces.BlackChessBishop:
                        Matrix[i, j] = new ChessBishop(new Coordinate(i, j), PlayerColor.Black);
                        break;
                    case Pieces.WhiteChessQueen:
                        Matrix[i, j] = new ChessQueen(new Coordinate(i, j), PlayerColor.White);
                        break;
                    case Pieces.BlackChessQueen:
                        Matrix[i, j] = new ChessQueen(new Coordinate(i, j), PlayerColor.Black);
                        break;
                    case Pieces.WhiteChessKing:
                        Matrix[i, j] = new ChessKing(new Coordinate(i, j), PlayerColor.White);
                        break;
                    case Pieces.BlackChessKing:
                        Matrix[i, j] = new ChessKing(new Coordinate(i, j), PlayerColor.Black);
                        break;
                    case Pieces.WhiteCheckersMen:
                        Matrix[i, j] = new CheckersMen(new Coordinate(i, j), PlayerColor.White);
                        break;
                    case Pieces.BlackCheckersMen:
                        Matrix[i, j] = new CheckersMen(new Coordinate(i, j), PlayerColor.Black);
                        break;
                    case Pieces.WhiteCheckersKing:
                        Matrix[i, j] = new CheckersKing(new Coordinate(i, j), PlayerColor.White);
                        break;
                    case Pieces.BlackCheckersKing:
                        Matrix[i, j] = new CheckersKing(new Coordinate(i, j), PlayerColor.Black);
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