using System.Collections.Generic;
using UnityEngine;

namespace MinMax.Scripts {
    public class Men : Piece {
        
        public Men(PlayerColor owner, Coordinate currentCoordinate) : base(owner, currentCoordinate) { }

        public override List<Coordinate> AvailableMoves(Board board) {
            List<Coordinate> availableMoves = new List<Coordinate>();
            if (Owner == PlayerColor.White) {
                // Base moves
                if (board.ValidCoordinate(CurrentCoordinate.TopRight) && board.GetPiece(CurrentCoordinate.TopRight) == null)
                    availableMoves.Add(CurrentCoordinate.TopRight);
                if (board.ValidCoordinate(CurrentCoordinate.TopLeft) && board.GetPiece(CurrentCoordinate.TopLeft) == null)
                    availableMoves.Add(CurrentCoordinate.TopLeft);
            }
            if (Owner == PlayerColor.Black) {
                // Base moves
                if (board.ValidCoordinate(CurrentCoordinate.BottomRight) && board.GetPiece(CurrentCoordinate.BottomRight) == null)
                    availableMoves.Add(CurrentCoordinate.BottomRight);
                if (board.ValidCoordinate(CurrentCoordinate.BottomLeft) && board.GetPiece(CurrentCoordinate.BottomLeft) == null)
                    availableMoves.Add(CurrentCoordinate.BottomLeft);
            }
            // Eat moves
            if (board.ValidCoordinate(CurrentCoordinate.TopRight) && board.ValidCoordinate(CurrentCoordinate.TopRightJump) && 
                board.GetPiece(CurrentCoordinate.TopRight) != null && board.GetPiece(CurrentCoordinate.TopRight).Owner != Owner &&
                board.GetPiece(CurrentCoordinate.TopRightJump) == null) 
                availableMoves.Add(CurrentCoordinate.TopRightJump); 
            if (board.ValidCoordinate(CurrentCoordinate.BottomRight) && board.ValidCoordinate(CurrentCoordinate.BottomRightJump) && 
                board.GetPiece(CurrentCoordinate.BottomRight) != null && board.GetPiece(CurrentCoordinate.BottomRight).Owner != Owner &&
                board.GetPiece(CurrentCoordinate.BottomRightJump) == null) 
                availableMoves.Add(CurrentCoordinate.BottomRightJump); 
            if (board.ValidCoordinate(CurrentCoordinate.BottomLeft) && board.ValidCoordinate(CurrentCoordinate.BottomLeftJump) && 
                board.GetPiece(CurrentCoordinate.BottomLeft) != null && board.GetPiece(CurrentCoordinate.BottomLeft).Owner != Owner &&
                board.GetPiece(CurrentCoordinate.BottomLeftJump) == null) 
                availableMoves.Add(CurrentCoordinate.BottomLeftJump); 
            if (board.ValidCoordinate(CurrentCoordinate.TopLeft) && board.ValidCoordinate(CurrentCoordinate.TopLeftJump) && 
                board.GetPiece(CurrentCoordinate.TopLeft) != null && board.GetPiece(CurrentCoordinate.TopLeft).Owner != Owner &&
                board.GetPiece(CurrentCoordinate.TopLeftJump) == null) 
                availableMoves.Add(CurrentCoordinate.TopLeftJump); 
            return availableMoves;
        }

        public override int EvaluateMove(Board board, Coordinate destination) {
            // Normal Eat move
            int xDistance = CurrentCoordinate.X - destination.X;
            int yDistance = CurrentCoordinate.Y - destination.Y;
            if (Mathf.Abs(xDistance) == 2 && Mathf.Abs(yDistance) == 2 && 
                board.Matrix[CurrentCoordinate.X + Mathf.Abs(xDistance) / 2, CurrentCoordinate.Y + Mathf.Abs(yDistance) / 2] != null) { 
                return 1;
            }
            return 0;
        }

        public override void ExecuteMove(Board board, Coordinate destination) {
            // Move to position
            board.Matrix[destination.X, destination.Y] = board.Matrix[CurrentCoordinate.X, CurrentCoordinate.Y];
            board.Matrix[CurrentCoordinate.X, CurrentCoordinate.Y] = null;
            CurrentCoordinate = destination;
        }

        public override object Clone() {
            return new Men(Owner, CurrentCoordinate);
        }
        
    }
}