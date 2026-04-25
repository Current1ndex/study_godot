# Godot 关键概念

游戏可以看作一棵由 **节点** 构成的 **树**，节点分组可以形成 **场景**，节点之间通过 **信号** 进行通信。

## 核心概念

- **节点**：最小的构筑块，Godot 提供了丰富的节点类型库。  
  所有节点都有以下特性：名称、属性、回调+更新、可扩展、与其他节点进行嵌套。

- **场景**：场景或预制件（Prefab），由一个或多个节点组成，可以互相嵌套。

- **场景树**：游戏的所有场景构成的树。

- **信号**：事件发生后发出信号，无需在代码中硬连接节点就能实现互相通信。

## 开发辅助

- **内置文档查阅方法**：<kbd>Fn</kbd> + <kbd>F1</kbd>

- **支持的脚本语言**：Godot 支持 4 种脚本语言。可以用 C 或 C++ 实现高要求算法，用 GDScript 或 C# 编写大部分游戏逻辑。

### C# 开发注意事项

1. 使用 C# 开发时要注意其垃圾回收机制。
2. **截止到 2026.04.19**，Godot 4 中用 C# 编写的项目无法导出到 Web 平台。

---

## 2D 游戏开发流程

> 官方教程：[2D 游戏开发入门](https://docs.godotengine.org/zh-cn/4.x/getting_started/first_2d_game/index.html)

### 开发步骤概览

1. **设置视图分辨率和放缩规则**
2. **场景划分**：主场景 + 玩家预制件 + 敌人预制件 + UI

#### a. 玩家预制件

| 节点 | 说明 |
|------|------|
| **根节点** `Area2D` | 代表 2D 空间中的一个区域，能够检测到其他 `CollisionObject2D` 的进出。 |
| **动画子节点** `AnimatedSprite2D` | 包含多个纹理作为动画播放帧的 Sprite 节点。利用 `SpriteFrames` 资源播放动画帧列表。 |
| **形状子节点** `CollisionShape2D` | 用于向 `CollisionObject2D` 父级提供 `Shape2D` 的节点。 |
| **_Ready** | 将常用属性 `export` 到检查器，获取视图大小，隐藏预制件。 |
| **_Process** | 监听用户输入进行移动（斜向速度处理、坐标裁剪）和动画播放（根据移动方向的不同控制播放）。 |
| **Signal** | `OnBodyEntered` 检测碰撞发送 `Hit` 信号，触发主场景的 `GameOver`。 |
| **交互** | 主场景调用 `Start` 初始化玩家位置并显示、启动碰撞。 |

#### b. 敌人预制件

| 节点 | 说明 |
|------|------|
| **根节点** `RigidBody2D` | 可以物理仿真进行移动的 2D 物理体。无法直接控制，必须对其施加力，物体仿真将计算由此产生的移动、旋转、碰撞反应及对沿途其他物理体的影响等。 |
| **动画子节点** `AnimatedSprite2D` | 同玩家预制件。 |
| **形状子节点** `CollisionShape2D` | 同玩家预制件。 |
| **检测节点** `VisibleOnScreenNotifier2D` | 表示 2D 空间的矩形区域，用于检测其在屏幕上是否可见。 |
| **_Ready** | 随机播放一种移动动画。 |
| **_Process** | 无。 |
| **Signal** | `OnVisibleOnScreenNotifier2DScreenExited` 检测是否可见，触发删除。 |
| **交互** | 主场景初始化敌人，根据 `Path2D` 的采样决定位置，生成随机角度和速度，加入视图 `OnMobTimerTimeout`。 |

#### c. UI

| 节点 | 说明 |
|------|------|
| **根节点** `CanvasLayer` | 用于 2D 场景中对象的独立渲染。 |
| **文本子节点** `Label` | 显示纯文本。 |
| **按钮子节点** `Label` | 按钮。 |
| **计时子节点** `Timer` | 用来倒数计时。 |
| **_Ready** | 无。 |
| **_Process** | 无。 |
| **Signal** | `OnStartButtonPressed` 按钮被按下，发送 `StartGame` 信号让主场景 `NewGame` 开始新游戏。 |
| **交互** | 主场景通过 `UpdateScore` 和 `ShowMessage` 修改展示文本（`OnMessageTimerTimeout` 隐藏文本），`ShowGameOver` 展示游戏结束页面。 |

#### d. 主场景

| 节点 | 说明 |
|------|------|
| **根节点** `Node` | 用于逻辑处理。 |
| **计时子节点** `Timer` | 用来倒数计时。 |
| **标记位置子节点** `Marker2D` | 用来提示 2D 位置。 |
| **路径子节点** `Path2D` | 用于创建 `Curve2D` 路径。其取样子节点为 `PathFollow2D`。 |
| **_Ready** | 无。 |
| **_Process** | 无。 |
| **Signal** | 无。 |
| **交互** | 主场景根据玩家的 `Hit` 信号，触发计时停止、UI `ShowGameOver` 和音乐切换；根据 UI 的 `StartGame` 信号，删除上局数据、生成玩家、开始计时（分数计算 `OnScoreTimerTimeout` 和敌人生成 `OnMobTimerTimeout`）、展示文本、播放音乐。 |


