<template>
  <div class="family-tree-view">
    <!-- 顶部工具栏 -->
    <div class="toolbar">
      <div class="toolbar-left">
        <h1 class="app-title">
          <span class="logo">👨‍👩‍👧‍👦</span>
          <span class="title-text">家族族谱管理系统</span>
        </h1>
        <select v-model="selectedFamilyId" @change="handleFamilyChange" class="family-selector">
          <option value="">选择家族</option>
          <option v-for="family in families" :key="family.id" :value="family.id">
            {{ family.name }}
          </option>
        </select>
        <button @click="showCreateFamily" class="btn btn-primary">
          <span class="btn-icon">➕</span>
          <span class="btn-text">创建家族</span>
        </button>
      </div>
      <div class="toolbar-right">
        <button @click="showAddMember" class="btn btn-success" :disabled="!currentFamily" title="添加成员">
          <span class="btn-icon">👤</span>
          <span class="btn-text">添加成员</span>
        </button>
        <button @click="handleSaveLayout" class="btn btn-info" :disabled="!currentFamily" title="保存布局">
          <span class="btn-icon">💾</span>
          <span class="btn-text">保存布局</span>
        </button>
        <button @click="toggleConfigPanel" class="btn btn-secondary" title="显示配置">
          <span class="btn-icon">⚙️</span>
          <span class="btn-text">显示配置</span>
        </button>
        <button @click="handleRefresh" class="btn btn-default" title="刷新">
          <span class="btn-icon">🔄</span>
          <span class="btn-text">刷新</span>
        </button>
      </div>
    </div>

    <!-- 主内容区 -->
    <div class="main-content">
      <!-- 左侧成员列表 -->
      <div class="sidebar">
        <h3>成员列表 ({{ members.length }})</h3>
        <div class="member-list">
          <div 
            v-for="member in sortedMembers" 
            :key="member.id" 
            class="member-item"
            :class="{ selected: selectedMember?.id === member.id }"
            @click="handleMemberSelect(member)"
          >
            <div class="member-avatar" :style="{ background: getMemberColor(member) }">
              {{ member.name[0] }}
            </div>
            <div class="member-info">
              <div class="member-name">{{ member.name }}</div>
              <div class="member-meta">
                {{ member.gender === 'Male' ? '男' : '女' }} · 第{{ member.generation }}代
              </div>
            </div>
            <div class="member-actions">
              <button @click.stop="handleEditMember(member)" class="icon-btn" title="编辑">
                ✏️
              </button>
              <button @click.stop="handleDeleteMember(member)" class="icon-btn" title="删除">
                🗑️
              </button>
            </div>
          </div>
        </div>
      </div>

      <!-- 中间图形区域 -->
      <div class="graph-area">
        <div v-if="!currentFamily" class="empty-state">
          <h2>👨‍👩‍👧‍👦</h2>
          <p>请选择或创建一个家族开始使用</p>
        </div>
        <FamilyTree 
          v-else
          :members="members"
          :display-config="displayConfig"
          @node-click="handleNodeClick"
          @node-double-click="handleNodeDoubleClick"
          @position-change="handlePositionChange"
        />
      </div>

      <!-- 配置面板 -->
      <div v-if="showConfigPanel" class="config-panel-overlay" @click="showConfigPanel = false">
        <div class="config-panel-container" @click.stop>
          <DisplayConfigPanel 
            :config="displayConfig"
            @change="handleConfigChange"
            @apply="handleConfigApply"
          />
        </div>
      </div>

      <!-- 右侧详情面板 -->
      <div class="detail-panel" v-if="selectedMember">
        <div class="detail-header">
          <h3>👤 成员详情</h3>
          <button class="close-detail-btn" @click="selectedMember = null">✕</button>
        </div>
        
        <div class="detail-content">
          <!-- 基本信息卡片 -->
          <div class="info-card">
            <div class="card-title">📋 基本信息</div>
            <div class="detail-item">
              <span class="item-icon">👤</span>
              <div class="item-content">
                <label>姓名</label>
                <span class="item-value">{{ selectedMember.name }}</span>
              </div>
            </div>
            <div class="detail-item">
              <span class="item-icon">{{ selectedMember.gender === 'Male' ? '♂️' : '♀️' }}</span>
              <div class="item-content">
                <label>性别</label>
                <span class="item-value">{{ selectedMember.gender === 'Male' ? '男' : '女' }}</span>
              </div>
            </div>
            <div class="detail-item">
              <span class="item-icon">🔢</span>
              <div class="item-content">
                <label>世代</label>
                <span class="item-value">第{{ selectedMember.generation }}代</span>
              </div>
            </div>
          </div>

          <!-- 日期信息卡片 -->
          <div class="info-card" v-if="selectedMember.birthDate || selectedMember.deathDate">
            <div class="card-title">📅 日期信息</div>
            <div class="detail-item" v-if="selectedMember.birthDate">
              <span class="item-icon">🎂</span>
              <div class="item-content">
                <label>出生日期</label>
                <span class="item-value">{{ formatDate(selectedMember.birthDate) }}</span>
              </div>
            </div>
            <div class="detail-item" v-if="selectedMember.deathDate">
              <span class="item-icon">🕊️</span>
              <div class="item-content">
                <label>去世日期</label>
                <span class="item-value">{{ formatDate(selectedMember.deathDate) }}</span>
              </div>
            </div>
          </div>

          <!-- 家庭关系卡片 -->
          <div class="info-card" v-if="getFather(selectedMember) || getMother(selectedMember) || getSpouse(selectedMember)">
            <div class="card-title">👨‍👩‍👧‍👦 家庭关系</div>
            <div class="detail-item" v-if="getFather(selectedMember)">
              <span class="item-icon">👨</span>
              <div class="item-content">
                <label>父亲</label>
                <span class="item-value">{{ getFather(selectedMember)?.name }}</span>
              </div>
            </div>
            <div class="detail-item" v-if="getMother(selectedMember)">
              <span class="item-icon">👩</span>
              <div class="item-content">
                <label>母亲</label>
                <span class="item-value">{{ getMother(selectedMember)?.name }}</span>
              </div>
            </div>
            <div class="detail-item" v-if="getSpouse(selectedMember)">
              <span class="item-icon">💑</span>
              <div class="item-content">
                <label>配偶</label>
                <span class="item-value">{{ getSpouse(selectedMember)?.name }}</span>
              </div>
            </div>
          </div>

          <!-- 备注信息卡片 -->
          <div class="info-card" v-if="selectedMember.notes">
            <div class="card-title">📝 备注</div>
            <div class="notes-content">{{ selectedMember.notes }}</div>
          </div>

          <!-- 自定义属性卡片 -->
          <div class="info-card" v-if="selectedMember.customAttributes">
            <div class="card-title">⚙️ 自定义属性</div>
            <pre class="custom-attrs">{{ formatJSON(selectedMember.customAttributes) }}</pre>
          </div>
        </div>
      </div>
    </div>

    <!-- 成员编辑器 -->
    <MemberEditor
      :visible="editorVisible"
      :member="editingMember"
      :members="members"
      :family-id="selectedFamilyId"
      @close="handleEditorClose"
      @submit="handleMemberSubmit"
    />

    <!-- 创建家族对话框 -->
    <div v-if="createFamilyVisible" class="modal-overlay" @click="createFamilyVisible = false">
      <div class="modal-content small" @click.stop>
        <div class="modal-header">
          <h2>创建家族</h2>
          <button class="close-btn" @click="createFamilyVisible = false">×</button>
        </div>
        <div class="modal-body">
          <div class="form-group">
            <label>家族名称 *</label>
            <input v-model="newFamilyName" type="text" placeholder="请输入家族名称" />
          </div>
          <div class="form-group">
            <label>描述</label>
            <textarea v-model="newFamilyDesc" rows="3" placeholder="请输入家族描述"></textarea>
          </div>
          <div class="form-group">
            <label>起源地</label>
            <input v-model="newFamilyOrigin" type="text" placeholder="请输入起源地" />
          </div>
        </div>
        <div class="modal-footer">
          <button class="btn btn-cancel" @click="createFamilyVisible = false">取消</button>
          <button class="btn btn-primary" @click="handleCreateFamily">创建</button>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import { useFamilyStore } from '@/stores/familyStore'
