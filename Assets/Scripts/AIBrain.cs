using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class AIBrain {

    public Board Board;
    public PlayerColor Player;
    public int DepthSearch;
    
    private List<Tuple<int, Node>> Nodes = new List<Tuple<int, Node>>();

    public AIBrain(Board board, PlayerColor player, int depthSearch) {
        Board = board;
        Player = player;
        DepthSearch = depthSearch;
    }

    public void Think(Algorithm algorithm) {
        Nodes.Clear();
        float startingTime = Time.realtimeSinceStartup;
        foreach (Piece availablePiece in Board.AvailablePieces(Player)) {
            foreach (Coordinate availableMove in availablePiece.AvailableMoves(Board)) {
                Node node = new Node(Board, Player, Player, availablePiece.CurrentCoordinate, availableMove);
                int value;
                switch (algorithm) {
                    case Algorithm.MinMax:
                        value = MinMax(node, DepthSearch, false);
                        break;
                    case Algorithm.MinMaxAlphaBeta:
                        value = MinMaxAlphaBeta(node, DepthSearch, int.MinValue, int.MaxValue, false);
                        break;
                    case Algorithm.NegaMax:
                        value = NegaMax(node, DepthSearch, -1);
                        break;
                    case Algorithm.NegaMaxAlphaBeta:
                        value = NegaMaxAlphaBeta(node, DepthSearch, int.MinValue, int.MaxValue, -1);
                        break;
                    case Algorithm.NegaMaxAlphaBetaWithTT:
                        value = NegaMaxAlphaBetaTranspositionTables(node, DepthSearch, int.MinValue, int.MaxValue, -1);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(algorithm), algorithm, null);
                }
                Nodes.Add(new Tuple<int, Node>(value, node));
            }
        }
        Debug.Log("Reflexion took about : " + (Time.realtimeSinceStartup - startingTime) + " seconds");
    }
    
    public void Act() {
        if (Nodes.Count == 0) throw new Exception("MinMax results is empty");
        int bestValue = Nodes.Max(node => node.Item1);
        Nodes.RemoveAll(node => node.Item1 < bestValue);
        Tuple<int, Node> selectedTuple = Nodes[Random.Range(0, Nodes.Count)];
        Board.GetPiece(selectedTuple.Item2.MoveOrigin).ExecuteMove(Board, selectedTuple.Item2.MoveDestination);
    }
        
    private int MinMax(Node node, int depth, bool isMax) {
        if (depth == 0 || node.IsTerminal) {
            return node.HeuristicValue;
        }
        int value;
        if (isMax) {
            value = int.MinValue;
            foreach (Node child in node.Children) {
                value = Mathf.Max(value, MinMax(child, depth - 1, false));
            }
        }
        else {
            value = int.MaxValue;
            foreach (Node child in node.Children) {
                value = Mathf.Min(value, MinMax(child, depth - 1, true));
            }
        }
        return value;
    }

    // int result = AlphaBetaMinMax(node, Depth, int.MinValue, int.MaxValue, false);
    private int MinMaxAlphaBeta(Node node, int depth, int alpha, int beta, bool isMax) {
        if (depth == 0 || node.IsTerminal)
            return node.HeuristicValue;
        int value;
        if (isMax) {
            value = int.MinValue;
            foreach (Node child in node.Children) {
                value = Mathf.Max(value, MinMaxAlphaBeta(child, depth - 1, alpha, beta, false));
                if (value >= beta) return value;
                alpha = Mathf.Max(alpha, value);
            }
        }
        else {
            value = int.MaxValue;
            foreach (Node child in node.Children) {
                value = Mathf.Min(value, MinMaxAlphaBeta(child, depth - 1, alpha, beta, true));
                if (alpha >= value) return value;
                beta = Mathf.Min(beta, value);
            }
        }
        return value;
    }
    
    private int NegaMax(Node node, int depth, int color) {
        if (depth == 0 || node.IsTerminal)
            return color * node.HeuristicValue;
        int value = int.MinValue;
        foreach (Node child in node.Children) {
            int childValue = -NegaMax(child, depth - 1, -color);
            value = Mathf.Max(value, childValue);
        }
        return color * value;
    }
    
    private int NegaMaxAlphaBeta(Node node, int depth, int alpha, int beta, int color) {
        if (depth == 0 || node.IsTerminal)
            return color * node.HeuristicValue;
        int value = int.MinValue;
        foreach (Node child in node.Children) {
            value = Mathf.Max(value, -NegaMaxAlphaBeta(child, depth - 1, -beta, -alpha, -color));
            alpha = Mathf.Max(alpha, value);
            if (alpha >= beta) break;
        }
        return color * value;
    }

    public Hashtable Hashtable = new Hashtable();

    public enum HashType {
        Exact,
        LowerBound,
        UpperBound
    }
    
    public class HashEntry {
        
        public int Depth;
        public int Value;
        public HashType Flag;
        
    }
    
    private int NegaMaxAlphaBetaTranspositionTables(Node node, int depth, int alpha, int beta, int color) {
        int alphaOrig = alpha;
        HashEntry hashEntry = (HashEntry) Hashtable[node];
        if (hashEntry != null && hashEntry.Depth >= depth) {
            if (hashEntry.Flag == HashType.Exact) return hashEntry.Value;
            else if (hashEntry.Flag == HashType.LowerBound) alpha = Mathf.Max(alpha, hashEntry.Value);
            else if (hashEntry.Flag == HashType.UpperBound) beta = Mathf.Min(beta, hashEntry.Value);
            if (alpha >= beta) return hashEntry.Value;
        }

        if (depth == 0 || node.IsTerminal) 
            return color * node.HeuristicValue;
        int value = int.MinValue;
        foreach (Node child in node.Children) {
            value = Mathf.Max(value, -NegaMaxAlphaBeta(child, depth - 1, -beta, -alpha, -color));
            alpha = Mathf.Max(alpha, value);
            if (alpha >= beta) break;
        }

        if (hashEntry == null) hashEntry = new HashEntry();
        hashEntry.Value = value;
        if (value <= alphaOrig) hashEntry.Flag = HashType.UpperBound;
        else if (value >= beta) hashEntry.Flag = HashType.LowerBound;
        else hashEntry.Flag = HashType.Exact;
        hashEntry.Depth = depth;
        Hashtable.Add(node, hashEntry);
        return value;
    }

}

public enum Algorithm {
    MinMax,
    MinMaxAlphaBeta,
    NegaMax,
    NegaMaxAlphaBeta,
    NegaMaxAlphaBetaWithTT
}