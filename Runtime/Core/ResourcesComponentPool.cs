using UnityEngine;

namespace MS.CommonUtils{
    using Profiler;


    /// <summary>
    /// 基于ComponentPool,扩展了自动加载功能。
    /// 进行Request请求时，如果Pool中没有了，那么自动根据提供的Path，从Resources中加载
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ResourcesComponentPool<T> :ComponentPool<T> where T:Component{
        private string _path;
        private GameObject _prefab;
        private bool _isPrefabLoaded = false;

        public ResourcesComponentPool(string poolName,string path):base(poolName){
            _path = path;
        }

        /// <summary>
        /// 解除对Prefab的缓存引用。使得相关资源可以被GC正常释放。
        /// </summary>
        public void UnrefPrefab(){
            _prefab = null;
            _isPrefabLoaded = false;
        }

        private GameObject prefab{
            get{
                if(!_isPrefabLoaded){
                    _isPrefabLoaded = true;
                    _prefab = Resources.Load<GameObject>(_path);
                    if(!_prefab){
                        Debug.LogError($"failed to load prefab under Resources/{_path}");
                    }
                }
                return _prefab;
            }
        }

        private T Load(){
            if(!prefab){
                return null;
            }
            var ret = Object.Instantiate<GameObject>(prefab).GetComponent<T>();
            EditorPoolProfiler.TrackAllocate(this);
            return ret;            
        }

        private T Load(Transform parent,Vector3 position,Quaternion rotation){
            if(!prefab){
                return null;
            }
            EditorPoolProfiler.TrackAllocate(this);
            return Object.Instantiate<GameObject>(prefab,position,rotation,parent).GetComponent<T>();
        }

        private T Load(Transform parent){
            if(!prefab){
                return null;
            }
            EditorPoolProfiler.TrackAllocate(this);
            return Object.Instantiate<GameObject>(prefab,parent).GetComponent<T>();
        }

        public override T Request(Transform parent){
            if(this.Count > 0){
                return base.Request(parent);
            }else{
                return this.Load(parent);
            }
        }

        public T Request(Transform parent,Vector3 position,Quaternion rotation){
            if(this.Count > 0){
                var ret = base.Request(parent);
                ret.transform.localPosition = position;
                ret.transform.localRotation = rotation;
                return ret;
            }else{
                return this.Load(parent,position,rotation);
            }           
        }
      
    }
}
