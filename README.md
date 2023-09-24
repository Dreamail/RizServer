# RizServer
## 项目介绍
**RizServer** 与此前的 **RizPS-Reborn** 一样，是 *某款简约风全屏判定音乐游戏的服务端重实现*，但 **RizServer** 支持游戏国际版的最新版本（1.0.10）（不支持港澳台）。它与此前的 **RizPS-Reborn** 相比，有以下优点：
- 使用C#重写了整个 RizPS-Reborn ，代码更规范、易读、易维护。
- 针对游戏的国际版进行了适配，支持更多的功能和特性。
- 将 RizServer 的功能拆成多个模块，提高了可扩展性和灵活性。
- 借助强大的 [.NET](dot.net) 框架，提高了兼容性和性能。
## 功能实现进度
- 本地资源分发（HotUpdateResources）：🟢
- 本地音乐资源分发（MusicResources）：🟡（很简单但懒得做）
- RhythNET模拟
    - 登录与注册：🟢
    - 完整账号系统架构：🟢
    - 静默登录：🟢
    - 注册时真正给用户的邮箱发送验证码（可选择开启与关闭）：🔴（总感觉没什么必要不是吗）
- 游戏API模拟
    - 用户存档的发送与存储：🟢
    - 打歌后的成绩存储：🟢
    - 官服完整体验模拟部分：
        - 打歌后掉dot：🟢
        - 拿coin和dot解锁歌：🟡（在做了在做了）
- RizServer额外功能部分：
    - WebPanel：🟡（在做了在做了，这次不可能做的那么简陋了）
## 解决方案项目说明
`RizServerCore` : 使用C#编写的RizServer模块，实现了RizServer的核心功能（处理请求），输出类型为`类库`，可被其它C#项目引用

`RizServerConsole` : 使用C#编写的RizServer服务器，这也是我们最常用到的部分。它创建了一个Https服务器来进行request的接受和response的发送，并在经过种种判定后通过引用`RizServerCore`对请求进行处理，输出类型为`可执行程序`

`RizBCSharp` : 这是`Portable.BouncyCastle`的克隆，但被我们转换为一个`Shared`项目，以便直接引用，并最终减少输出的DLL的大小
## 使用方法
TODO:（以下内容是粗略且不完整的，将在日后补全）

克隆整个仓库，打开RizServer.sln，运行`RizServerConsole`项目即可

剩下的代理配置和证书信任什么的还是老样子，我相信你会的

解锁全歌曲的方法请自行摸索，鸽游目前经济状况不佳，还请 **支持正版**。
