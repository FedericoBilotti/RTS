using Units.Visual;
using UnityEngine.Pool;
using Object = UnityEngine.Object;

namespace Pool.LifeBar
{
    public class BarLifeFactory
    {
        private readonly ObjectPool<BarLife> _pool;

        public BarLifeFactory(BarLife barLife, int amount)
        {
            _pool = new ObjectPool<BarLife>(
                    () => Object.Instantiate(barLife), 
                    x => x.gameObject.SetActive(true), 
                    x => x.gameObject.SetActive(false),
                    x => Object.Destroy(x.gameObject), 
                    defaultCapacity: amount, maxSize: 200);
        }

        public BarLife CreateObject()
        {
            return _pool.Get();
        }

        public void ReturnObject(BarLife item)
        {
            _pool.Release(item);
        }
    }
}