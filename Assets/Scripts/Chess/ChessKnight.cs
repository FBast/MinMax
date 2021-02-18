using System.Collections.Generic;

namespace Chess {
    public class ChessKnight : Piece {
        
        public ChessKnight(Coordinate currentCoordinate, PlayerColor player) : base(currentCoordinate, player) { }

        public override int Value => 3;

        public override List<Coordinate> AvailableMoves(Board board) {
            List<Coordinate> possibleMoves = new List<Coordinate> {
                CurrentCoordinate + Coordinate.Right + Coordinate.Top * 2,
                CurrentCoordinate + Coordinate.Right * 2 + Coordinate.Top,
                CurrentCoordinate + Coordinate.Right * 2 + Coordinate.Bottom,
                CurrentCoordinate + Coordinate.Right + Coordinate.Bottom * 2,
                CurrentCoordinate + Coordinate.Left + Coordinate.Bottom * 2,
                CurrentCoordinate + Coordinate.Left * 2 + Coordinate.Bottom,
                CurrentCoordinate + Coordinate.Left * 2 + Coordinate.Top,
                CurrentCoordinate + Coordinate.Left + Coordinate.Top * 2
            };
            List<Coordinate> availableMoves = new List<Coordinate>();
            foreach (Coordinate possibleMove in possibleMoves) {
                if (!board.ValidCoordinate(possibleMove)) continue;
                if (!board.OccupiedCoordinate(possibleMove, Player)) 
                    availableMoves.Add(possibleMove);
            }
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
            return new ChessKnight(CurrentCoordinate, Player);
        }


    }
}