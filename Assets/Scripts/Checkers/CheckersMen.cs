using System;
using System.Collections.Generic;
using UnityEngine;

namespace Checkers {
    public class CheckersMen : Piece {

        public CheckersMen(Coordinate currentCoordinate, PlayerColor player) : base(currentCoordinate, player) { }
        
        public override int Value => 1;
        
        public override List<Coordinate> AvailableMoves(Board board) {
            List<Coordinate> availableMoves = new List<Coordinate>();
            if (Player == PlayerColor.White) {
                // Base moves
                if (board.ValidCoordinate(CurrentCoordinate.ToTopRight) && board.GetPiece(CurrentCoordinate.ToTopRight) == null)
                    availableMoves.Add(CurrentCoordinate.ToTopRight);
                if (board.ValidCoordinate(CurrentCoordinate.ToTopLeft) && board.GetPiece(CurrentCoordinate.ToTopLeft) == null)
                    availableMoves.Add(CurrentCoordinate.ToTopLeft);
            }
            if (Player == PlayerColor.Black) {
                // Base moves
                if (board.ValidCoordinate(CurrentCoordinate.ToBottomRight) && board.GetPiece(CurrentCoordinate.ToBottomRight) == null)
                    availableMoves.Add(CurrentCoordinate.ToBottomRight);
                if (board.ValidCoordinate(CurrentCoordinate.ToBottomLeft) && board.GetPiece(CurrentCoordinate.ToBottomLeft) == null)
                    availableMoves.Add(CurrentCoordinate.ToBottomLeft);
            }
            // Eat moves
            if (board.ValidCoordinate(CurrentCoordinate.ToTopRight) && board.ValidCoordinate(CurrentCoordinate.ToTopRightJump) && 
                board.GetPiece(CurrentCoordinate.ToTopRight) != null && board.GetPiece(CurrentCoordinate.ToTopRight).Player != Player &&
                board.GetPiece(CurrentCoordinate.ToTopRightJump) == null) 
                availableMoves.Add(CurrentCoordinate.ToTopRightJump); 
            if (board.ValidCoordinate(CurrentCoordinate.ToBottomRight) && board.ValidCoordinate(CurrentCoordinate.ToBottomRightJump) && 
                board.GetPiece(CurrentCoordinate.ToBottomRight) != null && board.GetPiece(CurrentCoordinate.ToBottomRight).Player != Player &&
                board.GetPiece(CurrentCoordinate.ToBottomRightJump) == null) 
                availableMoves.Add(CurrentCoordinate.ToBottomRightJump); 
            if (board.ValidCoordinate(CurrentCoordinate.ToBottomLeft) && board.ValidCoordinate(CurrentCoordinate.ToBottomLeftJump) && 
                board.GetPiece(CurrentCoordinate.ToBottomLeft) != null && board.GetPiece(CurrentCoordinate.ToBottomLeft).Player != Player &&
                board.GetPiece(CurrentCoordinate.ToBottomLeftJump) == null) 
                availableMoves.Add(CurrentCoordinate.ToBottomLeftJump); 
            if (board.ValidCoordinate(CurrentCoordinate.ToTopLeft) && board.ValidCoordinate(CurrentCoordinate.ToTopLeftJump) && 
                board.GetPiece(CurrentCoordinate.ToTopLeft) != null && board.GetPiece(CurrentCoordinate.ToTopLeft).Player != Player &&
                board.GetPiece(CurrentCoordinate.ToTopLeftJump) == null) 
                availableMoves.Add(CurrentCoordinate.ToTopLeftJump); 
            return availableMoves;
        }

        public override int EvaluateMove(Board board, Coordinate destination) {
            // Normal Eat move
            int xDistance = CurrentCoordinate.X - destination.X;
            int yDistance = CurrentCoordinate.Y - destination.Y;
            if (Mathf.Abs(xDistance) == 2 && Mathf.Abs(yDistance) == 2) {
                return board.GetPiece(CurrentCoordinate + new Coordinate(xDistance, yDistance)).Value;
            }
            return 0;
        }

        public override void ExecuteMove(Board board, Coordinate destination) {
            // Move to position
            board.Matrix[destination.X, destination.Y] = board.Matrix[CurrentCoordinate.X, CurrentCoordinate.Y];
            board.Matrix[CurrentCoordinate.X, CurrentCoordinate.Y] = null;
            // Remove all checkers between the 2 positions
            int xSign = Math.Sign(destination.X - CurrentCoordinate.X);
            int ySign = Math.Sign(destination.Y - CurrentCoordinate.Y);
            for (int i = CurrentCoordinate.X; i != destination.X; i += xSign) {
                for (int j = CurrentCoordinate.Y; j != destination.Y; j += ySign) {
                    board.Matrix[i, j] = null;
                }
            }
            CurrentCoordinate = destination;
        }

        public override object Clone() {
            return new CheckersMen(CurrentCoordinate, Player);
        }


    }
}