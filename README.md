# 家族族谱管理系统

基于 DDD 领域驱动设计的家族族谱管理系统，支持可视化展示家族关系图谱。

## 技术栈

### 后端
- .NET Core 8 Web API
- Entity Framework Core
- SQLite 数据库
- Swagger API 文档

### 前端
- Vue 3
- TypeScript
- AntV G6 图可视化
- Pinia 状态管理
- Vite 构建工具

## 项目结构

```
AIProject/
├── Backend/
│   ├── Domain/              # 领域层（实体和值对象）
│   │   └── Entities/
│   ├── Application/         # 应用层（服务和DTOs）
│   │   ├── DTOs/
│   │   ├── Services/
│   │   └── Interfaces/
│   ├── Infrastructure/      # 基础设施层（数据访问）
│   │   ├── Data/
│   │   └── Repositories/
│   └── WebAPI/             # API接口层
│       └── Controllers/
└── Frontend/
    └── src/
        ├── api/            # API封装
        ├── components/     # 组件
        ├── stores/         # Pinia状态管理
        ├── types/          # TypeScript类型定义
        └── views/          # 页面视图
```

## 功能特性

### 后端功能
1. ✅ 家族管理（创建、查询、更新、删除）
2. ✅ 家族成员管理（CRUD操作）
3. ✅ 成员关系管理（父子、配偶关系）
4. ✅ 图形布局保存
5. ✅ Swagger API 文档

### 前端功能
1. ✅ 树状层级显示家族关系
2. ✅ 成员添加和修改
3. ✅ 节点可编辑（支持自定义属性）
4. ✅ 视图自适应
5. ✅ 美观的UI设计
6. ✅ 保存编辑好的关系图到数据库
7. ✅ 节点自定义样式和颜色

## 快速开始

### 前置要求

- .NET 8 SDK
- Node.js 18+ 和 npm

### 后端运行

#### 方式一：使用 Visual Studio（推荐）

1. 打开 `Backend\FamilyTreeSystem.sln`
2. 右键点击 **WebAPI** 项目，选择"设为启动项目"
3. 按 `F5` 运行

详细说明请查看：[Visual Studio 运行指南](Backend/README_VisualStudio.md)

#### 方式二：使用命令行

1. 进入后端目录：
```bash
cd Backend/WebAPI
```

2. 还原依赖：
```bash
dotnet restore
```

3. 运行项目：
```bash
dotnet run
```

后端将在 `http://localhost:5000` 启动，Swagger文档地址：`http://localhost:5000`

### 前端运行

1. 进入前端目录：
```bash
cd Frontend
```

2. 安装依赖：
```bash
npm install
```

3. 运行开发服务器：
```bash
npm run dev
```

前端将在 `http://localhost:5173` 启动

## 使用说明

### 1. 创建家族

点击顶部工具栏的"创建家族"按钮，填写家族信息。

### 2. 添加成员

选择家族后，点击"添加成员"按钮，填写成员信息：
- 基本信息：姓名、性别、世代
- 关系信息：父亲、母亲、配偶
- 可选信息：出生日期、头像、备注
- 自定义：节点颜色、自定义属性（JSON格式）

### 3. 编辑成员

- 在左侧成员列表中点击编辑按钮
- 或在图形中双击节点

### 4. 查看关系图

- 拖拽画布：移动视图
- 滚轮缩放：放大缩小
- 拖拽节点：调整位置
- 点击节点：查看详情

### 5. 保存布局

调整好节点位置后，点击"保存布局"按钮保存当前布局。

## API 文档

启动后端后，访问 `http://localhost:5000` 查看完整的 Swagger API 文档。

### 主要接口

#### 家族管理
- `GET /api/families` - 获取所有家族
- `GET /api/families/{id}` - 获取指定家族
- `POST /api/families` - 创建家族
- `PUT /api/families/{id}` - 更新家族
- `DELETE /api/families/{id}` - 删除家族

#### 家族成员管理
- `GET /api/familymembers/family/{familyId}` - 获取家族所有成员
- `GET /api/familymembers/{id}` - 获取指定成员
- `POST /api/familymembers` - 创建成员
- `PUT /api/familymembers/{id}` - 更新成员
- `DELETE /api/familymembers/{id}` - 删除成员
- `POST /api/familymembers/positions` - 批量更新成员位置

## 数据库

项目使用 SQLite 数据库，数据库文件 `family.db` 会在首次运行时自动创建在 `Backend/WebAPI` 目录下。

### 数据表结构

#### Families（家族表）
- Id - 主键
- Name - 家族名称
- Description - 描述
- OriginPlace - 起源地
- GraphLayout - 图形布局配置
- CreatedAt - 创建时间
- UpdatedAt - 更新时间

#### FamilyMembers（家族成员表）
- Id - 主键
- FamilyId - 所属家族ID
- Name - 姓名
- Gender - 性别
- BirthDate - 出生日期
- DeathDate - 去世日期
- FatherId - 父亲ID
- MotherId - 母亲ID
- SpouseId - 配偶ID
- Generation - 世代
- AvatarUrl - 头像URL
- CustomAttributes - 自定义属性（JSON）
- NodeStyle - 节点样式（JSON）
- Notes - 备注
- PositionX - X坐标
- PositionY - Y坐标
- CreatedAt - 创建时间
- UpdatedAt - 更新时间

## 开发说明

### DDD 架构说明

项目采用领域驱动设计（DDD）架构：

1. **Domain层**：定义核心业务实体和值对象
2. **Application层**：实现业务逻辑和应用服务
3. **Infrastructure层**：提供数据访问和外部服务
4. **WebAPI层**：暴露HTTP接口

### 图形可视化

使用 AntV G6 实现家族关系图可视化：
- 自定义节点样式
- 支持拖拽和缩放
- 自动布局算法
- 关系连线展示

## 许可证

MIT License

## 作者

AI Family Tree System
