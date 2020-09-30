# UniPool

Simple Object Pool Library for Unity

# ObjectPools

## `1. ObjectPool<T>`

最基础的Pool，可以通过:

- `Request` - 获取对象，如果Pool为空，则抛出`PoolException`
- `Release` - 放回对象

## `2. AutoAllocatePool<T>`

在`ObjectPool`的基础上，拥有自动创建对象功能, `T`满足`new()`约束。
在作Request请求时，若Pool为空，则自动`new T()`;

## `3. ComponentPool<T>`

1 . 基本特性

- 这个类型的Pool专门用来存储`UnityEngine.Component`类型的对象。
- 每个`ComponentPool`在场景中都有一个与之绑定的`gameObject`对象. 且该gameObject默认为`active=false`

- 所有释放回Pool中的对象，都会成为Pool的gameObject的子节点

2 . Global模式

`ComponentPool`的构造函数中有global变量, 默认为`true`，即会gameObject标记为DontDestroyOnLoad，不会随场景销毁而销毁。




## `4. ResourcesComponentPool<T>`

在`ComponentPool`基础上，拥有了从Resources目录下自动加载Prefab的功能.

### 构造函数

```csharp
ResourcesComponentPool(string name,string path)
```

path 指定了prefab要加载的路径


# Collections Pools

## `1. ListPool<T>`

可获取`List<T>`对象

## `2. SetPool<T>`

可获取`HashSet<T>`对象

## `3. DictPool<K,V>`

可获取`Dictionary<K,V>`对象


