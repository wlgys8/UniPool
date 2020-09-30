using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MS.CommonUtils{


    /// <summary>
    /// Designed for storing Component objects.
    /// All objects released into the pool will be the child of poolNode,which won't be destroyed on scene unload.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ComponentPool<T> :ObjectPool<T> where T:Component
    {
        
        private readonly string _name;
        private readonly bool _global;

        private PoolBehaviour _behaviour;
        
        public ComponentPool(string name):this(name,true){}

        /// <summary>
        /// [global=true]表示这个pool不随场景销毁.
        /// </summary>
        public ComponentPool(string name,bool global){
            _name = name;
            _global = global;
        }

        
        public Transform poolNode{
            get{
                if(AppQuittingCheck.quitting){
                    return null;
                }
                if(_behaviour == null){
                    _behaviour = new GameObject(_name).AddComponent<PoolBehaviour>();
                    _behaviour.gameObject.SetActive(false);
                    if(_global){
                        Object.DontDestroyOnLoad(_behaviour.gameObject);
                    }
                    _behaviour.onDestroy += OnBehaviourDestroy;
                }
                
                return _behaviour.transform;
            }
        }

        private void OnBehaviourDestroy(){
            this.Clear();
        }

        /// <summary>
        /// Remove all items from the pool, and call Object.Destroy on it's gameObject.
        /// </summary>
        public void ClearAndDestroyAll(){
            this.Clear();
        }

        protected override void OnClearItem(T item)
        {
            Object.Destroy(item.gameObject);
        }


        public T Request(Transform parent){
            T ret = base.Request();
            ret.transform.SetParent(parent,false);
            return ret;
        }

        public override T Request(){
            return Request(null);
        }

        public override void Release(T item){
            var p = poolNode;
            if(!item){
                return;
            }
            base.Release(item);
            item.transform.SetParent(poolNode,false);
        }

        public override string ToString()
        {
            return _name;
        }

        
    }

    internal static class AppQuittingCheck{
        public static bool quitting{
            get{
                return _quiting;
            }
        }
        private static void OnApplicationQuit(){
            _quiting = true;
        }

        [RuntimeInitializeOnLoadMethod]
        private static void InitializeOnLoad(){
            Application.quitting += OnApplicationQuit;
        }

        private static bool _quiting = false;        
    }

}
