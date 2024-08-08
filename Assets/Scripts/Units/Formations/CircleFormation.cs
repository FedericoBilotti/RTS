using System.Collections.Generic;
using UnityEngine;

namespace Units.Formations
{
    public class CircleFormation : IFormation
    {
        private readonly float _radius;

        public CircleFormation(float radius) => _radius = radius;

        public List<Vector3> FormationShape(Vector3 desiredPosition, List<Unit> indicatorsList)
        {
            List<Vector3> positions = new();
    
            int unitCount = indicatorsList.Count;
            if (unitCount == 0) return positions;

            float degreesToIncrease = 360f / unitCount;

            for (int i = 0; i < unitCount; i++)
            {
                float currentDegree = degreesToIncrease * i;
                Vector3 offset = Quaternion.AngleAxis(currentDegree, Vector3.up) * (Vector3.forward * _radius);
                Vector3 instantiationPoint = desiredPosition + offset;
        
                positions.Add(instantiationPoint);
            }

            return positions;
        }
    }
}