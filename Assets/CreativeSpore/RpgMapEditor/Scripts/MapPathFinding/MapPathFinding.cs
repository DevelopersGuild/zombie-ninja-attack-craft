using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CreativeSpore.PathFindingLib;

namespace CreativeSpore.RpgMapEditor
{

    public class MapTileNode : IPathNode
    {

        public int TileX { get; private set; }
        public int TileY { get; private set; }
        public int TileIdx { get; private set; }
        public Vector3 Position { get; private set; }
        
        List<int> m_neighborIdxList = new List<int>();
        MapPathFinding m_owner;

        internal MapTileNode(int idx, MapPathFinding owner) 
        {
            m_owner = owner;
            TileIdx = idx;
            TileX = idx % AutoTileMap.Instance.MapTileWidth;
            TileY = idx / AutoTileMap.Instance.MapTileWidth;
            Position = RpgMapHelper.GetTileCenterPosition(TileX, TileY);

            // get all neighbors row by row, neighIdx will be the idx of left most tile per each row
            int neighIdx = (idx-1)-AutoTileMap.Instance.MapTileWidth;
            for (int i = 0; i < 3; ++i, neighIdx += AutoTileMap.Instance.MapTileWidth)
            {
                for (int j = 0; j < 3; ++j)
                {
                    if (i != 1 || j != 1) // skip this node
                    {
                        m_neighborIdxList.Add(neighIdx + j);
                    }
                }
            }
        }

        #region IPathNode
        public override bool IsPassable() 
        {
            if (AutoTileMap.Instance.IsValidAutoTilePos(TileX, TileY))
            {
                AutoTileMap.AutoTile autoTile0 = AutoTileMap.Instance.GetAutoTile(TileX, TileY, 0);
                AutoTileMap.AutoTile autoTile1 = AutoTileMap.Instance.GetAutoTile(TileX, TileY, 1);
                AutoTileMap.eTileCollisionType collType0 = autoTile0.Type >= 0 ? AutoTileMap.Instance.Tileset.AutotileCollType[autoTile0.Type] : AutoTileMap.eTileCollisionType.EMPTY;
                AutoTileMap.eTileCollisionType collType1 = autoTile1.Type >= 0 ? AutoTileMap.Instance.Tileset.AutotileCollType[autoTile1.Type] : AutoTileMap.eTileCollisionType.EMPTY;

                // only layer 0 and 1 ( GROUND and OVERLAY_GROUND have collisions )
                bool isPassable0 =  (collType0 == AutoTileMap.eTileCollisionType.PASSABLE || collType0 == AutoTileMap.eTileCollisionType.WALL || collType0 == AutoTileMap.eTileCollisionType.OVERLAY);
                return collType1 == 
                    AutoTileMap.eTileCollisionType.EMPTY?
                    isPassable0 :
                    (collType1 == AutoTileMap.eTileCollisionType.PASSABLE || collType1 == AutoTileMap.eTileCollisionType.WALL || (collType1 == AutoTileMap.eTileCollisionType.OVERLAY && isPassable0));
            }
            return false;
        }

        public override float GetHeuristic( ) 
        {
            //NOTE: 10f in Manhattan and 14f in Diagonal should rally be 1f and 1.41421356237f, but I discovered by mistake these values improve the performance

            float h = 0f;

            switch( m_owner.HeuristicType )
            {
                case MapPathFinding.eHeuristicType.None: h = 0f; break;
                case MapPathFinding.eHeuristicType.Manhattan:
                    {
                        h = 10f * (Mathf.Abs(TileX - m_owner.EndNode.TileX) + Mathf.Abs(TileY - m_owner.EndNode.TileY));
                        break;
                    }
                case MapPathFinding.eHeuristicType.Diagonal:
                    {
                        float xf = Mathf.Abs(TileX - m_owner.EndNode.TileX);
                        float yf = Mathf.Abs(TileY - m_owner.EndNode.TileY);
                        if (xf > yf)
                            h = 14f * yf + 10f * (xf - yf);
                        else
                            h = 14f * xf + 10f * (yf - xf); 
                        break;
                    }
            }
            return h; 
        }

