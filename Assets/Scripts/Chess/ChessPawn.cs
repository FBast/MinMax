using System.Collections.Generic;

namespace Chess {
    public class ChessPawn : Piece {
        
        private bool _hasMoved;
        
        public ChessPawn(PlayerColor player, Coordinate currentCoordinate) : base(player, currentCoordinate) { }

        public override int Value => 1;

        public override List<Coordinate> AvailableMoves(Board board) {
            List<Coordinate> availableMoves = new List<Coordinate>();
            if (Player == PlayerColor.White) {
                if (!_hasMoved && board.ValidCoordinate(CurrentCoordinate.ToTopJump) && !board.OccupiedCoordinate(CurrentCoordinate.ToTopJump)) 
                    availableMoves.Add(CurrentCoordinate.ToTopJump);
                if (board.ValidCoordinate(CurrentCoordinate.ToTop) && !board.OccupiedCoordinate(CurrentCoordinate.ToTop)) 
                    availableMoves.Add(CurrentCoordinate.ToTop);
                if (board.ValidCoordinate(CurrentCoordinate.ToTopRight) && board.OccupiedCoordinate(CurrentCoordinate.ToTopRight, OtherPlayer)) 
                    availableMoves.Add(CurrentCoordinate.ToTopRight);
                if (board.ValidCoordinate(CurrentCoordinate.ToTopLeft) && board.OccupiedCoordinate(CurrentCoordinate.ToTopLeft, OtherPlayer)) 
                    availableMoves.Add(CurrentCoordinate.ToTopLeft);
            }
            if (Player == PlayerColor.Black) {
                if (!_hasMoved && board.ValidCoordinate(CurrentCoordinate.ToBottomJump) && !board.OccupiedCoordinate(CurrentCoordinate.ToBottomJump)) 
                    availableMoves.Add(CurrentCoordinate.ToBottomJump);
                if (board.ValidCoordinate(CurrentCoordinate.ToBottom) && !board.OccupiedCoordinate(CurrentCoordinate.ToBottom))
                    availableMoves.Add(CurrentCoordinate.ToBottom);
                if (board.ValidCoordinate(CurrentCoordinate.ToBottomRight) && board.OccupiedCoordinate(CurrentCoordinate.ToBottomRight, OtherPlayer))
                    availableMoves.Add(CurrentCoordinate.ToBottomRight);
                if (board.ValidCoordinate(CurrentCoordinate.ToBottomLeft) && board.OccupiedCoordinate(CurrentCoordinate.ToBottomLeft, OtherPlayer))
                    availableMoves.Add(CurrentCoordinate.ToBottomLeft);
            }
            return availableMoves;
        }

        public override int EvaluateMove(Board board, Coordinate destination) {
            return board.GetPiece(destination)?.Value ?? 0;
        }

        public override void ExecuteMove(Board board, Coordinate destination) {
            _hasMoved = true;
            // Move to position
            board.Matrix[destination.X, destination.Y] = board.Matrix[CurrentCoordinate.X, CurrentCoordinate.Y];
            board.Matrix[CurrentCoordinate.X, CurrentCoordinate.Y] = null;
            CurrentCoordinate = destination;
        }

        public override object Clone() {
            return new ChessPawn(Player, CurrentCoordinate);
        }
        
    }
}