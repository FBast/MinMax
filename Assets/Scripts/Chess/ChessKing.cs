﻿using System.Collections.Generic;

namespace Chess {
    public class ChessKing : Piece {
        
        public ChessKing(PlayerColor player, Coordinate currentCoordinate) : base(player, currentCoordinate) { }

        public override int Value => 10;

        public override List<Coordinate> AvailableMoves(Board board) {
            List<Coordinate> availableMoves = new List<Coordinate>();
            // Moves to the right
            if (board.ValidCoordinate(CurrentCoordinate.ToRight) && !board.OccupiedCoordinate(CurrentCoordinate.ToRight, Player)) 
                availableMoves.Add(CurrentCoordinate.ToRight);
            // Moves to the left
            if (board.ValidCoordinate(CurrentCoordinate.ToLeft) && !board.OccupiedCoordinate(CurrentCoordinate.ToLeft, Player)) 
                availableMoves.Add(CurrentCoordinate.ToLeft);
            // Moves to the top
            if (board.ValidCoordinate(CurrentCoordinate.ToTop) && !board.OccupiedCoordinate(CurrentCoordinate.ToTop, Player)) 
                availableMoves.Add(CurrentCoordinate.ToTop);
            // Moves to the bottom
            if (board.ValidCoordinate(CurrentCoordinate.ToBottom) && !board.OccupiedCoordinate(CurrentCoordinate.ToBottom, Player)) 
                availableMoves.Add(CurrentCoordinate.ToBottom);
            // Moves to the topRight
            if (board.ValidCoordinate(CurrentCoordinate.ToTopRight) && !board.OccupiedCoordinate(CurrentCoordinate.ToTopRight, Player)) 
                availableMoves.Add(CurrentCoordinate.ToTopRight);
            // Moves to the bottomRight
            if (board.ValidCoordinate(CurrentCoordinate.ToBottomRight) && !board.OccupiedCoordinate(CurrentCoordinate.ToBottomRight, Player)) 
                availableMoves.Add(CurrentCoordinate.ToBottomRight);
            // Moves to the bottomLeft
            if (board.ValidCoordinate(CurrentCoordinate.ToBottomLeft) && !board.OccupiedCoordinate(CurrentCoordinate.ToBottomLeft, Player)) 
                availableMoves.Add(CurrentCoordinate.ToBottomLeft);
            // Moves to the topLeft
            if (board.ValidCoordinate(CurrentCoordinate.ToTopLeft) && !board.OccupiedCoordinate(CurrentCoordinate.ToTopLeft, Player)) 
                availableMoves.Add(CurrentCoordinate.ToTopLeft);
            return availableMoves;
        }

        public override int EvaluateMove(Board board, Coordinate destination) {
            return board.GetPiece(destination)?.Value ?? 0;
        }

        public override void ExecuteMove(Board board, Coordinate destination) {
            // Move to position
            board.Matrix[destination.X, destination.Y] = board.Matrix[CurrentCoordinate.X, CurrentCoordinate.Y];
            board.Matrix[CurrentCoordinate.X, CurrentCoordinate.Y] = null;
            CurrentCoordinate = destination;
        }

        public override object Clone() {
            return new ChessKing(Player, CurrentCoordinate);
        }
        
    }
}