        // special case for walls
        bool _IsWallPassable( MapTileNode neighNode )
        {
            bool isPassableAutoTile1 = false;
            bool isPassableAutoTileNeigh1 = false;
            for (int iLayer = 1; iLayer >= 0; --iLayer)
            {
                AutoTileMap.AutoTile autoTile = AutoTileMap.Instance.GetAutoTile(TileX, TileY, iLayer);
                AutoTileMap.AutoTile autoTileNeigh = AutoTileMap.Instance.GetAutoTile(neighNode.TileX, neighNode.TileY, iLayer);
                if (iLayer == 1)
                {
                    isPassableAutoTile1 = (autoTile.Type >= 0 && AutoTileMap.Instance.Tileset.AutotileCollType[autoTile.Type] == AutoTileMap.eTileCollisionType.PASSABLE);
                    isPassableAutoTileNeigh1 = (autoTileNeigh.Type >= 0 && AutoTileMap.Instance.Tileset.AutotileCollType[autoTileNeigh.Type] == AutoTileMap.eTileCollisionType.PASSABLE);
                }

                if (autoTile.Type == autoTileNeigh.Type) // you can walk over two wall tiles if they have the same type
                {
                    continue;
                }
                else
                {
                    bool isWall = (autoTile.Type >= 0 && AutoTileMap.Instance.Tileset.AutotileCollType[autoTile.Type] == AutoTileMap.eTileCollisionType.WALL);
                    bool isWallNeigh = (autoTileNeigh.Type >= 0 && AutoTileMap.Instance.Tileset.AutotileCollType[autoTileNeigh.Type] == AutoTileMap.eTileCollisionType.WALL);
                    if (iLayer == 0)
                    { // even if it's a wall, a NONE tile over it make it walkable
                        isWall &= !isPassableAutoTile1;
                        isWallNeigh &= !isPassableAutoTileNeigh1;
                    }
                    if (isWall || isWallNeigh)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public override float GetNeigborMovingCost(int neigborIdx) 
        {
            float fCost = 1f;
            //012 // 
            //3X4 // neighbor index positions, X is the position of this node
            //567
            if( neigborIdx == 0 || neigborIdx == 2 || neigborIdx ==  5 || neigborIdx == 7 )
            {
                //check if can reach diagonals as it could be not possible if flank tiles are not passable      
                MapTileNode nodeN = GetNeighbor(1) as MapTileNode;
                MapTileNode nodeW = GetNeighbor(3) as MapTileNode;
                MapTileNode nodeE = GetNeighbor(4) as MapTileNode;
                MapTileNode nodeS = GetNeighbor(6) as MapTileNode;
                if (
                    (neigborIdx == 0 && (!nodeN.IsPassable() || !nodeW.IsPassable() || !_IsWallPassable(nodeN) || !_IsWallPassable(nodeW))) || // check North West
                    (neigborIdx == 2 && (!nodeN.IsPassable() || !nodeE.IsPassable() || !_IsWallPassable(nodeN) || !_IsWallPassable(nodeE))) || // check North East
                    (neigborIdx == 5 && (!nodeS.IsPassable() || !nodeW.IsPassable() || !_IsWallPassable(nodeS) || !_IsWallPassable(nodeW))) || // check South West
                    (neigborIdx == 7 && (!nodeS.IsPassable() || !nodeE.IsPassable() || !_IsWallPassable(nodeS) || !_IsWallPassable(nodeE)))    // check South East
                )
                {
                    return PathFinding.k_InfiniteCostValue;
                }
                else
                {
                    fCost = 1.41421356237f;
                }
            }
            else
            {
                fCost = 1f;
            }

            MapTileNode neighNode = GetNeighbor(neigborIdx) as MapTileNode;
            if (!_IsWallPassable(neighNode))
            {
                return PathFinding.k_InfiniteCostValue;
            }

            return fCost;  
        }
        public override int GetNeighborCount() { return m_neighborIdxList.Count; }
        public override IPathNode GetNeighbor(int idx) { return m_owner.GetMapTileNode(m_neighborIdxList[idx]); }
        #endregion
    }

    public class MapPathFinding
    {

        public enum eHeuristicType
        {
            None,       // very slow but guarantees the shortest path
            Manhattan, // faster than None, but does not guarantees the shortest path
            Diagonal // faster than Manhattan but less accurate
        }

        public eHeuristicType HeuristicType = eHeuristicType.Manhattan;

        /// <summary>
        /// Max iterations to find a path. Use a value <= 0 for infinite iterations.
        /// Remember max iterations will be reached when trying to find a path with no solutions.
        /// </summary>
        public int MaxIterations 
        {
            get { return m_pathFinding.MaxIterations; }
            set { MaxIterations = m_pathFinding.MaxIterations; }

        }
        
        public bool IsComputing { get { return m_pathFinding.IsComputing; } }

        PathFinding m_pathFinding = new PathFinding();
        Dictionary<int, MapTileNode> m_dicTileNodes = new Dictionary<int, MapTileNode>();
        internal MapTileNode EndNode { get; private set; }

        public MapTileNode GetMapTileNode( int idx )
        {
            MapTileNode mapTileNode;
            bool wasFound = m_dicTileNodes.TryGetValue(idx, out mapTileNode);
            if(!wasFound)
            {
                mapTileNode = new MapTileNode(idx, this);
                m_dicTileNodes[idx] = mapTileNode;
            }
            return mapTileNode;
        }

        public LinkedList<IPathNode> GetRouteFromTo(int startIdx, int endIdx)
        {
            LinkedList<IPathNode> nodeList = new LinkedList<IPathNode>();
            if (m_pathFinding.IsComputing)
            {
                Debug.LogWarning("PathFinding is already computing. GetRouteFromTo will not be executed!");
            }
            else
            {                
                IPathNode start = GetMapTileNode(startIdx);
                EndNode = GetMapTileNode(endIdx);
                nodeList = m_pathFinding.ComputePath(start, EndNode);
            }
            return nodeList;
        }

        public IEnumerator GetRouteFromToAsync( int startIdx, int endIdx )
        {
            IPathNode start = GetMapTileNode(startIdx);
            EndNode = GetMapTileNode(endIdx);
            return m_pathFinding.ComputePathAsync(start, EndNode);
        }
    }

}
