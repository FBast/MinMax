using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MinMax.Scripts {
    public class AI {

        public Board Board;
        public PlayerColor PlayerColor;
        public int Depth = 2;
        
        private Dictionary<Node, int> _minMaxResults = new Dictionary<Node, int>();

        public AI(Board board, PlayerColor playerColor) {
            Board = board;
            PlayerColor = playerColor;
        }

        public void Think() {
            _minMaxResults.Clear();
            foreach (Piece piece in Board.AvailablePieces(PlayerColor)) {
                foreach (Coordinate availableMove in piece.AvailableMoves(Board)) {
                    Debug.Log("NEW EVALUATION------------");
                    Node node = new Node(Board, PlayerColor, piece.CurrentCoordinate, availableMove);
                    int result = MinMax(node, Depth, true);
                    _minMaxResults.Add(node, result);
                    Debug.Log("END OF EVALUATION------------");
                }
            }
        }

        public void Act() {
            if (_minMaxResults.Count == 0) throw new Exception("MinMax results is empty");
            KeyValuePair<Node,int> first = _minMaxResults.OrderByDescending(pair => pair.Value).FirstOrDefault();
            Board.GetPiece(first.Key.MoveOrigin).ExecuteMove(Board, first.Key.MoveDestination);
        }
        
        private int MinMax(Node node, int depth, bool isMax) {
            Debug.Log("Actual depth : " + depth);
            if (depth == 0 && node.IsTerminal) {
                Debug.Log("END OF BRANCH------");
                return node.HeuristicValue;
            }
            int value;
            if (isMax) {
                value = int.MinValue;
                foreach (Node child in node.Childs) {
                    value = Mathf.Max(value, MinMax(child, depth - 1, false));
                }
            }
            else {
                value = int.MaxValue;
                foreach (Node child in node.Childs) {
                    value = Mathf.Min(value, MinMax(child, depth - 1, true));
                }
            }
            return value;
        }

    }
}