import FamilyTree from '@/components/FamilyTree.vue'
import MemberEditor from '@/components/MemberEditor.vue'
import DisplayConfigPanel from '@/components/DisplayConfigPanel.vue'
import type { FamilyMember, GraphDisplayConfig } from '@/types'

const familyStore = useFamilyStore()

// 状态
const selectedFamilyId = ref('')
const selectedMember = ref<FamilyMember | null>(null)
const editorVisible = ref(false)
const editingMember = ref<FamilyMember | undefined>(undefined)
const createFamilyVisible = ref(false)
const newFamilyName = ref('')
const newFamilyDesc = ref('')
const newFamilyOrigin = ref('')
const showConfigPanel = ref(false)

// 显示配置
const displayConfig = ref<GraphDisplayConfig>({
  layoutDirection: 'TB',
  cardFields: {
    showName: true,
    showGeneration: true,
    showBirthYear: true,
    showGender: false,
    showAvatar: true
  },
  nodeWidth: 150,
  nodeHeight: 80,
  nodeSpacing: 50,
  rankSpacing: 100,
  edgeStyle: {
    fatherLineColor: '#5B8FF9',
    motherLineColor: '#F759AB',
    lineWidth: 2.5,
    fatherLineStyle: 'solid',
    motherLineStyle: 'dashed',
    showShadow: true
  }
})

