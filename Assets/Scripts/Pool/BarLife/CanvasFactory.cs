using System;
using System.Collections.Generic;
using Units.Visual;
using UnityEngine.Pool;
using Object = UnityEngine.Object;

namespace Pool.BarLife
{
    public class CanvasFactory
    {
        private readonly Dictionary<string, ObjectPool<Units.Visual.BarLife>> _pools = new();

        public CanvasFactory(PoolCanvasConfigurationSO canvasConfigurationSO)
        {
            CanvasConfig[] canvasConfigs = canvasConfigurationSO.Canvas;

            foreach (CanvasConfig canvasConfig in canvasConfigs)
            {
                var pool = new ObjectPool<Units.Visual.BarLife>(() => 
                                Object.Instantiate(canvasConfig.barLife), 
                        x => x.gameObject.SetActive(true), 
                        x => x.gameObject.SetActive(false),
                        x => Object.Destroy(x.gameObject), 
                        true, 
                        canvasConfig.amount);

                _pools.TryAdd(canvasConfig.id, pool);
            }
        }

        public void ReturnObject(string id, Units.Visual.BarLife item)
        {
            if (!_pools.TryGetValue(id, out ObjectPool<Units.Visual.BarLife> poolType))
            {
                throw new Exception("Pool not found");
            }

            poolType.Release(item);
        }

        public Units.Visual.BarLife CreateObject(string id)
        {
            if (!_pools.TryGetValue(id, out ObjectPool<Units.Visual.BarLife> poolType))
            {
                throw new Exception("Pool not found");
            }

            return poolType.Get();
        }
    }
}