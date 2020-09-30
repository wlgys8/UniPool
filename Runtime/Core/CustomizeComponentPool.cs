using UnityEngine;


namespace MS.CommonUtils{
    using Profiler;

    public class CustomizeComponentPool<T>:ComponentPool<T> where T:Component{

        public delegate T AllocateFunc();
        public delegate void RequestHandler(T item);
        public delegate void ReleaseHandler(T item);

        private AllocateFunc _allocFunc;
        private RequestHandler _requestHandler;
        private ReleaseHandler _releaseHandler;

        public CustomizeComponentPool(string name,AllocateFunc alloc,RequestHandler requestHandler,ReleaseHandler releaseHandler):base(name){
            if(alloc == null){
                throw new System.ArgumentNullException("alloc");
            }
            _allocFunc = alloc;
            _requestHandler = requestHandler;
            _releaseHandler = releaseHandler;
        }

        public CustomizeComponentPool(AllocateFunc alloc,RequestHandler requestHandler,ReleaseHandler releaseHandler):this("CustomizeComponentPool",alloc,requestHandler,releaseHandler){

        }

        private T AllocateItem(){
            EditorPoolProfiler.TrackAllocate(this);
            return _allocFunc();
        }

        public override T Request(){
            T item = default(T);
            if(this.isEmpty){
                item = _allocFunc();
            }else{
                item = base.Request();
            }
            if(_requestHandler != null){
                _requestHandler(item);
            }
            return item;
        }

        public override void Release(T item){
            if(_releaseHandler != null){
                _releaseHandler(item);
            }
            base.Release(item);
        }
    }
}
