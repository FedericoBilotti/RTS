using System.Collections.Generic;
using UnityEngine;

namespace Units.Formations
{
    public class FormationManager : MonoBehaviour
    {
        [SerializeField] private FactoryFormation _factoryFormation;
        [SerializeField] private FormationType _actualFormationType = FormationType.Circle;

        private IFormation _actualFormation;

        private void OnValidate() => SetActualFormation(_factoryFormation.GetFormation(_actualFormationType));
        private void Awake() => SetActualFormation(_factoryFormation.GetFormation(_actualFormationType));

        public List<Vector3> GetActualFormation(Vector3 desiredPosition, List<Unit> indicatorsList) => _actualFormation.Shape(desiredPosition, indicatorsList);
        private void SetActualFormation(IFormation formation) => _actualFormation = formation;
    }
}