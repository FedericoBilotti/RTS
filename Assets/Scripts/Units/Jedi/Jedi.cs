using Units.SO;
using UnityEngine;

namespace Units.Jedi
{
    public class Jedi : Unit
    {
        [SerializeField] private string _actualState; // Para saber el estado en que me encuentro -> borrar después.
        [SerializeField] private JediSO _jediSO;


        private void Start()
        {
            CreateFSM();
        }

        private void Update() => fsm.Update();
        private void FixedUpdate() => fsm.FixedUpdate();

        public void SetStateName(string state) => _actualState = state; // Debug.

        private void CreateFSM()
        {
            
        }
    }
}