

using System.Collections.Generic;
using UnityEngine;

namespace MS.CommonUtils{

    using Profiler;

    public class ObjectPool<T>{


        private Stack<T> _cache = new Stack<T>();
        private HashSet<T> _cacheSet = new HashSet<T>();

        public ObjectPool(){
        }

        public bool isEmpty{
            get{
                return this.Count == 0;
            }
        }

        private void ThrowExceptionIfEmpty(){
            if(isEmpty){
                throw new PoolException(PoolExceptionType.NoEnoughItems);
            }
        }

        protected void Clear(){
            while(_cache.Count > 0){
                var item = _cache.Pop();
                _cacheSet.Remove(item);
                EditorPoolProfiler.TrackRequest(this);  
                OnClearItem(item);
            }
        }

        protected virtual void OnClearItem(T item){}

        public virtual T Request(){
            ThrowExceptionIfEmpty();
            T ret = _cache.Pop();
            _cacheSet.Remove(ret);
            EditorPoolProfiler.TrackRequest(this);    
            return ret;  
        }

        public virtual void Release(T item){
            if(item == null){
                throw new System.ArgumentNullException("item");
            }
            if(_cacheSet.Contains(item)){
                throw new PoolException(PoolExceptionType.DuplicateRelease);
            }
            _cache.Push(item);
            _cacheSet.Add(item);
            EditorPoolProfiler.TrackRelease(this);
        }

        public int Count{
            get{
                return _cache.Count;
            }
        }

        public override string ToString()
        {
            return $"{this.GetType().Name}<{typeof(T).Name}>";
        }

    } 


    public enum PoolExceptionType{
        Others,
        NoEnoughItems,
        DuplicateRelease,
    }

    public class PoolException:System.Exception{

        private PoolExceptionType _type;
        public PoolException(PoolExceptionType type){
            _type = type;
        }

        public PoolExceptionType exceptionType{
            get{
                return _type;
            }
        }
    }

}