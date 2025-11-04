# Visual Studio 运行指南

## 📋 解决方案文件

已创建解决方案文件：`FamilyTreeSystem.sln`

包含以下项目：
- **Domain** - 领域层
- **Application** - 应用层  
- **Infrastructure** - 基础设施层
- **WebAPI** - API接口层（启动项目）

## 🚀 使用 Visual Studio 运行

### 方式一：直接打开解决方案

1. 双击打开 `Backend\FamilyTreeSystem.sln`
2. 在解决方案资源管理器中，右键点击 **WebAPI** 项目
3. 选择"设为启动项目"
4. 按 `F5` 或点击"启动"按钮运行

### 方式二：从 Visual Studio 打开文件夹

1. 打开 Visual Studio
2. 选择"打开项目或解决方案"
3. 导航到 `Backend` 文件夹
4. 选择 `FamilyTreeSystem.sln`

## ⚙️ 端口配置

### 后端端口：5000
- **配置文件**：`WebAPI/Properties/launchSettings.json`
- **访问地址**：http://localhost:5000
- **Swagger文档**：http://localhost:5000

配置内容：
```json
{
  "profiles": {
    "http": {
      "applicationUrl": "http://localhost:5000"
    }
  }
}
```

### 前端端口：5173
- **配置文件**：`Frontend/vite.config.ts`
- **访问地址**：http://localhost:5173
- **API代理**：/api -> http://localhost:5000

配置内容：
```typescript
{
  server: {
    port: 5173,
    proxy: {
      '/api': {
        target: 'http://localhost:5000',
        changeOrigin: true
      }
    }
  }
}
```

## 🔧 首次运行配置

### 1. 还原 NuGet 包

在 Visual Studio 中：
- 右键点击解决方案
- 选择"还原 NuGet 包"

或使用命令行：
```bash
cd Backend
dotnet restore
```

### 2. 设置启动项目

- 右键点击 **WebAPI** 项目
- 选择"设为启动项目"
- WebAPI 项目会以粗体显示

### 3. 运行项目

- 按 `F5` 启动调试
- 或按 `Ctrl+F5` 启动不调试
- 或点击工具栏的"启动"按钮

## 📊 项目依赖关系

```
WebAPI
  ├── Application
  │     ├── Domain
  │     └── Infrastructure
  └── Infrastructure
        └── Domain
```

## 🗄️ 数据库

- **类型**：SQLite
- **文件位置**：`Backend/WebAPI/family.db`
- **自动创建**：首次运行时自动创建
- **重置数据**：删除 `family.db` 文件后重新运行

## 🐛 调试技巧

### 设置断点
1. 在代码行号左侧点击，设置断点（红点）
2. 按 `F5` 启动调试
3. 程序会在断点处暂停

### 查看变量
- 鼠标悬停在变量上查看值
- 使用"监视"窗口添加变量
- 使用"即时窗口"执行表达式

### 热重载
- Visual Studio 2022 支持热重载
- 修改代码后自动应用更改
- 无需重启应用

## 📝 常用快捷键

- `F5` - 启动调试
- `Ctrl+F5` - 启动不调试
- `Shift+F5` - 停止调试
- `F9` - 设置/取消断点
- `F10` - 单步跳过
- `F11` - 单步进入
- `Ctrl+Shift+B` - 生成解决方案

## 🔍 查看 Swagger 文档

运行后端后，浏览器会自动打开 Swagger UI：
- **地址**：http://localhost:5000
- **功能**：
  - 查看所有 API 接口
  - 测试 API 调用
  - 查看请求/响应模型

## 🌐 同时运行前后端

### 后端（Visual Studio）
1. 在 Visual Studio 中按 `F5` 启动后端
2. 等待 Swagger 页面打开

### 前端（命令行或 VS Code）
1. 打开新的命令行窗口
2. 运行：
```bash
cd Frontend
npm install  # 首次运行
npm run dev
```
3. 访问 http://localhost:5173

## ⚠️ 常见问题

### 问题1：端口被占用
**错误**：`Address already in use`

**解决**：
1. 检查是否有其他程序占用 5000 端口
2. 修改 `launchSettings.json` 中的端口号
3. 同时修改前端 `vite.config.ts` 中的代理目标端口

### 问题2：NuGet 包还原失败
**解决**：
1. 检查网络连接
2. 清除 NuGet 缓存：工具 -> 选项 -> NuGet 包管理器 -> 清除所有 NuGet 缓存
3. 重新还原包

### 问题3：数据库连接失败
**解决**：
1. 检查 `appsettings.json` 中的连接字符串
2. 确保 SQLite NuGet 包已安装
3. 删除现有数据库文件，让程序重新创建

### 问题4：CORS 错误
**解决**：
- 已在 `Program.cs` 中配置 CORS
- 确保前端使用正确的 API 地址
- 检查浏览器控制台错误信息

## 📦 发布应用

### 发布到文件夹
1. 右键点击 **WebAPI** 项目
2. 选择"发布"
3. 选择"文件夹"
4. 配置发布路径
5. 点击"发布"

### 发布配置
```bash
dotnet publish -c Release -o ./publish
```

## 🎯 下一步

1. ✅ 打开 `FamilyTreeSystem.sln`
2. ✅ 设置 WebAPI 为启动项目
3. ✅ 按 F5 运行后端
4. ✅ 在另一个终端运行前端
5. ✅ 访问 http://localhost:5173 开始使用

---

**Visual Studio 版本要求**：Visual Studio 2022 或更高版本  
**SDK 要求**：.NET 8.0 SDK
