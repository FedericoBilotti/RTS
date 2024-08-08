using System.Collections.Generic;
using UnityEngine;

namespace Units.Formations
{
    public interface IFormation
    {
        /// <summary>
        /// Devuelve la formación deseada con las posiciones de cada unidad
        /// </summary>
        /// <param name="desiredPosition">Posición deseada</param>
        /// <param name="indicatorsList">La lista de unidades para que se forme la formación</param>
        /// <returns></returns>
        List<Vector3> FormationShape(Vector3 desiredPosition, List<Unit> indicatorsList);
    }
}