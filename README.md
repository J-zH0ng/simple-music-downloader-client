# simple-music-downloader
一个WPF网络音乐播放器
# 实现的功能
1. 音乐管理功能：管理PC中的音乐文件，构建个人音乐资料库
2. 本地音乐播放器的相关功能
3. 用户界面自定义
4. 音乐市场：音乐信息发布、下载
# 技术架构
- 客户端/服务器架构：通过WCF技术实现客户端和服务器的通信，进而实现了音乐信息发布，分发下载等服务。客户端使用WPF技术开发。
- MVVM：MVVM Light 框架。保证了软件的可维护性和可重用性。
- Material design：谷歌Material Design设计语言 Material design in xaml框架，类似于bootstrap
- 分层架构：
  1. 展示层：按照页面划分为：商城页面、本地音乐页面、歌单页面。
  2. 控制层：每个页面对应一个控制层，控制页面的交互逻辑（指令）和数据绑定。
  3. 服务层：按照不同的功能划分为：
      - 本地配置文件存储模块。
      - 歌曲下载模块和歌曲信息获取模块。
      - 音乐播放和控制模块。
      - 用户登录和注册模块。
  4. 模型层：按照不同的实体分为：
      - 用户类：封装了用户的信息。
      - 歌曲类：封装了歌曲信息和相关方法。
      - 播放列表类：封装了播放列表信息和相关方法。 
  5. 数据层：MySQL数据库、Entity Framework ORM框架。
- database tool： 把目录里的音乐文件批量录入数据库的脚本
# 展示
<div align=center>
  <img src="https://user-images.githubusercontent.com/69312774/218294687-2b7d9282-660f-4e15-884a-2e68c34848e4.png" alt="音乐市场、下载" title="音乐市场、下载" height=350 width=600>
  <img src="https://user-images.githubusercontent.com/69312774/218294758-8a1bd35e-5ed9-45c3-8ee6-1f91b286f1a0.png" alt="下载管理" title="下载管理" height=350 width=600>
  <img src="https://user-images.githubusercontent.com/69312774/218294818-03307212-3fbf-449d-bbaa-8be648dfffe7.png" alt="歌单" title="歌单" height=350 width=600>
  <img src="https://user-images.githubusercontent.com/69312774/218294849-b1052309-bed1-43ad-8268-8df2ad34ee26.png" alt="歌单" title="歌单" height=350 width=600>
  <img src="https://user-images.githubusercontent.com/69312774/218294891-7775029f-447d-40bc-8a45-11e3b957d0d8.png" alt="日间夜间界面切换" title="日间夜间界面切换" height=350 width=600>
  <img src="https://user-images.githubusercontent.com/69312774/218294934-79ced113-a060-42fa-8878-1526a74a1f7e.png" alt="换背景" title="换背景" height=350 width=600>
  <img src="https://user-images.githubusercontent.com/69312774/218294939-9d754992-561c-4ecf-8a61-972a833245e0.png" alt="换背景" title="换背景" height=350 width=600>
</div>