// 计算属性
const families = computed(() => familyStore.families)
const currentFamily = computed(() => familyStore.currentFamily)
const members = computed(() => familyStore.members)
const loading = computed(() => familyStore.loading)

/**
 * 排序后的成员列表
 */
const sortedMembers = computed(() => {
  return [...members.value].sort((a, b) => {
    if (a.generation !== b.generation) {
      return a.generation - b.generation
    }
    return a.name.localeCompare(b.name)
  })
})

/**
 * 处理家族切换
 */
const handleFamilyChange = async () => {
  if (selectedFamilyId.value) {
    await familyStore.setCurrentFamily(selectedFamilyId.value)
    selectedMember.value = null
  }
}

/**
 * 显示创建家族对话框
 */
const showCreateFamily = () => {
  newFamilyName.value = ''
  newFamilyDesc.value = ''
  newFamilyOrigin.value = ''
  createFamilyVisible.value = true
}

/**
 * 创建家族
 */
const handleCreateFamily = async () => {
  if (!newFamilyName.value) {
    alert('请输入家族名称')
    return
  }

  try {
    const family = await familyStore.createFamily(
      newFamilyName.value,
      newFamilyDesc.value,
      newFamilyOrigin.value
    )
    selectedFamilyId.value = family.id
    await handleFamilyChange()
    createFamilyVisible.value = false
  } catch (error) {
    alert('创建家族失败')
  }
}

/**
 * 显示添加成员
 */
const showAddMember = () => {
  editingMember.value = undefined
  editorVisible.value = true
}

/**
 * 处理编辑成员
 */
const handleEditMember = (member: FamilyMember) => {
  editingMember.value = member
  editorVisible.value = true
}

/**
 * 处理删除成员
 */
const handleDeleteMember = async (member: FamilyMember) => {
  if (!confirm(`确定要删除 ${member.name} 吗？`)) {
    return
  }

  try {
    await familyStore.deleteMember(member.id)
    if (selectedMember.value?.id === member.id) {
      selectedMember.value = null
    }
  } catch (error) {
    alert('删除成员失败')
  }
}

/**
 * 处理成员选择
 */
const handleMemberSelect = (member: FamilyMember) => {
  selectedMember.value = member
}

/**
 * 处理节点点击
 */
const handleNodeClick = (member: FamilyMember) => {
  selectedMember.value = member
}

/**
 * 处理节点双击
 */
const handleNodeDoubleClick = (member: FamilyMember) => {
  handleEditMember(member)
}

/**
 * 处理位置变化
 */
const handlePositionChange = async (positions: Array<{ id: string; x: number; y: number }>) => {
  // 这里可以实现自动保存或者等待用户点击保存按钮
  console.log('位置已变化', positions)
}

/**
 * 保存布局
 */
const handleSaveLayout = async () => {
  try {
    // 这里可以保存图形布局配置
    alert('布局已保存')
  } catch (error) {
    alert('保存布局失败')
  }
}

/**
 * 刷新数据
 */
const handleRefresh = async () => {
  if (selectedFamilyId.value) {
    await familyStore.setCurrentFamily(selectedFamilyId.value)
  }
}

/**
 * 切换配置面板显示
 */
const toggleConfigPanel = () => {
  showConfigPanel.value = !showConfigPanel.value
}

/**
 * 配置变化处理
 */
const handleConfigChange = (config: GraphDisplayConfig) => {
  displayConfig.value = config
}

/**
 * 应用配置
 */
const handleConfigApply = (config: GraphDisplayConfig) => {
  displayConfig.value = config
  showConfigPanel.value = false
}

