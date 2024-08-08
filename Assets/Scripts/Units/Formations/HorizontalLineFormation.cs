using System.Collections.Generic;
using UnityEngine;

namespace Units.Formations
{
    public class HorizontalLineFormation : IFormation
    {
        private readonly float _length = 0;

        public HorizontalLineFormation(float length) => _length = length;

        public List<Vector3> FormationShape(Vector3 desiredPosition, List<Unit> indicatorsList)
        {
            List<Vector3> positions = new();
            if (indicatorsList.Count == 0) return positions;

            Vector3 spawnPosition = desiredPosition; // Prevents bugs
            positions.Add(spawnPosition);

            for (int i = 1; i < indicatorsList.Count; i++)
            {
                spawnPosition = desiredPosition + Vector3.right * (_length * i);
                positions.Add(spawnPosition);
            }

            return positions;
        }
    }
}