# RizServer
## 项目介绍
**RizServer** 与此前的 **RizPS-Reborn** 一样，是 *某款简约风全屏判定音乐游戏的服务端重实现*，但 **RizServer** 支持游戏国际版的最新版本（1.0.5）（不支持港澳台）；而 **RizPS-Reborn** 仅支持游戏的港澳台版本，且支持的版本号停留在1.0.2。**RizServer** 使用C#重写了整个 **RizPS-Reborn** ，并针对游戏的国际版进行了适配，它的代码比 **RizPS-Reborn** 要规范无数倍，更便于阅读和维护，并且我们将 **RizServer** 的功能拆成多个模块，这让它的未来能有更多的可能。还有，多亏了强大的 [.NET](dot.net) 框架，这让 **RizServer** 的兼容性更上一层楼。
## 解决方案项目说明
`RizServerCore` : 使用C#编写的RizServer模块，实现了RizServer的核心功能（处理请求），输出类型为`类库`，可被其它C#项目引用

`RizServerConsole` : 使用C#编写的RizServer服务器，这也是我们最常用到的部分。它创建了一个Https服务器来进行request的接受和response的发送，并在经过种种判定后通过引用`RizServerCore`对请求进行处理，输出类型为`可执行程序`

`RizBCSharp` : 这是`Portable.BouncyCastle`的克隆，但被我们转换为一个`Shared`项目，以便直接引用，并最终减少输出的DLL的大小

`RizGo` : 这是一个全新的项目，它基于`nanoFramework`，我们期望的目标平台是`ESP32`。但由于该项目还处于原型阶段，我也不能告诉大家太多，只能说：敬请期待
## 使用方法
TODO:（以下内容是粗略且不完整的，将在日后补全）

克隆整个仓库，打开RizServer.sln，运行`RizServerConsole`项目即可

剩下的代理配置和证书信任什么的还是老样子，我相信你会的

解锁全歌曲的方法请自行摸索，鸽游目前经济状况不佳，还请 **支持正版**。