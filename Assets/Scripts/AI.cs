using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AI {

    public Board Board;
    public PlayerColor PlayerColor;
    public int Depth = 10;
        
    private Dictionary<Node, int> _ThinkResults = new Dictionary<Node, int>();

    public AI(Board board, PlayerColor playerColor) {
        Board = board;
        PlayerColor = playerColor;
    }

    public void Think() {
        _ThinkResults.Clear();
        foreach (Piece piece in Board.AvailablePieces(PlayerColor)) {
            foreach (Coordinate availableMove in piece.AvailableMoves(Board)) {
                Debug.Log("NEW EVALUATION FOR " + piece.CurrentCoordinate + " TO " + availableMove);
                Node node = new Node(Board, PlayerColor, piece.CurrentCoordinate, availableMove);
//                int result = MinMax(node, Depth, true);
                int result = NegaMax(node, Depth, 1);
                _ThinkResults.Add(node, result);
                Debug.Log("END OF EVALUATION------------");
            }
        }
        foreach (KeyValuePair<Node,int> thinkResult in _ThinkResults) {
            Debug.Log(thinkResult.Key.MoveOrigin + " move to " + thinkResult.Key.MoveDestination + " with heuristic of " + thinkResult.Value);
        }
    }

    public void Act() {
        if (_ThinkResults.Count == 0) throw new Exception("MinMax results is empty");
        KeyValuePair<Node,int> first = _ThinkResults.OrderByDescending(pair => pair.Value).FirstOrDefault();
        Board.GetPiece(first.Key.MoveOrigin).ExecuteMove(Board, first.Key.MoveDestination);
    }
        
    private int MinMax(Node node, int depth, bool isMax) {
        if (depth == 0 || node.IsTerminal) {
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

    private int NegaMax(Node node, int depth, int color) {
        if (depth == 0 || node.IsTerminal)
            return color * node.HeuristicValue;
        int value = int.MinValue;
        foreach (Node child in node.Childs) {
            value = Mathf.Max(value, -NegaMax(child, depth - 1, -color));
        }
        return value;
    }
    
}