/**
 * 关闭编辑器
 */
const handleEditorClose = () => {
  editorVisible.value = false
  editingMember.value = undefined
}

/**
 * 提交成员数据
 */
const handleMemberSubmit = async (data: any) => {
  try {
    if (editingMember.value) {
      await familyStore.updateMember(editingMember.value.id, data)
    } else {
      await familyStore.createMember(data)
    }
    handleEditorClose()
  } catch (error) {
    alert(editingMember.value ? '更新成员失败' : '创建成员失败')
  }
}

/**
 * 获取父亲
 */
const getFather = (member: FamilyMember) => {
  return members.value.find(m => m.id === member.fatherId)
}

/**
 * 获取母亲
 */
const getMother = (member: FamilyMember) => {
  return members.value.find(m => m.id === member.motherId)
}

/**
 * 获取配偶
 */
const getSpouse = (member: FamilyMember) => {
  return members.value.find(m => m.id === member.spouseId)
}

/**
 * 获取成员颜色
 */
const getMemberColor = (member: FamilyMember) => {
  if (member.nodeStyle) {
    try {
      const style = JSON.parse(member.nodeStyle)
      return style.fill || (member.gender === 'Male' ? '#5B8FF9' : '#F759AB')
    } catch (e) {
      return member.gender === 'Male' ? '#5B8FF9' : '#F759AB'
    }
  }
  return member.gender === 'Male' ? '#5B8FF9' : '#F759AB'
}

/**
 * 格式化日期
 */
const formatDate = (dateStr: string) => {
  return new Date(dateStr).toLocaleDateString('zh-CN')
}

/**
 * 格式化JSON
 */
const formatJSON = (jsonStr: string) => {
  try {
    return JSON.stringify(JSON.parse(jsonStr), null, 2)
  } catch (e) {
    return jsonStr
  }
}

// 组件挂载时加载数据
onMounted(async () => {
  await familyStore.loadFamilies()
})
</script>

<style scoped>
.family-tree-view {
  width: 100%;
  height: 100%;
  display: flex;
  flex-direction: column;
  background: #f0f2f5;
}

.toolbar {
  background: white;
  padding: 16px 24px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
  display: flex;
  justify-content: space-between;
  align-items: center;
  z-index: 10;
}

.toolbar-left,
.toolbar-right {
  display: flex;
  align-items: center;
  gap: 12px;
}

.app-title {
  margin: 0;
  font-size: 20px;
  color: #333;
  display: flex;
  align-items: center;
  gap: 8px;
}

.logo {
  font-size: 24px;
}

.title-text {
  font-weight: 600;
}

.btn-icon {
  font-size: 16px;
}

.btn-text {
  margin-left: 4px;
}

.family-selector {
  padding: 8px 12px;
  border: 1px solid #d9d9d9;
  border-radius: 4px;
  font-size: 14px;
  min-width: 200px;
}

.main-content {
  flex: 1;
  display: flex;
  overflow: hidden;
}

.sidebar {
  width: 280px;
  background: white;
  border-right: 1px solid #e8e8e8;
  display: flex;
  flex-direction: column;
}

.sidebar h3 {
  padding: 16px;
  margin: 0;
  border-bottom: 1px solid #e8e8e8;
  font-size: 16px;
  color: #333;
}

.member-list {
  flex: 1;
  overflow-y: auto;
  padding: 8px;
}

.member-item {
  display: flex;
  align-items: center;
  padding: 12px;
  margin-bottom: 8px;
  background: #fafafa;
  border-radius: 4px;
  cursor: pointer;
  transition: all 0.3s;
}

.member-item:hover {
  background: #e6f7ff;
}

.member-item.selected {
  background: #bae7ff;
}

.member-avatar {
  width: 40px;
  height: 40px;
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  color: white;
  font-weight: bold;
  font-size: 18px;
  flex-shrink: 0;
}

.member-info {
  flex: 1;
  margin-left: 12px;
  min-width: 0;
  overflow: hidden;
}

.member-name {
  font-weight: 500;
  color: #333;
  margin-bottom: 4px;
  overflow: hidden;
  text-overflow: ellipsis;
  white-space: nowrap;
}

.member-meta {
  font-size: 12px;
  color: #999;
  overflow: hidden;
  text-overflow: ellipsis;
  white-space: nowrap;
}

.member-actions {
  display: flex;
  gap: 4px;
}

