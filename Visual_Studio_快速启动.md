# Visual Studio 快速启动指南

## 🎯 一分钟快速启动

### 步骤 1：打开解决方案
双击打开：`Backend\FamilyTreeSystem.sln`

### 步骤 2：设置启动项目
在解决方案资源管理器中：
- 右键点击 **WebAPI** 项目
- 选择"设为启动项目"

### 步骤 3：运行后端
按 `F5` 键启动调试

浏览器会自动打开 Swagger 文档：http://localhost:5000

### 步骤 4：运行前端
打开新的命令行窗口：
```bash
cd Frontend
npm install  # 首次运行
npm run dev
```

访问：http://localhost:5173

## ✅ 完成！

现在您可以开始使用家族族谱管理系统了！

---

## 📋 详细说明

### 端口配置
- **后端**：http://localhost:5000
- **前端**：http://localhost:5173

### 配置文件位置
- **后端端口**：`Backend/WebAPI/Properties/launchSettings.json`
- **前端端口**：`Frontend/vite.config.ts`

### 更多信息
- [Visual Studio 详细指南](Backend/README_VisualStudio.md)
- [端口配置说明](端口配置说明.md)
- [完整项目文档](README.md)

---

## 🔧 常见问题

### Q: 端口被占用怎么办？
A: 参考 [端口配置说明.md](端口配置说明.md) 修改端口

### Q: 如何调试代码？
A: 在代码行号左侧点击设置断点，按 F5 启动调试

### Q: 数据库在哪里？
A: `Backend/WebAPI/family.db`（首次运行自动创建）

---

**Visual Studio 版本**：推荐使用 Visual Studio 2022  
**SDK 版本**：.NET 8.0
