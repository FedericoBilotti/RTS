using System.Collections.Generic;
using UnityEngine;

namespace Units.Formations
{
    public class SquareFormation : IFormation
    {
        private readonly float _lengthX;
        private readonly float _lengthZ;

        public SquareFormation(float lengthX, float lengthZ)
        {
            _lengthX = lengthX;
            _lengthZ = lengthZ;
        }
        
        public List<Vector3> Shape(Vector3 desiredPosition, List<Unit> indicatorsList)
        {
            List<Vector3> positions = new();

            int rowCount = Mathf.CeilToInt(Mathf.Sqrt(indicatorsList.Count));
            int colCount = Mathf.CeilToInt((float)indicatorsList.Count / rowCount);

            for (int i = 0; i < indicatorsList.Count; i++)
            {
                int row = i / colCount;
                int col = i % colCount;
    
                Vector3 spawnPosition = desiredPosition + new Vector3(col * _lengthX, 0, row * _lengthZ);
                positions.Add(spawnPosition);
            }

            return positions;
        }
    }
}