.icon-btn {
  background: none;
  border: none;
  cursor: pointer;
  font-size: 16px;
  padding: 4px;
  opacity: 0.6;
  transition: opacity 0.3s;
}

.icon-btn:hover {
  opacity: 1;
}

.graph-area {
  flex: 1;
  position: relative;
  background: white;
}

.empty-state {
  position: absolute;
  top: 50%;
  left: 50%;
  transform: translate(-50%, -50%);
  text-align: center;
  color: #999;
}

.empty-state h2 {
  font-size: 64px;
  margin: 0 0 16px 0;
}

.empty-state p {
  font-size: 16px;
  margin: 0;
}

.detail-panel {
  width: 360px;
  background: #f8f9fa;
  border-left: 1px solid #e8e8e8;
  display: flex;
  flex-direction: column;
  overflow: hidden;
}

.detail-header {
  padding: 16px;
  background: white;
  border-bottom: 1px solid #e8e8e8;
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.detail-header h3 {
  margin: 0;
  font-size: 16px;
  color: #333;
  font-weight: 600;
}

.close-detail-btn {
  background: none;
  border: none;
  font-size: 20px;
  color: #999;
  cursor: pointer;
  padding: 4px 8px;
  transition: all 0.3s;
  border-radius: 4px;
}

.close-detail-btn:hover {
  background: #f0f0f0;
  color: #333;
}

.detail-content {
  flex: 1;
  overflow-y: auto;
  padding: 12px;
}

.info-card {
  background: white;
  border-radius: 8px;
  padding: 16px;
  margin-bottom: 12px;
  box-shadow: 0 1px 3px rgba(0, 0, 0, 0.05);
}

.card-title {
  font-size: 14px;
  font-weight: 600;
  color: #333;
  margin-bottom: 12px;
  padding-bottom: 8px;
  border-bottom: 2px solid #f0f0f0;
}

.detail-item {
  display: flex;
  align-items: flex-start;
  margin-bottom: 12px;
  padding: 8px;
  border-radius: 6px;
  transition: background 0.2s;
}

.detail-item:hover {
  background: #f8f9fa;
}

.detail-item:last-child {
  margin-bottom: 0;
}

.item-icon {
  font-size: 20px;
  margin-right: 12px;
  flex-shrink: 0;
}

.item-content {
  flex: 1;
  min-width: 0;
}

.detail-item label {
  display: block;
  font-size: 12px;
  color: #999;
  margin-bottom: 4px;
  font-weight: 500;
}

.item-value {
  color: #333;
  font-size: 14px;
  font-weight: 500;
}

.notes-content {
  color: #666;
  font-size: 14px;
  line-height: 1.6;
  padding: 8px;
  background: #f8f9fa;
  border-radius: 6px;
}

.custom-attrs {
  background: #f5f5f5;
  padding: 12px;
  border-radius: 6px;
  font-size: 12px;
  overflow-x: auto;
  margin: 0;
  color: #666;
  line-height: 1.5;
}

.btn {
  padding: 8px 16px;
  border: none;
  border-radius: 4px;
  font-size: 14px;
  cursor: pointer;
  transition: all 0.3s;
}

.btn:disabled {
  opacity: 0.5;
  cursor: not-allowed;
}

.btn-primary {
  background: #1890ff;
  color: white;
}

.btn-primary:hover:not(:disabled) {
  background: #40a9ff;
}

.btn-success {
  background: #52c41a;
  color: white;
}

.btn-success:hover:not(:disabled) {
  background: #73d13d;
}

.btn-info {
  background: #13c2c2;
  color: white;
}

.btn-info:hover:not(:disabled) {
  background: #36cfc9;
}

.btn-secondary {
  background: #722ed1;
  color: white;
}

.btn-secondary:hover:not(:disabled) {
  background: #9254de;
}

.btn-default {
  background: white;
  border: 1px solid #d9d9d9;
  color: #333;
}

.btn-default:hover {
  border-color: #40a9ff;
  color: #40a9ff;
}

.btn-cancel {
  background: white;
  border: 1px solid #d9d9d9;
  color: #333;
}

.btn-cancel:hover {
  border-color: #40a9ff;
  color: #40a9ff;
}

/* 模态框样式 */
.modal-overlay {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background: rgba(0, 0, 0, 0.5);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 1000;
}

.modal-content {
  background: white;
  border-radius: 8px;
  width: 90%;
  max-width: 600px;
  max-height: 90vh;
  display: flex;
  flex-direction: column;
  box-shadow: 0 4px 20px rgba(0, 0, 0, 0.15);
}

.modal-content.small {
  max-width: 500px;
}

.modal-header {
  padding: 20px;
  border-bottom: 1px solid #e8e8e8;
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.modal-header h2 {
  margin: 0;
  font-size: 20px;
  color: #333;
}

.close-btn {
  background: none;
  border: none;
  font-size: 28px;
  color: #999;
  cursor: pointer;
  padding: 0;
  width: 30px;
  height: 30px;
  display: flex;
  align-items: center;
  justify-content: center;
}

.close-btn:hover {
  color: #333;
}

/* 配置面板覆盖层 */
.config-panel-overlay {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background: rgba(0, 0, 0, 0.5);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 2000;
}

.config-panel-container {
  width: 90%;
  max-width: 450px;
  max-height: 90vh;
  overflow: auto;
}

.modal-body {
  padding: 20px;
  overflow-y: auto;
  flex: 1;
}

.form-group {
  margin-bottom: 16px;
}

/* 移动端适配 */
@media (max-width: 768px) {
  .toolbar {
    padding: 10px;
    gap: 10px;
  }

  .toolbar-left {
    flex-wrap: wrap;
    gap: 8px;
  }

  .toolbar-right {
    gap: 6px;
  }

  /* 移动端只显示logo，隐藏标题文字 */
  .title-text {
    display: none;
  }

  .logo {
    font-size: 28px;
  }

  .app-title {
    font-size: 28px;
  }

  .family-selector {
    flex: 1;
    min-width: 120px;
    font-size: 13px;
    padding: 6px 10px;
  }

  /* 移动端按钮只显示图标 */
  .btn {
    padding: 8px 10px;
    min-width: auto;
  }

  .btn-text {
    display: none;
  }

  .btn-icon {
    margin: 0;
    font-size: 18px;
  }

  .main-content {
    flex-direction: column;
  }

  .sidebar {
    width: 100%;
    max-height: 200px;
    border-right: none;
    border-bottom: 1px solid #e8e8e8;
  }

  .member-list {
    display: flex;
    overflow-x: auto;
    overflow-y: hidden;
    flex-direction: row;
    padding: 8px;
  }

  .member-item {
    min-width: 200px;
    margin-right: 8px;
    margin-bottom: 0;
  }

  .graph-area {
    min-height: 400px;
  }

  .detail-panel {
    position: fixed;
    top: 0;
    right: 0;
    bottom: 0;
    width: 100%;
    max-width: 100%;
    z-index: 1500;
    transform: translateX(0);
    box-shadow: -2px 0 8px rgba(0, 0, 0, 0.15);
  }

  .config-panel-container {
    width: 95%;
    max-width: none;
  }

  .info-card {
    padding: 12px;
  }

  .card-title {
    font-size: 13px;
  }

  .detail-item {
    padding: 6px;
  }

  .item-icon {
    font-size: 18px;
    margin-right: 8px;
  }

  .item-value {
    font-size: 13px;
  }
}

/* 小屏幕手机适配 */
@media (max-width: 480px) {
  .header-actions .btn {
    font-size: 12px;
    padding: 6px 10px;
  }

  .sidebar {
    max-height: 150px;
  }

  .member-item {
    min-width: 160px;
  }

  .member-name {
    font-size: 13px;
  }

  .member-meta {
    font-size: 11px;
  }

  .detail-header h3 {
    font-size: 14px;
  }

  .info-card {
    padding: 10px;
    margin-bottom: 8px;
  }

  .card-title {
    font-size: 12px;
    margin-bottom: 8px;
  }

  .item-value {
    font-size: 12px;
  }

  .notes-content,
  .custom-attrs {
    font-size: 12px;
  }
}

.form-group label {
  display: block;
  margin-bottom: 8px;
  font-weight: 500;
  color: #333;
}

.form-group input,
.form-group textarea {
  width: 100%;
  padding: 8px 12px;
  border: 1px solid #d9d9d9;
  border-radius: 4px;
  font-size: 14px;
  transition: border-color 0.3s;
}

.form-group input:focus,
.form-group textarea:focus {
  outline: none;
  border-color: #40a9ff;
}

.modal-footer {
  padding: 16px 20px;
  border-top: 1px solid #e8e8e8;
  display: flex;
  justify-content: flex-end;
  gap: 12px;
}
</style>
