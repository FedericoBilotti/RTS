using System.Collections.Generic;
using UnityEngine;

namespace Units.Formations
{
    [CreateAssetMenu(fileName = "FactoryFormation", menuName = "Units/Formation/FactoryFormation")]
    public class FactoryFormation : ScriptableObject
    {
        [Header("Circle")]
        [SerializeField] private float _radiusCircleFormation = 5f;
        
        [Header("Horizontal Line")]
        [SerializeField] private float _distanceHorizontal = 2f;

        [Header("Square")] 
        [SerializeField] private float _distanceX = 3f;
        [SerializeField] private float _distanceZ = 4f;
        
        private IFormation _circleFormation;
        private IFormation _horizontalLineFormation;
        private IFormation _squareFormation;
        
        private readonly Dictionary<FormationType, IFormation> _formations = new();

        private void OnEnable()
        {
            _circleFormation = new CircleFormation(_radiusCircleFormation);
            _horizontalLineFormation = new HorizontalLineFormation(_distanceHorizontal);
            _squareFormation = new SquareFormation(_distanceX, _distanceZ);
            
            _formations.Add(FormationType.Circle, _circleFormation);
            _formations.Add(FormationType.HorizontalLine, _horizontalLineFormation);
            _formations.Add(FormationType.Square, _squareFormation);
        }

        // Va a ser cambiado con botones de la UI
        /// <summary>
        /// Change the actual formation unit
        /// </summary>
        /// <param name="formationType">Desired formation type</param>
        public IFormation GetFormation(FormationType formationType)
        {
            if (!_formations.TryGetValue(formationType, out IFormation formation))
            {
                Debug.LogWarning("No existe la formación: " + formationType);
                return null;
            }

            return formation;
        }
    }

    public enum FormationType
    {
        Circle,
        HorizontalLine,
        Square,
    }
}