# RizServer
## 项目介绍
**RizServer** 与此前的 **RizPS-Reborn** 一样，是 *某款简约风全屏判定音乐游戏的服务端重实现*，但 **RizServer** 支持游戏国际版的最新版本（1.1.1）（不支持港澳台，不能向下兼容）。它与此前的 **RizPS-Reborn** 相比，有以下优点：
- 使用C#重写了整个 RizPS-Reborn ，代码更规范、易读、易维护。
- 针对游戏的国际版进行了适配，支持更多的功能和特性。
- 将 RizServer 的功能拆成多个模块，提高了可扩展性和灵活性。
- 借助强大的 [.NET](dot.net) 框架，提高了兼容性和性能。
## 功能实现进度
- 本地资源分发（HotUpdateResources）：🟢
- 本地音乐资源分发（MusicResources）：🟢
- RhythNET模拟
    - 登录与注册：🟢
    - 完整账号系统架构：🟢
    - 静默登录：🟢
    - 注册时真正给用户的邮箱发送验证码（可选择开启与关闭）：🔴（总感觉没什么必要不是吗）
- 游戏API模拟
    - 用户存档的发送与存储：🟢
    - 打歌后的成绩存储：🟢
    - RizCard相关数据存储：🟢
    - 官服完整体验模拟部分：
        - 打歌后掉dot：🟢
        - 拿coin和dot解锁歌：🟡（更新后数据结构出现变化，需要修复）
        - 收藏品商城：🟡
- RizServer额外功能部分：
    - WebPanel：🟡（在做了在做了，这次不可能做的那么简陋了）
## 解决方案项目说明
`RizServerCore` : 使用C#编写的RizServer模块，实现了RizServer的核心功能（处理请求），输出类型为`类库`，可被其它C#项目引用

`RizServerConsole` : 使用C#编写的RizServer服务器，这也是我们最常用到的部分。它创建了一个Https服务器来进行request的接受和response的发送，并在经过种种判定后通过引用`RizServerCore`对请求进行处理，输出类型为`可执行程序`

`RizBCSharp` : 这是`Portable.BouncyCastle`的克隆，但被我们转换为一个`Shared`项目，以便直接引用，并最终减少输出的DLL的大小
## 使用方法
以下教程较为粗略，出于某些原因也没法写的十分详细，因此请开动你巧妙的大脑！

1. 安装带有C#功能特性的`Visual Studio 2022`
2. 安装`.NET SDK 8.0`
3. 克隆整个仓库，打开RizServer.sln，编译`RizServerConsole`项目
4. 找到编译输出目录（与RizServerConsole.exe同目录），下载[Resources](https://github.com/osp-project/RizServerResources)，命名为resources文件夹，丢进编译输出目录
5. 运行RizServerConsole.exe
6. 安装`Fiddler Classic`
7. 找到让你手机信任电脑Fiddler Classic证书的方法，并进行信任
8. 选用合适的Fiddler Script：[1. 在线热更版（需要在线下载更新，官方版本升级即失效）](https://gist.github.com/Searchstars/4df7b9658a9ef3000a1673ed14b5bc7c) [2. 本地热更版（完全离线，版本更新需要同时更新resources）](https://gist.github.com/Searchstars/71e67cbf03e4da317b68fd9079c341ef)
9. 根据手机系统选择ProxyDroid或ShadowRocket（注意规则设置）作为你的代理软件，连接你电脑的Fiddler Classic代理
10. Enjoy it

进入游戏后默认只解锁新人三首歌，解锁全歌曲的方法请自行摸索，请时刻对游戏开发商保持敬畏之心，并在有能力的情况下 **支持正版**。