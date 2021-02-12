using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace MinMax.Scripts {
    public class Board : ICloneable {

        [ItemCanBeNull] public Piece[,] Matrix = new Piece[8, 8];

        public bool ValidCoordinate(Coordinate coordinate) {
            return coordinate.X > 0 && coordinate.X < Matrix.GetLength(0) && 
                   coordinate.Y > 0 && coordinate.Y < Matrix.GetLength(1);
        }
        
        public Piece GetPiece(Coordinate coordinate) {
            return Matrix[coordinate.X, coordinate.Y];
        }
        
        public void SetupBoard() {
            Matrix = new Piece[,] {
                {new Men(PlayerColor.White, new Coordinate(0, 0)), null, new Men(PlayerColor.White, new Coordinate(0, 2)), null, new Men(PlayerColor.White, new Coordinate(0, 4)), null, new Men(PlayerColor.White, new Coordinate(0, 6)), null},
                {null, new Men(PlayerColor.White, new Coordinate(1, 1)), null, new Men(PlayerColor.White, new Coordinate(1, 3)), null, new Men(PlayerColor.White, new Coordinate(1, 5)), null, new Men(PlayerColor.White, new Coordinate(1, 7))},
                {new Men(PlayerColor.White, new Coordinate(2, 0)), null, new Men(PlayerColor.White, new Coordinate(2, 2)), null, new Men(PlayerColor.White, new Coordinate(2, 4)), null, new Men(PlayerColor.White, new Coordinate(2, 6)), null},
                {null, null, null, null, null, null, null, null},
                {null, null, null, null, null, null, null, null},
                {null, new Men(PlayerColor.Black, new Coordinate(5, 1)), null, new Men(PlayerColor.Black, new Coordinate(5, 3)), null, new Men(PlayerColor.Black, new Coordinate(5, 5)), null, new Men(PlayerColor.Black, new Coordinate(5, 7))},
                {new Men(PlayerColor.Black, new Coordinate(6, 0)), null, new Men(PlayerColor.Black, new Coordinate(6, 2)), null, new Men(PlayerColor.Black, new Coordinate(6, 4)), null, new Men(PlayerColor.Black, new Coordinate(6, 6)), null},
                {null, new Men(PlayerColor.Black, new Coordinate(7, 1)), null, new Men(PlayerColor.Black, new Coordinate(7, 3)), null, new Men(PlayerColor.Black, new Coordinate(7, 5)), null, new Men(PlayerColor.Black, new Coordinate(7, 7))}
            };
        }
        
        public IEnumerable<Piece> AvailablePieces(PlayerColor playerColor) {
            return 
                from Piece piece in Matrix 
                where piece != null && piece.Owner == playerColor 
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
}