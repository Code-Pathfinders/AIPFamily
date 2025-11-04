# 快速启动指南

## 方式一：使用批处理脚本（推荐）

### 1. 安装前端依赖
双击运行 `install-frontend.bat`

### 2. 启动后端
双击运行 `start-backend.bat`

后端将在 http://localhost:5000 启动
Swagger API文档：http://localhost:5000

### 3. 启动前端
双击运行 `start-frontend.bat`

前端将在 http://localhost:5173 启动

## 方式二：手动命令行启动

### 后端启动

```bash
cd Backend/WebAPI
dotnet restore
dotnet run
```

### 前端启动

```bash
cd Frontend
npm install
npm run dev
```

## 首次使用

1. 打开浏览器访问 http://localhost:5173
2. 点击"创建家族"按钮，输入家族名称
3. 点击"添加成员"按钮，添加家族成员
4. 在图形中拖拽节点调整位置
5. 点击"保存布局"保存当前布局

## 注意事项

- 确保已安装 .NET 8 SDK
- 确保已安装 Node.js 18+
- 后端和前端需要同时运行
- 首次运行会自动创建 SQLite 数据库

## 端口配置

- 后端默认端口：5000
- 前端默认端口：5173

如需修改端口：
- 后端：修改 `Backend/WebAPI/Properties/launchSettings.json`
- 前端：修改 `Frontend/vite.config.ts` 中的 `server.port`

## 常见问题

### 1. 后端启动失败
- 检查是否安装了 .NET 8 SDK
- 运行 `dotnet --version` 确认版本

### 2. 前端启动失败
- 检查是否安装了 Node.js
- 删除 `node_modules` 文件夹后重新运行 `npm install`

### 3. 无法连接后端API
- 确认后端已成功启动
- 检查浏览器控制台是否有CORS错误
- 确认后端运行在 http://localhost:5000

## 技术支持

如遇到问题，请查看：
1. 后端日志（控制台输出）
2. 前端浏览器控制台
3. README.md 中的详细文档
