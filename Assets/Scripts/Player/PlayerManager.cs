using System;
using System.Collections.Generic;
using System.Linq;
using EventSystem.Channel;
using Manager;
using Units;
using Units.Formations;
using Units.Resources;
using Units.Structures;
using Units.Structures.Storages;
using Units.Villagers;
using UnityEngine;
using Utilities;

namespace Player
{
    public class PlayerManager : SingletonAutoGenerated<PlayerManager>
    {
        [Header("Player")] 
        [SerializeField] private EFaction _faction;
        public EFaction Faction => _faction;

        [Header("Events")] 
        [SerializeField] private ResourceChannel _onAddWorkVillager;
        [SerializeField] private ResourceChannel _onRemoveWorkVillager;

        private IController _controller;
        private FormationManager _formationManager;
        private UnitOrderManager _unitOrderManager;
        private UnitSelectorManager _unitSelectorManager;

        private readonly HashSet<Structure> _selectedStructures = new();
        
        private readonly HashSet<Unit> _selectedUnits = new();
        private readonly List<Villager> _selectedVillagers = new(); 
        private readonly List<Villager> _totalVillagers = new();
        private readonly Dictionary<ResourcesManager.ResourceType, List<Villager>> _villagersByResource = new(); 

        protected override void InitializeSingleton()
        {
            base.InitializeSingleton();

            _formationManager = GetComponent<FormationManager>();
            _unitOrderManager = new UnitOrderManager(this);
            _unitSelectorManager = new UnitSelectorManager(this);

            foreach (ResourcesManager.ResourceType resource in Enum.GetValues(typeof(ResourcesManager.ResourceType)))
            {
                _villagersByResource[resource] = new List<Villager>();
            }
        }
        
        private void Start() => _controller = new UnitController(UnitVisualManager.Instance, _unitSelectorManager, _unitOrderManager);

        private void Update() => _controller.ArtificialUpdate();
        private void FixedUpdate() => _controller.ArtificialFixedUpdate();

        public bool IsUnitSelected(Unit unit) => _selectedUnits.Contains(unit);

        public void SetStorage(IStorage storage)
        {
            if (_selectedVillagers.Count == 0) return;

            foreach (Villager selectedUnit in _selectedVillagers)
            {
                selectedUnit.SetWork(null);
                selectedUnit.SetStorage(storage);
            }
        }

        public void AssignWorkToUnits(IWork work)
        {
            if (_selectedVillagers.Count == 0) return;

            foreach (Villager selectedUnit in _selectedVillagers)
            {
                selectedUnit.SetStorage(null);
                selectedUnit.SetWork(work);
            }
        }

        public void SetEnemyTargets(ITargetable targetable)
        {
            foreach (Unit selectedUnit in _selectedUnits)
            {
                selectedUnit.SetTarget(targetable);
            }
        }

        public void MoveUnitsInFormation(Vector3 desiredPosition)
        {
            List<Vector3> positions = _formationManager.GetActualFormation(desiredPosition, _selectedUnits.ToList());

            int i = 0;
            foreach (Unit selectedUnit in _selectedUnits)
            {
                Vector3 position = positions[i];
                selectedUnit.SetDestination(position);
                i = (i + 1) % positions.Count;
            }
        }

        public void AddUnit(Unit unit)
        {
            _selectedUnits.Add(unit);
            unit.SelectUnit(); // ¿No se llama en la clase?
        }

        public void RemoveUnity(Unit unit)
        {
            _selectedUnits.Remove(unit);
            unit.DeselectUnit(); // ¿No se llama en la clase?
        }

        public void ClearUnits()
        {
            foreach (Unit selectedUnit in _selectedUnits)
            {
                selectedUnit.DeselectUnit();
            }

            _selectedUnits.Clear();
            _selectedVillagers.Clear();
        }

        public void AddSelectedVillager(Villager villager) => _selectedVillagers.Add(villager);
        public void RemoveSelectedVillager(Villager villager) => _selectedVillagers.Remove(villager);

        public void AddVillager(Villager villager)
        {
            if (villager.GetFaction() != Faction) return;

            _totalVillagers.Add(villager);
        }

        public void RemoveVillager(Villager villager)
        {
            if (villager.GetFaction() != Faction) return;

            _totalVillagers.Remove(villager);
        }

        /// <summary>
        /// Añade el villager a la lista de workingVillagers
        /// </summary>
        /// <param name="villager"></param>
        /// <param name="resourceType"></param>
        public void AddWorkingVillager(Villager villager, ResourcesManager.ResourceType resourceType)
        {
            if (!IsSameFaction(villager.GetFaction())) return;
            if (!_villagersByResource.TryGetValue(resourceType, out List<Villager> workingVillagers)) return;
            if (workingVillagers.Contains(villager)) return;  

            workingVillagers.Add(villager);
            _onAddWorkVillager.Invoke(new ResourceEvent(workingVillagers.Count, resourceType));
        }

        /// <summary>
        /// Remueve el villager de la lista de workingVillagers 
        /// </summary>
        /// <param name="villager"></param>
        /// <param name="resourceType"></param>
        public void RemoveWorkingVillager(Villager villager, ResourcesManager.ResourceType resourceType)
        {
            if (!IsSameFaction(villager.GetFaction())) return; 
            if (!_villagersByResource.TryGetValue(resourceType, out List<Villager> workingVillagers)) return;
            if (!workingVillagers.Contains(villager)) return;  
            
            workingVillagers.Remove(villager);
            _onRemoveWorkVillager.Invoke(new ResourceEvent(workingVillagers.Count, resourceType));
        }

        public void AddStructure(Structure structure) => _selectedStructures.Add(structure);
        public void RemoveStructure(Structure structure) => _selectedStructures.Remove(structure);
        public void ClearStructures() => _selectedStructures.Clear();
        public bool IsStructureSelected(Structure structure) => _selectedStructures.Contains(structure);


        private bool IsSameFaction(EFaction faction) => Faction == faction; 

        private void OnDrawGizmos() => _controller?.DrawGizmo();
    }

    public enum EFaction
    {
        Blue,
        Red
    }
}